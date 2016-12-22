namespace LaserGRBL.RasterConverter
{
	partial class RasterToLaserForm
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.label12 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.IISpeed = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IIOffsetX = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IIOffsetY = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IISizeH = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IISizeW = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.label6 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.UDQuality = new System.Windows.Forms.NumericUpDown();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label17 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.CbMode = new System.Windows.Forms.ComboBox();
			this.TBRed = new System.Windows.Forms.TrackBar();
			this.label14 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.TBGreen = new System.Windows.Forms.TrackBar();
			this.TbBright = new System.Windows.Forms.TrackBar();
			this.TBBlue = new System.Windows.Forms.TrackBar();
			this.TbContrast = new System.Windows.Forms.TrackBar();
			this.label3 = new System.Windows.Forms.Label();
			this.TbThreshold = new System.Windows.Forms.TrackBar();
			this.CbLinePreview = new System.Windows.Forms.CheckBox();
			this.CbThreshold = new System.Windows.Forms.CheckBox();
			this.Tp = new System.Windows.Forms.TabControl();
			this.TpPreview = new System.Windows.Forms.TabPage();
			this.PbConverted = new System.Windows.Forms.PictureBox();
			this.TpOriginal = new System.Windows.Forms.TabPage();
			this.PbOriginal = new System.Windows.Forms.PictureBox();
			this.BtnCreate = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UDQuality)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TBRed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TBGreen)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TbBright)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TBBlue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TbContrast)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TbThreshold)).BeginInit();
			this.Tp.SuspendLayout();
			this.TpPreview.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PbConverted)).BeginInit();
			this.TpOriginal.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PbOriginal)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.Tp, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.BtnCreate, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(905, 512);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Controls.Add(this.groupBox2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.tableLayoutPanel1.SetRowSpan(this.panel1, 2);
			this.panel1.Size = new System.Drawing.Size(220, 506);
			this.panel1.TabIndex = 2;
			// 
			// groupBox2
			// 
			this.groupBox2.AutoSize = true;
			this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox2.Controls.Add(this.tableLayoutPanel3);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(220, 108);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Gcode generation";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 5;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.label12, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.label9, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.label8, 3, 1);
			this.tableLayoutPanel3.Controls.Add(this.label5, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.label13, 3, 3);
			this.tableLayoutPanel3.Controls.Add(this.IISpeed, 2, 3);
			this.tableLayoutPanel3.Controls.Add(this.IIOffsetX, 2, 2);
			this.tableLayoutPanel3.Controls.Add(this.IIOffsetY, 4, 2);
			this.tableLayoutPanel3.Controls.Add(this.IISizeH, 4, 0);
			this.tableLayoutPanel3.Controls.Add(this.IISizeW, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.label6, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.label10, 1, 2);
			this.tableLayoutPanel3.Controls.Add(this.label7, 3, 0);
			this.tableLayoutPanel3.Controls.Add(this.label11, 3, 2);
			this.tableLayoutPanel3.Controls.Add(this.UDQuality, 2, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 4;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(214, 89);
			this.tableLayoutPanel3.TabIndex = 0;
			// 
			// label12
			// 
			this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(3, 72);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(38, 13);
			this.label12.TabIndex = 13;
			this.label12.Text = "Speed";
			// 
			// label9
			// 
			this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(3, 51);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(35, 13);
			this.label9.TabIndex = 9;
			this.label9.Text = "Offset";
			// 
			// label8
			// 
			this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label8.AutoSize = true;
			this.tableLayoutPanel3.SetColumnSpan(this.label8, 2);
			this.label8.Location = new System.Drawing.Point(126, 27);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(49, 13);
			this.label8.TabIndex = 8;
			this.label8.Text = "lines/mm";
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 27);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(39, 13);
			this.label5.TabIndex = 2;
			this.label5.Text = "Quality";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 4);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(27, 13);
			this.label4.TabIndex = 1;
			this.label4.Text = "Size";
			// 
			// label13
			// 
			this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label13.AutoSize = true;
			this.tableLayoutPanel3.SetColumnSpan(this.label13, 2);
			this.label13.Location = new System.Drawing.Point(126, 72);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(44, 13);
			this.label13.TabIndex = 15;
			this.label13.Text = "mm/min";
			// 
			// IISpeed
			// 
			this.IISpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IISpeed.CurrentValue = 1000;
			this.IISpeed.ForcedText = null;
			this.IISpeed.ForceMinMax = false;
			this.IISpeed.Location = new System.Drawing.Point(65, 71);
			this.IISpeed.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IISpeed.MaxValue = 4000;
			this.IISpeed.MinValue = 1;
			this.IISpeed.Name = "IISpeed";
			this.IISpeed.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IISpeed.Size = new System.Drawing.Size(55, 15);
			this.IISpeed.TabIndex = 16;
			// 
			// IIOffsetX
			// 
			this.IIOffsetX.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIOffsetX.ForcedText = null;
			this.IIOffsetX.ForceMinMax = false;
			this.IIOffsetX.Location = new System.Drawing.Point(65, 50);
			this.IIOffsetX.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IIOffsetX.MaxValue = 1000;
			this.IIOffsetX.MinValue = 0;
			this.IIOffsetX.Name = "IIOffsetX";
			this.IIOffsetX.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIOffsetX.Size = new System.Drawing.Size(55, 15);
			this.IIOffsetX.TabIndex = 17;
			// 
			// IIOffsetY
			// 
			this.IIOffsetY.ForcedText = null;
			this.IIOffsetY.ForceMinMax = false;
			this.IIOffsetY.Location = new System.Drawing.Point(143, 50);
			this.IIOffsetY.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IIOffsetY.MaxValue = 1000;
			this.IIOffsetY.MinValue = 0;
			this.IIOffsetY.Name = "IIOffsetY";
			this.IIOffsetY.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIOffsetY.Size = new System.Drawing.Size(55, 15);
			this.IIOffsetY.TabIndex = 18;
			// 
			// IISizeH
			// 
			this.IISizeH.CurrentValue = 100;
			this.IISizeH.ForcedText = null;
			this.IISizeH.ForceMinMax = false;
			this.IISizeH.Location = new System.Drawing.Point(143, 3);
			this.IISizeH.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IISizeH.MaxValue = 1000;
			this.IISizeH.MinValue = 5;
			this.IISizeH.Name = "IISizeH";
			this.IISizeH.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IISizeH.Size = new System.Drawing.Size(55, 15);
			this.IISizeH.TabIndex = 20;
			this.IISizeH.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IISizeH_CurrentValueChanged);
			// 
			// IISizeW
			// 
			this.IISizeW.CurrentValue = 100;
			this.IISizeW.ForcedText = null;
			this.IISizeW.ForceMinMax = false;
			this.IISizeW.Location = new System.Drawing.Point(65, 3);
			this.IISizeW.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IISizeW.MaxValue = 1000;
			this.IISizeW.MinValue = 5;
			this.IISizeW.Name = "IISizeW";
			this.IISizeW.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IISizeW.Size = new System.Drawing.Size(55, 15);
			this.IISizeW.TabIndex = 19;
			this.IISizeW.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IISizeW_CurrentValueChanged);
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(48, 4);
			this.label6.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(17, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "W";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label10
			// 
			this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(48, 51);
			this.label10.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(14, 13);
			this.label10.TabIndex = 11;
			this.label10.Text = "X";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(126, 4);
			this.label7.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(15, 13);
			this.label7.TabIndex = 6;
			this.label7.Text = "H";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label11
			// 
			this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(126, 51);
			this.label11.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(14, 13);
			this.label11.TabIndex = 12;
			this.label11.Text = "Y";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// UDQuality
			// 
			this.UDQuality.Location = new System.Drawing.Point(65, 24);
			this.UDQuality.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.UDQuality.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.UDQuality.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.UDQuality.Name = "UDQuality";
			this.UDQuality.Size = new System.Drawing.Size(55, 20);
			this.UDQuality.TabIndex = 7;
			this.UDQuality.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 108);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(220, 302);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Grayscale Conversion";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.label17, 0, 8);
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.CbMode, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.TBRed, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.label14, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.label16, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.label15, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.label2, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.TBGreen, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.TbBright, 1, 5);
			this.tableLayoutPanel2.Controls.Add(this.TBBlue, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.TbContrast, 1, 6);
			this.tableLayoutPanel2.Controls.Add(this.label3, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.TbThreshold, 1, 8);
			this.tableLayoutPanel2.Controls.Add(this.CbLinePreview, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.CbThreshold, 0, 7);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 9;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(214, 283);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// label17
			// 
			this.label17.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(3, 259);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(54, 13);
			this.label17.TabIndex = 13;
			this.label17.Text = "Threshold";
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Mode";
			// 
			// CbMode
			// 
			this.CbMode.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.CbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbMode.FormattingEnabled = true;
			this.CbMode.Location = new System.Drawing.Point(65, 3);
			this.CbMode.Name = "CbMode";
			this.CbMode.Size = new System.Drawing.Size(146, 21);
			this.CbMode.TabIndex = 2;
			this.CbMode.SelectedIndexChanged += new System.EventHandler(this.OnSelectorChange);
			// 
			// TBRed
			// 
			this.TBRed.AutoSize = false;
			this.TBRed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TBRed.Location = new System.Drawing.Point(65, 53);
			this.TBRed.Maximum = 160;
			this.TBRed.Minimum = 40;
			this.TBRed.Name = "TBRed";
			this.TBRed.Size = new System.Drawing.Size(146, 29);
			this.TBRed.TabIndex = 7;
			this.TBRed.TickFrequency = 10;
			this.TBRed.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.TBRed.Value = 100;
			this.TBRed.Scroll += new System.EventHandler(this.OnSelectorChange);
			// 
			// label14
			// 
			this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(3, 61);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(27, 13);
			this.label14.TabIndex = 6;
			this.label14.Text = "Red";
			// 
			// label16
			// 
			this.label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(3, 131);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(28, 13);
			this.label16.TabIndex = 11;
			this.label16.Text = "Blue";
			// 
			// label15
			// 
			this.label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(3, 96);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(36, 13);
			this.label15.TabIndex = 8;
			this.label15.Text = "Green";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 166);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Brightness";
			// 
			// TBGreen
			// 
			this.TBGreen.AutoSize = false;
			this.TBGreen.Location = new System.Drawing.Point(65, 88);
			this.TBGreen.Maximum = 160;
			this.TBGreen.Minimum = 40;
			this.TBGreen.Name = "TBGreen";
			this.TBGreen.Size = new System.Drawing.Size(146, 29);
			this.TBGreen.TabIndex = 9;
			this.TBGreen.TickFrequency = 10;
			this.TBGreen.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.TBGreen.Value = 100;
			this.TBGreen.Scroll += new System.EventHandler(this.OnSelectorChange);
			// 
			// TbBright
			// 
			this.TbBright.AutoSize = false;
			this.TbBright.Location = new System.Drawing.Point(65, 158);
			this.TbBright.Maximum = 160;
			this.TbBright.Minimum = 40;
			this.TbBright.Name = "TbBright";
			this.TbBright.Size = new System.Drawing.Size(146, 29);
			this.TbBright.TabIndex = 3;
			this.TbBright.TickFrequency = 10;
			this.TbBright.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.TbBright.Value = 100;
			this.TbBright.Scroll += new System.EventHandler(this.OnSelectorChange);
			// 
			// TBBlue
			// 
			this.TBBlue.AutoSize = false;
			this.TBBlue.Location = new System.Drawing.Point(65, 123);
			this.TBBlue.Maximum = 160;
			this.TBBlue.Minimum = 40;
			this.TBBlue.Name = "TBBlue";
			this.TBBlue.Size = new System.Drawing.Size(146, 29);
			this.TBBlue.TabIndex = 10;
			this.TBBlue.TickFrequency = 10;
			this.TBBlue.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.TBBlue.Value = 100;
			this.TBBlue.Scroll += new System.EventHandler(this.OnSelectorChange);
			// 
			// TbContrast
			// 
			this.TbContrast.AutoSize = false;
			this.TbContrast.Location = new System.Drawing.Point(65, 193);
			this.TbContrast.Maximum = 160;
			this.TbContrast.Minimum = 40;
			this.TbContrast.Name = "TbContrast";
			this.TbContrast.Size = new System.Drawing.Size(146, 29);
			this.TbContrast.TabIndex = 5;
			this.TbContrast.TickFrequency = 10;
			this.TbContrast.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.TbContrast.Value = 100;
			this.TbContrast.Scroll += new System.EventHandler(this.OnSelectorChange);
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 201);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(46, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Contrast";
			// 
			// TbThreshold
			// 
			this.TbThreshold.AutoSize = false;
			this.TbThreshold.Location = new System.Drawing.Point(65, 251);
			this.TbThreshold.Maximum = 100;
			this.TbThreshold.Name = "TbThreshold";
			this.TbThreshold.Size = new System.Drawing.Size(146, 29);
			this.TbThreshold.TabIndex = 14;
			this.TbThreshold.TickFrequency = 10;
			this.TbThreshold.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.TbThreshold.Value = 50;
			this.TbThreshold.Scroll += new System.EventHandler(this.OnSelectorChange);
			// 
			// CbLinePreview
			// 
			this.CbLinePreview.AutoSize = true;
			this.CbLinePreview.Checked = true;
			this.CbLinePreview.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tableLayoutPanel2.SetColumnSpan(this.CbLinePreview, 2);
			this.CbLinePreview.Location = new System.Drawing.Point(3, 30);
			this.CbLinePreview.Name = "CbLinePreview";
			this.CbLinePreview.Size = new System.Drawing.Size(86, 17);
			this.CbLinePreview.TabIndex = 12;
			this.CbLinePreview.Text = "Line preview";
			this.CbLinePreview.UseVisualStyleBackColor = true;
			this.CbLinePreview.CheckedChanged += new System.EventHandler(this.OnSelectorChange);
			// 
			// CbThreshold
			// 
			this.CbThreshold.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan(this.CbThreshold, 2);
			this.CbThreshold.Location = new System.Drawing.Point(3, 228);
			this.CbThreshold.Name = "CbThreshold";
			this.CbThreshold.Size = new System.Drawing.Size(94, 17);
			this.CbThreshold.TabIndex = 15;
			this.CbThreshold.Text = "BW Threshold";
			this.CbThreshold.UseVisualStyleBackColor = true;
			this.CbThreshold.CheckedChanged += new System.EventHandler(this.OnSelectorChange);
			// 
			// Tp
			// 
			this.Tp.Controls.Add(this.TpPreview);
			this.Tp.Controls.Add(this.TpOriginal);
			this.Tp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Tp.Location = new System.Drawing.Point(229, 3);
			this.Tp.Name = "Tp";
			this.Tp.SelectedIndex = 0;
			this.Tp.Size = new System.Drawing.Size(673, 459);
			this.Tp.TabIndex = 3;
			// 
			// TpPreview
			// 
			this.TpPreview.Controls.Add(this.PbConverted);
			this.TpPreview.Location = new System.Drawing.Point(4, 22);
			this.TpPreview.Name = "TpPreview";
			this.TpPreview.Padding = new System.Windows.Forms.Padding(3);
			this.TpPreview.Size = new System.Drawing.Size(665, 433);
			this.TpPreview.TabIndex = 0;
			this.TpPreview.Text = "Preview";
			this.TpPreview.UseVisualStyleBackColor = true;
			// 
			// PbConverted
			// 
			this.PbConverted.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PbConverted.Location = new System.Drawing.Point(3, 3);
			this.PbConverted.Name = "PbConverted";
			this.PbConverted.Size = new System.Drawing.Size(659, 427);
			this.PbConverted.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PbConverted.TabIndex = 0;
			this.PbConverted.TabStop = false;
			// 
			// TpOriginal
			// 
			this.TpOriginal.Controls.Add(this.PbOriginal);
			this.TpOriginal.Location = new System.Drawing.Point(4, 22);
			this.TpOriginal.Name = "TpOriginal";
			this.TpOriginal.Padding = new System.Windows.Forms.Padding(3);
			this.TpOriginal.Size = new System.Drawing.Size(665, 433);
			this.TpOriginal.TabIndex = 1;
			this.TpOriginal.Text = "Original";
			this.TpOriginal.UseVisualStyleBackColor = true;
			// 
			// PbOriginal
			// 
			this.PbOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PbOriginal.Location = new System.Drawing.Point(3, 3);
			this.PbOriginal.Name = "PbOriginal";
			this.PbOriginal.Size = new System.Drawing.Size(659, 427);
			this.PbOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PbOriginal.TabIndex = 0;
			this.PbOriginal.TabStop = false;
			// 
			// BtnCreate
			// 
			this.BtnCreate.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.BtnCreate.Location = new System.Drawing.Point(795, 468);
			this.BtnCreate.Name = "BtnCreate";
			this.BtnCreate.Size = new System.Drawing.Size(107, 41);
			this.BtnCreate.TabIndex = 4;
			this.BtnCreate.Text = "CREATE!";
			this.BtnCreate.UseVisualStyleBackColor = true;
			this.BtnCreate.Click += new System.EventHandler(this.BtnCreateClick);
			// 
			// RasterToLaserForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(905, 512);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "RasterToLaserForm";
			this.Text = "Import Raster Image";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.UDQuality)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.TBRed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TBGreen)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TbBright)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TBBlue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TbContrast)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TbThreshold)).EndInit();
			this.Tp.ResumeLayout(false);
			this.TpPreview.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PbConverted)).EndInit();
			this.TpOriginal.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PbOriginal)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl Tp;
		private System.Windows.Forms.TabPage TpPreview;
		private System.Windows.Forms.PictureBox PbConverted;
		private System.Windows.Forms.TabPage TpOriginal;
		private System.Windows.Forms.PictureBox PbOriginal;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox CbMode;
		private System.Windows.Forms.TrackBar TbBright;
		private System.Windows.Forms.TrackBar TbContrast;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown UDQuality;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button BtnCreate;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private UserControls.IntegerInput.IntegerInputRanged IISpeed;
		private UserControls.IntegerInput.IntegerInputRanged IIOffsetX;
		private UserControls.IntegerInput.IntegerInputRanged IIOffsetY;
		private UserControls.IntegerInput.IntegerInputRanged IISizeW;
		private UserControls.IntegerInput.IntegerInputRanged IISizeH;
		private System.Windows.Forms.TrackBar TBRed;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TrackBar TBGreen;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.TrackBar TBBlue;
		private System.Windows.Forms.CheckBox CbLinePreview;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.TrackBar TbThreshold;
		private System.Windows.Forms.CheckBox CbThreshold;
	}
}