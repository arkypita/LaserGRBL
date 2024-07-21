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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.ConnectionForm = new LaserGRBL.ConnectLogForm();
			this.JogForm = new LaserGRBL.JogForm();
			this.PreviewForm = new LaserGRBL.PreviewForm();
			this.StatusBar = new System.Windows.Forms.StatusStrip();
			this.TTLLines = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTLines = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLBuffer = new System.Windows.Forms.ToolStripStatusLabel();
			this.PbBuffer = new System.Windows.Forms.ToolStripProgressBar();
			this.BtnUnlockFromStuck = new System.Windows.Forms.ToolStripButton();
			this.TTLEstimated = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTEstimated = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLinkToNews = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTlaserLife = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTSep = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTOvS = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTOvG1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTOvG0 = new System.Windows.Forms.ToolStripStatusLabel();
			this.spacer = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
			this.MMn = new System.Windows.Forms.MenuStrip();
			this.MnGrbl = new System.Windows.Forms.ToolStripMenuItem();
			this.MnConnect = new System.Windows.Forms.ToolStripMenuItem();
			this.MnDisconnect = new System.Windows.Forms.ToolStripMenuItem();
			this.MnWiFiDiscovery = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.MnGrblReset = new System.Windows.Forms.ToolStripMenuItem();
			this.MnGoHome = new System.Windows.Forms.ToolStripMenuItem();
			this.MnUnlock = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.MnGrblConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MnMaterialDB = new System.Windows.Forms.ToolStripMenuItem();
			this.laserUsageStatsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.MnHotkeys = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.MnExit = new System.Windows.Forms.ToolStripMenuItem();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MnFileOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.MnFileAppend = new System.Windows.Forms.ToolStripMenuItem();
			this.MnReOpenFile = new System.Windows.Forms.ToolStripMenuItem();
			this.MnSaveProgram = new System.Windows.Forms.ToolStripMenuItem();
			this.MnAdvancedSave = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.MnSaveProject = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.MnFileSend = new System.Windows.Forms.ToolStripMenuItem();
			this.MnStartFromPosition = new System.Windows.Forms.ToolStripMenuItem();
			this.MnRunMultiSep = new System.Windows.Forms.ToolStripSeparator();
			this.MnRunMulti = new System.Windows.Forms.ToolStripMenuItem();
			this.MnGenerate = new System.Windows.Forms.ToolStripMenuItem();
			this.MnPowerVsSpeed = new System.Windows.Forms.ToolStripMenuItem();
			this.MnCuttingTest = new System.Windows.Forms.ToolStripMenuItem();
			this.MnAccuracyTest = new System.Windows.Forms.ToolStripMenuItem();
			this.shakeTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MNEsp8266 = new System.Windows.Forms.ToolStripMenuItem();
			this.MNGrblEmulator = new System.Windows.Forms.ToolStripMenuItem();
			this.schemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cadStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cadDarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.blueLaserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redLaserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.darkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hackerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nightyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.previewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
			this.showLaserOffMovementsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showExecutedCommandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showBoundingBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.crossCursorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autosizeModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.drawingAreaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.movingAreaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lineSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pxToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.pxToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.pxToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
			this.pxToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
			this.pxToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
			this.showDiagnosticDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.linguaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MNEnglish = new System.Windows.Forms.ToolStripMenuItem();
			this.MNItalian = new System.Windows.Forms.ToolStripMenuItem();
			this.MNSpanish = new System.Windows.Forms.ToolStripMenuItem();
			this.MNFrench = new System.Windows.Forms.ToolStripMenuItem();
			this.MNGerman = new System.Windows.Forms.ToolStripMenuItem();
			this.MNDanish = new System.Windows.Forms.ToolStripMenuItem();
			this.MNBrazilian = new System.Windows.Forms.ToolStripMenuItem();
			this.russianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.chinexeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.traditionalChineseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.slovakianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hungarianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.czechToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.polishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.greekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.turkishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.romanianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dutchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ukrainianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.installCH340DriverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.flashGrblFirmwareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.configurationWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MnOrtur = new System.Windows.Forms.ToolStripMenuItem();
			this.orturSupportGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.orturSupportAndFeedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.orturWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.youtubeChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.manualsDownloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.firmwareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MnSeparatorConfigWiFi = new System.Windows.Forms.ToolStripSeparator();
			this.MnConfigureOrturWiFi = new System.Windows.Forms.ToolStripMenuItem();
			this.questionMarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MnAutoUpdate = new System.Windows.Forms.ToolStripMenuItem();
			this.MnNotifyNewVersion = new System.Windows.Forms.ToolStripMenuItem();
			this.MnNotifyMinorVersion = new System.Windows.Forms.ToolStripMenuItem();
			this.MnNotifyPreRelease = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.MnCheckNow = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.openSessionLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.activateExtendedLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.helpOnLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.facebookCommunityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AwakeTimer = new System.Windows.Forms.Timer(this.components);
			this.MultipleInstanceTimer = new System.Windows.Forms.Timer(this.components);
			this.japaneseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.StatusBar.SuspendLayout();
			this.MMn.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.ConnectionForm);
			this.splitContainer1.Panel1.Controls.Add(this.JogForm);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.PreviewForm);
			this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
			this.splitContainer1.Paint += new System.Windows.Forms.PaintEventHandler(this.SplitContainer1_Paint);
			// 
			// ConnectionForm
			// 
			resources.ApplyResources(this.ConnectionForm, "ConnectionForm");
			this.ConnectionForm.Name = "ConnectionForm";
			// 
			// JogForm
			// 
			resources.ApplyResources(this.JogForm, "JogForm");
			this.JogForm.Name = "JogForm";
			// 
			// PreviewForm
			// 
			resources.ApplyResources(this.PreviewForm, "PreviewForm");
			this.PreviewForm.Name = "PreviewForm";
			// 
			// StatusBar
			// 
			this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TTLLines,
            this.TTTLines,
            this.TTLBuffer,
            this.PbBuffer,
            this.BtnUnlockFromStuck,
            this.TTLEstimated,
            this.TTTEstimated,
            this.TTLinkToNews,
            this.TTlaserLife,
            this.TTSep,
            this.TTOvS,
            this.TTOvG1,
            this.TTOvG0,
            this.spacer,
            this.TTLStatus,
            this.TTTStatus});
			resources.ApplyResources(this.StatusBar, "StatusBar");
			this.StatusBar.Name = "StatusBar";
			this.StatusBar.ShowItemToolTips = true;
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
			// TTLBuffer
			// 
			this.TTLBuffer.Name = "TTLBuffer";
			resources.ApplyResources(this.TTLBuffer, "TTLBuffer");
			// 
			// PbBuffer
			// 
			this.PbBuffer.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.PbBuffer.Maximum = 127;
			this.PbBuffer.Name = "PbBuffer";
			resources.ApplyResources(this.PbBuffer, "PbBuffer");
			this.PbBuffer.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			// 
			// BtnUnlockFromStuck
			// 
			this.BtnUnlockFromStuck.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.BtnUnlockFromStuck, "BtnUnlockFromStuck");
			this.BtnUnlockFromStuck.Margin = new System.Windows.Forms.Padding(0, 2, 2, 0);
			this.BtnUnlockFromStuck.Name = "BtnUnlockFromStuck";
			this.BtnUnlockFromStuck.Click += new System.EventHandler(this.BtnUnlockFromStuck_Click);
			// 
			// TTLEstimated
			// 
			this.TTLEstimated.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTLEstimated.Name = "TTLEstimated";
			resources.ApplyResources(this.TTLEstimated, "TTLEstimated");
			// 
			// TTTEstimated
			// 
			this.TTTEstimated.Name = "TTTEstimated";
			resources.ApplyResources(this.TTTEstimated, "TTTEstimated");
			// 
			// TTLinkToNews
			// 
			resources.ApplyResources(this.TTLinkToNews, "TTLinkToNews");
			this.TTLinkToNews.IsLink = true;
			this.TTLinkToNews.Name = "TTLinkToNews";
			this.TTLinkToNews.Spring = true;
			this.TTLinkToNews.Click += new System.EventHandler(this.TTLinkToNews_Click);
			// 
			// TTlaserLife
			// 
			this.TTlaserLife.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTlaserLife.Name = "TTlaserLife";
			resources.ApplyResources(this.TTlaserLife, "TTlaserLife");
			this.TTlaserLife.Click += new System.EventHandler(this.TTlaserLife_Click);
			// 
			// TTSep
			// 
			resources.ApplyResources(this.TTSep, "TTSep");
			this.TTSep.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTSep.Name = "TTSep";
			// 
			// TTOvS
			// 
			this.TTOvS.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTOvS.Margin = new System.Windows.Forms.Padding(1, 3, 1, 2);
			this.TTOvS.Name = "TTOvS";
			resources.ApplyResources(this.TTOvS, "TTOvS");
			this.TTOvS.Click += new System.EventHandler(this.TTOvClick);
			// 
			// TTOvG1
			// 
			this.TTOvG1.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTOvG1.Margin = new System.Windows.Forms.Padding(1, 3, 1, 2);
			this.TTOvG1.Name = "TTOvG1";
			resources.ApplyResources(this.TTOvG1, "TTOvG1");
			this.TTOvG1.Click += new System.EventHandler(this.TTOvClick);
			// 
			// TTOvG0
			// 
			this.TTOvG0.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTOvG0.Margin = new System.Windows.Forms.Padding(1, 3, 1, 2);
			this.TTOvG0.Name = "TTOvG0";
			resources.ApplyResources(this.TTOvG0, "TTOvG0");
			this.TTOvG0.Click += new System.EventHandler(this.TTOvClick);
			// 
			// spacer
			// 
			resources.ApplyResources(this.spacer, "spacer");
			this.spacer.Name = "spacer";
			// 
			// TTLStatus
			// 
			this.TTLStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTLStatus.Name = "TTLStatus";
			resources.ApplyResources(this.TTLStatus, "TTLStatus");
			// 
			// TTTStatus
			// 
			this.TTTStatus.DoubleClickEnabled = true;
			this.TTTStatus.Name = "TTTStatus";
			resources.ApplyResources(this.TTTStatus, "TTTStatus");
			this.TTTStatus.DoubleClick += new System.EventHandler(this.TTTStatus_DoubleClick);
			// 
			// UpdateTimer
			// 
			this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
			// 
			// MMn
			// 
			this.MMn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnGrbl,
            this.fileToolStripMenuItem,
            this.MnGenerate,
            this.MNEsp8266,
            this.schemaToolStripMenuItem,
            this.previewToolStripMenuItem,
            this.linguaToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.MnOrtur,
            this.questionMarkToolStripMenuItem});
			resources.ApplyResources(this.MMn, "MMn");
			this.MMn.Name = "MMn";
			// 
			// MnGrbl
			// 
			this.MnGrbl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnConnect,
            this.MnDisconnect,
            this.MnWiFiDiscovery,
            this.toolStripMenuItem2,
            this.MnGrblReset,
            this.MnGoHome,
            this.MnUnlock,
            this.toolStripSeparator1,
            this.MnGrblConfig,
            this.settingsToolStripMenuItem,
            this.MnMaterialDB,
            this.laserUsageStatsToolStripMenuItem,
            this.toolStripSeparator2,
            this.MnHotkeys,
            this.toolStripMenuItem6,
            this.MnExit});
			this.MnGrbl.Name = "MnGrbl";
			resources.ApplyResources(this.MnGrbl, "MnGrbl");
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
			// MnWiFiDiscovery
			// 
			this.MnWiFiDiscovery.Name = "MnWiFiDiscovery";
			resources.ApplyResources(this.MnWiFiDiscovery, "MnWiFiDiscovery");
			this.MnWiFiDiscovery.Click += new System.EventHandler(this.MnWiFiDiscovery_Click);
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
			// MnGrblConfig
			// 
			this.MnGrblConfig.Name = "MnGrblConfig";
			resources.ApplyResources(this.MnGrblConfig, "MnGrblConfig");
			this.MnGrblConfig.Click += new System.EventHandler(this.grblConfigurationToolStripMenuItem_Click);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// MnMaterialDB
			// 
			this.MnMaterialDB.Name = "MnMaterialDB";
			resources.ApplyResources(this.MnMaterialDB, "MnMaterialDB");
			this.MnMaterialDB.Click += new System.EventHandler(this.MnMaterialDB_Click);
			// 
			// laserUsageStatsToolStripMenuItem
			// 
			this.laserUsageStatsToolStripMenuItem.Name = "laserUsageStatsToolStripMenuItem";
			resources.ApplyResources(this.laserUsageStatsToolStripMenuItem, "laserUsageStatsToolStripMenuItem");
			this.laserUsageStatsToolStripMenuItem.Click += new System.EventHandler(this.laserUsageStatsToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// MnHotkeys
			// 
			this.MnHotkeys.Name = "MnHotkeys";
			resources.ApplyResources(this.MnHotkeys, "MnHotkeys");
			this.MnHotkeys.Click += new System.EventHandler(this.MnHotkeys_Click);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			resources.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
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
            this.MnFileAppend,
            this.MnReOpenFile,
            this.MnSaveProgram,
            this.MnAdvancedSave,
            this.toolStripMenuItem8,
            this.MnSaveProject,
            this.toolStripMenuItem1,
            this.MnFileSend,
            this.MnStartFromPosition,
            this.MnRunMultiSep,
            this.MnRunMulti});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
			this.fileToolStripMenuItem.DropDownOpening += new System.EventHandler(this.fileToolStripMenuItem_DropDownOpening);
			// 
			// MnFileOpen
			// 
			this.MnFileOpen.Name = "MnFileOpen";
			resources.ApplyResources(this.MnFileOpen, "MnFileOpen");
			this.MnFileOpen.Click += new System.EventHandler(this.MnFileOpen_Click);
			// 
			// MnFileAppend
			// 
			this.MnFileAppend.Name = "MnFileAppend";
			resources.ApplyResources(this.MnFileAppend, "MnFileAppend");
			this.MnFileAppend.Click += new System.EventHandler(this.MnFileAppend_Click);
			// 
			// MnReOpenFile
			// 
			resources.ApplyResources(this.MnReOpenFile, "MnReOpenFile");
			this.MnReOpenFile.Name = "MnReOpenFile";
			this.MnReOpenFile.Click += new System.EventHandler(this.MnReOpenFile_Click);
			// 
			// MnSaveProgram
			// 
			resources.ApplyResources(this.MnSaveProgram, "MnSaveProgram");
			this.MnSaveProgram.Name = "MnSaveProgram";
			this.MnSaveProgram.Click += new System.EventHandler(this.MnSaveProgramClick);
			// 
			// MnAdvancedSave
			// 
			resources.ApplyResources(this.MnAdvancedSave, "MnAdvancedSave");
			this.MnAdvancedSave.Name = "MnAdvancedSave";
			this.MnAdvancedSave.Click += new System.EventHandler(this.MnAdvancedSave_Click);
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			resources.ApplyResources(this.toolStripMenuItem8, "toolStripMenuItem8");
			// 
			// MnSaveProject
			// 
			resources.ApplyResources(this.MnSaveProject, "MnSaveProject");
			this.MnSaveProject.Name = "MnSaveProject";
			this.MnSaveProject.Click += new System.EventHandler(this.MnSaveProject_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
			// 
			// MnFileSend
			// 
			resources.ApplyResources(this.MnFileSend, "MnFileSend");
			this.MnFileSend.Name = "MnFileSend";
			this.MnFileSend.Click += new System.EventHandler(this.MnFileSend_Click);
			// 
			// MnStartFromPosition
			// 
			resources.ApplyResources(this.MnStartFromPosition, "MnStartFromPosition");
			this.MnStartFromPosition.Name = "MnStartFromPosition";
			this.MnStartFromPosition.Click += new System.EventHandler(this.MnStartFromPosition_Click);
			// 
			// MnRunMultiSep
			// 
			this.MnRunMultiSep.Name = "MnRunMultiSep";
			resources.ApplyResources(this.MnRunMultiSep, "MnRunMultiSep");
			// 
			// MnRunMulti
			// 
			this.MnRunMulti.Name = "MnRunMulti";
			resources.ApplyResources(this.MnRunMulti, "MnRunMulti");
			this.MnRunMulti.Click += new System.EventHandler(this.MnRunMulti_Click);
			// 
			// MnGenerate
			// 
			this.MnGenerate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnPowerVsSpeed,
            this.MnCuttingTest,
            this.MnAccuracyTest,
            this.shakeTestToolStripMenuItem});
			this.MnGenerate.Name = "MnGenerate";
			resources.ApplyResources(this.MnGenerate, "MnGenerate");
			// 
			// MnPowerVsSpeed
			// 
			this.MnPowerVsSpeed.Name = "MnPowerVsSpeed";
			resources.ApplyResources(this.MnPowerVsSpeed, "MnPowerVsSpeed");
			this.MnPowerVsSpeed.Click += new System.EventHandler(this.MnGrayscaleTest_Click);
			// 
			// MnCuttingTest
			// 
			this.MnCuttingTest.Name = "MnCuttingTest";
			resources.ApplyResources(this.MnCuttingTest, "MnCuttingTest");
			this.MnCuttingTest.Click += new System.EventHandler(this.MnCuttingTest_Click);
			// 
			// MnAccuracyTest
			// 
			this.MnAccuracyTest.Name = "MnAccuracyTest";
			resources.ApplyResources(this.MnAccuracyTest, "MnAccuracyTest");
			this.MnAccuracyTest.Click += new System.EventHandler(this.MnAccuracyTest_Click);
			// 
			// shakeTestToolStripMenuItem
			// 
			this.shakeTestToolStripMenuItem.Name = "shakeTestToolStripMenuItem";
			resources.ApplyResources(this.shakeTestToolStripMenuItem, "shakeTestToolStripMenuItem");
			this.shakeTestToolStripMenuItem.Click += new System.EventHandler(this.shakeTestToolStripMenuItem_Click);
			// 
			// MNEsp8266
			// 
			this.MNEsp8266.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MNGrblEmulator});
			this.MNEsp8266.Name = "MNEsp8266";
			resources.ApplyResources(this.MNEsp8266, "MNEsp8266");
			// 
			// MNGrblEmulator
			// 
			this.MNGrblEmulator.Name = "MNGrblEmulator";
			resources.ApplyResources(this.MNGrblEmulator, "MNGrblEmulator");
			this.MNGrblEmulator.Click += new System.EventHandler(this.MNGrblEmulator_Click);
			// 
			// schemaToolStripMenuItem
			// 
			this.schemaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cadStyleToolStripMenuItem,
            this.cadDarkToolStripMenuItem,
            this.blueLaserToolStripMenuItem,
            this.redLaserToolStripMenuItem,
            this.darkToolStripMenuItem,
            this.hackerToolStripMenuItem,
            this.nightyToolStripMenuItem});
			this.schemaToolStripMenuItem.Name = "schemaToolStripMenuItem";
			resources.ApplyResources(this.schemaToolStripMenuItem, "schemaToolStripMenuItem");
			// 
			// cadStyleToolStripMenuItem
			// 
			this.cadStyleToolStripMenuItem.Name = "cadStyleToolStripMenuItem";
			resources.ApplyResources(this.cadStyleToolStripMenuItem, "cadStyleToolStripMenuItem");
			this.cadStyleToolStripMenuItem.Click += new System.EventHandler(this.styleToolStripMenuItem_Click);
			// 
			// cadDarkToolStripMenuItem
			// 
			this.cadDarkToolStripMenuItem.Name = "cadDarkToolStripMenuItem";
			resources.ApplyResources(this.cadDarkToolStripMenuItem, "cadDarkToolStripMenuItem");
			this.cadDarkToolStripMenuItem.Click += new System.EventHandler(this.styleToolStripMenuItem_Click);
			// 
			// blueLaserToolStripMenuItem
			// 
			this.blueLaserToolStripMenuItem.Name = "blueLaserToolStripMenuItem";
			resources.ApplyResources(this.blueLaserToolStripMenuItem, "blueLaserToolStripMenuItem");
			this.blueLaserToolStripMenuItem.Click += new System.EventHandler(this.styleToolStripMenuItem_Click);
			// 
			// redLaserToolStripMenuItem
			// 
			this.redLaserToolStripMenuItem.Name = "redLaserToolStripMenuItem";
			resources.ApplyResources(this.redLaserToolStripMenuItem, "redLaserToolStripMenuItem");
			this.redLaserToolStripMenuItem.Click += new System.EventHandler(this.styleToolStripMenuItem_Click);
			// 
			// darkToolStripMenuItem
			// 
			this.darkToolStripMenuItem.Name = "darkToolStripMenuItem";
			resources.ApplyResources(this.darkToolStripMenuItem, "darkToolStripMenuItem");
			this.darkToolStripMenuItem.Click += new System.EventHandler(this.styleToolStripMenuItem_Click);
			// 
			// hackerToolStripMenuItem
			// 
			this.hackerToolStripMenuItem.Name = "hackerToolStripMenuItem";
			resources.ApplyResources(this.hackerToolStripMenuItem, "hackerToolStripMenuItem");
			this.hackerToolStripMenuItem.Click += new System.EventHandler(this.styleToolStripMenuItem_Click);
			// 
			// nightyToolStripMenuItem
			// 
			this.nightyToolStripMenuItem.Name = "nightyToolStripMenuItem";
			resources.ApplyResources(this.nightyToolStripMenuItem, "nightyToolStripMenuItem");
			this.nightyToolStripMenuItem.Click += new System.EventHandler(this.styleToolStripMenuItem_Click);
			// 
			// previewToolStripMenuItem
			// 
			this.previewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoSizeToolStripMenuItem,
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem,
            this.toolStripMenuItem10,
            this.showLaserOffMovementsToolStripMenuItem,
            this.showExecutedCommandsToolStripMenuItem,
            this.showBoundingBoxToolStripMenuItem,
            this.crossCursorToolStripMenuItem,
            this.autosizeModeToolStripMenuItem,
            this.lineSizeToolStripMenuItem,
            this.toolStripMenuItem9,
            this.showDiagnosticDataToolStripMenuItem});
			this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
			resources.ApplyResources(this.previewToolStripMenuItem, "previewToolStripMenuItem");
			// 
			// autoSizeToolStripMenuItem
			// 
			this.autoSizeToolStripMenuItem.Name = "autoSizeToolStripMenuItem";
			resources.ApplyResources(this.autoSizeToolStripMenuItem, "autoSizeToolStripMenuItem");
			this.autoSizeToolStripMenuItem.Click += new System.EventHandler(this.autoSizeToolStripMenuItem_Click);
			// 
			// zoomInToolStripMenuItem
			// 
			this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
			resources.ApplyResources(this.zoomInToolStripMenuItem, "zoomInToolStripMenuItem");
			this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
			// 
			// zoomOutToolStripMenuItem
			// 
			this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
			resources.ApplyResources(this.zoomOutToolStripMenuItem, "zoomOutToolStripMenuItem");
			this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
			// 
			// toolStripMenuItem10
			// 
			this.toolStripMenuItem10.Name = "toolStripMenuItem10";
			resources.ApplyResources(this.toolStripMenuItem10, "toolStripMenuItem10");
			// 
			// showLaserOffMovementsToolStripMenuItem
			// 
			this.showLaserOffMovementsToolStripMenuItem.CheckOnClick = true;
			this.showLaserOffMovementsToolStripMenuItem.Name = "showLaserOffMovementsToolStripMenuItem";
			resources.ApplyResources(this.showLaserOffMovementsToolStripMenuItem, "showLaserOffMovementsToolStripMenuItem");
			this.showLaserOffMovementsToolStripMenuItem.Click += new System.EventHandler(this.showLaserOffMovementsToolStripMenuItem_Click);
			// 
			// showExecutedCommandsToolStripMenuItem
			// 
			this.showExecutedCommandsToolStripMenuItem.CheckOnClick = true;
			this.showExecutedCommandsToolStripMenuItem.Name = "showExecutedCommandsToolStripMenuItem";
			resources.ApplyResources(this.showExecutedCommandsToolStripMenuItem, "showExecutedCommandsToolStripMenuItem");
			this.showExecutedCommandsToolStripMenuItem.Click += new System.EventHandler(this.showExecutedCommandsToolStripMenuItem_Click);
			// 
			// showBoundingBoxToolStripMenuItem
			// 
			this.showBoundingBoxToolStripMenuItem.CheckOnClick = true;
			this.showBoundingBoxToolStripMenuItem.Name = "showBoundingBoxToolStripMenuItem";
			resources.ApplyResources(this.showBoundingBoxToolStripMenuItem, "showBoundingBoxToolStripMenuItem");
			this.showBoundingBoxToolStripMenuItem.Click += new System.EventHandler(this.showBoundingBoxToolStripMenuItem_Click);
			// 
			// crossCursorToolStripMenuItem
			// 
			this.crossCursorToolStripMenuItem.CheckOnClick = true;
			this.crossCursorToolStripMenuItem.Name = "crossCursorToolStripMenuItem";
			resources.ApplyResources(this.crossCursorToolStripMenuItem, "crossCursorToolStripMenuItem");
			this.crossCursorToolStripMenuItem.Click += new System.EventHandler(this.crossCursorToolStripMenuItem_Click);
			// 
			// autosizeModeToolStripMenuItem
			// 
			this.autosizeModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawingAreaToolStripMenuItem,
            this.movingAreaToolStripMenuItem});
			this.autosizeModeToolStripMenuItem.Name = "autosizeModeToolStripMenuItem";
			resources.ApplyResources(this.autosizeModeToolStripMenuItem, "autosizeModeToolStripMenuItem");
			// 
			// drawingAreaToolStripMenuItem
			// 
			this.drawingAreaToolStripMenuItem.CheckOnClick = true;
			this.drawingAreaToolStripMenuItem.Name = "drawingAreaToolStripMenuItem";
			resources.ApplyResources(this.drawingAreaToolStripMenuItem, "drawingAreaToolStripMenuItem");
			this.drawingAreaToolStripMenuItem.Tag = "Drawing area";
			this.drawingAreaToolStripMenuItem.Click += new System.EventHandler(this.drawingAreaToolStripMenuItem_Click);
			// 
			// movingAreaToolStripMenuItem
			// 
			this.movingAreaToolStripMenuItem.CheckOnClick = true;
			this.movingAreaToolStripMenuItem.Name = "movingAreaToolStripMenuItem";
			resources.ApplyResources(this.movingAreaToolStripMenuItem, "movingAreaToolStripMenuItem");
			this.movingAreaToolStripMenuItem.Tag = "Machine area";
			this.movingAreaToolStripMenuItem.Click += new System.EventHandler(this.machineAreaToolStripMenuItem_Click);
			// 
			// lineSizeToolStripMenuItem
			// 
			this.lineSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pxToolStripMenuItem1,
            this.pxToolStripMenuItem2,
            this.pxToolStripMenuItem3,
            this.pxToolStripMenuItem4,
            this.pxToolStripMenuItem5});
			this.lineSizeToolStripMenuItem.Name = "lineSizeToolStripMenuItem";
			resources.ApplyResources(this.lineSizeToolStripMenuItem, "lineSizeToolStripMenuItem");
			// 
			// pxToolStripMenuItem1
			// 
			this.pxToolStripMenuItem1.Name = "pxToolStripMenuItem1";
			resources.ApplyResources(this.pxToolStripMenuItem1, "pxToolStripMenuItem1");
			this.pxToolStripMenuItem1.Tag = "1";
			this.pxToolStripMenuItem1.Click += new System.EventHandler(this.pxToolStripMenuItem_Click);
			// 
			// pxToolStripMenuItem2
			// 
			this.pxToolStripMenuItem2.Name = "pxToolStripMenuItem2";
			resources.ApplyResources(this.pxToolStripMenuItem2, "pxToolStripMenuItem2");
			this.pxToolStripMenuItem2.Tag = "2";
			this.pxToolStripMenuItem2.Click += new System.EventHandler(this.pxToolStripMenuItem_Click);
			// 
			// pxToolStripMenuItem3
			// 
			this.pxToolStripMenuItem3.Name = "pxToolStripMenuItem3";
			resources.ApplyResources(this.pxToolStripMenuItem3, "pxToolStripMenuItem3");
			this.pxToolStripMenuItem3.Tag = "3";
			this.pxToolStripMenuItem3.Click += new System.EventHandler(this.pxToolStripMenuItem_Click);
			// 
			// pxToolStripMenuItem4
			// 
			this.pxToolStripMenuItem4.Name = "pxToolStripMenuItem4";
			resources.ApplyResources(this.pxToolStripMenuItem4, "pxToolStripMenuItem4");
			this.pxToolStripMenuItem4.Tag = "4";
			this.pxToolStripMenuItem4.Click += new System.EventHandler(this.pxToolStripMenuItem_Click);
			// 
			// pxToolStripMenuItem5
			// 
			this.pxToolStripMenuItem5.Name = "pxToolStripMenuItem5";
			resources.ApplyResources(this.pxToolStripMenuItem5, "pxToolStripMenuItem5");
			this.pxToolStripMenuItem5.Tag = "5";
			this.pxToolStripMenuItem5.Click += new System.EventHandler(this.pxToolStripMenuItem_Click);
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			resources.ApplyResources(this.toolStripMenuItem9, "toolStripMenuItem9");
			// 
			// showDiagnosticDataToolStripMenuItem
			// 
			this.showDiagnosticDataToolStripMenuItem.CheckOnClick = true;
			this.showDiagnosticDataToolStripMenuItem.Name = "showDiagnosticDataToolStripMenuItem";
			resources.ApplyResources(this.showDiagnosticDataToolStripMenuItem, "showDiagnosticDataToolStripMenuItem");
			this.showDiagnosticDataToolStripMenuItem.Click += new System.EventHandler(this.showDiagnosticDataToolStripMenuItem_Click);
			// 
			// linguaToolStripMenuItem
			// 
			this.linguaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MNEnglish,
            this.MNItalian,
            this.MNSpanish,
            this.MNFrench,
            this.MNGerman,
            this.MNDanish,
            this.MNBrazilian,
            this.russianToolStripMenuItem,
            this.chinexeToolStripMenuItem,
            this.traditionalChineseToolStripMenuItem,
            this.slovakianToolStripMenuItem,
            this.hungarianToolStripMenuItem,
            this.czechToolStripMenuItem,
            this.polishToolStripMenuItem,
            this.greekToolStripMenuItem,
            this.turkishToolStripMenuItem,
            this.romanianToolStripMenuItem,
            this.dutchToolStripMenuItem,
            this.ukrainianToolStripMenuItem,
            this.japaneseToolStripMenuItem});
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
			// MNFrench
			// 
			this.MNFrench.Name = "MNFrench";
			resources.ApplyResources(this.MNFrench, "MNFrench");
			this.MNFrench.Click += new System.EventHandler(this.MNFrench_Click);
			// 
			// MNGerman
			// 
			this.MNGerman.Name = "MNGerman";
			resources.ApplyResources(this.MNGerman, "MNGerman");
			this.MNGerman.Click += new System.EventHandler(this.MNGerman_Click);
			// 
			// MNDanish
			// 
			this.MNDanish.Name = "MNDanish";
			resources.ApplyResources(this.MNDanish, "MNDanish");
			this.MNDanish.Click += new System.EventHandler(this.MNDanish_Click);
			// 
			// MNBrazilian
			// 
			this.MNBrazilian.Name = "MNBrazilian";
			resources.ApplyResources(this.MNBrazilian, "MNBrazilian");
			this.MNBrazilian.Click += new System.EventHandler(this.MNBrazilian_Click);
			// 
			// russianToolStripMenuItem
			// 
			this.russianToolStripMenuItem.Name = "russianToolStripMenuItem";
			resources.ApplyResources(this.russianToolStripMenuItem, "russianToolStripMenuItem");
			this.russianToolStripMenuItem.Click += new System.EventHandler(this.russianToolStripMenuItem_Click);
			// 
			// chinexeToolStripMenuItem
			// 
			this.chinexeToolStripMenuItem.Name = "chinexeToolStripMenuItem";
			resources.ApplyResources(this.chinexeToolStripMenuItem, "chinexeToolStripMenuItem");
			this.chinexeToolStripMenuItem.Click += new System.EventHandler(this.chineseToolStripMenuItem_Click);
			// 
			// traditionalChineseToolStripMenuItem
			// 
			this.traditionalChineseToolStripMenuItem.Name = "traditionalChineseToolStripMenuItem";
			resources.ApplyResources(this.traditionalChineseToolStripMenuItem, "traditionalChineseToolStripMenuItem");
			this.traditionalChineseToolStripMenuItem.Click += new System.EventHandler(this.traditionalChineseToolStripMenuItem_Click);
			// 
			// slovakianToolStripMenuItem
			// 
			this.slovakianToolStripMenuItem.Name = "slovakianToolStripMenuItem";
			resources.ApplyResources(this.slovakianToolStripMenuItem, "slovakianToolStripMenuItem");
			this.slovakianToolStripMenuItem.Click += new System.EventHandler(this.slovakToolStripMenuItem_Click);
			// 
			// hungarianToolStripMenuItem
			// 
			this.hungarianToolStripMenuItem.Name = "hungarianToolStripMenuItem";
			resources.ApplyResources(this.hungarianToolStripMenuItem, "hungarianToolStripMenuItem");
			this.hungarianToolStripMenuItem.Click += new System.EventHandler(this.hungarianToolStripMenuItem_Click);
			// 
			// czechToolStripMenuItem
			// 
			this.czechToolStripMenuItem.Name = "czechToolStripMenuItem";
			resources.ApplyResources(this.czechToolStripMenuItem, "czechToolStripMenuItem");
			this.czechToolStripMenuItem.Click += new System.EventHandler(this.czechToolStripMenuItem_Click);
			// 
			// polishToolStripMenuItem
			// 
			this.polishToolStripMenuItem.Name = "polishToolStripMenuItem";
			resources.ApplyResources(this.polishToolStripMenuItem, "polishToolStripMenuItem");
			this.polishToolStripMenuItem.Click += new System.EventHandler(this.polishToolStripMenuItem_Click);
			// 
			// greekToolStripMenuItem
			// 
			this.greekToolStripMenuItem.Name = "greekToolStripMenuItem";
			resources.ApplyResources(this.greekToolStripMenuItem, "greekToolStripMenuItem");
			this.greekToolStripMenuItem.Click += new System.EventHandler(this.greekToolStripMenuItem_Click);
			// 
			// turkishToolStripMenuItem
			// 
			this.turkishToolStripMenuItem.Name = "turkishToolStripMenuItem";
			resources.ApplyResources(this.turkishToolStripMenuItem, "turkishToolStripMenuItem");
			this.turkishToolStripMenuItem.Click += new System.EventHandler(this.turkishToolStripMenuItem_Click);
			// 
			// romanianToolStripMenuItem
			// 
			this.romanianToolStripMenuItem.Name = "romanianToolStripMenuItem";
			resources.ApplyResources(this.romanianToolStripMenuItem, "romanianToolStripMenuItem");
			this.romanianToolStripMenuItem.Click += new System.EventHandler(this.romanianToolStripMenuItem_Click);
			// 
			// dutchToolStripMenuItem
			// 
			this.dutchToolStripMenuItem.Name = "dutchToolStripMenuItem";
			resources.ApplyResources(this.dutchToolStripMenuItem, "dutchToolStripMenuItem");
			this.dutchToolStripMenuItem.Click += new System.EventHandler(this.dutchToolStripMenuItem_Click);
			// 
			// ukrainianToolStripMenuItem
			// 
			this.ukrainianToolStripMenuItem.Name = "ukrainianToolStripMenuItem";
			resources.ApplyResources(this.ukrainianToolStripMenuItem, "ukrainianToolStripMenuItem");
			this.ukrainianToolStripMenuItem.Click += new System.EventHandler(this.ukrainianToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installCH340DriverToolStripMenuItem,
            this.flashGrblFirmwareToolStripMenuItem,
            this.toolStripSeparator3,
            this.configurationWizardToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
			this.toolsToolStripMenuItem.DropDownOpening += new System.EventHandler(this.toolsToolStripMenuItem_DropDownOpening);
			// 
			// installCH340DriverToolStripMenuItem
			// 
			this.installCH340DriverToolStripMenuItem.Name = "installCH340DriverToolStripMenuItem";
			resources.ApplyResources(this.installCH340DriverToolStripMenuItem, "installCH340DriverToolStripMenuItem");
			this.installCH340DriverToolStripMenuItem.Click += new System.EventHandler(this.installCH340DriverToolStripMenuItem_Click);
			// 
			// flashGrblFirmwareToolStripMenuItem
			// 
			this.flashGrblFirmwareToolStripMenuItem.Name = "flashGrblFirmwareToolStripMenuItem";
			resources.ApplyResources(this.flashGrblFirmwareToolStripMenuItem, "flashGrblFirmwareToolStripMenuItem");
			this.flashGrblFirmwareToolStripMenuItem.Click += new System.EventHandler(this.flashGrblFirmwareToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			// 
			// configurationWizardToolStripMenuItem
			// 
			this.configurationWizardToolStripMenuItem.Name = "configurationWizardToolStripMenuItem";
			resources.ApplyResources(this.configurationWizardToolStripMenuItem, "configurationWizardToolStripMenuItem");
			// 
			// MnOrtur
			// 
			this.MnOrtur.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.orturSupportGroupToolStripMenuItem,
            this.orturSupportAndFeedbackToolStripMenuItem,
            this.orturWebsiteToolStripMenuItem,
            this.youtubeChannelToolStripMenuItem,
            this.toolStripSeparator4,
            this.manualsDownloadToolStripMenuItem,
            this.firmwareToolStripMenuItem,
            this.MnSeparatorConfigWiFi,
            this.MnConfigureOrturWiFi});
			this.MnOrtur.Name = "MnOrtur";
			resources.ApplyResources(this.MnOrtur, "MnOrtur");
			// 
			// orturSupportGroupToolStripMenuItem
			// 
			this.orturSupportGroupToolStripMenuItem.Name = "orturSupportGroupToolStripMenuItem";
			resources.ApplyResources(this.orturSupportGroupToolStripMenuItem, "orturSupportGroupToolStripMenuItem");
			this.orturSupportGroupToolStripMenuItem.Click += new System.EventHandler(this.orturSupportGroupToolStripMenuItem_Click);
			// 
			// orturSupportAndFeedbackToolStripMenuItem
			// 
			this.orturSupportAndFeedbackToolStripMenuItem.Name = "orturSupportAndFeedbackToolStripMenuItem";
			resources.ApplyResources(this.orturSupportAndFeedbackToolStripMenuItem, "orturSupportAndFeedbackToolStripMenuItem");
			this.orturSupportAndFeedbackToolStripMenuItem.Click += new System.EventHandler(this.orturSupportAndFeedbackToolStripMenuItem_Click);
			// 
			// orturWebsiteToolStripMenuItem
			// 
			this.orturWebsiteToolStripMenuItem.Name = "orturWebsiteToolStripMenuItem";
			resources.ApplyResources(this.orturWebsiteToolStripMenuItem, "orturWebsiteToolStripMenuItem");
			this.orturWebsiteToolStripMenuItem.Click += new System.EventHandler(this.orturWebsiteToolStripMenuItem_Click);
			// 
			// youtubeChannelToolStripMenuItem
			// 
			this.youtubeChannelToolStripMenuItem.Name = "youtubeChannelToolStripMenuItem";
			resources.ApplyResources(this.youtubeChannelToolStripMenuItem, "youtubeChannelToolStripMenuItem");
			this.youtubeChannelToolStripMenuItem.Click += new System.EventHandler(this.youtubeChannelToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			// 
			// manualsDownloadToolStripMenuItem
			// 
			this.manualsDownloadToolStripMenuItem.Name = "manualsDownloadToolStripMenuItem";
			resources.ApplyResources(this.manualsDownloadToolStripMenuItem, "manualsDownloadToolStripMenuItem");
			this.manualsDownloadToolStripMenuItem.Click += new System.EventHandler(this.manualsDownloadToolStripMenuItem_Click);
			// 
			// firmwareToolStripMenuItem
			// 
			this.firmwareToolStripMenuItem.Name = "firmwareToolStripMenuItem";
			resources.ApplyResources(this.firmwareToolStripMenuItem, "firmwareToolStripMenuItem");
			this.firmwareToolStripMenuItem.Click += new System.EventHandler(this.firmwareToolStripMenuItem_Click);
			// 
			// MnSeparatorConfigWiFi
			// 
			this.MnSeparatorConfigWiFi.Name = "MnSeparatorConfigWiFi";
			resources.ApplyResources(this.MnSeparatorConfigWiFi, "MnSeparatorConfigWiFi");
			// 
			// MnConfigureOrturWiFi
			// 
			resources.ApplyResources(this.MnConfigureOrturWiFi, "MnConfigureOrturWiFi");
			this.MnConfigureOrturWiFi.Name = "MnConfigureOrturWiFi";
			this.MnConfigureOrturWiFi.Click += new System.EventHandler(this.MnConfigureOrturWiFi_Click);
			// 
			// questionMarkToolStripMenuItem
			// 
			this.questionMarkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnAutoUpdate,
            this.toolStripMenuItem5,
            this.openSessionLogToolStripMenuItem,
            this.activateExtendedLogToolStripMenuItem,
            this.toolStripMenuItem7,
            this.helpOnLineToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.facebookCommunityToolStripMenuItem,
            this.toolStripMenuItem3,
            this.donateToolStripMenuItem,
            this.licenseToolStripMenuItem});
			this.questionMarkToolStripMenuItem.Name = "questionMarkToolStripMenuItem";
			resources.ApplyResources(this.questionMarkToolStripMenuItem, "questionMarkToolStripMenuItem");
			this.questionMarkToolStripMenuItem.DropDownOpening += new System.EventHandler(this.toolStripMenuItem4_DropDownOpening);
			// 
			// MnAutoUpdate
			// 
			this.MnAutoUpdate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnNotifyNewVersion,
            this.MnNotifyMinorVersion,
            this.MnNotifyPreRelease,
            this.toolStripMenuItem4,
            this.MnCheckNow});
			this.MnAutoUpdate.Name = "MnAutoUpdate";
			resources.ApplyResources(this.MnAutoUpdate, "MnAutoUpdate");
			// 
			// MnNotifyNewVersion
			// 
			this.MnNotifyNewVersion.Checked = true;
			this.MnNotifyNewVersion.CheckState = System.Windows.Forms.CheckState.Checked;
			this.MnNotifyNewVersion.Name = "MnNotifyNewVersion";
			resources.ApplyResources(this.MnNotifyNewVersion, "MnNotifyNewVersion");
			this.MnNotifyNewVersion.CheckedChanged += new System.EventHandler(this.MnNotifyNewVersion_CheckedChanged);
			this.MnNotifyNewVersion.Click += new System.EventHandler(this.MnNotifyNewVersion_Click);
			// 
			// MnNotifyMinorVersion
			// 
			this.MnNotifyMinorVersion.Name = "MnNotifyMinorVersion";
			resources.ApplyResources(this.MnNotifyMinorVersion, "MnNotifyMinorVersion");
			this.MnNotifyMinorVersion.Click += new System.EventHandler(this.MnNotifyMinorVersion_Click);
			// 
			// MnNotifyPreRelease
			// 
			this.MnNotifyPreRelease.Name = "MnNotifyPreRelease";
			resources.ApplyResources(this.MnNotifyPreRelease, "MnNotifyPreRelease");
			this.MnNotifyPreRelease.Click += new System.EventHandler(this.MnNotifyPreRelease_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
			// 
			// MnCheckNow
			// 
			this.MnCheckNow.Name = "MnCheckNow";
			resources.ApplyResources(this.MnCheckNow, "MnCheckNow");
			this.MnCheckNow.Click += new System.EventHandler(this.MnCheckNow_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			resources.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
			// 
			// openSessionLogToolStripMenuItem
			// 
			this.openSessionLogToolStripMenuItem.Name = "openSessionLogToolStripMenuItem";
			resources.ApplyResources(this.openSessionLogToolStripMenuItem, "openSessionLogToolStripMenuItem");
			this.openSessionLogToolStripMenuItem.Click += new System.EventHandler(this.openSessionLogToolStripMenuItem_Click);
			// 
			// activateExtendedLogToolStripMenuItem
			// 
			this.activateExtendedLogToolStripMenuItem.Name = "activateExtendedLogToolStripMenuItem";
			resources.ApplyResources(this.activateExtendedLogToolStripMenuItem, "activateExtendedLogToolStripMenuItem");
			this.activateExtendedLogToolStripMenuItem.Click += new System.EventHandler(this.activateExtendedLogToolStripMenuItem_Click);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			resources.ApplyResources(this.toolStripMenuItem7, "toolStripMenuItem7");
			// 
			// helpOnLineToolStripMenuItem
			// 
			this.helpOnLineToolStripMenuItem.Name = "helpOnLineToolStripMenuItem";
			resources.ApplyResources(this.helpOnLineToolStripMenuItem, "helpOnLineToolStripMenuItem");
			this.helpOnLineToolStripMenuItem.Click += new System.EventHandler(this.helpOnLineToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// facebookCommunityToolStripMenuItem
			// 
			this.facebookCommunityToolStripMenuItem.Name = "facebookCommunityToolStripMenuItem";
			resources.ApplyResources(this.facebookCommunityToolStripMenuItem, "facebookCommunityToolStripMenuItem");
			this.facebookCommunityToolStripMenuItem.Click += new System.EventHandler(this.facebookCommunityToolStripMenuItem_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
			// 
			// donateToolStripMenuItem
			// 
			this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
			resources.ApplyResources(this.donateToolStripMenuItem, "donateToolStripMenuItem");
			this.donateToolStripMenuItem.Click += new System.EventHandler(this.donateToolStripMenuItem_Click);
			// 
			// licenseToolStripMenuItem
			// 
			this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
			resources.ApplyResources(this.licenseToolStripMenuItem, "licenseToolStripMenuItem");
			this.licenseToolStripMenuItem.Click += new System.EventHandler(this.licenseToolStripMenuItem_Click);
			// 
			// AwakeTimer
			// 
			this.AwakeTimer.Enabled = true;
			this.AwakeTimer.Interval = 20000;
			this.AwakeTimer.Tick += new System.EventHandler(this.AwakeTimer_Tick);
			// 
			// MultipleInstanceTimer
			// 
			this.MultipleInstanceTimer.Interval = 1000;
			this.MultipleInstanceTimer.Tick += new System.EventHandler(this.MultipleInstanceTimer_Tick);
			// 
			// japaneseToolStripMenuItem
			// 
			this.japaneseToolStripMenuItem.Name = "japaneseToolStripMenuItem";
			resources.ApplyResources(this.japaneseToolStripMenuItem, "japaneseToolStripMenuItem");
			this.japaneseToolStripMenuItem.Click += new System.EventHandler(this.japaneseToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.StatusBar);
			this.Controls.Add(this.MMn);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.KeyPreview = true;
			this.Name = "MainForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
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
		private System.Windows.Forms.ToolStripMenuItem MnGrbl;
		private System.Windows.Forms.ToolStripMenuItem MnGrblReset;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.Timer UpdateTimer;
		private System.Windows.Forms.ToolStripStatusLabel TTLStatus;
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
		private System.Windows.Forms.ToolStripMenuItem MnExit;
		private System.Windows.Forms.ToolStripMenuItem MnSaveProgram;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripStatusLabel TTTLines;
		private System.Windows.Forms.ToolStripStatusLabel TTTStatus;
		private System.Windows.Forms.ToolStripMenuItem linguaToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MNEnglish;
		private System.Windows.Forms.ToolStripMenuItem MNItalian;
		private System.Windows.Forms.ToolStripMenuItem MNSpanish;
		private System.Windows.Forms.ToolStripMenuItem questionMarkToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpOnLineToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private PreviewForm PreviewForm;
		private JogForm JogForm;
		private ConnectLogForm ConnectionForm;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem openSessionLogToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem MNFrench;
		private System.Windows.Forms.ToolStripMenuItem MNGerman;
		private System.Windows.Forms.ToolStripMenuItem MNDanish;
		private System.Windows.Forms.ToolStripMenuItem MNBrazilian;
		private System.Windows.Forms.ToolStripMenuItem russianToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MNEsp8266;
		private System.Windows.Forms.ToolStripMenuItem MNGrblEmulator;
		private System.Windows.Forms.ToolStripMenuItem chinexeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem schemaToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem blueLaserToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem redLaserToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem darkToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hackerToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel TTLBuffer;
		private System.Windows.Forms.ToolStripProgressBar PbBuffer;
		private System.Windows.Forms.ToolStripMenuItem MnGrblConfig;
		private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MnReOpenFile;
		private System.Windows.Forms.ToolStripMenuItem MnHotkeys;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.Timer AwakeTimer;
		private System.Windows.Forms.ToolStripMenuItem MnStartFromPosition;
		private System.Windows.Forms.ToolStripMenuItem MnFileAppend;
		private System.Windows.Forms.ToolStripMenuItem slovakianToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hungarianToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem czechToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem flashGrblFirmwareToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem configurationWizardToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem installCH340DriverToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem activateExtendedLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nightyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MnAdvancedSave;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem licenseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MnAutoUpdate;
		private System.Windows.Forms.ToolStripMenuItem MnNotifyMinorVersion;
		private System.Windows.Forms.ToolStripMenuItem MnNotifyNewVersion;
		private System.Windows.Forms.ToolStripMenuItem MnNotifyPreRelease;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem MnCheckNow;
		private System.Windows.Forms.ToolStripMenuItem MnMaterialDB;
		private System.Windows.Forms.ToolStripMenuItem polishToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MnOrtur;
		private System.Windows.Forms.ToolStripMenuItem orturSupportGroupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem orturWebsiteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem facebookCommunityToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem traditionalChineseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem youtubeChannelToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem manualsDownloadToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem firmwareToolStripMenuItem;
		private System.Windows.Forms.Timer MultipleInstanceTimer;
		private System.Windows.Forms.ToolStripMenuItem MnRunMulti;
		private System.Windows.Forms.ToolStripSeparator MnRunMultiSep;
		private System.Windows.Forms.ToolStripMenuItem orturSupportAndFeedbackToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel TTLinkToNews;
		private System.Windows.Forms.ToolStripMenuItem MnWiFiDiscovery;
        private System.Windows.Forms.ToolStripMenuItem greekToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem turkishToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
		private System.Windows.Forms.ToolStripMenuItem MnSaveProject;
		private System.Windows.Forms.ToolStripButton BtnUnlockFromStuck;
        private System.Windows.Forms.ToolStripMenuItem romanianToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dutchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MnConfigureOrturWiFi;
		private System.Windows.Forms.ToolStripSeparator MnSeparatorConfigWiFi;
		private System.Windows.Forms.ToolStripMenuItem ukrainianToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel TTlaserLife;
		private System.Windows.Forms.ToolStripStatusLabel TTSep;
		private System.Windows.Forms.ToolStripMenuItem laserUsageStatsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem MnGenerate;
		private System.Windows.Forms.ToolStripMenuItem MnPowerVsSpeed;
		private System.Windows.Forms.ToolStripMenuItem MnCuttingTest;
		private System.Windows.Forms.ToolStripMenuItem MnAccuracyTest;
		private System.Windows.Forms.ToolStripMenuItem shakeTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLaserOffMovementsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showExecutedCommandsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cadStyleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cadDarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pxToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pxToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem pxToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem pxToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem pxToolStripMenuItem5;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
		private System.Windows.Forms.ToolStripMenuItem showDiagnosticDataToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
		private System.Windows.Forms.ToolStripMenuItem showBoundingBoxToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem crossCursorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem autosizeModeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawingAreaToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem movingAreaToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem japaneseToolStripMenuItem;
	}
}

