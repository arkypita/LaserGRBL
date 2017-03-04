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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			LaserGRBL.UserControls.DockingManager.DockPanelSkin dockPanelSkin4 = new LaserGRBL.UserControls.DockingManager.DockPanelSkin();
			LaserGRBL.UserControls.DockingManager.AutoHideStripSkin autoHideStripSkin4 = new LaserGRBL.UserControls.DockingManager.AutoHideStripSkin();
			LaserGRBL.UserControls.DockingManager.DockPanelGradient dockPanelGradient10 = new LaserGRBL.UserControls.DockingManager.DockPanelGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient22 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			LaserGRBL.UserControls.DockingManager.DockPaneStripSkin dockPaneStripSkin4 = new LaserGRBL.UserControls.DockingManager.DockPaneStripSkin();
			LaserGRBL.UserControls.DockingManager.DockPaneStripGradient dockPaneStripGradient4 = new LaserGRBL.UserControls.DockingManager.DockPaneStripGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient23 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			LaserGRBL.UserControls.DockingManager.DockPanelGradient dockPanelGradient11 = new LaserGRBL.UserControls.DockingManager.DockPanelGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient24 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			LaserGRBL.UserControls.DockingManager.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient4 = new LaserGRBL.UserControls.DockingManager.DockPaneStripToolWindowGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient25 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient26 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			LaserGRBL.UserControls.DockingManager.DockPanelGradient dockPanelGradient12 = new LaserGRBL.UserControls.DockingManager.DockPanelGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient27 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			LaserGRBL.UserControls.DockingManager.TabGradient tabGradient28 = new LaserGRBL.UserControls.DockingManager.TabGradient();
			this.StatusBar = new System.Windows.Forms.StatusStrip();
			this.TTLLines = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTLines = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLEstimated = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTEstimated = new System.Windows.Forms.ToolStripStatusLabel();
			this.spring = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTOvG0 = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTOvG1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTOvS = new System.Windows.Forms.ToolStripStatusLabel();
			this.spacer = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTStatus = new System.Windows.Forms.ToolStripStatusLabel();
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
			this.linguaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MNEnglish = new System.Windows.Forms.ToolStripMenuItem();
			this.MNItalian = new System.Windows.Forms.ToolStripMenuItem();
			this.MNSpanish = new System.Windows.Forms.ToolStripMenuItem();
			this.DockArea = new LaserGRBL.UserControls.DockingManager.DockPanel();
			this.StatusBar.SuspendLayout();
			this.MMn.SuspendLayout();
			this.SuspendLayout();
			// 
			// StatusBar
			// 
			this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TTLLines,
            this.TTTLines,
            this.TTLEstimated,
            this.TTTEstimated,
            this.spring,
            this.TTOvG0,
            this.TTOvG1,
            this.TTOvS,
            this.spacer,
            this.TTLStatus,
            this.TTTStatus});
			resources.ApplyResources(this.StatusBar, "StatusBar");
			this.StatusBar.Name = "StatusBar";
			// 
			// TTLLines
			// 
			this.TTLLines.Name = "TTLLines";
			resources.ApplyResources(this.TTLLines, "TTLLines");
			// 
			// TTTLines
			// 
			this.TTTLines.Name = "TTTLines";
			resources.ApplyResources(this.TTTLines, "TTTLines");
			// 
			// TTLEstimated
			// 
			this.TTLEstimated.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTLEstimated.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTLEstimated.Name = "TTLEstimated";
			resources.ApplyResources(this.TTLEstimated, "TTLEstimated");
			// 
			// TTTEstimated
			// 
			this.TTTEstimated.Name = "TTTEstimated";
			resources.ApplyResources(this.TTTEstimated, "TTTEstimated");
			// 
			// spring
			// 
			this.spring.Name = "spring";
			resources.ApplyResources(this.spring, "spring");
			this.spring.Spring = true;
			// 
			// TTOvG0
			// 
			resources.ApplyResources(this.TTOvG0, "TTOvG0");
			this.TTOvG0.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTOvG0.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTOvG0.Name = "TTOvG0";
			this.TTOvG0.Click += new System.EventHandler(this.TTOvClick);
			// 
			// TTOvG1
			// 
			resources.ApplyResources(this.TTOvG1, "TTOvG1");
			this.TTOvG1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTOvG1.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTOvG1.Name = "TTOvG1";
			this.TTOvG1.Click += new System.EventHandler(this.TTOvClick);
			// 
			// TTOvS
			// 
			resources.ApplyResources(this.TTOvS, "TTOvS");
			this.TTOvS.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTOvS.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTOvS.Name = "TTOvS";
			this.TTOvS.Click += new System.EventHandler(this.TTOvClick);
			// 
			// spacer
			// 
			resources.ApplyResources(this.spacer, "spacer");
			this.spacer.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.spacer.Name = "spacer";
			// 
			// TTLStatus
			// 
			this.TTLStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTLStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTLStatus.Name = "TTLStatus";
			resources.ApplyResources(this.TTLStatus, "TTLStatus");
			// 
			// TTTStatus
			// 
			this.TTTStatus.Name = "TTTStatus";
			resources.ApplyResources(this.TTTStatus, "TTTStatus");
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
            this.windowsToolStripMenuItem,
            this.linguaToolStripMenuItem});
			resources.ApplyResources(this.MMn, "MMn");
			this.MMn.Name = "MMn";
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
			resources.ApplyResources(this.grblToolStripMenuItem, "grblToolStripMenuItem");
			// 
			// MnConnect
			// 
			this.MnConnect.Name = "MnConnect";
			resources.ApplyResources(this.MnConnect, "MnConnect");
			this.MnConnect.Click += new System.EventHandler(this.MnConnect_Click);
			// 
			// MnDisconnect
			// 
			this.MnDisconnect.Name = "MnDisconnect";
			resources.ApplyResources(this.MnDisconnect, "MnDisconnect");
			this.MnDisconnect.Click += new System.EventHandler(this.MnDisconnect_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
			// 
			// MnGrblReset
			// 
			this.MnGrblReset.Name = "MnGrblReset";
			resources.ApplyResources(this.MnGrblReset, "MnGrblReset");
			this.MnGrblReset.Click += new System.EventHandler(this.MnGrblReset_Click);
			// 
			// MnGoHome
			// 
			this.MnGoHome.Name = "MnGoHome";
			resources.ApplyResources(this.MnGoHome, "MnGoHome");
			this.MnGoHome.Click += new System.EventHandler(this.MnGoHome_Click);
			// 
			// MnUnlock
			// 
			this.MnUnlock.Name = "MnUnlock";
			resources.ApplyResources(this.MnUnlock, "MnUnlock");
			this.MnUnlock.Click += new System.EventHandler(this.MnUnlock_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// MnExportConfig
			// 
			this.MnExportConfig.Name = "MnExportConfig";
			resources.ApplyResources(this.MnExportConfig, "MnExportConfig");
			this.MnExportConfig.Click += new System.EventHandler(this.MnExportConfigClick);
			// 
			// MnImportConfig
			// 
			this.MnImportConfig.Name = "MnImportConfig";
			resources.ApplyResources(this.MnImportConfig, "MnImportConfig");
			this.MnImportConfig.Click += new System.EventHandler(this.MnImportConfigClick);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
			// 
			// MnExit
			// 
			this.MnExit.Name = "MnExit";
			resources.ApplyResources(this.MnExit, "MnExit");
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
			resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
			// 
			// MnFileOpen
			// 
			this.MnFileOpen.Name = "MnFileOpen";
			resources.ApplyResources(this.MnFileOpen, "MnFileOpen");
			this.MnFileOpen.Click += new System.EventHandler(this.MnFileOpen_Click);
			// 
			// MnSaveProgram
			// 
			this.MnSaveProgram.Name = "MnSaveProgram";
			resources.ApplyResources(this.MnSaveProgram, "MnSaveProgram");
			this.MnSaveProgram.Click += new System.EventHandler(this.MnSaveProgramClick);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			// 
			// MnFileSend
			// 
			this.MnFileSend.Name = "MnFileSend";
			resources.ApplyResources(this.MnFileSend, "MnFileSend");
			this.MnFileSend.Click += new System.EventHandler(this.MnFileSend_Click);
			// 
			// windowsToolStripMenuItem
			// 
			this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.joggingToolStripMenuItem});
			this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
			resources.ApplyResources(this.windowsToolStripMenuItem, "windowsToolStripMenuItem");
			// 
			// joggingToolStripMenuItem
			// 
			this.joggingToolStripMenuItem.Name = "joggingToolStripMenuItem";
			resources.ApplyResources(this.joggingToolStripMenuItem, "joggingToolStripMenuItem");
			this.joggingToolStripMenuItem.Click += new System.EventHandler(this.joggingToolStripMenuItem_Click);
			// 
			// linguaToolStripMenuItem
			// 
			this.linguaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MNEnglish,
            this.MNItalian,
            this.MNSpanish});
			this.linguaToolStripMenuItem.Name = "linguaToolStripMenuItem";
			resources.ApplyResources(this.linguaToolStripMenuItem, "linguaToolStripMenuItem");
			// 
			// MNEnglish
			// 
			this.MNEnglish.Name = "MNEnglish";
			resources.ApplyResources(this.MNEnglish, "MNEnglish");
			this.MNEnglish.Click += new System.EventHandler(this.MNEnglish_Click);
			// 
			// MNItalian
			// 
			this.MNItalian.Name = "MNItalian";
			resources.ApplyResources(this.MNItalian, "MNItalian");
			this.MNItalian.Click += new System.EventHandler(this.MNItalian_Click);
			// 
			// MNSpanish
			// 
			this.MNSpanish.Name = "MNSpanish";
			resources.ApplyResources(this.MNSpanish, "MNSpanish");
			this.MNSpanish.Click += new System.EventHandler(this.MNSpanish_Click);
			// 
			// DockArea
			// 
			resources.ApplyResources(this.DockArea, "DockArea");
			this.DockArea.DockBackColor = System.Drawing.SystemColors.Control;
			this.DockArea.Name = "DockArea";
			dockPanelGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
			autoHideStripSkin4.DockStripGradient = dockPanelGradient10;
			tabGradient22.EndColor = System.Drawing.SystemColors.Control;
			tabGradient22.StartColor = System.Drawing.SystemColors.Control;
			tabGradient22.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			autoHideStripSkin4.TabGradient = tabGradient22;
			autoHideStripSkin4.TextFont = new System.Drawing.Font("Segoe UI", 9F);
			dockPanelSkin4.AutoHideStripSkin = autoHideStripSkin4;
			tabGradient23.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient23.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient23.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient4.ActiveTabGradient = tabGradient23;
			dockPanelGradient11.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient11.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripGradient4.DockStripGradient = dockPanelGradient11;
			tabGradient24.EndColor = System.Drawing.SystemColors.ControlLight;
			tabGradient24.StartColor = System.Drawing.SystemColors.ControlLight;
			tabGradient24.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient4.InactiveTabGradient = tabGradient24;
			dockPaneStripSkin4.DocumentGradient = dockPaneStripGradient4;
			dockPaneStripSkin4.TextFont = new System.Drawing.Font("Segoe UI", 9F);
			tabGradient25.EndColor = System.Drawing.SystemColors.ActiveCaption;
			tabGradient25.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient25.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			tabGradient25.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient4.ActiveCaptionGradient = tabGradient25;
			tabGradient26.EndColor = System.Drawing.SystemColors.Control;
			tabGradient26.StartColor = System.Drawing.SystemColors.Control;
			tabGradient26.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient4.ActiveTabGradient = tabGradient26;
			dockPanelGradient12.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient12.StartColor = System.Drawing.SystemColors.ControlLight;
			dockPaneStripToolWindowGradient4.DockStripGradient = dockPanelGradient12;
			tabGradient27.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient27.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient27.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient27.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient4.InactiveCaptionGradient = tabGradient27;
			tabGradient28.EndColor = System.Drawing.Color.Transparent;
			tabGradient28.StartColor = System.Drawing.Color.Transparent;
			tabGradient28.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient4.InactiveTabGradient = tabGradient28;
			dockPaneStripSkin4.ToolWindowGradient = dockPaneStripToolWindowGradient4;
			dockPanelSkin4.DockPaneStripSkin = dockPaneStripSkin4;
			this.DockArea.Skin = dockPanelSkin4;
			// 
			// MainForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.StatusBar);
			this.Controls.Add(this.MMn);
			this.Controls.Add(this.DockArea);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.IsMdiContainer = true;
			this.Name = "MainForm";
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
		private System.Windows.Forms.ToolStripStatusLabel TTLLines;
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
		private System.Windows.Forms.ToolStripStatusLabel TTLStatus;
		private LaserGRBL.UserControls.DockingManager.DockPanel DockArea;
		private System.Windows.Forms.ToolStripStatusLabel TTOvG0;
		private System.Windows.Forms.ToolStripStatusLabel TTOvG1;
		private System.Windows.Forms.ToolStripStatusLabel TTOvS;
		private System.Windows.Forms.ToolStripStatusLabel spacer;

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
		private System.Windows.Forms.ToolStripStatusLabel TTTLines;
		private System.Windows.Forms.ToolStripStatusLabel TTTStatus;
		private System.Windows.Forms.ToolStripMenuItem linguaToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MNEnglish;
		private System.Windows.Forms.ToolStripMenuItem MNItalian;
		private System.Windows.Forms.ToolStripMenuItem MNSpanish;
	}
}

