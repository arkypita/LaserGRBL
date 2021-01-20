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
			this.TTLEstimated = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTEstimated = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLinkToNews = new System.Windows.Forms.ToolStripStatusLabel();
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
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.MnFileSend = new System.Windows.Forms.ToolStripMenuItem();
			this.MnStartFromPosition = new System.Windows.Forms.ToolStripMenuItem();
			this.MnRunMultiSep = new System.Windows.Forms.ToolStripSeparator();
			this.MnRunMulti = new System.Windows.Forms.ToolStripMenuItem();
			this.MNEsp8266 = new System.Windows.Forms.ToolStripMenuItem();
			this.MNGrblEmulator = new System.Windows.Forms.ToolStripMenuItem();
			this.schemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.blueLaserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redLaserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.darkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hackerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nightyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
			this.turkishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.TTLEstimated,
            this.TTTEstimated,
            this.TTLinkToNews,
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
			this.TTLBuffer.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTLBuffer.Name = "TTLBuffer";
			resources.ApplyResources(this.TTLBuffer, "TTLBuffer");
			// 
			// PbBuffer
			// 
			this.PbBuffer.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this.PbBuffer.Maximum = 127;
			this.PbBuffer.Name = "PbBuffer";
			resources.ApplyResources(this.PbBuffer, "PbBuffer");
			this.PbBuffer.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
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
			// TTLinkToNews
			// 
			resources.ApplyResources(this.TTLinkToNews, "TTLinkToNews");
			this.TTLinkToNews.IsLink = true;
			this.TTLinkToNews.Name = "TTLinkToNews";
			this.TTLinkToNews.Spring = true;
			this.TTLinkToNews.Click += new System.EventHandler(this.TTLinkToNews_Click);
			// 
			// TTOvS
			// 
			resources.ApplyResources(this.TTOvS, "TTOvS");
			this.TTOvS.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTOvS.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTOvS.Name = "TTOvS";
			this.TTOvS.Click += new System.EventHandler(this.TTOvClick);
			// 
			// TTOvG1
			// 
			resources.ApplyResources(this.TTOvG1, "TTOvG1");
			this.TTOvG1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTOvG1.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTOvG1.Name = "TTOvG1";
			this.TTOvG1.Click += new System.EventHandler(this.TTOvClick);
			// 
			// TTOvG0
			// 
			resources.ApplyResources(this.TTOvG0, "TTOvG0");
			this.TTOvG0.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
			this.TTOvG0.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this.TTOvG0.Name = "TTOvG0";
			this.TTOvG0.Click += new System.EventHandler(this.TTOvClick);
			// 
			// spacer
			// 
			resources.ApplyResources(this.spacer, "spacer");
			this.spacer.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
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
            this.MnGrbl,
            this.fileToolStripMenuItem,
            this.MNEsp8266,
            this.schemaToolStripMenuItem,
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
            this.toolStripSeparator2,
            this.MnHotkeys,
            this.toolStripMenuItem6,
            this.MnExit});
			this.MnGrbl.Name = "MnGrbl";
			resources.ApplyResources(this.MnGrbl, "MnGrbl");
			// 
			// MnConnect
			// 
			resources.ApplyResources(this.MnConnect, "MnConnect");
			this.MnConnect.Name = "MnConnect";
			this.MnConnect.Click += new System.EventHandler(this.MnConnect_Click);
			// 
			// MnDisconnect
			// 
			resources.ApplyResources(this.MnDisconnect, "MnDisconnect");
			this.MnDisconnect.Name = "MnDisconnect";
			this.MnDisconnect.Click += new System.EventHandler(this.MnDisconnect_Click);
			// 
			// MnWiFiDiscovery
			// 
			resources.ApplyResources(this.MnWiFiDiscovery, "MnWiFiDiscovery");
			this.MnWiFiDiscovery.Name = "MnWiFiDiscovery";
			this.MnWiFiDiscovery.Click += new System.EventHandler(this.MnWiFiDiscovery_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
			// 
			// MnGrblReset
			// 
			resources.ApplyResources(this.MnGrblReset, "MnGrblReset");
			this.MnGrblReset.Name = "MnGrblReset";
			this.MnGrblReset.Click += new System.EventHandler(this.MnGrblReset_Click);
			// 
			// MnGoHome
			// 
			resources.ApplyResources(this.MnGoHome, "MnGoHome");
			this.MnGoHome.Name = "MnGoHome";
			this.MnGoHome.Click += new System.EventHandler(this.MnGoHome_Click);
			// 
			// MnUnlock
			// 
			resources.ApplyResources(this.MnUnlock, "MnUnlock");
			this.MnUnlock.Name = "MnUnlock";
			this.MnUnlock.Click += new System.EventHandler(this.MnUnlock_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// MnGrblConfig
			// 
			resources.ApplyResources(this.MnGrblConfig, "MnGrblConfig");
			this.MnGrblConfig.Name = "MnGrblConfig";
			this.MnGrblConfig.Click += new System.EventHandler(this.grblConfigurationToolStripMenuItem_Click);
			// 
			// settingsToolStripMenuItem
			// 
			resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// MnMaterialDB
			// 
			resources.ApplyResources(this.MnMaterialDB, "MnMaterialDB");
			this.MnMaterialDB.Name = "MnMaterialDB";
			this.MnMaterialDB.Click += new System.EventHandler(this.MnMaterialDB_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// MnHotkeys
			// 
			resources.ApplyResources(this.MnHotkeys, "MnHotkeys");
			this.MnHotkeys.Name = "MnHotkeys";
			this.MnHotkeys.Click += new System.EventHandler(this.MnHotkeys_Click);
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			resources.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
			// 
			// MnExit
			// 
			resources.ApplyResources(this.MnExit, "MnExit");
			this.MnExit.Name = "MnExit";
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
			resources.ApplyResources(this.MnFileOpen, "MnFileOpen");
			this.MnFileOpen.Name = "MnFileOpen";
			this.MnFileOpen.Click += new System.EventHandler(this.MnFileOpen_Click);
			// 
			// MnFileAppend
			// 
			resources.ApplyResources(this.MnFileAppend, "MnFileAppend");
			this.MnFileAppend.Name = "MnFileAppend";
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
			resources.ApplyResources(this.MnRunMulti, "MnRunMulti");
			this.MnRunMulti.Name = "MnRunMulti";
			this.MnRunMulti.Click += new System.EventHandler(this.MnRunMulti_Click);
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
            this.blueLaserToolStripMenuItem,
            this.redLaserToolStripMenuItem,
            this.darkToolStripMenuItem,
            this.hackerToolStripMenuItem,
            this.nightyToolStripMenuItem});
			this.schemaToolStripMenuItem.Name = "schemaToolStripMenuItem";
			resources.ApplyResources(this.schemaToolStripMenuItem, "schemaToolStripMenuItem");
			// 
			// blueLaserToolStripMenuItem
			// 
			this.blueLaserToolStripMenuItem.Name = "blueLaserToolStripMenuItem";
			resources.ApplyResources(this.blueLaserToolStripMenuItem, "blueLaserToolStripMenuItem");
			this.blueLaserToolStripMenuItem.Click += new System.EventHandler(this.blueLaserToolStripMenuItem_Click);
			// 
			// redLaserToolStripMenuItem
			// 
			this.redLaserToolStripMenuItem.Name = "redLaserToolStripMenuItem";
			resources.ApplyResources(this.redLaserToolStripMenuItem, "redLaserToolStripMenuItem");
			this.redLaserToolStripMenuItem.Click += new System.EventHandler(this.redLaserToolStripMenuItem_Click);
			// 
			// darkToolStripMenuItem
			// 
			this.darkToolStripMenuItem.Name = "darkToolStripMenuItem";
			resources.ApplyResources(this.darkToolStripMenuItem, "darkToolStripMenuItem");
			this.darkToolStripMenuItem.Click += new System.EventHandler(this.darkToolStripMenuItem_Click);
			// 
			// hackerToolStripMenuItem
			// 
			this.hackerToolStripMenuItem.Name = "hackerToolStripMenuItem";
			resources.ApplyResources(this.hackerToolStripMenuItem, "hackerToolStripMenuItem");
			this.hackerToolStripMenuItem.Click += new System.EventHandler(this.hackerToolStripMenuItem_Click);
			// 
			// nightyToolStripMenuItem
			// 
			this.nightyToolStripMenuItem.Name = "nightyToolStripMenuItem";
			resources.ApplyResources(this.nightyToolStripMenuItem, "nightyToolStripMenuItem");
			this.nightyToolStripMenuItem.Click += new System.EventHandler(this.nightyToolStripMenuItem_Click);
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
            this.turkishToolStripMenuItem});
			this.linguaToolStripMenuItem.Name = "linguaToolStripMenuItem";
			resources.ApplyResources(this.linguaToolStripMenuItem, "linguaToolStripMenuItem");
			// 
			// MNEnglish
			// 
			resources.ApplyResources(this.MNEnglish, "MNEnglish");
			this.MNEnglish.Name = "MNEnglish";
			this.MNEnglish.Click += new System.EventHandler(this.MNEnglish_Click);
			// 
			// MNItalian
			// 
			resources.ApplyResources(this.MNItalian, "MNItalian");
			this.MNItalian.Name = "MNItalian";
			this.MNItalian.Click += new System.EventHandler(this.MNItalian_Click);
			// 
			// MNSpanish
			// 
			resources.ApplyResources(this.MNSpanish, "MNSpanish");
			this.MNSpanish.Name = "MNSpanish";
			this.MNSpanish.Click += new System.EventHandler(this.MNSpanish_Click);
			// 
			// MNFrench
			// 
			resources.ApplyResources(this.MNFrench, "MNFrench");
			this.MNFrench.Name = "MNFrench";
			this.MNFrench.Click += new System.EventHandler(this.MNFrench_Click);
			// 
			// MNGerman
			// 
			resources.ApplyResources(this.MNGerman, "MNGerman");
			this.MNGerman.Name = "MNGerman";
			this.MNGerman.Click += new System.EventHandler(this.MNGerman_Click);
			// 
			// MNDanish
			// 
			resources.ApplyResources(this.MNDanish, "MNDanish");
			this.MNDanish.Name = "MNDanish";
			this.MNDanish.Click += new System.EventHandler(this.MNDanish_Click);
			// 
			// MNBrazilian
			// 
			resources.ApplyResources(this.MNBrazilian, "MNBrazilian");
			this.MNBrazilian.Name = "MNBrazilian";
			this.MNBrazilian.Click += new System.EventHandler(this.MNBrazilian_Click);
			// 
			// russianToolStripMenuItem
			// 
			resources.ApplyResources(this.russianToolStripMenuItem, "russianToolStripMenuItem");
			this.russianToolStripMenuItem.Name = "russianToolStripMenuItem";
			this.russianToolStripMenuItem.Click += new System.EventHandler(this.russianToolStripMenuItem_Click);
			// 
			// chinexeToolStripMenuItem
			// 
			resources.ApplyResources(this.chinexeToolStripMenuItem, "chinexeToolStripMenuItem");
			this.chinexeToolStripMenuItem.Name = "chinexeToolStripMenuItem";
			this.chinexeToolStripMenuItem.Click += new System.EventHandler(this.chineseToolStripMenuItem_Click);
			// 
			// traditionalChineseToolStripMenuItem
			// 
			resources.ApplyResources(this.traditionalChineseToolStripMenuItem, "traditionalChineseToolStripMenuItem");
			this.traditionalChineseToolStripMenuItem.Name = "traditionalChineseToolStripMenuItem";
			this.traditionalChineseToolStripMenuItem.Click += new System.EventHandler(this.traditionalChineseToolStripMenuItem_Click);
			// 
			// slovakianToolStripMenuItem
			// 
			resources.ApplyResources(this.slovakianToolStripMenuItem, "slovakianToolStripMenuItem");
			this.slovakianToolStripMenuItem.Name = "slovakianToolStripMenuItem";
			this.slovakianToolStripMenuItem.Click += new System.EventHandler(this.slovakToolStripMenuItem_Click);
			// 
			// hungarianToolStripMenuItem
			// 
			resources.ApplyResources(this.hungarianToolStripMenuItem, "hungarianToolStripMenuItem");
			this.hungarianToolStripMenuItem.Name = "hungarianToolStripMenuItem";
			this.hungarianToolStripMenuItem.Click += new System.EventHandler(this.hungarianToolStripMenuItem_Click);
			// 
			// czechToolStripMenuItem
			// 
			resources.ApplyResources(this.czechToolStripMenuItem, "czechToolStripMenuItem");
			this.czechToolStripMenuItem.Name = "czechToolStripMenuItem";
			this.czechToolStripMenuItem.Click += new System.EventHandler(this.czechToolStripMenuItem_Click);
			// 
			// polishToolStripMenuItem
			// 
			resources.ApplyResources(this.polishToolStripMenuItem, "polishToolStripMenuItem");
			this.polishToolStripMenuItem.Name = "polishToolStripMenuItem";
			this.polishToolStripMenuItem.Click += new System.EventHandler(this.polishToolStripMenuItem_Click);
			// 
			// greekToolStripMenuItem
			// 
			resources.ApplyResources(this.greekToolStripMenuItem, "greekToolStripMenuItem");
			this.greekToolStripMenuItem.Name = "greekToolStripMenuItem";
			this.greekToolStripMenuItem.Click += new System.EventHandler(this.greekToolStripMenuItem_Click);
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
			resources.ApplyResources(this.installCH340DriverToolStripMenuItem, "installCH340DriverToolStripMenuItem");
			this.installCH340DriverToolStripMenuItem.Name = "installCH340DriverToolStripMenuItem";
			this.installCH340DriverToolStripMenuItem.Click += new System.EventHandler(this.installCH340DriverToolStripMenuItem_Click);
			// 
			// flashGrblFirmwareToolStripMenuItem
			// 
			resources.ApplyResources(this.flashGrblFirmwareToolStripMenuItem, "flashGrblFirmwareToolStripMenuItem");
			this.flashGrblFirmwareToolStripMenuItem.Name = "flashGrblFirmwareToolStripMenuItem";
			this.flashGrblFirmwareToolStripMenuItem.Click += new System.EventHandler(this.flashGrblFirmwareToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			// 
			// configurationWizardToolStripMenuItem
			// 
			resources.ApplyResources(this.configurationWizardToolStripMenuItem, "configurationWizardToolStripMenuItem");
			this.configurationWizardToolStripMenuItem.Name = "configurationWizardToolStripMenuItem";
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
            this.firmwareToolStripMenuItem});
			this.MnOrtur.Name = "MnOrtur";
			resources.ApplyResources(this.MnOrtur, "MnOrtur");
			// 
			// orturSupportGroupToolStripMenuItem
			// 
			resources.ApplyResources(this.orturSupportGroupToolStripMenuItem, "orturSupportGroupToolStripMenuItem");
			this.orturSupportGroupToolStripMenuItem.Name = "orturSupportGroupToolStripMenuItem";
			this.orturSupportGroupToolStripMenuItem.Click += new System.EventHandler(this.orturSupportGroupToolStripMenuItem_Click);
			// 
			// orturSupportAndFeedbackToolStripMenuItem
			// 
			resources.ApplyResources(this.orturSupportAndFeedbackToolStripMenuItem, "orturSupportAndFeedbackToolStripMenuItem");
			this.orturSupportAndFeedbackToolStripMenuItem.Name = "orturSupportAndFeedbackToolStripMenuItem";
			this.orturSupportAndFeedbackToolStripMenuItem.Click += new System.EventHandler(this.orturSupportAndFeedbackToolStripMenuItem_Click);
			// 
			// orturWebsiteToolStripMenuItem
			// 
			resources.ApplyResources(this.orturWebsiteToolStripMenuItem, "orturWebsiteToolStripMenuItem");
			this.orturWebsiteToolStripMenuItem.Name = "orturWebsiteToolStripMenuItem";
			this.orturWebsiteToolStripMenuItem.Click += new System.EventHandler(this.orturWebsiteToolStripMenuItem_Click);
			// 
			// youtubeChannelToolStripMenuItem
			// 
			resources.ApplyResources(this.youtubeChannelToolStripMenuItem, "youtubeChannelToolStripMenuItem");
			this.youtubeChannelToolStripMenuItem.Name = "youtubeChannelToolStripMenuItem";
			this.youtubeChannelToolStripMenuItem.Click += new System.EventHandler(this.youtubeChannelToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			// 
			// manualsDownloadToolStripMenuItem
			// 
			resources.ApplyResources(this.manualsDownloadToolStripMenuItem, "manualsDownloadToolStripMenuItem");
			this.manualsDownloadToolStripMenuItem.Name = "manualsDownloadToolStripMenuItem";
			this.manualsDownloadToolStripMenuItem.Click += new System.EventHandler(this.manualsDownloadToolStripMenuItem_Click);
			// 
			// firmwareToolStripMenuItem
			// 
			resources.ApplyResources(this.firmwareToolStripMenuItem, "firmwareToolStripMenuItem");
			this.firmwareToolStripMenuItem.Name = "firmwareToolStripMenuItem";
			this.firmwareToolStripMenuItem.Click += new System.EventHandler(this.firmwareToolStripMenuItem_Click);
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
			resources.ApplyResources(this.openSessionLogToolStripMenuItem, "openSessionLogToolStripMenuItem");
			this.openSessionLogToolStripMenuItem.Name = "openSessionLogToolStripMenuItem";
			this.openSessionLogToolStripMenuItem.Click += new System.EventHandler(this.openSessionLogToolStripMenuItem_Click);
			// 
			// activateExtendedLogToolStripMenuItem
			// 
			resources.ApplyResources(this.activateExtendedLogToolStripMenuItem, "activateExtendedLogToolStripMenuItem");
			this.activateExtendedLogToolStripMenuItem.Name = "activateExtendedLogToolStripMenuItem";
			this.activateExtendedLogToolStripMenuItem.Click += new System.EventHandler(this.activateExtendedLogToolStripMenuItem_Click);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			resources.ApplyResources(this.toolStripMenuItem7, "toolStripMenuItem7");
			// 
			// helpOnLineToolStripMenuItem
			// 
			resources.ApplyResources(this.helpOnLineToolStripMenuItem, "helpOnLineToolStripMenuItem");
			this.helpOnLineToolStripMenuItem.Name = "helpOnLineToolStripMenuItem";
			this.helpOnLineToolStripMenuItem.Click += new System.EventHandler(this.helpOnLineToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// facebookCommunityToolStripMenuItem
			// 
			resources.ApplyResources(this.facebookCommunityToolStripMenuItem, "facebookCommunityToolStripMenuItem");
			this.facebookCommunityToolStripMenuItem.Name = "facebookCommunityToolStripMenuItem";
			this.facebookCommunityToolStripMenuItem.Click += new System.EventHandler(this.facebookCommunityToolStripMenuItem_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
			// 
			// donateToolStripMenuItem
			// 
			resources.ApplyResources(this.donateToolStripMenuItem, "donateToolStripMenuItem");
			this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
			this.donateToolStripMenuItem.Click += new System.EventHandler(this.donateToolStripMenuItem_Click);
			// 
			// licenseToolStripMenuItem
			// 
			resources.ApplyResources(this.licenseToolStripMenuItem, "licenseToolStripMenuItem");
			this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
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
			// turkishToolStripMenuItem
			// 
			resources.ApplyResources(this.turkishToolStripMenuItem, "turkishToolStripMenuItem");
			this.turkishToolStripMenuItem.Name = "turkishToolStripMenuItem";
			this.turkishToolStripMenuItem.Click += new System.EventHandler(this.turkishToolStripMenuItem_Click);
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
	}
}

