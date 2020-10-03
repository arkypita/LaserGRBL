﻿//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Threading;

namespace LaserGRBL
{
	public partial class MainForm : Form
	{
		private GrblCore Core;
		private bool FirstIdle = true;

		public MainForm()
		{
			InitializeComponent();

			MnOrtur.Visible = false;
			MMn.Renderer = new MMnRenderer();

			splitContainer1.FixedPanel = FixedPanel.Panel1;
			splitContainer1.SplitterDistance = Settings.GetObject("MainForm Splitter Position", 260);
			MnNotifyNewVersion.Checked = Settings.GetObject("Auto Update", true);
			MnNotifyMinorVersion.Checked = Settings.GetObject("Auto Update Build", false);
			MnNotifyPreRelease.Checked = Settings.GetObject("Auto Update Pre", false);

			MnAutoUpdate.DropDown.Closing += MnAutoUpdateDropDown_Closing;

			if (System.Threading.Thread.CurrentThread.Name == null)
				System.Threading.Thread.CurrentThread.Name = "Main Thread";

			using (SplashScreenForm f = new SplashScreenForm())
				f.ShowDialog();

			//build main communication object
			Firmware ftype = Settings.GetObject("Firmware Type", Firmware.Grbl);
			if (ftype == Firmware.Smoothie)
				Core = new SmoothieCore(this, PreviewForm, JogForm);
			else if (ftype == Firmware.Marlin)
				Core = new MarlinCore(this, PreviewForm, JogForm);
			else
				Core = new GrblCore(this, PreviewForm, JogForm);

			ExceptionManager.Core = Core;

			MnGrblConfig.Visible = Core.UIShowGrblConfig;
			MnUnlock.Visible = Core.UIShowUnlockButtons;

			MnGrbl.Text = Core.Type.ToString();

			Core.MachineStatusChanged += OnMachineStatus;
			Core.OnFileLoaded += OnFileLoaded;
			Core.OnOverrideChange += RefreshOverride;
			Core.IssueDetected += OnIssueDetected;

			PreviewForm.SetCore(Core);
			ConnectionForm.SetCore(Core);
			JogForm.SetCore(Core);

			GitHub.NewVersion += GitHub_NewVersion;

			ColorScheme.CurrentScheme = Settings.GetObject("Color Schema", ColorScheme.Scheme.BlueLaser); ;
			RefreshColorSchema(); //include RefreshOverride();
			RefreshFormTitle();
		}

		private void MnAutoUpdateDropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
			{
				e.Cancel = true;
			}
		}

		void OnIssueDetected(GrblCore.DetectedIssue issue)
		{
			if (!Settings.GetObject("Do not show Issue Detector", false))
				IssueDetectorForm.CreateAndShowDialog(issue);
		}

		private void RefreshColorSchema()
		{
			MMn.BackColor = BackColor = StatusBar.BackColor = ColorScheme.FormBackColor;
			MMn.ForeColor = ForeColor = ColorScheme.FormForeColor;
			blueLaserToolStripMenuItem.Checked = ColorScheme.CurrentScheme == ColorScheme.Scheme.BlueLaser;
			redLaserToolStripMenuItem.Checked = ColorScheme.CurrentScheme == ColorScheme.Scheme.RedLaser;
			darkToolStripMenuItem.Checked = ColorScheme.CurrentScheme == ColorScheme.Scheme.Dark;
			hackerToolStripMenuItem.Checked = ColorScheme.CurrentScheme == ColorScheme.Scheme.Hacker;
			nightyToolStripMenuItem.Checked = ColorScheme.CurrentScheme == ColorScheme.Scheme.Nighty;
			ConnectionForm.OnColorChange();
			PreviewForm.OnColorChange();
			RefreshOverride();
		}

		void GitHub_NewVersion(Version current, GitHub.OnlineVersion available, Exception error)
		{
			if (InvokeRequired)
			{
				Invoke(new GitHub.NewVersionDlg(GitHub_NewVersion), current, available, error);
			}
			else
			{
				if (error != null)
					MessageBox.Show(this, "Cannot check for new version, please verify http://lasergrbl.com manually.", "Software info", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
				else if (available != null)
					NewVersionForm.CreateAndShowDialog(current, available, this);
				else
					MessageBox.Show(this, "You have the most updated version!", "Software info", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1); 
			
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			UpdateTimer.Enabled = true;


			if (Settings.GetObject("Auto Update", true))
				GitHub.CheckVersion(false);

			SuspendLayout();
			//restore last size and position
			Object[] msp = Settings.GetObject<Object[]>("Mainform Size and Position", null);
			FormWindowState state = msp == null ? FormWindowState.Maximized : (FormWindowState)msp[2] != FormWindowState.Minimized ? (FormWindowState)msp[2] : FormWindowState.Maximized;
			if (state == FormWindowState.Normal)
			{ WindowState = state; Size = (Size)msp[0]; Location = (Point)msp[1]; }
			ResumeLayout();
		}

		void OnFileLoaded(long elapsed, string filename)
		{
			if (InvokeRequired)
			{
				Invoke(new GrblFile.OnFileLoadedDlg(OnFileLoaded), elapsed, filename);
			}
			else
			{
				TimerUpdate();
				//TTTFile.Text = System.IO.Path.GetFileName(filename);
				TTTLines.Text = Core.LoadedFile.Count.ToString();
				//TTTLoadedIn.Text = elapsed.ToString() + " ms";
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(Core.LoadedFile.EstimatedTime, Tools.Utils.TimePrecision.Second, Tools.Utils.TimePrecision.Second, " ,", true);
			}
		}


		void OnMachineStatus()
		{
			if (Core.MachineStatus == GrblCore.MacStatus.Idle && FirstIdle && Core.Configuration.Count == 0)
			{
				try
				{
					Core.RefreshConfig();
					FirstIdle = false;
				}
				catch { }
			}


			TimerUpdate();
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (Core.InProgram && System.Windows.Forms.MessageBox.Show(Strings.ExitAnyway, Strings.WarnMessageBoxHeader, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
				e.Cancel = true;

			if (!e.Cancel)
			{
				Core.CloseCom(true);
				Settings.SetObject("Mainform Size and Position", new object[] { Size, Location, WindowState });
				Settings.Save();

				UsageStats.SaveFile(Core);
			}
		}


		private void UpdateTimer_Tick(object sender, EventArgs e)
		{
			TimerUpdate();
			ConnectionForm.TimerUpdate();
			PreviewForm.TimerUpdate();
			JogForm.Enabled = Core.JogEnabled;
		}

		private void TimerUpdate()
		{
			SuspendLayout();
			TTTStatus.Text = GrblCore.TranslateEnum(Core.MachineStatus);

			if (Core.InProgram)
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(Core.ProjectedTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second, " ,", true);
			else
				TTTEstimated.Text = Tools.Utils.TimeSpanToString(Core.LoadedFile.EstimatedTime, Tools.Utils.TimePrecision.Minute, Tools.Utils.TimePrecision.Second, " ,", true);

			if (Core.InProgram)
				TTLEstimated.Text = Strings.MainFormProjectedTime;
			else
				TTLEstimated.Text = Strings.MainFormEstimatedTime;

			MnFileOpen.Enabled = Core.CanLoadNewFile;
			MnAdvancedSave.Enabled = MnSaveProgram.Enabled = Core.HasProgram;
			MnFileSend.Enabled = Core.CanSendFile;
			MnStartFromPosition.Enabled = Core.CanSendFile;
			MnGrblConfig.Enabled = true;
			//MnExportConfig.Enabled = Core.CanImportExport;
			//MnImportConfig.Enabled = Core.CanImportExport;
			MnGrblReset.Enabled = Core.CanResetGrbl;

			MNEsp8266.Visible = (Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial)) == ComWrapper.WrapperType.LaserWebESP8266;

			MnConnect.Visible = !Core.IsOpen;
			MnDisconnect.Visible = Core.IsOpen;

			MnGoHome.Visible = Core.Configuration.HomingEnabled;
			MnGoHome.Enabled = Core.CanDoHoming;
			MnUnlock.Enabled = Core.CanUnlock;

			TTOvG0.Visible = Core.SupportOverride;
			TTOvG1.Visible = Core.SupportOverride;
			TTOvS.Visible = Core.SupportOverride;
			spacer.Visible = Core.SupportOverride;


			switch (Core.MachineStatus)
			{
				//Disconnected, Connecting, Idle, *Run, *Hold, *Door, Home, *Alarm, *Check, *Jog

				case GrblCore.MacStatus.Alarm:
					TTTStatus.BackColor = Color.Red;
					TTTStatus.ForeColor = Color.White;
					break;
				case GrblCore.MacStatus.Door:
				case GrblCore.MacStatus.Hold:
				case GrblCore.MacStatus.Cooling:
					TTTStatus.BackColor = Color.DarkOrange;
					TTTStatus.ForeColor = Color.Black;
					break;
				case GrblCore.MacStatus.Jog:
				case GrblCore.MacStatus.Run:
				case GrblCore.MacStatus.Check:
					TTTStatus.BackColor = Color.LightGreen;
					TTTStatus.ForeColor = Color.Black;
					break;
				default:
					TTTStatus.BackColor = ColorScheme.FormBackColor;
					TTTStatus.ForeColor = ColorScheme.FormForeColor;
					break;
			}

			PbBuffer.Maximum = Core.BufferSize;
			PbBuffer.Value = Core.UsedBuffer;
			PbBuffer.ToolTipText = $"Buffer: {Core.UsedBuffer}/{Core.BufferSize} Free:{Core.FreeBuffer}";
			MnOrtur.Visible = Core.IsOrturBoard;

			ResumeLayout();
		}

		private void RefreshFormTitle()
		{
			Version current = typeof(GitHub).Assembly.GetName().Version;
			string FormTitle = string.Format("LaserGRBL v{0}", current.ToString(3));

			if (Core.Type != Firmware.Grbl)
				FormTitle = FormTitle + $" (for {Core.Type})";


			if (Text != FormTitle) Text = FormTitle;
		}

		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}

		private void MnFileOpen_Click(object sender, EventArgs e)
		{
			Core.OpenFile(this);
		}

		private void MnFileSend_Click(object sender, EventArgs e)
		{
			Core.RunProgram();
		}

		private void MnGrblReset_Click(object sender, EventArgs e)
		{
			Core.GrblReset();
		}

		void RefreshOverride()
		{
			SuspendLayout();
			TTOvG0.Text = string.Format("G0 [{0:0.00}x]", Core.OverrideG0 / 100.0);
			TTOvG0.BackColor = Core.OverrideG0 > 100 ? Color.LightPink : (Core.OverrideG0 < 100 ? Color.LightBlue : ColorScheme.FormBackColor);
			TTOvG0.ForeColor = Core.OverrideG0 != 100 ? Color.Black : ColorScheme.FormForeColor;
			TTOvG1.Text = string.Format("G1 [{0:0.00}x]", Core.OverrideG1 / 100.0);
			TTOvG1.BackColor = Core.OverrideG1 > 100 ? Color.LightPink : (Core.OverrideG1 < 100 ? Color.LightBlue : ColorScheme.FormBackColor);
			TTOvG1.ForeColor = Core.OverrideG1 != 100 ? Color.Black : ColorScheme.FormForeColor;
			TTOvS.Text = string.Format("S [{0:0.00}x]", Core.OverrideS / 100.0);
			TTOvS.BackColor = Core.OverrideS > 100 ? Color.LightPink : (Core.OverrideS < 100 ? Color.LightBlue : ColorScheme.FormBackColor);
			TTOvS.ForeColor = Core.OverrideS != 100 ? Color.Black : ColorScheme.FormForeColor;

			ResumeLayout();
		}
		void TTOvClick(object sender, EventArgs e)
		{
			GetOvMenu().Show(Cursor.Position, ToolStripDropDownDirection.AboveLeft);
		}



		internal virtual System.Windows.Forms.ContextMenuStrip GetOvMenu()
		{
			System.Windows.Forms.ContextMenuStrip CM = new System.Windows.Forms.ContextMenuStrip();
			CM.Items.Add(new ToolStripTraceBarItem(Core, 2));
			CM.Items.Add(new ToolStripTraceBarItem(Core, 1));
			CM.Items.Add(new ToolStripTraceBarItem(Core, 0));
			CM.Width = 150;

			return CM;
		}

		/// <summary>
		/// Adds trackbar to toolstrip stuff
		/// </summary>
		[System.Windows.Forms.Design.ToolStripItemDesignerAvailability(System.Windows.Forms.Design.ToolStripItemDesignerAvailability.ToolStrip | System.Windows.Forms.Design.ToolStripItemDesignerAvailability.StatusStrip)]
		public class ToolStripTraceBarItem : System.Windows.Forms.ToolStripControlHost
		{
			public ToolStripTraceBarItem(GrblCore core, int function)
				: base(new UserControls.LabelTB(core, function))
			{
				Control.Dock = System.Windows.Forms.DockStyle.Fill;
			}
		}

		private void MnGoHome_Click(object sender, EventArgs e)
		{
			Core.SendHomingCommand();
		}

		private void MnUnlock_Click(object sender, EventArgs e)
		{
			Core.SendUnlockCommand();
		}

		private void MnConnect_Click(object sender, EventArgs e)
		{
			Core.OpenCom();
		}

		private void MnDisconnect_Click(object sender, EventArgs e)
		{
			if (!(Core.InProgram && System.Windows.Forms.MessageBox.Show(Strings.DisconnectAnyway, Strings.WarnMessageBoxHeader, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
				Core.CloseCom(true);
		}
		void MnSaveProgramClick(object sender, EventArgs e)
		{
			Core.SaveProgram(false, false, false, 1);
		}

		private void MNEnglish_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("en"));
		}

		private void MNItalian_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("it"));
		}

		private void MNSpanish_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("es"));
		}

		private void SetLanguage(System.Globalization.CultureInfo ci)
		{
			if (ci != null)
				Settings.SetObject("User Language", ci);
			else
				Settings.DeleteObject("User Language");

			Settings.Save();

			if (MessageBox.Show(Strings.LanguageRequireRestartNow, Strings.LanguageRequireRestart, MessageBoxButtons.OKCancel) == DialogResult.OK)
				Application.Restart();
		}

		private void helpOnLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Core.HelpOnLine();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://lasergrbl.com/faq/");
		}

		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			Settings.SetObject("MainForm Splitter Position", splitContainer1.SplitterDistance);
			Settings.Save();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SettingsForm.CreateAndShowDialog(Core);
		}

		private void openSessionLogToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Logger.ExistLog)
				Logger.ShowLog();
		}

		private void toolStripMenuItem4_DropDownOpening(object sender, EventArgs e)
		{
			openSessionLogToolStripMenuItem.Enabled = Logger.ExistLog;
			activateExtendedLogToolStripMenuItem.Checked = ComWrapper.ComLogger.Enabled;
		}

		private void MNFrench_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("fr"));
		}

		private void MNGerman_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("de"));
		}

		private void MNDanish_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("da"));
		}

		private void MNBrazilian_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("pt-BR"));
		}

		private void russianToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("ru"));
		}

		private void chineseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("zh-CN"));
		}

		private void slovakToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("sk-SK"));
		}

		private void MNGrblEmulator_Click(object sender, EventArgs e)
		{
			LaserGRBL.GrblEmulator.WebSocketEmulator.Start();
		}

		private void blueLaserToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetSchema(ColorScheme.Scheme.BlueLaser);
		}

		private void redLaserToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetSchema(ColorScheme.Scheme.RedLaser);
		}

		private void darkToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetSchema(ColorScheme.Scheme.Dark);
		}

		private void hackerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetSchema(ColorScheme.Scheme.Hacker);
		}

		private void nightyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetSchema(ColorScheme.Scheme.Nighty);
		}

		private void SetSchema(ColorScheme.Scheme schema)
		{
			Settings.SetObject("Color Schema", schema);
			ColorScheme.CurrentScheme = schema;
			Settings.Save();

			RefreshColorSchema();
		}

		private void grblConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GrblConfig.CreateAndShowDialog(Core);
		}

		private void donateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://paypal.me/pools/c/8cQ1Lo4sRA");
		}


		protected override void OnKeyUp(KeyEventArgs e)
		{
			mLastkeyData = Keys.None;
			Core.ManageHotKeys(Keys.None);
			base.OnKeyUp(e);
		}

		Keys mLastkeyData = Keys.None;
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData != mLastkeyData)
			{
				mLastkeyData = keyData;
				return Core.ManageHotKeys(keyData);
			}
			else
			{
				return base.ProcessCmdKey(ref msg, keyData);
			}
		}

		private void MnReOpenFile_Click(object sender, EventArgs e)
		{
			Core.ReOpenFile(this);
		}

		private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			MnReOpenFile.Enabled = Core.CanReOpenFile;
		}

		private void MnHotkeys_Click(object sender, EventArgs e)
		{
			HotkeyManagerForm.CreateAndShowDialog(Core);
		}


		private void AwakeTimer_Tick(object sender, EventArgs e)
		{
			if (Core.InProgram)
				Tools.WinAPI.SignalActvity();
		}

		private void MnStartFromPosition_Click(object sender, EventArgs e)
		{
			Core.RunProgramFromPosition();
		}

		private void MnFileAppend_Click(object sender, EventArgs e)
		{
			Core.OpenFile(this, null, true);
		}

		private void hungarianToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("hu-HU"));
		}

		private void czechToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("cs-CZ"));
		}

		private void installCH340DriverToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(".\\Driver\\CH341SER.EXE");
		}

		private void flashGrblFirmwareToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FlashGrbl form = new FlashGrbl();
			form.ShowDialog(this);
			if (form.retval != int.MinValue)
			{
				if (form.retval == 0)
					MessageBox.Show("Firmware flashed succesfull!", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else
					MessageBox.Show("Error: cannot flash firmware.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			form.Dispose();

		}

		private void toolsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			flashGrblFirmwareToolStripMenuItem.Enabled = (Core.MachineStatus == GrblCore.MacStatus.Disconnected);
		}

		private void activateExtendedLogToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!ComWrapper.ComLogger.Enabled)
			{
				using (SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog())
				{
					sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
					sfd.Filter = "Communication log|*.txt";
					sfd.AddExtension = true;
					sfd.OverwritePrompt = false;
					sfd.FileName = "comlog.txt";
					sfd.Title = "Select extended log filename";

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

					if (dialogResult == DialogResult.OK && sfd.FileName != null)
						ComWrapper.ComLogger.FileName = sfd.FileName;
				}
			}
			else
			{
				ComWrapper.ComLogger.FileName = null;
			}
		}

		private DispatcherTimer dropDispatcherTimer;
		private string droppedFile;

		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (droppedFile == null)
			{
				if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
			}
		}

		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			if (droppedFile == null)
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (files.Length == 1)
				{
					droppedFile = files[0];

					// call via DispatcherTimer to unblock the source of the drag-event (e.g. Explorer-Window)
					if (dropDispatcherTimer == null)
					{
						this.dropDispatcherTimer = new DispatcherTimer();
						this.dropDispatcherTimer.Interval = TimeSpan.FromSeconds(0.5);
						this.dropDispatcherTimer.Tick += new EventHandler(dropDispatcherTimer_Tick);
					}
					this.dropDispatcherTimer.Start();
				}
			}
		}

		void dropDispatcherTimer_Tick(object sender, EventArgs e)
		{
			if (this.droppedFile != null)
			{
				Core.OpenFile(this, this.droppedFile);
				this.droppedFile = null;
				dropDispatcherTimer.Stop();
			}
		}

		private void MnAdvancedSave_Click(object sender, EventArgs e)
		{
			SaveOptionForm.CreateAndShowDialog(Core);
		}

		private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LicenseForm.CreateAndShowDialog();
		}

		private void MnNotifyNewVersion_Click(object sender, EventArgs e)
		{
			MnNotifyNewVersion.Checked = !MnNotifyNewVersion.Checked;
			Settings.SetObject("Auto Update", MnNotifyNewVersion.Checked);
			Settings.Save();

			//if (MnNotifyNewVersion.Checked)
			//	GitHub.CheckVersion();
		}

		private void MnNotifyNewVersion_CheckedChanged(object sender, EventArgs e)
		{
			if (!MnNotifyNewVersion.Checked) //disabilita il minor update
			{
				MnNotifyMinorVersion.Enabled = false;
				MnNotifyMinorVersion.Checked = false;
				MnNotifyPreRelease.Enabled = false;
				MnNotifyPreRelease.Checked = false;
				Settings.SetObject("Auto Update Build", false);
				Settings.SetObject("Auto Update Pre", false);
			}
			else
			{
				MnNotifyMinorVersion.Enabled = true;
				MnNotifyMinorVersion.Checked = Settings.GetObject("Auto Update Build", false);
				MnNotifyPreRelease.Enabled = true;
				MnNotifyPreRelease.Checked = Settings.GetObject("Auto Update Pre", false);
			}
		}

		private void MnNotifyMinorVersion_Click(object sender, EventArgs e)
		{
			MnNotifyMinorVersion.Checked = !MnNotifyMinorVersion.Checked;
			Settings.SetObject("Auto Update Build", MnNotifyMinorVersion.Checked);
			Settings.Save();

			//if (MnNotifyNewVersion.Checked && MnNotifyMinorVersion.Checked)
			//	GitHub.CheckVersion();
		}

		private void MnNotifyPreRelease_Click(object sender, EventArgs e)
		{
			MnNotifyPreRelease.Checked = !MnNotifyPreRelease.Checked;
			Settings.SetObject("Auto Update Pre", MnNotifyPreRelease.Checked);
			Settings.Save();

			//if (MnNotifyNewVersion.Checked && MnNotifyPreRelease.Checked)
			//	GitHub.CheckVersion();
		}

		private void MnCheckNow_Click(object sender, EventArgs e)
		{
			questionMarkToolStripMenuItem.HideDropDown();
			GitHub.CheckVersion(true);
		}

		private void MnMaterialDB_Click(object sender, EventArgs e)
		{
			PSHelper.PSEditorForm.CreateAndShowDialog();
		}

		private void polishToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("pl-PL"));
		}

		private void orturSupportGroupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://lasergrbl.com/orturfacebook/");
		}

		private void orturWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://lasergrbl.com/orturwebsite/");
		}

		private void traditionalChineseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("zh-TW"));
		}

		private void youtubeChannelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://lasergrbl.com/orturYTchannel/");
		}

		private void firmwareToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://lasergrbl.com/ortur-firmware/");
		}

		private void manualsDownloadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://lasergrbl.com/ortur-manuals/");
		}

        private void MnCreateText_Click(object sender, EventArgs e)
        {
			string fileName = Path.GetTempPath();
			fileName = Path.Combine(fileName, "image_" + DateTime.Now.ToString("yyyyMMdd_ss") + ".bmp");

			using (Bitmap bmp = new Bitmap(200, 100))
			{

				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.DrawString("Write something", new Font(FontFamily.GenericSansSerif, 22), Brushes.Black, 0, 0);
					bmp.Save(fileName);



					RasterConverter.TextToLaserForm form = new RasterConverter.TextToLaserForm(Core, fileName, false);
					form.ShowDialog();
				}
			}
		}
    }


    public class MMnRenderer : ToolStripProfessionalRenderer
	{
		public MMnRenderer() : base(new CustomMenuColor()) { }

		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			Color c = e.Item.Selected ? ColorScheme.MenuHighlightColor : ColorScheme.FormBackColor;
			e.Graphics.Clear(c);
		}

		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
			e.Graphics.Clear(ColorScheme.FormBackColor);
		}

		protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
		{
			e.Graphics.Clear(ColorScheme.FormBackColor);
		}

		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			Color c = e.Item.Enabled ? ColorScheme.FormForeColor : Color.Gray;
			TextRenderer.DrawText(e.Graphics, e.Text, e.TextFont, e.TextRectangle, c, e.TextFormat);
		}

		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			e.Graphics.Clear(ColorScheme.FormBackColor);
		}

		protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
		{
			e.Graphics.Clear(ColorScheme.FormBackColor);
		}

		protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
		{
			e.Graphics.Clear(ColorScheme.FormBackColor);
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			base.OnRenderToolStripBorder(e);

			using (Brush b = new SolidBrush(ColorScheme.FormBackColor))
				e.Graphics.FillRectangle(b, e.ConnectedArea);
		}

	}
	public class CustomMenuColor : ProfessionalColorTable
	{
		public override Color SeparatorDark
		{ get { return ColorScheme.MenuSeparatorColor; } }

		public override Color SeparatorLight
		{ get { return ColorScheme.MenuSeparatorColor; } }
	}
}
