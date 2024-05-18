/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 05/12/2016
 * Time: 23:41
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Drawing;

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
		private UserControls.TextInput TbFileName;
		private LaserGRBL.UserControls.DoubleProgressBar PB;
		private LaserGRBL.UserControls.ImageButton BtnOpen;
		private LaserGRBL.UserControls.ImageButton BtnRunProgram;
		private System.Windows.Forms.Panel GBConnection;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Label LblComPort;
		private System.Windows.Forms.Label LblBaudRate;
		private LaserGRBL.UserControls.FlatComboBox CBPort;
		private LaserGRBL.UserControls.FlatComboBox CBSpeed;
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
            this.LblProgress = new System.Windows.Forms.Label();
            this.LblFilename = new System.Windows.Forms.Label();
            this.TbFileName = new UserControls.TextInput();
            this.PB = new LaserGRBL.UserControls.DoubleProgressBar();
            this.BtnOpen = new LaserGRBL.UserControls.ImageButton();
            this.BtnRunProgram = new LaserGRBL.UserControls.ImageButton();
            this.UDLoopCounter = new LaserGRBL.UserControls.NumericInput.NumericUpDown();
            this.BtnAbortProgram = new LaserGRBL.UserControls.ImageButton();
            this.GBConnection = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.LblEmulator = new System.Windows.Forms.Label();
            this.LblComPort = new System.Windows.Forms.Label();
            this.CBPort = new LaserGRBL.UserControls.FlatComboBox();
            this.LblBaudRate = new System.Windows.Forms.Label();
            this.CBSpeed = new LaserGRBL.UserControls.FlatComboBox();
            this.LblAddress = new System.Windows.Forms.Label();
            this.TxtAddress = new UserControls.TextInput();
            this.TxtEmulator = new UserControls.TextInput();
            this.BtnConnectDisconnect = new LaserGRBL.UserControls.ImageButton();
            this.TT = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.GBCommands.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.GBFile.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
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
            resources.ApplyResources(this.CmdLog, "CmdLog");
            this.CmdLog.Name = "CmdLog";
            this.CmdLog.TabStop = false;
            this.CmdLog.Paint += new System.Windows.Forms.PaintEventHandler(this.CmdLog_Paint);
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
            this.tableLayoutPanel5.Controls.Add(this.LblProgress, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.LblFilename, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.TbFileName, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.PB, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.BtnOpen, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.BtnRunProgram, 3, 1);
            this.tableLayoutPanel5.Controls.Add(this.UDLoopCounter, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.BtnAbortProgram, 4, 1);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // LblProgress
            // 
            resources.ApplyResources(this.LblProgress, "LblProgress");
            this.LblProgress.Name = "LblProgress";
            // 
            // LblFilename
            // 
            resources.ApplyResources(this.LblFilename, "LblFilename");
            this.LblFilename.Name = "LblFilename";
            // 
            // TbFileName
            // 
            this.TbFileName.BackColor = System.Drawing.Color.White;
            this.TbFileName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel5.SetColumnSpan(this.TbFileName, 2);
            resources.ApplyResources(this.TbFileName, "TbFileName");
            this.TbFileName.Name = "TbFileName";
            this.TbFileName.ReadOnly = true;
            this.TbFileName.TabStop = false;
            this.TbFileName.Height = 20;
            this.TbFileName.MouseEnter += new System.EventHandler(this.TbFileName_MouseEnter);
            this.TbFileName.MouseLeave += new System.EventHandler(this.TbFileName_MouseLeave);
            // 
            // PB
            // 
            this.PB.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.PB.BorderColor = System.Drawing.Color.Black;
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
            // 
            // BtnOpen
            // 
            this.BtnOpen.AltImage = null;
            resources.ApplyResources(this.BtnOpen, "BtnOpen");
            this.BtnOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnOpen.Caption = null;
            this.tableLayoutPanel5.SetColumnSpan(this.BtnOpen, 2);
            this.BtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("BtnOpen.Image")));
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnOpen.TabStop = false;
            this.TT.SetToolTip(this.BtnOpen, resources.GetString("BtnOpen.ToolTip"));
            this.BtnOpen.UseAltImage = false;
            this.BtnOpen.Click += new System.EventHandler(this.BtnOpenClick);
            // 
            // BtnRunProgram
            // 
            this.BtnRunProgram.AltImage = null;
            resources.ApplyResources(this.BtnRunProgram, "BtnRunProgram");
            this.BtnRunProgram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnRunProgram.Caption = null;
            this.BtnRunProgram.Image = ((System.Drawing.Image)(resources.GetObject("BtnRunProgram.Image")));
            this.BtnRunProgram.Name = "BtnRunProgram";
            this.BtnRunProgram.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnRunProgram.TabStop = false;
            this.TT.SetToolTip(this.BtnRunProgram, resources.GetString("BtnRunProgram.ToolTip"));
            this.BtnRunProgram.UseAltImage = false;
            this.BtnRunProgram.Click += new System.EventHandler(this.BtnRunProgramClick);
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
            // BtnAbortProgram
            // 
            this.BtnAbortProgram.AltImage = null;
            resources.ApplyResources(this.BtnAbortProgram, "BtnAbortProgram");
            this.BtnAbortProgram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnAbortProgram.Caption = null;
            this.BtnAbortProgram.Image = ((System.Drawing.Image)(resources.GetObject("BtnAbortProgram.Image")));
            this.BtnAbortProgram.Name = "BtnAbortProgram";
            this.BtnAbortProgram.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnAbortProgram.TabStop = false;
            this.TT.SetToolTip(this.BtnAbortProgram, resources.GetString("BtnAbortProgram.ToolTip"));
            this.BtnAbortProgram.UseAltImage = false;
            this.BtnAbortProgram.Click += new System.EventHandler(this.BtnAbortProgram_Click);
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
            this.CBPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
		private UserControls.TextInput TxtAddress;
		private System.Windows.Forms.Panel GBFile;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private UserControls.NumericInput.NumericUpDown UDLoopCounter;
		private System.Windows.Forms.Label LblEmulator;
		private UserControls.TextInput TxtEmulator;
        private UserControls.ImageButton BtnAbortProgram;
    }
}
