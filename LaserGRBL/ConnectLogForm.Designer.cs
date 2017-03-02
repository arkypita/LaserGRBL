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
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox TbFileName;
		private LaserGRBL.UserControls.DoubleProgressBar PB;
		private LaserGRBL.UserControls.ImageButton BtnOpen;
		private LaserGRBL.UserControls.ImageButton BtnRunProgram;
		private System.Windows.Forms.Panel GBConnection;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
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
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.TbFileName = new System.Windows.Forms.TextBox();
			this.PB = new LaserGRBL.UserControls.DoubleProgressBar();
			this.BtnOpen = new LaserGRBL.UserControls.ImageButton();
			this.BtnRunProgram = new LaserGRBL.UserControls.ImageButton();
			this.GBConnection = new System.Windows.Forms.Panel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.CBPort = new System.Windows.Forms.ComboBox();
			this.CBSpeed = new System.Windows.Forms.ComboBox();
			this.BtnConnectDisconnect = new LaserGRBL.UserControls.ImageButton();
			this.TT = new System.Windows.Forms.ToolTip(this.components);
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
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.tableLayoutPanel1.Controls.Add(this.GBCommands, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.GBFile, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.GBConnection, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.TT.SetToolTip(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.ToolTip"));
			// 
			// GBCommands
			// 
			resources.ApplyResources(this.GBCommands, "GBCommands");
			this.GBCommands.BackColor = System.Drawing.SystemColors.Control;
			this.GBCommands.Controls.Add(this.tableLayoutPanel6);
			this.GBCommands.Name = "GBCommands";
			this.TT.SetToolTip(this.GBCommands, resources.GetString("GBCommands.ToolTip"));
			// 
			// tableLayoutPanel6
			// 
			resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
			this.tableLayoutPanel6.Controls.Add(this.TxtManualCommand, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.CmdLog, 0, 1);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.TT.SetToolTip(this.tableLayoutPanel6, resources.GetString("tableLayoutPanel6.ToolTip"));
			// 
			// TxtManualCommand
			// 
			resources.ApplyResources(this.TxtManualCommand, "TxtManualCommand");
			this.TxtManualCommand.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.TxtManualCommand.Name = "TxtManualCommand";
			this.TT.SetToolTip(this.TxtManualCommand, resources.GetString("TxtManualCommand.ToolTip"));
			this.TxtManualCommand.CommandEntered += new LaserGRBL.UserControls.GrblTextBox.CommandEnteredDlg(this.TxtManualCommandCommandEntered);
			// 
			// CmdLog
			// 
			resources.ApplyResources(this.CmdLog, "CmdLog");
			this.CmdLog.BackColor = System.Drawing.Color.White;
			this.CmdLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.CmdLog.Name = "CmdLog";
			this.TT.SetToolTip(this.CmdLog, resources.GetString("CmdLog.ToolTip"));
			// 
			// GBFile
			// 
			resources.ApplyResources(this.GBFile, "GBFile");
			this.GBFile.BackColor = System.Drawing.SystemColors.Control;
			this.GBFile.Controls.Add(this.tableLayoutPanel5);
			this.GBFile.Name = "GBFile";
			this.TT.SetToolTip(this.GBFile, resources.GetString("GBFile.ToolTip"));
			// 
			// tableLayoutPanel5
			// 
			resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
			this.tableLayoutPanel5.Controls.Add(this.label5, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.label3, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.TbFileName, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.PB, 1, 1);
			this.tableLayoutPanel5.Controls.Add(this.BtnOpen, 3, 0);
			this.tableLayoutPanel5.Controls.Add(this.BtnRunProgram, 3, 1);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.TT.SetToolTip(this.tableLayoutPanel5, resources.GetString("tableLayoutPanel5.ToolTip"));
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			this.TT.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.TT.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
			// 
			// TbFileName
			// 
			resources.ApplyResources(this.TbFileName, "TbFileName");
			this.tableLayoutPanel5.SetColumnSpan(this.TbFileName, 2);
			this.TbFileName.Name = "TbFileName";
			this.TbFileName.ReadOnly = true;
			this.TT.SetToolTip(this.TbFileName, resources.GetString("TbFileName.ToolTip"));
			// 
			// PB
			// 
			resources.ApplyResources(this.PB, "PB");
			this.PB.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.PB.BorderColor = System.Drawing.Color.Black;
			this.tableLayoutPanel5.SetColumnSpan(this.PB, 2);
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
			this.TT.SetToolTip(this.PB, resources.GetString("PB.ToolTip"));
			this.PB.Value = 0D;
			// 
			// BtnOpen
			// 
			resources.ApplyResources(this.BtnOpen, "BtnOpen");
			this.BtnOpen.AltImage = null;
			this.BtnOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnOpen.Coloration = System.Drawing.Color.Empty;
			this.BtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("BtnOpen.Image")));
			this.BtnOpen.Name = "BtnOpen";
			this.BtnOpen.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtnOpen, resources.GetString("BtnOpen.ToolTip"));
			this.BtnOpen.UseAltImage = false;
			this.BtnOpen.Click += new System.EventHandler(this.BtnOpenClick);
			// 
			// BtnRunProgram
			// 
			resources.ApplyResources(this.BtnRunProgram, "BtnRunProgram");
			this.BtnRunProgram.AltImage = null;
			this.BtnRunProgram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnRunProgram.Coloration = System.Drawing.Color.Empty;
			this.BtnRunProgram.Image = ((System.Drawing.Image)(resources.GetObject("BtnRunProgram.Image")));
			this.BtnRunProgram.Name = "BtnRunProgram";
			this.BtnRunProgram.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtnRunProgram, resources.GetString("BtnRunProgram.ToolTip"));
			this.BtnRunProgram.UseAltImage = false;
			this.BtnRunProgram.Click += new System.EventHandler(this.BtnRunProgramClick);
			// 
			// GBConnection
			// 
			resources.ApplyResources(this.GBConnection, "GBConnection");
			this.GBConnection.BackColor = System.Drawing.SystemColors.Control;
			this.GBConnection.Controls.Add(this.tableLayoutPanel4);
			this.GBConnection.Name = "GBConnection";
			this.TT.SetToolTip(this.GBConnection, resources.GetString("GBConnection.ToolTip"));
			// 
			// tableLayoutPanel4
			// 
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.CBPort, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.CBSpeed, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.BtnConnectDisconnect, 2, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.TT.SetToolTip(this.tableLayoutPanel4, resources.GetString("tableLayoutPanel4.ToolTip"));
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.TT.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.TT.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
			// 
			// CBPort
			// 
			resources.ApplyResources(this.CBPort, "CBPort");
			this.CBPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBPort.FormattingEnabled = true;
			this.CBPort.Name = "CBPort";
			this.TT.SetToolTip(this.CBPort, resources.GetString("CBPort.ToolTip"));
			this.CBPort.SelectedIndexChanged += new System.EventHandler(this.CBPort_SelectedIndexChanged);
			// 
			// CBSpeed
			// 
			resources.ApplyResources(this.CBSpeed, "CBSpeed");
			this.CBSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBSpeed.FormattingEnabled = true;
			this.CBSpeed.Name = "CBSpeed";
			this.TT.SetToolTip(this.CBSpeed, resources.GetString("CBSpeed.ToolTip"));
			this.CBSpeed.SelectedIndexChanged += new System.EventHandler(this.CBSpeed_SelectedIndexChanged);
			// 
			// BtnConnectDisconnect
			// 
			resources.ApplyResources(this.BtnConnectDisconnect, "BtnConnectDisconnect");
			this.BtnConnectDisconnect.AltImage = ((System.Drawing.Image)(resources.GetObject("BtnConnectDisconnect.AltImage")));
			this.BtnConnectDisconnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnConnectDisconnect.Coloration = System.Drawing.Color.Empty;
			this.BtnConnectDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("BtnConnectDisconnect.Image")));
			this.BtnConnectDisconnect.Name = "BtnConnectDisconnect";
			this.tableLayoutPanel4.SetRowSpan(this.BtnConnectDisconnect, 2);
			this.BtnConnectDisconnect.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.TT.SetToolTip(this.BtnConnectDisconnect, resources.GetString("BtnConnectDisconnect.ToolTip"));
			this.BtnConnectDisconnect.UseAltImage = false;
			this.BtnConnectDisconnect.Click += new System.EventHandler(this.BtnConnectDisconnectClick);
			// 
			// ConnectLogForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.DockAreas = ((LaserGRBL.UserControls.DockingManager.DockAreas)(((LaserGRBL.UserControls.DockingManager.DockAreas.Float | LaserGRBL.UserControls.DockingManager.DockAreas.DockLeft) 
            | LaserGRBL.UserControls.DockingManager.DockAreas.DockRight)));
			this.Name = "ConnectLogForm";
			this.ShowHint = LaserGRBL.UserControls.DockingManager.DockState.DockLeft;
			this.TT.SetToolTip(this, resources.GetString("$this.ToolTip"));
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
	}
}
