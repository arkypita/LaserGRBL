namespace LaserGRBL.UI.Forms.RasterConverter
{
	partial class SetupLaser
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

		#region Codice generato da Progettazione componenti

		/// <summary> 
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupLaser));
			this.GbLaser = new System.Windows.Forms.GroupBox();
			this.BtnRepo = new LaserGRBL.UserControls.ImageButton();
			this.TLP = new System.Windows.Forms.TableLayoutPanel();
			this.label26 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.LblModWhiteCode = new System.Windows.Forms.Label();
			this.LblWhatModulate = new System.Windows.Forms.Label();
			this.CbModulate = new LaserGRBL.UserControls.EnumComboBox();
			this.LblModWhite = new System.Windows.Forms.Label();
			this.LblFilling = new System.Windows.Forms.Label();
			this.LblBorder = new System.Windows.Forms.Label();
			this.LblModBlack = new System.Windows.Forms.Label();
			this.IIModWhite = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.LblModBlackCode = new System.Windows.Forms.Label();
			this.IIModBlack = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.LblBorderUM = new System.Windows.Forms.Label();
			this.IIBorder = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.LblBorderCode = new System.Windows.Forms.Label();
			this.LblFillingCode = new System.Windows.Forms.Label();
			this.IIFilling = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.LblFillingUM = new System.Windows.Forms.Label();
			this.CBLaserON = new System.Windows.Forms.ComboBox();
			this.CBLaserOFF = new System.Windows.Forms.ComboBox();
			this.BtnOnOffInfo = new LaserGRBL.UserControls.ImageButton();
			this.BtnModulationInfo = new LaserGRBL.UserControls.ImageButton();
			this.LblModWhiteUM = new System.Windows.Forms.Label();
			this.LblModBlackUM = new System.Windows.Forms.Label();
			this.GbLaser.SuspendLayout();
			this.TLP.SuspendLayout();
			this.SuspendLayout();
			// 
			// GbLaser
			// 
			this.GbLaser.AutoSize = true;
			this.GbLaser.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GbLaser.Controls.Add(this.BtnRepo);
			this.GbLaser.Controls.Add(this.TLP);
			this.GbLaser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbLaser.Location = new System.Drawing.Point(0, 0);
			this.GbLaser.Name = "GbLaser";
			this.GbLaser.Size = new System.Drawing.Size(212, 182);
			this.GbLaser.TabIndex = 3;
			this.GbLaser.TabStop = false;
			this.GbLaser.Text = "Laser";
			// 
			// BtnRepo
			// 
			this.BtnRepo.AltImage = null;
			this.BtnRepo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnRepo.BackColor = System.Drawing.Color.Transparent;
			this.BtnRepo.Coloration = System.Drawing.Color.Empty;
			this.BtnRepo.Image = ((System.Drawing.Image)(resources.GetObject("BtnRepo.Image")));
			this.BtnRepo.Location = new System.Drawing.Point(189, 0);
			this.BtnRepo.Name = "BtnRepo";
			this.BtnRepo.Size = new System.Drawing.Size(17, 17);
			this.BtnRepo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnRepo.TabIndex = 2;
			this.BtnRepo.UseAltImage = false;
			// 
			// TLP
			// 
			this.TLP.AutoSize = true;
			this.TLP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP.ColumnCount = 4;
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP.Controls.Add(this.label26, 0, 6);
			this.TLP.Controls.Add(this.label18, 0, 5);
			this.TLP.Controls.Add(this.LblModWhiteCode, 1, 1);
			this.TLP.Controls.Add(this.LblWhatModulate, 0, 0);
			this.TLP.Controls.Add(this.CbModulate, 1, 0);
			this.TLP.Controls.Add(this.LblModWhite, 0, 1);
			this.TLP.Controls.Add(this.LblFilling, 0, 4);
			this.TLP.Controls.Add(this.LblBorder, 0, 3);
			this.TLP.Controls.Add(this.LblModBlack, 0, 2);
			this.TLP.Controls.Add(this.IIModWhite, 2, 1);
			this.TLP.Controls.Add(this.LblModBlackCode, 1, 2);
			this.TLP.Controls.Add(this.IIModBlack, 2, 2);
			this.TLP.Controls.Add(this.LblBorderUM, 3, 3);
			this.TLP.Controls.Add(this.IIBorder, 2, 3);
			this.TLP.Controls.Add(this.LblBorderCode, 1, 3);
			this.TLP.Controls.Add(this.LblFillingCode, 1, 4);
			this.TLP.Controls.Add(this.IIFilling, 2, 4);
			this.TLP.Controls.Add(this.LblFillingUM, 3, 4);
			this.TLP.Controls.Add(this.CBLaserON, 1, 5);
			this.TLP.Controls.Add(this.CBLaserOFF, 1, 6);
			this.TLP.Controls.Add(this.BtnOnOffInfo, 3, 5);
			this.TLP.Controls.Add(this.BtnModulationInfo, 3, 6);
			this.TLP.Controls.Add(this.LblModWhiteUM, 3, 1);
			this.TLP.Controls.Add(this.LblModBlackUM, 3, 2);
			this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP.Location = new System.Drawing.Point(3, 16);
			this.TLP.Name = "TLP";
			this.TLP.RowCount = 7;
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.Size = new System.Drawing.Size(206, 163);
			this.TLP.TabIndex = 0;
			// 
			// label26
			// 
			this.label26.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label26.AutoSize = true;
			this.label26.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label26.Location = new System.Drawing.Point(3, 143);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(56, 13);
			this.label26.TabIndex = 21;
			this.label26.Text = "Laser OFF";
			// 
			// label18
			// 
			this.label18.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label18.AutoSize = true;
			this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label18.Location = new System.Drawing.Point(3, 116);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(52, 13);
			this.label18.TabIndex = 19;
			this.label18.Text = "Laser ON";
			// 
			// LblModWhiteCode
			// 
			this.LblModWhiteCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblModWhiteCode.AutoSize = true;
			this.LblModWhiteCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblModWhiteCode.Location = new System.Drawing.Point(62, 29);
			this.LblModWhiteCode.Margin = new System.Windows.Forms.Padding(0);
			this.LblModWhiteCode.Name = "LblModWhiteCode";
			this.LblModWhiteCode.Size = new System.Drawing.Size(14, 13);
			this.LblModWhiteCode.TabIndex = 26;
			this.LblModWhiteCode.Text = "S";
			// 
			// LblWhatModulate
			// 
			this.LblWhatModulate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblWhatModulate.AutoSize = true;
			this.LblWhatModulate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblWhatModulate.Location = new System.Drawing.Point(3, 6);
			this.LblWhatModulate.Name = "LblWhatModulate";
			this.LblWhatModulate.Size = new System.Drawing.Size(51, 13);
			this.LblWhatModulate.TabIndex = 25;
			this.LblWhatModulate.Text = "Modulate";
			// 
			// CbModulate
			// 
			this.TLP.SetColumnSpan(this.CbModulate, 3);
			this.CbModulate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CbModulate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbModulate.FormattingEnabled = true;
			this.CbModulate.Location = new System.Drawing.Point(64, 2);
			this.CbModulate.Margin = new System.Windows.Forms.Padding(2);
			this.CbModulate.Name = "CbModulate";
			this.CbModulate.SelectedItem = null;
			this.CbModulate.Size = new System.Drawing.Size(140, 21);
			this.CbModulate.TabIndex = 24;
			this.CbModulate.SelectedIndexChanged += new System.EventHandler(this.CbModulate_SelectedIndexChanged);
			// 
			// LblModWhite
			// 
			this.LblModWhite.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblModWhite.AutoSize = true;
			this.LblModWhite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblModWhite.Location = new System.Drawing.Point(3, 29);
			this.LblModWhite.Name = "LblModWhite";
			this.LblModWhite.Size = new System.Drawing.Size(35, 13);
			this.LblModWhite.TabIndex = 13;
			this.LblModWhite.Text = "White";
			// 
			// LblFilling
			// 
			this.LblFilling.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFilling.AutoSize = true;
			this.LblFilling.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblFilling.Location = new System.Drawing.Point(3, 92);
			this.LblFilling.Name = "LblFilling";
			this.LblFilling.Size = new System.Drawing.Size(33, 13);
			this.LblFilling.TabIndex = 13;
			this.LblFilling.Text = "Filling";
			// 
			// LblBorder
			// 
			this.LblBorder.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblBorder.AutoSize = true;
			this.LblBorder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblBorder.Location = new System.Drawing.Point(3, 71);
			this.LblBorder.Name = "LblBorder";
			this.LblBorder.Size = new System.Drawing.Size(38, 13);
			this.LblBorder.TabIndex = 23;
			this.LblBorder.Text = "Border";
			// 
			// LblModBlack
			// 
			this.LblModBlack.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblModBlack.AutoSize = true;
			this.LblModBlack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblModBlack.Location = new System.Drawing.Point(3, 50);
			this.LblModBlack.Name = "LblModBlack";
			this.LblModBlack.Size = new System.Drawing.Size(34, 13);
			this.LblModBlack.TabIndex = 17;
			this.LblModBlack.Text = "Black";
			// 
			// IIModWhite
			// 
			this.IIModWhite.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIModWhite.ForcedText = null;
			this.IIModWhite.ForceMinMax = false;
			this.IIModWhite.Location = new System.Drawing.Point(79, 28);
			this.IIModWhite.MaxValue = 999;
			this.IIModWhite.MinValue = 0;
			this.IIModWhite.Name = "IIModWhite";
			this.IIModWhite.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIModWhite.Size = new System.Drawing.Size(52, 15);
			this.IIModWhite.TabIndex = 11;
			// 
			// LblModBlackCode
			// 
			this.LblModBlackCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblModBlackCode.AutoSize = true;
			this.LblModBlackCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblModBlackCode.Location = new System.Drawing.Point(62, 50);
			this.LblModBlackCode.Margin = new System.Windows.Forms.Padding(0);
			this.LblModBlackCode.Name = "LblModBlackCode";
			this.LblModBlackCode.Size = new System.Drawing.Size(14, 13);
			this.LblModBlackCode.TabIndex = 27;
			this.LblModBlackCode.Text = "S";
			// 
			// IIModBlack
			// 
			this.IIModBlack.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIModBlack.CurrentValue = 255;
			this.IIModBlack.ForcedText = null;
			this.IIModBlack.ForceMinMax = false;
			this.IIModBlack.Location = new System.Drawing.Point(79, 49);
			this.IIModBlack.MaxValue = 1000;
			this.IIModBlack.MinValue = 1;
			this.IIModBlack.Name = "IIModBlack";
			this.IIModBlack.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIModBlack.Size = new System.Drawing.Size(52, 15);
			this.IIModBlack.TabIndex = 12;
			// 
			// LblBorderUM
			// 
			this.LblBorderUM.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblBorderUM.AutoSize = true;
			this.LblBorderUM.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblBorderUM.Location = new System.Drawing.Point(137, 71);
			this.LblBorderUM.Name = "LblBorderUM";
			this.LblBorderUM.Size = new System.Drawing.Size(44, 13);
			this.LblBorderUM.TabIndex = 22;
			this.LblBorderUM.Text = "mm/min";
			// 
			// IIBorder
			// 
			this.IIBorder.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIBorder.CurrentValue = 1000;
			this.IIBorder.ForcedText = null;
			this.IIBorder.ForceMinMax = false;
			this.IIBorder.Location = new System.Drawing.Point(79, 70);
			this.IIBorder.MaxValue = 4000;
			this.IIBorder.MinValue = 1;
			this.IIBorder.Name = "IIBorder";
			this.IIBorder.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIBorder.Size = new System.Drawing.Size(52, 15);
			this.IIBorder.TabIndex = 6;
			// 
			// LblBorderCode
			// 
			this.LblBorderCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblBorderCode.AutoSize = true;
			this.LblBorderCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblBorderCode.Location = new System.Drawing.Point(62, 71);
			this.LblBorderCode.Margin = new System.Windows.Forms.Padding(0);
			this.LblBorderCode.Name = "LblBorderCode";
			this.LblBorderCode.Size = new System.Drawing.Size(13, 13);
			this.LblBorderCode.TabIndex = 28;
			this.LblBorderCode.Text = "F";
			// 
			// LblFillingCode
			// 
			this.LblFillingCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFillingCode.AutoSize = true;
			this.LblFillingCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblFillingCode.Location = new System.Drawing.Point(62, 92);
			this.LblFillingCode.Margin = new System.Windows.Forms.Padding(0);
			this.LblFillingCode.Name = "LblFillingCode";
			this.LblFillingCode.Size = new System.Drawing.Size(13, 13);
			this.LblFillingCode.TabIndex = 29;
			this.LblFillingCode.Text = "F";
			// 
			// IIFilling
			// 
			this.IIFilling.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIFilling.CurrentValue = 1000;
			this.IIFilling.ForcedText = null;
			this.IIFilling.ForceMinMax = false;
			this.IIFilling.Location = new System.Drawing.Point(79, 91);
			this.IIFilling.MaxValue = 4000;
			this.IIFilling.MinValue = 1;
			this.IIFilling.Name = "IIFilling";
			this.IIFilling.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIFilling.Size = new System.Drawing.Size(52, 15);
			this.IIFilling.TabIndex = 7;
			// 
			// LblFillingUM
			// 
			this.LblFillingUM.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFillingUM.AutoSize = true;
			this.LblFillingUM.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblFillingUM.Location = new System.Drawing.Point(137, 92);
			this.LblFillingUM.Name = "LblFillingUM";
			this.LblFillingUM.Size = new System.Drawing.Size(44, 13);
			this.LblFillingUM.TabIndex = 15;
			this.LblFillingUM.Text = "mm/min";
			// 
			// CBLaserON
			// 
			this.TLP.SetColumnSpan(this.CBLaserON, 2);
			this.CBLaserON.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CBLaserON.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBLaserON.FormattingEnabled = true;
			this.CBLaserON.Items.AddRange(new object[] {
            "M3",
            "M4"});
			this.CBLaserON.Location = new System.Drawing.Point(65, 112);
			this.CBLaserON.Name = "CBLaserON";
			this.CBLaserON.Size = new System.Drawing.Size(66, 21);
			this.CBLaserON.TabIndex = 24;
			// 
			// CBLaserOFF
			// 
			this.TLP.SetColumnSpan(this.CBLaserOFF, 2);
			this.CBLaserOFF.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CBLaserOFF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBLaserOFF.FormattingEnabled = true;
			this.CBLaserOFF.Items.AddRange(new object[] {
            "M5"});
			this.CBLaserOFF.Location = new System.Drawing.Point(65, 139);
			this.CBLaserOFF.Name = "CBLaserOFF";
			this.CBLaserOFF.Size = new System.Drawing.Size(66, 21);
			this.CBLaserOFF.TabIndex = 25;
			// 
			// BtnOnOffInfo
			// 
			this.BtnOnOffInfo.AltImage = null;
			this.BtnOnOffInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.BtnOnOffInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnOnOffInfo.Coloration = System.Drawing.Color.Empty;
			this.BtnOnOffInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnOnOffInfo.Image")));
			this.BtnOnOffInfo.Location = new System.Drawing.Point(137, 114);
			this.BtnOnOffInfo.Name = "BtnOnOffInfo";
			this.BtnOnOffInfo.Size = new System.Drawing.Size(17, 17);
			this.BtnOnOffInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnOnOffInfo.TabIndex = 22;
			this.BtnOnOffInfo.UseAltImage = false;
			// 
			// BtnModulationInfo
			// 
			this.BtnModulationInfo.AltImage = null;
			this.BtnModulationInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.BtnModulationInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnModulationInfo.Coloration = System.Drawing.Color.Empty;
			this.BtnModulationInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnModulationInfo.Image")));
			this.BtnModulationInfo.Location = new System.Drawing.Point(137, 141);
			this.BtnModulationInfo.Name = "BtnModulationInfo";
			this.BtnModulationInfo.Size = new System.Drawing.Size(17, 17);
			this.BtnModulationInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnModulationInfo.TabIndex = 23;
			this.BtnModulationInfo.UseAltImage = false;
			// 
			// LblModWhiteUM
			// 
			this.LblModWhiteUM.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblModWhiteUM.AutoSize = true;
			this.LblModWhiteUM.Location = new System.Drawing.Point(137, 29);
			this.LblModWhiteUM.Name = "LblModWhiteUM";
			this.LblModWhiteUM.Size = new System.Drawing.Size(34, 13);
			this.LblModWhiteUM.TabIndex = 30;
			this.LblModWhiteUM.Text = "PWM";
			// 
			// LblModBlackUM
			// 
			this.LblModBlackUM.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblModBlackUM.AutoSize = true;
			this.LblModBlackUM.Location = new System.Drawing.Point(137, 50);
			this.LblModBlackUM.Name = "LblModBlackUM";
			this.LblModBlackUM.Size = new System.Drawing.Size(34, 13);
			this.LblModBlackUM.TabIndex = 31;
			this.LblModBlackUM.Text = "PWM";
			// 
			// SetupLaser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.GbLaser);
			this.Name = "SetupLaser";
			this.Size = new System.Drawing.Size(212, 182);
			this.GbLaser.ResumeLayout(false);
			this.GbLaser.PerformLayout();
			this.TLP.ResumeLayout(false);
			this.TLP.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox GbLaser;
		private System.Windows.Forms.TableLayoutPanel TLP;
		private System.Windows.Forms.Label LblBorder;
		private System.Windows.Forms.Label LblBorderUM;
		private UserControls.IntegerInput.IntegerInputRanged IIBorder;
		private UserControls.IntegerInput.IntegerInputRanged IIFilling;
		private System.Windows.Forms.Label LblFillingUM;
		private System.Windows.Forms.Label LblFilling;
		private UserControls.ImageButton BtnModulationInfo;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label LblModWhite;
		private UserControls.IntegerInput.IntegerInputRanged IIModWhite;
		private System.Windows.Forms.Label LblModBlack;
		private UserControls.IntegerInput.IntegerInputRanged IIModBlack;
		private System.Windows.Forms.Label label18;
		private UserControls.ImageButton BtnOnOffInfo;
		private System.Windows.Forms.ComboBox CBLaserON;
		private System.Windows.Forms.ComboBox CBLaserOFF;
		private System.Windows.Forms.Label LblWhatModulate;
		private UserControls.EnumComboBox CbModulate;
		private System.Windows.Forms.Label LblModWhiteCode;
		private System.Windows.Forms.Label LblModBlackCode;
		private System.Windows.Forms.Label LblBorderCode;
		private System.Windows.Forms.Label LblFillingCode;
		private UserControls.ImageButton BtnRepo;
		private System.Windows.Forms.Label LblModWhiteUM;
		private System.Windows.Forms.Label LblModBlackUM;

	}
}
