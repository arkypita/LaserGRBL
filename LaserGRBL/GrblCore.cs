
using System;

namespace LaserGRBL
{
	/// <summary>
	/// Description of CommandThread.
	/// </summary>
	public class GrblCore
	{
		[Serializable]
		public class ThreadingMode
		{
			public readonly int StatusQuery;
			public readonly int TxLong;
			public readonly int TxShort;
			public readonly int RxLong;
			public readonly int RxShort;
			private readonly string Name;

			public ThreadingMode(int query, int txlong, int txshort, int rxlong, int rxshort, string name)
			{ StatusQuery = query; TxLong = txlong; TxShort = txshort; RxLong = rxlong; RxShort = rxshort; Name = name; }

			public static ThreadingMode Slow
			{ get { return new ThreadingMode(2000, 15, 4, 2, 1, "Slow"); } }

			public static ThreadingMode Quiet
			{ get { return new ThreadingMode(1000, 10, 2, 1, 1, "Quiet"); } }

			public static ThreadingMode Fast
			{ get { return new ThreadingMode(500, 5, 1, 1, 0, "Fast"); } }

			public static ThreadingMode UltraFast
			{ get { return new ThreadingMode(200, 1, 0, 0, 0, "UltraFast"); } }

			public static ThreadingMode Insane
			{ get { return new ThreadingMode(100, 1, 0, 0, 0, "Insane"); } }

			public override string ToString()
			{ return Name; }

			public override bool Equals(object obj)
			{ return obj != null && obj is ThreadingMode && ((ThreadingMode)obj).Name == Name; }
		}

		public enum DetectedIssue
		{
			Unknown = 0,
			ManualReset = -1,
			ManualDisconnect = -2,
			StopResponding = 1,
			//StopMoving = 2, 
			UnexpectedReset = 3,
			UnexpectedDisconnect = 4
		}

		public enum MacStatus
		{ Unknown, Disconnected, Connecting, Idle, Run, Hold, Door, Home, Alarm, Check, Jog, Queue }

		public enum JogDirection
		{ N, S, W, E, NW, NE, SW, SE }

		public enum StreamingMode
		{ Buffered, Synchronous, RepeatOnError }

		[Serializable]
		public class GrblVersionInfo : IComparable, ICloneable
		{
			int mMajor;
			int mMinor;
			char mBuild;

			public GrblVersionInfo(int major, int minor, char build)
			{ mMajor = major; mMinor = minor; mBuild = build; }

			public GrblVersionInfo(int major, int minor)
			{ mMajor = major; mMinor = minor; mBuild = (char)0; }

			public static bool operator !=(GrblVersionInfo a, GrblVersionInfo b)
			{ return !(a == b); }

			public static bool operator ==(GrblVersionInfo a, GrblVersionInfo b)
			{
				if (Object.ReferenceEquals(a, null))
					return Object.ReferenceEquals(b, null);
				else
					return a.Equals(b);
			}

			public static bool operator <(GrblVersionInfo a, GrblVersionInfo b)
			{
				if ((Object)a == null)
					throw new ArgumentNullException("a");
				return (a.CompareTo(b) < 0);
			}

			public static bool operator <=(GrblVersionInfo a, GrblVersionInfo b)
			{
				if ((Object)a == null)
					throw new ArgumentNullException("a");
				return (a.CompareTo(b) <= 0);
			}

			public static bool operator >(GrblVersionInfo a, GrblVersionInfo b)
			{ return (b < a); }

			public static bool operator >=(GrblVersionInfo a, GrblVersionInfo b)
			{ return (b <= a); }

			public override string ToString()
			{
				if (mBuild == (char)0)
					return string.Format("{0}.{1}", mMajor, mMinor);
				else
					return string.Format("{0}.{1}{2}", mMajor, mMinor, mBuild);
			}

			public override bool Equals(object obj)
			{
				GrblVersionInfo v = obj as GrblVersionInfo;
				return v != null && this.mMajor == v.mMajor && this.mMinor == v.mMinor && this.mBuild == v.mBuild;
			}

			public override int GetHashCode()
			{
				unchecked
				{
					int hash = 17;
					// Maybe nullity checks, if these are objects not primitives!
					hash = hash * 23 + mMajor.GetHashCode();
					hash = hash * 23 + mMinor.GetHashCode();
					hash = hash * 23 + mBuild.GetHashCode();
					return hash;
				}
			}

			public int CompareTo(Object version)
			{
				if (version == null)
					return 1;

				GrblVersionInfo v = version as GrblVersionInfo;
				if (v == null)
					throw new ArgumentException("Argument must be GrblVersionInfo");

				if (this.mMajor != v.mMajor)
					if (this.mMajor > v.mMajor)
						return 1;
					else
						return -1;

				if (this.mMinor != v.mMinor)
					if (this.mMinor > v.mMinor)
						return 1;
					else
						return -1;

				if (this.mBuild != v.mBuild)
					if (this.mBuild > v.mBuild)
						return 1;
					else
						return -1;

				return 0;
			}

			public object Clone()
			{ return this.MemberwiseClone(); }
		}

		public delegate void dlgIssueDetector(DetectedIssue issue);
		public delegate void dlgOnMachineStatus();
		public delegate void dlgOnOverrideChange();
		public delegate void dlgOnLoopCountChange(decimal current);

		public event dlgIssueDetector IssueDetected;
		public event dlgOnMachineStatus MachineStatusChanged;
		public event GrblFile.OnFileLoadedDlg OnFileLoaded;
		public event dlgOnOverrideChange OnOverrideChange;
		public event dlgOnLoopCountChange OnLoopCountChange;

		private System.Windows.Forms.Control syncro;
		private ComWrapper.IComWrapper com;
		private GrblFile file;
		private System.Collections.Generic.Queue<GrblCommand> mQueue; //vera coda di quelli da mandare
		private GrblCommand mRetryQueue; //coda[1] di quelli in attesa di risposta
		private System.Collections.Generic.Queue<GrblCommand> mPending; //coda di quelli in attesa di risposta
		private System.Collections.Generic.List<IGrblRow> mSent; //lista di quelli mandati

		private System.Collections.Generic.Queue<GrblCommand> mQueuePtr; //puntatore a coda di quelli da mandare (normalmente punta a mQueue, salvo per import/export configurazione)
		private System.Collections.Generic.List<IGrblRow> mSentPtr; //puntatore a lista di quelli mandati (normalmente punta a mSent, salvo per import/export configurazione)

		//private StreamingMode mStreamingMode = StreamingMode.Buffered;

		private int mBuffer;
		private System.Drawing.PointF mMPos;
		private System.Drawing.PointF mWCO;
		private int mGrblBlocks = -1;
		private int mGrblBuffer = -1;

		private TimeProjection mTP = new TimeProjection();

		private MacStatus mMachineStatus;
		private const int BUFFER_SIZE = 127;

		private int mCurOvFeed;
		private int mCurOvRapids;
		private int mCurOvSpindle;

		private int mTarOvFeed;
		private int mTarOvRapids;
		private int mTarOvSpindle;

		private decimal mLoopCount = 1;

		private Tools.PeriodicEventTimer QueryTimer;
		private Tools.ThreadObject TX;
		private Tools.ThreadObject RX;

		private long connectStart;

		private Tools.ElapsedFromEvent debugLastStatusDelay;
		private Tools.ElapsedFromEvent debugLastMoveDelay;

		private ThreadingMode mThreadingMode = ThreadingMode.UltraFast;
		private HotKeysManager mHotKeyManager;

		public UsageStats.UsageCounters UsageCounters;

		public GrblCore(System.Windows.Forms.Control syncroObject)
		{
			SetStatus(MacStatus.Disconnected);

			syncro = syncroObject;
			com = new ComWrapper.UsbSerial();

			debugLastStatusDelay = new Tools.ElapsedFromEvent();
			debugLastMoveDelay = new Tools.ElapsedFromEvent();

			mThreadingMode = (ThreadingMode)Settings.GetObject("Threading Mode", ThreadingMode.UltraFast);
			QueryTimer = new Tools.PeriodicEventTimer(TimeSpan.FromMilliseconds(mThreadingMode.StatusQuery), false);
			TX = new Tools.ThreadObject(ThreadTX, 1, true, "Serial TX Thread", StartTX);
			RX = new Tools.ThreadObject(ThreadRX, 1, true, "Serial RX Thread", null);

			file = new GrblFile(0, 0, 200, 300);  //create a fake range to use with manual movements

			file.OnFileLoaded += RiseOnFileLoaded;

			mQueue = new System.Collections.Generic.Queue<GrblCommand>();
			mPending = new System.Collections.Generic.Queue<GrblCommand>();
			mSent = new System.Collections.Generic.List<IGrblRow>();
			mBuffer = 0;

			mSentPtr = mSent;
			mQueuePtr = mQueue;

			mCurOvFeed = mCurOvRapids = mCurOvSpindle = 100;
			mTarOvFeed = mTarOvRapids = mTarOvSpindle = 100;

			if (!Settings.ExistObject("Hotkey Setup")) Settings.SetObject("Hotkey Setup", new HotKeysManager());
			mHotKeyManager = (HotKeysManager)Settings.GetObject("Hotkey Setup", null);
			mHotKeyManager.Init(this);

			UsageCounters = new UsageStats.UsageCounters();
		}

		public GrblConf Configuration
		{
			get { return (GrblConf)Settings.GetObject("Grbl Configuration", new GrblConf()); }
			set
			{
				if (value.Count > 0 && value.GrblVersion != null)
				{
					Settings.SetObject("Grbl Configuration", value);
					Settings.Save();
				}
			}
		}

		private void SetStatus(MacStatus value)
		{
			if (mMachineStatus != value)
			{
				mMachineStatus = value;
				Logger.LogMessage("SetStatus", "Machine status [{0}]", mMachineStatus);

				RiseMachineStatusChanged();

				if (mTP != null && mTP.InProgram)
				{
					if (InPause)
						mTP.JobPause();
					else
						mTP.JobResume();
				}
			}
		}

		public GrblVersionInfo GrblVersion
		{
			get { return (GrblVersionInfo)Settings.GetObject("Last GrblVersion known", null); }
			set
			{
				if (GrblVersion != null)
					Logger.LogMessage("VersionInfo", "Detected Grbl v{0}", value);

				if (GrblVersion == null || !GrblVersion.Equals(value))
				{
					Settings.SetObject("Last GrblVersion known", value);
					Settings.Save();
				}
			}
		}

		private void SetIssue(DetectedIssue issue)
		{
			mTP.JobIssue(issue);
			Logger.LogMessage("Issue detector", "{0} [{1},{2},{3}]", issue, FreeBuffer, GrblBuffer, GrblBlock);

			if (issue > 0) //negative numbers indicate issue caused by the user, so must not be report to UI
				RiseIssueDetected(issue);
		}

		void RiseIssueDetected(DetectedIssue issue)
		{
			if (IssueDetected != null)
			{
				if (syncro.InvokeRequired)
					syncro.BeginInvoke(new dlgIssueDetector(RiseIssueDetected), issue);
				else
					IssueDetected(issue);
			}
		}


		void RiseMachineStatusChanged()
		{
			if (MachineStatusChanged != null)
			{
				if (syncro.InvokeRequired)
					syncro.BeginInvoke(new dlgOnMachineStatus(RiseMachineStatusChanged));
				else
					MachineStatusChanged();
			}
		}

		void RiseOverrideChanged()
		{
			if (OnOverrideChange != null)
			{
				if (syncro.InvokeRequired)
					syncro.BeginInvoke(new dlgOnOverrideChange(RiseOverrideChanged));
				else
					OnOverrideChange();
			}
		}

		void RiseOnFileLoaded(long elapsed, string filename)
		{
			mTP.Reset();

			if (OnFileLoaded != null)
				OnFileLoaded(elapsed, filename);
		}

		public GrblFile LoadedFile
		{ get { return file; } }

		public void ReOpenFile(System.Windows.Forms.Form parent)
		{
			if (CanReOpenFile)
				OpenFile(parent, (string)Settings.GetObject("Core.LastOpenFile", null));
		}

		public static readonly System.Collections.Generic.List<string> ImageExtensions = new System.Collections.Generic.List<string>(new string[] { ".jpg", ".bmp", ".png", ".gif" });
		public void OpenFile(System.Windows.Forms.Form parent, string filename = null)
		{
			if (!CanLoadNewFile) return;

			try
			{
				if (filename == null)
				{
					using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
					{
						//pre-select last file if exist
						string lastFN = (string)Settings.GetObject("Core.LastOpenFile", null);
						if (lastFN != null && System.IO.File.Exists(lastFN))
							ofd.FileName = lastFN;

						ofd.Filter = "Any supported file|*.nc;*.cnc;*.tap;*.gcode;*.bmp;*.png;*.jpg;*.gif|GCODE Files|*.nc;*.cnc;*.tap;*.gcode|Raster Image|*.bmp;*.png;*.jpg;*.gif";
						ofd.CheckFileExists = true;
						ofd.Multiselect = false;
						ofd.RestoreDirectory = true;
						if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
							filename = ofd.FileName;
					}
				}
				if (filename != null)
				{
					Logger.LogMessage("OpenFile", "Open {0}", filename);
					Settings.SetObject("Core.LastOpenFile", filename);

					if (ImageExtensions.Contains(System.IO.Path.GetExtension(filename).ToLowerInvariant())) //import raster image
					{
						try
						{
							RasterConverter.RasterToLaserForm.CreateAndShowDialog(this, filename, parent);
							UsageCounters.RasterFile++;
						}
						catch (Exception ex)
						{ Logger.LogException("RasterImport", ex); }
					}
					else //load GCODE file
					{
						System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

						try
						{
							file.LoadFile(filename);
							UsageCounters.GCodeFile++;
						}
						catch (Exception ex)
						{ Logger.LogException("GCodeImport", ex); }

						System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
					}
				}
			}
			catch (Exception ex)
			{
				Logger.LogException("OpenFile", ex);
			}
		}

		public void SaveProgram()
		{
			if (HasProgram)
			{
				string filename = null;
				using (System.Windows.Forms.SaveFileDialog ofd = new System.Windows.Forms.SaveFileDialog())
				{
					ofd.Filter = "GCODE Files|*.nc";
					ofd.AddExtension = true;
					ofd.RestoreDirectory = true;
					if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
						filename = ofd.FileName;
				}

				if (filename != null)
					file.SaveProgram(filename);
			}
		}

		public void RefreshConfig()
		{
			if (mMachineStatus == MacStatus.Idle)
			{
				try
				{
					GrblConf conf = new GrblConf(GrblVersion);
					GrblCommand cmd = new GrblCommand("$$");

					lock (this)
					{
						mSentPtr = new System.Collections.Generic.List<IGrblRow>(); //assign sent queue
						mQueuePtr = new System.Collections.Generic.Queue<GrblCommand>();
						mQueuePtr.Enqueue(cmd);
					}

					Tools.PeriodicEventTimer WaitResponseTimeout = new Tools.PeriodicEventTimer(TimeSpan.FromSeconds(10), true);

					//resta in attesa dell'invio del comando e della risposta
					while (cmd.Status == GrblCommand.CommandStatus.Queued || cmd.Status == GrblCommand.CommandStatus.WaitingResponse)
					{
						if (WaitResponseTimeout.Expired)
							throw new TimeoutException("No response received from grbl!");
						else
							System.Threading.Thread.Sleep(10);
					}

					if (cmd.Status == GrblCommand.CommandStatus.ResponseGood)
					{
						//attendi la ricezione di tutti i parametri
						long tStart = Tools.HiResTimer.TotalMilliseconds;
						long tLast = tStart;
						int counter = mSentPtr.Count;
						int target = conf.ExpectedCount + 1; //il +1 è il comando $$

						//finché ne devo ricevere ancora && l'ultima risposta è più recente di 500mS && non sono passati più di 5s totali
						while (mSentPtr.Count < target && Tools.HiResTimer.TotalMilliseconds - tLast < 500 && Tools.HiResTimer.TotalMilliseconds - tStart < 5000)
						{
							if (mSentPtr.Count != counter)
							{ tLast = Tools.HiResTimer.TotalMilliseconds; counter = mSentPtr.Count; }
							else
								System.Threading.Thread.Sleep(10);
						}

						foreach (IGrblRow row in mSentPtr)
						{
							if (row is GrblMessage)
							{
								string msg = row.GetMessage(); //"$0=10 (Step pulse time)"
								int num = int.Parse(msg.Split('=')[0].Substring(1));
								decimal val = decimal.Parse(msg.Split('=')[1].Split(' ')[0], System.Globalization.NumberFormatInfo.InvariantInfo);
								conf.Add(num, val);
							}
						}

						if (conf.Count >= conf.ExpectedCount)
							Configuration = conf;
						else
							throw new TimeoutException(string.Format("Wrong number of config param found! ({0}/{1})", conf.Count, conf.ExpectedCount));
					}
				}
				catch (Exception ex)
				{
					Logger.LogException("Refresh Config", ex);
					throw (ex);
				}
				finally
				{
					lock (this)
					{
						mQueuePtr = mQueue;
						mSentPtr = mSent; //restore queue
					}
				}
			}
		}

		public class WriteConfigException : Exception
		{
			private System.Collections.Generic.List<IGrblRow> ErrorLines = new System.Collections.Generic.List<IGrblRow>();

			public WriteConfigException(System.Collections.Generic.List<IGrblRow> mSentPtr)
			{
				foreach (IGrblRow row in mSentPtr)
					if (row is GrblCommand)
						if (((GrblCommand)row).Status != GrblCommand.CommandStatus.ResponseGood)
							ErrorLines.Add(row);
			}

			public override string Message
			{
				get
				{
					string rv = "";
					foreach (IGrblRow r in ErrorLines)
						rv += string.Format("{0} {1}\n", r.GetMessage(), r.GetResult(true, false));
					return rv.Trim();
				}
			}

			public System.Collections.Generic.List<IGrblRow> Errors
			{ get { return ErrorLines; } }
		}

		public void WriteConfig(System.Collections.Generic.List<GrblConf.GrblConfParam> config)
		{
			if (mMachineStatus == MacStatus.Idle)
			{
				lock (this)
				{
					mSentPtr = new System.Collections.Generic.List<IGrblRow>(); //assign sent queue
					mQueuePtr = new System.Collections.Generic.Queue<GrblCommand>();

					foreach (GrblConf.GrblConfParam p in config)
						mQueuePtr.Enqueue(new GrblCommand(string.Format("${0}={1}", p.Number, p.Value.ToString(System.Globalization.NumberFormatInfo.InvariantInfo))));
				}

				try
				{
					while (com.IsOpen && (mQueuePtr.Count > 0 || mPending.Count > 0)) //resta in attesa del completamento della comunicazione
						;

					int errors = 0;
					foreach (IGrblRow row in mSentPtr)
					{
						if (row is GrblCommand)
							if (((GrblCommand)row).Status != GrblCommand.CommandStatus.ResponseGood)
								errors++;
					}

					if (errors > 0)
						throw new WriteConfigException(mSentPtr);
				}
				catch (Exception ex)
				{
					Logger.LogException("Write Config", ex);
					throw (ex);
				}
				finally
				{
					lock (this)
					{
						mQueuePtr = mQueue;
						mSentPtr = mSent; //restore queue
					}
				}
			}
		}


		public void RunProgram()
		{
			if (CanSendFile)
			{
				if (mTP.Executed == 0 || mTP.Executed == mTP.Target) //mai iniziato oppure correttamente finito
					RunProgramFromStart(false);
				else
					UserWantToContinue();
			}
		}

		private void UserWantToContinue()
		{
			bool setwco = mWCO == System.Drawing.PointF.Empty && mTP.LastKnownWCO != System.Drawing.PointF.Empty;
			bool homing = MachinePosition == System.Drawing.PointF.Empty; //potrebbe essere dovuto ad un hard reset -> posizione non affidabile
			int position = LaserGRBL.ResumeJobForm.CreateAndShowDialog(mTP.Executed, mTP.Sent, mTP.Target, mTP.LastIssue, Configuration.HomingEnabled, homing, out homing, setwco, setwco, out setwco, mTP.LastKnownWCO);

			if (position == 0)
				RunProgramFromStart(homing);
			if (position > 0)
				ContinueProgramFromKnown(position, homing, setwco);
		}

		private void RunProgramFromStart(bool homing)
		{
			lock (this)
			{
				ClearQueue(true);

				mTP.Reset();
				mTP.JobStart(file.EstimatedTime, file.Count);
				Logger.LogMessage("EnqueueProgram", "Running program, {0} lines", file.Count);

				if (homing)
					mQueuePtr.Enqueue(new GrblCommand("$H"));

				foreach (GrblCommand cmd in file)
					mQueuePtr.Enqueue(cmd.Clone() as GrblCommand);
			}
		}

		private void ContinueProgramFromKnown(int position, bool homing, bool setwco)
		{
			lock (this)
			{

				ClearQueue(false); //lascia l'eventuale lista delle cose già mandate, se ce l'hai ancora

				mSentPtr.Add(new GrblMessage(string.Format("[resume from #{0}]", position + 1), false));
				Logger.LogMessage("ResumeProgram", "Resume program from #{0}", position + 1);

				GrblCommand.StatePositionBuilder spb = new GrblCommand.StatePositionBuilder();

				if (homing) mQueuePtr.Enqueue(new GrblCommand("$H"));

				if (setwco)
				{
					//compute current point and set offset
					System.Drawing.PointF pos = homing ? new System.Drawing.PointF(0, 0) : MachinePosition;
					System.Drawing.PointF wco = mTP.LastKnownWCO;
					System.Drawing.PointF cur = new System.Drawing.PointF(pos.X - wco.X, pos.Y - wco.Y);
					mQueue.Enqueue(new GrblCommand(String.Format("G92 X{0} Y{1}", cur.X.ToString(System.Globalization.CultureInfo.InvariantCulture), cur.Y.ToString(System.Globalization.CultureInfo.InvariantCulture))));
				}

				for (int i = 0; i < position && i < file.Count; i++) //analizza fino alla posizione
					spb.AnalyzeCommand(file[i], false);

				mQueuePtr.Enqueue(new GrblCommand("G90")); //absolute coordinate
				mQueuePtr.Enqueue(new GrblCommand(string.Format("M5 G0 {0} {1} {2} {3}", spb.X, spb.Y, spb.F, spb.S))); //fast go to the computed position with laser off and set speed and power
				mQueuePtr.Enqueue(new GrblCommand(spb.GetSettledModals()));

				mTP.JobContinue(position, mQueuePtr.Count);

				for (int i = position; i < file.Count; i++) //enqueue remaining commands
				{
					if (spb != null) //check the very first movement command and ensure modal MotionMode is settled
					{
						GrblCommand cmd = file[i].Clone() as GrblCommand;
						cmd.BuildHelper();
						if (cmd.IsMovement && cmd.G == null)
							mQueuePtr.Enqueue(new GrblCommand(spb.MotionMode, cmd));
						else
							mQueuePtr.Enqueue(cmd);
						cmd.DeleteHelper();
						spb = null; //only the first time
					}
					else
					{
						mQueuePtr.Enqueue(file[i].Clone() as GrblCommand);
					}
				}
			}
		}

		public bool HasProgram
		{ get { return file != null && file.Count > 0; } }

		public void EnqueueCommand(GrblCommand cmd)
		{
			lock (this)
			{ mQueuePtr.Enqueue(cmd.Clone() as GrblCommand); }
		}

		public void Configure(ComWrapper.WrapperType wraptype, params object[] conf)
		{
			if (wraptype == ComWrapper.WrapperType.UsbSerial && (com == null || com.GetType() != typeof(ComWrapper.UsbSerial)))
				com = new ComWrapper.UsbSerial();
			else if (wraptype == ComWrapper.WrapperType.Telnet && (com == null || com.GetType() != typeof(ComWrapper.Telnet)))
				com = new ComWrapper.Telnet();
			else if (wraptype == ComWrapper.WrapperType.LaserWebESP8266 && (com == null || com.GetType() != typeof(ComWrapper.LaserWebESP8266)))
				com = new ComWrapper.LaserWebESP8266();
			else if (wraptype == ComWrapper.WrapperType.Emulator && (com == null || com.GetType() != typeof(ComWrapper.Emulator)))
				com = new ComWrapper.Emulator();

			com.Configure(conf);
		}

		public void OpenCom()
		{
			try
			{
				SetStatus(MacStatus.Connecting);
				connectStart = Tools.HiResTimer.TotalMilliseconds;

				if (!com.IsOpen)
				{
					com.Open();
				}

				lock (this)
				{
					RX.Start();
					TX.Start();
				}
			}
			catch (Exception ex)
			{
				Logger.LogMessage("OpenCom", "Error: {0}", ex.Message);
				SetStatus(MacStatus.Disconnected);
				com.Close(true);

				System.Windows.Forms.MessageBox.Show(ex.Message, Strings.BoxConnectErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			}
		}

		public void CloseCom(bool user)
		{
			if (mTP.LastIssue == DetectedIssue.Unknown && MachineStatus == MacStatus.Run && InProgram)
				SetIssue(user ? DetectedIssue.ManualDisconnect : DetectedIssue.UnexpectedDisconnect);

			try
			{
				if (com.IsOpen)
					com.Close(!user);

				mBuffer = 0;
				mTP.JobEnd();

				TX.Stop();
				RX.Stop();

				lock (this)
				{ ClearQueue(false); } //non resettare l'elenco delle cose mandate così da non sbiancare la lista

				SetStatus(MacStatus.Disconnected);
			}
			catch (Exception ex)
			{
				Logger.LogException("CloseCom", ex);
			}
		}

		public bool IsOpen
		{ get { return mMachineStatus != MacStatus.Disconnected; } }

		#region Comandi immediati

		public void CycleStartResume()
		{ if (CanResumeHold) SendImmediate(126); }

		public void FeedHold()
		{ if (CanFeedHold) SendImmediate(33); }

		public void SafetyDoor()
		{ SendImmediate(64); }

		private void QueryPosition()
		{ SendImmediate(63, true); }

		public void GrblReset() //da comando manuale esterno (pulsante)
		{
			if (CanResetGrbl)
			{
				if (mTP.LastIssue == DetectedIssue.Unknown && MachineStatus == MacStatus.Run && InProgram)
					SetIssue(DetectedIssue.ManualReset);
				InternalReset(true);
			}
		}

		private void InternalReset(bool grbl)
		{
			lock (this)
			{
				ClearQueue(true);
				mBuffer = 0;
				mTP.JobEnd();
				mCurOvFeed = mCurOvRapids = mCurOvSpindle = 100;
				mTarOvFeed = mTarOvRapids = mTarOvSpindle = 100;

				if (grbl)
					SendImmediate(24);
			}

			RiseOverrideChanged();
		}

		public void SendImmediate(byte b, bool mute = false)
		{
			try
			{
				if (!mute) Logger.LogMessage("SendImmediate", "Send Immediate Command [0x{0:X}]", b);

				lock (this)
				{ if (com.IsOpen) com.Write(b); }
			}
			catch (Exception ex)
			{ Logger.LogException("SendImmediate", ex); }
		}

		#endregion

		#region Public Property

		public int ProgramTarget
		{ get { return mTP.Target; } }

		public int ProgramSent
		{ get { return mTP.Sent; } }

		public int ProgramExecuted
		{ get { return mTP.Executed; } }

		public TimeSpan ProgramTime
		{ get { return mTP.TotalJobTime; } }

		public TimeSpan ProjectedTime
		{ get { return mTP.ProjectedTarget; } }

		public MacStatus MachineStatus
		{ get { return mMachineStatus; } }

		public bool InProgram
		{ get { return mTP.InProgram; } }

		public System.Drawing.PointF MachinePosition
		{ get { return mMPos; } }

		public System.Drawing.PointF WorkPosition //WCO = MPos - WPos
		{ get { return new System.Drawing.PointF(mMPos.X - mWCO.X, mMPos.Y - mWCO.Y); } }

		public System.Drawing.PointF WorkingOffset
		{ get { return mWCO; } }

		public int Executed
		{ get { return mSent.Count; } }

		public System.Collections.Generic.List<IGrblRow> SentCommand(int index, int count)
		{
			System.Collections.Generic.List<IGrblRow> rv;
			rv = mSent.GetRange(index, count);
			return rv;
		}

		#endregion

		#region Grbl Version Support

		public bool SupportRTO
		{ get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

		public bool SupportJogging
		{ get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

		public bool SupportCSV
		{ get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

		public bool SupportOverride
		{ get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

		#endregion

		public bool JogEnabled
		{
			get
			{
				if (SupportJogging)
					return IsOpen && (MachineStatus == GrblCore.MacStatus.Idle || MachineStatus == GrblCore.MacStatus.Jog);
				else
					return IsOpen && (MachineStatus == GrblCore.MacStatus.Idle || MachineStatus == GrblCore.MacStatus.Run) && !InProgram;
			}
		}

		public void Jog(JogDirection dir)
		{
			if (JogEnabled)
			{
				decimal size = JogStep;
				decimal speed = JogSpeed;

				string cmd = SupportJogging ? "$J=G91X{1}Y{0}F{2}" : "G0X{1}Y{0}F{2}";

				if (dir == JogDirection.N)
					cmd = string.Format(cmd, size, 0, speed);
				else if (dir == JogDirection.S)
					cmd = string.Format(cmd, -size, 0, speed);
				else if (dir == JogDirection.W)
					cmd = string.Format(cmd, 0, -size, speed);
				else if (dir == JogDirection.E)
					cmd = string.Format(cmd, 0, size, speed);
				else if (dir == JogDirection.NE)
					cmd = string.Format(cmd, size, size, speed);
				else if (dir == JogDirection.NW)
					cmd = string.Format(cmd, size, -size, speed);
				else if (dir == JogDirection.SE)
					cmd = string.Format(cmd, -size, size, speed);
				else if (dir == JogDirection.SW)
					cmd = string.Format(cmd, -size, -size, speed);

				if (!SupportJogging)
					EnqueueCommand(new GrblCommand("G91"));

				EnqueueCommand(new GrblCommand(cmd));

				if (!SupportJogging)
					EnqueueCommand(new GrblCommand("G90"));
			}
		}

		internal void JogHome()
		{
			if (JogEnabled)
			{
				if (SupportJogging)
				{
					EnqueueCommand(new GrblCommand(string.Format("$J=G90X0Y0F{0}", JogSpeed)));
				}
				else
				{
					EnqueueCommand(new GrblCommand(string.Format("G90")));
					EnqueueCommand(new GrblCommand(string.Format("G0X0Y0F{0}", JogSpeed)));
				}
			}
		}

		private void StartTX()
		{
			lock (this)
			{
				InternalReset((bool)Settings.GetObject("Reset Grbl On Connect", true));
				QueryPosition();
				QueryTimer.Start();
			}
		}

		protected void ThreadTX()
		{
			lock (this)
			{
				try
				{
					if (MachineStatus == MacStatus.Connecting && Tools.HiResTimer.TotalMilliseconds - connectStart > 10000)
						OnConnectTimeout();

					if (!TX.MustExitTH() && CanSend())
						SendLine();

					if (QueryTimer.Expired)
						QueryPosition();

					DetectHang();

					TX.SleepTime = CanSend() ? CurrentThreadingMode.TxShort : CurrentThreadingMode.TxLong;
					QueryTimer.Period = TimeSpan.FromMilliseconds(CurrentThreadingMode.StatusQuery);
				}
				catch (Exception ex)
				{ Logger.LogException("ThreadTX", ex); }
			}
		}

		private void DetectHang()
		{
			if (mTP.LastIssue == DetectedIssue.Unknown && MachineStatus == MacStatus.Run && InProgram)
			{
				//bool executingM4 = false;
				//if (mPending.Count > 0)
				//{
				//	GrblCommand cur = mPending.Peek();
				//	cur.BuildHelper();
				//	executingM4 = cur.IsPause;
				//	cur.DeleteHelper();
				//}

				bool noQueryResponse = debugLastStatusDelay.ElapsedTime > TimeSpan.FromTicks(QueryTimer.Period.Ticks * 10) && debugLastStatusDelay.ElapsedTime > TimeSpan.FromSeconds(5);
				//bool noMovement = !executingM4 && debugLastMoveDelay.ElapsedTime > TimeSpan.FromSeconds(10);

				if (noQueryResponse)
					SetIssue(DetectedIssue.StopResponding);
				//else if (noMovement)
				//	SetIssue(DetectedIssue.StopMoving);
			}
		}


		private void OnConnectTimeout()
		{
			if (com.IsOpen)
			{
				Logger.LogMessage("OpenCom", "Connection timeout!");
				com.Close(true); //this cause disconnection from RX thread ("ReadLineOrDisconnect")
			}
		}

		private bool CanSend()
		{
			GrblCommand next = PeekNextCommand();
			return next != null && HasSpaceInBuffer(next);
		}

		private GrblCommand PeekNextCommand()
		{
			if (mPending.Count > 0 && mPending.Peek().IsWriteEEPROM) //if managing eeprom write act like sync
				return null;
			else if (CurrentStreamingMode == StreamingMode.Buffered && mQueuePtr.Count > 0) //sono buffered e ho roba da trasmettere
				return mQueuePtr.Peek();
			else if (CurrentStreamingMode != StreamingMode.Buffered && mPending.Count == 0) //sono sync e sono vuoto
				if (mRetryQueue != null) return mRetryQueue;
				else if (mQueuePtr.Count > 0) return mQueuePtr.Peek();
				else return null;
			else
				return null;
		}

		private void RemoveManagedCommand()
		{
			if (mRetryQueue != null)
				mRetryQueue = null;
			else
				mQueuePtr.Dequeue();
		}

		private bool HasSpaceInBuffer(GrblCommand command)
		{ return (mBuffer + command.SerialData.Length) <= BUFFER_SIZE; }

		private void SendLine()
		{
			GrblCommand tosend = PeekNextCommand();
			if (tosend != null)
			{
				try
				{

					tosend.BuildHelper();

					tosend.SetSending();
					mSentPtr.Add(tosend);
					mPending.Enqueue(tosend);
					RemoveManagedCommand();

					mBuffer += tosend.SerialData.Length;
					com.Write(tosend.SerialData);

					if (mTP.InProgram)
						mTP.JobSent();

					debugLastMoveDelay.Start();
				}
				catch (Exception ex)
				{
					if (tosend != null) Logger.LogMessage("SendLine", "Error sending [{0}] command: {1}", tosend.Command, ex.Message);
					//Logger.LogException("SendLine", ex);
				}
				finally { tosend.DeleteHelper(); }
			}
		}

		public int UsedBuffer
		{ get { return mBuffer; } }

		public int FreeBuffer
		{ get { return BUFFER_SIZE - mBuffer; } }

		public int BufferSize
		{ get { return BUFFER_SIZE; } }

		public int GrblBlock
		{ get { return mGrblBlocks; } }

		public int GrblBuffer
		{ get { return mGrblBuffer; } }

		void ThreadRX()
		{
			try
			{
				string rline = null;
				if ((rline = WaitComLineOrDisconnect()) != null)
				{
					if (rline.Length > 0)
					{
						lock (this)
						{
							if (rline.ToLower().StartsWith("ok") || rline.ToLower().StartsWith("error"))
								ManageCommandResponse(rline);
							else if (rline.StartsWith("<") && rline.EndsWith(">"))
								ManageRealTimeStatus(rline);
							else if (rline.StartsWith("Grbl "))
								ManageVersionInfo(rline);
							else
								ManageGenericMessage(rline);
						}
					}
				}

				RX.SleepTime = HasIncomingData() ? CurrentThreadingMode.RxShort : CurrentThreadingMode.RxLong;
			}
			catch (Exception ex)
			{ Logger.LogException("ThreadRX", ex); }
		}

		private void ManageGenericMessage(string rline)
		{
			try { mSentPtr.Add(new GrblMessage(rline, SupportCSV)); }
			catch (Exception ex)
			{
				Logger.LogMessage("GenericMessage", "Ex on [{0}] message", rline);
				Logger.LogException("GenericMessage", ex);
			}
		}

		private void ManageVersionInfo(string rline)
		{
			//Grbl vX.Xx ['$' for help]
			try
			{
				int maj = int.Parse(rline.Substring(5, 1));
				int min = int.Parse(rline.Substring(7, 1));
				char build = rline.Substring(8, 1).ToCharArray()[0];
				GrblVersion = new GrblVersionInfo(maj, min, build);

				DetectUnexpectedReset();
				OnStartupMessage();
			}
			catch (Exception ex)
			{
				Logger.LogMessage("VersionInfo", "Ex on [{0}] message", rline);
				Logger.LogException("VersionInfo", ex);
			}
			mSentPtr.Add(new GrblMessage(rline, false));
		}

		private void OnStartupMessage() //resetta tutto, così funziona anche nel caso di hard-unexpected reset
		{
			lock (this)
			{
				ClearQueue(false);
				mBuffer = 0;
				mTP.JobEnd();
				mCurOvFeed = mCurOvRapids = mCurOvSpindle = 100;
				mTarOvFeed = mTarOvRapids = mTarOvSpindle = 100;
			}
			RiseOverrideChanged();
		}

		private void DetectUnexpectedReset()
		{
			if (mTP.LastIssue == DetectedIssue.Unknown && MachineStatus == MacStatus.Run && InProgram)
				SetIssue(DetectedIssue.UnexpectedReset);
		}

		private void ManageRealTimeStatus(string rline)
		{
			try
			{
				debugLastStatusDelay.Start();

				rline = rline.Substring(1, rline.Length - 2);
				//System.Diagnostics.Debug.WriteLine(rline);
				if (rline.Contains("|")) //grbl > 1.1 - https://github.com/gnea/grbl/wiki/Grbl-v1.1-Interface#real-time-status-reports
				{
					string[] arr = rline.Split("|".ToCharArray());

					ParseMachineStatus(arr[0]);

					for (int i = 1; i < arr.Length; i++)
					{
						if (arr[i].StartsWith("Ov"))
							ParseOverrides(arr[i]);
						if (arr[i].StartsWith("Bf"))
							ParseBf(arr[i]);
						if (arr[i].StartsWith("WPos"))
							ParseWPos(arr[i]);
						if (arr[i].StartsWith("MPos"))
							ParseMPos(arr[i]);
						if (arr[i].StartsWith("WCO"))
							ParseWCO(arr[i]);
					}
				}
				else //<Idle,MPos:0.000,0.000,0.000,WPos:0.000,0.000,0.000>
				{
					string[] arr = rline.Split(",".ToCharArray());

					if (arr.Length > 0)
						ParseMachineStatus(arr[0]);
					if (arr.Length > 2)
						SetMPosition(new System.Drawing.PointF(float.Parse(arr[1].Substring(5, arr[1].Length - 5), System.Globalization.NumberFormatInfo.InvariantInfo), float.Parse(arr[2], System.Globalization.NumberFormatInfo.InvariantInfo)));
					if (arr.Length > 6)
						ComputeWCO(new System.Drawing.PointF(float.Parse(arr[4].Substring(5, arr[4].Length - 5), System.Globalization.NumberFormatInfo.InvariantInfo), float.Parse(arr[5], System.Globalization.NumberFormatInfo.InvariantInfo)));
				}
			}
			catch (Exception ex)
			{
				Logger.LogMessage("RealTimeStatus", "Ex on [{0}] message", rline);
				Logger.LogException("RealTimeStatus", ex);
			}
		}

		private void ComputeWCO(System.Drawing.PointF wpos) //WCO = MPos - WPos
		{
			SetWCO(new System.Drawing.PointF(mMPos.X - wpos.X, mMPos.Y - wpos.Y));
		}

		private void ParseWCO(string p)
		{
			string wco = p.Substring(4, p.Length - 4);
			string[] xyz = wco.Split(",".ToCharArray());
			SetWCO(new System.Drawing.PointF(float.Parse(xyz[0], System.Globalization.NumberFormatInfo.InvariantInfo), float.Parse(xyz[1], System.Globalization.NumberFormatInfo.InvariantInfo)));
		}

		private void ParseWPos(string p)
		{
			string wpos = p.Substring(5, p.Length - 5);
			string[] xyz = wpos.Split(",".ToCharArray());
			SetMPosition(new System.Drawing.PointF(float.Parse(xyz[0], System.Globalization.NumberFormatInfo.InvariantInfo) + mWCO.X, float.Parse(xyz[1], System.Globalization.NumberFormatInfo.InvariantInfo) + mWCO.Y));
		}

		private void ParseMPos(string p)
		{
			string mpos = p.Substring(5, p.Length - 5);
			string[] xyz = mpos.Split(",".ToCharArray());
			SetMPosition(new System.Drawing.PointF(float.Parse(xyz[0], System.Globalization.NumberFormatInfo.InvariantInfo), float.Parse(xyz[1], System.Globalization.NumberFormatInfo.InvariantInfo)));
		}

		private void ParseBf(string p)
		{
			string bf = p.Substring(3, p.Length - 3);
			string[] ab = bf.Split(",".ToCharArray());

			mGrblBlocks = int.Parse(ab[0]);
			mGrblBuffer = int.Parse(ab[1]);
		}

		private void SetMPosition(System.Drawing.PointF pos)
		{
			if (pos != mMPos)
			{
				mMPos = pos;
				debugLastMoveDelay.Start();
			}
		}

		private void SetWCO(System.Drawing.PointF wco)
		{
			mWCO = wco;
			mTP.LastKnownWCO = wco; //remember last wco for job resume
		}

		private void ManageCommandResponse(string rline)
		{
			try
			{
				debugLastMoveDelay.Start(); //add a reset to prevent HangDetector trigger on G4

				if (mPending.Count > 0)
				{
					GrblCommand pending = mPending.Peek();	//necessario fare peek
					pending.SetResult(rline, SupportCSV);	//assegnare lo stato
					mPending.Dequeue();						//solo alla fine rimuoverlo dalla lista (per write config che si aspetta che lo stato sia noto non appena la coda si svuota)

					mBuffer -= pending.SerialData.Length;

					if (mTP.InProgram && pending.RepeatCount == 0) //solo se non è una ripetizione aggiorna il tempo
						mTP.JobExecuted(pending.TimeOffset);

					if (mTP.InProgram && pending.Status == GrblCommand.CommandStatus.ResponseBad)
						mTP.JobError(); //incrementa il contatore

					//ripeti errori programma && non ho una coda (magari mi sto allineando per cambio conf buff/sync) && ho un errore && non l'ho già ripetuto troppe volte
					if (InProgram && CurrentStreamingMode == StreamingMode.RepeatOnError && mPending.Count == 0 && pending.Status == GrblCommand.CommandStatus.ResponseBad && pending.RepeatCount < 3) //il comando eseguito ha dato errore
						mRetryQueue = new GrblCommand(pending.Command, pending.RepeatCount + 1); //repeat on error
				}

				if (InProgram && mQueuePtr.Count == 0 && mPending.Count == 0)
					OnProgramEnd();
			}
			catch (Exception ex)
			{
				Logger.LogMessage("CommandResponse", "Ex on [{0}] message", rline);
				Logger.LogException("CommandResponse", ex);
			}
		}

		private static char[] trimarray = new char[] { '\r', '\n', ' ' };
		private string WaitComLineOrDisconnect()
		{
			try
			{
				string rv = com.ReadLineBlocking();
				rv = rv.TrimEnd(trimarray); //rimuovi ritorno a capo
				rv = rv.Trim(); //rimuovi spazi iniziali e finali
				return rv.Length > 0 ? rv : null;
			}
			catch
			{
				try { CloseCom(false); }
				catch { }
				return null;
			}
		}

		private bool HasIncomingData()
		{
			try
			{
				return com.HasData();
			}
			catch
			{
				try { CloseCom(false); }
				catch { }
				return false;
			}
		}

		public void ManageOverrides()
		{
			if (mTarOvFeed == 100 && mCurOvFeed != 100) //devo fare un reset
				SendImmediate(144);
			else if (mTarOvFeed - mCurOvFeed >= 10) //devo fare un bigstep +
				SendImmediate(145);
			else if (mCurOvFeed - mTarOvFeed >= 10) //devo fare un bigstep -
				SendImmediate(146);
			else if (mTarOvFeed - mCurOvFeed >= 1) //devo fare uno smallstep +
				SendImmediate(147);
			else if (mCurOvFeed - mTarOvFeed >= 1) //devo fare uno smallstep -
				SendImmediate(148);

			if (mTarOvSpindle == 100 && mCurOvSpindle != 100) //devo fare un reset
				SendImmediate(153);
			else if (mTarOvSpindle - mCurOvSpindle >= 10) //devo fare un bigstep +
				SendImmediate(154);
			else if (mCurOvSpindle - mTarOvSpindle >= 10) //devo fare un bigstep -
				SendImmediate(155);
			else if (mTarOvSpindle - mCurOvSpindle >= 1) //devo fare uno smallstep +
				SendImmediate(156);
			else if (mCurOvSpindle - mTarOvSpindle >= 1) //devo fare uno smallstep -
				SendImmediate(157);

			if (mTarOvRapids == 100 && mCurOvRapids != 100)
				SendImmediate(149);
			else if (mTarOvRapids == 50 && mCurOvRapids != 50)
				SendImmediate(150);
			else if (mTarOvRapids == 25 && mCurOvRapids != 25)
				SendImmediate(151);
		}

		private void ParseOverrides(string data)
		{
			//Ov:100,100,100
			//indicates current override values in percent of programmed values
			//for feed, rapids, and spindle speed, respectively.

			data = data.Substring(data.IndexOf(':') + 1);
			string[] arr = data.Split(",".ToCharArray());

			ChangeOverrides(int.Parse(arr[0]), int.Parse(arr[1]), int.Parse(arr[2]));
			ManageOverrides();
		}

		private void ChangeOverrides(int feed, int rapids, int spindle)
		{
			bool notify = (feed != mCurOvFeed || rapids != mCurOvRapids || spindle != mCurOvSpindle);
			mCurOvFeed = feed;
			mCurOvRapids = rapids;
			mCurOvSpindle = spindle;

			if (notify)
				RiseOverrideChanged();
		}

		public int OverrideG1
		{ get { return mCurOvFeed; } }

		public int OverrideG0
		{ get { return mCurOvRapids; } }

		public int OverrideS
		{ get { return mCurOvSpindle; } }

		public int TOverrideG1
		{
			get { return mTarOvFeed; }
			set { mTarOvFeed = value; }
		}

		public int TOverrideG0
		{
			get { return mTarOvRapids; }
			set { mTarOvRapids = value; }
		}

		public int TOverrideS
		{
			get { return mTarOvSpindle; }
			set { mTarOvSpindle = value; }
		}

		private void ParseMachineStatus(string data)
		{
			MacStatus var = MacStatus.Disconnected;

			if (data.Contains(":"))
				data = data.Substring(0, data.IndexOf(':'));

			try { var = (MacStatus)Enum.Parse(typeof(MacStatus), data); }
			catch (Exception ex) { Logger.LogException("ParseMachineStatus", ex); }

			if (InProgram && var == MacStatus.Idle) //bugfix for grbl sending Idle on G4
				var = MacStatus.Run;

			SetStatus(var);
		}

		private void OnProgramEnd()
		{
			Logger.LogMessage("ProgramEnd", "Job Executed: {0} lines, {1} errors, {2}", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(mTP.TotalJobTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second, ",", true));
			mSentPtr.Add(new GrblMessage(string.Format("[{0} lines, {1} errors, {2}]", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(mTP.TotalJobTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second, ",", true)), false));

			if (mTP.JobEnd() && mLoopCount > 1 && mMachineStatus != MacStatus.Check)
			{
				LoopCount--;
				RunProgramFromStart(false);
			}
		}

		private bool InPause
		{ get { return mMachineStatus != MacStatus.Run && mMachineStatus != MacStatus.Idle; } }

		private void ClearQueue(bool sent)
		{
			mQueue.Clear();
			mPending.Clear();
			if (sent) mSent.Clear();
			mRetryQueue = null;
		}

		public bool CanReOpenFile
		{
			get
			{
				string lastFile = (string)Settings.GetObject("Core.LastOpenFile", null);
				return CanLoadNewFile && lastFile != null && System.IO.File.Exists(lastFile);
			}
		}

		public bool CanLoadNewFile
		{ get { return !InProgram; } }

		public bool CanSendFile
		{ get { return IsOpen && IdleOrCheck && HasProgram; } }

		public bool CanImportExport
		{ get { return IsOpen && MachineStatus == MacStatus.Idle; } }

		public bool CanResetGrbl
		{ get { return IsOpen && MachineStatus != MacStatus.Disconnected; } }

		public bool CanSendManualCommand
		{ get { return IsOpen && MachineStatus != MacStatus.Disconnected && !InProgram; } }

		public bool CanDoHoming
		{ get { return IsOpen && (MachineStatus == MacStatus.Idle || MachineStatus == GrblCore.MacStatus.Alarm) && Configuration.HomingEnabled; } }

		public bool CanDoZeroing
		{ get { return IsOpen && MachineStatus == MacStatus.Idle && WorkPosition != System.Drawing.PointF.Empty; } }

		public bool CanUnlock
		{ get { return IsOpen && (MachineStatus == MacStatus.Idle || MachineStatus == GrblCore.MacStatus.Alarm); } }

		public bool CanFeedHold
		{ get { return IsOpen && MachineStatus == MacStatus.Run; } }

		public bool CanResumeHold
		{ get { return IsOpen && (MachineStatus == MacStatus.Door || MachineStatus == MacStatus.Hold); } }

		public decimal LoopCount
		{ get { return mLoopCount; } set { mLoopCount = value; if (OnLoopCountChange != null) OnLoopCountChange(mLoopCount); } }

		private ThreadingMode CurrentThreadingMode
		{ get { return (ThreadingMode)Settings.GetObject("Threading Mode", ThreadingMode.UltraFast); } }

		private StreamingMode CurrentStreamingMode
		{ get { return (StreamingMode)Settings.GetObject("Streaming Mode", StreamingMode.Buffered); } }

		private bool IdleOrCheck
		{ get { return MachineStatus == MacStatus.Idle || MachineStatus == MacStatus.Check; } }

		private static string mDataPath;
		public static string DataPath
		{
			get
			{
				if (mDataPath == null)
				{
					mDataPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LaserGRBL");
					if (!System.IO.Directory.Exists(mDataPath))
						System.IO.Directory.CreateDirectory(mDataPath);
				}

				return mDataPath;
			}
		}

		private static string mTempPath;
		public static string TempPath
		{
			get
			{
				if (mTempPath == null)
				{
					mTempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "LaserGRBL");
					if (!System.IO.Directory.Exists(mTempPath))
						System.IO.Directory.CreateDirectory(mTempPath);
				}

				return mTempPath;
			}
		}


		public static string TranslateEnum(Enum value)
		{
			try
			{
				string rv = Strings.ResourceManager.GetString(value.GetType().Name + value.ToString());
				return string.IsNullOrEmpty(rv) ? value.ToString() : rv;
			}
			catch { return value.ToString(); }
		}


		internal bool ManageHotKeys(System.Windows.Forms.Keys keys)
		{
			if (SuspendHK)
				return false;
			else
				return mHotKeyManager.ManageHotKeys(keys);
		}

		internal void HKConnectDisconnect()
		{
			if (IsOpen)
				HKDisconnect();
			else
				HKConnect();
		}

		internal void HKConnect()
		{
			if (!IsOpen)
				OpenCom();
		}

		internal void HKDisconnect()
		{
			if (IsOpen)
			{
				if (!(InProgram && System.Windows.Forms.MessageBox.Show(Strings.DisconnectAnyway, "Warning", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning, System.Windows.Forms.MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
					CloseCom(true);
			}
		}

		internal void HelpOnLine()
		{ System.Diagnostics.Process.Start(@"http://lasergrbl.com/usage/"); }

		internal void GrblHoming()
		{ if (CanDoHoming)EnqueueCommand(new GrblCommand("$H")); }

		internal void GrblUnlock()
		{ if (CanUnlock) EnqueueCommand(new GrblCommand("$X")); }

		internal void SetNewZero()
		{ if (CanDoZeroing) EnqueueCommand(new GrblCommand("G92 X0 Y0")); }

		public int JogSpeed { get; set; }

		public int JogStep { get; set; }

		public bool SuspendHK { get; set; }

		public HotKeysManager HotKeys { get { return mHotKeyManager; } }

		internal void WriteHotkeys(System.Collections.Generic.List<HotKeysManager.HotKey> mLocalList)
		{
			mHotKeyManager.Clear();
			mHotKeyManager.AddRange(mLocalList);
			Settings.SetObject("Hotkey Setup", mHotKeyManager);
			Settings.Save();
		}

		internal void HKCustomButton(int index)
		{
			CustomButton cb = CustomButtons.GetButton(index);
			if (cb != null && cb.EnabledNow(this))
				ExecuteCustombutton(cb.GCode);
		}

		static System.Text.RegularExpressions.Regex bracketsRegEx = new System.Text.RegularExpressions.Regex(@"\[(?:[^]]+)\]");
		internal void ExecuteCustombutton(string buttoncode)
		{
			string[] arr = buttoncode.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			foreach (string str in arr)
			{
				if (str.Trim().Length > 0)
				{
					string tosend = bracketsRegEx.Replace(str, new System.Text.RegularExpressions.MatchEvaluator(EvaluateCB));
					EnqueueCommand(new GrblCommand(tosend));
				}
			}
		}


		private string EvaluateCB(System.Text.RegularExpressions.Match m)
		{
			try
			{
				decimal left = LoadedFile != null && LoadedFile.Range.DrawingRange.ValidRange ? LoadedFile.Range.DrawingRange.X.Min : 0;
				decimal right = LoadedFile != null && LoadedFile.Range.DrawingRange.ValidRange ? LoadedFile.Range.DrawingRange.X.Max : 0;
				decimal top = LoadedFile != null && LoadedFile.Range.DrawingRange.ValidRange ? LoadedFile.Range.DrawingRange.Y.Max : 0;
				decimal bottom = LoadedFile != null && LoadedFile.Range.DrawingRange.ValidRange ? LoadedFile.Range.DrawingRange.Y.Min : 0;
				decimal width = right - left;
				decimal height = top - bottom;
				decimal jogstep = JogStep;
				decimal jogspeed = JogSpeed;

				String text = m.Value.Substring(1, m.Value.Length - 2);
				Tools.Expression exp = new Tools.Expression(text);
				
				exp.AddSetVariable("left", (double)left);
				exp.AddSetVariable("right", (double)right);
				exp.AddSetVariable("top", (double)top);
				exp.AddSetVariable("bottom", (double)bottom);
				exp.AddSetVariable("width", (double)width);
				exp.AddSetVariable("height", (double)height);
				exp.AddSetVariable("jogstep", (double)jogstep);
				exp.AddSetVariable("jogspeed", (double)jogspeed);
				double dval = exp.EvaluateD();
				return m.Result(FormatNumber((decimal)dval));
			}
			catch { return m.Value; }
		}

		static string FormatNumber(decimal value)
		{ return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.000}", value); }

	}

	public class TimeProjection
	{
		private TimeSpan mETarget;
		private TimeSpan mEProgress;

		private long mStart;		//Start Time
		private long mEnd;			//End Time
		private long mPauseBegin;	//Pause begin Time
		private long mCumulatedPause;

		private bool mInPause;
		private bool mCompleted;
		private bool mStarted;

		private int mTargetCount;
		private int mExecutedCount;
		private int mSentCount;
		private int mErrorCount;
		private int mContinueCorrection;

		GrblCore.DetectedIssue mLastIssue;
		private System.Drawing.PointF mLastKnownWCO;

		public System.Drawing.PointF LastKnownWCO
		{
			get { return mLastKnownWCO; }
			set { if (InProgram) mLastKnownWCO = value; }
		}

		public TimeProjection()
		{ Reset(); }

		public void Reset()
		{
			mETarget = TimeSpan.Zero;
			mEProgress = TimeSpan.Zero;
			mStart = 0;
			mEnd = 0;
			mPauseBegin = 0;
			mCumulatedPause = 0;
			mInPause = false;
			mCompleted = false;
			mStarted = false;
			mExecutedCount = 0;
			mSentCount = 0;
			mErrorCount = 0;
			mTargetCount = 0;
			mContinueCorrection = 0;
			mLastIssue = GrblCore.DetectedIssue.Unknown;
			mLastKnownWCO = System.Drawing.PointF.Empty;
		}

		public TimeSpan EstimatedTarget
		{ get { return mETarget; } }

		public bool InProgram
		{ get { return mStarted && !mCompleted; } }

		public int Target
		{ get { return mTargetCount; } }

		public int Sent
		{ get { return mSentCount - mContinueCorrection; } }

		public int Executed
		{ get { return mExecutedCount - mContinueCorrection; } }

		public TimeSpan ProjectedTarget
		{
			get
			{
				if (mStarted)
				{
					double real = TrueJobTime.TotalSeconds;	//job time spent in execution
					double target = mETarget.TotalSeconds;	//total estimated
					double done = mEProgress.TotalSeconds;	//done of estimated

					if (done != 0)
						return TimeSpan.FromSeconds(real * target / done) + TotalJobPauses;
					else
						return EstimatedTarget;
				}
				else
					return TimeSpan.Zero;
			}
		}

		private TimeSpan TrueJobTime
		{ get { return TotalJobTime - TotalJobPauses; } }

		public TimeSpan TotalJobTime
		{
			get
			{
				if (mCompleted)
					return TimeSpan.FromMilliseconds(mEnd - mStart);
				else if (mStarted)
					return TimeSpan.FromMilliseconds(now - mStart);
				else
					return TimeSpan.Zero;
			}
		}

		private TimeSpan TotalJobPauses
		{
			get
			{
				if (mInPause)
					return TimeSpan.FromMilliseconds(mCumulatedPause + (now - mPauseBegin));
				else
					return TimeSpan.FromMilliseconds(mCumulatedPause);
			}
		}

		public void JobStart(TimeSpan EstimatedTarget, int LineCount)
		{
			if (!mStarted)
			{
				mETarget = EstimatedTarget;
				mTargetCount = LineCount;
				mEProgress = TimeSpan.Zero;
				mStart = Tools.HiResTimer.TotalMilliseconds;
				mPauseBegin = 0;
				mInPause = false;
				mCompleted = false;
				mStarted = true;
				mExecutedCount = 0;
				mSentCount = 0;
				mErrorCount = 0;
				mContinueCorrection = 0;
				mLastIssue = GrblCore.DetectedIssue.Unknown;
				mLastKnownWCO = System.Drawing.PointF.Empty;
			}
		}

		public void JobContinue(int position, int added)
		{
			if (!mStarted)
			{
				//	mETarget = EstimatedTarget;
				//	mTargetCount = LineCount;
				//	mEProgress = TimeSpan.Zero;
				//	mStart = Tools.HiResTimer.TotalMilliseconds;
				mPauseBegin = 0;
				mInPause = false;
				mCompleted = false;
				mStarted = true;
				mExecutedCount = position;
				mSentCount = position;
				mLastIssue = GrblCore.DetectedIssue.Unknown;
				//	mErrorCount = 0;
				mContinueCorrection = added;
			}
		}

		public void JobSent()
		{
			if (mStarted && !mCompleted)
				mSentCount++;
		}

		public void JobError()
		{
			if (mStarted && !mCompleted)
				mErrorCount++;
		}

		public void JobExecuted(TimeSpan EstimatedProgress)
		{
			if (mStarted && !mCompleted)
			{
				mExecutedCount++;
				mEProgress = EstimatedProgress;
			}
		}

		public void JobPause()
		{
			if (mStarted && !mCompleted && !mInPause)
			{
				mInPause = true;
				mPauseBegin = now;
			}
		}

		public void JobResume()
		{
			if (mStarted && !mCompleted && mInPause)
			{
				mCumulatedPause += Tools.HiResTimer.TotalMilliseconds - mPauseBegin;
				mInPause = false;
			}
		}

		public bool JobEnd()
		{
			if (mStarted && !mCompleted)
			{
				JobResume(); //nel caso l'ultimo comando fosse una pausa, la chiudo e la cumulo
				mEnd = Tools.HiResTimer.TotalMilliseconds;
				mCompleted = true;
				mStarted = false;
				return true;
			}

			return false;
		}

		public void JobIssue(GrblCore.DetectedIssue issue)
		{ mLastIssue = issue; }

		private long now
		{ get { return Tools.HiResTimer.TotalMilliseconds; } }

		public int ErrorCount
		{ get { return mErrorCount; } }

		public GrblCore.DetectedIssue LastIssue
		{ get { return mLastIssue; } }
	}

	[Serializable]
	public class GrblConf : System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int, decimal>>
	{
		public class GrblConfParam : ICloneable
		{
			private int mNumber;
			private decimal mValue;

			public GrblConfParam(int number, decimal value)
			{ mNumber = number; mValue = value; }

			public int Number
			{ get { return mNumber; } }

			public string DollarNumber
			{ get { return "$" + mNumber.ToString(); } }

			public string Parameter
			{ get { return CSVD.Settings.GetItem(mNumber.ToString(), 0); } }

			public decimal Value
			{
				get { return mValue; }
				set { mValue = value; }
			}

			public string Unit
			{ get { return CSVD.Settings.GetItem(mNumber.ToString(), 1); } }

			public string Description
			{ get { return CSVD.Settings.GetItem(mNumber.ToString(), 2); } }

			public object Clone()
			{ return this.MemberwiseClone(); }

		}

		private System.Collections.Generic.Dictionary<int, decimal> mData;
		private GrblCore.GrblVersionInfo mVersion;

		public GrblConf(GrblCore.GrblVersionInfo GrblVersion)
			: this()
		{
			mVersion = GrblVersion;
		}

		public GrblConf(GrblCore.GrblVersionInfo GrblVersion, System.Collections.Generic.Dictionary<int, decimal> configTable)
			: this(GrblVersion)
		{
			foreach (System.Collections.Generic.KeyValuePair<int, decimal> kvp in configTable)
				mData.Add(kvp.Key, kvp.Value);
		}

		public GrblConf()
		{ mData = new System.Collections.Generic.Dictionary<int, decimal>(); }

		public GrblCore.GrblVersionInfo GrblVersion
		{ get { return mVersion; } }

		private bool Version11
		{ get { return mVersion != null && mVersion >= new GrblCore.GrblVersionInfo(1, 1); } }

		private bool Version9
		{ get { return mVersion != null && mVersion >= new GrblCore.GrblVersionInfo(0, 9); } }

		private bool NoVersionInfo
		{ get { return mVersion == null; } }

		public int ExpectedCount
		{ get { return Version11 ? 34 : Version9 ? 31 : 23; } }

		public bool HomingEnabled
		{ get { return ReadWithDefault(Version9 ? 22 : 17, 1) != 0; } }

		public decimal MaxRateX
		{ get { return ReadWithDefault(Version9 ? 110 : 4, 4000); } }

		public decimal MaxRateY
		{ get { return ReadWithDefault(Version9 ? 111 : 5, 4000); } }

		public bool LaserMode
		{
			get
			{
				if (NoVersionInfo)
					return true;
				else
					return ReadWithDefault(Version11 ? 32 : -1, 0) != 0;
			}
		}

		public decimal MinPWM
		{ get { return ReadWithDefault(Version11 ? 31 : -1, 0); } }

		public decimal MaxPWM
		{ get { return ReadWithDefault(Version11 ? 30 : -1, 1000); } }

		public decimal ResolutionX
		{ get { return ReadWithDefault(Version9 ? 100 : 0, 250); } }

		public decimal ResolutionY
		{ get { return ReadWithDefault(Version9 ? 101 : 1, 250); } }

		public decimal TableWidth
		{ get { return ReadWithDefault(Version9 ? 130 : -1, 3000); } }

		public decimal TableHeight
		{ get { return ReadWithDefault(Version9 ? 131 : -1, 2000); } }


		private decimal ReadWithDefault(int number, decimal defval)
		{
			if (mVersion == null)
				return defval;
			else if (!mData.ContainsKey(number))
				return defval;
			else
				return mData[number];
		}

		//public object Clone()
		//{
		//	GrblConf rv = new GrblConf();
		//	rv.mVersion = mVersion != null ? mVersion.Clone() as GrblCore.GrblVersionInfo : null;
		//	foreach (System.Collections.Generic.KeyValuePair<int, GrblConf.GrblConfParam> kvp in this)
		//		rv.Add(kvp.Key, kvp.Value.Clone() as GrblConfParam);
		//	return rv;
		//}

		public System.Collections.Generic.List<GrblConf.GrblConfParam> ToList()
		{
			System.Collections.Generic.List<GrblConfParam> rv = new System.Collections.Generic.List<GrblConfParam>();
			foreach (System.Collections.Generic.KeyValuePair<int, decimal> kvp in mData)
				rv.Add(new GrblConfParam(kvp.Key, kvp.Value));
			return rv;
		}

		internal void Add(int num, decimal val)
		{
			mData.Add(num, val);
		}

		public int Count { get { return mData.Count; } }

		internal bool HasChanges(GrblConfParam p)
		{
			if (!mData.ContainsKey(p.Number))
				return true;
			else if (mData[p.Number] != p.Value)
				return true;
			else
				return false;
		}

		internal bool ContainsKey(int key)
		{
			return mData.ContainsKey(key);
		}

		internal void SetValue(int key, decimal value)
		{
			mData[key] = value;
		}

		public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int, decimal>> GetEnumerator()
		{
			return mData.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return mData.GetEnumerator();
		}
	}
}

/*
Idle: All systems are go, no motions queued, and it's ready for anything.
Run: Indicates a cycle is running.
Hold: A feed hold is in process of executing, or slowing down to a stop. After the hold is complete, Grbl will remain in Hold and wait for a cycle start to resume the program.
Door: (New in v0.9i) This compile-option causes Grbl to feed hold, shut-down the spindle and coolant, and wait until the door switch has been closed and the user has issued a cycle start. Useful for OEM that need safety doors.
Home: In the middle of a homing cycle. NOTE: Positions are not updated live during the homing cycle, but they'll be set to the home position once done.
Alarm: This indicates something has gone wrong or Grbl doesn't know its position. This state locks out all G-code commands, but allows you to interact with Grbl's settings if you need to. '$X' kill alarm lock releases this state and puts Grbl in the Idle state, which will let you move things again. As said before, be cautious of what you are doing after an alarm.
Check: Grbl is in check G-code mode. It will process and respond to all G-code commands, but not motion or turn on anything. Once toggled off with another '$C' command, Grbl will reset itself.
*/
