/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 05/12/2016
 * Time: 23:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace LaserGRBL
{
	partial class ConnectLogForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel GBCommands;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private LaserGRBL.UserControls.GrblTextBox TxtManualCommand;
		private LaserGRBL.UserControls.CommandLog CmdLog;
		private System.Windows.Forms.TextBox TbFileName;
		private LaserGRBL.UserControls.DoubleProgressBar PB;
		private LaserGRBL.UserControls.ImageButton BtnOpen;
		private LaserGRBL.UserControls.ImageButton BtnRunProgram;
		private System.Windows.Forms.Panel GBConnection;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Label LblComPort;
		private System.Windows.Forms.Label LblBaudRate;
		private System.Windows.Forms.ComboBox CBPort;
		private System.Windows.Forms.ComboBox CBSpeed;
		private LaserGRBL.UserControls.ImageButton BtnConnectDisconnect;
		private System.Windows.Forms.ToolTip TT;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectLogForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.GBCommands = new System.Windows.Forms.Panel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.TxtManualCommand = new LaserGRBL.UserControls.GrblTextBox();
            this.CmdLog = new LaserGRBL.UserControls.CommandLog();
            this.GBFile = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnFileAppend2 = new LaserGRBL.UserControls.ImageButton();
            this.BtnFileAppend1 = new LaserGRBL.UserControls.ImageButton();
            this.BtnFileAppend = new LaserGRBL.UserControls.ImageButton();
            this.chkFileEnable2 = new System.Windows.Forms.CheckBox();
            this.chkFileEnable1 = new System.Windows.Forms.CheckBox();
            this.UDLoopCounter2 = new System.Windows.Forms.NumericUpDown();
            this.UDLoopCounter1 = new System.Windows.Forms.NumericUpDown();
            this.btnFileSetup2 = new LaserGRBL.UserControls.ImageButton();
            this.btnFileSetup1 = new LaserGRBL.UserControls.ImageButton();
            this.btnFileSetup = new LaserGRBL.UserControls.ImageButton();
            this.BtnOpen2 = new LaserGRBL.UserControls.ImageButton();
            this.TbFileName2 = new System.Windows.Forms.TextBox();
            this.BtnOpen1 = new LaserGRBL.UserControls.ImageButton();
            this.TbFileName1 = new System.Windows.Forms.TextBox();
            this.LblFilename = new System.Windows.Forms.Label();
            this.TbFileName = new System.Windows.Forms.TextBox();
            this.PB = new LaserGRBL.UserControls.DoubleProgressBar();
            this.BtnOpen = new LaserGRBL.UserControls.ImageButton();
            this.LblFilename1 = new System.Windows.Forms.Label();
            this.UDLoopCounter = new System.Windows.Forms.NumericUpDown();
            this.BtnRunProgram = new LaserGRBL.UserControls.ImageButton();
            this.BtnAbortProgram = new LaserGRBL.UserControls.ImageButton();
            this.LblFilename2 = new System.Windows.Forms.Label();
            this.LblProgress = new System.Windows.Forms.Label();
            this.chkFileEnable = new System.Windows.Forms.CheckBox();
            this.GBConnection = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.LblEmulator = new System.Windows.Forms.Label();
            this.LblComPort = new System.Windows.Forms.Label();
            this.CBPort = new System.Windows.Forms.ComboBox();
            this.LblBaudRate = new System.Windows.Forms.Label();
            this.CBSpeed = new System.Windows.Forms.ComboBox();
            this.LblAddress = new System.Windows.Forms.Label();
            this.TxtAddress = new System.Windows.Forms.TextBox();
            this.TxtEmulator = new System.Windows.Forms.TextBox();
            this.BtnConnectDisconnect = new LaserGRBL.UserControls.ImageButton();
            this.TT = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.GBCommands.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.GBFile.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UDLoopCounter2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UDLoopCounter1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UDLoopCounter)).BeginInit();
            this.GBConnection.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.GBCommands, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.GBFile, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.GBConnection, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // GBCommands
            // 
            this.GBCommands.Controls.Add(this.tableLayoutPanel6);
            resources.ApplyResources(this.GBCommands, "GBCommands");
            this.GBCommands.Name = "GBCommands";
            // 
            // tableLayoutPanel6
            // 
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.Controls.Add(this.TxtManualCommand, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.CmdLog, 0, 1);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            // 
            // TxtManualCommand
            // 
            resources.ApplyResources(this.TxtManualCommand, "TxtManualCommand");
            this.TxtManualCommand.Name = "TxtManualCommand";
            this.TxtManualCommand.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
            this.TxtManualCommand.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtManualCommand.WaterMarkForeColor = System.Drawing.Color.LightGray;
            this.TxtManualCommand.CommandEntered += new LaserGRBL.UserControls.GrblTextBox.CommandEnteredDlg(this.TxtManualCommandCommandEntered);
            this.TxtManualCommand.Enter += new System.EventHandler(this.TxtManualCommand_Enter);
            this.TxtManualCommand.Leave += new System.EventHandler(this.TxtManualCommand_Leave);
            // 
            // CmdLog
            // 
            this.CmdLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.CmdLog, "CmdLog");
            this.CmdLog.Name = "CmdLog";
            this.CmdLog.TabStop = false;
            // 
            // GBFile
            // 
            resources.ApplyResources(this.GBFile, "GBFile");
            this.GBFile.Controls.Add(this.tableLayoutPanel5);
            this.GBFile.Name = "GBFile";
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.BtnFileAppend2, 3, 2);
            this.tableLayoutPanel5.Controls.Add(this.BtnFileAppend1, 3, 1);
            this.tableLayoutPanel5.Controls.Add(this.BtnFileAppend, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.chkFileEnable2, 6, 2);
            this.tableLayoutPanel5.Controls.Add(this.chkFileEnable1, 6, 1);
            this.tableLayoutPanel5.Controls.Add(this.UDLoopCounter2, 5, 2);
            this.tableLayoutPanel5.Controls.Add(this.UDLoopCounter1, 5, 1);
            this.tableLayoutPanel5.Controls.Add(this.btnFileSetup2, 4, 2);
            this.tableLayoutPanel5.Controls.Add(this.btnFileSetup1, 4, 1);
            this.tableLayoutPanel5.Controls.Add(this.btnFileSetup, 4, 0);
            this.tableLayoutPanel5.Controls.Add(this.BtnOpen2, 2, 2);
            this.tableLayoutPanel5.Controls.Add(this.TbFileName2, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.BtnOpen1, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.TbFileName1, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.LblFilename, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.TbFileName, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.PB, 1, 3);
            this.tableLayoutPanel5.Controls.Add(this.BtnOpen, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.LblFilename1, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.UDLoopCounter, 5, 0);
            this.tableLayoutPanel5.Controls.Add(this.BtnRunProgram, 5, 3);
            this.tableLayoutPanel5.Controls.Add(this.BtnAbortProgram, 6, 3);
            this.tableLayoutPanel5.Controls.Add(this.LblFilename2, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.LblProgress, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.chkFileEnable, 6, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // BtnFileAppend2
            // 
            this.BtnFileAppend2.AltImage = null;
            this.BtnFileAppend2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnFileAppend2.Caption = null;
            this.BtnFileAppend2.Coloration = System.Drawing.Color.Empty;
            resources.ApplyResources(this.BtnFileAppend2, "BtnFileAppend2");
            this.BtnFileAppend2.Image = ((System.Drawing.Image)(resources.GetObject("BtnFileAppend2.Image")));
            this.BtnFileAppend2.Name = "BtnFileAppend2";
            this.BtnFileAppend2.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnFileAppend2.TabStop = false;
            this.TT.SetToolTip(this.BtnFileAppend2, resources.GetString("BtnFileAppend2.ToolTip"));
            this.BtnFileAppend2.UseAltImage = false;
            this.BtnFileAppend2.Click += new System.EventHandler(this.BtnFileAppend2_Click);
            // 
            // BtnFileAppend1
            // 
            this.BtnFileAppend1.AltImage = null;
            this.BtnFileAppend1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnFileAppend1.Caption = null;
            this.BtnFileAppend1.Coloration = System.Drawing.Color.Empty;
            resources.ApplyResources(this.BtnFileAppend1, "BtnFileAppend1");
            this.BtnFileAppend1.Image = ((System.Drawing.Image)(resources.GetObject("BtnFileAppend1.Image")));
            this.BtnFileAppend1.Name = "BtnFileAppend1";
            this.BtnFileAppend1.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnFileAppend1.TabStop = false;
            this.TT.SetToolTip(this.BtnFileAppend1, resources.GetString("BtnFileAppend1.ToolTip"));
            this.BtnFileAppend1.UseAltImage = false;
            this.BtnFileAppend1.Click += new System.EventHandler(this.BtnFileAppend1_Click);
            // 
            // BtnFileAppend
            // 
            this.BtnFileAppend.AltImage = null;
            this.BtnFileAppend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnFileAppend.Caption = null;
            this.BtnFileAppend.Coloration = System.Drawing.Color.Empty;
            resources.ApplyResources(this.BtnFileAppend, "BtnFileAppend");
            this.BtnFileAppend.Image = ((System.Drawing.Image)(resources.GetObject("BtnFileAppend.Image")));
            this.BtnFileAppend.Name = "BtnFileAppend";
            this.BtnFileAppend.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnFileAppend.TabStop = false;
            this.TT.SetToolTip(this.BtnFileAppend, resources.GetString("BtnFileAppend.ToolTip"));
            this.BtnFileAppend.UseAltImage = false;
            this.BtnFileAppend.Click += new System.EventHandler(this.BtnFileAppend_Click);
            // 
            // chkFileEnable2
            // 
            resources.ApplyResources(this.chkFileEnable2, "chkFileEnable2");
            this.chkFileEnable2.Checked = true;
            this.chkFileEnable2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFileEnable2.Name = "chkFileEnable2";
            this.chkFileEnable2.UseVisualStyleBackColor = true;
            this.chkFileEnable2.CheckedChanged += new System.EventHandler(this.chkFileEnable2_CheckedChanged);
            // 
            // chkFileEnable1
            // 
            resources.ApplyResources(this.chkFileEnable1, "chkFileEnable1");
            this.chkFileEnable1.Checked = true;
            this.chkFileEnable1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFileEnable1.Name = "chkFileEnable1";
            this.chkFileEnable1.UseVisualStyleBackColor = true;
            this.chkFileEnable1.CheckedChanged += new System.EventHandler(this.chkFileEnable1_CheckedChanged);
            // 
            // UDLoopCounter2
            // 
            resources.ApplyResources(this.UDLoopCounter2, "UDLoopCounter2");
            this.UDLoopCounter2.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.UDLoopCounter2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UDLoopCounter2.Name = "UDLoopCounter2";
            this.TT.SetToolTip(this.UDLoopCounter2, resources.GetString("UDLoopCounter2.ToolTip"));
            this.UDLoopCounter2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UDLoopCounter2.ValueChanged += new System.EventHandler(this.UDLoopCounter2_ValueChanged);
            // 
            // UDLoopCounter1
            // 
            resources.ApplyResources(this.UDLoopCounter1, "UDLoopCounter1");
            this.UDLoopCounter1.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.UDLoopCounter1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UDLoopCounter1.Name = "UDLoopCounter1";
            this.TT.SetToolTip(this.UDLoopCounter1, resources.GetString("UDLoopCounter1.ToolTip"));
            this.UDLoopCounter1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UDLoopCounter1.ValueChanged += new System.EventHandler(this.UDLoopCounter1_ValueChanged);
            // 
            // btnFileSetup2
            // 
            this.btnFileSetup2.AltImage = null;
            this.btnFileSetup2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnFileSetup2.Caption = null;
            this.btnFileSetup2.Coloration = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnFileSetup2, "btnFileSetup2");
            this.btnFileSetup2.Image = ((System.Drawing.Image)(resources.GetObject("btnFileSetup2.Image")));
            this.btnFileSetup2.Name = "btnFileSetup2";
            this.btnFileSetup2.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.btnFileSetup2.TabStop = false;
            this.TT.SetToolTip(this.btnFileSetup2, resources.GetString("btnFileSetup2.ToolTip"));
            this.btnFileSetup2.UseAltImage = false;
            this.btnFileSetup2.Click += new System.EventHandler(this.BtnReOpen2_Click);
            // 
            // btnFileSetup1
            // 
            this.btnFileSetup1.AltImage = null;
            this.btnFileSetup1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnFileSetup1.Caption = null;
            this.btnFileSetup1.Coloration = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnFileSetup1, "btnFileSetup1");
            this.btnFileSetup1.Image = ((System.Drawing.Image)(resources.GetObject("btnFileSetup1.Image")));
            this.btnFileSetup1.Name = "btnFileSetup1";
            this.btnFileSetup1.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.btnFileSetup1.TabStop = false;
            this.TT.SetToolTip(this.btnFileSetup1, resources.GetString("btnFileSetup1.ToolTip"));
            this.btnFileSetup1.UseAltImage = false;
            this.btnFileSetup1.Click += new System.EventHandler(this.BtnReOpen1_Click);
            // 
            // btnFileSetup
            // 
            this.btnFileSetup.AltImage = null;
            this.btnFileSetup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnFileSetup.Caption = null;
            this.btnFileSetup.Coloration = System.Drawing.Color.Empty;
            resources.ApplyResources(this.btnFileSetup, "btnFileSetup");
            this.btnFileSetup.Image = ((System.Drawing.Image)(resources.GetObject("btnFileSetup.Image")));
            this.btnFileSetup.Name = "btnFileSetup";
            this.btnFileSetup.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.btnFileSetup.TabStop = false;
            this.TT.SetToolTip(this.btnFileSetup, resources.GetString("btnFileSetup.ToolTip"));
            this.btnFileSetup.UseAltImage = false;
            this.btnFileSetup.Click += new System.EventHandler(this.BtnReOpenClick);
            // 
            // BtnOpen2
            // 
            this.BtnOpen2.AltImage = null;
            this.BtnOpen2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnOpen2.Caption = null;
            this.BtnOpen2.Coloration = System.Drawing.Color.Empty;
            resources.ApplyResources(this.BtnOpen2, "BtnOpen2");
            this.BtnOpen2.Image = ((System.Drawing.Image)(resources.GetObject("BtnOpen2.Image")));
            this.BtnOpen2.Name = "BtnOpen2";
            this.BtnOpen2.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnOpen2.TabStop = false;
            this.TT.SetToolTip(this.BtnOpen2, resources.GetString("BtnOpen2.ToolTip"));
            this.BtnOpen2.UseAltImage = false;
            this.BtnOpen2.Click += new System.EventHandler(this.BtnOpen2_Click);
            // 
            // TbFileName2
            // 
            resources.ApplyResources(this.TbFileName2, "TbFileName2");
            this.TbFileName2.Name = "TbFileName2";
            this.TbFileName2.ReadOnly = true;
            this.TbFileName2.TabStop = false;
            this.TbFileName2.MouseEnter += new System.EventHandler(this.TbFileName2_MouseEnter);
            this.TbFileName2.MouseLeave += new System.EventHandler(this.TbFileName2_MouseLeave);
            // 
            // BtnOpen1
            // 
            this.BtnOpen1.AltImage = null;
            this.BtnOpen1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnOpen1.Caption = null;
            this.BtnOpen1.Coloration = System.Drawing.Color.Empty;
            resources.ApplyResources(this.BtnOpen1, "BtnOpen1");
            this.BtnOpen1.Image = ((System.Drawing.Image)(resources.GetObject("BtnOpen1.Image")));
            this.BtnOpen1.Name = "BtnOpen1";
            this.BtnOpen1.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnOpen1.TabStop = false;
            this.TT.SetToolTip(this.BtnOpen1, resources.GetString("BtnOpen1.ToolTip"));
            this.BtnOpen1.UseAltImage = false;
            this.BtnOpen1.Click += new System.EventHandler(this.BtnOpen1_Click);
            // 
            // TbFileName1
            // 
            resources.ApplyResources(this.TbFileName1, "TbFileName1");
            this.TbFileName1.Name = "TbFileName1";
            this.TbFileName1.ReadOnly = true;
            this.TbFileName1.TabStop = false;
            this.TbFileName1.MouseEnter += new System.EventHandler(this.TbFileName1_MouseEnter);
            this.TbFileName1.MouseLeave += new System.EventHandler(this.TbFileName1_MouseLeave);
            // 
            // LblFilename
            // 
            resources.ApplyResources(this.LblFilename, "LblFilename");
            this.LblFilename.Name = "LblFilename";
            // 
            // TbFileName
            // 
            resources.ApplyResources(this.TbFileName, "TbFileName");
            this.TbFileName.Name = "TbFileName";
            this.TbFileName.ReadOnly = true;
            this.TbFileName.TabStop = false;
            this.TbFileName.MouseEnter += new System.EventHandler(this.TbFileName_MouseEnter);
            this.TbFileName.MouseLeave += new System.EventHandler(this.TbFileName_MouseLeave);
            // 
            // PB
            // 
            this.PB.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.PB.BorderColor = System.Drawing.Color.Black;
            this.tableLayoutPanel5.SetColumnSpan(this.PB, 4);
            resources.ApplyResources(this.PB, "PB");
            this.PB.DrawProgressString = true;
            this.PB.FillColor = System.Drawing.Color.White;
            this.PB.FillStyle = LaserGRBL.UserControls.FillStyles.Solid;
            this.PB.ForeColor = System.Drawing.Color.Black;
            this.PB.Maximum = 100D;
            this.PB.Minimum = 0D;
            this.PB.Name = "PB";
            this.PB.PercString = null;
            this.PB.ProgressStringDecimals = 0;
            this.PB.Reverse = false;
            this.PB.Step = 10D;
            this.PB.ThrowExceprion = false;
            this.PB.Value = 0D;
            this.PB.Load += new System.EventHandler(this.PB_Load);
            // 
            // BtnOpen
            // 
            this.BtnOpen.AltImage = null;
            this.BtnOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnOpen.Caption = null;
            this.BtnOpen.Coloration = System.Drawing.Color.Empty;
            resources.ApplyResources(this.BtnOpen, "BtnOpen");
            this.BtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("BtnOpen.Image")));
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnOpen.TabStop = false;
            this.TT.SetToolTip(this.BtnOpen, resources.GetString("BtnOpen.ToolTip"));
            this.BtnOpen.UseAltImage = false;
            this.BtnOpen.Click += new System.EventHandler(this.BtnOpenClick);
            // 
            // LblFilename1
            // 
            resources.ApplyResources(this.LblFilename1, "LblFilename1");
            this.LblFilename1.Name = "LblFilename1";
            // 
            // UDLoopCounter
            // 
            resources.ApplyResources(this.UDLoopCounter, "UDLoopCounter");
            this.UDLoopCounter.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.UDLoopCounter.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UDLoopCounter.Name = "UDLoopCounter";
            this.TT.SetToolTip(this.UDLoopCounter, resources.GetString("UDLoopCounter.ToolTip"));
            this.UDLoopCounter.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UDLoopCounter.ValueChanged += new System.EventHandler(this.UDLoopCounter_ValueChanged);
            // 
            // BtnRunProgram
            // 
            this.BtnRunProgram.AltImage = null;
            this.BtnRunProgram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnRunProgram.Caption = null;
            this.BtnRunProgram.Coloration = System.Drawing.Color.Empty;
            resources.ApplyResources(this.BtnRunProgram, "BtnRunProgram");
            this.BtnRunProgram.Image = ((System.Drawing.Image)(resources.GetObject("BtnRunProgram.Image")));
            this.BtnRunProgram.Name = "BtnRunProgram";
            this.BtnRunProgram.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnRunProgram.TabStop = false;
            this.TT.SetToolTip(this.BtnRunProgram, resources.GetString("BtnRunProgram.ToolTip"));
            this.BtnRunProgram.UseAltImage = false;
            this.BtnRunProgram.Click += new System.EventHandler(this.BtnRunProgramClick);
            // 
            // BtnAbortProgram
            // 
            this.BtnAbortProgram.AltImage = null;
            resources.ApplyResources(this.BtnAbortProgram, "BtnAbortProgram");
            this.BtnAbortProgram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnAbortProgram.Caption = null;
            this.BtnAbortProgram.Coloration = System.Drawing.Color.Empty;
            this.BtnAbortProgram.Image = ((System.Drawing.Image)(resources.GetObject("BtnAbortProgram.Image")));
            this.BtnAbortProgram.Name = "BtnAbortProgram";
            this.BtnAbortProgram.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnAbortProgram.TabStop = false;
            this.TT.SetToolTip(this.BtnAbortProgram, resources.GetString("BtnAbortProgram.ToolTip"));
            this.BtnAbortProgram.UseAltImage = false;
            this.BtnAbortProgram.Click += new System.EventHandler(this.BtnAbortProgram_Click);
            // 
            // LblFilename2
            // 
            resources.ApplyResources(this.LblFilename2, "LblFilename2");
            this.LblFilename2.Name = "LblFilename2";
            // 
            // LblProgress
            // 
            resources.ApplyResources(this.LblProgress, "LblProgress");
            this.LblProgress.Name = "LblProgress";
            // 
            // chkFileEnable
            // 
            resources.ApplyResources(this.chkFileEnable, "chkFileEnable");
            this.chkFileEnable.Checked = true;
            this.chkFileEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFileEnable.Name = "chkFileEnable";
            this.chkFileEnable.UseVisualStyleBackColor = true;
            this.chkFileEnable.CheckedChanged += new System.EventHandler(this.chkFileEnable_CheckedChanged);
            // 
            // GBConnection
            // 
            resources.ApplyResources(this.GBConnection, "GBConnection");
            this.GBConnection.Controls.Add(this.tableLayoutPanel4);
            this.GBConnection.Name = "GBConnection";
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.LblEmulator, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.LblComPort, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.CBPort, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.LblBaudRate, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.CBSpeed, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.LblAddress, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.TxtAddress, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.TxtEmulator, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.BtnConnectDisconnect, 4, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // LblEmulator
            // 
            resources.ApplyResources(this.LblEmulator, "LblEmulator");
            this.LblEmulator.Name = "LblEmulator";
            // 
            // LblComPort
            // 
            resources.ApplyResources(this.LblComPort, "LblComPort");
            this.LblComPort.Name = "LblComPort";
            // 
            // CBPort
            // 
            resources.ApplyResources(this.CBPort, "CBPort");
            this.CBPort.FormattingEnabled = true;
            this.CBPort.Name = "CBPort";
            this.CBPort.SelectedIndexChanged += new System.EventHandler(this.CBPort_SelectedIndexChanged);
            this.CBPort.TextChanged += new System.EventHandler(this.CBPort_TextChanged);
            // 
            // LblBaudRate
            // 
            resources.ApplyResources(this.LblBaudRate, "LblBaudRate");
            this.LblBaudRate.Name = "LblBaudRate";
            // 
            // CBSpeed
            // 
            resources.ApplyResources(this.CBSpeed, "CBSpeed");
            this.CBSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBSpeed.FormattingEnabled = true;
            this.CBSpeed.Name = "CBSpeed";
            this.CBSpeed.SelectedIndexChanged += new System.EventHandler(this.CBSpeed_SelectedIndexChanged);
            // 
            // LblAddress
            // 
            resources.ApplyResources(this.LblAddress, "LblAddress");
            this.LblAddress.Name = "LblAddress";
            // 
            // TxtAddress
            // 
            resources.ApplyResources(this.TxtAddress, "TxtAddress");
            this.TxtAddress.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel4.SetColumnSpan(this.TxtAddress, 3);
            this.TxtAddress.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TxtAddress.Name = "TxtAddress";
            this.TxtAddress.TextChanged += new System.EventHandler(this.TxtHostName_TextChanged);
            // 
            // TxtEmulator
            // 
            resources.ApplyResources(this.TxtEmulator, "TxtEmulator");
            this.tableLayoutPanel4.SetColumnSpan(this.TxtEmulator, 3);
            this.TxtEmulator.Name = "TxtEmulator";
            // 
            // BtnConnectDisconnect
            // 
            this.BtnConnectDisconnect.AltImage = ((System.Drawing.Image)(resources.GetObject("BtnConnectDisconnect.AltImage")));
            resources.ApplyResources(this.BtnConnectDisconnect, "BtnConnectDisconnect");
            this.BtnConnectDisconnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnConnectDisconnect.Caption = null;
            this.BtnConnectDisconnect.Coloration = System.Drawing.Color.Empty;
            this.BtnConnectDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("BtnConnectDisconnect.Image")));
            this.BtnConnectDisconnect.Name = "BtnConnectDisconnect";
            this.tableLayoutPanel4.SetRowSpan(this.BtnConnectDisconnect, 3);
            this.BtnConnectDisconnect.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnConnectDisconnect.TabStop = false;
            this.TT.SetToolTip(this.BtnConnectDisconnect, resources.GetString("BtnConnectDisconnect.ToolTip"));
            this.BtnConnectDisconnect.UseAltImage = false;
            this.BtnConnectDisconnect.Click += new System.EventHandler(this.BtnConnectDisconnectClick);
            // 
            // ConnectLogForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ConnectLogForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.GBCommands.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.GBFile.ResumeLayout(false);
            this.GBFile.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UDLoopCounter2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UDLoopCounter1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UDLoopCounter)).EndInit();
            this.GBConnection.ResumeLayout(false);
            this.GBConnection.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

		}

		private System.Windows.Forms.Label LblProgress;
		private System.Windows.Forms.Label LblFilename;
		private System.Windows.Forms.Label LblAddress;
		private System.Windows.Forms.TextBox TxtAddress;
		private System.Windows.Forms.Panel GBFile;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label LblEmulator;
		private System.Windows.Forms.TextBox TxtEmulator;
        private UserControls.ImageButton BtnAbortProgram;
        private System.Windows.Forms.Label LblFilename1;
        private UserControls.ImageButton BtnOpen1;
        private System.Windows.Forms.TextBox TbFileName1;
        private UserControls.ImageButton btnFileSetup2;
        private UserControls.ImageButton btnFileSetup1;
        private UserControls.ImageButton btnFileSetup;
        private UserControls.ImageButton BtnOpen2;
        private System.Windows.Forms.TextBox TbFileName2;
        private System.Windows.Forms.Label LblFilename2;
        private System.Windows.Forms.CheckBox chkFileEnable2;
        private System.Windows.Forms.CheckBox chkFileEnable1;
        private System.Windows.Forms.CheckBox chkFileEnable;
        private UserControls.ImageButton BtnFileAppend;
        private UserControls.ImageButton BtnFileAppend2;
        private UserControls.ImageButton BtnFileAppend1;
        public System.Windows.Forms.NumericUpDown UDLoopCounter;
        public System.Windows.Forms.NumericUpDown UDLoopCounter2;
        public System.Windows.Forms.NumericUpDown UDLoopCounter1;
    }
}
