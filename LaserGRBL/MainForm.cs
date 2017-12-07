using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class MainForm : Form
	{
		private GrblCore Core;
		private bool FirstIdle = true;

		public MainForm()
		{
			InitializeComponent();

			MMn.Renderer = new MMnRenderer();

			splitContainer1.FixedPanel = FixedPanel.Panel1;
			splitContainer1.SplitterDistance = (int)Settings.GetObject("MainForm Splitter Position", 260);
			autoUpdateToolStripMenuItem.Checked = (bool)Settings.GetObject("Auto Update", true);

			if (System.Threading.Thread.CurrentThread.Name == null)
				System.Threading.Thread.CurrentThread.Name = "Main Thread";
			
			using (SplashScreenForm f = new SplashScreenForm())
				f.ShowDialog();

			//build main communication object
			Core = new GrblCore(this);
			Core.MachineStatusChanged += OnMachineStatus;
			Core.OnFileLoaded += OnFileLoaded;
			Core.OnOverrideChange += RefreshOverride;
			Core.IssueDetected += OnIssueDetected;

			PreviewForm.SetCore(Core);
			ConnectionForm.SetCore(Core);
			JogForm.SetCore(Core);

			GitHub.NewVersion += GitHub_NewVersion;

			ColorScheme.CurrentScheme = (ColorScheme.Scheme)Settings.GetObject("Color Schema", ColorScheme.Scheme.BlueLaser); ;
			RefreshColorSchema(); //include RefreshOverride();
			RefreshFormTitle();
		}

		void OnIssueDetected(GrblCore.DetectedIssue issue)
		{
			if (!(bool)Settings.GetObject("Do not show Issue Detector", false))
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
			ConnectionForm.OnColorChange();
			PreviewForm.OnColorChange();
			RefreshOverride();
		}

		void GitHub_NewVersion(Version current, Version latest, string name, string url)
		{
			if (InvokeRequired)
			{
				Invoke(new GitHub.NewVersionDlg(GitHub_NewVersion), current, latest, name, url);
			}
			else
			{
				NewVersionForm.CreateAndShowDialog(current, latest, name, url, this);
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			UpdateTimer.Enabled = true;
			GitHub.CheckVersion();

			SuspendLayout();
			//restore last size and position
			Object[] msp = (Object[])Settings.GetObject("Mainform Size and Position", null);
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
			if (Core.InProgram && System.Windows.Forms.MessageBox.Show(Strings.ExitAnyway, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
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
			MnSaveProgram.Enabled = Core.HasProgram;
			MnFileSend.Enabled = Core.CanSendFile;
			MnGrblConfig.Enabled = true;
			//MnExportConfig.Enabled = Core.CanImportExport;
			//MnImportConfig.Enabled = Core.CanImportExport;
			MnGrblReset.Enabled = Core.CanResetGrbl;

			MNEsp8266.Visible = ((ComWrapper.WrapperType)Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial)) == ComWrapper.WrapperType.LaserWebESP8266;

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

			ResumeLayout();
		}

		private void RefreshFormTitle()
		{
			Version current = typeof(GitHub).Assembly.GetName().Version;
			string FormTitle = string.Format("LaserGRBL v{0}", current.ToString(3));
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
			TTOvS.BackColor = Core.OverrideS > 100 ? Color.LightPink : (Core.OverrideS < 100 ? Color.LightBlue : ColorScheme.FormBackColor) ;
			TTOvS.ForeColor = Core.OverrideS != 100 ? Color.Black : ColorScheme.FormForeColor;

			ResumeLayout();
		}
		void TTOvClick(object sender, EventArgs e)
		{
			GetOvMenu().Show(Cursor.Position,ToolStripDropDownDirection.AboveLeft);
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
			Core.EnqueueCommand(new GrblCommand("$H"));
		}

		private void MnUnlock_Click(object sender, EventArgs e)
		{
			Core.EnqueueCommand(new GrblCommand("$X"));
		}

		private void MnConnect_Click(object sender, EventArgs e)
		{
			Core.OpenCom();
		}

		private void MnDisconnect_Click(object sender, EventArgs e)
		{
			if (!(Core.InProgram && System.Windows.Forms.MessageBox.Show(Strings.DisconnectAnyway, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes))
				Core.CloseCom(true);
		}
		void MnSaveProgramClick(object sender, EventArgs e)
		{
			Core.SaveProgram();
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

			if (System.Windows.Forms.MessageBox.Show(Strings.LanguageRequireRestartNow, Strings.LanguageRequireRestart, MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
				Application.Restart();
		}

		private void helpOnLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Core.HelpOnLine();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/");
		}

		private void autoUpdateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			autoUpdateToolStripMenuItem.Checked = !autoUpdateToolStripMenuItem.Checked;
			Settings.SetObject("Auto Update", autoUpdateToolStripMenuItem.Checked);
			Settings.Save();

			if (autoUpdateToolStripMenuItem.Checked)
				GitHub.CheckVersion();
		}

		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			Settings.SetObject("MainForm Splitter Position", splitContainer1.SplitterDistance);
			Settings.Save();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SettingsForm.CreateAndShowDialog();
		}

		private void openSessionLogToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Logger.ExistLog)
				Logger.ShowLog();
		}

		private void toolStripMenuItem4_DropDownOpening(object sender, EventArgs e)
		{
			openSessionLogToolStripMenuItem.Enabled = Logger.ExistLog;
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

		private void MNGrblEmulator_Click(object sender, EventArgs e)
		{
			LaserGRBL.GrblEmulator.WebSocketEmulator.Start();
		}

		private void chinexeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetLanguage(new System.Globalization.CultureInfo("zh-CN"));
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
			System.Diagnostics.Process.Start(@"https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=mlpita%40bergamo3%2eit&lc=US&item_name=LaserGRBL&item_number=Support%20development&currency_code=EUR");
		}


		protected override void OnKeyUp(KeyEventArgs e)
		{
			mLastkeyData = Keys.None;
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
		{get{return ColorScheme.MenuSeparatorColor;}}

		public override Color SeparatorLight
		{get{return ColorScheme.MenuSeparatorColor;}}
	}
}
