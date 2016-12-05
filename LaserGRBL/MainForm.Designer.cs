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
			this.Splitter = new System.Windows.Forms.SplitContainer();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.GBCommands = new System.Windows.Forms.Panel();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.TxtManualCommand = new LaserGRBL.UserControls.GrblTextBox();
			this.CmdLog = new LaserGRBL.UserControls.CommandLog();
			this.GBFile = new System.Windows.Forms.Panel();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.comboBox3 = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.TbFileName = new System.Windows.Forms.TextBox();
			this.PB = new LaserGRBL.UserControls.DoubleProgressBar();
			this.BtnJobOptions = new LaserGRBL.UserControls.ImageButton();
			this.BtnOpen = new LaserGRBL.UserControls.ImageButton();
			this.BtnRunProgram = new LaserGRBL.UserControls.ImageButton();
			this.GBConnection = new System.Windows.Forms.Panel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.CBPort = new System.Windows.Forms.ComboBox();
			this.CBSpeed = new System.Windows.Forms.ComboBox();
			this.BtnConnectDisconnect = new LaserGRBL.UserControls.ImageButton();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.Preview = new LaserGRBL.UserControls.GrblPanel();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnReset = new LaserGRBL.UserControls.ImageButton();
			this.BtnGoHome = new LaserGRBL.UserControls.ImageButton();
			this.BtnStop = new LaserGRBL.UserControls.ImageButton();
			this.BtnResume = new LaserGRBL.UserControls.ImageButton();
			this.StatusBar = new System.Windows.Forms.StatusStrip();
			this.TTLFile = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTFile = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLLines = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTLines = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLEstimated = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTEstimated = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTLLoadedIn = new System.Windows.Forms.ToolStripStatusLabel();
			this.TTTLoadedIn = new System.Windows.Forms.ToolStripStatusLabel();
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
			((System.ComponentModel.ISupportInitialize)(this.Splitter)).BeginInit();
			this.Splitter.Panel1.SuspendLayout();
			this.Splitter.Panel2.SuspendLayout();
			this.Splitter.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.GBCommands.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.GBFile.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.GBConnection.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			this.StatusBar.SuspendLayout();
			this.MMn.SuspendLayout();
			this.SuspendLayout();
			// 
			// Splitter
			// 
			this.Splitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Splitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.Splitter.Location = new System.Drawing.Point(0, 24);
			this.Splitter.Name = "Splitter";
			// 
			// Splitter.Panel1
			// 
			this.Splitter.Panel1.Controls.Add(this.tableLayoutPanel1);
			// 
			// Splitter.Panel2
			// 
			this.Splitter.Panel2.Controls.Add(this.tableLayoutPanel2);
			this.Splitter.Size = new System.Drawing.Size(1006, 622);
			this.Splitter.SplitterDistance = 313;
			this.Splitter.SplitterWidth = 1;
			this.Splitter.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.GBCommands, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.GBFile, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.GBConnection, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(313, 622);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// GBCommands
			// 
			this.GBCommands.BackColor = System.Drawing.SystemColors.Control;
			this.GBCommands.Controls.Add(this.tableLayoutPanel6);
			this.GBCommands.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GBCommands.Location = new System.Drawing.Point(1, 115);
			this.GBCommands.Margin = new System.Windows.Forms.Padding(1);
			this.GBCommands.Name = "GBCommands";
			this.GBCommands.Size = new System.Drawing.Size(311, 506);
			this.GBCommands.TabIndex = 2;
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.ColumnCount = 1;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.Controls.Add(this.TxtManualCommand, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.CmdLog, 0, 1);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 2;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size(311, 506);
			this.tableLayoutPanel6.TabIndex = 0;
			// 
			// TxtManualCommand
			// 
			this.TxtManualCommand.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.TxtManualCommand.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TxtManualCommand.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TxtManualCommand.Location = new System.Drawing.Point(1, 1);
			this.TxtManualCommand.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.TxtManualCommand.Name = "TxtManualCommand";
			this.TxtManualCommand.Size = new System.Drawing.Size(309, 22);
			this.TxtManualCommand.TabIndex = 4;
			this.TxtManualCommand.CommandEntered += new LaserGRBL.UserControls.GrblTextBox.CommandEnteredDlg(this.TxtManualCommandCommandEntered);
			// 
			// CmdLog
			// 
			this.CmdLog.BackColor = System.Drawing.Color.White;
			this.CmdLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.CmdLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CmdLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CmdLog.Location = new System.Drawing.Point(3, 26);
			this.CmdLog.Name = "CmdLog";
			this.CmdLog.Size = new System.Drawing.Size(305, 477);
			this.CmdLog.TabIndex = 5;
			// 
			// GBFile
			// 
			this.GBFile.AutoSize = true;
			this.GBFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GBFile.BackColor = System.Drawing.SystemColors.Control;
			this.GBFile.Controls.Add(this.tableLayoutPanel5);
			this.GBFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GBFile.Location = new System.Drawing.Point(1, 47);
			this.GBFile.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.GBFile.Name = "GBFile";
			this.GBFile.Size = new System.Drawing.Size(311, 67);
			this.GBFile.TabIndex = 1;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel5.ColumnCount = 4;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel5.Controls.Add(this.comboBox3, 1, 1);
			this.tableLayoutPanel5.Controls.Add(this.label5, 0, 2);
			this.tableLayoutPanel5.Controls.Add(this.label4, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.label3, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.TbFileName, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.PB, 1, 2);
			this.tableLayoutPanel5.Controls.Add(this.BtnJobOptions, 3, 1);
			this.tableLayoutPanel5.Controls.Add(this.BtnOpen, 3, 0);
			this.tableLayoutPanel5.Controls.Add(this.BtnRunProgram, 3, 2);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 3;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(311, 67);
			this.tableLayoutPanel5.TabIndex = 0;
			// 
			// comboBox3
			// 
			this.tableLayoutPanel5.SetColumnSpan(this.comboBox3, 2);
			this.comboBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Location = new System.Drawing.Point(52, 22);
			this.comboBox3.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(239, 21);
			this.comboBox3.TabIndex = 9;
			this.comboBox3.Visible = false;
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(1, 49);
			this.label5.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Progress";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(1, 26);
			this.label4.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(37, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Preset";
			this.label4.Visible = false;
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(1, 4);
			this.label3.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(49, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Filename";
			// 
			// TbFileName
			// 
			this.tableLayoutPanel5.SetColumnSpan(this.TbFileName, 2);
			this.TbFileName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TbFileName.Location = new System.Drawing.Point(52, 1);
			this.TbFileName.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.TbFileName.Name = "TbFileName";
			this.TbFileName.ReadOnly = true;
			this.TbFileName.Size = new System.Drawing.Size(239, 20);
			this.TbFileName.TabIndex = 1;
			// 
			// PB
			// 
			this.PB.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.PB.BorderColor = System.Drawing.Color.Black;
			this.tableLayoutPanel5.SetColumnSpan(this.PB, 2);
			this.PB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PB.DrawProgressString = true;
			this.PB.FillColor = System.Drawing.Color.White;
			this.PB.FillStyle = LaserGRBL.UserControls.FillStyles.Solid;
			this.PB.Location = new System.Drawing.Point(52, 44);
			this.PB.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.PB.Maximum = 100D;
			this.PB.Minimum = 0D;
			this.PB.Name = "PB";
			this.PB.PercString = null;
			this.PB.ProgressStringDecimals = 0;
			this.PB.Reverse = false;
			this.PB.Size = new System.Drawing.Size(239, 23);
			this.PB.Step = 10D;
			this.PB.TabIndex = 7;
			this.PB.ThrowExceprion = false;
			this.PB.Value = 0D;
			// 
			// BtnJobOptions
			// 
			this.BtnJobOptions.AltImage = null;
			this.BtnJobOptions.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.BtnJobOptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnJobOptions.Coloration = System.Drawing.Color.Empty;
			this.BtnJobOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnJobOptions.Image")));
			this.BtnJobOptions.Location = new System.Drawing.Point(293, 24);
			this.BtnJobOptions.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.BtnJobOptions.Name = "BtnJobOptions";
			this.BtnJobOptions.Size = new System.Drawing.Size(17, 17);
			this.BtnJobOptions.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnJobOptions.TabIndex = 10;
			this.BtnJobOptions.UseAltImage = false;
			this.BtnJobOptions.Visible = false;
			// 
			// BtnOpen
			// 
			this.BtnOpen.AltImage = null;
			this.BtnOpen.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.BtnOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnOpen.Coloration = System.Drawing.Color.Empty;
			this.BtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("BtnOpen.Image")));
			this.BtnOpen.Location = new System.Drawing.Point(293, 2);
			this.BtnOpen.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.BtnOpen.Name = "BtnOpen";
			this.BtnOpen.Size = new System.Drawing.Size(17, 17);
			this.BtnOpen.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnOpen.TabIndex = 2;
			this.BtnOpen.UseAltImage = false;
			this.BtnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
			// 
			// BtnRunProgram
			// 
			this.BtnRunProgram.AltImage = null;
			this.BtnRunProgram.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.BtnRunProgram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnRunProgram.Coloration = System.Drawing.Color.Empty;
			this.BtnRunProgram.Enabled = false;
			this.BtnRunProgram.Image = ((System.Drawing.Image)(resources.GetObject("BtnRunProgram.Image")));
			this.BtnRunProgram.Location = new System.Drawing.Point(293, 47);
			this.BtnRunProgram.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.BtnRunProgram.Name = "BtnRunProgram";
			this.BtnRunProgram.Size = new System.Drawing.Size(17, 17);
			this.BtnRunProgram.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnRunProgram.TabIndex = 3;
			this.BtnRunProgram.UseAltImage = false;
			this.BtnRunProgram.Click += new System.EventHandler(this.BtnRunProgramClick);
			// 
			// GBConnection
			// 
			this.GBConnection.AutoSize = true;
			this.GBConnection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GBConnection.BackColor = System.Drawing.SystemColors.Control;
			this.GBConnection.Controls.Add(this.tableLayoutPanel4);
			this.GBConnection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GBConnection.Location = new System.Drawing.Point(1, 1);
			this.GBConnection.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.GBConnection.Name = "GBConnection";
			this.GBConnection.Size = new System.Drawing.Size(311, 45);
			this.GBConnection.TabIndex = 0;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel4.ColumnCount = 3;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.CBPort, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.CBSpeed, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.BtnConnectDisconnect, 2, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(311, 45);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(1, 5);
			this.label1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "COM Port";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(1, 27);
			this.label2.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Baud Rate";
			// 
			// CBPort
			// 
			this.CBPort.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CBPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBPort.FormattingEnabled = true;
			this.CBPort.Location = new System.Drawing.Point(61, 1);
			this.CBPort.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.CBPort.Name = "CBPort";
			this.CBPort.Size = new System.Drawing.Size(203, 21);
			this.CBPort.TabIndex = 2;
			// 
			// CBSpeed
			// 
			this.CBSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CBSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBSpeed.FormattingEnabled = true;
			this.CBSpeed.Location = new System.Drawing.Point(61, 23);
			this.CBSpeed.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.CBSpeed.Name = "CBSpeed";
			this.CBSpeed.Size = new System.Drawing.Size(203, 21);
			this.CBSpeed.TabIndex = 3;
			// 
			// BtnConnectDisconnect
			// 
			this.BtnConnectDisconnect.AltImage = ((System.Drawing.Image)(resources.GetObject("BtnConnectDisconnect.AltImage")));
			this.BtnConnectDisconnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnConnectDisconnect.Coloration = System.Drawing.Color.Empty;
			this.BtnConnectDisconnect.Dock = System.Windows.Forms.DockStyle.Left;
			this.BtnConnectDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("BtnConnectDisconnect.Image")));
			this.BtnConnectDisconnect.Location = new System.Drawing.Point(266, 1);
			this.BtnConnectDisconnect.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.BtnConnectDisconnect.Name = "BtnConnectDisconnect";
			this.tableLayoutPanel4.SetRowSpan(this.BtnConnectDisconnect, 2);
			this.BtnConnectDisconnect.Size = new System.Drawing.Size(44, 44);
			this.BtnConnectDisconnect.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.BtnConnectDisconnect.TabIndex = 4;
			this.BtnConnectDisconnect.UseAltImage = false;
			this.BtnConnectDisconnect.Click += new System.EventHandler(this.BtnConnectDisconnect_Click);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.Preview, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel8, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(692, 622);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// Preview
			// 
			this.Preview.BackColor = System.Drawing.SystemColors.Info;
			this.Preview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Preview.Location = new System.Drawing.Point(1, 1);
			this.Preview.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.Preview.Name = "Preview";
			this.Preview.Size = new System.Drawing.Size(690, 564);
			this.Preview.TabIndex = 3;
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.AutoSize = true;
			this.tableLayoutPanel8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel8.BackColor = System.Drawing.SystemColors.Control;
			this.tableLayoutPanel8.ColumnCount = 5;
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel8.Controls.Add(this.BtnReset, 4, 0);
			this.tableLayoutPanel8.Controls.Add(this.BtnGoHome, 0, 0);
			this.tableLayoutPanel8.Controls.Add(this.BtnStop, 1, 0);
			this.tableLayoutPanel8.Controls.Add(this.BtnResume, 2, 0);
			this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel8.Location = new System.Drawing.Point(1, 566);
			this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(1);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 1;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.Size = new System.Drawing.Size(690, 55);
			this.tableLayoutPanel8.TabIndex = 3;
			// 
			// BtnReset
			// 
			this.BtnReset.AltImage = null;
			this.BtnReset.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.BtnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnReset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BtnReset.Coloration = System.Drawing.Color.Empty;
			this.BtnReset.Enabled = false;
			this.BtnReset.Image = ((System.Drawing.Image)(resources.GetObject("BtnReset.Image")));
			this.BtnReset.Location = new System.Drawing.Point(638, 3);
			this.BtnReset.Name = "BtnReset";
			this.BtnReset.Size = new System.Drawing.Size(49, 49);
			this.BtnReset.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnReset.TabIndex = 4;
			this.BtnReset.UseAltImage = false;
			this.BtnReset.Click += new System.EventHandler(this.BtnResetClick);
			// 
			// BtnGoHome
			// 
			this.BtnGoHome.AltImage = null;
			this.BtnGoHome.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.BtnGoHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnGoHome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BtnGoHome.Coloration = System.Drawing.Color.Empty;
			this.BtnGoHome.Enabled = false;
			this.BtnGoHome.Image = ((System.Drawing.Image)(resources.GetObject("BtnGoHome.Image")));
			this.BtnGoHome.Location = new System.Drawing.Point(3, 3);
			this.BtnGoHome.Name = "BtnGoHome";
			this.BtnGoHome.Size = new System.Drawing.Size(49, 49);
			this.BtnGoHome.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnGoHome.TabIndex = 3;
			this.BtnGoHome.UseAltImage = false;
			this.BtnGoHome.Click += new System.EventHandler(this.BtnGoHomeClick);
			// 
			// BtnStop
			// 
			this.BtnStop.AltImage = ((System.Drawing.Image)(resources.GetObject("BtnStop.AltImage")));
			this.BtnStop.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.BtnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnStop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BtnStop.Coloration = System.Drawing.Color.Empty;
			this.BtnStop.Enabled = false;
			this.BtnStop.Image = ((System.Drawing.Image)(resources.GetObject("BtnStop.Image")));
			this.BtnStop.Location = new System.Drawing.Point(58, 3);
			this.BtnStop.Name = "BtnStop";
			this.BtnStop.Size = new System.Drawing.Size(49, 49);
			this.BtnStop.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnStop.TabIndex = 5;
			this.BtnStop.UseAltImage = false;
			this.BtnStop.Click += new System.EventHandler(this.BtnStopClick);
			// 
			// BtnResume
			// 
			this.BtnResume.AltImage = null;
			this.BtnResume.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.BtnResume.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnResume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BtnResume.Coloration = System.Drawing.Color.Empty;
			this.BtnResume.Enabled = false;
			this.BtnResume.Image = ((System.Drawing.Image)(resources.GetObject("BtnResume.Image")));
			this.BtnResume.Location = new System.Drawing.Point(113, 3);
			this.BtnResume.Name = "BtnResume";
			this.BtnResume.Size = new System.Drawing.Size(49, 49);
			this.BtnResume.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnResume.TabIndex = 6;
			this.BtnResume.UseAltImage = false;
			this.BtnResume.Click += new System.EventHandler(this.BtnResumeClick);
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
			this.StatusBar.Location = new System.Drawing.Point(0, 646);
			this.StatusBar.Name = "StatusBar";
			this.StatusBar.Size = new System.Drawing.Size(1006, 24);
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
			// spring
			// 
			this.spring.Name = "spring";
			this.spring.Size = new System.Drawing.Size(533, 19);
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
            this.grblToolStripMenuItem});
			this.MMn.Location = new System.Drawing.Point(0, 0);
			this.MMn.Name = "MMn";
			this.MMn.Size = new System.Drawing.Size(1006, 24);
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
			this.MnFileOpen.Click += new System.EventHandler(this.BtnOpen_Click);
			// 
			// MnFileSend
			// 
			this.MnFileSend.Name = "MnFileSend";
			this.MnFileSend.Size = new System.Drawing.Size(103, 22);
			this.MnFileSend.Text = "&Send";
			this.MnFileSend.Click += new System.EventHandler(this.BtnRunProgramClick);
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
			this.MnGrblReset.Click += new System.EventHandler(this.BtnResetClick);
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
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1006, 670);
			this.Controls.Add(this.Splitter);
			this.Controls.Add(this.StatusBar);
			this.Controls.Add(this.MMn);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.MMn;
			this.Name = "MainForm";
			this.Text = "Laser GRBL";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Splitter.Panel1.ResumeLayout(false);
			this.Splitter.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.Splitter)).EndInit();
			this.Splitter.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.GBCommands.ResumeLayout(false);
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.GBFile.ResumeLayout(false);
			this.GBFile.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.GBConnection.ResumeLayout(false);
			this.GBConnection.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel8.ResumeLayout(false);
			this.StatusBar.ResumeLayout(false);
			this.StatusBar.PerformLayout();
			this.MMn.ResumeLayout(false);
			this.MMn.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		
		private System.Windows.Forms.SplitContainer Splitter;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel GBConnection;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Panel GBFile;
		private System.Windows.Forms.Panel GBCommands;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox CBPort;
		private System.Windows.Forms.ComboBox CBSpeed;
		private UserControls.ImageButton BtnConnectDisconnect;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox TbFileName;
		private UserControls.ImageButton BtnOpen;
		private System.Windows.Forms.Label label4;
		private UserControls.ImageButton BtnRunProgram;
		private UserControls.DoubleProgressBar PB;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboBox3;
		private UserControls.ImageButton BtnJobOptions;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private UserControls.GrblTextBox TxtManualCommand;
		private UserControls.GrblPanel Preview;
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
		private UserControls.CommandLog CmdLog;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private UserControls.ImageButton BtnStop;
		private UserControls.ImageButton BtnReset;
		private UserControls.ImageButton BtnGoHome;
		private LaserGRBL.UserControls.ImageButton BtnResume;
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

	}
}

