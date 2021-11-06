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
using System.Windows.Forms;

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
		}

		public enum MacStatus
		{ Disconnected, Connecting, Idle, Run, Hold, Door, Home, Alarm, Check, Jog, Queue, Cooling }

		public enum JogDirection
		{ None, Abort, Home, N, S, W, E, NW, NE, SW, SE, Zup, Zdown }

		public enum StreamingMode
		{ Buffered, Synchronous, RepeatOnError }

		[Serializable]
		public class GrblVersionInfo : IComparable, ICloneable
		{
			int mMajor;
			int mMinor;
			char mBuild;
			bool mOrtur;
			string mVendorInfo;
			string mVendorVersion;

			public GrblVersionInfo(int major, int minor, char build, string VendorInfo, string VendorVersion)
			{
				mMajor = major; mMinor = minor; mBuild = build;
				mVendorInfo = VendorInfo;
				mVendorVersion = VendorVersion;
				mOrtur = VendorInfo != null && VendorInfo.Contains("Ortur");
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
				return v != null && this.mMajor == v.mMajor && this.mMinor == v.mMinor && this.mBuild == v.mBuild && this.mOrtur == v.mOrtur;
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

			public int Major { get { return mMajor; } }

			public int Minor { get { return mMinor; } }

			public bool IsOrtur { get => mOrtur; internal set => mOrtur = value; }

			public string Vendor
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
		private string mVersionSeen = null;
		protected int mUsedBuffer;
		private int mAutoBufferSize = 127;
		private GPoint mMPos;
		private GPoint mWCO;
		private int mGrblBlocks = -1;
		private int mGrblBuffer = -1;
		private JogDirection mPrenotedJogDirection = JogDirection.None;
		private float mPrenotedJogSpeed = 100;
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

		protected Tools.ElapsedFromEvent debugLastStatusDelay;
		protected Tools.ElapsedFromEvent debugLastMoveOrActivityDelay;

		private ThreadingMode mThreadingMode = ThreadingMode.Fast;
		private HotKeysManager mHotKeyManager;

		public UsageStats.UsageCounters UsageCounters;
 

		public GrblCore(System.Windows.Forms.Control syncroObject, PreviewForm cbform, JogForm jogform)
		{
            if (Type != Firmware.Grbl) Logger.LogMessage("Program", "Load {0} core", Type);

            SetStatus(MacStatus.Disconnected);
			syncro = syncroObject;
			com = new ComWrapper.UsbSerial();

			debugLastStatusDelay = new Tools.ElapsedFromEvent();
			debugLastMoveOrActivityDelay = new Tools.ElapsedFromEvent();

            //with version 4.5.0 default ThreadingMode change from "UltraFast" to "Fast"
            if (!Settings.IsNewFile && Settings.PrevVersion < new Version(4, 5, 0)) 
            {
                ThreadingMode CurrentMode = Settings.GetObject("Threading Mode", ThreadingMode.Fast);
                if (Equals(CurrentMode, ThreadingMode.Insane) || Equals(CurrentMode, ThreadingMode.UltraFast))
                    Settings.SetObject("Threading Mode", ThreadingMode.Fast);
            }

            mThreadingMode = Settings.GetObject("Threading Mode", ThreadingMode.Fast);

            

			QueryTimer = new Tools.PeriodicEventTimer(TimeSpan.FromMilliseconds(mThreadingMode.StatusQuery), false);
			TX = new Tools.ThreadObject(ThreadTX, 1, true, "Serial TX Thread", StartTX);
			RX = new Tools.ThreadObject(ThreadRX, 1, true, "Serial RX Thread", null);

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
					Settings.SetObject("Grbl Configuration", value);
			}
		}

		protected void SetStatus(MacStatus newStatus)
		{
			lock (this)
			{
				if (mMachineStatus != newStatus)
				{
					MacStatus oldStatus = mMachineStatus;
					mMachineStatus = newStatus;

					Logger.LogMessage("SetStatus", "Machine status [{0}]", mMachineStatus);

					if (oldStatus == MacStatus.Connecting && newStatus != MacStatus.Disconnected)
						RefreshConfigOnConnect();

					if (oldStatus == MacStatus.Connecting && newStatus != MacStatus.Disconnected)
						SoundEvent.PlaySound(SoundEvent.EventId.Connect);
					if (newStatus == MacStatus.Disconnected)
						SoundEvent.PlaySound(SoundEvent.EventId.Disconnect);

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

		private void RefreshConfigOnConnect()
		{
			try {System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(RefreshConfigOnConnect)); }
			catch{ }
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
					CSVD.LoadAppropriateSettings(value);
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

		public static readonly System.Collections.Generic.List<string> ImageExtensions = new System.Collections.Generic.List<string>(new string[] { ".jpg", ".bmp", ".png", ".gif" });
		public static readonly System.Collections.Generic.List<string> GCodeExtensions = new System.Collections.Generic.List<string>(new string[] { ".nc", ".cnc", ".tap", ".gcode", ".ngc" });
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
							dialogResult = ofd.ShowDialog(parent);
						}
						catch (System.Runtime.InteropServices.COMException)
						{
							ofd.AutoUpgradeEnabled = false;
							dialogResult = ofd.ShowDialog(parent);
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
								RasterConverter.RasterToLaserForm.CreateAndShowDialog(this, bmpname, parent, append);
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

		public void SaveProgram(System.Windows.Forms.Form parent, bool header, bool footer, bool between, int cycles)
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
						dialogResult = sfd.ShowDialog(parent);
					}
					catch (System.Runtime.InteropServices.COMException)
					{
						sfd.AutoUpgradeEnabled = false;
						dialogResult = sfd.ShowDialog(parent);
					}

					if (dialogResult == System.Windows.Forms.DialogResult.OK)
						filename = sfd.FileName;
				}

				if (filename != null)
					file.SaveProgram(filename, header, footer, between, cycles);
			}
		}

		private void RefreshConfigOnConnect(object state) //da usare per la chiamata asincrona
		{
			try { RefreshConfig(); }
			catch { }
		}

		public virtual void RefreshConfig()
		{
			if (CanReadWriteConfig)
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
			if (CanReadWriteConfig)
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
			if (CanSendFile)
			{
				if (mTP.Executed == 0 || mTP.Executed == mTP.Target) //mai iniziato oppure correttamente finito
					RunProgramFromStart(false, true);
				else
					UserWantToContinue(parent);
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
			int position = LaserGRBL.ResumeJobForm.CreateAndShowDialog(parent, mTP.Executed, mTP.Sent, mTP.Target, mTP.LastIssue, Configuration.HomingEnabled, homing, out homing, setwco, setwco, out setwco, mTP.LastKnownWCO);

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
					OnJobBegin();

				if (pass)
					OnJobCycle();


				Logger.LogMessage("EnqueueProgram", "Push File, {0} lines", file.Count);
				foreach (GrblCommand cmd in file)
					mQueuePtr.Enqueue(cmd.Clone() as GrblCommand);

				mTP.JobStart(LoadedFile, mQueuePtr);
				Logger.LogMessage("EnqueueProgram", "Running program, {0} lines", file.Count);
			}
		}

		private void OnJobCycle()
		{
			Logger.LogMessage("EnqueueProgram", "Push Passes");
			ExecuteCustombutton(Settings.GetObject("GCode.CustomPasses", GrblCore.GCODE_STD_PASSES));
		}

		protected virtual void OnJobBegin()
		{
			Logger.LogMessage("EnqueueProgram", "Push Header");
			ExecuteCustombutton(Settings.GetObject("GCode.CustomHeader", GrblCore.GCODE_STD_HEADER));
		}

		protected virtual void OnJobEnd()
		{
			Logger.LogMessage("EnqueueProgram", "Push Footer");
			ExecuteCustombutton(Settings.GetObject("GCode.CustomFooter", GrblCore.GCODE_STD_FOOTER));
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
				mAutoBufferSize = 127; //reset to default buffer size
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

		public bool IsConnected => mMachineStatus != MacStatus.Disconnected && mMachineStatus != MacStatus.Connecting;

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
				mUsedBuffer = 0;
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

		public System.Collections.Generic.List<IGrblRow> SentCommand(int index, int count)
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

        internal void EnqueueZJog(JogDirection dir, decimal step, bool fast)
        {
            if (JogEnabled)
            {
				mPrenotedJogSpeed = (fast ? 100000 : JogSpeed);

				if (SupportTrueJogging)
                    DoJogV11(dir, step);
                else
                    EmulateJogV09(dir, step); //immediato
            }
        }

		public void BeginJog(PointF target, bool fast)
		{
			if (JogEnabled)
			{
				mPrenotedJogSpeed = (fast ? 100000 : JogSpeed);
				target = LimitToBound(target);

				if (SupportTrueJogging)
					DoJogV11(target);
				else
					EmulateJogV09(target);
			}
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

		public void BeginJog(JogDirection dir, bool fast) //da chiamare su ButtonDown
		{
			if (JogEnabled)
			{
				mPrenotedJogSpeed = (fast ? 100000 : JogSpeed);

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
                EnqueueCommand(new GrblCommand(string.Format("G0X0Y0F{0}", mPrenotedJogSpeed)));
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

                cmd += $"F{mPrenotedJogSpeed}";

				EnqueueCommand(new GrblCommand("G91"));
				EnqueueCommand(new GrblCommand(cmd));
				EnqueueCommand(new GrblCommand("G90"));
			}
		}

		private void EmulateJogV09(PointF target) //emulate jog using plane G-Code
		{
			string cmd = "G0";

			cmd += $"X{target.X.ToString("0.00", NumberFormatInfo.InvariantInfo)}";
			cmd += $"Y{target.Y.ToString("0.00", NumberFormatInfo.InvariantInfo)}";
			cmd += $"F{mPrenotedJogSpeed}";

			EnqueueCommand(new GrblCommand("G90"));
			EnqueueCommand(new GrblCommand(cmd));
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
                    EnqueueCommand(new GrblCommand(string.Format("$J=G90X0Y0F{0}", mPrenotedJogSpeed)));
                else
                    EnqueueCommand(GetRelativeJogCommandv11(dir, step));
            }
		}

		private void DoJogV11(PointF target)
		{
			mPrenotedJogDirection = JogDirection.None;
			SendImmediate(0x85); //abort previous jog
			EnqueueCommand(new GrblCommand(string.Format("$J=G90X{0}Y{1}F{2}", target.X.ToString("0.00", NumberFormatInfo.InvariantInfo), target.Y.ToString("0.00", NumberFormatInfo.InvariantInfo), mPrenotedJogSpeed)));
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

            cmd += $"F{mPrenotedJogSpeed}";
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
			cmd += $"F{mPrenotedJogSpeed}";
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
                    EnqueueCommand(new GrblCommand(string.Format("$J=G90X0Y0F{0}", mPrenotedJogSpeed)));
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

		// this function try to detect and unlock from a "buffer stuck" condition
		// a "buffer stuck" condition occurs when LaserGRBL does not receive some "ok's" 
		// back from grbl (i.e. because of electrical noise on wire) and so LaserGRBL
		// does no longer send commands anymore because think the buffer is full
		// this feature can work only if $10=3 (status report with buffer size report enabled)
		private void HandleMissingOK() 
		{
			if (HasPendingCommands() && !BufferIsFree() && MachineSayBufferFree() && MachineNotMovingOrReply() && MachineStatus == MacStatus.Run)
				CreateFakeOK(mPending.Count); //rispondi "ok" a tutti i comandi pending
		}

		private void CreateFakeOK(int count)
		{
			mSentPtr.Add(new GrblMessage("Unlock from buffer stuck!", false));

			ComWrapper.ComLogger.Log("com", $"Handle Missing OK [{count}]");
			Logger.LogMessage("Issue detector", "Handle Missing OK [{0}]", count);

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
			else if (IsVigoWelcomeMessage(rline))
				ManageVigoWelcomeMessage(rline);
			else if (IsOrturModelMessage(rline))
				ManageOrturModelMessage(rline);
			else if (IsOrturFirmwareMessage(rline))
				ManageOrturFirmwareMessage(rline);
			else if (IsStandardWelcomeMessage(rline))
				ManageStandardWelcomeMessage(rline);
			else if (IsBrokenOkMessage(rline))
				ManageBrokenOkMessage(rline);
			else
				ManageGenericMessage(rline);
		}

		private bool IsCommandReplyMessage(string rline) => rline.ToLower().StartsWith("ok") || rline.ToLower().StartsWith("error");
		private bool IsRealtimeStatusMessage(string rline) => rline.StartsWith("<") && rline.EndsWith(">");
		private bool IsVigoWelcomeMessage(string rline) => rline.StartsWith("Grbl-Vigo");
		private bool IsOrturModelMessage(string rline) => rline.StartsWith("Ortur ");
		private bool IsOrturFirmwareMessage(string rline) => rline.StartsWith("OLF");
		private bool IsStandardWelcomeMessage(string rline) => rline.StartsWith("Grbl");
		private bool IsBrokenOkMessage(string rline) => rline.ToLower().Contains("ok");

		private void ManageGenericMessage(string rline)
		{
			try { mSentPtr.Add(new GrblMessage(rline, SupportCSV)); }
			catch (Exception ex)
			{
				Logger.LogMessage("GenericMessage", "Ex on [{0}] message", rline);
				Logger.LogException("GenericMessage", ex);
			}
		}

		private void ManageStandardWelcomeMessage(string rline)
		{
			//Grbl vX.Xx ['$' for help]
			try
			{
				int maj = int.Parse(rline.Substring(5, 1));
				int min = int.Parse(rline.Substring(7, 1));
				char build = rline.Substring(8, 1).ToCharArray()[0];
				GrblVersion = new GrblVersionInfo(maj, min, build, mWelcomeSeen, mVersionSeen);

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
				int maj = int.Parse(rline.Substring(10, 1));
				int min = int.Parse(rline.Substring(12, 1));
				char build = rline.Substring(13, 1).ToCharArray()[0];
				string VendorVersion = rline.Split(':')[2];
				GrblVersion = new GrblVersionInfo(maj, min, build, "Vigotec", VendorVersion);
				Logger.LogMessage("VigoInfo", "Detected {0}", VendorVersion);

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
				mWelcomeSeen = rline;
				mWelcomeSeen = mWelcomeSeen.Replace("Ready", "");
				mWelcomeSeen = mWelcomeSeen.Replace("!", "");
				mWelcomeSeen = mWelcomeSeen.Trim();
				Logger.LogMessage("OrturInfo", "Detected {0}", mWelcomeSeen);
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
				mVersionSeen = rline;
				mVersionSeen = mVersionSeen.Replace("OLF", "");
				mVersionSeen = mVersionSeen.Trim('.');
				mVersionSeen = mVersionSeen.Trim();
				Logger.LogMessage("OrturInfo", "Detected OLF {0}", mVersionSeen);
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

		private void ManageRealTimeStatus(string rline)
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
			SetWCO(new GPoint(ParseFloat(xyz,0), ParseFloat(xyz,1), ParseFloat(xyz,2)));
		}

		private void ParseWPos(string p)
		{
			string wpos = p.Substring(5, p.Length - 5);
			string[] xyz = wpos.Split(",".ToCharArray());
			SetMPosition(mWCO + new GPoint(ParseFloat(xyz,0), ParseFloat(xyz,1), ParseFloat(xyz,2)));
		}

		private void ParseMPos(string p)
		{
			string mpos = p.Substring(5, p.Length - 5);
			string[] xyz = mpos.Split(",".ToCharArray());
			SetMPosition(new GPoint(ParseFloat(xyz,0), ParseFloat(xyz,1), ParseFloat(xyz,2)));
		}

		protected static float ParseFloat(string value)
		{
			return float.Parse(value, NumberFormatInfo.InvariantInfo);
		}

		protected static float ParseFloat(string [] arr, int idx, float defval = 0.0f)
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

			EnlargeBuffer(mGrblBuffer);
		}

		private void EnlargeBuffer(int mGrblBuffer)
		{
			if (BufferSize == 127) //act only to change default value at first event, do not re-act without a new connect
			{
				if (mGrblBuffer == 128) //Grbl v1.1 with enabled buffer report
					mAutoBufferSize = 128;
                else if (mGrblBuffer == 255) //Grbl-Mega fixed
					mAutoBufferSize = 255;
                else if (mGrblBuffer == 256) //Grbl-Mega
					mAutoBufferSize = 256;
				else if (mGrblBuffer == 10240) //Grbl-LPC
					mAutoBufferSize = 10240;
				else if (mGrblBuffer == 254) //Ortur
					mAutoBufferSize = 254;
			}
		}

		private void ParseFS(string p)
		{
			string sfs = p.Substring(3, p.Length - 3);
			string[] fs = sfs.Split(",".ToCharArray());
			SetFS(ParseFloat(fs,0), ParseFloat(fs,1));
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
			mSentPtr.Add(new GrblMessage("Handle broken ok!", false));
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
						Configuration.AddOrUpdate(pending.GetMessage());

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
			if (data.Contains(":"))
				data = data.Substring(0, data.IndexOf(':'));

			MacStatus var = (MacStatus)Enum.Parse(typeof(MacStatus), data);

			if (InProgram && var == MacStatus.Idle) //bugfix for grbl sending Idle on G4
				var = MacStatus.Run;

			if (var == MacStatus.Hold && !mHoldByUserRequest)
				var = MacStatus.Cooling;

			SetStatus(var);
		}

		
		protected virtual void ForceStatusIdle() {} // Used by Marlin to update status to Idle (As Marlin has no immediate message)

		private void OnProgramEnd()
		{
			if (mTP.JobEnd() && mLoopCount > 1 && mMachineStatus != MacStatus.Check)
			{
				Logger.LogMessage("CycleEnd", "Cycle Executed: {0} lines, {1} errors, {2}", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(mTP.TotalJobTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second, ",", true));
				mSentPtr.Add(new GrblMessage(string.Format("[{0} lines, {1} errors, {2}]", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(mTP.TotalJobTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second, ",", true)), false));

				LoopCount--;
				RunProgramFromStart(false, false, true);
			}
			else
			{
				Logger.LogMessage("ProgramEnd", "Job Executed: {0} lines, {1} errors, {2}", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(mTP.TotalJobTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second, ",", true));
				mSentPtr.Add(new GrblMessage(string.Format("[{0} lines, {1} errors, {2}]", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(mTP.TotalJobTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second, ",", true)), false));

				OnJobEnd();

				SoundEvent.PlaySound(SoundEvent.EventId.Success);
				Telegram.NotifyEvent(String.Format("<b>Job Executed</b>\n{0} lines, {1} errors\nTime: {2}", file.Count, mTP.ErrorCount, Tools.Utils.TimeSpanToString(mTP.TotalJobTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second, ",", true)));

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
		{ get { return IsConnected && HasProgram && IdleOrCheck; } }

		public bool CanAbortProgram
		{ get { return IsConnected && HasProgram && (MachineStatus == MacStatus.Run || MachineStatus == MacStatus.Hold || MachineStatus == MacStatus.Cooling); } }

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
		{ get { return IsConnected && (MachineStatus == MacStatus.Door || MachineStatus == MacStatus.Hold || MachineStatus == MacStatus.Cooling); } }

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


		internal bool ManageHotKeys(Form parent,  System.Windows.Forms.Keys keys)
		{
			if (SuspendHK)
				return false;
			else
				return mHotKeyManager.ManageHotKeys(parent, keys);
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
			Settings.SetObject("Hotkey Setup", mHotKeyManager);
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
					{
						byte b = GetImmediate(tosend);
						if (b == '!') mHoldByUserRequest = true;
						SendImmediate(b);
					}
					else
					{ EnqueueCommand(new GrblCommand(tosend)); }
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
				exp.AddSetVariable("WCO.X", (double)WorkingOffset.X);
				exp.AddSetVariable("WCO.Y", (double)WorkingOffset.Y);
				exp.AddSetVariable("WCO.Z", (double)WorkingOffset.Z);
				exp.AddSetVariable("MPos.X", (double)MachinePosition.X);
				exp.AddSetVariable("MPos.Y", (double)MachinePosition.Y);
				exp.AddSetVariable("MPos.Z", (double)MachinePosition.Z);
				exp.AddSetVariable("WPos.X", (double)MachinePosition.X);
				exp.AddSetVariable("WPos.Y", (double)MachinePosition.Y);
				exp.AddSetVariable("WPos.Z", (double)MachinePosition.Z);

				GrblConf conf = Configuration;
				if (conf != null)
				{
					foreach (KeyValuePair<int, decimal> p in conf)
						exp.AddSetVariable("$" + p.Key, (double)p.Value);
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

        GrblCore.DetectedIssue mLastIssue;
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
				if (mStart == 0) mStart = Tools.HiResTimer.TotalMilliseconds;

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
	public class GrblConf : IEnumerable<System.Collections.Generic.KeyValuePair<int, decimal>>
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
