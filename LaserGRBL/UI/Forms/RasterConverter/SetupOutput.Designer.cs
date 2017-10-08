namespace LaserGRBL.UI.Forms.RasterConverter
{
	partial class SetupOutput
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupOutput));
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this.GbSize = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.label9 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.IIOffsetX = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IIOffsetY = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IISizeH = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IISizeW = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.label6 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.GbSpeed = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.LblWhatModulate = new System.Windows.Forms.Label();
			this.CbTools = new LaserGRBL.UserControls.EnumComboBox();
			this.LblModMin = new System.Windows.Forms.Label();
			this.IIModMin = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.LblModMax = new System.Windows.Forms.Label();
			this.IIModMax = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.GbLaser = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.LblLFP2u = new System.Windows.Forms.Label();
			this.IILinearFilling = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.LblFP1u = new System.Windows.Forms.Label();
			this.LblFP1 = new System.Windows.Forms.Label();
			this.LblFP2 = new System.Windows.Forms.Label();
			this.IIFixedParam = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.label18 = new System.Windows.Forms.Label();
			this.CBLaserON = new System.Windows.Forms.ComboBox();
			this.label26 = new System.Windows.Forms.Label();
			this.CBLaserOFF = new System.Windows.Forms.ComboBox();
			this.BtnOnOffInfo = new LaserGRBL.UserControls.ImageButton();
			this.BtnModulationInfo = new LaserGRBL.UserControls.ImageButton();
			this.tableLayoutPanel9.SuspendLayout();
			this.GbSize.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.GbSpeed.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.GbLaser.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel9
			// 
			this.tableLayoutPanel9.AutoSize = true;
			this.tableLayoutPanel9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel9.ColumnCount = 1;
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel9.Controls.Add(this.GbSize, 0, 2);
			this.tableLayoutPanel9.Controls.Add(this.GbSpeed, 0, 0);
			this.tableLayoutPanel9.Controls.Add(this.GbLaser, 0, 1);
			this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			this.tableLayoutPanel9.RowCount = 3;
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.Size = new System.Drawing.Size(225, 257);
			this.tableLayoutPanel9.TabIndex = 2;
			// 
			// GbSize
			// 
			this.GbSize.AutoSize = true;
			this.GbSize.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GbSize.Controls.Add(this.tableLayoutPanel3);
			this.GbSize.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbSize.Location = new System.Drawing.Point(3, 193);
			this.GbSize.Name = "GbSize";
			this.GbSize.Size = new System.Drawing.Size(219, 61);
			this.GbSize.TabIndex = 1;
			this.GbSize.TabStop = false;
			this.GbSize.Text = "Image Size and Position [mm]";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 5;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.label9, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.IIOffsetX, 2, 1);
			this.tableLayoutPanel3.Controls.Add(this.IIOffsetY, 4, 1);
			this.tableLayoutPanel3.Controls.Add(this.IISizeH, 4, 0);
			this.tableLayoutPanel3.Controls.Add(this.IISizeW, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.label6, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.label10, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.label7, 3, 0);
			this.tableLayoutPanel3.Controls.Add(this.label11, 3, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(213, 42);
			this.tableLayoutPanel3.TabIndex = 0;
			// 
			// label9
			// 
			this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label9.AutoSize = true;
			this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label9.Location = new System.Drawing.Point(3, 25);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(35, 13);
			this.label9.TabIndex = 9;
			this.label9.Text = "Offset";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label4.Location = new System.Drawing.Point(3, 4);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(27, 13);
			this.label4.TabIndex = 1;
			this.label4.Text = "Size";
			// 
			// IIOffsetX
			// 
			this.IIOffsetX.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIOffsetX.ForcedText = null;
			this.IIOffsetX.ForceMinMax = false;
			this.IIOffsetX.Location = new System.Drawing.Point(56, 24);
			this.IIOffsetX.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IIOffsetX.MaxValue = 1000;
			this.IIOffsetX.MinValue = 0;
			this.IIOffsetX.Name = "IIOffsetX";
			this.IIOffsetX.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIOffsetX.Size = new System.Drawing.Size(55, 15);
			this.IIOffsetX.TabIndex = 3;
			// 
			// IIOffsetY
			// 
			this.IIOffsetY.ForcedText = null;
			this.IIOffsetY.ForceMinMax = false;
			this.IIOffsetY.Location = new System.Drawing.Point(129, 24);
			this.IIOffsetY.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IIOffsetY.MaxValue = 1000;
			this.IIOffsetY.MinValue = 0;
			this.IIOffsetY.Name = "IIOffsetY";
			this.IIOffsetY.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIOffsetY.Size = new System.Drawing.Size(55, 15);
			this.IIOffsetY.TabIndex = 4;
			// 
			// IISizeH
			// 
			this.IISizeH.ForcedText = null;
			this.IISizeH.ForceMinMax = false;
			this.IISizeH.Location = new System.Drawing.Point(129, 3);
			this.IISizeH.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IISizeH.MaxValue = 1000;
			this.IISizeH.MinValue = 10;
			this.IISizeH.Name = "IISizeH";
			this.IISizeH.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IISizeH.Size = new System.Drawing.Size(55, 15);
			this.IISizeH.TabIndex = 2;
			// 
			// IISizeW
			// 
			this.IISizeW.ForcedText = null;
			this.IISizeW.ForceMinMax = false;
			this.IISizeW.Location = new System.Drawing.Point(56, 3);
			this.IISizeW.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IISizeW.MaxValue = 1000;
			this.IISizeW.MinValue = 10;
			this.IISizeW.Name = "IISizeW";
			this.IISizeW.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IISizeW.Size = new System.Drawing.Size(55, 15);
			this.IISizeW.TabIndex = 1;
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label6.AutoSize = true;
			this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label6.Location = new System.Drawing.Point(44, 4);
			this.label6.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(12, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "W";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label10
			// 
			this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label10.AutoSize = true;
			this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label10.Location = new System.Drawing.Point(44, 25);
			this.label10.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(12, 13);
			this.label10.TabIndex = 11;
			this.label10.Text = "X";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label7.AutoSize = true;
			this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label7.Location = new System.Drawing.Point(117, 4);
			this.label7.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(12, 13);
			this.label7.TabIndex = 6;
			this.label7.Text = "H";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label11
			// 
			this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label11.AutoSize = true;
			this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label11.Location = new System.Drawing.Point(117, 25);
			this.label11.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(12, 13);
			this.label11.TabIndex = 12;
			this.label11.Text = "Y";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// GbSpeed
			// 
			this.GbSpeed.AutoSize = true;
			this.GbSpeed.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GbSpeed.Controls.Add(this.tableLayoutPanel6);
			this.GbSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbSpeed.Location = new System.Drawing.Point(3, 3);
			this.GbSpeed.Name = "GbSpeed";
			this.GbSpeed.Size = new System.Drawing.Size(219, 65);
			this.GbSpeed.TabIndex = 3;
			this.GbSpeed.TabStop = false;
			this.GbSpeed.Text = "Modulation";
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel6.ColumnCount = 4;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.Controls.Add(this.LblWhatModulate, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.CbTools, 1, 0);
			this.tableLayoutPanel6.Controls.Add(this.LblModMin, 0, 1);
			this.tableLayoutPanel6.Controls.Add(this.IIModMin, 1, 1);
			this.tableLayoutPanel6.Controls.Add(this.LblModMax, 2, 1);
			this.tableLayoutPanel6.Controls.Add(this.IIModMax, 3, 1);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 2;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size(213, 46);
			this.tableLayoutPanel6.TabIndex = 0;
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
			// CbTools
			// 
			this.tableLayoutPanel6.SetColumnSpan(this.CbTools, 3);
			this.CbTools.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CbTools.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbTools.FormattingEnabled = true;
			this.CbTools.Location = new System.Drawing.Point(59, 2);
			this.CbTools.Margin = new System.Windows.Forms.Padding(2);
			this.CbTools.Name = "CbTools";
			this.CbTools.SelectedItem = null;
			this.CbTools.Size = new System.Drawing.Size(152, 21);
			this.CbTools.TabIndex = 24;
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
			// IIModMin
			// 
			this.IIModMin.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIModMin.ForcedText = null;
			this.IIModMin.ForceMinMax = false;
			this.IIModMin.Location = new System.Drawing.Point(60, 28);
			this.IIModMin.MaxValue = 999;
			this.IIModMin.MinValue = 0;
			this.IIModMin.Name = "IIModMin";
			this.IIModMin.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIModMin.Size = new System.Drawing.Size(52, 15);
			this.IIModMin.TabIndex = 11;
			// 
			// LblModMax
			// 
			this.LblModMax.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblModMax.AutoSize = true;
			this.LblModMax.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblModMax.Location = new System.Drawing.Point(118, 29);
			this.LblModMax.Name = "LblModMax";
			this.LblModMax.Size = new System.Drawing.Size(34, 13);
			this.LblModMax.TabIndex = 17;
			this.LblModMax.Text = "Black";
			// 
			// IIModMax
			// 
			this.IIModMax.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIModMax.CurrentValue = 255;
			this.IIModMax.ForcedText = null;
			this.IIModMax.ForceMinMax = false;
			this.IIModMax.Location = new System.Drawing.Point(158, 28);
			this.IIModMax.MaxValue = 1000;
			this.IIModMax.MinValue = 1;
			this.IIModMax.Name = "IIModMax";
			this.IIModMax.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIModMax.Size = new System.Drawing.Size(52, 15);
			this.IIModMax.TabIndex = 12;
			// 
			// GbLaser
			// 
			this.GbLaser.AutoSize = true;
			this.GbLaser.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GbLaser.Controls.Add(this.tableLayoutPanel7);
			this.GbLaser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbLaser.Location = new System.Drawing.Point(3, 74);
			this.GbLaser.Name = "GbLaser";
			this.GbLaser.Size = new System.Drawing.Size(219, 113);
			this.GbLaser.TabIndex = 4;
			this.GbLaser.TabStop = false;
			this.GbLaser.Text = "Laser Options";
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.AutoSize = true;
			this.tableLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel7.ColumnCount = 3;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.Controls.Add(this.LblLFP2u, 2, 3);
			this.tableLayoutPanel7.Controls.Add(this.IILinearFilling, 1, 3);
			this.tableLayoutPanel7.Controls.Add(this.LblFP1u, 2, 2);
			this.tableLayoutPanel7.Controls.Add(this.LblFP1, 0, 2);
			this.tableLayoutPanel7.Controls.Add(this.LblFP2, 0, 3);
			this.tableLayoutPanel7.Controls.Add(this.IIFixedParam, 1, 2);
			this.tableLayoutPanel7.Controls.Add(this.label18, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.CBLaserON, 1, 0);
			this.tableLayoutPanel7.Controls.Add(this.label26, 0, 1);
			this.tableLayoutPanel7.Controls.Add(this.CBLaserOFF, 1, 1);
			this.tableLayoutPanel7.Controls.Add(this.BtnOnOffInfo, 2, 0);
			this.tableLayoutPanel7.Controls.Add(this.BtnModulationInfo, 2, 1);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 4;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel7.Size = new System.Drawing.Size(213, 94);
			this.tableLayoutPanel7.TabIndex = 0;
			// 
			// LblLFP2u
			// 
			this.LblLFP2u.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblLFP2u.AutoSize = true;
			this.LblLFP2u.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblLFP2u.Location = new System.Drawing.Point(139, 77);
			this.LblLFP2u.Name = "LblLFP2u";
			this.LblLFP2u.Size = new System.Drawing.Size(44, 13);
			this.LblLFP2u.TabIndex = 15;
			this.LblLFP2u.Text = "mm/min";
			// 
			// IILinearFilling
			// 
			this.IILinearFilling.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IILinearFilling.CurrentValue = 1000;
			this.IILinearFilling.ForcedText = null;
			this.IILinearFilling.ForceMinMax = false;
			this.IILinearFilling.Location = new System.Drawing.Point(78, 77);
			this.IILinearFilling.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IILinearFilling.MaxValue = 4000;
			this.IILinearFilling.MinValue = 1;
			this.IILinearFilling.Name = "IILinearFilling";
			this.IILinearFilling.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IILinearFilling.Size = new System.Drawing.Size(55, 15);
			this.IILinearFilling.TabIndex = 7;
			// 
			// LblFP1u
			// 
			this.LblFP1u.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFP1u.AutoSize = true;
			this.LblFP1u.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblFP1u.Location = new System.Drawing.Point(139, 57);
			this.LblFP1u.Name = "LblFP1u";
			this.LblFP1u.Size = new System.Drawing.Size(44, 13);
			this.LblFP1u.TabIndex = 22;
			this.LblFP1u.Text = "mm/min";
			// 
			// LblFP1
			// 
			this.LblFP1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFP1.AutoSize = true;
			this.LblFP1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblFP1.Location = new System.Drawing.Point(3, 57);
			this.LblFP1.Name = "LblFP1";
			this.LblFP1.Size = new System.Drawing.Size(72, 13);
			this.LblFP1.TabIndex = 23;
			this.LblFP1.Text = "Border Speed";
			// 
			// LblFP2
			// 
			this.LblFP2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFP2.AutoSize = true;
			this.LblFP2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblFP2.Location = new System.Drawing.Point(3, 77);
			this.LblFP2.Name = "LblFP2";
			this.LblFP2.Size = new System.Drawing.Size(67, 13);
			this.LblFP2.TabIndex = 13;
			this.LblFP2.Text = "Filling Speed";
			// 
			// IIFixedParam
			// 
			this.IIFixedParam.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIFixedParam.CurrentValue = 1000;
			this.IIFixedParam.ForcedText = null;
			this.IIFixedParam.ForceMinMax = false;
			this.IIFixedParam.Location = new System.Drawing.Point(78, 57);
			this.IIFixedParam.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IIFixedParam.MaxValue = 4000;
			this.IIFixedParam.MinValue = 1;
			this.IIFixedParam.Name = "IIFixedParam";
			this.IIFixedParam.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIFixedParam.Size = new System.Drawing.Size(55, 15);
			this.IIFixedParam.TabIndex = 6;
			// 
			// label18
			// 
			this.label18.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label18.AutoSize = true;
			this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label18.Location = new System.Drawing.Point(3, 7);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(52, 13);
			this.label18.TabIndex = 19;
			this.label18.Text = "Laser ON";
			// 
			// CBLaserON
			// 
			this.CBLaserON.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBLaserON.FormattingEnabled = true;
			this.CBLaserON.Items.AddRange(new object[] {
            "M3",
            "M4"});
			this.CBLaserON.Location = new System.Drawing.Point(81, 3);
			this.CBLaserON.Name = "CBLaserON";
			this.CBLaserON.Size = new System.Drawing.Size(52, 21);
			this.CBLaserON.TabIndex = 24;
			// 
			// label26
			// 
			this.label26.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label26.AutoSize = true;
			this.label26.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label26.Location = new System.Drawing.Point(3, 34);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(56, 13);
			this.label26.TabIndex = 21;
			this.label26.Text = "Laser OFF";
			// 
			// CBLaserOFF
			// 
			this.CBLaserOFF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBLaserOFF.FormattingEnabled = true;
			this.CBLaserOFF.Items.AddRange(new object[] {
            "M5"});
			this.CBLaserOFF.Location = new System.Drawing.Point(81, 30);
			this.CBLaserOFF.Name = "CBLaserOFF";
			this.CBLaserOFF.Size = new System.Drawing.Size(52, 21);
			this.CBLaserOFF.TabIndex = 25;
			// 
			// BtnOnOffInfo
			// 
			this.BtnOnOffInfo.AltImage = null;
			this.BtnOnOffInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.BtnOnOffInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnOnOffInfo.Coloration = System.Drawing.Color.Empty;
			this.BtnOnOffInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnOnOffInfo.Image")));
			this.BtnOnOffInfo.Location = new System.Drawing.Point(139, 5);
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
			this.BtnModulationInfo.Location = new System.Drawing.Point(139, 32);
			this.BtnModulationInfo.Name = "BtnModulationInfo";
			this.BtnModulationInfo.Size = new System.Drawing.Size(17, 17);
			this.BtnModulationInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnModulationInfo.TabIndex = 23;
			this.BtnModulationInfo.UseAltImage = false;
			// 
			// SetupOutput
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.tableLayoutPanel9);
			this.Name = "SetupOutput";
			this.Size = new System.Drawing.Size(225, 257);
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.GbSize.ResumeLayout(false);
			this.GbSize.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.GbSpeed.ResumeLayout(false);
			this.GbSpeed.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.GbLaser.ResumeLayout(false);
			this.GbLaser.PerformLayout();
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.GroupBox GbSize;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label4;
		private UserControls.IntegerInput.IntegerInputRanged IIOffsetX;
		private UserControls.IntegerInput.IntegerInputRanged IIOffsetY;
		private UserControls.IntegerInput.IntegerInputRanged IISizeH;
		private UserControls.IntegerInput.IntegerInputRanged IISizeW;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.GroupBox GbSpeed;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.Label LblFP1;
		private System.Windows.Forms.Label LblFP1u;
		private UserControls.IntegerInput.IntegerInputRanged IIFixedParam;
		private UserControls.IntegerInput.IntegerInputRanged IILinearFilling;
		private System.Windows.Forms.Label LblLFP2u;
		private System.Windows.Forms.Label LblFP2;
		private System.Windows.Forms.GroupBox GbLaser;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
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

	}
}
