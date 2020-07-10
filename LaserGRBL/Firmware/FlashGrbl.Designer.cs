namespace LaserGRBL
{
	partial class FlashGrbl
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlashGrbl));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnOK = new System.Windows.Forms.Button();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.LblWarning = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.CbTarget = new System.Windows.Forms.ComboBox();
			this.LblTarget = new System.Windows.Forms.Label();
			this.CbFirmware = new System.Windows.Forms.ComboBox();
			this.LblFirmware = new System.Windows.Forms.Label();
			this.LblPort = new System.Windows.Forms.Label();
			this.CbPort = new System.Windows.Forms.ComboBox();
			this.LblSpeed = new System.Windows.Forms.Label();
			this.CbBaudRate = new System.Windows.Forms.ComboBox();
			this.BtnTarget = new LaserGRBL.UserControls.ImageButton();
			this.BtnFirmware = new LaserGRBL.UserControls.ImageButton();
			this.TT = new System.Windows.Forms.ToolTip(this.components);
			this.LblNotForOrtur = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.tableLayoutPanel5.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(317, 230);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.BtnCancel, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnOK, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 194);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(311, 33);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BtnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnCancel.Location = new System.Drawing.Point(154, 3);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(74, 27);
			this.BtnCancel.TabIndex = 4;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			// 
			// BtnOK
			// 
			this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BtnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnOK.Location = new System.Drawing.Point(234, 3);
			this.BtnOK.Name = "BtnOK";
			this.BtnOK.Size = new System.Drawing.Size(74, 27);
			this.BtnOK.TabIndex = 5;
			this.BtnOK.Text = "OK";
			this.BtnOK.UseVisualStyleBackColor = true;
			this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 2);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 3;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(311, 185);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.Controls.Add(this.LblNotForOrtur, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.LblWarning, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(305, 58);
			this.tableLayoutPanel4.TabIndex = 4;
			// 
			// LblWarning
			// 
			this.LblWarning.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.LblWarning.AutoSize = true;
			this.LblWarning.ForeColor = System.Drawing.Color.Red;
			this.LblWarning.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblWarning.Location = new System.Drawing.Point(44, 6);
			this.LblWarning.Margin = new System.Windows.Forms.Padding(2);
			this.LblWarning.Name = "LblWarning";
			this.LblWarning.Size = new System.Drawing.Size(255, 26);
			this.LblWarning.TabIndex = 3;
			this.LblWarning.Text = "Warning! This operation can damage your hardware.\r\nThe operation can be irreversi" +
    "ble: use at your risk.";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.pictureBox1.Location = new System.Drawing.Point(3, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.ColumnCount = 3;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel5.Controls.Add(this.CbTarget, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.LblTarget, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.CbFirmware, 1, 1);
			this.tableLayoutPanel5.Controls.Add(this.LblFirmware, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.LblPort, 0, 2);
			this.tableLayoutPanel5.Controls.Add(this.CbPort, 1, 2);
			this.tableLayoutPanel5.Controls.Add(this.LblSpeed, 0, 3);
			this.tableLayoutPanel5.Controls.Add(this.CbBaudRate, 1, 3);
			this.tableLayoutPanel5.Controls.Add(this.BtnTarget, 2, 0);
			this.tableLayoutPanel5.Controls.Add(this.BtnFirmware, 2, 1);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 67);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 5;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(305, 128);
			this.tableLayoutPanel5.TabIndex = 5;
			// 
			// CbTarget
			// 
			this.CbTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.CbTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbTarget.FormattingEnabled = true;
			this.CbTarget.Items.AddRange(new object[] {
            "Arduino Uno",
            "Arduino Nano"});
			this.CbTarget.Location = new System.Drawing.Point(70, 3);
			this.CbTarget.Name = "CbTarget";
			this.CbTarget.Size = new System.Drawing.Size(209, 21);
			this.CbTarget.TabIndex = 0;
			this.CbTarget.SelectedIndexChanged += new System.EventHandler(this.CbTarget_SelectedIndexChanged);
			// 
			// LblTarget
			// 
			this.LblTarget.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblTarget.AutoSize = true;
			this.LblTarget.Location = new System.Drawing.Point(3, 7);
			this.LblTarget.Name = "LblTarget";
			this.LblTarget.Size = new System.Drawing.Size(41, 13);
			this.LblTarget.TabIndex = 2;
			this.LblTarget.Text = "Target:";
			// 
			// CbFirmware
			// 
			this.CbFirmware.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.CbFirmware.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbFirmware.FormattingEnabled = true;
			this.CbFirmware.Location = new System.Drawing.Point(70, 30);
			this.CbFirmware.Name = "CbFirmware";
			this.CbFirmware.Size = new System.Drawing.Size(209, 21);
			this.CbFirmware.TabIndex = 1;
			// 
			// LblFirmware
			// 
			this.LblFirmware.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFirmware.AutoSize = true;
			this.LblFirmware.Location = new System.Drawing.Point(3, 34);
			this.LblFirmware.Name = "LblFirmware";
			this.LblFirmware.Size = new System.Drawing.Size(52, 13);
			this.LblFirmware.TabIndex = 0;
			this.LblFirmware.Text = "Firmware:";
			// 
			// LblPort
			// 
			this.LblPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblPort.AutoSize = true;
			this.LblPort.Location = new System.Drawing.Point(3, 61);
			this.LblPort.Name = "LblPort";
			this.LblPort.Size = new System.Drawing.Size(29, 13);
			this.LblPort.TabIndex = 1;
			this.LblPort.Text = "Port:";
			// 
			// CbPort
			// 
			this.CbPort.FormattingEnabled = true;
			this.CbPort.Location = new System.Drawing.Point(70, 57);
			this.CbPort.Name = "CbPort";
			this.CbPort.Size = new System.Drawing.Size(115, 21);
			this.CbPort.TabIndex = 2;
			this.CbPort.TextChanged += new System.EventHandler(this.CbPort_TextChanged);
			// 
			// LblSpeed
			// 
			this.LblSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblSpeed.AutoSize = true;
			this.LblSpeed.Location = new System.Drawing.Point(3, 88);
			this.LblSpeed.Name = "LblSpeed";
			this.LblSpeed.Size = new System.Drawing.Size(61, 13);
			this.LblSpeed.TabIndex = 3;
			this.LblSpeed.Text = "Baud Rate:";
			// 
			// CbBaudRate
			// 
			this.CbBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbBaudRate.FormattingEnabled = true;
			this.CbBaudRate.Location = new System.Drawing.Point(70, 84);
			this.CbBaudRate.Name = "CbBaudRate";
			this.CbBaudRate.Size = new System.Drawing.Size(115, 21);
			this.CbBaudRate.TabIndex = 3;
			// 
			// BtnTarget
			// 
			this.BtnTarget.AltImage = null;
			this.BtnTarget.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.BtnTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnTarget.Caption = null;
			this.BtnTarget.Coloration = System.Drawing.Color.Empty;
			this.BtnTarget.Image = ((System.Drawing.Image)(resources.GetObject("BtnTarget.Image")));
			this.BtnTarget.Location = new System.Drawing.Point(285, 5);
			this.BtnTarget.Name = "BtnTarget";
			this.BtnTarget.Size = new System.Drawing.Size(17, 17);
			this.BtnTarget.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnTarget.TabIndex = 23;
			this.TT.SetToolTip(this.BtnTarget, "Select exact kind of arduino board you want to upgrade.\r\nClick here for more info" +
        "...");
			this.BtnTarget.UseAltImage = false;
			this.BtnTarget.Click += new System.EventHandler(this.BtnTarget_Click);
			// 
			// BtnFirmware
			// 
			this.BtnFirmware.AltImage = null;
			this.BtnFirmware.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.BtnFirmware.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnFirmware.Caption = null;
			this.BtnFirmware.Coloration = System.Drawing.Color.Empty;
			this.BtnFirmware.Image = ((System.Drawing.Image)(resources.GetObject("BtnFirmware.Image")));
			this.BtnFirmware.Location = new System.Drawing.Point(285, 32);
			this.BtnFirmware.Name = "BtnFirmware";
			this.BtnFirmware.Size = new System.Drawing.Size(17, 17);
			this.BtnFirmware.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnFirmware.TabIndex = 24;
			this.TT.SetToolTip(this.BtnFirmware, "Select the firmware you want to flash on your board\r\nClick here if you need more " +
        "info about available versions...");
			this.BtnFirmware.UseAltImage = false;
			this.BtnFirmware.Click += new System.EventHandler(this.BtnFirmware_Click);
			// 
			// TT
			// 
			this.TT.AutoPopDelay = 10000;
			this.TT.InitialDelay = 500;
			this.TT.ReshowDelay = 100;
			// 
			// LblNotForOrtur
			// 
			this.LblNotForOrtur.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblNotForOrtur.AutoSize = true;
			this.LblNotForOrtur.ForeColor = System.Drawing.Color.Red;
			this.LblNotForOrtur.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblNotForOrtur.Location = new System.Drawing.Point(40, 41);
			this.LblNotForOrtur.Margin = new System.Windows.Forms.Padding(2);
			this.LblNotForOrtur.Name = "LblNotForOrtur";
			this.LblNotForOrtur.Size = new System.Drawing.Size(240, 13);
			this.LblNotForOrtur.TabIndex = 4;
			this.LblNotForOrtur.Text = "This procedure is not intended for Ortur Engraver!";
			// 
			// FlashGrbl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.BtnCancel;
			this.ClientSize = new System.Drawing.Size(317, 230);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FlashGrbl";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Flash Grbl Firmware";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button BtnOK;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label LblWarning;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label LblFirmware;
		private System.Windows.Forms.Label LblPort;
		private System.Windows.Forms.Label LblSpeed;
		private System.Windows.Forms.ComboBox CbFirmware;
		private System.Windows.Forms.ComboBox CbBaudRate;
		private System.Windows.Forms.ComboBox CbPort;
		private System.Windows.Forms.Label LblTarget;
		private System.Windows.Forms.ComboBox CbTarget;
		private System.Windows.Forms.Button BtnCancel;
		private UserControls.ImageButton BtnTarget;
		private UserControls.ImageButton BtnFirmware;
		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.Label LblNotForOrtur;
	}
}