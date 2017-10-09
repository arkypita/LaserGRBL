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
			this.GbSpeed = new System.Windows.Forms.GroupBox();
			this.TLP = new System.Windows.Forms.TableLayoutPanel();
			this.label26 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.LblWhatModulate = new System.Windows.Forms.Label();
			this.LblModMin = new System.Windows.Forms.Label();
			this.LblFP2 = new System.Windows.Forms.Label();
			this.LblFP1 = new System.Windows.Forms.Label();
			this.LblModMax = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.LblFP1u = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.LblLFP2u = new System.Windows.Forms.Label();
			this.CBLaserON = new System.Windows.Forms.ComboBox();
			this.CBLaserOFF = new System.Windows.Forms.ComboBox();
			this.CbTools = new LaserGRBL.UserControls.EnumComboBox();
			this.IIModMin = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IIModMax = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IIFixedParam = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IILinearFilling = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.BtnOnOffInfo = new LaserGRBL.UserControls.ImageButton();
			this.BtnModulationInfo = new LaserGRBL.UserControls.ImageButton();
			this.BtnRepo = new LaserGRBL.UserControls.ImageButton();
			this.GbSpeed.SuspendLayout();
			this.TLP.SuspendLayout();
			this.SuspendLayout();
			// 
			// GbSpeed
			// 
			this.GbSpeed.AutoSize = true;
			this.GbSpeed.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GbSpeed.Controls.Add(this.BtnRepo);
			this.GbSpeed.Controls.Add(this.TLP);
			this.GbSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbSpeed.Location = new System.Drawing.Point(0, 0);
			this.GbSpeed.Name = "GbSpeed";
			this.GbSpeed.Size = new System.Drawing.Size(190, 179);
			this.GbSpeed.TabIndex = 3;
			this.GbSpeed.TabStop = false;
			this.GbSpeed.Text = "Laser";
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
			this.TLP.Controls.Add(this.label1, 1, 1);
			this.TLP.Controls.Add(this.LblWhatModulate, 0, 0);
			this.TLP.Controls.Add(this.CbTools, 1, 0);
			this.TLP.Controls.Add(this.LblModMin, 0, 1);
			this.TLP.Controls.Add(this.LblFP2, 0, 4);
			this.TLP.Controls.Add(this.LblFP1, 0, 3);
			this.TLP.Controls.Add(this.LblModMax, 0, 2);
			this.TLP.Controls.Add(this.IIModMin, 2, 1);
			this.TLP.Controls.Add(this.label2, 1, 2);
			this.TLP.Controls.Add(this.IIModMax, 2, 2);
			this.TLP.Controls.Add(this.LblFP1u, 3, 3);
			this.TLP.Controls.Add(this.IIFixedParam, 2, 3);
			this.TLP.Controls.Add(this.label3, 1, 3);
			this.TLP.Controls.Add(this.label5, 1, 4);
			this.TLP.Controls.Add(this.IILinearFilling, 2, 4);
			this.TLP.Controls.Add(this.LblLFP2u, 3, 4);
			this.TLP.Controls.Add(this.CBLaserON, 1, 5);
			this.TLP.Controls.Add(this.CBLaserOFF, 1, 6);
			this.TLP.Controls.Add(this.BtnOnOffInfo, 3, 5);
			this.TLP.Controls.Add(this.BtnModulationInfo, 3, 6);
			this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP.Location = new System.Drawing.Point(3, 16);
			this.TLP.Name = "TLP";
			this.TLP.RowCount = 7;
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.Size = new System.Drawing.Size(184, 160);
			this.TLP.TabIndex = 0;
			// 
			// label26
			// 
			this.label26.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label26.AutoSize = true;
			this.label26.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label26.Location = new System.Drawing.Point(3, 140);
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
			this.label18.Location = new System.Drawing.Point(3, 113);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(52, 13);
			this.label18.TabIndex = 19;
			this.label18.Text = "Laser ON";
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(62, 29);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(14, 13);
			this.label1.TabIndex = 26;
			this.label1.Text = "S";
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
			// LblModMin
			// 
			this.LblModMin.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblModMin.AutoSize = true;
			this.LblModMin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblModMin.Location = new System.Drawing.Point(3, 29);
			this.LblModMin.Name = "LblModMin";
			this.LblModMin.Size = new System.Drawing.Size(35, 13);
			this.LblModMin.TabIndex = 13;
			this.LblModMin.Text = "White";
			// 
			// LblFP2
			// 
			this.LblFP2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFP2.AutoSize = true;
			this.LblFP2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblFP2.Location = new System.Drawing.Point(3, 89);
			this.LblFP2.Name = "LblFP2";
			this.LblFP2.Size = new System.Drawing.Size(33, 13);
			this.LblFP2.TabIndex = 13;
			this.LblFP2.Text = "Filling";
			// 
			// LblFP1
			// 
			this.LblFP1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFP1.AutoSize = true;
			this.LblFP1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblFP1.Location = new System.Drawing.Point(3, 69);
			this.LblFP1.Name = "LblFP1";
			this.LblFP1.Size = new System.Drawing.Size(38, 13);
			this.LblFP1.TabIndex = 23;
			this.LblFP1.Text = "Border";
			// 
			// LblModMax
			// 
			this.LblModMax.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblModMax.AutoSize = true;
			this.LblModMax.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblModMax.Location = new System.Drawing.Point(3, 49);
			this.LblModMax.Name = "LblModMax";
			this.LblModMax.Size = new System.Drawing.Size(34, 13);
			this.LblModMax.TabIndex = 17;
			this.LblModMax.Text = "Black";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(62, 49);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(14, 13);
			this.label2.TabIndex = 27;
			this.label2.Text = "S";
			// 
			// LblFP1u
			// 
			this.LblFP1u.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFP1u.AutoSize = true;
			this.LblFP1u.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblFP1u.Location = new System.Drawing.Point(137, 69);
			this.LblFP1u.Name = "LblFP1u";
			this.LblFP1u.Size = new System.Drawing.Size(44, 13);
			this.LblFP1u.TabIndex = 22;
			this.LblFP1u.Text = "mm/min";
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(62, 69);
			this.label3.Margin = new System.Windows.Forms.Padding(0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(13, 13);
			this.label3.TabIndex = 28;
			this.label3.Text = "F";
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label5.AutoSize = true;
			this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label5.Location = new System.Drawing.Point(62, 89);
			this.label5.Margin = new System.Windows.Forms.Padding(0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(13, 13);
			this.label5.TabIndex = 29;
			this.label5.Text = "F";
			// 
			// LblLFP2u
			// 
			this.LblLFP2u.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblLFP2u.AutoSize = true;
			this.LblLFP2u.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblLFP2u.Location = new System.Drawing.Point(137, 89);
			this.LblLFP2u.Name = "LblLFP2u";
			this.LblLFP2u.Size = new System.Drawing.Size(44, 13);
			this.LblLFP2u.TabIndex = 15;
			this.LblLFP2u.Text = "mm/min";
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
			this.CBLaserON.Location = new System.Drawing.Point(65, 109);
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
			this.CBLaserOFF.Location = new System.Drawing.Point(65, 136);
			this.CBLaserOFF.Name = "CBLaserOFF";
			this.CBLaserOFF.Size = new System.Drawing.Size(66, 21);
			this.CBLaserOFF.TabIndex = 25;
			// 
			// CbTools
			// 
			this.TLP.SetColumnSpan(this.CbTools, 3);
			this.CbTools.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CbTools.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbTools.FormattingEnabled = true;
			this.CbTools.Location = new System.Drawing.Point(64, 2);
			this.CbTools.Margin = new System.Windows.Forms.Padding(2);
			this.CbTools.Name = "CbTools";
			this.CbTools.SelectedItem = null;
			this.CbTools.Size = new System.Drawing.Size(118, 21);
			this.CbTools.TabIndex = 24;
			// 
			// IIModMin
			// 
			this.IIModMin.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIModMin.ForcedText = null;
			this.IIModMin.ForceMinMax = false;
			this.IIModMin.Location = new System.Drawing.Point(79, 28);
			this.IIModMin.MaxValue = 999;
			this.IIModMin.MinValue = 0;
			this.IIModMin.Name = "IIModMin";
			this.IIModMin.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIModMin.Size = new System.Drawing.Size(52, 15);
			this.IIModMin.TabIndex = 11;
			// 
			// IIModMax
			// 
			this.IIModMax.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIModMax.CurrentValue = 255;
			this.IIModMax.ForcedText = null;
			this.IIModMax.ForceMinMax = false;
			this.IIModMax.Location = new System.Drawing.Point(79, 49);
			this.IIModMax.MaxValue = 1000;
			this.IIModMax.MinValue = 1;
			this.IIModMax.Name = "IIModMax";
			this.IIModMax.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIModMax.Size = new System.Drawing.Size(52, 15);
			this.IIModMax.TabIndex = 12;
			// 
			// IIFixedParam
			// 
			this.IIFixedParam.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIFixedParam.CurrentValue = 1000;
			this.IIFixedParam.ForcedText = null;
			this.IIFixedParam.ForceMinMax = false;
			this.IIFixedParam.Location = new System.Drawing.Point(79, 69);
			this.IIFixedParam.MaxValue = 4000;
			this.IIFixedParam.MinValue = 1;
			this.IIFixedParam.Name = "IIFixedParam";
			this.IIFixedParam.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIFixedParam.Size = new System.Drawing.Size(52, 15);
			this.IIFixedParam.TabIndex = 6;
			// 
			// IILinearFilling
			// 
			this.IILinearFilling.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IILinearFilling.CurrentValue = 1000;
			this.IILinearFilling.ForcedText = null;
			this.IILinearFilling.ForceMinMax = false;
			this.IILinearFilling.Location = new System.Drawing.Point(79, 89);
			this.IILinearFilling.MaxValue = 4000;
			this.IILinearFilling.MinValue = 1;
			this.IILinearFilling.Name = "IILinearFilling";
			this.IILinearFilling.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IILinearFilling.Size = new System.Drawing.Size(52, 15);
			this.IILinearFilling.TabIndex = 7;
			// 
			// BtnOnOffInfo
			// 
			this.BtnOnOffInfo.AltImage = null;
			this.BtnOnOffInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.BtnOnOffInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnOnOffInfo.Coloration = System.Drawing.Color.Empty;
			this.BtnOnOffInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnOnOffInfo.Image")));
			this.BtnOnOffInfo.Location = new System.Drawing.Point(137, 111);
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
			this.BtnModulationInfo.Location = new System.Drawing.Point(137, 138);
			this.BtnModulationInfo.Name = "BtnModulationInfo";
			this.BtnModulationInfo.Size = new System.Drawing.Size(17, 17);
			this.BtnModulationInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnModulationInfo.TabIndex = 23;
			this.BtnModulationInfo.UseAltImage = false;
			// 
			// BtnRepo
			// 
			this.BtnRepo.AltImage = null;
			this.BtnRepo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnRepo.BackColor = System.Drawing.Color.Transparent;
			this.BtnRepo.Coloration = System.Drawing.Color.Empty;
			this.BtnRepo.Image = ((System.Drawing.Image)(resources.GetObject("BtnRepo.Image")));
			this.BtnRepo.Location = new System.Drawing.Point(168, 0);
			this.BtnRepo.Name = "BtnRepo";
			this.BtnRepo.Size = new System.Drawing.Size(17, 17);
			this.BtnRepo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnRepo.TabIndex = 2;
			this.BtnRepo.UseAltImage = false;
			// 
			// SetupLaser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.GbSpeed);
			this.Name = "SetupLaser";
			this.Size = new System.Drawing.Size(190, 179);
			this.GbSpeed.ResumeLayout(false);
			this.GbSpeed.PerformLayout();
			this.TLP.ResumeLayout(false);
			this.TLP.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox GbSpeed;
		private System.Windows.Forms.TableLayoutPanel TLP;
		private System.Windows.Forms.Label LblFP1;
		private System.Windows.Forms.Label LblFP1u;
		private UserControls.IntegerInput.IntegerInputRanged IIFixedParam;
		private UserControls.IntegerInput.IntegerInputRanged IILinearFilling;
		private System.Windows.Forms.Label LblLFP2u;
		private System.Windows.Forms.Label LblFP2;
		private UserControls.ImageButton BtnModulationInfo;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label LblModMin;
		private UserControls.IntegerInput.IntegerInputRanged IIModMin;
		private System.Windows.Forms.Label LblModMax;
		private UserControls.IntegerInput.IntegerInputRanged IIModMax;
		private System.Windows.Forms.Label label18;
		private UserControls.ImageButton BtnOnOffInfo;
		private System.Windows.Forms.ComboBox CBLaserON;
		private System.Windows.Forms.ComboBox CBLaserOFF;
		private System.Windows.Forms.Label LblWhatModulate;
		private UserControls.EnumComboBox CbTools;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private UserControls.ImageButton BtnRepo;

	}
}
