/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 18/11/2016
 * Time: 18:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace LaserGRBL
{
	/// <summary>
	/// Description of CommandThread.
	/// </summary>
	public class GrblCore : Tools.ThreadClass
	{
		public enum MacStatus
		{Disconnected, Connecting, Idle, Run, Hold, Door, Home, Alarm, Check, Jog}

		public enum JogDirection
		{ N ,S ,W ,E , NW, NE, SW, SE }
				
		public delegate void dlgOnMachineStatus();
		public delegate void dlgOnOverrideChange();
		
		public event dlgOnMachineStatus MachineStatusChanged;
		public event GrblFile.OnFileLoadedDlg OnFileLoaded;
		public event dlgOnOverrideChange OnOverrideChange;

		private System.Windows.Forms.Control syncro;
		private System.IO.Ports.SerialPort com;
		private GrblFile file;
		private System.Collections.Generic.Queue<GrblCommand> mQueue ;
		private System.Collections.Generic.List<IGrblRow> mSent;
		private System.Collections.Generic.Queue<GrblCommand> mPending ;
		
		private System.Collections.Generic.List<IGrblRow> mSentPtr;
		private System.Collections.Generic.Queue<GrblCommand> mQueuePtr;
		
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
		
		public GrblCore(System.Windows.Forms.Control syncroObject) : base(1, false, "Command Queue Thread")
		{
			syncro = syncroObject;
			com = new System.IO.Ports.SerialPort();
			
			file = new GrblFile();
			file.OnFileLoaded += RiseOnFileLoaded;
 
			mMachineStatus = MacStatus.Disconnected;
			
			this.com.DataReceived += OnDataReceived;
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


		public static readonly System.Collections.Generic.List<string> ImageExtensions = new System.Collections.Generic.List<string> { ".jpg", ".bmp", ".png", ".gif" };
		public void OpenFile()
		{
			string filename = null;
			using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
			{
				ofd.Filter = "Any supported file|*.nc;*.gcode;*.bmp;*.png;*.jpg;*.gif|GCODE Files|*.nc;*.gcode|Raster Image|*.bmp;*.png;*.jpg;*.gif";
				ofd.CheckFileExists = true;
				ofd.Multiselect = false;
				ofd.RestoreDirectory = true;
				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					filename = ofd.FileName;
			}

			if (filename != null)
			{


				if (ImageExtensions.Contains(System.IO.Path.GetExtension(filename).ToLowerInvariant())) //import raster image
				{
					RasterConverter.RasterToLaserForm.CreateAndShowDialog(file, filename);
				}
				else //load GCODE file
				{
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
					file.LoadFile(filename);
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
				}
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
						lock(this)
						{
							mSentPtr =  new System.Collections.Generic.List<IGrblRow>(); //assign sent queue
							mQueuePtr = new System.Collections.Generic.Queue<GrblCommand>();
							mQueuePtr.Enqueue(cmd);
						}

						try
						{
							
							while (cmd.Result == null) //resta in attesa della risposta
								;
							
							if (cmd.Result != "OK")
							{
								System.Windows.Forms.MessageBox.Show("Error exporting config!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
							}
							else
							{
							
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
								System.Windows.Forms.MessageBox.Show(String.Format("{0} Config exported with success!", msg), "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
							}
						}catch {System.Windows.Forms.MessageBox.Show("Error exporting config!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);}
						
						lock(this)
						{
							mQueuePtr = mQueue;
							mSentPtr = mSent; //restore queue
						}
					}
				}
				catch {System.Windows.Forms.MessageBox.Show("Error exporting config!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);}
			}
		}

		public void ImportConfig(string filename)
		{
			
			if (!System.IO.File.Exists(filename))
			{
				System.Windows.Forms.MessageBox.Show("File does not exist!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return;
			}
			
			if (mMachineStatus == MacStatus.Idle)
			{
				try
				{
					using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
					{
						lock(this)
						{
							mSentPtr =  new System.Collections.Generic.List<IGrblRow>(); //assign sent queue
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
							foreach (IGrblRow row in mSentPtr) {
								if (row is GrblCommand)
									if (((GrblCommand)row).Result != "OK")
										errors ++;
							}
							
							if (errors > 0)
								System.Windows.Forms.MessageBox.Show(String.Format("{0} Config imported with {1} errors!", mSentPtr.Count, errors) , "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
							else
								System.Windows.Forms.MessageBox.Show(String.Format("{0} Config imported with success!", mSentPtr.Count), "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
						}
						catch {System.Windows.Forms.MessageBox.Show("Error reading file!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);}
						
						lock(this)
						{
							mQueuePtr = mQueue;
							mSentPtr = mSent; //restore queue
						}
					}
				}
				catch {System.Windows.Forms.MessageBox.Show("Error reading file!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);}
			}
		}
		
		public void EnqueueProgram()
		{
			lock(this)
			{
				ClearQueue(true);
			
				mTP.Reset();
				mTP.JobStart(file.EstimatedTime, file.Count);

				foreach (GrblCommand cmd in file)
					mQueuePtr.Enqueue(cmd.Clone() as GrblCommand);
			}
		}

		public bool HasProgram
		{ get { return file != null && file.Count > 0; } }

		public void EnqueueCommand(GrblCommand cmd)
		{
			lock(this)
			{mQueuePtr.Enqueue(cmd.Clone() as GrblCommand);}
		}

		public string PortName
		{
			get { return com.PortName; }
			set { com.PortName = value; }
		}

		public int BaudRate
		{
			get { return com.BaudRate; }
			set { com.BaudRate = value; }
		}
		
		public void OpenCom()
		{
			mMachineStatus = MacStatus.Connecting;
			
			if (!com.IsOpen)
			{
				com.Open();
				com.DiscardOutBuffer();
				com.DiscardInBuffer();		
			}
			
			lock(this)
			{
				GrblReset();
				Start();
			}
		}
		
		public void CloseCom()
		{
			Stop();

			if (com.IsOpen)
			{
				com.DiscardOutBuffer();
				com.DiscardInBuffer();			
				com.Close();
			}
			
			mBuffer = 0;
			
			lock(this)
			{ClearQueue(false);}
			
			mMachineStatus = MacStatus.Disconnected;
		}
		
		public bool IsOpen
		{get{return mMachineStatus != MacStatus.Disconnected;}}
	
		#region Comandi immediati

		public void CycleStartResume()
		{SendImmediate(126);}
		
		public void FeedHold()
		{SendImmediate(33);}
		
		public void SafetyDoor()
		{SendImmediate(64);}
		
		public void QueryPosition()
		{SendImmediate(63);}
		
		public void GrblReset()
		{
			lock(this)
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

		public void SendImmediate(byte b)
		{
			try{
				lock(this)
				{if (com.IsOpen) com.Write(new byte[] { b }, 0, 1);}
			}catch{}
		}
		
		#endregion
		
		#region Public Property
		
		public int ProgramTarget
		{get { return mTP.Target; }}

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
		{ get {return mGrblVersion != null && mGrblVersion >= new Version(1,1); }}

		public bool SupportLaserMode
		{ get {return mGrblVersion != null && mGrblVersion >= new Version(1,1); }}
		
		public bool SupportJogging
		{ get {return mGrblVersion != null && mGrblVersion >= new Version(1,1); }}
		
		public bool SupportCSV
		{ get {return mGrblVersion != null && mGrblVersion >= new Version(1,1); }}

		public bool SupportOverride
		{ get {return mGrblVersion != null && mGrblVersion >= new Version(1,1); }}
		
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
		protected override void DoTheWork()
		{
			lock(this)
			{
				if (CanSend())
					SendLine();
				
				long now = Tools.HiResTimer.TotalMilliseconds;
				if (now - lastPosRequest > 200)
				{
					QueryPosition();
					lastPosRequest = now;
				}
			}
		}
		
		private bool CanSend()
		{
			GrblCommand next = mQueuePtr.Count > 0 ? mQueuePtr.Peek() : null;
			return next != null && (mBuffer + next.Command.Length +2) <= BUFFER_SIZE; //+2 for /r/n
		}

		private void SendLine()
		{
			try
			{
				GrblCommand tosend = mQueuePtr.Count > 0 ? mQueuePtr.Peek() : null;

				if (tosend != null)
				{
					tosend.SetSending();
					mSentPtr.Add(tosend);
					mPending.Enqueue(tosend);
					mQueuePtr.Dequeue();
				
					mBuffer += (tosend.Command.Length +2); //+2 for \r\n
					com.WriteLine(tosend.Command);

					if (mTP.InProgram)
						mTP.JobSent();
				}
			}
			catch {}
		}

		public int Buffer
		{ get { return BUFFER_SIZE - mBuffer; } }

		void OnDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
		{
			try
			{
				string rline = null;
				while ((rline = GetComLineOrDisconnect()) != null)
				{
					if (rline.Length > 0)
					{
						lock(this)
						{
							if (rline.ToLower().StartsWith("ok") || rline.ToLower().StartsWith("error"))
							{
								if (mPending.Count > 0)
								{
									GrblCommand pending = mPending.Dequeue();
	
									//mLaserPosition = new System.Drawing.PointF(pending.X != null ? (float)pending.X.Number : mLaserPosition.X, pending.Y != null ? (float)pending.Y.Number : mLaserPosition.Y);
	
									pending.SetResult(rline, SupportCSV);
									mBuffer -= (pending.Command.Length + 2); //+2 for \r\n

									if (mTP.InProgram)
										mTP.JobExecuted(pending.TimeOffset);

									//if (mQueue.Count == 0 && mPending.Count == 0)
									//{
									//	if (QueueStatus != null)
									//		QueueStatus(true);
									//}
								}
							}
							else if (rline.StartsWith("<") && rline.EndsWith(">"))
							{
								rline = rline.Substring(1, rline.Length - 2);
								System.Diagnostics.Debug.WriteLine(rline);
								if (rline.Contains("|")) //grbl > 1.1
								{
									string[] arr = rline.Split("|".ToCharArray());
									
									ParseMachineStatus(arr[0]);
									string mpos = arr[1].Substring(5, arr[1].Length - 5);
									string[] xyz = mpos.Split(",".ToCharArray());
									mLaserPosition = new System.Drawing.PointF(float.Parse(xyz[0], System.Globalization.NumberFormatInfo.InvariantInfo), float.Parse(xyz[1], System.Globalization.NumberFormatInfo.InvariantInfo));
									
									if (arr.Length > 4 && arr[4].StartsWith("Ov"))
										ParseOverrides(arr[4]);
								}
								else //<Idle,MPos:0.000,0.000,0.000,WPos:0.000,0.000,0.000>
								{
									string[] arr = rline.Split(",".ToCharArray());
									
									ParseMachineStatus(arr[0]);
									mLaserPosition = new System.Drawing.PointF(float.Parse(arr[1].Substring(5, arr[1].Length -5), System.Globalization.NumberFormatInfo.InvariantInfo), float.Parse(arr[2], System.Globalization.NumberFormatInfo.InvariantInfo));
								}
								
							}
							else if (rline.StartsWith("Grbl "))
							{
								//Grbl vX.Xx ['$' for help]
								try
								{
									int maj = int.Parse(rline.Substring(5,1));
									int min = int.Parse(rline.Substring(7,1));
									int build = (int)(rline.Substring(8,1).ToCharArray()[0]);
									
									mGrblVersion = new Version(maj, min, build);
								}
								catch {}
								mSentPtr.Add(new GrblMessage(rline, false));
							}
							else
							{
								mSentPtr.Add(new GrblMessage(rline, SupportCSV));
							}
						}
					}
				}
			} catch {}
		}

		private char[] trimarray = new char[] { '\r', '\n', ' ' };
		private string GetComLineOrDisconnect()
		{
			try
			{
				string rv = com.ReadLine();
				rv = rv.TrimEnd(trimarray); //rimuovi ritorno a capo
				rv = rv.Trim(); //rimuovi spazi iniziali e finali
				return rv.Length > 0 ? rv : null;
			}
			catch
			{
				try { CloseCom(); }
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
		{get {return mCurOvFeed;}}
		
		public int OverrideG0
		{get {return mCurOvRapids;}}
		
		public int OverrideS
		{get {return mCurOvSpindle;}}

		public int TOverrideG1
		{
			get {return mTarOvFeed;}
			set {mTarOvFeed = value;}
		}
		
		public int TOverrideG0
		{
			get {return mTarOvRapids;}
			set {mTarOvRapids = value;}
		}
		
		public int TOverrideS
		{
			get {return mTarOvSpindle;}
			set {mTarOvSpindle = value;}
		}
		
		private void ParseMachineStatus(string data)
		{
			MacStatus var = MacStatus.Disconnected;
			
			if (data.Contains(":"))
				data = data.Substring(0, data.IndexOf(':'));
			
			Enum.TryParse(data, out var);
			
			if (var == MacStatus.Idle && mQueuePtr.Count == 0 && mPending.Count == 0)
				mTP.JobEnd();
			
			if (mTP.InProgram && var == MacStatus.Idle) //bugfix for grbl sending Idle on G4
				var = MacStatus.Run;
			
			if (mMachineStatus != var)
			{
				mMachineStatus = var;
				RiseMachineStatusChanged();
				
				if (mTP.InProgram)
				{
					if (InPause)
						mTP.JobPause();
					else
						mTP.JobResume();
				}
			}

		}

		private bool InPause
		{ get { return mMachineStatus != MacStatus.Run && mMachineStatus != MacStatus.Idle; } }
		
		private void ClearQueue(bool sent)
		{
			mQueue.Clear();
			mPending.Clear();
			if (sent) mSent.Clear();
		}





		public bool CanLoadNewFile
		{ get { return !InProgram; } }

		public bool CanSendFile
		{ get { return IsOpen && MachineStatus == MacStatus.Idle && HasProgram; } }

		public bool CanImportExport
		{ get { return IsOpen && MachineStatus == MacStatus.Idle; } }

		public bool CanResetGrbl
		{ get { return IsOpen && MachineStatus != MacStatus.Disconnected; } }

		public bool CanSendManualCommand
		{ get { return IsOpen && MachineStatus != MacStatus.Disconnected; } }

		public bool CanGoHome
		{ get { return IsOpen && (MachineStatus == MacStatus.Idle || MachineStatus == GrblCore.MacStatus.Alarm); } }

		public bool CanFeedHold
		{ get { return IsOpen && MachineStatus == MacStatus.Run; } }

		public bool CanResumeHold
		{ get { return IsOpen && (MachineStatus == MacStatus.Door || MachineStatus == MacStatus.Hold); } }
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
		{Reset();}

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
		{get { return mETarget; }}

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

		public void JobEnd()
		{
			if (mStarted && !mCompleted)
			{
				JobResume(); //nel caso l'ultimo comando fosse una pausa, la chiudo e la cumulo
				mEnd = Tools.HiResTimer.TotalMilliseconds;
				mCompleted = true;
				mStarted = false;
			}
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
