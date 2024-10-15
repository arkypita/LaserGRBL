//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using Sound;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;
using Tools;
using static LaserGRBL.GrblCore;
using static LaserGRBL.HotKeysManager;

namespace LaserGRBL
{
	public enum Firmware
	{ Grbl, Smoothie, Marlin, VigoWork }

	/// <summary>
	/// Description of CommandThread.
	/// </summary>
	public class GrblCore
	{
		//public static PSHelper.PSFile MaterialDB = PSHelper.PSFile.Load();
		public static PSHelper.MaterialDB MaterialDB = PSHelper.MaterialDB.Load();

		public static string GCODE_STD_HEADER = "G90 (use absolute coordinates)";
		public static string GCODE_STD_PASSES = ";(Uncomment if you want to sink Z axis)\r\n;G91 (use relative coordinates)\r\n;G0 Z-1 (sinks the Z axis, 1mm)\r\n;G90 (use absolute coordinates)";
		public static string GCODE_STD_FOOTER = "G0 X0 Y0 Z0 (move back to origin)";

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
			{ get { return new ThreadingMode(250, 1, 0, 0, 0, "UltraFast"); } }

			public static ThreadingMode Insane
			{ get { return new ThreadingMode(200, 1, 0, 0, 0, "Insane"); } }

			public override string ToString()
			{ return Name; }

			public override bool Equals(object obj)
			{ return obj != null && obj is ThreadingMode && ((ThreadingMode)obj).Name == Name; }

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
		}

		public enum DetectedIssue
		{
			Unknown = 0,
			ManualReset = -1,
			ManualDisconnect = -2,
			ManualAbort = -3,
			StopResponding = 1,
			//StopMoving = 2, 
			UnexpectedReset = 3,
			UnexpectedDisconnect = 4,
			MachineAlarm = 5,
		}

		public enum MacStatus
		{ Disconnected, Connecting, Idle, Run, Hold, Door, Home, Alarm, Check, Jog, Queue, Cooling, AutoHold, Tool } // "Tool" added in GrblHal

		public enum JogDirection
		{ Abort, Home, N, S, W, E, NW, NE, SW, SE, Zup, Zdown, Position }

		public enum StreamingMode
		{ Buffered, Synchronous, RepeatOnError }

		[Serializable]
		public class GrblVersionInfo : IComparable, ICloneable
		{
			int mMajor;
			int mMinor;
			char mBuild;
			bool mOrtur;
			bool mGrblHal;

			string mVendorInfo;
			string mVendorVersion;

			public GrblVersionInfo(int major, int minor, char build, string VendorInfo, string VendorVersion, bool IsHAL)
			{
				mMajor = major; mMinor = minor; mBuild = build;
				mVendorInfo = VendorInfo;
				mVendorVersion = VendorVersion;
				mOrtur = VendorInfo != null && (VendorInfo.Contains("Ortur") || VendorInfo.Contains("Aufero"));
				mGrblHal = IsHAL;
			}

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
				return v != null && this.mMajor == v.mMajor && this.mMinor == v.mMinor && this.mBuild == v.mBuild && this.mOrtur == v.mOrtur && this.mGrblHal == v.mGrblHal;
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
					hash = hash * 23 + mOrtur.GetHashCode();
					hash = hash * 23 + mGrblHal.GetHashCode();
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

			public int Major => mMajor;
			public int Minor => mMinor;
			public bool IsOrtur => mOrtur;
			public bool IsHAL => mGrblHal;
			public bool IsLuckyOrturWiFi => IsOrtur && mVendorInfo == "Ortur Laser Master 3";
			public string MachineName => mVendorInfo;

			public string VendorName
			{
				get
				{
					if (mVendorInfo != null && mVendorInfo.ToLower().Contains("ortur"))
						return "Ortur";
					else if (mVendorInfo != null && mVendorInfo.ToLower().Contains("Vigotec"))
						return "Vigotec";
					else if (mBuild == '#')
						return "Emulator";
					else
						return "Unknown";
				}
			}

			public int OrturFWVersionNumber
			{
				get
				{
					try { return int.Parse(mVendorVersion); }
					catch { return -1; }
				}
			}
		}

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
		public event Action<GrblCore> OnAutoSizeDrawing;
		public event Action<GrblCore> OnZoomInDrawing;
		public event Action<GrblCore> OnZoomOutDrawing;

		private System.Windows.Forms.Control syncro;
		protected ComWrapper.IComWrapper com;
		private GrblFile file;
		private System.Collections.Generic.Queue<GrblCommand> mQueue; //vera coda di quelli da mandare
		private GrblCommand mRetryQueue; //coda[1] di quelli in attesa di risposta
		private System.Collections.Generic.Queue<GrblCommand> mPending; //coda di quelli in attesa di risposta
		private System.Collections.Generic.List<IGrblRow> mSent; //lista di quelli mandati

		private System.Collections.Generic.Queue<GrblCommand> mQueuePtr; //puntatore a coda di quelli da mandare (normalmente punta a mQueue, salvo per import/export configurazione)
		private System.Collections.Generic.List<IGrblRow> mSentPtr; //puntatore a lista di quelli mandati (normalmente punta a mSent, salvo per import/export configurazione)

		private string mWelcomeSeen = null;
		private string mVendorInfoSeen = null;
		private string mVendorVersionSeen = null;
		protected int mUsedBuffer;

		private const int DEFAULT_BUFFER_SIZE = 127;
		private int mAutoBufferSize = DEFAULT_BUFFER_SIZE;
		private GPoint mMPos;
		private GPoint mWCO;
		private int mGrblBlocks = -1;
		private int mGrblBuffer = -1;
		private int mFailedConnection = 0;

		protected TimeProjection mTP = new TimeProjection();
		private MacStatus mMachineStatus = MacStatus.Disconnected;

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

		protected ElapsedFromEvent debugLastStatusDelay;
		protected ElapsedFromEvent debugLastMoveOrActivityDelay;

		private ThreadingMode mThreadingMode = ThreadingMode.Fast;
		private HotKeysManager mHotKeyManager;

		public UsageStats.UsageCounters UsageCounters;

		private string mDetectedIP = null;
		private bool mDoingSend = false;

        public RetainedSetting<bool> ShowLaserOffMovements { get; } = new RetainedSetting<bool>("ShowLaserOffMovements", true);
        public RetainedSetting<bool> ShowExecutedCommands { get; } = new RetainedSetting<bool>("ShowExecutedCommands", true);
		public RetainedSetting<bool> ShowPerformanceDiagnostic { get; } = new RetainedSetting<bool>("ShowPerformanceDiagnostic", false);
        public RetainedSetting<bool> ShowBoundingBox { get; } = new RetainedSetting<bool>("ShowBoundingBox", true);
        public RetainedSetting<bool> CrossCursor { get; } = new RetainedSetting<bool>("CrossCursor", true);
        public RetainedSetting<float> PreviewLineSize { get; } = new RetainedSetting<float>("PreviewLineSize", 1f);
        public RetainedSetting<bool> AutoSizeOnDrawing { get; } = new RetainedSetting<bool>("AutoSizeOnDrawing", true);

        public GrblCore(System.Windows.Forms.Control syncroObject, PreviewForm cbform, JogForm jogform)
		{
			if (Type != Firmware.Grbl) Logger.LogMessage("Program", "Load {0} core", Type);

			SetStatus(MacStatus.Disconnected);
			syncro = syncroObject;
			com = new ComWrapper.UsbSerial();

			debugLastStatusDelay = new ElapsedFromEvent();
			debugLastMoveOrActivityDelay = new ElapsedFromEvent();

			//with version 4.5.0 default ThreadingMode change from "UltraFast" to "Fast"
			if (!Settings.IsNewFile && Settings.PrevVersion < new Version(4, 5, 0))
			{
				ThreadingMode CurrentMode = Settings.GetObject("Threading Mode", ThreadingMode.Fast);
				if (Equals(CurrentMode, ThreadingMode.Insane) || Equals(CurrentMode, ThreadingMode.UltraFast))
					Settings.SetObject("Threading Mode", ThreadingMode.Fast);
			}

			mThreadingMode = Settings.GetObject("Threading Mode", ThreadingMode.Fast);

            QueryTimer = new Tools.PeriodicEventTimer(TimeSpan.FromMilliseconds(mThreadingMode.StatusQuery), false);
			TX = new Tools.ThreadObject(ThreadTX, 1, true, "Serial TX Thread", StartTX, System.Threading.ThreadPriority.Highest);
			RX = new Tools.ThreadObject(ThreadRX, 1, true, "Serial RX Thread", null, System.Threading.ThreadPriority.Highest);

			file = new GrblFile(0, 0, Configuration.TableWidth, Configuration.TableHeight);  //create a fake range to use with manual movements

			file.OnFileLoading += RiseOnFileLoading;
			file.OnFileLoaded += RiseOnFileLoaded;

			mQueue = new System.Collections.Generic.Queue<GrblCommand>();
			mPending = new System.Collections.Generic.Queue<GrblCommand>();
			mSent = new System.Collections.Generic.List<IGrblRow>();
			mUsedBuffer = 0;

			mSentPtr = mSent;
			mQueuePtr = mQueue;

			mCurOvLinear = mCurOvRapids = mCurOvPower = 100;
			mTarOvLinear = mTarOvRapids = mTarOvPower = 100;

			if (!Settings.ExistObject("Hotkey Setup")) Settings.SetObject("Hotkey Setup", new HotKeysManager());
			mHotKeyManager = Settings.GetObject<HotKeysManager>("Hotkey Setup", null);
			mHotKeyManager.Init(this, cbform, jogform);

			UsageCounters = new UsageStats.UsageCounters();

			if (GrblVersion != null)
				CSVD.LoadAppropriateCSV(GrblVersion); //load setting for last known version
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

		public static GrblConfST Configuration
		{
			get
			{
				if (Settings.ExistObject("Grbl Configuration"))
				{
					Settings.SetObject("Grbl Configuration ST", new GrblConfST(Settings.GetObject("Grbl Configuration", (GrblConf)null)));  //convert old format with only numbers
					Settings.DeleteObject("Grbl Configuration");                                                                            //delete old format
				}
				return Settings.GetObject("Grbl Configuration ST", new GrblConfST());
			}
			set
			{
				if (value.Count > 0/* && value.GrblVersion != null*/)
					Settings.SetObject("Grbl Configuration ST", value);
			}
		}

		protected void SetStatus(MacStatus newStatus)
		{
			lock (this)
			{
				LaserLifeHandler.ComputeLaserTime(mMachineStatus);

				if (mMachineStatus != newStatus)
				{
					MacStatus oldStatus = mMachineStatus;
					mMachineStatus = newStatus;

					Logger.LogMessage("SetStatus", "Machine status [{0}]", mMachineStatus);
					if (oldStatus == MacStatus.Connecting && newStatus == MacStatus.Disconnected)
						mFailedConnection++;
					if (oldStatus == MacStatus.Connecting && newStatus != MacStatus.Disconnected)
						mFailedConnection = 0;
					if (oldStatus == MacStatus.Connecting && newStatus != MacStatus.Disconnected)
						RefreshConfigOnConnect();
					if (oldStatus == MacStatus.Connecting && newStatus != MacStatus.Disconnected)
						SoundEvent.PlaySound(SoundEvent.EventId.Connect);
					if (newStatus == MacStatus.Disconnected)
						SoundEvent.PlaySound(SoundEvent.EventId.Disconnect);
					if (IsAnyHoldState(oldStatus) && !IsAnyHoldState(newStatus))
						mHoldByCoolingRequest = mHoldByUserRequest = false; //se sto uscendo da uno stato di hold per qualsiasi motivo (tipo un reset o altro) mi tolgo l'userHold e il coolingHold

					RiseMachineStatusChanged();

					if (mTP != null && mTP.InProgram)
					{
						if (InPause)
							mTP.JobPause();
						else
							mTP.JobResume();
					}

					if (newStatus == MacStatus.Disconnected)
						SetFS(0, 0);
				}
			}
		}

		private static bool IsAnyHoldState(MacStatus status)
		{
			return status == MacStatus.Hold || status == MacStatus.Cooling || status == MacStatus.AutoHold;
		}

		private void RefreshConfigOnConnect()
		{
			try { System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(RefreshConfigOnConnect)); }
			catch { }
		}

		internal virtual void SendHomingCommand()
		{
			EnqueueCommand(new GrblCommand("$H"));
		}

		internal virtual void SendUnlockCommand()
		{
			EnqueueCommand(new GrblCommand("$X"));
		}

		public GrblVersionInfo GrblVersion //attenzione! può essere null
		{
			get { return Settings.GetObject<GrblVersionInfo>("Last GrblVersion known", null); }
			set
			{
				if (GrblVersion != null)
					Logger.LogMessage("VersionInfo", "Detected Grbl v{0}", value);

				if (GrblVersion == null || !GrblVersion.Equals(value))
				{
					CSVD.LoadAppropriateCSV(value);
					Settings.SetObject("Last GrblVersion known", value);
				}
			}
		}

		protected void SetIssue(DetectedIssue issue)
		{
			mTP.JobIssue(issue);
			Logger.LogMessage("Issue detector", "{0} [{1},{2},{3}]", issue, FreeBuffer, GrblBuffer, GrblBlock);

			if (issue > 0) //negative numbers indicate issue caused by the user, so must not be report to UI
			{
				RiseIssueDetected(issue);
				Telegram.NotifyEvent(String.Format("<b>Job Issue</b>\n{0}", issue));
				SoundEvent.PlaySound(SoundEvent.EventId.Fatal);
			}
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
			mTP.Reset(true);

			if (OnFileLoaded != null)
				OnFileLoading?.Invoke(elapsed, filename);
		}

		void RiseOnFileLoaded(long elapsed, string filename)
		{
			mTP.Reset(true);

			if (OnFileLoaded != null)
				OnFileLoaded?.Invoke(elapsed, filename);
		}

		public GrblFile LoadedFile
		{ get { return file; } }

		public void ReOpenFile()
		{
			if (CanReOpenFile)
				OpenFile(Settings.GetObject<string>("Core.LastOpenFile", null));
		}

		public static readonly List<string> ImageExtensions = new List<string>(new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif" });
		public static readonly List<string> GCodeExtensions = new List<string>(new string[] { ".nc", ".cnc", ".tap", ".gcode", ".ngc" });
		public static readonly List<string> ProjectFileExtensions = new List<string>(new string[] { ".lps" });
		public void OpenFile(string filename = null, bool append = false)
		{
			if (!CanLoadNewFile) return;

			try
			{
				if (filename == null)
				{
					using (OpenFileDialog ofd = new OpenFileDialog())
					{
						//pre-select last file if exist
						string lastFN = Settings.GetObject<string>("Core.LastOpenFile", null);
						if (lastFN != null && System.IO.File.Exists(lastFN))
							ofd.FileName = lastFN;

						ofd.Filter = "Any supported file|*.nc;*.cnc;*.tap;*.gcode;*.ngc;*.bmp;*.png;*.jpg;*.jpeg;*.gif;*.svg;*.lps|GCODE Files|*.nc;*.cnc;*.tap;*.gcode;*.ngc|Raster Image|*.bmp;*.png;*.jpg;*.jpeg;*.gif|Vector Image (experimental)|*.svg|LaserGRBL Project|*.lps";
						ofd.CheckFileExists = true;
						ofd.Multiselect = false;
						ofd.RestoreDirectory = true;

						DialogResult dialogResult = DialogResult.Cancel;
						try
						{
							dialogResult = ofd.ShowDialog(FormsHelper.MainForm);
						}
						catch (System.Runtime.InteropServices.COMException)
						{
							ofd.AutoUpgradeEnabled = false;
							dialogResult = ofd.ShowDialog(FormsHelper.MainForm);
						}

						if (dialogResult == DialogResult.OK)
							filename = ofd.FileName;
					}
				}

				if (filename == null) return;

				Logger.LogMessage("OpenFile", "Open {0}", filename);
				Settings.SetObject("Core.LastOpenFile", filename);

				if (ImageExtensions.Contains(System.IO.Path.GetExtension(filename).ToLowerInvariant())) //import raster image
				{
					try
					{
						RasterConverter.RasterToLaserForm.CreateAndShowDialog(this, filename, append);
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
							SvgConverter.SvgToGCodeForm.CreateAndShowDialog(this, filename, append);
							UsageCounters.SvgFile++;
						}
						catch (Exception ex)
						{ Logger.LogException("SvgImport", ex); }
					}
					else if (mode == SvgConverter.SvgModeForm.Mode.Raster)
					{
						string bmpname = filename + ".png";
						string fcontent = System.IO.File.ReadAllText(filename);
						Svg.SvgDocument svg = Svg.SvgDocument.FromSvg<Svg.SvgDocument>(fcontent);
						svg.Ppi = 600;

						using (Bitmap bmp = svg.Draw())
						{
							bmp.SetResolution(600, 600);

							//codec options not supported in C# png encoder https://efundies.com/c-sharp-save-png/
							//quality always 100%

							//ImageCodecInfo codecinfo = GetEncoder(ImageFormat.Png);
							//EncoderParameters paramlist = new EncoderParameters(1);
							//paramlist.Param[0] = new EncoderParameter(Encoder.Quality, 30L); 


							if (System.IO.File.Exists(bmpname))
								System.IO.File.Delete(bmpname);

							bmp.Save(bmpname/*, codecinfo, paramlist*/);
						}

						try
						{
							RasterConverter.RasterToLaserForm.CreateAndShowDialog(this, bmpname, append);
							UsageCounters.RasterFile++;
							if (System.IO.File.Exists(bmpname))
								System.IO.File.Delete(bmpname);
						}
						catch (Exception ex)
						{ Logger.LogException("SvgBmpImport", ex); }
					}
				}
				else if (GCodeExtensions.Contains(System.IO.Path.GetExtension(filename).ToLowerInvariant()))  //load GCODE file
				{
					Cursor.Current = Cursors.WaitCursor;

					try
					{
						file.LoadFile(filename, append);
						UsageCounters.GCodeFile++;
					}
					catch (Exception ex)
					{ Logger.LogException("GCodeImport", ex); }

					Cursor.Current = Cursors.Default;
				}
				else if (ProjectFileExtensions.Contains(System.IO.Path.GetExtension(filename).ToLowerInvariant()))  //load LaserGRBL project
				{
					var project = Project.LoadProject(filename);

					for (var i = 0; i < project.Count; i++)
					{
						var settings = project[i];

						// Save image temporary
						var imageFilepath = $"{System.IO.Path.GetTempPath()}\\{settings["ImageName"]}";
						Project.SaveImage(settings["ImageBase64"].ToString(), imageFilepath);

						// Restore settings
						foreach (var setting in settings.Where(setting =>
							setting.Key != "ImageName" && setting.Key != "ImageBase64"))
							Settings.SetObject(setting.Key, setting.Value);

						// Open file
						Settings.SetObject("Core.LastOpenFile", imageFilepath);
						if (i == 0)
							ReOpenFile();
						else
							OpenFile(imageFilepath, true);

						// Delete temporary image file
						System.IO.File.Delete(imageFilepath);
					}
				}
				else
				{
					System.Windows.Forms.MessageBox.Show(Strings.UnsupportedFiletype, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

		public void SaveProgram(Form parent, bool header, bool footer, bool between, int cycles, bool useLFLineEndings)
		{
			if (HasProgram)
			{
				string filename = null;
				using (SaveFileDialog sfd = new SaveFileDialog())
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

					DialogResult rv = DialogResult.Cancel;
					try
					{
						rv = sfd.ShowDialog(parent);
					}
					catch (System.Runtime.InteropServices.COMException)
					{
						sfd.AutoUpgradeEnabled = false;
						rv = sfd.ShowDialog(parent);
					}

					if (rv == DialogResult.OK)
						filename = sfd.FileName;
				}

				if (filename != null)
					file.SaveGCODE(filename, header, footer, between, cycles, useLFLineEndings, this);
			}
		}

		public void SaveProject(Form parent)
		{
			if (HasProgram)
			{
				string filename = null;
				using (SaveFileDialog sfd = new SaveFileDialog())
				{
					string lastFN = Settings.GetObject<string>("Core.LastOpenFile", null);
					if (lastFN != null)
					{
						string fn = System.IO.Path.GetFileNameWithoutExtension(lastFN);
						string path = System.IO.Path.GetDirectoryName(lastFN);
						sfd.FileName = System.IO.Path.Combine(path, fn + ".lps");
					}

					sfd.Filter = "LaserGRBL Project|*.lps";
					sfd.AddExtension = true;
					sfd.RestoreDirectory = true;

					DialogResult rv = DialogResult.Cancel;
					try
					{
						rv = sfd.ShowDialog(parent);
					}
					catch (System.Runtime.InteropServices.COMException)
					{
						sfd.AutoUpgradeEnabled = false;
						rv = sfd.ShowDialog(parent);
					}

					if (rv == DialogResult.OK)
						filename = sfd.FileName;
				}

				if (filename != null)
					Project.StoreSettings(filename);
			}
		}

		private void RefreshConfigOnConnect(object state) //da usare per la chiamata asincrona
		{
			System.Threading.Thread.Sleep(500); //allow the machine to send all the startup messages before refreshing config and info

			try
			{
				RefreshConfig(RefreshCause.OnConnect);
			}
			catch (Exception ex) { Logger.LogMessage("Refresh Config", ex.Message); }

			try { RefreshMachineInfo(); }
			catch (Exception ex) { Logger.LogMessage("Refresh Config", ex.Message); }
		}

		public virtual void RefreshMachineInfo()
		{
			if (Settings.GetObject("Query MachineInfo ($I) at connect", true))
			{
				try
				{
					//GrblConf conf = new GrblConf(GrblVersion);
					GrblCommand cmd = new GrblCommand("$I");

					lock (this)
					{
						mSentPtr = new List<IGrblRow>(); //assign sent queue
						mQueuePtr = new Queue<GrblCommand>();
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
						int target = 2 + 1; //il +1 è il comando $I

						//finché ne devo ricevere ancora && l'ultima risposta è più recente di 500mS && non sono passati più di 5s totali
						while (mSentPtr.Count < target && Tools.HiResTimer.TotalMilliseconds - tLast < 500 && Tools.HiResTimer.TotalMilliseconds - tStart < 5000)
						{
							if (mSentPtr.Count != counter)
							{ tLast = Tools.HiResTimer.TotalMilliseconds; counter = mSentPtr.Count; }
							else
								System.Threading.Thread.Sleep(10);
						}

						foreach (IGrblRow row in mSentPtr) //read the reply
						{
							string rline = row.GetDecodedMessage();
							if (IsIVerMessage(rline))
								ManageVerMessage(rline);
							else if (IsIOptMessage(rline))
								ManageOptMessage(rline);
						}
					}
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

		private void ManageOptMessage(string rline)
		{
			try
			{
				rline = rline.Substring(5, rline.Length - 6);
				string[] arr = rline.Split(',');
				string letters = arr.Length > 0 ? arr[0] : null;
				int bbuffer = arr.Length > 1 ? int.Parse(arr[1]) : 0;
				int txbuffer = arr.Length > 2 ? int.Parse(arr[2]) : 0;

				if (txbuffer > DEFAULT_BUFFER_SIZE) //more then default buffer size
					EnlargeBuffer(txbuffer, true);

			}
			catch (Exception ex)
			{
				Logger.LogMessage("OptionsMessage", "Ex on [{0}] message", rline);
				Logger.LogException("OptionsMessage", ex);
			}
		}

		private void ManageVerMessage(string rline)
		{
			try
			{
				rline = rline.Substring(5, rline.Length - 6);
				//if (rline.ToLower().Contains("neje"))
				//    GrblVersion?.IsNeje = true;
			}
			catch (Exception ex)
			{
				Logger.LogMessage("VersionMessage", "Ex on [{0}] message", rline);
				Logger.LogException("VersionMessage", ex);
			}
		}

		public enum RefreshCause { OnConnect, OnDialog, OnRead, OnExport, OnImport, OnWriteBegin, OnWriteEnd }
		public virtual void RefreshConfig(RefreshCause cause)
		{
			Exception exc = null;
			if (CanReadWriteConfig)
			{
				try
				{
					//mSentPtr.Add(new GrblMessage(string.Format("Refresh conf [{0}]", cause), false));

					Logger.LogMessage("Refresh Config", "Refreshing grbl configuration [{0}]", cause);

					GrblConfST conf = new GrblConfST(GrblVersion);
					GrblCommand cmd = new GrblCommand("$$");

					lock (this)
					{
						mSentPtr = new List<IGrblRow>(); //assign sent queue
						mQueuePtr = new Queue<GrblCommand>();
						mQueuePtr.Enqueue(cmd);
					}

					PeriodicEventTimer WaitResponseTimeout = new PeriodicEventTimer(TimeSpan.FromSeconds(10), true);

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
						while (mSentPtr.Count < target && HiResTimer.TotalMilliseconds - tLast < 500 && Tools.HiResTimer.TotalMilliseconds - tStart < 5000)
						{
							if (mSentPtr.Count != counter)
							{ tLast = HiResTimer.TotalMilliseconds; counter = mSentPtr.Count; }
							else
								System.Threading.Thread.Sleep(10);
						}

						foreach (IGrblRow row in mSentPtr)
						{
							if (row is GrblMessage)
								conf.AddOrUpdate(((GrblMessage)row).GetNativeMessage());
						}

						Configuration = conf; //accept configuration

						if (Configuration.Count < Configuration.ExpectedCount)	//log but do not show error if some param is missing
							Logger.LogMessage("Refresh Config", "Wrong number of config param found! (Expected: {0} Found: {1})", Configuration.ExpectedCount, Configuration.Count);
						else
							Logger.LogMessage("Refresh Config", "Configuration successfully received! (Expected: {0} Found: {1})", Configuration.ExpectedCount, Configuration.Count);
					}
				}
				catch (Exception ex)
				{
					exc = ex;
					Logger.LogException("Refresh Config", ex);
					throw ex;
				}
				finally
				{
					lock (this)
					{
						mQueuePtr = mQueue;
						mSentPtr = mSent; //restore queue

						//mSentPtr.Add(new GrblMessage(string.Format("Refresh conf: [{0}] parameter found!", Configuration.Count), false));
						//if (exc != null) mSentPtr.Add(new GrblMessage(string.Format("Refresh conf error: [{0}]", exc.Message), false));
					}
				}
			}
			else
			{
				Logger.LogMessage("Refresh Config", "Cannot refresh config now [{0}] [Connected:{1} Program:{2} Status:{3}]", cause, IsConnected, InProgram, MachineStatus);
			}
		}



		public class ReadConfigCountException : Exception
		{
			public ReadConfigCountException(string message) : base(message)
			{
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
						rv += string.Format("{0} {1}\n", r.GetDecodedMessage(), r.GetResult(true, false));
					return rv.Trim();
				}
			}

			public List<IGrblRow> Errors
			{ get { return ErrorLines; } }
		}

		public string DetectedIP => mDetectedIP;

		internal void WriteWiFiConfig(string ssid, string password)
		{
			//throw new NotImplementedException();
			mDetectedIP = null;
			if (CanReadWriteConfig)
			{
				lock (this)
				{
					//mSentPtr = new System.Collections.Generic.List<IGrblRow>(); //assign sent queue
					//mQueuePtr = new System.Collections.Generic.Queue<GrblCommand>();

					mQueuePtr.Enqueue(new GrblCommand(string.Format("${0}={1}", 74, ssid), 0, true));
					mQueuePtr.Enqueue(new GrblCommand(string.Format("${0}={1}", 75, password), 0, true));
					mQueuePtr.Enqueue(new GrblCommand("$WRS"));
				}

				try
				{
					while (com.IsOpen && (mQueuePtr.Count > 0 || HasPendingCommands())) //resta in attesa del completamento della comunicazione
						;

					int errors = 0;
					foreach (IGrblRow row in mSentPtr)
					{
						if (row is GrblCommand)
							if (((GrblCommand)row).Status != GrblCommand.CommandStatus.ResponseGood)
								errors++;
					}

					//if (errors > 0)
					//	throw new WriteConfigException(mSentPtr);
				}
				catch (Exception ex)
				{
					//Logger.LogException("Write Config", ex);
					//throw (ex);
				}
				finally
				{
					//lock (this)
					//{
					//	//mQueuePtr = mQueue;
					//	//mSentPtr = mSent; //restore queue
					//}
				}
			}

		}

		public void WriteConfig(List<GrblConfST.GrblConfParam> config)
		{
			if (CanReadWriteConfig)
			{
				lock (this)
				{
					mSentPtr = new System.Collections.Generic.List<IGrblRow>(); //assign sent queue
					mQueuePtr = new System.Collections.Generic.Queue<GrblCommand>();

					foreach (GrblConfST.GrblConfParam p in config)
						mQueuePtr.Enqueue(new GrblCommand(string.Format("${0}={1}", p.Number, p.Value), 0, true));
				}

				try
				{
					while (com.IsOpen && (mQueuePtr.Count > 0 || HasPendingCommands())) //resta in attesa del completamento della comunicazione
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


		public void RunProgram(Form parent)
		{
			try
			{
				if (CanSendFile)
				{
					mDoingSend = true;

					if (mTP.Executed == 0 || mTP.Executed == mTP.Target) //mai iniziato oppure correttamente finito
						RunProgramFromStart(false, true);
					else
						UserWantToContinue(parent);
				}
			}
			catch { }
			finally { mDoingSend = false; }
		}

		public void AbortProgram()
		{
			if (CanAbortProgram)
			{
				try
				{
					Logger.LogMessage("ManualAbort", "Program aborted by user action!");

					SetIssue(DetectedIssue.ManualAbort);
					mTP.JobEnd(true);

					lock (this)
					{
						ClearQueue(mQueue); //flush the queue of item to send
						mQueue.Enqueue(new GrblCommand("M5")); //shut down laser
					}
				}
				catch (Exception ex)
				{
					Logger.LogException("Abort Program", ex);
				}

			}
		}

		public void RunProgramFromPosition(Form parent)
		{
			if (CanSendFile)
			{
				bool homing = false;
				int position = LaserGRBL.RunFromPositionForm.CreateAndShowDialog(parent, LoadedFile.Count, Configuration.HomingEnabled, out homing);

				if (position >= 0)
					ContinueProgramFromKnown(position, homing, false);
			}
		}

		private void UserWantToContinue(Form parent)
		{
			bool setwco = mWCO == GPoint.Zero && mTP.LastKnownWCO != GPoint.Zero;
			bool homing = MachinePosition == GPoint.Zero && mTP.LastIssue != DetectedIssue.ManualAbort && mTP.LastIssue != DetectedIssue.ManualReset; //potrebbe essere dovuto ad un hard reset -> posizione non affidabile
			int position = ResumeJobForm.CreateAndShowDialog(parent, mTP.Executed, mTP.Sent, mTP.Target, mTP.LastIssue, Configuration.HomingEnabled, homing, out homing, setwco, setwco, out setwco, mTP.LastKnownWCO);

			if (position == 0)
				RunProgramFromStart(homing);
			if (position > 0)
				ContinueProgramFromKnown(position, homing, setwco);
		}

		private void RunProgramFromStart(bool homing, bool first = false, bool pass = false)
		{
			//solo se non siamo tra le passate
			if (!pass && !SafetyCountdown.CanGo())
				return;

			lock (this)
			{
				ClearQueue(true);

				mTP.Reset(first);

				if (homing)
				{
					Logger.LogMessage("EnqueueProgram", "Push Homing ($H)");
					mQueuePtr.Enqueue(new GrblCommand("$H"));
				}

				if (first)
					OnJobBegin();

				if (pass)
					OnJobCycle();


				Logger.LogMessage("EnqueueProgram", "Push File, {0} lines", file.Count);
				foreach (GrblCommand cmd in file)
					mQueuePtr.Enqueue(cmd.Clone() as GrblCommand);

				mTP.JobStart(LoadedFile, mQueuePtr, first);
				Logger.LogMessage("EnqueueProgram", "Running program, {0} lines", file.Count);
			}
		}

		private void OnJobCycle()
		{
			Logger.LogMessage("EnqueueProgram", "Push Passes");
			ExecuteCustomCode(Settings.GetObject("GCode.CustomPasses", GrblCore.GCODE_STD_PASSES));
		}

		protected virtual void OnJobBegin()
		{
			Logger.LogMessage("EnqueueProgram", "Push Header");
			ExecuteCustomCode(Settings.GetObject("GCode.CustomHeader", GrblCore.GCODE_STD_HEADER));
		}

		protected virtual void OnJobEnd()
		{
			Logger.LogMessage("EnqueueProgram", "Push Footer");
			ExecuteCustomCode(Settings.GetObject("GCode.CustomFooter", GrblCore.GCODE_STD_FOOTER));
		}

		private void ContinueProgramFromKnown(int position, bool homing, bool setwco)
		{
			if (!SafetyCountdown.CanGo())
				return;

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
			else if (wraptype == ComWrapper.WrapperType.UsbSerial2 && (com == null || com.GetType() != typeof(ComWrapper.UsbSerial2)))
				com = new ComWrapper.UsbSerial2();
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
				mAutoBufferSize = DEFAULT_BUFFER_SIZE; //reset to default buffer size
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

				mUsedBuffer = 0;
				mTP.JobEnd(true);

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

		internal void Exiting()
		{
			try { CloseCom(true); }
			catch { }

			try { RX.Stop(); }
			catch { }

			try { TX.Stop(); }
			catch { }

			LaserLifeHandler.SaveNow();
		}

		public bool IsConnected => mMachineStatus != MacStatus.Disconnected && mMachineStatus != MacStatus.Connecting;

		#region Comandi immediati

		public void CycleStartResume(bool auto)
		{
			if (CanResumeHold)
			{
				mHoldByCoolingRequest = mHoldByUserRequest = false;
				SendImmediate(126);
			}
		}

		public void FeedHold(bool cooling)
		{
			if (CanFeedHold)
			{
				mHoldByUserRequest = !cooling;
				mHoldByCoolingRequest = cooling;
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
				mUsedBuffer = 0;
				mTP.JobEnd(true);
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

		public TimeSpan ProgramGlobalTime
		{ get { return mTP.TotalGlobalJobTime; } }

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
					return IsConnected && (MachineStatus == GrblCore.MacStatus.Idle || MachineStatus == GrblCore.MacStatus.Jog);
				else
					return IsConnected && (MachineStatus == GrblCore.MacStatus.Idle || MachineStatus == GrblCore.MacStatus.Run) && !InProgram;
			}
		}

		public void JogToPosition(PointF target, bool fast) => JogToPosition(target, fast ? 100000 : JogSpeed); //da chiamare su doppio click
		public void JogToPosition(PointF target, float speed)
		{
			target = LimitToBound(target); //if soft limit enabled -> crop to machine area

			if (!JogEnabled) //cannot jog now
				return;

			if (!SupportTrueJogging) //old firmware
				EnqueueJogV09(target, speed);
			else if (!ContinuosJogEnabled)	//continuous jog disabled
				EnqueueJogV11(target, speed);
			else
				ContinuousJog.ToPosition(target, speed);
		}

		public void ContinuousJogToPosition(PointF target, float speed) 
		{
			target = LimitToBound(target); //if soft limit enabled -> crop to machine area

			if (!JogEnabled) //cannot jog now
				return;

			if (!SupportTrueJogging)                                                            // old firmware
				return; //not supported by firmware

			ContinuousJog.ToPosition(target, speed);
		}

		public void JogToDirection(JogDirection dir, bool fast) => JogToDirection(dir, fast, JogStep);
		public void JogToDirection(JogDirection dir, bool fast, decimal step) => JogToDirection(dir, fast ? 100000 : JogSpeed, step);
		public void JogToDirection(JogDirection dir, float speed, decimal step) 
		{
			if (dir == JogDirection.Abort)
				throw new ArgumentException("Invalid option", "dir");
			if (dir == JogDirection.Position)
				throw new ArgumentException("Invalid option", "dir");

			if (!JogEnabled)																		// cannot jog now
				return;																					// ignore request
			
			if (!SupportTrueJogging)															// old firmware
				EnqueueJogV09(dir, step, speed);														// immediate enqueue old command
			else if (!ContinuosJogEnabled || dir == JogDirection.Zdown || dir == JogDirection.Zup)  // continuous jog disabled && Z movement
				EnqueueJogV11(dir, step, speed);                                                        // immediate enqueue new command
			else                                                                                    // continuoud jog enabled
				ContinuousJog.ToDirection(dir, speed);                                                    // assign jog target
		}

		public void ContinuousJogToDirection(JogDirection dir, float speed)
		{
			if (dir == JogDirection.Abort)
				throw new ArgumentException("Invalid option", "dir");
			if (dir == JogDirection.Position)
				throw new ArgumentException("Invalid option", "dir");

			if (!JogEnabled)                                                                        // cannot jog now
				return;

			if (!SupportTrueJogging)                                                            // old firmware
				return; //not supported by firmware

			ContinuousJog.ToDirection(dir, speed);
		}

		public void JogAbort() //da chiamare su ButtonUp
		{
			if (!SupportTrueJogging)																// old firmware
				;																						// abort not supported
			else if (!ContinuosJogEnabled)															// continuous jog disabled
				;                                                                                       // we can abort but we don't want
			else                                                                                    // continuoud jog enabled
				ContinuousJog.Abort();														               // assign jog target
		}

		public void ContinuousJogAbort() //da chiamare su ButtonUp
		{
			if (!SupportTrueJogging)                                                                // old firmware
				return;                                                                                       // abort not supported
			
			ContinuousJog.Abort();                                                                     // assign jog target
		}

		private PointF LimitToBound(PointF target)
		{
			if (Configuration.SoftLimit)
			{
				GPoint p = mWCO;
				PointF rv = new PointF(Math.Min(Math.Max(target.X, -mWCO.X), (float)Configuration.TableWidth - mWCO.X), Math.Min(Math.Max(target.Y, -mWCO.Y), (float)Configuration.TableHeight - mWCO.Y));
				return rv;
			}

			return target;
		}

		private void EnqueueJogV09(JogDirection dir, decimal step, float speed) //emulate jog using plane G-Code
		{
			if (dir == JogDirection.Home)
			{
				EnqueueCommand(new GrblCommand(string.Format("G90")));
				EnqueueCommand(new GrblCommand(string.Format("G1X0Y0F{0}", speed)));
			}
			else
			{
				string cmd = "G1";

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

				cmd += $"F{speed}";

				EnqueueCommand(new GrblCommand("G91"));
				EnqueueCommand(new GrblCommand(cmd));
				EnqueueCommand(new GrblCommand("G90"));
			}
		}

		private void EnqueueJogV09(PointF target, float speed) //emulate jog using plane G-Code
		{
			string cmd = "G1";

			cmd += $"X{target.X.ToString("0.00", NumberFormatInfo.InvariantInfo)}";
			cmd += $"Y{target.Y.ToString("0.00", NumberFormatInfo.InvariantInfo)}";
			cmd += $"F{speed}";

			EnqueueCommand(new GrblCommand("G90"));
			EnqueueCommand(new GrblCommand(cmd));
		}

		private void EnqueueJogV11(PointF target, float speed)
		{
			EnqueueCommand(new GrblCommand(string.Format("$J=G90X{0}Y{1}F{2}", target.X.ToString("0.00", NumberFormatInfo.InvariantInfo), target.Y.ToString("0.00", NumberFormatInfo.InvariantInfo), speed)));
		}

		private void EnqueueJogV11(JogDirection dir, decimal step, float speed)
		{
			if (dir == JogDirection.Home)
			{
				EnqueueCommand(new GrblCommand(string.Format("$J=G90X0Y0F{0}", speed)));
			}
			else
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

				cmd += $"F{speed}";
				EnqueueCommand(new GrblCommand(cmd));
			}

		}

		//private void DoJogV11(JogDirection dir, decimal step)
		//{
		//	// TODO: rewrite

		//	//if (ContinuosJogEnabled && dir != JogDirection.Zdown && dir != JogDirection.Zup) //se C.J. e non Z => prenotato
		//	//{
		//	//	mPrenotedJogDirection = dir;
		//	//	//lo step è quello configurato
		//	//}
		//	//else //non è CJ o non è Z => immediate enqueue jog command
		//	//{
		//	//	mPrenotedJogDirection = JogDirection.None;
		//	//	if (dir == JogDirection.Home)
		//	//		EnqueueCommand(new GrblCommand(string.Format("$J=G90X0Y0F{0}", mPrenotedJogSpeed)));
		//	//	else
		//	//		EnqueueCommand(GetRelativeJogCommandv11(dir, step));
		//	//}
		//}

		//private void DoJogV11(PointF target)
		//{
		//	// TODO: rewrite

		//	//mPrenotedJogDirection = JogDirection.None;
		//	//SendImmediate(0x85); //abort previous jog
		//	//EnqueueCommand(new GrblCommand(string.Format("$J=G90X{0}Y{1}F{2}", target.X.ToString("0.00", NumberFormatInfo.InvariantInfo), target.Y.ToString("0.00", NumberFormatInfo.InvariantInfo), mPrenotedJogSpeed)));
		//}



		public class ContinuousJog
		{
			public JogDirection Direction { get; private set; }
			public float Speed { get; private set; } = 0.0f;
			public PointF Target { get; private set; } = PointF.Empty;

			private static string mCurrentTargetLock = "--- JOG TARGET LOCK ---";
			private static ContinuousJog mPrev = null;
			private static ContinuousJog mCurr = null;

			private static void SetJogTarget(ContinuousJog target)
			{
				lock (mCurrentTargetLock)
				{
					mCurr = target; //what to do now
				}
			}

			public static ContinuousJog GetAndClearTarget(out bool abortPrevCommand)
			{
				ContinuousJog rv = null;
				abortPrevCommand = false;
				lock (mCurrentTargetLock)
				{
					if (mCurr != null && mCurr != mPrev)
					{
						abortPrevCommand = mPrev != null && mPrev.Direction != JogDirection.Abort;	//if i have a prev command, and the prev command is not an abort itself, we need to abort the prev command
						rv = mPrev = mCurr; //set prev = cur and return cur
					}
				}
				return rv;
			}

			public static void Abort() => SetJogTarget(new ContinuousJog(JogDirection.Abort, 0, PointF.Empty));
			public static void ToPosition(PointF target, float speed) => SetJogTarget(new ContinuousJog(JogDirection.Position, speed, target));
			public static void ToDirection(JogDirection direction, float speed)
			{
				if (direction == JogDirection.Abort)
					throw new ArgumentException("Invalid option", "direction");
				if (direction == JogDirection.Position)
					throw new ArgumentException("Invalid option", "direction");
				if (direction == JogDirection.Zup)
					throw new ArgumentException("Z Not supported in Continuous Jog", "direction");
				if (direction == JogDirection.Zdown)
					throw new ArgumentException("Z Not supported in Continuous Jog", "direction");

				SetJogTarget(new ContinuousJog(direction, speed, PointF.Empty));
			}

			private ContinuousJog(JogDirection direction, float speed, PointF target)
			{
				this.Direction = direction;
				this.Target = target;
				this.Speed = speed;
			}
		}

		private void HandleContinuosJog() // Handle only Continuos Jog - Other Jog modes are executed by enqueuing command directly
		{
			ContinuousJog newJog = null;
			bool abortRequired = false;
			if (SupportTrueJogging && !HasPendingCommands() && (newJog = ContinuousJog.GetAndClearTarget(out abortRequired)) != null) // we have all the condition for sending jog command, and we have one to send
			{
				if (abortRequired)
					SendImmediate(0x85); // abort previous jog command

				if (newJog.Direction == JogDirection.Abort)
					; //nothing to send, the abort is sent by previous test if needed
				else if (newJog.Direction == JogDirection.Position)
					EnqueueCommand(new GrblCommand(string.Format("$J=G90X{0}Y{1}F{2}", newJog.Target.X.ToString("0.00", NumberFormatInfo.InvariantInfo), newJog.Target.Y.ToString("0.00", NumberFormatInfo.InvariantInfo), newJog.Speed)));
				else if (newJog.Direction == JogDirection.Home)
					EnqueueCommand(new GrblCommand(string.Format("$J=G90X0Y0F{0}", newJog.Speed)));
				else
				{
					JogDirection dir = newJog.Direction;
					string cmd = "$J=G53";
					if (dir == JogDirection.NE || dir == JogDirection.E || dir == JogDirection.SE)
						cmd += $"X{Configuration.TableWidth.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
					if (dir == JogDirection.NW || dir == JogDirection.W || dir == JogDirection.SW)
						cmd += $"X{0.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
					if (dir == JogDirection.NW || dir == JogDirection.N || dir == JogDirection.NE)
						cmd += $"Y{Configuration.TableHeight.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
					if (dir == JogDirection.SW || dir == JogDirection.S || dir == JogDirection.SE)
						cmd += $"Y{0.ToString("0.0", NumberFormatInfo.InvariantInfo)}";
					cmd += $"F{newJog.Speed}";
					EnqueueCommand(new GrblCommand(cmd));
				}
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
						HandleContinuosJog();

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
				//if (HasPendingCommand())
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

		private bool BufferIsFull()
		{
			GrblCommand next = PeekNextCommand();
			return mUsedBuffer > 0 && next != null && !HasSpaceInBuffer(next);
		}

		private GrblCommand PeekNextCommand()
		{
			if (HasPendingCommands() && mPending.Peek().IsWriteEEPROM) //if managing eeprom write act like sync
				return null;
			else if (CurrentStreamingMode == StreamingMode.Buffered && mQueuePtr.Count > 0) //sono buffered e ho roba da trasmettere
				return mQueuePtr.Peek();
			else if (CurrentStreamingMode != StreamingMode.Buffered && !HasPendingCommands()) //sono sync e sono vuoto
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

		protected virtual bool HasSpaceInBuffer(GrblCommand command)
		{ return (mUsedBuffer + command.SerialData.Length) <= BufferSize; }

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

					SendToSerial(tosend);

					if (mTP.InProgram)
						mTP.JobSent();

					debugLastMoveOrActivityDelay.Start();
				}
				catch (Exception ex)
				{
					if (tosend != null) Logger.LogMessage("SendLine", "Error sending [{0}] command: {1}", tosend.Command, ex.Message);
					//Logger.LogException("SendLine", ex);
				}
				finally { tosend.DeleteHelper(); }
			}
		}

		protected virtual void SendToSerial(GrblCommand tosend)
		{
			mUsedBuffer += tosend.SerialData.Length;
			com.Write(tosend.SerialData); //invio dei dati alla linea di comunicazione
		}

		public int UsedBuffer
		{ get { return mUsedBuffer; } }

		public int FreeBuffer
		{ get { return BufferSize - mUsedBuffer; } }

		public virtual int BufferSize
		{ get { return mAutoBufferSize; } }

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
					lock (this)
					{
						ManageReceivedLine(rline);
						HandleMissingOK();
					}
				}

				RX.SleepTime = HasIncomingData() ? CurrentThreadingMode.RxShort : CurrentThreadingMode.RxLong;
			}
			catch (Exception ex)
			{ Logger.LogException("ThreadRX", ex); }
		}

		// this function try to detect and automatically unlock from a "buffer stuck" condition
		// a "buffer stuck" condition occurs when LaserGRBL does not receive some "ok's" 
		// back from grbl (i.e. because of electrical noise on wire) and so LaserGRBL
		// does no longer send commands anymore because think the buffer is full
		// this feature can work only if $10=3 (status report with buffer size report enabled)
		private void HandleMissingOK()
		{
			if (IsBufferStuck() && MachineSayBufferFree())
				UnlockFromBufferStuck(true);
		}

		public void UnlockFromBufferStuck(bool auto)
		{
			if (IsBufferStuck())
				CreateFakeOK(mPending.Count, auto); //rispondi "ok" a tutti i comandi pending
		}

		public bool IsBufferStuck()
		{
			return MachineStatus == MacStatus.Run && HasPendingCommands() && !BufferIsFree() && MachineNotMovingOrReply();
		}

		private void CreateFakeOK(int count, bool auto)
		{
			mSentPtr.Add(new GrblMessage("Unlock from buffer stuck!", GrblMessage.MessageType.Warning));
			string act = auto ? "auto" : "manual";

			ComWrapper.ComLogger.Log("com", $"Handle Missing OK [{count}] ({act})");
			Logger.LogMessage("Issue detector", $"Handle Missing OK [{count}] ({act})");

			for (int i = 0; i < count; i++)
				ManageCommandResponse("ok");
		}

		private bool MachineNotMovingOrReply() => debugLastMoveOrActivityDelay.ElapsedTime > TimeSpan.FromSeconds(10);
		private bool MachineSayBufferFree() => mGrblBuffer == BufferSize;
		private bool BufferIsFree() => mUsedBuffer == 0;
		private bool HasPendingCommands() => mPending.Count > 0;

		protected virtual void ManageReceivedLine(string rline)
		{
			if (IsCommandReplyMessage(rline))
				ManageCommandResponse(rline);
			else if (IsRealtimeStatusMessage(rline))
				ManageRealTimeStatus(rline);
			else if (IsGrblHalWelcomeMessage(rline))
				ManageGrblHalWelcomeMessage(rline);
			else if (IsVigoWelcomeMessage(rline))
				ManageVigoWelcomeMessage(rline);
			else if (IsOrturModelMessage(rline))
				ManageOrturModelMessage(rline);
			else if (IsAuferoModelMessage(rline))
				ManageOrturModelMessage(rline);
			else if (IsOrturFirmwareMessage(rline))
				ManageOrturFirmwareMessage(rline);
			else if (IsSimpleLaserWelcomeMessage(rline))
				ManageSimpleLaserWelcomeMessage(rline);
			else if (IsStandardWelcomeMessage(rline))
				ManageStandardWelcomeMessage(rline);
			else if (IsUnknownWelcomeMessage(rline))
				ManageUnknownWelcomeMessage(rline);
			else if (IsBrokenOkMessage(rline))
				ManageBrokenOkMessage(rline);
			else if (IsStandardBlockingAlarm(rline))
				ManageStandardBlockingAlarm(rline);
			else if (IsStaIPMessage(rline))
				ManageStaIPMessage(rline);
			//else if (IsOrturBlockingAlarm(rline))
			//	ManageOrturBlockingAlarm(rline);
			else
				ManageGenericMessage(rline);
		}

		private void ManageStaIPMessage(string rline)
		{
			try
			{
				//[MSG:Get IP 192.168.1.182]
				mDetectedIP = rline.Substring(12, rline.Length - 1 - 12);
				ManageGenericMessage(rline); //process as usual
			}
			catch (Exception ex)
			{
				Logger.LogMessage("IP Detector", "Ex on [{0}] message", rline);
				Logger.LogException("IP Detector", ex);
			}
		}

		System.Text.RegularExpressions.Regex unknownWelcomeRegex = new System.Text.RegularExpressions.Regex(@"(?<fw>[a-zA-Z]+)\s+(?<maj>\d+)\.(?<min>\d+)(?<build>\D)($|\s)", System.Text.RegularExpressions.RegexOptions.Compiled);

		private bool IsCommandReplyMessage(string rline) => rline.ToLower().StartsWith("ok") || rline.ToLower().StartsWith("error");
		private bool IsRealtimeStatusMessage(string rline) => rline.StartsWith("<") && rline.EndsWith(">");
		private bool IsGrblHalWelcomeMessage(string rline) => rline.StartsWith("GrblHAL ");
		private bool IsVigoWelcomeMessage(string rline) => rline.StartsWith("Grbl-Vigo:");
		private bool IsOrturModelMessage(string rline) => rline.StartsWith("Ortur ");
		private bool IsAuferoModelMessage(string rline) => rline.StartsWith("Aufero ");
		private bool IsOrturFirmwareMessage(string rline) => rline.StartsWith("OLF");
		private bool IsStandardWelcomeMessage(string rline) => rline.StartsWith("Grbl ");
		private bool IsSimpleLaserWelcomeMessage(string rline) => rline.StartsWith("SimpleLaser ");
		private bool IsUnknownWelcomeMessage(string rline) => mWelcomeSeen == null && unknownWelcomeRegex.IsMatch(rline);
		private bool IsBrokenOkMessage(string rline) => rline.ToLower().Contains("ok");
		private bool IsStandardBlockingAlarm(string rline) => rline.ToLower().StartsWith("alarm:");
		private bool IsIVerMessage(string rline) => rline.StartsWith("[VER:") && rline.EndsWith("]");
		private bool IsIOptMessage(string rline) => rline.StartsWith("[OPT:") && rline.EndsWith("]");
		private bool IsStaIPMessage(string rline) => rline.StartsWith("[MSG:Get IP ") && rline.EndsWith("]");
		private bool IsOrturBlockingAlarm(string rline) => false;

		private void ManageGenericMessage(string rline)
		{
			try { mSentPtr.Add(new GrblMessage(rline, SupportCSV)); }
			catch (Exception ex)
			{
				Logger.LogMessage("GenericMessage", "Ex on [{0}] message", rline);
				Logger.LogException("GenericMessage", ex);
			}
		}

		private void ManageStandardBlockingAlarm(string rline)
		{
			if (mTP.LastIssue == DetectedIssue.Unknown && InProgram)
				SetIssue(DetectedIssue.MachineAlarm);

			ManageGenericMessage(rline); //process as usual
		}

		private void ManageOrturBlockingAlarm(string rline)
		{
			ManageGenericMessage(rline); //process as usual
		}

		private void ManageStandardWelcomeMessage(string rline)
		{
			//Grbl X.Xx ['$' for help]
			try
			{
				mWelcomeSeen = rline; //welcome already seen - disable unknown firmware detection
				int maj = int.Parse(rline.Substring(5, 1));
				int min = int.Parse(rline.Substring(7, 1));
				char build = rline.Substring(8, 1).ToCharArray()[0];
				GrblVersion = new GrblVersionInfo(maj, min, build, mVendorInfoSeen, mVendorVersionSeen, false);

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

		private void ManageUnknownWelcomeMessage(string rline)
		{
			//BlaBla X.Xx
			try
			{
				mWelcomeSeen = rline; //unknwn welcome seen - disable next unknown firmware detection
				unknownWelcomeRegex.Match(rline);
				int maj = int.Parse(unknownWelcomeRegex.Match(rline).Groups["maj"].Value);
				int min = int.Parse(unknownWelcomeRegex.Match(rline).Groups["min"].Value);
				char build = unknownWelcomeRegex.Match(rline).Groups["build"].Value.ToCharArray()[0];
				string fw = unknownWelcomeRegex.Match(rline).Groups["fw"].Value;
				mVendorInfoSeen = fw;

				GrblVersion = new GrblVersionInfo(maj, min, build, mVendorInfoSeen, mVendorVersionSeen, false);
				Logger.LogMessage("UnknInfo", "Detected {0}", mVendorInfoSeen);

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

		private void ManageSimpleLaserWelcomeMessage(string rline)
		{
			//SimpleLaser X.Xx ['$' for help]
			try
			{
				mWelcomeSeen = rline; //welcome already seen - disable unknown firmware detection
				int maj = int.Parse(rline.Substring(12, 1));
				int min = int.Parse(rline.Substring(14, 1));
				char build = rline.Substring(15, 1).ToCharArray()[0];
				mVendorInfoSeen = "SimpleLaser";
				GrblVersion = new GrblVersionInfo(maj, min, build, mVendorInfoSeen, mVendorVersionSeen, false);
				Logger.LogMessage("SimpleInfo", "Detected {0}", mVendorInfoSeen);

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
		private void ManageGrblHalWelcomeMessage(string rline)
		{
			//GrblHAL X.Xx ['$' or '$HELP' for help]
			try
			{
				mWelcomeSeen = rline; //welcome already seen - disable unknown firmware detection
				int maj = int.Parse(rline.Substring(8, 1));
				int min = int.Parse(rline.Substring(10, 1));
				char build = rline.Substring(11, 1).ToCharArray()[0];
				GrblVersion = new GrblVersionInfo(maj, min, build, mVendorInfoSeen, mVendorVersionSeen, true);

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

		private void ManageVigoWelcomeMessage(string rline)
		{
			//Grbl-Vigo:1.1f|Build:G-20170131-V3.0-20200720
			try
			{
				mWelcomeSeen = rline; //welcome already seen - disable unknown firmware detection
				int maj = int.Parse(rline.Substring(10, 1));
				int min = int.Parse(rline.Substring(12, 1));
				char build = rline.Substring(13, 1).ToCharArray()[0];
				mVendorInfoSeen = "Grbl-Vigo";
				mVendorVersionSeen = rline.Split(':')[2];
				GrblVersion = new GrblVersionInfo(maj, min, build, mVendorInfoSeen, mVendorVersionSeen, false);
				Logger.LogMessage("VigoInfo", "Detected {0} {1}", mVendorInfoSeen, mVendorVersionSeen);

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

		private void ManageOrturModelMessage(string rline)
		{
			try
			{
				mVendorInfoSeen = rline;
				mVendorInfoSeen = mVendorInfoSeen.Replace("Ready", "");
				mVendorInfoSeen = mVendorInfoSeen.Replace("!", "");
				mVendorInfoSeen = mVendorInfoSeen.Trim();
				Logger.LogMessage("OrturInfo", "Detected {0}", mVendorInfoSeen);
			}
			catch (Exception ex)
			{
				Logger.LogMessage("OrturInfo", "Ex on [{0}] message", rline);
				Logger.LogException("OrturInfo", ex);
			}
			mSentPtr.Add(new GrblMessage(rline, false));
		}

		private void ManageOrturFirmwareMessage(string rline)
		{
			try
			{
				mVendorVersionSeen = rline;
				mVendorVersionSeen = mVendorVersionSeen.Replace("OLF", "");
				mVendorVersionSeen = mVendorVersionSeen.Trim(new char[] { '.', ' ', ':' });
				Logger.LogMessage("OrturInfo", "Detected OLF {0}", mVendorVersionSeen);
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
				mUsedBuffer = 0;
				mTP.JobEnd(true);
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

		private void ManageRealTimeStatus(string rline)
		{
			try
			{
				debugLastStatusDelay.Start();

				rline = rline.Substring(1, rline.Length - 2);

				GrblVersionInfo rversion = StatusReportVersion(rline);
				if (rversion >= new GrblVersionInfo(1, 1))
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
			SetWCO(new GPoint(ParseFloat(xyz, 0), ParseFloat(xyz, 1), ParseFloat(xyz, 2)));
		}

		private void ParseWPos(string p)
		{
			string wpos = p.Substring(5, p.Length - 5);
			string[] xyz = wpos.Split(",".ToCharArray());
			SetMPosition(mWCO + new GPoint(ParseFloat(xyz, 0), ParseFloat(xyz, 1), ParseFloat(xyz, 2)));
		}

		private void ParseMPos(string p)
		{
			string mpos = p.Substring(5, p.Length - 5);
			string[] xyz = mpos.Split(",".ToCharArray());
			SetMPosition(new GPoint(ParseFloat(xyz, 0), ParseFloat(xyz, 1), ParseFloat(xyz, 2)));
		}

		protected static float ParseFloat(string value)
		{
			return float.Parse(value, NumberFormatInfo.InvariantInfo);
		}

		protected static float ParseFloat(string[] arr, int idx, float defval = 0.0f)
		{
			if (arr == null || idx < 0 || idx >= arr.Length) return defval;
			return float.Parse(arr[idx], NumberFormatInfo.InvariantInfo);
		}

		private void ParseBf(string p)
		{
			string bf = p.Substring(3, p.Length - 3);
			string[] ab = bf.Split(",".ToCharArray());

			mGrblBlocks = int.Parse(ab[0]);
			mGrblBuffer = int.Parse(ab[1]);

			EnlargeBuffer(mGrblBuffer, false); //do not force here, we cannot rely on values received from report (electrical noise?!)
		}

		private void EnlargeBuffer(int newval, bool force)
		{
			int oldval = mAutoBufferSize;

			if (force) //this came from $I at connect, and so it is a verified buffer size
			{
				mAutoBufferSize = newval;
			}
			else if (mAutoBufferSize == DEFAULT_BUFFER_SIZE)
			{
				// act only to change default value at first event
				// do not re-act without a new connect
				// only allow known values (do not accept error in bf message caused by electrical noise)

				if (newval == 128) //Grbl v1.1 with enabled buffer report
					mAutoBufferSize = 128;
				else if (newval == 255) //Grbl-Mega fixed
					mAutoBufferSize = 255;
				else if (newval == 256) //Grbl-Mega
					mAutoBufferSize = 256;
				else if (newval == 10240) //Grbl-LPC
					mAutoBufferSize = 10240;
				else if (newval == 254) //Ortur
					mAutoBufferSize = 254;
			}

			if (mAutoBufferSize != oldval)
				Logger.LogMessage("EnlargeBuffer", "Buffer size changed to {0} [{1}]", mAutoBufferSize, force);
		}

		private void ParseFS(string p)
		{
			string sfs = p.Substring(3, p.Length - 3);
			string[] fs = sfs.Split(",".ToCharArray());
			SetFS(ParseFloat(fs, 0), ParseFloat(fs, 1));
		}

		protected virtual void ParseF(string p)
		{
			string f = p.Substring(2, p.Length - 2);
			SetFS(ParseFloat(f), 0);
		}

		protected void SetFS(float f, float s)
		{
			LaserLifeHandler.ComputeLaserTrueTime(mCurS);
			mCurF = f;
			mCurS = s;
		}


		protected void SetMPosition(GPoint pos)
		{
			if (pos != mMPos)
			{
				mMPos = pos;
				debugLastMoveOrActivityDelay.Start();
			}
		}

		private void SetWCO(GPoint wco)
		{
			mWCO = wco;
			mTP.LastKnownWCO = wco; //remember last wco for job resume
		}


		protected void ManageBrokenOkMessage(string rline) //
		{
			mSentPtr.Add(new GrblMessage("Handle broken ok!", GrblMessage.MessageType.Warning));
			Logger.LogMessage("CommandResponse", "Broken \"ok\" message: [{0}]", rline);
			ManageCommandResponse("ok");
		}

		protected void ManageCommandResponse(string rline)
		{
			try
			{
				debugLastMoveOrActivityDelay.Start(); //add a reset to prevent HangDetector trigger on G4
				if (HasPendingCommands())
				{
					GrblCommand pending = mPending.Peek();  //necessario fare peek
					pending.SetResult(rline, SupportCSV);   //assegnare lo stato
					mPending.Dequeue();                     //solo alla fine rimuoverlo dalla lista (per write config che si aspetta che lo stato sia noto non appena la coda si svuota)

					mUsedBuffer = Math.Max(0, mUsedBuffer - pending.SerialData.Length);

					if (mTP.InProgram && pending.RepeatCount == 0) //solo se non è una ripetizione aggiorna il tempo
						mTP.JobExecuted(pending.TimeOffset);

					if (mTP.InProgram && pending.Status == GrblCommand.CommandStatus.ResponseBad)
						mTP.JobError(); //incrementa il contatore

					if (pending.IsWriteEEPROM && pending.Status == GrblCommand.CommandStatus.ResponseGood)
						Configuration.AddOrUpdate(pending.GetDecodedMessage());

					//ripeti errori programma && non ho una coda (magari mi sto allineando per cambio conf buff/sync) && ho un errore && non l'ho già ripetuto troppe volte
					if (InProgram && CurrentStreamingMode == StreamingMode.RepeatOnError && !HasPendingCommands() && pending.Status == GrblCommand.CommandStatus.ResponseBad && pending.RepeatCount < 3) //il comando eseguito ha dato errore
						mRetryQueue = new GrblCommand(pending.Command, pending.RepeatCount + 1); //repeat on error
				}

				if (InProgram && mQueuePtr.Count == 0 && !HasPendingCommands())
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
				if (rv == null) return null;
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
			if (data.Contains(":"))
				data = data.Substring(0, data.IndexOf(':'));

			MacStatus var = (MacStatus)Enum.Parse(typeof(MacStatus), data);

			if (InProgram && var == MacStatus.Idle) //bugfix for grbl sending Idle on G4
				var = MacStatus.Run;

			if (var == MacStatus.Hold && mHoldByCoolingRequest)
				var = MacStatus.Cooling;
			else if (var == MacStatus.Hold && !mHoldByUserRequest)
				var = MacStatus.AutoHold;

			SetStatus(var);
		}


		protected virtual void ForceStatusIdle() { } // Used by Marlin to update status to Idle (As Marlin has no immediate message)

		private void OnProgramEnd()
		{
			if (mTP.JobEnd(mLoopCount == 1) && mLoopCount > 1 && mMachineStatus != MacStatus.Check)
			{
				Logger.LogMessage("CycleEnd", "Cycle Executed: {0} lines, {1} errors, {2}", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(ProgramTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second, ",", true));
				mSentPtr.Add(new GrblMessage(string.Format("[{0} lines, {1} errors, {2}]", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(ProgramTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second, ",", true)), GrblMessage.MessageType.Diagnostic));
				OnProgramEnded?.Invoke();
				LoopCount--;
				RunProgramFromStart(false, false, true);
			}
			else
			{
				Logger.LogMessage("ProgramEnd", "Job Executed: {0} lines, {1} errors, {2}", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(ProgramTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second, ",", true));
				mSentPtr.Add(new GrblMessage(string.Format("[{0} lines, {1} errors, {2}]", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(ProgramTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second, ",", true)), GrblMessage.MessageType.Diagnostic));
                OnProgramEnded?.Invoke();
                OnJobEnd();

				SoundEvent.PlaySound(SoundEvent.EventId.Success);

				if (ProgramGlobalTime.TotalMinutes >= (int)Settings.GetObject("TelegramNotification.Threshold", 1))
					Telegram.NotifyEvent(String.Format("<b>Job Executed</b>\n{0} lines, {1} errors\nTime: {2}", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(ProgramGlobalTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second, ",", true)));

				ForceStatusIdle();
			}
		}

		private bool InPause
		{ get { return mMachineStatus != MacStatus.Run && mMachineStatus != MacStatus.Idle; } }

		private void ClearQueue(Queue<GrblCommand> queue)
        {
			foreach (GrblCommand command in queue)
			{
				command.Dispose();
			}
			queue.Clear();
        }

        private void ClearQueue(bool sent)
		{
            ClearQueue(mQueue);
            ClearQueue(mPending);
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

		public bool QueueEmpty { get { return mQueue.Count == 0; } }

		public bool CanLoadNewFile
		{ get { return !InProgram && !file.CheckInUse(false); } }

		public bool CanSendFile
		{ get { return IsConnected && HasProgram && IdleOrCheck && QueueEmpty && !mDoingSend; } }

		public bool CanAbortProgram
		{ get { return IsConnected && HasProgram && (MachineStatus == MacStatus.Run || IsAnyHoldState(MachineStatus)) || !QueueEmpty; } }

		public bool CanImportExport
		{ get { return IsConnected && MachineStatus == MacStatus.Idle; } }

		public bool CanResetGrbl
		{ get { return IsConnected && MachineStatus != MacStatus.Disconnected; } }

		public bool CanSendManualCommand
		{ get { return IsConnected && MachineStatus != MacStatus.Disconnected && !InProgram; } }

		public bool CanDoHoming
		{ get { return IsConnected && (MachineStatus == MacStatus.Idle || MachineStatus == GrblCore.MacStatus.Alarm) && Configuration.HomingEnabled; } }

		public bool CanDoZeroing
		{ get { return IsConnected && MachineStatus == MacStatus.Idle && WorkPosition != GPoint.Zero; } }

		public bool CanUnlock
		{ get { return IsConnected && (MachineStatus == MacStatus.Idle || MachineStatus == GrblCore.MacStatus.Alarm); } }

		public bool CanFeedHold
		{ get { return IsConnected && MachineStatus == MacStatus.Run; } }

		public bool CanResumeHold
		{ get { return IsConnected && (MachineStatus == MacStatus.Door || IsAnyHoldState(MachineStatus)); } }

		public bool CanReadWriteConfig
		{ get { return IsConnected && !InProgram && (MachineStatus == MacStatus.Idle || MachineStatus == MacStatus.Alarm); } }

		public decimal LoopCount
		{ get { return mLoopCount; } set { mLoopCount = value; if (OnLoopCountChange != null) OnLoopCountChange(mLoopCount); } }

		private ThreadingMode CurrentThreadingMode
		{ get { return Settings.GetObject("Threading Mode", ThreadingMode.UltraFast); } }

		public virtual StreamingMode CurrentStreamingMode
		{ get { return Settings.GetObject("Streaming Mode", StreamingMode.Buffered); } }

		private bool IdleOrCheck
		{ get { return MachineStatus == MacStatus.Idle || MachineStatus == MacStatus.Check; } }

		public bool AutoCooling
		{ get { return Settings.GetObject("AutoCooling", false) && SupportAutoCooling; } }
		public bool SupportAutoCooling { get => GrblVersion != null && GrblVersion >= new GrblVersionInfo(1, 1) && Configuration != null && Configuration.LaserMode; }

		public TimeSpan AutoCoolingOn
		{ get { return Settings.GetObject("AutoCooling TOn", TimeSpan.FromMinutes(10)); } }

		public TimeSpan AutoCoolingOff
		{ get { return Settings.GetObject("AutoCooling TOff", TimeSpan.FromMinutes(1)); } }

		private void ManageCoolingCycles()
		{
			if (AutoCooling && InProgram && MachineStatus != MacStatus.Hold && MachineStatus != MacStatus.AutoHold)
				NowCooling = (ProgramGlobalTime.Ticks % (AutoCoolingOn + AutoCoolingOff).Ticks) > AutoCoolingOn.Ticks;
		}

		protected bool mHoldByUserRequest = false;
		protected bool mHoldByCoolingRequest = false;
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
			get { return System.IO.Path.GetDirectoryName(Application.ExecutablePath); }
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


		internal bool ManageHotKeys(Form parent, System.Windows.Forms.Keys keys)
		{
			if (SuspendHK)
				return false;
			else
				return mHotKeyManager.ManageHotKeys(parent, keys);
		}

		internal string GetHotKeyString(HotKey.Actions action)
		{
			return mHotKeyManager.GetHotKeyString(action);
		}

		internal void HKConnectDisconnect()
		{
			if (IsConnected)
				HKDisconnect();
			else
				HKConnect();
		}

		internal void HKConnect()
		{
			if (!IsConnected)
				OpenCom();
		}

		internal void HKDisconnect()
		{
			if (IsConnected)
			{
				if (!(InProgram && System.Windows.Forms.MessageBox.Show(Strings.DisconnectAnyway, Strings.WarnMessageBoxHeader, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning, System.Windows.Forms.MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
					CloseCom(true);
			}
		}

		internal void HelpOnLine()
		{ Tools.Utils.OpenLink(@"https://lasergrbl.com/usage/"); }

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

		internal void WriteHotkeys(System.Collections.Generic.List<HotKeysManager.HotKey> mLocalList)
		{
			mHotKeyManager.Clear();
			mHotKeyManager.AddRange(mLocalList);
			Settings.SetObject("Hotkey Setup", mHotKeyManager, true);
		}

		//internal void HKCustomButton(int index)
		//{
		//	CustomButton cb = CustomButtons.GetButton(index);
		//	if (cb != null && cb.EnabledNow(this))
		//		ExecuteCustombutton(cb.GCode);
		//}

		static System.Text.RegularExpressions.Regex bracketsRegEx = new System.Text.RegularExpressions.Regex(@"\[(?:[^]]+)\]");
		internal void ExecuteCustomCode(string buttoncode)
		{
			buttoncode = buttoncode.Trim();
			string[] arr = buttoncode.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			foreach (string str in arr)
			{
				if (str.Trim().Length > 0)
				{
					string tosend = EvaluateExpression(str);

					if (IsImmediate(tosend))
					{
						byte b = GetImmediate(tosend);
						if (b == '!') mHoldByUserRequest = true;
						SendImmediate(b);
					}
					else
					{ EnqueueCommand(new GrblCommand(tosend, 0, true)); }
				}
			}
		}

		internal string EvaluateExpression(string str)
		{
			return bracketsRegEx.Replace(str, new System.Text.RegularExpressions.MatchEvaluator(EvaluateCB)).Trim();
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
				exp.AddSetVariable("WCO.X", (double)WorkingOffset.X);
				exp.AddSetVariable("WCO.Y", (double)WorkingOffset.Y);
				exp.AddSetVariable("WCO.Z", (double)WorkingOffset.Z);
				exp.AddSetVariable("MPos.X", (double)MachinePosition.X);
				exp.AddSetVariable("MPos.Y", (double)MachinePosition.Y);
				exp.AddSetVariable("MPos.Z", (double)MachinePosition.Z);
				exp.AddSetVariable("WPos.X", (double)MachinePosition.X);
				exp.AddSetVariable("WPos.Y", (double)MachinePosition.Y);
				exp.AddSetVariable("WPos.Z", (double)MachinePosition.Z);

				GrblConfST conf = Configuration;
				if (conf != null)
				{
					foreach (KeyValuePair<int, string> p in conf)
					{
						if (double.TryParse(p.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double tod)) //aggiungi solo ciò che si riesce a convertire in double
							exp.AddSetVariable("$" + p.Key, tod);
					}
				}

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

        public void AutoSizeDrawing()
        {
			OnAutoSizeDrawing?.Invoke(this);
        }

		public void ZoomInDrawing()
		{
			OnZoomInDrawing?.Invoke(this);
		}

		public void ZoomOutDrawing()
		{
			OnZoomOutDrawing?.Invoke(this);
		}

		public virtual bool UIShowGrblConfig => true;
		public virtual bool UIShowUnlockButtons => true;

		public bool IsOrturBoard { get => GrblVersion != null && GrblVersion.IsOrtur; }
		public int FailedConnectionCount => mFailedConnection;

		public event Action OnProgramEnded;
    }

	public class TimeProjection
	{
		private TimeSpan mETarget;
		private TimeSpan mEProgress;


		private long mStart;        //Start Time
		private long mEnd;          //End Time
		private long mGlobalStart;  //Global Start (multiple pass)
		private long mGlobalEnd;    //Global End (multiple pass)
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

		GrblCore.DetectedIssue mLastIssue;
		private GPoint mLastKnownWCO;

		public GPoint LastKnownWCO
		{
			get { return mLastKnownWCO; }
			set { if (InProgram) mLastKnownWCO = value; }
		}

		public TimeProjection()
		{ Reset(true); }

		public void Reset(bool global)
		{
			mETarget = TimeSpan.Zero;
			mEProgress = TimeSpan.Zero;
			mStart = mEnd = 0;
			if (global) mGlobalStart = mGlobalEnd = 0;
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

		public TimeSpan TotalGlobalJobTime
		{
			get
			{
				if (mCompleted)
					return TimeSpan.FromMilliseconds(mGlobalEnd - mGlobalStart);
				else if (mStarted)
					return TimeSpan.FromMilliseconds(now - mGlobalStart);
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

		public void JobStart(GrblFile file, Queue<GrblCommand> mQueuePtr, bool global)
		{
			if (!mStarted)
			{
				mETarget = file.EstimatedTime;
				mTargetCount = mQueuePtr.Count;
				mEProgress = TimeSpan.Zero;
				mStart = Tools.HiResTimer.TotalMilliseconds;
				if (global) mGlobalStart = mStart;
				mPauseBegin = 0;
				mInPause = false;
				mCompleted = false;
				mStarted = true;
				mExecutedCount = 0;
				mSentCount = 0;
				mErrorCount = 0;
				mContinueCorrection = 0;
				mLastIssue = GrblCore.DetectedIssue.Unknown;
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
				if (mStart == 0)
					mGlobalStart = mStart = Tools.HiResTimer.TotalMilliseconds;

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

		public bool JobEnd(bool global)
		{
			if (mStarted && !mCompleted)
			{
				JobResume(); //nel caso l'ultimo comando fosse una pausa, la chiudo e la cumulo
				mEnd = Tools.HiResTimer.TotalMilliseconds;
				if (global) mGlobalEnd = mEnd;
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


		//private Tools.ElapsedFromEvent crono = new Tools.ElapsedFromEvent();

		//private string LastJobFileName => System.IO.Path.Combine(GrblCore.DataPath, "lastjob.nc");
		//private string LastPositionFileName => System.IO.Path.Combine(GrblCore.DataPath, "jobprogress.dat");

		//public void LoadFile(GrblFile file)
		//{
		//	try
		//	{
		//		if (System.IO.File.Exists(LastJobFileName))
		//			System.IO.File.Delete(LastJobFileName);
		//		if (file.Count > 0)
		//			file.SaveProgram(LastJobFileName, false, false, false, 1);
		//	}
		//	catch
		//	{ }
		//}

		//public void JobBegin()
		//{
		//	try
		//	{
		//		if (System.IO.File.Exists(LastPositionFileName))
		//			System.IO.File.Delete(LastPositionFileName);

		//		crono.Start();
		//		WriteCurrentPosition();
		//	}
		//	catch
		//	{ }
		//}

		//public void JobEnd()
		//{
		//	//try
		//	//{
		//	//	if (System.IO.File.Exists(LastPositionFileName))
		//	//		System.IO.File.Delete(LastPositionFileName);
		//	//}
		//	//catch
		//	//{ }
		//}

		//public void JobProgress()
		//{
		//	try
		//	{
		//		if (crono.ElapsedTime > TimeSpan.FromSeconds(10))
		//		{
		//			crono.Start();
		//			WriteCurrentPosition();
		//		}
		//	}
		//	catch
		//	{ }
		//}

		//private void WriteCurrentPosition()
		//{
		//	using (System.IO.FileStream fs = new System.IO.FileStream(LastPositionFileName, System.IO.FileMode.Create))
		//	{
		//		using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(fs))
		//		{
		//			int exec = Executed;
		//			int sent = Sent;
		//			int target = Target;

		//			w.Write(exec);
		//			w.Write(sent);
		//			w.Write(target);
		//			w.Write(exec ^ sent ^ target ^ 0x55555555); //checksum valid data
		//		}
		//	}
		//}
	}

	[Serializable]
	public class GrblConfST : IEnumerable<KeyValuePair<int, string>>
	{
		private const decimal MAX_CONFIG_SPEED = 2000000;
		private const decimal MAX_CONFIG_PWM = 2000000;
		private const decimal MAX_CONFIG_RESOLUTION = 10000;
		private const decimal MAX_CONFIG_SIZE = 2000000;
		private const decimal MAX_CONFIG_ACCEL = 2000000;


		public class GrblConfParam : ICloneable
		{
			private int mNumber;
			private string mValue;

			public GrblConfParam(int number, string value)
			{ mNumber = number; mValue = value; }

			public int Number
			{ get { return mNumber; } }

			public string DollarNumber
			{ get { return "$" + mNumber.ToString(); } }

			public string Parameter
			{ get { return CSVD.Settings.GetItem(mNumber.ToString(), 0); } }

			//public decimal Value
			//{
			//	get { return decimal.Parse(mValue, CultureInfo.InvariantCulture); }
			//	set { mValue = value.ToString(CultureInfo.InvariantCulture); }
			//}

			public string Value
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

		private Dictionary<int, string> mData;
		private GrblCore.GrblVersionInfo mVersion;


		public GrblConfST()
		{
			mData = new Dictionary<int, string>();
		}

		public GrblConfST(GrblCore.GrblVersionInfo GrblVersion) : this()
		{
			mVersion = GrblVersion;
		}

		public GrblConfST(GrblCore.GrblVersionInfo GrblVersion, Dictionary<int, string> configTable) : this(GrblVersion)
		{
			foreach (KeyValuePair<int, string> kvp in configTable)
				mData.Add(kvp.Key, kvp.Value);
		}

		public GrblConfST(GrblConf old) : this(old?.GrblVersion)
		{
			if (old != null)
				foreach (KeyValuePair<int, decimal> kvp in old)
					mData.Add(kvp.Key, kvp.Value.ToString(CultureInfo.InvariantCulture));
		}

		public GrblCore.GrblVersionInfo GrblVersion => mVersion;
		private bool NoVersionInfo => mVersion == null;
		private bool Version11 => mVersion != null && mVersion >= new GrblCore.GrblVersionInfo(1, 1);
		private bool Version9 => mVersion != null && mVersion >= new GrblCore.GrblVersionInfo(0, 9);

		public int ExpectedCount => Version11 ? 34 : Version9 ? 31 : 23;
		public bool HomingEnabled => ReadDecimal(Version9 ? 22 : 17, 1, 0, 1) != 0;
		public decimal MaxRateX => ReadDecimal(Version9 ? 110 : 4, 4000, 1, MAX_CONFIG_SPEED);
		public decimal MaxRateY => ReadDecimal(Version9 ? 111 : 5, 4000, 1, MAX_CONFIG_SPEED);

		public bool LaserMode
		{
			get
			{
				if (NoVersionInfo)
					return true;
				else
					return ReadDecimal(Version11 ? 32 : -1, 0, 0, 1) != 0;
			}
		}

		public decimal MinPWM => ReadDecimal(Version11 ? 31 : -1, 0, 0, MAX_CONFIG_PWM);
		public decimal MaxPWM => ReadDecimal(Version11 ? 30 : -1, 1000, 1, MAX_CONFIG_PWM);
		public decimal ResolutionX => ReadDecimal(Version9 ? 100 : 0, 250, 1, MAX_CONFIG_RESOLUTION);
		public decimal ResolutionY => ReadDecimal(Version9 ? 101 : 1, 250, 1, MAX_CONFIG_RESOLUTION);
		public decimal TableWidth => ReadDecimal(Version9 ? 130 : -1, 300, 1, MAX_CONFIG_SIZE);
		public decimal TableHeight => ReadDecimal(Version9 ? 131 : -1, 200, 1, MAX_CONFIG_SIZE);
		public bool SoftLimit => ReadDecimal(20, 0, 0, 1) != 0;

		public decimal AccelerationXY => (AccelerationX + AccelerationY) / 2;
		private decimal AccelerationX => ReadDecimal(Version9 ? 120 : -1, 2000, 1, MAX_CONFIG_ACCEL);
		private decimal AccelerationY => ReadDecimal(Version9 ? 121 : -1, 2000, 1, MAX_CONFIG_ACCEL);

		public string WiFi_SSID => ReadString(74, null);
		public string WiFi_Pwd => ReadString(75, null);
		public string TelnetPort => ReadString(305, "23");


		private decimal ReadDecimal(int key, decimal defval, decimal min, decimal max)
		{
			if (mVersion == null)
				return defval;
			else if (!mData.ContainsKey(key))
				return defval;
			else
			{
				try
				{
					return Math.Max(min, Math.Min(max, decimal.Parse(mData[key], CultureInfo.InvariantCulture)));
				}
				catch
				{
					try //proviamo a pulire la stringa con una regex?!
					{
						System.Text.RegularExpressions.Regex ExtractNumber = new System.Text.RegularExpressions.Regex(@"(\d+\.?\d*)");
						System.Text.RegularExpressions.MatchCollection matches = ExtractNumber.Matches(mData[key]);
						return decimal.Parse(matches[0].Groups[1].Value);
					}
					catch
					{
						return defval;
					}
				}
			}
		}

		private string ReadString(int number, string defval)
		{
			if (mVersion == null)
				return defval;
			else if (!mData.ContainsKey(number))
				return defval;
			else
				return mData[number];
		}

		public List<GrblConfParam> ToList()
		{
			List<GrblConfParam> rv = new List<GrblConfParam>();
			foreach (KeyValuePair<int, string> kvp in mData)
				rv.Add(new GrblConfParam(kvp.Key, kvp.Value));
			return rv;
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

		bool CompareValues(string a, string b)
		{
			if (Equals(a, b))
				return true;
			return false;
		}

		private bool ContainsKey(int key)
		{
			return mData.ContainsKey(key);
		}


		public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
		{
			return mData.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return mData.GetEnumerator();
		}

		// "$" at the beginning of a line, followed by numbers, by any spaces, by "=" and then averything
		private static System.Text.RegularExpressions.Regex ConfRegEX = new System.Text.RegularExpressions.Regex(@"^[$](\d+)\s*=(.*)");

		public static bool IsSetConf(string p)
		{ return ConfRegEX.IsMatch(p); }

		public void AddOrUpdate(string line)
		{
			try
			{
				if (IsSetConf(line))
				{
					line = FixFoxalienConfig(line);

					System.Text.RegularExpressions.MatchCollection matches = ConfRegEX.Matches(line);
					int key = int.Parse(matches[0].Groups[1].Value);
					string val = matches[0].Groups[2].Value.Trim();

					if (ContainsKey(key))
						mData[key] = val;
					else
						mData.Add(key, val);
				}
			}
			catch (Exception)
			{

			}
		}


		// Foxalien send config values with descriprion inside it i.e. "$20=0 (soft limits, bool)\r" , so we need to do a cleanup to remove description between parentesis
		// See issue https://github.com/arkypita/LaserGRBL/issues/1890
		private static System.Text.RegularExpressions.Regex FoxalienCleanupConfig = new System.Text.RegularExpressions.Regex(@"^[$](\d+)\s*=\s*(\d+\.?\d*)\s*\(.*\)");
		private string FixFoxalienConfig(string line)
		{
			if (!FoxalienCleanupConfig.IsMatch(line))
				return line;

			System.Text.RegularExpressions.MatchCollection matches = FoxalienCleanupConfig.Matches(line);
			int key = int.Parse(matches[0].Groups[1].Value);
			string val = matches[0].Groups[2].Value.Trim();

			return $"${key}={val}";
		}

		internal bool SetValueIfKeyExist(string p)
		{
			try
			{
				if (IsSetConf(p))
				{
					System.Text.RegularExpressions.MatchCollection matches = ConfRegEX.Matches(p);
					int key = int.Parse(matches[0].Groups[1].Value);
					string val = matches[0].Groups[2].Value.Trim();

					if (!ContainsKey(key))
						return false;

					mData[key] = val;
					return true;
				}
			}
			catch (Exception)
			{

			}

			return false;
		}

		internal string ValidateConfig(int parid, object value)
		{
			if (parid == 33 && mVersion != null && mVersion.IsOrtur)
				return "This param control an Ortur safety feature. Please do not change this value!";

			return null;
		}
	}



	[Serializable, Obsolete]
	public class GrblConf : IEnumerable<KeyValuePair<int, decimal>>
	{
		[Obsolete]
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

		public GrblCore.GrblVersionInfo GrblVersion => mVersion;
		private bool NoVersionInfo => mVersion == null;
		private bool Version11 => mVersion != null && mVersion >= new GrblCore.GrblVersionInfo(1, 1);
		private bool Version9 => mVersion != null && mVersion >= new GrblCore.GrblVersionInfo(0, 9);

		public int ExpectedCount => Version11 ? 34 : Version9 ? 31 : 23;
		public bool HomingEnabled => ReadWithDefault(Version9 ? 22 : 17, 1) != 0;
		public decimal MaxRateX => ReadWithDefault(Version9 ? 110 : 4, 4000);
		public decimal MaxRateY => ReadWithDefault(Version9 ? 111 : 5, 4000);

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

		public decimal MinPWM => ReadWithDefault(Version11 ? 31 : -1, 0);
		public decimal MaxPWM => ReadWithDefault(Version11 ? 30 : -1, 1000);
		public decimal ResolutionX => ReadWithDefault(Version9 ? 100 : 0, 250);
		public decimal ResolutionY => ReadWithDefault(Version9 ? 101 : 1, 250);
		public decimal TableWidth => ReadWithDefault(Version9 ? 130 : -1, 300);
		public decimal TableHeight => ReadWithDefault(Version9 ? 131 : -1, 200);
		public bool SoftLimit => ReadWithDefault(20, 0) != 0;

		public decimal AccelerationXY => (AccelerationX + AccelerationY) / 2;
		private decimal AccelerationX => ReadWithDefault(Version9 ? 120 : -1, 2000);
		private decimal AccelerationY => ReadWithDefault(Version9 ? 121 : -1, 2000);

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

		private void Add(int num, decimal val)
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

		private bool ContainsKey(int key)
		{
			return mData.ContainsKey(key);
		}

		private void SetValue(int key, decimal value)
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


		private static System.Text.RegularExpressions.Regex ConfRegEX = new System.Text.RegularExpressions.Regex(@"^[$](\d+) *= *(\d+\.?\d*)");

		public static bool IsSetConf(string p)
		{ return ConfRegEX.IsMatch(p); }

		public void AddOrUpdate(string p)
		{
			try
			{
				if (IsSetConf(p))
				{
					System.Text.RegularExpressions.MatchCollection matches = ConfRegEX.Matches(p);
					int key = int.Parse(matches[0].Groups[1].Value);
					decimal val = decimal.Parse(matches[0].Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);

					if (ContainsKey(key))
						SetValue(key, val);
					else
						Add(key, val);
				}
			}
			catch (Exception)
			{

			}
		}

		internal bool SetValueIfKeyExist(string p)
		{
			try
			{
				if (IsSetConf(p))
				{
					System.Text.RegularExpressions.MatchCollection matches = ConfRegEX.Matches(p);
					int key = int.Parse(matches[0].Groups[1].Value);
					decimal val = decimal.Parse(matches[0].Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);

					if (!ContainsKey(key))
						return false;

					SetValue(key, val);
					return true;
				}
			}
			catch (Exception)
			{

			}

			return false;
		}

		internal string ValidateConfig(int parid, object value)
		{
			if (parid == 33 && mVersion != null && mVersion.IsOrtur)
				return "This param control an Ortur safety feature. Please do not change this value!";

			return null;
		}
	}

	public struct GPoint
	{
		public float X, Y, Z;

		public GPoint(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static GPoint Zero { get { return new GPoint(); } }

		public static bool operator ==(GPoint a, GPoint b)
		{ return a.X == b.X && a.Y == b.Y && a.Z == b.Z; }

		public static bool operator !=(GPoint a, GPoint b)
		{ return !(a == b); }

		public static GPoint operator -(GPoint a, GPoint b)
		{ return new GPoint(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }

		public static GPoint operator +(GPoint a, GPoint b)
		{ return new GPoint(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }

		public override bool Equals(object obj)
		{
			return obj is GPoint && ((GPoint)obj) == this;
		}

		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;
				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				hash = hash * 23 + Z.GetHashCode();
				return hash;
			}
		}

		internal PointF ToPointF()
		{
			return new PointF(X, Y);
		}
	}


	public static class LaserLifeHandler
	{
		[Serializable]
		public class ListLLC : List<LaserLifeCounter>
		{
			public DateTime LastAttempt = new DateTime(2000, 1, 1);
			public DateTime LastSent = new DateTime(2000, 1, 1);

			internal ListLLC GetClone()
			{
				ListLLC clone = new ListLLC();

				lock (this)
				{
					foreach (LaserLifeCounter LLC in this)
						clone.Add(LLC.Clone());
					clone.LastSent = LastSent;
					clone.LastAttempt = LastAttempt;
				}

				return clone;
			}

			//internal void FixWeirdMeasures()
			//{
			//	foreach (LaserLifeCounter LLC in this)
			//		LLC.FixWeirdMeasures();
			//}
		}

		static ListLLC mLLCL;
		static LaserLifeCounter mCurrentLLC;
		private static long? mLastStatusHiResTimeNano;
		private static long? mLastPowerHiResTimeNano;
		private static long mLastSave;
		private static string mLLCFileName;

		[Serializable]
		public class LaserLifeCounter : IDeserializationCallback
		{
			public static string DEF_NAME = "Default";
			public static string DEF_BRAND = "Unknown";
			public static string DEF_MODEL = "Unknown";
			private string mGuid;

			private string mName;
			private string mBrand;
			private string mModuleModel;
			private double? mOpticalPower;

			private DateTime? mPurchaseDate;
			private DateTime? mMonitoringDate;
			private DateTime? mDeathDate;
			private DateTime? mLastUsage;

			private TimeSpan mTimeInRun;
			private TimeSpan mTimeUsageNormalizedPower;
			private TimeSpan mTimeUsageNonZero;
			private TimeSpan[] mTimeClasses;

			public DateTime? PurchaseDate { get => mPurchaseDate; set => mPurchaseDate = value; }
			public DateTime? MonitoringDate { get => mMonitoringDate; set => mMonitoringDate = value; }
			public DateTime? DeathDate { get => mDeathDate; set => mDeathDate = value; }
			public DateTime? LastUsage { get => mLastUsage; set => mLastUsage = value; }
			public string Name { get => mName; set => mName = value; }
			public string Brand { get => mBrand; set => mBrand = value; }
			public string Model { get => mModuleModel; set => mModuleModel = value; }
			public double? OpticalPower { get => mOpticalPower; set => mOpticalPower = value; }
			public TimeSpan TimeInRun { get => mTimeInRun; }
			public TimeSpan TimeUsageNormalizedPower { get => mTimeUsageNormalizedPower; }
			public TimeSpan StressTime { get => mTimeClasses[9]; }
			public double AveragePowerFactor { get => mTimeUsageNonZero.TotalSeconds == 0 ? 0 : mTimeUsageNormalizedPower.TotalSeconds / mTimeUsageNonZero.TotalSeconds; }
			public string Guid { get => mGuid; }
			public TimeSpan[] Classes { get => mTimeClasses; }

			private LaserLifeCounter()
			{
				mGuid = System.Guid.NewGuid().ToString();
				mMonitoringDate = DateTime.Today;
				mTimeClasses = new TimeSpan[10];
			}

			internal static LaserLifeCounter CreateNew()
			{
				LaserLifeCounter rv = new LaserLifeCounter();
				return rv;
			}
			public static LaserLifeCounter CreateDefault()
			{
				LaserLifeCounter rv = new LaserLifeCounter();
				rv.Name = DEF_NAME;
				return rv;
			}

			internal void AddRunTime(TimeSpan elapsed)
			{
				//elapsed = TimeSpan.FromSeconds(elapsed.TotalSeconds * 100);
				mTimeInRun = mTimeInRun.Add(elapsed);
				mLastUsage = DateTime.Today;
			}

			internal LaserLifeCounter Clone()
			{
				return MemberwiseClone() as LaserLifeCounter;
			}

			internal void AddTrueLaserTimePower(TimeSpan elapsed, double powerperc)
			{
				//elapsed = TimeSpan.FromSeconds(elapsed.TotalSeconds * 100);

				if (powerperc > 0.03) //do not track any usage under 3% (framing etc)
				{
					TimeSpan normalized = TimeSpan.FromMilliseconds(elapsed.TotalMilliseconds * powerperc); //normalize elapsed time with power
					mTimeUsageNormalizedPower = mTimeUsageNormalizedPower + normalized;
					mTimeUsageNonZero = mTimeUsageNonZero + elapsed;
				}

				if (powerperc > 0.01)
				{
					int clx = (int)Math.Floor((powerperc * 100 - 1) / 10); //1-10 = 0, 11-20 = 1; 91-100 = 9
					clx = Math.Min(9, Math.Max(0, clx)); //ensure 0-9 range
					mTimeClasses[clx] = mTimeClasses[clx] + elapsed;
				}
				mLastUsage = DateTime.Today;
				//System.Diagnostics.Debug.WriteLine($"LT: {mTimeInRun.TotalSeconds} LTT: {mTimeUsageNormalizedPower.TotalSeconds}");
			}

			internal void Update(LaserLifeCounter selected)
			{
				mName = selected.mName;
				mPurchaseDate = selected.mPurchaseDate;
				mDeathDate = selected.mDeathDate;
				mModuleModel = selected.mModuleModel;
				mBrand = selected.mBrand;
				mOpticalPower = selected.mOpticalPower;
			}

			internal bool HasWorked()
			{
				return mTimeInRun.TotalHours > 1;
			}

			void IDeserializationCallback.OnDeserialization(object sender)
			{
				if (mTimeClasses == null)
					mTimeClasses = new TimeSpan[10];
			}

			//internal void FixWeirdMeasures()
			//{
			//	TimeSpan TT = TimeSpan.Zero;
			//	foreach (TimeSpan TS in Classes)
			//		TT = TT.Add(TS);

			//	if (
			//		LastUsage.HasValue &&
			//		DateTime.Today < new DateTime(2023, 12, 31) && // smette di farlo ad una certa data
			//		TimeInRun.TotalHours > 100 && // almeno 100 ore
			//		TimeInRun.TotalHours > ((LastUsage.Value - MonitoringDate.Value).TotalHours + 24) * 2 && //TIR > 2 * DELTA TRA DATE MONITORATE
			//		TimeInRun.TotalHours > TT.TotalHours * 100 //TIR > 100 volte Classes.Time
			//		)
			//		mTimeInRun = TimeSpan.FromHours(TT.TotalHours * 1.5); //ri-assegna da TT (total time preso dalle classi) esteso * 1.5
			//}
		}

		static LaserLifeHandler()
		{
			if (mLLCL == null)
			{
				mLLCFileName = System.IO.Path.Combine(GrblCore.DataPath, "LaserLifeCounter.bin");
				mLLCL = Serializer.ObjFromFile(mLLCFileName) as ListLLC;
				if (mLLCL == null) mLLCL = Serializer.ObjFromFile(mLLCFileName + ".old") as ListLLC;

				if (mLLCL == null) mLLCL = new ListLLC();
				if (mLLCL.Count == 0) mLLCL.Add(LaserLifeCounter.CreateDefault());

				//mLLCL.FixWeirdMeasures();
			}
		}

		public static void ComputeLaserTime(GrblCore.MacStatus status)
		{
			try
			{
				lock (mLLCL)
				{
					long now = HiResTimer.TotalNano;
					if (mLastStatusHiResTimeNano != null)
					{
						long delta = now - (mLastPowerHiResTimeNano ?? now);
						if (delta > 0 && delta < 600000000000L) //do not add delta if bigger then 600s (or negative) - just a safety test
						{
							double perc = status == MacStatus.Run ? 1 : 0; //potenza da applicare a questo delta
							double normal = delta * perc;
							mCurrentLLC?.AddRunTime(TimeSpan.FromMilliseconds(normal / 1000 / 1000));
						}
					}
					mLastStatusHiResTimeNano = now;
				}
				SendAndSave();
			}
			catch { }
		}

		private static void SendAndSave()
		{
			DoSend();
			DoSave();
		}

		public static void ComputeLaserTrueTime(float power)
		{
			try
			{
				lock (mLLCL)
				{
					long now = HiResTimer.TotalNano;
					if (mLastPowerHiResTimeNano != null)
					{
						long delta = now - mLastPowerHiResTimeNano.Value;
						if (delta > 0 && delta < 600000000000L) //do not add delta if bigger then 600s (or negative) - just a safety test
						{
							decimal maxpwm = Configuration.MaxPWM;

							if (maxpwm >= 10 && maxpwm <= 100000) //we have a configured value in a valid range
							{
								double powerperc = Math.Max(0, Math.Min(1, (double)power / (double)maxpwm)); //potenza da applicare a questo delta
								mCurrentLLC?.AddTrueLaserTimePower(TimeSpan.FromMilliseconds((double)delta / 1000000.0), powerperc);
							}
						}
					}
					mLastPowerHiResTimeNano = now;
				}
				SendAndSave();
			}
			catch { }
		}

		public static void OnConnect(Form parent)
		{
			try
			{
				List<LaserLifeCounter> alive = mLLCL.Where(l => !l.DeathDate.HasValue).ToList();
				List<LaserLifeCounter> death = mLLCL.Where(l => !l.DeathDate.HasValue).ToList();

				if (mLLCL.Count == 0)           //non ce ne sono... non tracciamo
					mCurrentLLC = null;
				if (mLLCL.Count == 1)           //se ne abbiamo uno solo è per forza lui, vivo o morto che sia
					mCurrentLLC = mLLCL[0];
				else if (alive.Count == 1)      //se ne abbiamo più di uno, ma uno solo è vivo => è per forza lui 
					mCurrentLLC = alive[0];
				else
				{
					string selguid = LaserSelector.CreateAndShowDialog(parent);
					mCurrentLLC = mLLCL.First(l => l.Guid == selguid);
				}
			}
			catch { mCurrentLLC = null; }

			Settings.SetObject("Last laser used", mCurrentLLC != null ? mCurrentLLC.Guid : null);
		}

		internal static void OnDisconnect()
		{
			mCurrentLLC = null;
		}

		private static void DoSave()
		{
			try
			{
				lock(mLLCFileName)
				{
					long now = HiResTimer.TotalMilliseconds;
					if (mLastSave == 0 || now - mLastSave > 20000) //save every 20s
					{
						ListLLC tosave = GetListClone();
						System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(RealDoSave), tosave);
						mLastSave = now;
					}
				}
			}
			catch { }
		}

		private static void DoSend()
		{
			try
			{
				DateTime now = DateTime.Now;
				bool sendnow = false;
				lock (mLLCL)
				{
					if ((now - mLLCL.LastSent).TotalDays > 14 && (now - mLLCL.LastAttempt).TotalDays > 1)
					{
						mLLCL.LastAttempt = now;
						sendnow = true;
					}
				}

				if (sendnow)
				{
					ListLLC tosend = GetListClone();
					System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(RealDoSend), tosend);
				}
			}
			catch { }
		}

		public static ListLLC GetListClone()
		{
			return mLLCL?.GetClone();
		}

		public static void SaveNow()
		{
			try { RealDoSave(GetListClone()); }
			catch { }
		}

		private static void RealDoSend(object state)
		{
			try
			{
				List<LaserLifeCounter> tosave = (state as ListLLC).Where(t => t.HasWorked()).ToList();
				if (UrlManager.LaserStatistics != null && tosave.Count > 0)
				{
					string urlAddress = UrlManager.LaserStatistics;
					using (UsageStats.MyWebClient client = new UsageStats.MyWebClient())
					{
						System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection()
						{
							{ "version" , "2" },
							{ "guid", UsageStats.GetID() },
							{ "data", BuildJson(tosave) },
						};

						// client.UploadValues returns page's source as byte array (byte[]) so it must be transformed into a string
						string json = System.Text.Encoding.UTF8.GetString(client.UploadValues(urlAddress, postData));

						//RealDoSendRV RV = Tools.JSONParser.FromJson<RealDoSendRV>(json);
						if (json == "Success!")
							mLLCL.LastSent = DateTime.Now;
					}
				}

			}
			catch { }
		}

		private static string BuildJson(List<LaserLifeCounter> tosend)
		{
			//return "[ { \"id\": \"5001\", \"type\": \"None\" }, { \"id\": \"5004\", \"type\": \"Maple\" } ]";

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("[ ");



			for (int i = 0; i < tosend.Count; i++)
			{
				LaserLifeCounter LLC = tosend[i];

				sb.Append("{ ");

				sb.Append($"\"Guid\": {EscapeJson(LLC.Guid)},");
				sb.Append($"\"Name\": {EscapeJson(LLC.Name)},");
				sb.Append($"\"Brand\": {EscapeJson(LLC.Brand)},");
				sb.Append($"\"Model\": {EscapeJson(LLC.Model)},");
				sb.Append($"\"OpticalPower\": {EscapeJson(LLC.OpticalPower)},");
				sb.Append($"\"PurchaseDate\": {EscapeJson(LLC.PurchaseDate?.ToString("yyyy-MM-dd HH:mm:ss"))},");
				sb.Append($"\"MonitoringDate\": {EscapeJson(LLC.MonitoringDate?.ToString("yyyy-MM-dd HH:mm:ss"))},");
				sb.Append($"\"DeathDate\": {EscapeJson(LLC.DeathDate?.ToString("yyyy-MM-dd HH:mm:ss"))},");
				sb.Append($"\"LastUsage\": {EscapeJson(LLC.LastUsage?.ToString("yyyy-MM-dd HH:mm:ss"))},");
				sb.Append($"\"TimeInRun\": {EscapeJson(LLC.TimeInRun.TotalHours)},");
				sb.Append($"\"TimeUsageNormalizedPower\": {EscapeJson(LLC.TimeUsageNormalizedPower.TotalHours)},");
				sb.Append($"\"AveragePowerFactor\": {EscapeJson(LLC.AveragePowerFactor)},");

				sb.Append($"\"Classes\": ");
				{
					sb.Append("{ ");

					for (int j = 0; j < LLC.Classes.Length; j++)
					{
						sb.Append($"{LLC.Classes[j].TotalHours.ToString("0.000", NumberFormatInfo.InvariantInfo)}");

						if (j == LLC.Classes.Length - 1) //ultimo
							sb.Append(" } ");
						else
							sb.Append(", ");

					}
				}

				if (i == tosend.Count - 1) //ultimo
					sb.Append(" } ");
				else
					sb.Append(" }, ");
			}

			sb.Append(" ]");
			return sb.ToString();
		}

		static string EscapeJson(object o)
		{
			if (o is double?)
				return $"\"{(o as double?).GetValueOrDefault().ToString("0.000", NumberFormatInfo.InvariantInfo)}\"";
			else if (o is double)
				return $"\"{((double)o).ToString("0.000", NumberFormatInfo.InvariantInfo)}\"";
			else
				return HttpUtility.JavaScriptStringEncode(o != null ? o.ToString() : "", true);
		}

		public class RealDoSendRV
		{
			public int UpdateResult = -1;

			[IgnoreDataMember] public bool Success => UpdateResult == 1;
		}

		private static void RealDoSave(object state)
		{
			try
			{
				lock (mLLCFileName)
				{
					ListLLC tosave = state as ListLLC;
					if (tosave != null)
					{
						try
						{
							if (System.IO.File.Exists(mLLCFileName))
							{
								if (System.IO.File.Exists(mLLCFileName + ".old"))
									System.IO.File.Delete(mLLCFileName + ".old");
								System.IO.File.Move(mLLCFileName, mLLCFileName + ".old");
							}
						}
						catch { }

						try
						{
							if (System.IO.File.Exists(mLLCFileName))
								System.IO.File.Delete(mLLCFileName);
						}
						catch { }

						Serializer.ObjToFile(tosave, mLLCFileName);
					}
				}
			}
			catch { }
		}

		internal static TimeSpan GetCurrentTime()
		{
			return mCurrentLLC != null ? mCurrentLLC.TimeInRun : TimeSpan.Zero;
		}

		internal static void Add(LaserLifeCounter llc)
		{
			lock (mLLCL)
			{
				mLLCL.Add(llc);
				SaveNow();
			}
		}

		internal static void Edit(LaserLifeCounter selected)
		{
			lock (mLLCL)
			{
				foreach (LaserLifeCounter llc in mLLCL)
				{
					if (llc.Guid == selected.Guid)
						llc.Update(selected);
				}
				SaveNow();
			}
		}

		internal static string Delete(LaserLifeCounter selected)
		{
			lock (mLLCL)
			{
				if (mCurrentLLC != null && selected.Guid == mCurrentLLC.Guid)
					return "You cannot delete currently connected Laser module!";

				mLLCL.RemoveAll(l => l.Guid == selected.Guid);
				SaveNow();
				return null;
			}
		}

		internal static void Death(LaserLifeCounter selected)
		{
			lock (mLLCL)
			{
				foreach (LaserLifeCounter llc in mLLCL)
				{
					if (llc.Guid == selected.Guid && llc.DeathDate == null)
						llc.DeathDate = DateTime.Today;
				}
				SaveNow();
			}
		}

		internal static void UnDeath(LaserLifeCounter selected)
		{
			lock (mLLCL)
			{
				foreach (LaserLifeCounter llc in mLLCL)
				{
					if (llc.Guid == selected.Guid && llc.DeathDate != null)
						llc.DeathDate = null;
				}
				SaveNow();
			}
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
