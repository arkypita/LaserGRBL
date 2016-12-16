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
			this.TTLFile = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTFile = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLLines = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTLines = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLLoadedIn = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTLoadedIn = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLEstimated = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTEstimated = new System.Windows.Forms.ToolStripStatusLabel();
			this.spring = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
			this.MMn = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MnFileOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.MnFileSend = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.grblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MnGrblReset = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.MnExportConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.MnImportConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.joggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.overridesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DockArea = new LaserGRBL.UserControls.DockingManager.DockPanel();
			this.StatusBar.SuspendLayout();
			this.MMn.SuspendLayout();
			this.SuspendLayout();
			// 
			// StatusBar
			// 
			this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TTLFile,
            this.TTTFile,
            this.TTLLines,
            this.TTTLines,
            this.TTLLoadedIn,
            this.TTTLoadedIn,
            this.TTLEstimated,
            this.TTTEstimated,
            this.spring,
            this.TTLStatus,
            this.TTTStatus});
			this.StatusBar.Location = new System.Drawing.Point(0, 459);
			this.StatusBar.Name = "StatusBar";
			this.StatusBar.Size = new System.Drawing.Size(856, 24);
			this.StatusBar.TabIndex = 1;
			this.StatusBar.Text = "statusStrip1";
			// 
			// TTLFile
			// 
			this.TTLFile.Name = "TTLFile";
			this.TTLFile.Size = new System.Drawing.Size(28, 19);
			this.TTLFile.Text = "File:";
			// 
			// TTTFile
			// 
			this.TTTFile.Name = "TTTFile";
			this.TTTFile.Size = new System.Drawing.Size(0, 19);
			// 
			// TTLLines
			// 
			this.TTLLines.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTLLines.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTLLines.Name = "TTLLines";
			this.TTLLines.Size = new System.Drawing.Size(41, 19);
			this.TTLLines.Text = "Lines:";
			// 
			// TTTLines
			// 
			this.TTTLines.Name = "TTTLines";
			this.TTTLines.Size = new System.Drawing.Size(13, 19);
			this.TTTLines.Text = "0";
			// 
			// TTLLoadedIn
			// 
			this.TTLLoadedIn.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTLLoadedIn.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTLLoadedIn.Name = "TTLLoadedIn";
			this.TTLLoadedIn.Size = new System.Drawing.Size(66, 19);
			this.TTLLoadedIn.Text = "Loaded in:";
			// 
			// TTTLoadedIn
			// 
			this.TTTLoadedIn.Name = "TTTLoadedIn";
			this.TTTLoadedIn.Size = new System.Drawing.Size(32, 19);
			this.TTTLoadedIn.Text = "0 ms";
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
			this.spring.Size = new System.Drawing.Size(383, 19);
			this.spring.Spring = true;
			// 
			// TTLStatus
			// 
			this.TTLStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTLStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTLStatus.Name = "TTLStatus";
			this.TTLStatus.Size = new System.Drawing.Size(46, 19);
			this.TTLStatus.Text = "Status:";
			// 
			// TTTStatus
			// 
			this.TTTStatus.Name = "TTTStatus";
			this.TTTStatus.Size = new System.Drawing.Size(79, 19);
			this.TTTStatus.Text = "Disconnected";
			// 
			// UpdateTimer
			// 
			this.UpdateTimer.Enabled = true;
			this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
			// 
			// MMn
			// 
			this.MMn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.grblToolStripMenuItem,
            this.windowsToolStripMenuItem});
			this.MMn.Location = new System.Drawing.Point(0, 0);
			this.MMn.Name = "MMn";
			this.MMn.Size = new System.Drawing.Size(856, 24);
			this.MMn.TabIndex = 2;
			this.MMn.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnFileOpen,
            this.MnFileSend,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// MnFileOpen
			// 
			this.MnFileOpen.Name = "MnFileOpen";
			this.MnFileOpen.Size = new System.Drawing.Size(103, 22);
			this.MnFileOpen.Text = "&Open";
			this.MnFileOpen.Click += new System.EventHandler(this.MnFileOpen_Click);
			// 
			// MnFileSend
			// 
			this.MnFileSend.Name = "MnFileSend";
			this.MnFileSend.Size = new System.Drawing.Size(103, 22);
			this.MnFileSend.Text = "&Send";
			this.MnFileSend.Click += new System.EventHandler(this.MnFileSend_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.exitToolStripMenuItem.Text = "&Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
			// 
			// grblToolStripMenuItem
			// 
			this.grblToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnGrblReset,
            this.toolStripSeparator1,
            this.MnExportConfig,
            this.MnImportConfig});
			this.grblToolStripMenuItem.Name = "grblToolStripMenuItem";
			this.grblToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.grblToolStripMenuItem.Text = "&Grbl";
			// 
			// MnGrblReset
			// 
			this.MnGrblReset.Name = "MnGrblReset";
			this.MnGrblReset.Size = new System.Drawing.Size(149, 22);
			this.MnGrblReset.Text = "&Reset";
			this.MnGrblReset.Click += new System.EventHandler(this.MnGrblReset_Click);
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
			this.MnExportConfig.Text = "&Export Config";
			this.MnExportConfig.Click += new System.EventHandler(this.MnExportConfigClick);
			// 
			// MnImportConfig
			// 
			this.MnImportConfig.Name = "MnImportConfig";
			this.MnImportConfig.Size = new System.Drawing.Size(149, 22);
			this.MnImportConfig.Text = "&Import Config";
			this.MnImportConfig.Click += new System.EventHandler(this.MnImportConfigClick);
			// 
			// windowsToolStripMenuItem
			// 
			this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.joggingToolStripMenuItem,
            this.overridesToolStripMenuItem});
			this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
			this.windowsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
			this.windowsToolStripMenuItem.Text = "&Windows";
			// 
			// joggingToolStripMenuItem
			// 
			this.joggingToolStripMenuItem.Name = "joggingToolStripMenuItem";
			this.joggingToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.joggingToolStripMenuItem.Text = "&Jogging";
			this.joggingToolStripMenuItem.Click += new System.EventHandler(this.joggingToolStripMenuItem_Click);
			// 
			// overridesToolStripMenuItem
			// 
			this.overridesToolStripMenuItem.Name = "overridesToolStripMenuItem";
			this.overridesToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.overridesToolStripMenuItem.Text = "&Overrides";
			this.overridesToolStripMenuItem.Click += new System.EventHandler(this.overridesToolStripMenuItem_Click);
			// 
			// DockArea
			// 
			this.DockArea.ActiveAutoHideContent = null;
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
			this.MainMenuStrip = this.MMn;
			this.Name = "MainForm";
			this.Text = "Laser GRBL";
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
		private System.Windows.Forms.ToolStripStatusLabel TTLFile;
		private System.Windows.Forms.ToolStripStatusLabel TTTFile;
		private System.Windows.Forms.ToolStripStatusLabel TTLLines;
		private System.Windows.Forms.ToolStripStatusLabel TTTLines;
		private System.Windows.Forms.ToolStripStatusLabel TTLLoadedIn;
		private System.Windows.Forms.ToolStripStatusLabel TTTLoadedIn;
		private System.Windows.Forms.ToolStripStatusLabel TTLEstimated;
		private System.Windows.Forms.ToolStripStatusLabel TTTEstimated;
		
		#endregion
		private System.Windows.Forms.Timer UpdateTimer;
		private System.Windows.Forms.ToolStripStatusLabel spring;
		private System.Windows.Forms.ToolStripStatusLabel TTLStatus;
		private System.Windows.Forms.ToolStripStatusLabel TTTStatus;
		private System.Windows.Forms.MenuStrip MMn;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MnFileOpen;
		private System.Windows.Forms.ToolStripMenuItem MnFileSend;
		private System.Windows.Forms.ToolStripMenuItem grblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MnGrblReset;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem MnExportConfig;
		private System.Windows.Forms.ToolStripMenuItem MnImportConfig;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private LaserGRBL.UserControls.DockingManager.DockPanel DockArea;
		private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem joggingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem overridesToolStripMenuItem;

	}
}

