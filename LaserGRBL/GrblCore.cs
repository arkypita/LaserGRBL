﻿//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using Sound;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;

namespace LaserGRBL
{

	/// <summary>
	/// Description of CommandThread.
	/// </summary>
	public class GrblCore
	{
		public static PSHelper.MaterialDB MaterialDB = PSHelper.MaterialDB.Load();

		public delegate void dlgIssueDetector(DetectedIssue issue);
		public delegate void dlgOnMachineStatus();
		public delegate void dlgOnOverrideChange();
		public delegate void dlgOnLoopCountChange(decimal current);
		public delegate void dlgJogStateChange(bool jog);

		public event dlgIssueDetector IssueDetected;
		public event dlgOnMachineStatus MachineStatusChanged;
		public event GrblFile.OnFileLoadedDlg OnFileLoading;
		public event GrblFile.OnFileLoadedDlg OnFileLoaded;
		public event dlgOnOverrideChange OnOverrideChange;
		public event dlgOnLoopCountChange OnLoopCountChange;
		public event dlgJogStateChange JogStateChange;

		private System.Windows.Forms.Control syncro;
		protected ComWrapper.IComWrapper com;
		private GrblFile file;
		private Queue<GrblCommand> mQueue; //vera coda di quelli da mandare
		private GrblCommand mRetryQueue; //coda[1] di quelli in attesa di risposta
		private Queue<GrblCommand> mPending; //coda di quelli in attesa di risposta
		private List<IGrblRow> mSent; //lista di quelli mandati

		private Queue<GrblCommand> mQueuePtr; //puntatore a coda di quelli da mandare (normalmente punta a mQueue, salvo per import/export configurazione)
		private List<IGrblRow> mSentPtr; //puntatore a lista di quelli mandati (normalmente punta a mSent, salvo per import/export configurazione)

		private string mOrturWelcomeSeen = null;
		private string mOrturVersionSeen = null;
		private int mBuffer;
		private int stuckBufferCounter = 0;
		private GPoint mMPos;
		private GPoint mWCO;
		private int mGrblBlocks = -1;
		private int mGrblBuffer = -1;
		private JogDirection mPrenotedJogDirection = JogDirection.None;

		protected TimeProjection mTP = new TimeProjection();

		private MacStatus mMachineStatus;
		private static int BUFFER_SIZE = 127;

		private float mCurF;
		private float mCurS;

		private int mCurOvLinear;
		private int mCurOvRapids;
		private int mCurOvPower;

		private int mTarOvLinear;
		private int mTarOvRapids;
		private int mTarOvPower;

		private decimal mLoopCount = 1;

		protected Tools.PeriodicEventTimer QueryTimer;

		private Tools.ThreadObject TX;
		private Tools.ThreadObject RX;

		private long connectStart;

		protected Tools.ElapsedFromEvent debugLastStatusDelay;
		protected Tools.ElapsedFromEvent debugLastMoveDelay;

		private ThreadingMode mThreadingMode = ThreadingMode.UltraFast;
		private HotKeysManager mHotKeyManager;

		public UsageStats.UsageCounters UsageCounters;


		public GrblCore(System.Windows.Forms.Control syncroObject, PreviewForm cbform, JogForm jogform)
		{
			if (Type != Firmware.Grbl) Logger.LogMessage("Program", "Load {0} core", Type);

			SetStatus(MacStatus.Disconnected);
			syncro = syncroObject;
			com = new ComWrapper.UsbSerial();

			debugLastStatusDelay = new Tools.ElapsedFromEvent();
			debugLastMoveDelay = new Tools.ElapsedFromEvent();

			mThreadingMode = Settings.GetObject("Threading Mode", ThreadingMode.UltraFast);
			QueryTimer = new Tools.PeriodicEventTimer(TimeSpan.FromMilliseconds(mThreadingMode.StatusQuery), false);
			TX = new Tools.ThreadObject(ThreadTX, 1, true, "Serial TX Thread", StartTX);
			RX = new Tools.ThreadObject(ThreadRX, 1, true, "Serial RX Thread", null);

			file = new GrblFile(0, 0, 200, 300);  //create a fake range to use with manual movements

			file.OnFileLoading += RiseOnFileLoading;
			file.OnFileLoaded += RiseOnFileLoaded;

			mQueue = new System.Collections.Generic.Queue<GrblCommand>();
			mPending = new System.Collections.Generic.Queue<GrblCommand>();
			mSent = new System.Collections.Generic.List<IGrblRow>();
			mBuffer = 0;

			mSentPtr = mSent;
			mQueuePtr = mQueue;

			mCurOvLinear = mCurOvRapids = mCurOvPower = 100;
			mTarOvLinear = mTarOvRapids = mTarOvPower = 100;

			if (!Settings.ExistObject("Hotkey Setup")) Settings.SetObject("Hotkey Setup", new HotKeysManager());
			mHotKeyManager = Settings.GetObject<HotKeysManager>("Hotkey Setup", null);
			mHotKeyManager.Init(this, cbform, jogform);

			UsageCounters = new UsageStats.UsageCounters();

			if (GrblVersion != null)
				CSVD.LoadAppropriateSettings(GrblVersion); //load setting for last known version
		}

		internal void HotKeyOverride(HotKeysManager.HotKey.Actions action)
		{

			switch (action)
			{
				case HotKeysManager.HotKey.Actions.OverridePowerDefault:
					mTarOvPower = 100; break;
				case HotKeysManager.HotKey.Actions.OverridePowerUp:
					mTarOvPower = Math.Min(mTarOvPower + 1, 200); break;
				case HotKeysManager.HotKey.Actions.OverridePowerDown:
					mTarOvPower = Math.Max(mTarOvPower - 1, 10); break;
				case HotKeysManager.HotKey.Actions.OverrideLinearDefault:
					mTarOvLinear = 100; break;
				case HotKeysManager.HotKey.Actions.OverrideLinearUp:
					mTarOvLinear = Math.Min(mTarOvLinear + 1, 200); break;
				case HotKeysManager.HotKey.Actions.OverrideLinearDown:
					mTarOvLinear = Math.Max(mTarOvLinear - 1, 10); break;
				case HotKeysManager.HotKey.Actions.OverrideRapidDefault:
					mTarOvRapids = 100; break;
				case HotKeysManager.HotKey.Actions.OverrideRapidUp:
					mTarOvRapids = Math.Min(mTarOvRapids * 2, 100); break;
				case HotKeysManager.HotKey.Actions.OverrideRapidDown:
					mTarOvRapids = Math.Max(mTarOvRapids / 2, 25); break;
				default:
					break;
			}
		}

		internal string ValidateConfig(int parid, object value)
		{
			if (Configuration == null)
				return null;

			return Configuration.ValidateConfig(parid, value);
		}

		public virtual Firmware Type
		{ get { return Firmware.Grbl; } }

		public GrblConf Configuration
		{
			get { return Settings.GetObject("Grbl Configuration", new GrblConf()); }
			set
			{
				if (value.Count > 0 && value.GrblVersion != null)
				{
					Settings.SetObject("Grbl Configuration", value);
					Settings.Save();
				}
			}
		}

		protected void SetStatus(MacStatus value)
		{
			lock (this)
			{
				if (mMachineStatus != value)
				{
					Logger.LogMessage("SetStatus", "Machine status [{0}]", value);

					if (mMachineStatus == MacStatus.Connecting && value != MacStatus.Disconnected)
						SoundEvent.PlaySound(SoundEvent.EventId.Connect);
					if (mMachineStatus != MacStatus.Unknown && value == MacStatus.Disconnected)
						SoundEvent.PlaySound(SoundEvent.EventId.Disconnect);

					mMachineStatus = value;
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
		}

		internal virtual void SendHomingCommand()
		{
			EnqueueCommand(new GrblCommand("$H"));
		}

		internal virtual void SendUnlockCommand()
		{
			EnqueueCommand(new GrblCommand("$X"));
		}

		public GrblVersionInfo GrblVersion
		{
			get { return Settings.GetObject<GrblVersionInfo>("Last GrblVersion known", null); }
			set
			{
				if (GrblVersion != null)
					Logger.LogMessage("VersionInfo", "Detected Grbl v{0}", value);

				if (GrblVersion == null || !GrblVersion.Equals(value))
				{
					CSVD.LoadAppropriateSettings(value);

					Settings.SetObject("Last GrblVersion known", value);
					Settings.Save();
				}
			}
		}

		protected void SetIssue(DetectedIssue issue)
		{
			mTP.JobIssue(issue);
			Logger.LogMessage("Issue detector", "{0} [{1},{2},{3}]", issue, FreeBuffer, GrblBuffer, GrblBlock);

			if (issue > 0) //negative numbers indicate issue caused by the user, so must not be report to UI
				RiseIssueDetected(issue);
		}

		void RiseJogStateChange(bool jog)
		{
			if (JogStateChange != null)
			{
				if (syncro.InvokeRequired)
					syncro.BeginInvoke(new dlgJogStateChange(RiseJogStateChange), jog);
				else
					JogStateChange(jog);
			}
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

		void RiseOnFileLoading(long elapsed, string filename)
		{
			mTP.Reset();

			if (OnFileLoaded != null)
				OnFileLoading(elapsed, filename);
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
				OpenFile(parent, Settings.GetObject<string>("Core.LastOpenFile", null));
		}

		public static readonly List<string> ImageExtensions = new List<string>(new string[] { ".jpg", ".bmp", ".png", ".gif" });
		public static readonly List<string> GCodeExtensions = new List<string>(new string[] { ".nc", ".cnc", ".tap", ".gcode", ".ngc" });
		public void OpenFile(System.Windows.Forms.Form parent, string filename = null, bool append = false)
		{
			if (!CanLoadNewFile) return;

			try
			{
				if (filename == null)
				{
					using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
					{
						//pre-select last file if exist
						string lastFN = Settings.GetObject<string>("Core.LastOpenFile", null);
						if (lastFN != null && System.IO.File.Exists(lastFN))
							ofd.FileName = lastFN;

						ofd.Filter = "Any supported file|*.nc;*.cnc;*.tap;*.gcode;*.ngc;*.bmp;*.png;*.jpg;*.gif;*.svg|GCODE Files|*.nc;*.cnc;*.tap;*.gcode;*.ngc|Raster Image|*.bmp;*.png;*.jpg;*.gif|Vector Image (experimental)|*.svg";
						ofd.CheckFileExists = true;
						ofd.Multiselect = false;
						ofd.RestoreDirectory = true;

						System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.DialogResult.Cancel;
						try
						{
							dialogResult = ofd.ShowDialog();
						}
						catch (System.Runtime.InteropServices.COMException)
						{
							ofd.AutoUpgradeEnabled = false;
							dialogResult = ofd.ShowDialog();
						}

						if (dialogResult == System.Windows.Forms.DialogResult.OK)
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
							RasterConverter.RasterToLaserForm.CreateAndShowDialog(this, filename, parent, append);
							UsageCounters.RasterFile++;
						}
						catch (Exception ex)
						{ Logger.LogException("RasterImport", ex); }
					}
					else if (System.IO.Path.GetExtension(filename).ToLowerInvariant() == ".svg")
					{
						SvgConverter.SvgModeForm.Mode mode = SvgConverter.SvgModeForm.Mode.Vector;// SvgConverter.SvgModeForm.CreateAndShow(filename);
						if (mode == SvgConverter.SvgModeForm.Mode.Vector)
						{
							try
							{
								SvgConverter.SvgToGCodeForm.CreateAndShowDialog(this, filename, parent, append);
								UsageCounters.SvgFile++;
							}
							catch (Exception ex)
							{ Logger.LogException("SvgImport", ex); }
						}
						else if (mode == SvgConverter.SvgModeForm.Mode.Raster)
						{
							throw new NotImplementedException();
							//string bmpname = filename + ".png";
							//string fcontent = System.IO.File.ReadAllText(filename);
							//Svg.SvgDocument svg = Svg.SvgDocument.FromSvg<Svg.SvgDocument>(fcontent);
							//svg.Ppi = 600;
							//
							//using (Bitmap bmp = svg.Draw())
							//{
							//	bmp.SetResolution(600, 600);
							//
							//	//codec options not supported in C# png encoder https://efundies.com/c-sharp-save-png/
							//	//quality always 100%
							//
							//	//ImageCodecInfo codecinfo = GetEncoder(ImageFormat.Png);
							//	//EncoderParameters paramlist = new EncoderParameters(1);
							//	//paramlist.Param[0] = new EncoderParameter(Encoder.Quality, 30L); 
							//
							//
							//	if (System.IO.File.Exists(bmpname))
							//		System.IO.File.Delete(bmpname);
							//
							//	bmp.Save(bmpname/*, codecinfo, paramlist*/);
							//}
							//
							//try
							//{
							//	RasterConverter.RasterToLaserForm.CreateAndShowDialog(this, bmpname, parent, append);
							//	UsageCounters.RasterFile++;
							//	if (System.IO.File.Exists(bmpname))
							//		System.IO.File.Delete(bmpname);
							//}
							//catch (Exception ex)
							//{ Logger.LogException("SvgBmpImport", ex); }
						}
					}
					else if (GCodeExtensions.Contains(System.IO.Path.GetExtension(filename).ToLowerInvariant()))  //load GCODE file
					{
						System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

						try
						{
							file.LoadFile(filename, append);
							UsageCounters.GCodeFile++;
						}
						catch (Exception ex)
						{ Logger.LogException("GCodeImport", ex); }

						System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
					}
					else
					{
						System.Windows.Forms.MessageBox.Show(Strings.UnsupportedFiletype, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.LogException("OpenFile", ex);
			}
		}

		private ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
				{
					return codec;
				}
			}
			return null;
		}

		public void SaveProgram(bool header, bool footer, bool between, int cycles)
		{
			if (HasProgram)
			{
				string filename = null;
				using (System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog())
				{
					string lastFN = Settings.GetObject<string>("Core.LastOpenFile", null);
					if (lastFN != null)
					{
						string fn = System.IO.Path.GetFileNameWithoutExtension(lastFN);
						string path = System.IO.Path.GetDirectoryName(lastFN);
						sfd.FileName = System.IO.Path.Combine(path, fn + ".nc");
					}

					sfd.Filter = "GCODE Files|*.nc";
					sfd.AddExtension = true;
					sfd.RestoreDirectory = true;

					System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.DialogResult.Cancel;
					try
					{
						dialogResult = sfd.ShowDialog();
					}
					catch (System.Runtime.InteropServices.COMException)
					{
						sfd.AutoUpgradeEnabled = false;
						dialogResult = sfd.ShowDialog();
					}

					if (dialogResult == System.Windows.Forms.DialogResult.OK)
						filename = sfd.FileName;
				}

				if (filename != null)
					file.SaveProgram(filename, header, footer, between, cycles);
			}
		}

		public virtual void RefreshConfig()
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
					while (cmd.Status == CommandStatus.Queued || cmd.Status == CommandStatus.WaitingResponse)
					{
						if (WaitResponseTimeout.Expired)
							throw new TimeoutException("No response received from grbl!");
						else
							System.Threading.Thread.Sleep(10);
					}

					if (cmd.Status == CommandStatus.ResponseGood)
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
								conf.AddOrUpdate(row.GetMessage());
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
			private List<IGrblRow> ErrorLines = new List<IGrblRow>();

			public WriteConfigException(System.Collections.Generic.List<IGrblRow> mSentPtr)
			{
				foreach (IGrblRow row in mSentPtr)
					if (row is GrblCommand)
						if (((GrblCommand)row).Status != CommandStatus.ResponseGood)
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

			public List<IGrblRow> Errors
			{ get { return ErrorLines; } }
		}

		public void WriteConfig(List<GrblConf.GrblConfParam> config)
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
							if (((GrblCommand)row).Status != CommandStatus.ResponseGood)
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
					RunProgramFromStart(false, true);
				else
					UserWantToContinue();
			}
		}

		public void AbortProgram()
		{
			if (CanAbortProgram)
			{
				try
				{
					Logger.LogMessage("ManualAbort", "Program aborted by user action!");

					SetIssue(DetectedIssue.ManualAbort);
					mTP.JobEnd();

					lock (this)
					{
						mQueue.Clear(); //flush the queue of item to send
						mQueue.Enqueue(new GrblCommand("M5")); //shut down laser
					}
				}
				catch (Exception ex)
				{
					Logger.LogException("Abort Program", ex);
				}

			}
		}

		public void RunProgramFromPosition()
		{
			if (CanSendFile)
			{
				bool homing = false;
				int position = LaserGRBL.RunFromPositionForm.CreateAndShowDialog(LoadedFile.Count, Configuration.HomingEnabled, out homing);
				if (position >= 0)
					ContinueProgramFromKnown(position, homing, false);
			}
		}

		private void UserWantToContinue()
		{
			bool setwco = mWCO == GPoint.Zero && mTP.LastKnownWCO != GPoint.Zero;
			bool homing = MachinePosition == GPoint.Zero && mTP.LastIssue != DetectedIssue.ManualAbort && mTP.LastIssue != DetectedIssue.ManualReset; //potrebbe essere dovuto ad un hard reset -> posizione non affidabile
			int position = LaserGRBL.ResumeJobForm.CreateAndShowDialog(mTP.Executed, mTP.Sent, mTP.Target, mTP.LastIssue, Configuration.HomingEnabled, homing, out homing, setwco, setwco, out setwco, mTP.LastKnownWCO);

			if (position == 0)
				RunProgramFromStart(homing);
			if (position > 0)
				ContinueProgramFromKnown(position, homing, setwco);
		}

		private void RunProgramFromStart(bool homing, bool first = false, bool pass = false)
		{
			lock (this)
			{
				ClearQueue(true);

				mTP.Reset();

				if (homing)
				{
					Logger.LogMessage("EnqueueProgram", "Push Homing ($H)");
					mQueuePtr.Enqueue(new GrblCommand("$H"));
				}

				if (first)
				{
					Logger.LogMessage("EnqueueProgram", "Push Header");
					ExecuteCustombutton(Settings.GetObject("GCode.CustomHeader", Constants.GCODE_STD_HEADER));
				}

				if (pass)
				{
					Logger.LogMessage("EnqueueProgram", "Push Passes");
					ExecuteCustombutton(Settings.GetObject("GCode.CustomPasses", Constants.GCODE_STD_PASSES));
				}


				Logger.LogMessage("EnqueueProgram", "Push File, {0} lines", file.Count);
				foreach (GrblCommand cmd in file)
					mQueuePtr.Enqueue(cmd.Clone() as GrblCommand);

				mTP.JobStart(LoadedFile, mQueuePtr);
				Logger.LogMessage("EnqueueProgram", "Running program, {0} lines", file.Count);
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
					GPoint pos = homing ? GPoint.Zero : MachinePosition;
					GPoint wco = mTP.LastKnownWCO;
					GPoint cur = pos - wco;
					mQueue.Enqueue(new GrblCommand(String.Format("G92 X{0} Y{1} Z{2}", cur.X.ToString(System.Globalization.CultureInfo.InvariantCulture), cur.Y.ToString(System.Globalization.CultureInfo.InvariantCulture), cur.Z.ToString(System.Globalization.CultureInfo.InvariantCulture))));
				}

				for (int i = 0; i < position && i < file.Count; i++) //analizza fino alla posizione
					spb.AnalyzeCommand(file[i], false);

				mQueuePtr.Enqueue(new GrblCommand("G90")); //absolute coordinate
				mQueuePtr.Enqueue(new GrblCommand(string.Format("M5 G0 {0} {1} {2} {3} {4}", spb.X, spb.Y, spb.Z, spb.F, spb.S))); //fast go to the computed position with laser off and set speed and power
				mQueuePtr.Enqueue(new GrblCommand(spb.GetSettledModals()));

				mTP.JobContinue(LoadedFile, position, mQueuePtr.Count);

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
				BUFFER_SIZE = 127; //reset to default buffer size
				SetStatus(MacStatus.Connecting);
				connectStart = Tools.HiResTimer.TotalMilliseconds;

				if (!com.IsOpen)
					com.Open();

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

		public void CycleStartResume(bool auto)
		{
			if (CanResumeHold)
			{
				mHoldByUserRequest = false;
				SendImmediate(126);
			}
		}

		public void FeedHold(bool auto)
		{
			if (CanFeedHold)
			{
				mHoldByUserRequest = !auto;
				SendImmediate(33);
			}
		}

		public void SafetyDoor()
		{ SendImmediate(64); }

		// GRBL & smoothie : Send "?" to retrieve position
		// MarlinCore : Override : Send "M114\n"
		protected virtual void QueryPosition()
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

		private void InternalReset(bool device)
		{
			lock (this)
			{
				ClearQueue(true);
				mBuffer = 0;
				mTP.JobEnd();
				mCurOvLinear = mCurOvRapids = mCurOvPower = 100;
				mTarOvLinear = mTarOvRapids = mTarOvPower = 100;

				if (device) SendBoardResetCommand();
			}
			RiseOverrideChanged();
		}

		protected virtual void SendBoardResetCommand()
		{
			SendImmediate(24);
		}

		public virtual void SendImmediate(byte b, bool mute = false)
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

		public GPoint MachinePosition
		{ get { return mMPos; } }

		public GPoint WorkPosition //WCO = MPos - WPos
		{ get { return mMPos - mWCO; } }

		public GPoint WorkingOffset
		{ get { return mWCO; } }

		public int Executed
		{ get { return mSent.Count; } }

		public List<IGrblRow> SentCommand(int index, int count)
		{
			index = Math.Min(index, mSent.Count - 1);       //force index to be in range
			count = Math.Min(count, mSent.Count - index);   //force count to be in range

			if (index >= 0 && count > 0)
				return mSent.GetRange(index, count);
			else
				return new System.Collections.Generic.List<IGrblRow>();
		}
		#endregion

		#region Grbl Version Support

		public bool SupportRTO
		{ get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

		public virtual bool SupportTrueJogging
		{ get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

		public bool SupportCSV
		{ get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

		public bool SupportOverride
		{ get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

		public bool SupportLaserMode
		{ get { return GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1); } }

		#endregion

		public bool JogEnabled
		{
			get
			{
				if (SupportTrueJogging)
					return IsOpen && (MachineStatus == MacStatus.Idle || MachineStatus == MacStatus.Jog);
				else
					return IsOpen && (MachineStatus == MacStatus.Idle || MachineStatus == MacStatus.Run) && !InProgram;
			}
		}

		internal void EnqueueZJog(JogDirection dir, decimal step)
		{
			if (JogEnabled)
			{
				if (SupportTrueJogging)
					DoJogV11(dir, step);
				else
					EmulateJogV09(dir, step); //immediato
			}
		}

		public void BeginJog(JogDirection dir) //da chiamare su ButtonDown
		{
			if (JogEnabled)
			{
				if (SupportTrueJogging)
					DoJogV11(dir, JogStep);
				else
					EmulateJogV09(dir, JogStep);
			}
		}

		private void EmulateJogV09(JogDirection dir, decimal step) //emulate jog using plane G-Code
		{
			if (dir == JogDirection.Home)
			{
				EnqueueCommand(new GrblCommand(string.Format("G90")));
				EnqueueCommand(new GrblCommand(string.Format("G0X0Y0F{0}", JogSpeed)));
			}
			else
			{
				string cmd = "G0";

				if (dir == JogDirection.NE || dir == JogDirection.E || dir == JogDirection.SE)
					cmd += $"X{step.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
				if (dir == JogDirection.NW || dir == JogDirection.W || dir == JogDirection.SW)
					cmd += $"X-{step.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
				if (dir == JogDirection.NW || dir == JogDirection.N || dir == JogDirection.NE)
					cmd += $"Y{step.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
				if (dir == JogDirection.SW || dir == JogDirection.S || dir == JogDirection.SE)
					cmd += $"Y-{step.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
				if (dir == JogDirection.Zdown)
					cmd += $"Z-{step.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
				if (dir == JogDirection.Zup)
					cmd += $"Z{step.ToString("0.0", NumberFormatInfo.InvariantInfo)}";

				cmd += $"F{JogSpeed}";

				EnqueueCommand(new GrblCommand("G91"));
				EnqueueCommand(new GrblCommand(cmd));
				EnqueueCommand(new GrblCommand("G90"));
			}
		}

		private void DoJogV11(JogDirection dir, decimal step)
		{
			if (ContinuosJogEnabled && dir != JogDirection.Zdown && dir != JogDirection.Zup) //se C.J. e non Z => prenotato
			{
				mPrenotedJogDirection = dir;
				//lo step è quello configurato
			}
			else //non è CJ o non è Z => immediate enqueue jog command
			{
				mPrenotedJogDirection = JogDirection.None;

				if (dir == JogDirection.Home)
					EnqueueCommand(new GrblCommand(string.Format("$J=G90X0Y0F{0}", JogSpeed)));
				else
					EnqueueCommand(GetRelativeJogCommandv11(dir, step));
			}
		}

		private GrblCommand GetRelativeJogCommandv11(JogDirection dir, decimal step)
		{
			string cmd = "$J=G91";
			if (dir == JogDirection.NE || dir == JogDirection.E || dir == JogDirection.SE)
				cmd += $"X{step.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
			if (dir == JogDirection.NW || dir == JogDirection.W || dir == JogDirection.SW)
				cmd += $"X-{step.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
			if (dir == JogDirection.NW || dir == JogDirection.N || dir == JogDirection.NE)
				cmd += $"Y{step.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
			if (dir == JogDirection.SW || dir == JogDirection.S || dir == JogDirection.SE)
				cmd += $"Y-{step.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
			if (dir == JogDirection.Zdown)
				cmd += $"Z-{step.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
			if (dir == JogDirection.Zup)
				cmd += $"Z{step.ToString("0.0", NumberFormatInfo.InvariantInfo)}";

			cmd += $"F{JogSpeed}";
			return new GrblCommand(cmd);
		}

		private GrblCommand GetContinuosJogCommandv11(JogDirection dir)
		{
			string cmd = "$J=G53";
			if (dir == JogDirection.NE || dir == JogDirection.E || dir == JogDirection.SE)
				cmd += $"X{Configuration.TableWidth.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
			if (dir == JogDirection.NW || dir == JogDirection.W || dir == JogDirection.SW)
				cmd += $"X{0.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
			if (dir == JogDirection.NW || dir == JogDirection.N || dir == JogDirection.NE)
				cmd += $"Y{Configuration.TableHeight.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
			if (dir == JogDirection.SW || dir == JogDirection.S || dir == JogDirection.SE)
				cmd += $"Y{0.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
			cmd += $"F{JogSpeed}";
			return new GrblCommand(cmd);
		}

		public void EndJogV11() //da chiamare su ButtonUp
		{
			mPrenotedJogDirection = JogDirection.Abort;
		}

		private void PushJogCommand()
		{
			if (SupportTrueJogging && mPrenotedJogDirection != JogDirection.None && mPending.Count == 0)
			{
				if (mPrenotedJogDirection == JogDirection.Abort)
				{
					if (ContinuosJogEnabled)
						SendImmediate(0x85);
				}
				else if (mPrenotedJogDirection == JogDirection.Home)
				{
					EnqueueCommand(new GrblCommand(string.Format("$J=G90X0Y0F{0}", JogSpeed)));
				}
				else
				{
					if (ContinuosJogEnabled)
						EnqueueCommand(GetContinuosJogCommandv11(mPrenotedJogDirection));
					else
						EnqueueCommand(GetRelativeJogCommandv11(mPrenotedJogDirection, JogStep));
				}

				mPrenotedJogDirection = JogDirection.None;
			}
		}


		private void StartTX()
		{
			lock (this)
			{
				// No soft reset when opening COM port for smoothieware
				if (Type != Firmware.Smoothie)
					InternalReset(Settings.GetObject("Reset Grbl On Connect", true));

				InitializeBoard();
				QueryTimer.Start();
			}
		}

		protected virtual void InitializeBoard()
		{
			QueryPosition();
		}

		protected void ThreadTX()
		{
			lock (this)
			{
				try
				{
					if (MachineStatus == MacStatus.Connecting && Tools.HiResTimer.TotalMilliseconds - connectStart > 10000)
						OnConnectTimeout();

					if (!TX.MustExitTH())
					{
						PushJogCommand();

						if (CanSend())
							SendLine();

						ManageCoolingCycles();
					}

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

		// Override by Marlin
		protected virtual void DetectHang()
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
				{
					SetIssue(DetectedIssue.StopResponding);
					SoundEvent.PlaySound(SoundEvent.EventId.Fatal);
				}
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
							if (IsCommandReplyMessage(rline))
								ManageCommandResponse(rline);
							else if (IsRealtimeStatusMessage(rline))
								ManageRealTimeStatus(rline);
							else if (IsWelcomeMessage(rline))
								ManageWelcomeMessage(rline);
							else if (IsOrturWelcomeMessage(rline))
								ManageOrturWelcomeMessage(rline);
							else if (IsOrturVersionInfo(rline))
								ManageOrturVersionInfo(rline);
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

		private bool IsCommandReplyMessage(string rline)
		{
			return rline.ToLower().StartsWith("ok") || rline.ToLower().StartsWith("error");
		}

		private bool IsWelcomeMessage(string rline)
		{
			return rline.StartsWith("Grbl ");
		}

		private bool IsOrturWelcomeMessage(string rline)
		{
			return rline.StartsWith("Ortur ");
		}

		private bool IsOrturVersionInfo(string rline)
		{
			return rline.StartsWith("OLF");
		}

		// Return true if message received start with < and finish by >
		// Overrided by Marlin
		protected virtual bool IsRealtimeStatusMessage(string rline)
		{
			return rline.StartsWith("<") && rline.EndsWith(">");
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

		private void ManageWelcomeMessage(string rline)
		{
			//Grbl vX.Xx ['$' for help]
			try
			{
				int maj = int.Parse(rline.Substring(5, 1));
				int min = int.Parse(rline.Substring(7, 1));
				char build = rline.Substring(8, 1).ToCharArray()[0];
				GrblVersion = new GrblVersionInfo(maj, min, build, mOrturWelcomeSeen, mOrturVersionSeen);

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

		private void ManageOrturWelcomeMessage(string rline)
		{
			try
			{
				mOrturWelcomeSeen = rline;
				mOrturWelcomeSeen = mOrturWelcomeSeen.Replace("Ready", "");
				mOrturWelcomeSeen = mOrturWelcomeSeen.Replace("!", "");
				mOrturWelcomeSeen = mOrturWelcomeSeen.Trim();
				Logger.LogMessage("OrturInfo", "Detected {0}", mOrturWelcomeSeen);
			}
			catch (Exception ex)
			{
				Logger.LogMessage("OrturInfo", "Ex on [{0}] message", rline);
				Logger.LogException("OrturInfo", ex);
			}
			mSentPtr.Add(new GrblMessage(rline, false));
		}

		private void ManageOrturVersionInfo(string rline)
		{
			try
			{
				mOrturVersionSeen = rline;
				mOrturVersionSeen = mOrturVersionSeen.Replace("OLF", "");
				mOrturVersionSeen = mOrturVersionSeen.Trim('.');
				mOrturVersionSeen = mOrturVersionSeen.Trim();
				Logger.LogMessage("OrturInfo", "Detected OLF {0}", mOrturVersionSeen);
			}
			catch (Exception ex)
			{
				Logger.LogMessage("OrturInfo", "Ex on [{0}] message", rline);
				Logger.LogException("OrturInfo", ex);
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
				mCurOvLinear = mCurOvRapids = mCurOvPower = 100;
				mTarOvLinear = mTarOvRapids = mTarOvPower = 100;
			}
			RiseOverrideChanged();
		}

		private void DetectUnexpectedReset()
		{
			if (mTP.LastIssue == DetectedIssue.Unknown && MachineStatus == MacStatus.Run && InProgram)
				SetIssue(DetectedIssue.UnexpectedReset);
		}

		private GrblVersionInfo StatusReportVersion(string rline)
		{
			//if version is known -> return version
			if (GrblVersion != null)
				return GrblVersion;

			//else guess from rline
			//the check of Pin: is due to compatibility with 1.0c https://github.com/arkypita/LaserGRBL/issues/317
			if (rline.Contains("|") && !rline.Contains("Pin:"))
				return new GrblVersionInfo(1, 1);
			else if (rline.Contains("|") && rline.Contains("Pin:"))
				return new GrblVersionInfo(1, 0, 'c');
			else
				return new GrblVersionInfo(0, 9);
		}

		protected virtual void ManageRealTimeStatus(string rline)
		{
			try
			{
				debugLastStatusDelay.Start();

				rline = rline.Substring(1, rline.Length - 2);

				GrblVersionInfo rversion = StatusReportVersion(rline);
				if (rversion > new GrblVersionInfo(1, 1))
				{
					//grbl > 1.1 - https://github.com/gnea/grbl/wiki/Grbl-v1.1-Interface#real-time-status-reports
					string[] arr = rline.Split("|".ToCharArray());

					ParseMachineStatus(arr[0]);

					for (int i = 1; i < arr.Length; i++)
					{
						if (arr[i].StartsWith("Ov:"))
							ParseOverrides(arr[i]);
						else if (arr[i].StartsWith("Bf:"))
							ParseBf(arr[i]);
						else if (arr[i].StartsWith("WPos:"))
							ParseWPos(arr[i]);
						else if (arr[i].StartsWith("MPos:"))
							ParseMPos(arr[i]);
						else if (arr[i].StartsWith("WCO:"))
							ParseWCO(arr[i]);
						else if (arr[i].StartsWith("FS:"))
							ParseFS(arr[i]);
						else if (arr[i].StartsWith("F:"))
							ParseF(arr[i]);
					}
				}
				else //<Idle,MPos:0.000,0.000,0.000,WPos:0.000,0.000,0.000>
				{
					string[] arr = rline.Split(",".ToCharArray());

					if (arr.Length > 0)
						ParseMachineStatus(arr[0]);
					if (arr.Length > 3)
						SetMPosition(new GPoint(float.Parse(arr[1].Substring(5, arr[1].Length - 5), System.Globalization.NumberFormatInfo.InvariantInfo), float.Parse(arr[2], System.Globalization.NumberFormatInfo.InvariantInfo), float.Parse(arr[3], System.Globalization.NumberFormatInfo.InvariantInfo)));
					if (arr.Length > 6)
						ComputeWCO(new GPoint(float.Parse(arr[4].Substring(5, arr[4].Length - 5), System.Globalization.NumberFormatInfo.InvariantInfo), float.Parse(arr[5], System.Globalization.NumberFormatInfo.InvariantInfo), float.Parse(arr[6], System.Globalization.NumberFormatInfo.InvariantInfo)));
				}
			}
			catch (Exception ex)
			{
				Logger.LogMessage("RealTimeStatus", "Ex on [{0}] message", rline);
				Logger.LogException("RealTimeStatus", ex);
			}
		}

		private void ComputeWCO(GPoint wpos) //WCO = MPos - WPos
		{
			SetWCO(mMPos - wpos);
		}

		private void ParseWCO(string p)
		{
			string wco = p.Substring(4, p.Length - 4);
			string[] xyz = wco.Split(",".ToCharArray());
			SetWCO(new GPoint(ParseFloat(xyz[0]), ParseFloat(xyz[1]), ParseFloat(xyz[2])));
		}

		private void ParseWPos(string p)
		{
			string wpos = p.Substring(5, p.Length - 5);
			string[] xyz = wpos.Split(",".ToCharArray());
			SetMPosition(mWCO + new GPoint(ParseFloat(xyz[0]), ParseFloat(xyz[1]), ParseFloat(xyz[2])));
		}

		private void ParseMPos(string p)
		{
			string mpos = p.Substring(5, p.Length - 5);
			string[] xyz = mpos.Split(",".ToCharArray());
			SetMPosition(new GPoint(ParseFloat(xyz[0]), ParseFloat(xyz[1]), ParseFloat(xyz[2])));
		}

		protected static float ParseFloat(string value)
		{
			return float.Parse(value, NumberFormatInfo.InvariantInfo);
		}

		private void ParseBf(string p)
		{
			string bf = p.Substring(3, p.Length - 3);
			string[] ab = bf.Split(",".ToCharArray());

			mGrblBlocks = int.Parse(ab[0]);
			mGrblBuffer = int.Parse(ab[1]);

			if (stuckBufferCounter > 10 && mPending.Count > 0)
				ManageCommandResponse("ok");
			if (mGrblBuffer == BUFFER_SIZE && mPending.Count > 0)
				stuckBufferCounter++;
			if ((mGrblBuffer != BUFFER_SIZE) || (mPending.Count == 0 && mGrblBuffer == BUFFER_SIZE))
				stuckBufferCounter = 0;

			EnlargeBuffer(mGrblBuffer);
		}

		private void EnlargeBuffer(int mGrblBuffer)
		{
			if (BUFFER_SIZE == 127) //act only to change default value at first event, do not re-act without a new connect
			{
				if (mGrblBuffer == 128) //Grbl v1.1 with enabled buffer report
					BUFFER_SIZE = 128;
				else if (mGrblBuffer == 255) //Grbl-Mega fixed
					BUFFER_SIZE = 255;
				else if (mGrblBuffer == 256) //Grbl-Mega
					BUFFER_SIZE = 256;
				else if (mGrblBuffer == 10240) //Grbl-LPC
					BUFFER_SIZE = 10240;
			}
		}

		private void ParseFS(string p)
		{
			string sfs = p.Substring(3, p.Length - 3);
			string[] fs = sfs.Split(",".ToCharArray());
			SetFS(ParseFloat(fs[0]), ParseFloat(fs[1]));
		}

		protected virtual void ParseF(string p)
		{
			string f = p.Substring(2, p.Length - 2);
			SetFS(ParseFloat(f), 0);
		}

		protected void SetFS(float f, float s)
		{
			mCurF = f;
			mCurS = s;
		}

		protected void SetMPosition(GPoint pos)
		{
			if (pos != mMPos)
			{
				mMPos = pos;
				debugLastMoveDelay.Start();
			}
		}

		private void SetWCO(GPoint wco)
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
					GrblCommand pending = mPending.Peek();  //necessario fare peek
					pending.SetResult(rline, SupportCSV);   //assegnare lo stato
					mPending.Dequeue();                     //solo alla fine rimuoverlo dalla lista (per write config che si aspetta che lo stato sia noto non appena la coda si svuota)

					mBuffer -= pending.SerialData.Length;

					if (mTP.InProgram && pending.RepeatCount == 0) //solo se non è una ripetizione aggiorna il tempo
						mTP.JobExecuted(pending.TimeOffset);

					if (mTP.InProgram && pending.Status == CommandStatus.ResponseBad)
						mTP.JobError(); //incrementa il contatore

					if (pending.IsWriteEEPROM && pending.Status == CommandStatus.ResponseGood)
						Configuration.AddOrUpdate(pending.GetMessage());

					//ripeti errori programma && non ho una coda (magari mi sto allineando per cambio conf buff/sync) && ho un errore && non l'ho già ripetuto troppe volte
					if (InProgram && CurrentStreamingMode == StreamingMode.RepeatOnError && mPending.Count == 0 && pending.Status == CommandStatus.ResponseBad && pending.RepeatCount < 3) //il comando eseguito ha dato errore
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

		protected static char[] trimarray = new char[] { '\r', '\n', ' ' };
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
			if (mTarOvLinear == 100 && mCurOvLinear != 100) //devo fare un reset
				SendImmediate(144);
			else if (mTarOvLinear - mCurOvLinear >= 10) //devo fare un bigstep +
				SendImmediate(145);
			else if (mCurOvLinear - mTarOvLinear >= 10) //devo fare un bigstep -
				SendImmediate(146);
			else if (mTarOvLinear - mCurOvLinear >= 1) //devo fare uno smallstep +
				SendImmediate(147);
			else if (mCurOvLinear - mTarOvLinear >= 1) //devo fare uno smallstep -
				SendImmediate(148);

			if (mTarOvPower == 100 && mCurOvPower != 100) //devo fare un reset
				SendImmediate(153);
			else if (mTarOvPower - mCurOvPower >= 10) //devo fare un bigstep +
				SendImmediate(154);
			else if (mCurOvPower - mTarOvPower >= 10) //devo fare un bigstep -
				SendImmediate(155);
			else if (mTarOvPower - mCurOvPower >= 1) //devo fare uno smallstep +
				SendImmediate(156);
			else if (mCurOvPower - mTarOvPower >= 1) //devo fare uno smallstep -
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
			bool notify = (feed != mCurOvLinear || rapids != mCurOvRapids || spindle != mCurOvPower);
			mCurOvLinear = feed;
			mCurOvRapids = rapids;
			mCurOvPower = spindle;

			if (notify)
				RiseOverrideChanged();
		}

		public int OverrideG1
		{ get { return mCurOvLinear; } }

		public int OverrideG0
		{ get { return mCurOvRapids; } }

		public int OverrideS
		{ get { return mCurOvPower; } }

		public int TOverrideG1
		{
			get { return mTarOvLinear; }
			set { mTarOvLinear = value; }
		}

		public int TOverrideG0
		{
			get { return mTarOvRapids; }
			set { mTarOvRapids = value; }
		}

		public int TOverrideS
		{
			get { return mTarOvPower; }
			set { mTarOvPower = value; }
		}

		protected virtual void ParseMachineStatus(string data)
		{
			MacStatus var = MacStatus.Disconnected;

			if (data.Contains(":"))
				data = data.Substring(0, data.IndexOf(':'));

			try { var = (MacStatus)Enum.Parse(typeof(MacStatus), data); }
			catch (Exception ex) { Logger.LogException("ParseMachineStatus", ex); }

			if (InProgram && var == MacStatus.Idle) //bugfix for grbl sending Idle on G4
				var = MacStatus.Run;

			if (var == MacStatus.Hold && !mHoldByUserRequest)
				var = MacStatus.Cooling;

			SetStatus(var);
		}

		// Used by Marlin to update status to Idle (As Marlin has no immediate message)
		protected virtual void ForceStatusIdle() { }
		private void OnProgramEnd()
		{
			if (mTP.JobEnd() && mLoopCount > 1 && mMachineStatus != MacStatus.Check)
			{
				Logger.LogMessage("CycleEnd", "Cycle Executed: {0} lines, {1} errors, {2}", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(mTP.TotalJobTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second, ",", true));
				mSentPtr.Add(new GrblMessage(string.Format("[{0} lines, {1} errors, {2}]", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(mTP.TotalJobTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second, ",", true)), false));

				LoopCount--;
				RunProgramFromStart(false, false, true);
			}
			else
			{
				Logger.LogMessage("ProgramEnd", "Job Executed: {0} lines, {1} errors, {2}", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(mTP.TotalJobTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second, ",", true));
				mSentPtr.Add(new GrblMessage(string.Format("[{0} lines, {1} errors, {2}]", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(mTP.TotalJobTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second, ",", true)), false));

				Logger.LogMessage("EnqueueProgram", "Push Footer");
				ExecuteCustombutton(Settings.GetObject("GCode.CustomFooter", Constants.GCODE_STD_FOOTER));

				SoundEvent.PlaySound(SoundEvent.EventId.Success);

				ForceStatusIdle();
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
				string lastFile = Settings.GetObject<string>("Core.LastOpenFile", null);
				return CanLoadNewFile && lastFile != null && System.IO.File.Exists(lastFile);
			}
		}

		public bool CanLoadNewFile
		{ get { return !InProgram; } }

		public bool CanSendFile
		{ get { return IsOpen && HasProgram && IdleOrCheck; } }

		public bool CanAbortProgram
		{ get { return IsOpen && HasProgram && (MachineStatus == MacStatus.Run || MachineStatus == MacStatus.Hold || MachineStatus == MacStatus.Cooling); } }

		public bool CanImportExport
		{ get { return IsOpen && MachineStatus == MacStatus.Idle; } }

		public bool CanResetGrbl
		{ get { return IsOpen && MachineStatus != MacStatus.Disconnected; } }

		public bool CanSendManualCommand
		{ get { return IsOpen && MachineStatus != MacStatus.Disconnected && !InProgram; } }

		public bool CanDoHoming
		{ get { return IsOpen && (MachineStatus == MacStatus.Idle || MachineStatus == MacStatus.Alarm) && Configuration.HomingEnabled; } }

		public bool CanDoZeroing
		{ get { return IsOpen && MachineStatus == MacStatus.Idle && WorkPosition != GPoint.Zero; } }

		public bool CanUnlock
		{ get { return IsOpen && (MachineStatus == MacStatus.Idle || MachineStatus == MacStatus.Alarm); } }

		public bool CanFeedHold
		{ get { return IsOpen && MachineStatus == MacStatus.Run; } }

		public bool CanResumeHold
		{ get { return IsOpen && (MachineStatus == MacStatus.Door || MachineStatus == MacStatus.Hold || MachineStatus == MacStatus.Cooling); } }

		public decimal LoopCount
		{ get { return mLoopCount; } set { mLoopCount = value; if (OnLoopCountChange != null) OnLoopCountChange(mLoopCount); } }

		private ThreadingMode CurrentThreadingMode
		{ get { return Settings.GetObject("Threading Mode", ThreadingMode.UltraFast); } }

		public virtual StreamingMode CurrentStreamingMode
		{ get { return Settings.GetObject("Streaming Mode", StreamingMode.Buffered); } }

		private bool IdleOrCheck
		{ get { return MachineStatus == MacStatus.Idle || MachineStatus == MacStatus.Check; } }

		public bool AutoCooling
		{ get { return Settings.GetObject("AutoCooling", false); } }

		public TimeSpan AutoCoolingOn
		{ get { return Settings.GetObject("AutoCooling TOn", TimeSpan.FromMinutes(10)); } }

		public TimeSpan AutoCoolingOff
		{ get { return Settings.GetObject("AutoCooling TOff", TimeSpan.FromMinutes(1)); } }

		private void ManageCoolingCycles()
		{
			if (AutoCooling && InProgram && !mHoldByUserRequest)
				NowCooling = ((ProgramTime.Ticks % (AutoCoolingOn + AutoCoolingOff).Ticks) > AutoCoolingOn.Ticks);
		}

		protected bool mHoldByUserRequest = false;
		private bool mNowCooling = false;
		private bool NowCooling
		{
			set
			{
				if (mNowCooling != value)
				{
					if (value)
						StartCooling();
					else
						ResumeCooling();

					mNowCooling = value;
				}
			}
		}

		private void StartCooling()
		{
			if (SupportLaserMode && Configuration.LaserMode)
			{
				FeedHold(true);
			}
			else //TODO: emulate pause by pushing laser off and sleep into stream
			{

			}
		}

		private void ResumeCooling()
		{
			if (SupportLaserMode && Configuration.LaserMode)
			{
				CycleStartResume(true);
			}
		}

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

		public static string ExePath
		{
			get
			{
				return AppDomain.CurrentDomain.BaseDirectory;
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
				if (!(InProgram && System.Windows.Forms.MessageBox.Show(Strings.DisconnectAnyway, Strings.WarnMessageBoxHeader, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning, System.Windows.Forms.MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
					CloseCom(true);
			}
		}

		internal void HelpOnLine()
		{ System.Diagnostics.Process.Start(@"https://lasergrbl.com/usage/"); }

		internal void GrblHoming()
		{ if (CanDoHoming) EnqueueCommand(new GrblCommand("$H")); }

		internal void GrblUnlock()
		{ if (CanUnlock) EnqueueCommand(new GrblCommand("$X")); }

		internal void SetNewZero()
		{ if (CanDoZeroing) EnqueueCommand(new GrblCommand("G92 X0 Y0 Z0")); }

		public int JogSpeed { get; set; }
		public decimal JogStep { get; set; }

		public bool ContinuosJogEnabled { get { return Settings.GetObject("Enable Continuous Jog", false); } }

		public bool SuspendHK { get; set; }

		public HotKeysManager HotKeys { get { return mHotKeyManager; } }

		internal void WriteHotkeys(List<HotKeysManager.HotKey> mLocalList)
		{
			mHotKeyManager.Clear();
			mHotKeyManager.AddRange(mLocalList);
			Settings.SetObject("Hotkey Setup", mHotKeyManager);
			Settings.Save();
		}

		//internal void HKCustomButton(int index)
		//{
		//	CustomButton cb = CustomButtons.GetButton(index);
		//	if (cb != null && cb.EnabledNow(this))
		//		ExecuteCustombutton(cb.GCode);
		//}

		static System.Text.RegularExpressions.Regex bracketsRegEx = new System.Text.RegularExpressions.Regex(@"\[(?:[^]]+)\]");
		internal void ExecuteCustombutton(string buttoncode)
		{
			buttoncode = buttoncode.Trim();
			string[] arr = buttoncode.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			foreach (string str in arr)
			{
				if (str.Trim().Length > 0)
				{
					string tosend = bracketsRegEx.Replace(str, new System.Text.RegularExpressions.MatchEvaluator(EvaluateCB)).Trim();

					if (IsImmediate(tosend))
						SendImmediate(GetImmediate(tosend));
					else
						EnqueueCommand(new GrblCommand(tosend));
				}
			}
		}

		bool IsImmediate(string code)
		{
			if (code.ToLower() == "ctrl-x")
				return true;
			if (code == "?")
				return true;
			if (code == "!")
				return true;
			if (code == "~")
				return true;

			if (code.ToLower().StartsWith("0x"))
			{
				try
				{
					byte value = Convert.ToByte(code, 16);
					return true;
				}
				catch { return false; }
			}

			return false;
		}

		byte GetImmediate(string code)
		{
			if (code.ToLower() == "ctrl-x")
				return 24;
			if (code == "?")
				return 63;
			if (code == "!")
				return 33;
			if (code == "~")
				return 126;

			byte value = Convert.ToByte(code, 16);
			return value;
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
				exp.AddSetVariable("WCO.X", WorkingOffset.X);
				exp.AddSetVariable("WCO.Y", WorkingOffset.Y);
				exp.AddSetVariable("WCO.Z", WorkingOffset.Z);
				exp.AddSetVariable("MPos.X", MachinePosition.X);
				exp.AddSetVariable("MPos.Y", MachinePosition.Y);
				exp.AddSetVariable("MPos.Z", MachinePosition.Z);
				exp.AddSetVariable("WPos.X", MachinePosition.X);
				exp.AddSetVariable("WPos.Y", MachinePosition.Y);
				exp.AddSetVariable("WPos.Z", MachinePosition.Z);
				double dval = exp.EvaluateD();
				return m.Result(FormatNumber((decimal)dval));
			}
			catch { return m.Value; }
		}

		static string FormatNumber(decimal value)
		{ return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.000}", value); }


		public float CurrentF { get { return mCurF; } }
		public float CurrentS { get { return mCurS; } }

		private static IEnumerable<GrblCommand> StringToGCode(string input)
		{
			if (string.IsNullOrEmpty(input))
				yield break;

			using (System.IO.StringReader reader = new System.IO.StringReader(input))
			{
				string line;
				while (!string.IsNullOrEmpty(line = reader.ReadLine()))
					yield return new GrblCommand(line);
			}
		}

		public virtual bool UIShowGrblConfig => true;
		public virtual bool UIShowUnlockButtons => true;

		public bool IsOrturBoard { get => GrblVersion != null && GrblVersion.IsOrtur; }
	}

	public class TimeProjection
	{
		private TimeSpan mETarget;
		private TimeSpan mEProgress;

		private long mStart;        //Start Time
		private long mEnd;          //End Time
		private long mPauseBegin;   //Pause begin Time
		private long mCumulatedPause;

		private bool mInPause;
		private bool mCompleted;
		private bool mStarted;

		private int mTargetCount;
		private int mExecutedCount;
		private int mSentCount;
		private int mErrorCount;
		private int mContinueCorrection;

		DetectedIssue mLastIssue;
		private GPoint mLastKnownWCO;

		public GPoint LastKnownWCO
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
			mLastIssue = DetectedIssue.Unknown;
			mLastKnownWCO = GPoint.Zero;
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
					double real = TrueJobTime.TotalSeconds; //job time spent in execution
					double target = mETarget.TotalSeconds;  //total estimated
					double done = mEProgress.TotalSeconds;  //done of estimated

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

		public void JobStart(GrblFile file, Queue<GrblCommand> mQueuePtr)
		{
			if (!mStarted)
			{
				mETarget = file.EstimatedTime;
				mTargetCount = mQueuePtr.Count;
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
				mLastIssue = DetectedIssue.Unknown;
				mLastKnownWCO = GPoint.Zero;
			}
		}

		public void JobContinue(GrblFile file, int position, int added)
		{
			if (!mStarted)
			{
				if (mETarget == TimeSpan.Zero) mETarget = file.EstimatedTime;
				if (mTargetCount == 0) mTargetCount = file.Count;
				//mEProgress = TimeSpan.Zero;
				if (mStart == 0) mStart = Tools.HiResTimer.TotalMilliseconds;

				mPauseBegin = 0;
				mInPause = false;
				mCompleted = false;
				mStarted = true;
				mExecutedCount = position;
				mSentCount = position;
				mLastIssue = DetectedIssue.Unknown;
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
			{
				SoundEvent.PlaySound(SoundEvent.EventId.Warning);
				mErrorCount++;
			}
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

		public void JobIssue(DetectedIssue issue)
		{ mLastIssue = issue; }

		private long now
		{ get { return Tools.HiResTimer.TotalMilliseconds; } }

		public int ErrorCount
		{ get { return mErrorCount; } }

		public DetectedIssue LastIssue
		{ get { return mLastIssue; } }
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
