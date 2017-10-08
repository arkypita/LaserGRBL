
using System;

namespace LaserGRBL
{
	/// <summary>
	/// Description of CommandThread.
	/// </summary>
	public class GrblCore
	{
		public enum MacStatus
		{ Unknown, Disconnected, Connecting, Idle, Run, Hold, Door, Home, Alarm, Check, Jog }

		public enum JogDirection
		{ N, S, W, E, NW, NE, SW, SE }

		public enum StreamingMode
		{ Buffered, Synchronous, RepeatOnError }

		public delegate void dlgOnMachineStatus();
		public delegate void dlgOnOverrideChange();
		public delegate void dlgOnLoopCountChange(decimal current);

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
		private System.Drawing.PointF mLaserPosition;

		private TimeProjection mTP = new TimeProjection();

		private MacStatus mMachineStatus;
		private const int BUFFER_SIZE = 120;
		private Version mGrblVersion;

		private int mCurOvFeed;
		private int mCurOvRapids;
		private int mCurOvSpindle;

		private int mTarOvFeed;
		private int mTarOvRapids;
		private int mTarOvSpindle;

		private decimal mLoopCount = 1;

		private Tools.ThreadObject TX;
		private Tools.ThreadObject RX;

		public GrblCore(System.Windows.Forms.Control syncroObject)
		{
			SetStatus(MacStatus.Disconnected);

			syncro = syncroObject;
			com = new ComWrapper.UsbSerial();

			TX = new Tools.ThreadObject(ThreadTX, 1, true, "Serial TX Thread", null);
			RX = new Tools.ThreadObject(ThreadRX, 1, true, "Serial RX Thread", null);

			file = new GrblFile(0, 0, 200, 300);  //create a fake range to use with manual movements

			file.OnFileLoaded += RiseOnFileLoaded;

			mQueue = new System.Collections.Generic.Queue<GrblCommand>();
			mPending = new System.Collections.Generic.Queue<GrblCommand>();
			mSent = new System.Collections.Generic.List<IGrblRow>();
			mBuffer = 0;

			mSentPtr = mSent;
			mQueuePtr = mQueue;
			mGrblVersion = null;

			mCurOvFeed = mCurOvRapids = mCurOvSpindle = 100;
			mTarOvFeed = mTarOvRapids = mTarOvSpindle = 100;
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
			if (OnFileLoaded != null)
				OnFileLoaded(elapsed, filename);
		}

		public GrblFile LoadedFile
		{ get { return file; } }


		public static readonly System.Collections.Generic.List<string> ImageExtensions = new System.Collections.Generic.List<string>(new string[] { ".jpg", ".bmp", ".png", ".gif" });
		public void OpenFile(System.Windows.Forms.Form parent)
		{
			try
			{
				string filename = null;
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

				if (filename != null)
				{
					Logger.LogMessage("OpenFile", "Open {0}", filename);
					Settings.SetObject("Core.LastOpenFile", filename);

					if (ImageExtensions.Contains(System.IO.Path.GetExtension(filename).ToLowerInvariant())) //import raster image
					{
						try
						{ RasterConverter.RasterToLaserForm.CreateAndShowDialog(this, filename, parent); }
						catch (Exception ex)
						{ Logger.LogException("RasterImport", ex); }
					}
					else //load GCODE file
					{
						System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

						try
						{ file.LoadFile(filename); }
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

		public void SaveProgram(string filename)
		{
			if (HasProgram)
				file.SaveProgram(filename);
		}

		public void ExportConfig(string filename)
		{
			if (mMachineStatus == MacStatus.Idle)
			{
				try
				{
					using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename))
					{
						GrblCommand cmd = new GrblCommand("$$");
						lock (this)
						{
							mSentPtr = new System.Collections.Generic.List<IGrblRow>(); //assign sent queue
							mQueuePtr = new System.Collections.Generic.Queue<GrblCommand>();
							mQueuePtr.Enqueue(cmd);
						}

						try
						{
							Tools.AutoResetTimer WaitResponseTimeout = new Tools.AutoResetTimer(TimeSpan.FromSeconds(10), true);

							//resta in attesa dell'invio del comando e della risposta
							while (cmd.Status == GrblCommand.CommandStatus.Queued || cmd.Status == GrblCommand.CommandStatus.WaitingResponse)
								if (WaitResponseTimeout.Expired)
									throw new TimeoutException("No response received from grbl!");
								else
									System.Threading.Thread.Sleep(10);

							if (cmd.Status == GrblCommand.CommandStatus.ResponseGood)
							{
								//attendi la ricezione di tutti i parametri
								long tStart = Tools.HiResTimer.TotalMilliseconds;
								long tLast = tStart;
								int counter = mSentPtr.Count;

								//finché l'ultima risposta è più recente di 1s e non sono passati più di 10s totali
								while (Tools.HiResTimer.TotalMilliseconds - tLast < 1000 && Tools.HiResTimer.TotalMilliseconds - tStart < 10000)
								{
									if (mSentPtr.Count != counter)
									{
										tLast = Tools.HiResTimer.TotalMilliseconds;
										counter = mSentPtr.Count;
									}
									else
									{
										System.Threading.Thread.Sleep(10);
									}
								}

								int msg = 0;
								foreach (IGrblRow row in mSentPtr)
								{
									if (row is GrblMessage)
									{
										sw.WriteLine(((GrblMessage)row).Message);
										msg++;
									}
								}

								sw.Close();
								System.Windows.Forms.MessageBox.Show(String.Format(Strings.BoxExportConfigSuccess, msg), Strings.BoxExportConfigSuccessTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
							}
							else
							{
								System.Windows.Forms.MessageBox.Show(Strings.BoxExportConfigError, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
							}
						}
						catch (Exception ex)
						{
							Logger.LogException("ExportConfig", ex);
							System.Windows.Forms.MessageBox.Show(Strings.BoxExportConfigError, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
						}


						lock (this)
						{
							mQueuePtr = mQueue;
							mSentPtr = mSent; //restore queue
						}
					}
				}
				catch (Exception ex)
				{
					Logger.LogException("ExportConfig", ex);
					System.Windows.Forms.MessageBox.Show(Strings.BoxExportConfigError, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				}
			}
		}

		public void ImportConfig(string filename)
		{

			if (!System.IO.File.Exists(filename))
			{
				System.Windows.Forms.MessageBox.Show(Strings.BoxExportConfigFileNotFound, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return;
			}

			if (mMachineStatus == MacStatus.Idle)
			{
				try
				{
					using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
					{
						lock (this)
						{
							mSentPtr = new System.Collections.Generic.List<IGrblRow>(); //assign sent queue
							mQueuePtr = new System.Collections.Generic.Queue<GrblCommand>();

							string rline = null;
							while ((rline = sr.ReadLine()) != null)
								if (rline.StartsWith("$"))
									mQueuePtr.Enqueue(new GrblCommand(rline));
						}

						try
						{
							sr.Close();
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
								System.Windows.Forms.MessageBox.Show(String.Format(Strings.BoxImportConfigWithError, mSentPtr.Count, errors), Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
							else
								System.Windows.Forms.MessageBox.Show(String.Format(Strings.BoxImportConfigWithoutError, mSentPtr.Count), Strings.BoxExportConfigSuccessTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
						}
						catch (Exception ex)
						{
							Logger.LogException("ImportConfig", ex);
							System.Windows.Forms.MessageBox.Show(Strings.BoxImportConfigFileError, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
						}

						lock (this)
						{
							mQueuePtr = mQueue;
							mSentPtr = mSent; //restore queue
						}
					}
				}
				catch (Exception ex)
				{
					Logger.LogException("ImportConfig", ex);
					System.Windows.Forms.MessageBox.Show(Strings.BoxImportConfigFileError, Strings.BoxExportConfigErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				}
			}
		}

		public void EnqueueProgram()
		{
			lock (this)
			{
				ClearQueue(true);

				mTP.Reset();
				mTP.JobStart(file.EstimatedTime, file.Count);
				Logger.LogMessage("EnqueueProgram", "Running program, {0} lines", file.Count);

				foreach (GrblCommand cmd in file)
					mQueuePtr.Enqueue(cmd.Clone() as GrblCommand);
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

			com.Configure(conf);
		}

		public void OpenCom()
		{
			try
			{
				SetStatus(MacStatus.Connecting);

				if (!com.IsOpen)
				{
					com.Open();
				}

				lock (this)
				{
					GrblReset();
					RX.Start();
					TX.Start();
				}
			}
			catch (Exception ex)
			{
				Logger.LogException("OpenCom", ex);
				SetStatus(MacStatus.Disconnected);
				System.Windows.Forms.MessageBox.Show(ex.Message, Strings.BoxConnectErrorTitle, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				com.Close(true);
			}
		}

		public void CloseCom(bool auto)
		{
			try
			{
				if (com.IsOpen)
					com.Close(auto);

				mBuffer = 0;

				TX.Stop();
				RX.Stop();

				lock (this)
				{ ClearQueue(false); }

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
		{ SendImmediate(126); }

		public void FeedHold()
		{ SendImmediate(33); }

		public void SafetyDoor()
		{ SendImmediate(64); }

		private void QueryPosition()
		{ SendImmediate(63, true); }

		public void GrblReset()
		{
			lock (this)
			{
				ClearQueue(true);
				mBuffer = 0;
				mTP.JobEnd();
				SendImmediate(24);

				mCurOvFeed = mCurOvRapids = mCurOvSpindle = 100;
				mTarOvFeed = mTarOvRapids = mTarOvSpindle = 100;
			}

			RiseOverrideChanged();
		}

		public void SendImmediate(byte b, bool mute = false)
		{
			try
			{
				if (!mute) Logger.LogMessage("SendImmediate", "Send Immediate Command [{0}]", b);

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

		public System.Drawing.PointF LaserPosition
		{ get { return mLaserPosition; } }

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
		{ get { return mGrblVersion != null && mGrblVersion >= new Version(1, 1); } }

		public bool SupportLaserMode
		{ get { return mGrblVersion != null && mGrblVersion >= new Version(1, 1); } }

		public bool SupportJogging
		{ get { return mGrblVersion != null && mGrblVersion >= new Version(1, 1); } }

		public bool SupportCSV
		{ get { return mGrblVersion != null && mGrblVersion >= new Version(1, 1); } }

		public bool SupportOverride
		{ get { return mGrblVersion != null && mGrblVersion >= new Version(1, 1); } }

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

		public void Jog(JogDirection dir, decimal size, decimal speed)
		{
			if (JogEnabled)
			{
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

		internal void JogHome(decimal speed)
		{
			if (JogEnabled)
			{
				if (SupportJogging)
				{
					EnqueueCommand(new GrblCommand(string.Format("$J=G90X0Y0F{0}", speed)));
				}
				else
				{
					EnqueueCommand(new GrblCommand(string.Format("G90")));
					EnqueueCommand(new GrblCommand(string.Format("G0X0Y0F{0}", speed)));
				}
			}
		}

		private long lastPosRequest;
		protected void ThreadTX()
		{
			lock (this)
			{
				try
				{
					if (!TX.MustExitTH() && CanSend())
						SendLine();

					long now = Tools.HiResTimer.TotalMilliseconds;
					if (now - lastPosRequest > 200)
					{
						QueryPosition();
						lastPosRequest = now;
					}

					TX.SleepTime = CanSend() ? 0 : 1; //sleep only if no more data to send
				}
				catch (Exception ex)
				{ Logger.LogException("ThreadTX", ex); }
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
		{return (mBuffer + command.SerialData.Length) <= BUFFER_SIZE;}

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
				}
				catch (Exception ex)
				{
					if (tosend != null) Logger.LogMessage("SendLine", "Error sending [{0}] command", tosend.Command);
					Logger.LogException("SendLine", ex);
				}
				finally { tosend.DeleteHelper(); }
			}
		}

		public int Buffer
		{ get { return BUFFER_SIZE - mBuffer; } }

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
				int build = (int)(rline.Substring(8, 1).ToCharArray()[0]);
				mGrblVersion = new Version(maj, min, build);
			}
			catch (Exception ex)
			{
				Logger.LogMessage("VersionInfo", "Ex on [{0}] message", rline);
				Logger.LogException("VersionInfo", ex);
			}
			mSentPtr.Add(new GrblMessage(rline, false));
		}

		private void ManageRealTimeStatus(string rline)
		{
			try
			{
				rline = rline.Substring(1, rline.Length - 2);
				//System.Diagnostics.Debug.WriteLine(rline);
				if (rline.Contains("|")) //grbl > 1.1
				{
					string[] arr = rline.Split("|".ToCharArray());

					ParseMachineStatus(arr[0]);
					string mpos = arr[1].Substring(5, arr[1].Length - 5);
					string[] xyz = mpos.Split(",".ToCharArray());
					mLaserPosition = new System.Drawing.PointF(float.Parse(xyz[0], System.Globalization.NumberFormatInfo.InvariantInfo), float.Parse(xyz[1], System.Globalization.NumberFormatInfo.InvariantInfo));

					for (int i = 1; i < arr.Length; i++)
						if (arr[i].StartsWith("Ov"))
							ParseOverrides(arr[i]);
				}
				else //<Idle,MPos:0.000,0.000,0.000,WPos:0.000,0.000,0.000>
				{
					string[] arr = rline.Split(",".ToCharArray());

					ParseMachineStatus(arr[0]);
					mLaserPosition = new System.Drawing.PointF(float.Parse(arr[1].Substring(5, arr[1].Length - 5), System.Globalization.NumberFormatInfo.InvariantInfo), float.Parse(arr[2], System.Globalization.NumberFormatInfo.InvariantInfo));
				}
			}
			catch (Exception ex)
			{
				Logger.LogMessage("RealTimeStatus", "Ex on [{0}] message", rline);
				Logger.LogException("RealTimeStatus", ex);
			}
		}

		private void ManageCommandResponse(string rline)
		{
			try
			{
				if (mPending.Count > 0)
				{
					GrblCommand pending = mPending.Dequeue();

					pending.SetResult(rline, SupportCSV);
					mBuffer -= pending.SerialData.Length;

					if (mTP.InProgram && pending.RepeatCount == 0) //solo se non è una ripetizione aggiorna il tempo
						mTP.JobExecuted(pending.TimeOffset);

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
				try { CloseCom(true); }
				catch { }
				return null;
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
			if (mTP.JobEnd() && mLoopCount > 1 && mMachineStatus != MacStatus.Check)
			{
				LoopCount--;
				EnqueueProgram();
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

		public bool CanGoHome
		{ get { return IsOpen && (MachineStatus == MacStatus.Idle || MachineStatus == GrblCore.MacStatus.Alarm); } }

		public bool CanFeedHold
		{ get { return IsOpen && MachineStatus == MacStatus.Run; } }

		public bool CanResumeHold
		{ get { return IsOpen && (MachineStatus == MacStatus.Door || MachineStatus == MacStatus.Hold); } }

		public decimal LoopCount 
		{ get { return mLoopCount; } set { mLoopCount = value; if (OnLoopCountChange != null) OnLoopCountChange(mLoopCount); } }

		private StreamingMode CurrentStreamingMode
		{ get {return (StreamingMode)Settings.GetObject("Streaming Mode", StreamingMode.Buffered); }}

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

		public TimeProjection()
		{ Reset(); }

		public void Reset()
		{
			mStart = 0;
			mEnd = 0;
			mPauseBegin = 0;
			mCumulatedPause = 0;
			mInPause = false;
			mCompleted = false;
			mStarted = false;
		}

		public TimeSpan EstimatedTarget
		{ get { return mETarget; } }

		public bool InProgram
		{ get { return mStarted && !mCompleted; } }

		public int Target
		{ get { return mTargetCount; } }

		public int Sent
		{ get { return mSentCount; } }

		public int Executed
		{ get { return mExecutedCount; } }

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
			}
		}

		public void JobSent()
		{
			if (mStarted && !mCompleted)
				mSentCount++;
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

		private long now
		{ get { return Tools.HiResTimer.TotalMilliseconds; } }
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
