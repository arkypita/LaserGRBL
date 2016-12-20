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
		private System.Windows.Forms.ComboBox comboBox3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox TbFileName;
		private LaserGRBL.UserControls.DoubleProgressBar PB;
		private LaserGRBL.UserControls.ImageButton BtnJobOptions;
		private LaserGRBL.UserControls.ImageButton BtnOpen;
		private LaserGRBL.UserControls.ImageButton BtnRunProgram;
		private System.Windows.Forms.Panel GBConnection;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox CBPort;
		private System.Windows.Forms.ComboBox CBSpeed;
		private LaserGRBL.UserControls.ImageButton BtnConnectDisconnect;
		
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectLogForm));
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
			this.tableLayoutPanel1.Size = new System.Drawing.Size(333, 468);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// GBCommands
			// 
			this.GBCommands.BackColor = System.Drawing.SystemColors.Control;
			this.GBCommands.Controls.Add(this.tableLayoutPanel6);
			this.GBCommands.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GBCommands.Location = new System.Drawing.Point(1, 115);
			this.GBCommands.Margin = new System.Windows.Forms.Padding(1);
			this.GBCommands.Name = "GBCommands";
			this.GBCommands.Size = new System.Drawing.Size(331, 352);
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
			this.tableLayoutPanel6.Size = new System.Drawing.Size(331, 352);
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
			this.TxtManualCommand.Size = new System.Drawing.Size(329, 22);
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
			this.CmdLog.Size = new System.Drawing.Size(325, 323);
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
			this.GBFile.Size = new System.Drawing.Size(331, 67);
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
			this.tableLayoutPanel5.Size = new System.Drawing.Size(331, 67);
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
			this.comboBox3.Size = new System.Drawing.Size(259, 21);
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
			this.TbFileName.Size = new System.Drawing.Size(259, 20);
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
			this.PB.Size = new System.Drawing.Size(259, 23);
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
			this.BtnJobOptions.Location = new System.Drawing.Point(313, 24);
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
			this.BtnOpen.Location = new System.Drawing.Point(313, 2);
			this.BtnOpen.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.BtnOpen.Name = "BtnOpen";
			this.BtnOpen.Size = new System.Drawing.Size(17, 17);
			this.BtnOpen.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnOpen.TabIndex = 2;
			this.BtnOpen.UseAltImage = false;
			this.BtnOpen.Click += new System.EventHandler(this.BtnOpenClick);
			// 
			// BtnRunProgram
			// 
			this.BtnRunProgram.AltImage = null;
			this.BtnRunProgram.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.BtnRunProgram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnRunProgram.Coloration = System.Drawing.Color.Empty;
			this.BtnRunProgram.Enabled = false;
			this.BtnRunProgram.Image = ((System.Drawing.Image)(resources.GetObject("BtnRunProgram.Image")));
			this.BtnRunProgram.Location = new System.Drawing.Point(313, 47);
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
			this.GBConnection.Size = new System.Drawing.Size(331, 45);
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
			this.tableLayoutPanel4.Size = new System.Drawing.Size(331, 45);
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
			this.CBPort.Size = new System.Drawing.Size(223, 21);
			this.CBPort.TabIndex = 2;
			this.CBPort.SelectedIndexChanged += new System.EventHandler(this.CBPort_SelectedIndexChanged);
			// 
			// CBSpeed
			// 
			this.CBSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CBSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBSpeed.FormattingEnabled = true;
			this.CBSpeed.Location = new System.Drawing.Point(61, 23);
			this.CBSpeed.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.CBSpeed.Name = "CBSpeed";
			this.CBSpeed.Size = new System.Drawing.Size(223, 21);
			this.CBSpeed.TabIndex = 3;
			this.CBSpeed.SelectedIndexChanged += new System.EventHandler(this.CBSpeed_SelectedIndexChanged);
			// 
			// BtnConnectDisconnect
			// 
			this.BtnConnectDisconnect.AltImage = ((System.Drawing.Image)(resources.GetObject("BtnConnectDisconnect.AltImage")));
			this.BtnConnectDisconnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnConnectDisconnect.Coloration = System.Drawing.Color.Empty;
			this.BtnConnectDisconnect.Dock = System.Windows.Forms.DockStyle.Left;
			this.BtnConnectDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("BtnConnectDisconnect.Image")));
			this.BtnConnectDisconnect.Location = new System.Drawing.Point(286, 1);
			this.BtnConnectDisconnect.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
			this.BtnConnectDisconnect.Name = "BtnConnectDisconnect";
			this.tableLayoutPanel4.SetRowSpan(this.BtnConnectDisconnect, 2);
			this.BtnConnectDisconnect.Size = new System.Drawing.Size(44, 44);
			this.BtnConnectDisconnect.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.BtnConnectDisconnect.TabIndex = 4;
			this.BtnConnectDisconnect.UseAltImage = false;
			this.BtnConnectDisconnect.Click += new System.EventHandler(this.BtnConnectDisconnectClick);
			// 
			// ConnectLogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(333, 468);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.DockAreas = ((LaserGRBL.UserControls.DockingManager.DockAreas)(((LaserGRBL.UserControls.DockingManager.DockAreas.Float | LaserGRBL.UserControls.DockingManager.DockAreas.DockLeft) 
            | LaserGRBL.UserControls.DockingManager.DockAreas.DockRight)));
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "ConnectLogForm";
			this.ShowHint = LaserGRBL.UserControls.DockingManager.DockState.DockLeft;
			this.Text = "Connection";
			this.ToolTipText = "Connection";
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
