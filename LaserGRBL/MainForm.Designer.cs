namespace LaserGRBL
{
	partial class MainForm
	{
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Liberare le risorse in uso.
		/// </summary>
		/// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Codice generato da Progettazione Windows Form

		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			LaserGRBL.UserControls.DockingManager.DockPanelSkin dockPanelSkin1 = new LaserGRBL.UserControls.DockingManager.DockPanelSkin();
			LaserGRBL.UserControls.DockingManager.AutoHideStripSkin autoHideStripSkin1 = new LaserGRBL.UserControls.DockingManager.AutoHideStripSkin();
			LaserGRBL.UserControls.DockingManager.DockPanelGradient dockPanelGradient1 = new LaserGRBL.UserControls.DockingManager.DockPanelGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient1 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			LaserGRBL.UserControls.DockingManager.DockPaneStripSkin dockPaneStripSkin1 = new LaserGRBL.UserControls.DockingManager.DockPaneStripSkin();
			LaserGRBL.UserControls.DockingManager.DockPaneStripGradient dockPaneStripGradient1 = new LaserGRBL.UserControls.DockingManager.DockPaneStripGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient2 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			LaserGRBL.UserControls.DockingManager.DockPanelGradient dockPanelGradient2 = new LaserGRBL.UserControls.DockingManager.DockPanelGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient3 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			LaserGRBL.UserControls.DockingManager.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new LaserGRBL.UserControls.DockingManager.DockPaneStripToolWindowGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient4 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient5 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			LaserGRBL.UserControls.DockingManager.DockPanelGradient dockPanelGradient3 = new LaserGRBL.UserControls.DockingManager.DockPanelGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient6 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient7 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.StatusBar = new System.Windows.Forms.StatusStrip();
			this.TTLines = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLEstimated = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTEstimated = new System.Windows.Forms.ToolStripStatusLabel();
			this.spring = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTOvG0 = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTOvG1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTOvS = new System.Windows.Forms.ToolStripStatusLabel();
			this.spacer = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.spacer2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
			this.MMn = new System.Windows.Forms.MenuStrip();
			this.grblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MnConnect = new System.Windows.Forms.ToolStripMenuItem();
			this.MnDisconnect = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.MnGrblReset = new System.Windows.Forms.ToolStripMenuItem();
			this.MnGoHome = new System.Windows.Forms.ToolStripMenuItem();
			this.MnUnlock = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.MnExportConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.MnImportConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.MnExit = new System.Windows.Forms.ToolStripMenuItem();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MnFileOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.MnSaveProgram = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.MnFileSend = new System.Windows.Forms.ToolStripMenuItem();
			this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.joggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DockArea = new LaserGRBL.UserControls.DockingManager.DockPanel();
			this.StatusBar.SuspendLayout();
			this.MMn.SuspendLayout();
			this.SuspendLayout();
			// 
			// StatusBar
			// 
			this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.TTLines,
			this.TTLEstimated,
			this.TTTEstimated,
			this.spring,
			this.TTOvG0,
			this.TTOvG1,
			this.TTOvS,
			this.spacer,
			this.TTStatus,
			this.spacer2});
			this.StatusBar.Location = new System.Drawing.Point(0, 459);
			this.StatusBar.Name = "StatusBar";
			this.StatusBar.Size = new System.Drawing.Size(856, 24);
			this.StatusBar.TabIndex = 1;
			this.StatusBar.Text = "statusStrip1";
			// 
			// TTLines
			// 
			this.TTLines.Name = "TTLines";
			this.TTLines.Size = new System.Drawing.Size(46, 19);
			this.TTLines.Text = "Lines: 0";
			// 
			// TTLEstimated
			// 
			this.TTLEstimated.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTLEstimated.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTLEstimated.Name = "TTLEstimated";
			this.TTLEstimated.Size = new System.Drawing.Size(96, 19);
			this.TTLEstimated.Text = "Estimated Time:";
			// 
			// TTTEstimated
			// 
			this.TTTEstimated.Name = "TTTEstimated";
			this.TTTEstimated.Size = new System.Drawing.Size(57, 19);
			this.TTTEstimated.Text = "unknown";
			// 
			// spring
			// 
			this.spring.Name = "spring";
			this.spring.Size = new System.Drawing.Size(506, 19);
			this.spring.Spring = true;
			// 
			// TTOvG0
			// 
			this.TTOvG0.AutoSize = false;
			this.TTOvG0.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTOvG0.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTOvG0.Name = "TTOvG0";
			this.TTOvG0.Size = new System.Drawing.Size(60, 19);
			this.TTOvG0.Text = "G0: 100%";
			this.TTOvG0.Visible = false;
			this.TTOvG0.Click += new System.EventHandler(this.TTOvClick);
			// 
			// TTOvG1
			// 
			this.TTOvG1.AutoSize = false;
			this.TTOvG1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTOvG1.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTOvG1.Name = "TTOvG1";
			this.TTOvG1.Size = new System.Drawing.Size(60, 19);
			this.TTOvG1.Text = "G1: 100%";
			this.TTOvG1.Visible = false;
			this.TTOvG1.Click += new System.EventHandler(this.TTOvClick);
			// 
			// TTOvS
			// 
			this.TTOvS.AutoSize = false;
			this.TTOvS.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTOvS.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTOvS.Name = "TTOvS";
			this.TTOvS.Size = new System.Drawing.Size(55, 19);
			this.TTOvS.Text = "S: 100%";
			this.TTOvS.Visible = false;
			this.TTOvS.Click += new System.EventHandler(this.TTOvClick);
			// 
			// spacer
			// 
			this.spacer.AutoSize = false;
			this.spacer.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.spacer.Name = "spacer";
			this.spacer.Size = new System.Drawing.Size(10, 19);
			// 
			// TTStatus
			// 
			this.TTStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTStatus.Name = "TTStatus";
			this.TTStatus.Size = new System.Drawing.Size(121, 19);
			this.TTStatus.Text = "Status: Disconnected";
			// 
			// spacer2
			// 
			this.spacer2.AutoSize = false;
			this.spacer2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.spacer2.Name = "spacer2";
			this.spacer2.Size = new System.Drawing.Size(5, 19);
			// 
			// UpdateTimer
			// 
			this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
			// 
			// MMn
			// 
			this.MMn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.grblToolStripMenuItem,
			this.fileToolStripMenuItem,
			this.windowsToolStripMenuItem});
			this.MMn.Location = new System.Drawing.Point(0, 0);
			this.MMn.Name = "MMn";
			this.MMn.Size = new System.Drawing.Size(856, 24);
			this.MMn.TabIndex = 2;
			this.MMn.Text = "menuStrip1";
			// 
			// grblToolStripMenuItem
			// 
			this.grblToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.MnConnect,
			this.MnDisconnect,
			this.toolStripMenuItem2,
			this.MnGrblReset,
			this.MnGoHome,
			this.MnUnlock,
			this.toolStripSeparator1,
			this.MnExportConfig,
			this.MnImportConfig,
			this.toolStripMenuItem3,
			this.MnExit});
			this.grblToolStripMenuItem.Name = "grblToolStripMenuItem";
			this.grblToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.grblToolStripMenuItem.Text = "&Grbl";
			// 
			// MnConnect
			// 
			this.MnConnect.Name = "MnConnect";
			this.MnConnect.Size = new System.Drawing.Size(149, 22);
			this.MnConnect.Text = "&Connect";
			this.MnConnect.Click += new System.EventHandler(this.MnConnect_Click);
			// 
			// MnDisconnect
			// 
			this.MnDisconnect.Name = "MnDisconnect";
			this.MnDisconnect.Size = new System.Drawing.Size(149, 22);
			this.MnDisconnect.Text = "&Disconnect";
			this.MnDisconnect.Click += new System.EventHandler(this.MnDisconnect_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(146, 6);
			// 
			// MnGrblReset
			// 
			this.MnGrblReset.Name = "MnGrblReset";
			this.MnGrblReset.Size = new System.Drawing.Size(149, 22);
			this.MnGrblReset.Text = "&Reset";
			this.MnGrblReset.Click += new System.EventHandler(this.MnGrblReset_Click);
			// 
			// MnGoHome
			// 
			this.MnGoHome.Name = "MnGoHome";
			this.MnGoHome.Size = new System.Drawing.Size(149, 22);
			this.MnGoHome.Text = "&Homing";
			this.MnGoHome.Click += new System.EventHandler(this.MnGoHome_Click);
			// 
			// MnUnlock
			// 
			this.MnUnlock.Name = "MnUnlock";
			this.MnUnlock.Size = new System.Drawing.Size(149, 22);
			this.MnUnlock.Text = "&Unlock";
			this.MnUnlock.Click += new System.EventHandler(this.MnUnlock_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(146, 6);
			// 
			// MnExportConfig
			// 
			this.MnExportConfig.Name = "MnExportConfig";
			this.MnExportConfig.Size = new System.Drawing.Size(149, 22);
			this.MnExportConfig.Text = "E&xport Config";
			this.MnExportConfig.Click += new System.EventHandler(this.MnExportConfigClick);
			// 
			// MnImportConfig
			// 
			this.MnImportConfig.Name = "MnImportConfig";
			this.MnImportConfig.Size = new System.Drawing.Size(149, 22);
			this.MnImportConfig.Text = "&Import Config";
			this.MnImportConfig.Click += new System.EventHandler(this.MnImportConfigClick);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(146, 6);
			// 
			// MnExit
			// 
			this.MnExit.Name = "MnExit";
			this.MnExit.Size = new System.Drawing.Size(149, 22);
			this.MnExit.Text = "&Exit";
			this.MnExit.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.MnFileOpen,
			this.MnSaveProgram,
			this.toolStripMenuItem1,
			this.MnFileSend});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// MnFileOpen
			// 
			this.MnFileOpen.Name = "MnFileOpen";
			this.MnFileOpen.Size = new System.Drawing.Size(165, 22);
			this.MnFileOpen.Text = "&Open File";
			this.MnFileOpen.Click += new System.EventHandler(this.MnFileOpen_Click);
			// 
			// MnSaveProgram
			// 
			this.MnSaveProgram.Name = "MnSaveProgram";
			this.MnSaveProgram.Size = new System.Drawing.Size(165, 22);
			this.MnSaveProgram.Text = "&Save Program";
			this.MnSaveProgram.Click += new System.EventHandler(this.MnSaveProgramClick);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(162, 6);
			// 
			// MnFileSend
			// 
			this.MnFileSend.Name = "MnFileSend";
			this.MnFileSend.Size = new System.Drawing.Size(165, 22);
			this.MnFileSend.Text = "Send To &Machine";
			this.MnFileSend.Click += new System.EventHandler(this.MnFileSend_Click);
			// 
			// windowsToolStripMenuItem
			// 
			this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.joggingToolStripMenuItem});
			this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
			this.windowsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
			this.windowsToolStripMenuItem.Text = "&Windows";
			// 
			// joggingToolStripMenuItem
			// 
			this.joggingToolStripMenuItem.Name = "joggingToolStripMenuItem";
			this.joggingToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.joggingToolStripMenuItem.Text = "&Jogging";
			this.joggingToolStripMenuItem.Click += new System.EventHandler(this.joggingToolStripMenuItem_Click);
			// 
			// DockArea
			// 
			this.DockArea.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DockArea.DockBackColor = System.Drawing.SystemColors.Control;
			this.DockArea.Location = new System.Drawing.Point(0, 0);
			this.DockArea.Name = "DockArea";
			this.DockArea.Size = new System.Drawing.Size(856, 483);
			dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
			autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
			tabGradient1.EndColor = System.Drawing.SystemColors.Control;
			tabGradient1.StartColor = System.Drawing.SystemColors.Control;
			tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			autoHideStripSkin1.TabGradient = tabGradient1;
			autoHideStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
			dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
			tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
			dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
			tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
			tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
			tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
			dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
			dockPaneStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
			tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
			tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
			tabGradient5.EndColor = System.Drawing.SystemColors.Control;
			tabGradient5.StartColor = System.Drawing.SystemColors.Control;
			tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
			dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
			dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
			tabGradient6.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient6.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
			tabGradient7.EndColor = System.Drawing.Color.Transparent;
			tabGradient7.StartColor = System.Drawing.Color.Transparent;
			tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
			dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
			dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
			this.DockArea.Skin = dockPanelSkin1;
			this.DockArea.TabIndex = 3;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(856, 483);
			this.Controls.Add(this.StatusBar);
			this.Controls.Add(this.MMn);
			this.Controls.Add(this.DockArea);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.Name = "MainForm";
			this.Text = "Laser GRBL";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.StatusBar.ResumeLayout(false);
			this.StatusBar.PerformLayout();
			this.MMn.ResumeLayout(false);
			this.MMn.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		
		private System.Windows.Forms.StatusStrip StatusBar;
		private System.Windows.Forms.ToolStripStatusLabel TTLines;
		private System.Windows.Forms.ToolStripStatusLabel TTLEstimated;
		private System.Windows.Forms.ToolStripStatusLabel TTTEstimated;
		private System.Windows.Forms.MenuStrip MMn;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MnFileOpen;
		private System.Windows.Forms.ToolStripMenuItem MnFileSend;
		private System.Windows.Forms.ToolStripMenuItem grblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MnGrblReset;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem MnExportConfig;
		private System.Windows.Forms.ToolStripMenuItem MnImportConfig;
		private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem joggingToolStripMenuItem;
		private System.Windows.Forms.Timer UpdateTimer;
		private System.Windows.Forms.ToolStripStatusLabel spring;
		private System.Windows.Forms.ToolStripStatusLabel TTStatus;
		private LaserGRBL.UserControls.DockingManager.DockPanel DockArea;
		private System.Windows.Forms.ToolStripStatusLabel TTOvG0;
		private System.Windows.Forms.ToolStripStatusLabel TTOvG1;
		private System.Windows.Forms.ToolStripStatusLabel TTOvS;
		private System.Windows.Forms.ToolStripStatusLabel spacer;
		private System.Windows.Forms.ToolStripStatusLabel spacer2;

		#endregion
		private System.Windows.Forms.ToolStripMenuItem MnConnect;
		private System.Windows.Forms.ToolStripMenuItem MnDisconnect;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem MnGoHome;
		private System.Windows.Forms.ToolStripMenuItem MnUnlock;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem MnExit;
		private System.Windows.Forms.ToolStripMenuItem MnSaveProgram;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
	}
}

