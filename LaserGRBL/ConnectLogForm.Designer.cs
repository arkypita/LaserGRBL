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
		private System.Windows.Forms.Panel GBFile;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
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
			this.GBFile = new System.Windows.Forms.Panel();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.TbFileName = new System.Windows.Forms.TextBox();
			this.GBConnection = new System.Windows.Forms.Panel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.CBProtocol = new System.Windows.Forms.ComboBox();
			this.LblTcpPort = new System.Windows.Forms.Label();
			this.LblComPort = new System.Windows.Forms.Label();
			this.LblBaudRate = new System.Windows.Forms.Label();
			this.CBPort = new System.Windows.Forms.ComboBox();
			this.CBSpeed = new System.Windows.Forms.ComboBox();
			this.LblHostName = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.TxtHostName = new System.Windows.Forms.TextBox();
			this.TT = new System.Windows.Forms.ToolTip(this.components);
			this.TxtManualCommand = new LaserGRBL.UserControls.GrblTextBox();
			this.CmdLog = new LaserGRBL.UserControls.CommandLog();
			this.PB = new LaserGRBL.UserControls.DoubleProgressBar();
			this.BtnOpen = new LaserGRBL.UserControls.ImageButton();
			this.BtnRunProgram = new LaserGRBL.UserControls.ImageButton();
			this.BtnConnectDisconnect = new LaserGRBL.UserControls.ImageButton();
			this.ITcpPort = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.tableLayoutPanel1.SuspendLayout();
			this.GBCommands.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.GBFile.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.GBConnection.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDark;
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.GBCommands, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.GBFile, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.GBConnection, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// GBCommands
			// 
			this.GBCommands.BackColor = System.Drawing.SystemColors.Control;
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
			// GBFile
			// 
			resources.ApplyResources(this.GBFile, "GBFile");
			this.GBFile.BackColor = System.Drawing.SystemColors.Control;
			this.GBFile.Controls.Add(this.tableLayoutPanel5);
			this.GBFile.Name = "GBFile";
			// 
			// tableLayoutPanel5
			// 
			resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
			this.tableLayoutPanel5.Controls.Add(this.label5, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.label3, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.TbFileName, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.PB, 1, 1);
			this.tableLayoutPanel5.Controls.Add(this.BtnOpen, 2, 0);
			this.tableLayoutPanel5.Controls.Add(this.BtnRunProgram, 2, 1);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// TbFileName
			// 
			resources.ApplyResources(this.TbFileName, "TbFileName");
			this.TbFileName.Name = "TbFileName";
			this.TbFileName.ReadOnly = true;
			this.TbFileName.TabStop = false;
			// 
			// GBConnection
			// 
			resources.ApplyResources(this.GBConnection, "GBConnection");
			this.GBConnection.BackColor = System.Drawing.SystemColors.Control;
			this.GBConnection.Controls.Add(this.tableLayoutPanel4);
			this.GBConnection.Name = "GBConnection";
			// 
			// tableLayoutPanel4
			// 
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel4.Controls.Add(this.CBProtocol, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.LblTcpPort, 0, 4);
			this.tableLayoutPanel4.Controls.Add(this.LblComPort, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.LblBaudRate, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.CBPort, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.CBSpeed, 1, 2);
			this.tableLayoutPanel4.Controls.Add(this.BtnConnectDisconnect, 2, 1);
			this.tableLayoutPanel4.Controls.Add(this.LblHostName, 0, 3);
			this.tableLayoutPanel4.Controls.Add(this.label6, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.ITcpPort, 1, 4);
			this.tableLayoutPanel4.Controls.Add(this.TxtHostName, 1, 3);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			// 
			// CBProtocol
			// 
			this.tableLayoutPanel4.SetColumnSpan(this.CBProtocol, 2);
			resources.ApplyResources(this.CBProtocol, "CBProtocol");
			this.CBProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBProtocol.FormattingEnabled = true;
			this.CBProtocol.Name = "CBProtocol";
			this.CBProtocol.SelectedIndexChanged += new System.EventHandler(this.CBProtocol_SelectedIndexChanged);
			// 
			// LblTcpPort
			// 
			resources.ApplyResources(this.LblTcpPort, "LblTcpPort");
			this.LblTcpPort.Name = "LblTcpPort";
			// 
			// LblComPort
			// 
			resources.ApplyResources(this.LblComPort, "LblComPort");
			this.LblComPort.Name = "LblComPort";
			// 
			// LblBaudRate
			// 
			resources.ApplyResources(this.LblBaudRate, "LblBaudRate");
			this.LblBaudRate.Name = "LblBaudRate";
			// 
			// CBPort
			// 
			resources.ApplyResources(this.CBPort, "CBPort");
			this.CBPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBPort.FormattingEnabled = true;
			this.CBPort.Name = "CBPort";
			this.CBPort.SelectedIndexChanged += new System.EventHandler(this.CBPort_SelectedIndexChanged);
			// 
			// CBSpeed
			// 
			resources.ApplyResources(this.CBSpeed, "CBSpeed");
			this.CBSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBSpeed.FormattingEnabled = true;
			this.CBSpeed.Name = "CBSpeed";
			this.CBSpeed.SelectedIndexChanged += new System.EventHandler(this.CBSpeed_SelectedIndexChanged);
			// 
			// LblHostName
			// 
			resources.ApplyResources(this.LblHostName, "LblHostName");
			this.LblHostName.Name = "LblHostName";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// TxtHostName
			// 
			resources.ApplyResources(this.TxtHostName, "TxtHostName");
			this.TxtHostName.Name = "TxtHostName";
			this.TxtHostName.TextChanged += new System.EventHandler(this.TxtHostName_TextChanged);
			// 
			// TxtManualCommand
			// 
			this.TxtManualCommand.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			resources.ApplyResources(this.TxtManualCommand, "TxtManualCommand");
			this.TxtManualCommand.Name = "TxtManualCommand";
			this.TxtManualCommand.CommandEntered += new LaserGRBL.UserControls.GrblTextBox.CommandEnteredDlg(this.TxtManualCommandCommandEntered);
			// 
			// CmdLog
			// 
			this.CmdLog.BackColor = System.Drawing.Color.White;
			this.CmdLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(this.CmdLog, "CmdLog");
			this.CmdLog.Name = "CmdLog";
			this.CmdLog.TabStop = false;
			// 
			// PB
			// 
			this.PB.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.PB.BorderColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.PB, "PB");
			this.PB.DrawProgressString = true;
			this.PB.FillColor = System.Drawing.Color.White;
			this.PB.FillStyle = LaserGRBL.UserControls.FillStyles.Solid;
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
			this.BtnOpen.Coloration = System.Drawing.Color.Empty;
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
			this.BtnRunProgram.Coloration = System.Drawing.Color.Empty;
			this.BtnRunProgram.Image = ((System.Drawing.Image)(resources.GetObject("BtnRunProgram.Image")));
			this.BtnRunProgram.Name = "BtnRunProgram";
			this.BtnRunProgram.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnRunProgram.TabStop = false;
			this.TT.SetToolTip(this.BtnRunProgram, resources.GetString("BtnRunProgram.ToolTip"));
			this.BtnRunProgram.UseAltImage = false;
			this.BtnRunProgram.Click += new System.EventHandler(this.BtnRunProgramClick);
			// 
			// BtnConnectDisconnect
			// 
			this.BtnConnectDisconnect.AltImage = ((System.Drawing.Image)(resources.GetObject("BtnConnectDisconnect.AltImage")));
			this.BtnConnectDisconnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnConnectDisconnect.Coloration = System.Drawing.Color.Empty;
			resources.ApplyResources(this.BtnConnectDisconnect, "BtnConnectDisconnect");
			this.BtnConnectDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("BtnConnectDisconnect.Image")));
			this.BtnConnectDisconnect.Name = "BtnConnectDisconnect";
			this.tableLayoutPanel4.SetRowSpan(this.BtnConnectDisconnect, 4);
			this.BtnConnectDisconnect.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.BtnConnectDisconnect.TabStop = false;
			this.TT.SetToolTip(this.BtnConnectDisconnect, resources.GetString("BtnConnectDisconnect.ToolTip"));
			this.BtnConnectDisconnect.UseAltImage = false;
			this.BtnConnectDisconnect.Click += new System.EventHandler(this.BtnConnectDisconnectClick);
			// 
			// ITcpPort
			// 
			this.ITcpPort.ForcedText = null;
			this.ITcpPort.ForceMinMax = false;
			resources.ApplyResources(this.ITcpPort, "ITcpPort");
			this.ITcpPort.MaxValue = 65535;
			this.ITcpPort.Name = "ITcpPort";
			this.ITcpPort.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.ITcpPort_CurrentValueChanged);
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
			this.GBConnection.ResumeLayout(false);
			this.GBConnection.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.ResumeLayout(false);

		}

		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label LblTcpPort;
		private System.Windows.Forms.Label LblHostName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox CBProtocol;
		private UserControls.IntegerInput.IntegerInputRanged ITcpPort;
		private System.Windows.Forms.TextBox TxtHostName;
	}
}
