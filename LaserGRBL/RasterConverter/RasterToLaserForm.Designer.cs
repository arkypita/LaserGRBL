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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RasterToLaserForm));
			this.RightGrid = new System.Windows.Forms.TableLayoutPanel();
			this.TCOriginalPreview = new System.Windows.Forms.TabControl();
			this.TpPreview = new System.Windows.Forms.TabPage();
			this.WB = new LaserGRBL.UserControls.WaitingProgressBar();
			this.PbConverted = new System.Windows.Forms.PictureBox();
			this.TpOriginal = new System.Windows.Forms.TabPage();
			this.PbOriginal = new System.Windows.Forms.PictureBox();
			this.BtnCreate = new System.Windows.Forms.Button();
			this.TCOptions = new System.Windows.Forms.TabControl();
			this.TPConversion = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.GbVectorizeOptions = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.UDFillingQuality = new System.Windows.Forms.NumericUpDown();
			this.CbFillingDirection = new System.Windows.Forms.ComboBox();
			this.label22 = new System.Windows.Forms.Label();
			this.UDSpotRemoval = new System.Windows.Forms.NumericUpDown();
			this.label23 = new System.Windows.Forms.Label();
			this.UDSmoothing = new System.Windows.Forms.NumericUpDown();
			this.label24 = new System.Windows.Forms.Label();
			this.UDOptimize = new System.Windows.Forms.NumericUpDown();
			this.CbSpotRemoval = new System.Windows.Forms.CheckBox();
			this.CbSmoothing = new System.Windows.Forms.CheckBox();
			this.CbOptimize = new System.Windows.Forms.CheckBox();
			this.CbShowImage = new System.Windows.Forms.CheckBox();
			this.CbShowDots = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.LblFillingQuality = new System.Windows.Forms.Label();
			this.LblFillingLineLbl = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.CbResize = new System.Windows.Forms.ComboBox();
			this.LblGrayscale = new System.Windows.Forms.Label();
			this.CbMode = new System.Windows.Forms.ComboBox();
			this.TBRed = new LaserGRBL.UserControls.ColorSlider();
			this.LblRed = new System.Windows.Forms.Label();
			this.LblBlue = new System.Windows.Forms.Label();
			this.LblGreen = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.TBGreen = new LaserGRBL.UserControls.ColorSlider();
			this.TbBright = new LaserGRBL.UserControls.ColorSlider();
			this.TBBlue = new LaserGRBL.UserControls.ColorSlider();
			this.TbContrast = new LaserGRBL.UserControls.ColorSlider();
			this.label3 = new System.Windows.Forms.Label();
			this.CbThreshold = new System.Windows.Forms.CheckBox();
			this.label28 = new System.Windows.Forms.Label();
			this.TbThreshold = new LaserGRBL.UserControls.ColorSlider();
			this.GbLineToLineOptions = new System.Windows.Forms.GroupBox();
			this.TLP = new System.Windows.Forms.TableLayoutPanel();
			this.CbDirections = new System.Windows.Forms.ComboBox();
			this.UDQuality = new System.Windows.Forms.NumericUpDown();
			this.CbLinePreview = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.RbVectorize = new System.Windows.Forms.RadioButton();
			this.RbLineToLineTracing = new System.Windows.Forms.RadioButton();
			this.TPGCode = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
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
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.label20 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.IIMarkSpeed = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IITravelSpeed = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.label13 = new System.Windows.Forms.Label();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.TxtLaserOff = new System.Windows.Forms.TextBox();
			this.label26 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.IIMinPower = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.label25 = new System.Windows.Forms.Label();
			this.IIMaxPower = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.label18 = new System.Windows.Forms.Label();
			this.TxtLaserOn = new System.Windows.Forms.TextBox();
			this.FlipControl = new System.Windows.Forms.TableLayoutPanel();
			this.BtFlipV = new LaserGRBL.UserControls.ImageButton();
			this.BtFlipH = new LaserGRBL.UserControls.ImageButton();
			this.BtRotateCW = new LaserGRBL.UserControls.ImageButton();
			this.BtRotateCCW = new LaserGRBL.UserControls.ImageButton();
			this.WT = new System.Windows.Forms.Timer(this.components);
			this.RightGrid.SuspendLayout();
			this.TCOriginalPreview.SuspendLayout();
			this.TpPreview.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PbConverted)).BeginInit();
			this.TpOriginal.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PbOriginal)).BeginInit();
			this.TCOptions.SuspendLayout();
			this.TPConversion.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			this.GbVectorizeOptions.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UDFillingQuality)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.UDSpotRemoval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.UDSmoothing)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.UDOptimize)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.GbLineToLineOptions.SuspendLayout();
			this.TLP.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UDQuality)).BeginInit();
			this.groupBox4.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.TPGCode.SuspendLayout();
			this.tableLayoutPanel9.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.FlipControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// RightGrid
			// 
			this.RightGrid.ColumnCount = 4;
			this.RightGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 236F));
			this.RightGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.RightGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.RightGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.RightGrid.Controls.Add(this.TCOriginalPreview, 1, 0);
			this.RightGrid.Controls.Add(this.BtnCreate, 3, 1);
			this.RightGrid.Controls.Add(this.TCOptions, 0, 0);
			this.RightGrid.Controls.Add(this.FlipControl, 1, 1);
			this.RightGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RightGrid.Location = new System.Drawing.Point(0, 0);
			this.RightGrid.Name = "RightGrid";
			this.RightGrid.RowCount = 2;
			this.RightGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.RightGrid.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RightGrid.Size = new System.Drawing.Size(912, 549);
			this.RightGrid.TabIndex = 0;
			// 
			// TCOriginalPreview
			// 
			this.RightGrid.SetColumnSpan(this.TCOriginalPreview, 3);
			this.TCOriginalPreview.Controls.Add(this.TpPreview);
			this.TCOriginalPreview.Controls.Add(this.TpOriginal);
			this.TCOriginalPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TCOriginalPreview.Location = new System.Drawing.Point(239, 3);
			this.TCOriginalPreview.Name = "TCOriginalPreview";
			this.TCOriginalPreview.SelectedIndex = 0;
			this.TCOriginalPreview.Size = new System.Drawing.Size(670, 506);
			this.TCOriginalPreview.TabIndex = 3;
			// 
			// TpPreview
			// 
			this.TpPreview.Controls.Add(this.WB);
			this.TpPreview.Controls.Add(this.PbConverted);
			this.TpPreview.Location = new System.Drawing.Point(4, 22);
			this.TpPreview.Name = "TpPreview";
			this.TpPreview.Padding = new System.Windows.Forms.Padding(3);
			this.TpPreview.Size = new System.Drawing.Size(662, 480);
			this.TpPreview.TabIndex = 0;
			this.TpPreview.Text = "Preview";
			this.TpPreview.UseVisualStyleBackColor = true;
			// 
			// WB
			// 
			this.WB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.WB.BarColor = System.Drawing.Color.SteelBlue;
			this.WB.BorderColor = System.Drawing.Color.Black;
			this.WB.BouncingMode = LaserGRBL.UserControls.WaitingProgressBar.BouncingModeEnum.PingPong;
			this.WB.DrawProgressString = false;
			this.WB.FillColor = System.Drawing.Color.White;
			this.WB.FillStyle = LaserGRBL.UserControls.FillStyles.Solid;
			this.WB.Interval = 25D;
			this.WB.Location = new System.Drawing.Point(591, 458);
			this.WB.Maximum = 20D;
			this.WB.Minimum = 0D;
			this.WB.Name = "WB";
			this.WB.ProgressStringDecimals = 0;
			this.WB.Reverse = true;
			this.WB.Running = false;
			this.WB.Size = new System.Drawing.Size(65, 16);
			this.WB.Step = 1D;
			this.WB.TabIndex = 1;
			this.WB.ThrowExceprion = false;
			this.WB.Value = 0D;
			// 
			// PbConverted
			// 
			this.PbConverted.BackColor = System.Drawing.Color.White;
			this.PbConverted.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PbConverted.Location = new System.Drawing.Point(3, 3);
			this.PbConverted.Name = "PbConverted";
			this.PbConverted.Size = new System.Drawing.Size(656, 474);
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
			this.TpOriginal.Size = new System.Drawing.Size(662, 522);
			this.TpOriginal.TabIndex = 1;
			this.TpOriginal.Text = "Original";
			this.TpOriginal.UseVisualStyleBackColor = true;
			// 
			// PbOriginal
			// 
			this.PbOriginal.BackColor = System.Drawing.Color.White;
			this.PbOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PbOriginal.Location = new System.Drawing.Point(3, 3);
			this.PbOriginal.Name = "PbOriginal";
			this.PbOriginal.Size = new System.Drawing.Size(656, 516);
			this.PbOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PbOriginal.TabIndex = 0;
			this.PbOriginal.TabStop = false;
			// 
			// BtnCreate
			// 
			this.BtnCreate.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.BtnCreate.Location = new System.Drawing.Point(835, 517);
			this.BtnCreate.Name = "BtnCreate";
			this.BtnCreate.Size = new System.Drawing.Size(74, 27);
			this.BtnCreate.TabIndex = 4;
			this.BtnCreate.Text = "CREATE!";
			this.BtnCreate.UseVisualStyleBackColor = true;
			this.BtnCreate.Click += new System.EventHandler(this.BtnCreateClick);
			// 
			// TCOptions
			// 
			this.TCOptions.Controls.Add(this.TPConversion);
			this.TCOptions.Controls.Add(this.TPGCode);
			this.TCOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TCOptions.Location = new System.Drawing.Point(3, 3);
			this.TCOptions.Name = "TCOptions";
			this.RightGrid.SetRowSpan(this.TCOptions, 2);
			this.TCOptions.SelectedIndex = 0;
			this.TCOptions.Size = new System.Drawing.Size(230, 543);
			this.TCOptions.TabIndex = 7;
			// 
			// TPConversion
			// 
			this.TPConversion.BackColor = System.Drawing.SystemColors.Control;
			this.TPConversion.Controls.Add(this.tableLayoutPanel8);
			this.TPConversion.Location = new System.Drawing.Point(4, 22);
			this.TPConversion.Name = "TPConversion";
			this.TPConversion.Padding = new System.Windows.Forms.Padding(3);
			this.TPConversion.Size = new System.Drawing.Size(222, 517);
			this.TPConversion.TabIndex = 0;
			this.TPConversion.Text = "Conversion";
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.ColumnCount = 1;
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel8.Controls.Add(this.GbVectorizeOptions, 0, 3);
			this.tableLayoutPanel8.Controls.Add(this.groupBox1, 0, 0);
			this.tableLayoutPanel8.Controls.Add(this.GbLineToLineOptions, 0, 2);
			this.tableLayoutPanel8.Controls.Add(this.groupBox4, 0, 1);
			this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 5;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel8.Size = new System.Drawing.Size(216, 511);
			this.tableLayoutPanel8.TabIndex = 3;
			// 
			// GbVectorizeOptions
			// 
			this.GbVectorizeOptions.AutoSize = true;
			this.GbVectorizeOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GbVectorizeOptions.Controls.Add(this.tableLayoutPanel5);
			this.GbVectorizeOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbVectorizeOptions.Location = new System.Drawing.Point(3, 402);
			this.GbVectorizeOptions.Name = "GbVectorizeOptions";
			this.GbVectorizeOptions.Size = new System.Drawing.Size(210, 161);
			this.GbVectorizeOptions.TabIndex = 4;
			this.GbVectorizeOptions.TabStop = false;
			this.GbVectorizeOptions.Text = "Vectorize! Options";
			this.GbVectorizeOptions.Visible = false;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel5.ColumnCount = 3;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel5.Controls.Add(this.UDFillingQuality, 1, 4);
			this.tableLayoutPanel5.Controls.Add(this.CbFillingDirection, 1, 3);
			this.tableLayoutPanel5.Controls.Add(this.label22, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.UDSpotRemoval, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.label23, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.UDSmoothing, 1, 1);
			this.tableLayoutPanel5.Controls.Add(this.label24, 0, 2);
			this.tableLayoutPanel5.Controls.Add(this.UDOptimize, 1, 2);
			this.tableLayoutPanel5.Controls.Add(this.CbSpotRemoval, 2, 0);
			this.tableLayoutPanel5.Controls.Add(this.CbSmoothing, 2, 1);
			this.tableLayoutPanel5.Controls.Add(this.CbOptimize, 2, 2);
			this.tableLayoutPanel5.Controls.Add(this.CbShowImage, 1, 5);
			this.tableLayoutPanel5.Controls.Add(this.CbShowDots, 2, 5);
			this.tableLayoutPanel5.Controls.Add(this.label1, 0, 5);
			this.tableLayoutPanel5.Controls.Add(this.label14, 0, 3);
			this.tableLayoutPanel5.Controls.Add(this.LblFillingQuality, 0, 4);
			this.tableLayoutPanel5.Controls.Add(this.LblFillingLineLbl, 2, 4);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 6;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(204, 142);
			this.tableLayoutPanel5.TabIndex = 0;
			// 
			// UDFillingQuality
			// 
			this.UDFillingQuality.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.UDFillingQuality.Location = new System.Drawing.Point(82, 99);
			this.UDFillingQuality.Margin = new System.Windows.Forms.Padding(2);
			this.UDFillingQuality.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.UDFillingQuality.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.UDFillingQuality.Name = "UDFillingQuality";
			this.UDFillingQuality.Size = new System.Drawing.Size(55, 20);
			this.UDFillingQuality.TabIndex = 32;
			this.UDFillingQuality.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.UDFillingQuality.ValueChanged += new System.EventHandler(this.UDFillingQuality_ValueChanged);
			// 
			// CbFillingDirection
			// 
			this.CbFillingDirection.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.tableLayoutPanel5.SetColumnSpan(this.CbFillingDirection, 2);
			this.CbFillingDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbFillingDirection.FormattingEnabled = true;
			this.CbFillingDirection.Location = new System.Drawing.Point(82, 74);
			this.CbFillingDirection.Margin = new System.Windows.Forms.Padding(2);
			this.CbFillingDirection.Name = "CbFillingDirection";
			this.CbFillingDirection.Size = new System.Drawing.Size(120, 21);
			this.CbFillingDirection.TabIndex = 31;
			this.CbFillingDirection.SelectedIndexChanged += new System.EventHandler(this.CbFillingDirection_SelectedIndexChanged);
			// 
			// label22
			// 
			this.label22.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label22.AutoSize = true;
			this.label22.Location = new System.Drawing.Point(3, 5);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(74, 13);
			this.label22.TabIndex = 22;
			this.label22.Text = "Spot Removal";
			// 
			// UDSpotRemoval
			// 
			this.UDSpotRemoval.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.UDSpotRemoval.Enabled = false;
			this.UDSpotRemoval.Location = new System.Drawing.Point(82, 2);
			this.UDSpotRemoval.Margin = new System.Windows.Forms.Padding(2);
			this.UDSpotRemoval.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.UDSpotRemoval.Name = "UDSpotRemoval";
			this.UDSpotRemoval.Size = new System.Drawing.Size(54, 20);
			this.UDSpotRemoval.TabIndex = 19;
			this.UDSpotRemoval.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.UDSpotRemoval.ValueChanged += new System.EventHandler(this.UDSpotRemoval_ValueChanged);
			// 
			// label23
			// 
			this.label23.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label23.AutoSize = true;
			this.label23.Location = new System.Drawing.Point(3, 29);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(57, 13);
			this.label23.TabIndex = 23;
			this.label23.Text = "Smoothing";
			// 
			// UDSmoothing
			// 
			this.UDSmoothing.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.UDSmoothing.DecimalPlaces = 1;
			this.UDSmoothing.Enabled = false;
			this.UDSmoothing.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.UDSmoothing.Location = new System.Drawing.Point(82, 26);
			this.UDSmoothing.Margin = new System.Windows.Forms.Padding(2);
			this.UDSmoothing.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.UDSmoothing.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
			this.UDSmoothing.Name = "UDSmoothing";
			this.UDSmoothing.Size = new System.Drawing.Size(54, 20);
			this.UDSmoothing.TabIndex = 20;
			this.UDSmoothing.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
			this.UDSmoothing.ValueChanged += new System.EventHandler(this.UDSmoothing_ValueChanged);
			// 
			// label24
			// 
			this.label24.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label24.AutoSize = true;
			this.label24.Location = new System.Drawing.Point(3, 53);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(47, 13);
			this.label24.TabIndex = 24;
			this.label24.Text = "Optimize";
			// 
			// UDOptimize
			// 
			this.UDOptimize.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.UDOptimize.DecimalPlaces = 1;
			this.UDOptimize.Enabled = false;
			this.UDOptimize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.UDOptimize.Location = new System.Drawing.Point(82, 50);
			this.UDOptimize.Margin = new System.Windows.Forms.Padding(2);
			this.UDOptimize.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.UDOptimize.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            65536});
			this.UDOptimize.Name = "UDOptimize";
			this.UDOptimize.Size = new System.Drawing.Size(54, 20);
			this.UDOptimize.TabIndex = 21;
			this.UDOptimize.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
			this.UDOptimize.ValueChanged += new System.EventHandler(this.UDOptimize_ValueChanged);
			// 
			// CbSpotRemoval
			// 
			this.CbSpotRemoval.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.CbSpotRemoval.AutoSize = true;
			this.CbSpotRemoval.Location = new System.Drawing.Point(141, 5);
			this.CbSpotRemoval.Margin = new System.Windows.Forms.Padding(2);
			this.CbSpotRemoval.Name = "CbSpotRemoval";
			this.CbSpotRemoval.Size = new System.Drawing.Size(15, 14);
			this.CbSpotRemoval.TabIndex = 25;
			this.CbSpotRemoval.UseVisualStyleBackColor = true;
			this.CbSpotRemoval.CheckedChanged += new System.EventHandler(this.CbSpotRemoval_CheckedChanged);
			// 
			// CbSmoothing
			// 
			this.CbSmoothing.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.CbSmoothing.AutoSize = true;
			this.CbSmoothing.Location = new System.Drawing.Point(141, 29);
			this.CbSmoothing.Margin = new System.Windows.Forms.Padding(2);
			this.CbSmoothing.Name = "CbSmoothing";
			this.CbSmoothing.Size = new System.Drawing.Size(15, 14);
			this.CbSmoothing.TabIndex = 26;
			this.CbSmoothing.UseVisualStyleBackColor = true;
			this.CbSmoothing.CheckedChanged += new System.EventHandler(this.CbSmoothing_CheckedChanged);
			// 
			// CbOptimize
			// 
			this.CbOptimize.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.CbOptimize.AutoSize = true;
			this.CbOptimize.Location = new System.Drawing.Point(141, 53);
			this.CbOptimize.Margin = new System.Windows.Forms.Padding(2);
			this.CbOptimize.Name = "CbOptimize";
			this.CbOptimize.Size = new System.Drawing.Size(15, 14);
			this.CbOptimize.TabIndex = 27;
			this.CbOptimize.UseVisualStyleBackColor = true;
			this.CbOptimize.CheckedChanged += new System.EventHandler(this.CbOptimize_CheckedChanged);
			// 
			// CbShowImage
			// 
			this.CbShowImage.AutoSize = true;
			this.CbShowImage.Checked = true;
			this.CbShowImage.CheckState = System.Windows.Forms.CheckState.Checked;
			this.CbShowImage.Location = new System.Drawing.Point(82, 123);
			this.CbShowImage.Margin = new System.Windows.Forms.Padding(2);
			this.CbShowImage.Name = "CbShowImage";
			this.CbShowImage.Size = new System.Drawing.Size(55, 17);
			this.CbShowImage.TabIndex = 29;
			this.CbShowImage.Text = "Image";
			this.CbShowImage.UseVisualStyleBackColor = true;
			this.CbShowImage.CheckedChanged += new System.EventHandler(this.CbShowImage_CheckedChanged);
			// 
			// CbShowDots
			// 
			this.CbShowDots.AutoSize = true;
			this.CbShowDots.Checked = true;
			this.CbShowDots.CheckState = System.Windows.Forms.CheckState.Checked;
			this.CbShowDots.Location = new System.Drawing.Point(141, 123);
			this.CbShowDots.Margin = new System.Windows.Forms.Padding(2);
			this.CbShowDots.Name = "CbShowDots";
			this.CbShowDots.Size = new System.Drawing.Size(48, 17);
			this.CbShowDots.TabIndex = 28;
			this.CbShowDots.Text = "Dots";
			this.CbShowDots.UseVisualStyleBackColor = true;
			this.CbShowDots.CheckedChanged += new System.EventHandler(this.CbShowDots_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 125);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 13);
			this.label1.TabIndex = 30;
			this.label1.Text = "Preview";
			// 
			// label14
			// 
			this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(3, 78);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(33, 13);
			this.label14.TabIndex = 33;
			this.label14.Text = "Filling";
			// 
			// LblFillingQuality
			// 
			this.LblFillingQuality.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFillingQuality.AutoSize = true;
			this.LblFillingQuality.Location = new System.Drawing.Point(3, 102);
			this.LblFillingQuality.Name = "LblFillingQuality";
			this.LblFillingQuality.Size = new System.Drawing.Size(39, 13);
			this.LblFillingQuality.TabIndex = 34;
			this.LblFillingQuality.Text = "Quality";
			// 
			// LblFillingLineLbl
			// 
			this.LblFillingLineLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFillingLineLbl.AutoSize = true;
			this.LblFillingLineLbl.Location = new System.Drawing.Point(142, 102);
			this.LblFillingLineLbl.Name = "LblFillingLineLbl";
			this.LblFillingLineLbl.Size = new System.Drawing.Size(53, 13);
			this.LblFillingLineLbl.TabIndex = 35;
			this.LblFillingLineLbl.Text = "Lines/mm";
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(210, 231);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Parameters";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.CbResize, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.LblGrayscale, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.CbMode, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.TBRed, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.LblRed, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.LblBlue, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.LblGreen, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.label2, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.TBGreen, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.TbBright, 1, 5);
			this.tableLayoutPanel2.Controls.Add(this.TBBlue, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.TbContrast, 1, 6);
			this.tableLayoutPanel2.Controls.Add(this.label3, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.CbThreshold, 0, 7);
			this.tableLayoutPanel2.Controls.Add(this.label28, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.TbThreshold, 1, 7);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 8;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(204, 212);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// CbResize
			// 
			this.CbResize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.CbResize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbResize.FormattingEnabled = true;
			this.CbResize.Location = new System.Drawing.Point(64, 2);
			this.CbResize.Margin = new System.Windows.Forms.Padding(2);
			this.CbResize.Name = "CbResize";
			this.CbResize.Size = new System.Drawing.Size(138, 21);
			this.CbResize.TabIndex = 17;
			this.CbResize.SelectedIndexChanged += new System.EventHandler(this.CbResizeSelectedIndexChanged);
			// 
			// LblGrayscale
			// 
			this.LblGrayscale.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblGrayscale.AutoSize = true;
			this.LblGrayscale.Location = new System.Drawing.Point(3, 31);
			this.LblGrayscale.Name = "LblGrayscale";
			this.LblGrayscale.Size = new System.Drawing.Size(54, 13);
			this.LblGrayscale.TabIndex = 0;
			this.LblGrayscale.Text = "Grayscale";
			// 
			// CbMode
			// 
			this.CbMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.CbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbMode.FormattingEnabled = true;
			this.CbMode.Location = new System.Drawing.Point(64, 27);
			this.CbMode.Margin = new System.Windows.Forms.Padding(2);
			this.CbMode.Name = "CbMode";
			this.CbMode.Size = new System.Drawing.Size(138, 21);
			this.CbMode.TabIndex = 2;
			this.CbMode.SelectedIndexChanged += new System.EventHandler(this.CbMode_SelectedIndexChanged);
			// 
			// TBRed
			// 
			this.TBRed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TBRed.BackColor = System.Drawing.Color.Transparent;
			this.TBRed.BarInnerColor = System.Drawing.Color.Firebrick;
			this.TBRed.BarOuterColor = System.Drawing.Color.DarkRed;
			this.TBRed.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TBRed.ElapsedInnerColor = System.Drawing.Color.Red;
			this.TBRed.ElapsedOuterColor = System.Drawing.Color.DarkRed;
			this.TBRed.LargeChange = ((uint)(5u));
			this.TBRed.Location = new System.Drawing.Point(63, 51);
			this.TBRed.Margin = new System.Windows.Forms.Padding(1);
			this.TBRed.Maximum = 160;
			this.TBRed.Minimum = 40;
			this.TBRed.Name = "TBRed";
			this.TBRed.Size = new System.Drawing.Size(140, 25);
			this.TBRed.SmallChange = ((uint)(1u));
			this.TBRed.TabIndex = 7;
			this.TBRed.ThumbRoundRectSize = new System.Drawing.Size(6, 6);
			this.TBRed.ThumbSize = 10;
			this.TBRed.Value = 100;
			this.TBRed.Visible = false;
			this.TBRed.ValueChanged += new System.EventHandler(this.TBRed_ValueChanged);
			this.TBRed.DoubleClick += new System.EventHandler(this.OnRGBCBDoubleClick);
			// 
			// LblRed
			// 
			this.LblRed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblRed.AutoSize = true;
			this.LblRed.Location = new System.Drawing.Point(3, 57);
			this.LblRed.Name = "LblRed";
			this.LblRed.Size = new System.Drawing.Size(27, 13);
			this.LblRed.TabIndex = 6;
			this.LblRed.Text = "Red";
			this.LblRed.Visible = false;
			// 
			// LblBlue
			// 
			this.LblBlue.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblBlue.AutoSize = true;
			this.LblBlue.Location = new System.Drawing.Point(3, 111);
			this.LblBlue.Name = "LblBlue";
			this.LblBlue.Size = new System.Drawing.Size(28, 13);
			this.LblBlue.TabIndex = 11;
			this.LblBlue.Text = "Blue";
			this.LblBlue.Visible = false;
			// 
			// LblGreen
			// 
			this.LblGreen.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblGreen.AutoSize = true;
			this.LblGreen.Location = new System.Drawing.Point(3, 84);
			this.LblGreen.Name = "LblGreen";
			this.LblGreen.Size = new System.Drawing.Size(36, 13);
			this.LblGreen.TabIndex = 8;
			this.LblGreen.Text = "Green";
			this.LblGreen.Visible = false;
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 138);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Brightness";
			// 
			// TBGreen
			// 
			this.TBGreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TBGreen.BackColor = System.Drawing.Color.Transparent;
			this.TBGreen.BarInnerColor = System.Drawing.Color.Green;
			this.TBGreen.BarOuterColor = System.Drawing.Color.DarkGreen;
			this.TBGreen.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TBGreen.LargeChange = ((uint)(5u));
			this.TBGreen.Location = new System.Drawing.Point(63, 78);
			this.TBGreen.Margin = new System.Windows.Forms.Padding(1);
			this.TBGreen.Maximum = 160;
			this.TBGreen.Minimum = 40;
			this.TBGreen.Name = "TBGreen";
			this.TBGreen.Size = new System.Drawing.Size(140, 25);
			this.TBGreen.SmallChange = ((uint)(1u));
			this.TBGreen.TabIndex = 9;
			this.TBGreen.ThumbRoundRectSize = new System.Drawing.Size(6, 6);
			this.TBGreen.ThumbSize = 10;
			this.TBGreen.Value = 100;
			this.TBGreen.Visible = false;
			this.TBGreen.ValueChanged += new System.EventHandler(this.TBGreen_ValueChanged);
			this.TBGreen.DoubleClick += new System.EventHandler(this.OnRGBCBDoubleClick);
			// 
			// TbBright
			// 
			this.TbBright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TbBright.BackColor = System.Drawing.Color.Transparent;
			this.TbBright.BarInnerColor = System.Drawing.Color.DimGray;
			this.TbBright.BarOuterColor = System.Drawing.Color.Black;
			this.TbBright.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TbBright.ElapsedInnerColor = System.Drawing.Color.White;
			this.TbBright.ElapsedOuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.TbBright.LargeChange = ((uint)(5u));
			this.TbBright.Location = new System.Drawing.Point(63, 132);
			this.TbBright.Margin = new System.Windows.Forms.Padding(1);
			this.TbBright.Maximum = 160;
			this.TbBright.Minimum = 40;
			this.TbBright.Name = "TbBright";
			this.TbBright.Size = new System.Drawing.Size(140, 25);
			this.TbBright.SmallChange = ((uint)(1u));
			this.TbBright.TabIndex = 3;
			this.TbBright.ThumbRoundRectSize = new System.Drawing.Size(6, 6);
			this.TbBright.ThumbSize = 10;
			this.TbBright.Value = 100;
			this.TbBright.ValueChanged += new System.EventHandler(this.TbBright_ValueChanged);
			this.TbBright.DoubleClick += new System.EventHandler(this.OnRGBCBDoubleClick);
			// 
			// TBBlue
			// 
			this.TBBlue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TBBlue.BackColor = System.Drawing.Color.Transparent;
			this.TBBlue.BarInnerColor = System.Drawing.Color.MediumBlue;
			this.TBBlue.BarOuterColor = System.Drawing.Color.DarkBlue;
			this.TBBlue.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TBBlue.ElapsedInnerColor = System.Drawing.Color.DodgerBlue;
			this.TBBlue.ElapsedOuterColor = System.Drawing.Color.SteelBlue;
			this.TBBlue.LargeChange = ((uint)(5u));
			this.TBBlue.Location = new System.Drawing.Point(63, 105);
			this.TBBlue.Margin = new System.Windows.Forms.Padding(1);
			this.TBBlue.Maximum = 160;
			this.TBBlue.Minimum = 40;
			this.TBBlue.Name = "TBBlue";
			this.TBBlue.Size = new System.Drawing.Size(140, 25);
			this.TBBlue.SmallChange = ((uint)(1u));
			this.TBBlue.TabIndex = 10;
			this.TBBlue.ThumbRoundRectSize = new System.Drawing.Size(6, 6);
			this.TBBlue.ThumbSize = 10;
			this.TBBlue.Value = 100;
			this.TBBlue.Visible = false;
			this.TBBlue.ValueChanged += new System.EventHandler(this.TBBlue_ValueChanged);
			this.TBBlue.DoubleClick += new System.EventHandler(this.OnRGBCBDoubleClick);
			// 
			// TbContrast
			// 
			this.TbContrast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TbContrast.BackColor = System.Drawing.Color.Transparent;
			this.TbContrast.BarInnerColor = System.Drawing.Color.DimGray;
			this.TbContrast.BarOuterColor = System.Drawing.Color.Black;
			this.TbContrast.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TbContrast.ElapsedInnerColor = System.Drawing.Color.White;
			this.TbContrast.ElapsedOuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.TbContrast.LargeChange = ((uint)(5u));
			this.TbContrast.Location = new System.Drawing.Point(63, 159);
			this.TbContrast.Margin = new System.Windows.Forms.Padding(1);
			this.TbContrast.Maximum = 160;
			this.TbContrast.Minimum = 40;
			this.TbContrast.Name = "TbContrast";
			this.TbContrast.Size = new System.Drawing.Size(140, 25);
			this.TbContrast.SmallChange = ((uint)(1u));
			this.TbContrast.TabIndex = 5;
			this.TbContrast.ThumbRoundRectSize = new System.Drawing.Size(6, 6);
			this.TbContrast.ThumbSize = 10;
			this.TbContrast.Value = 100;
			this.TbContrast.ValueChanged += new System.EventHandler(this.TbContrast_ValueChanged);
			this.TbContrast.DoubleClick += new System.EventHandler(this.OnRGBCBDoubleClick);
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 165);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(46, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Contrast";
			// 
			// CbThreshold
			// 
			this.CbThreshold.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.CbThreshold.AutoSize = true;
			this.CbThreshold.Location = new System.Drawing.Point(2, 190);
			this.CbThreshold.Margin = new System.Windows.Forms.Padding(2);
			this.CbThreshold.Name = "CbThreshold";
			this.CbThreshold.Size = new System.Drawing.Size(45, 17);
			this.CbThreshold.TabIndex = 15;
			this.CbThreshold.Text = "Thr.";
			this.CbThreshold.UseVisualStyleBackColor = true;
			this.CbThreshold.CheckedChanged += new System.EventHandler(this.CbThreshold_CheckedChanged);
			// 
			// label28
			// 
			this.label28.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label28.AutoSize = true;
			this.label28.Location = new System.Drawing.Point(3, 6);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(39, 13);
			this.label28.TabIndex = 16;
			this.label28.Text = "Resize";
			// 
			// TbThreshold
			// 
			this.TbThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TbThreshold.BackColor = System.Drawing.Color.Transparent;
			this.TbThreshold.BarInnerColor = System.Drawing.Color.DimGray;
			this.TbThreshold.BarOuterColor = System.Drawing.Color.Black;
			this.TbThreshold.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TbThreshold.ElapsedInnerColor = System.Drawing.Color.White;
			this.TbThreshold.ElapsedOuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.TbThreshold.LargeChange = ((uint)(5u));
			this.TbThreshold.Location = new System.Drawing.Point(63, 186);
			this.TbThreshold.Margin = new System.Windows.Forms.Padding(1);
			this.TbThreshold.Name = "TbThreshold";
			this.TbThreshold.Size = new System.Drawing.Size(140, 25);
			this.TbThreshold.SmallChange = ((uint)(1u));
			this.TbThreshold.TabIndex = 14;
			this.TbThreshold.ThumbRoundRectSize = new System.Drawing.Size(6, 6);
			this.TbThreshold.ThumbSize = 10;
			this.TbThreshold.Visible = false;
			this.TbThreshold.ValueChanged += new System.EventHandler(this.TbThreshold_ValueChanged);
			this.TbThreshold.DoubleClick += new System.EventHandler(this.OnThresholdDoubleClick);
			// 
			// GbLineToLineOptions
			// 
			this.GbLineToLineOptions.AutoSize = true;
			this.GbLineToLineOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GbLineToLineOptions.Controls.Add(this.TLP);
			this.GbLineToLineOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbLineToLineOptions.Location = new System.Drawing.Point(3, 307);
			this.GbLineToLineOptions.Name = "GbLineToLineOptions";
			this.GbLineToLineOptions.Size = new System.Drawing.Size(210, 89);
			this.GbLineToLineOptions.TabIndex = 2;
			this.GbLineToLineOptions.TabStop = false;
			this.GbLineToLineOptions.Text = "Line To Line Options";
			// 
			// TLP
			// 
			this.TLP.AutoSize = true;
			this.TLP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP.ColumnCount = 3;
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP.Controls.Add(this.CbDirections, 1, 0);
			this.TLP.Controls.Add(this.UDQuality, 1, 1);
			this.TLP.Controls.Add(this.CbLinePreview, 0, 2);
			this.TLP.Controls.Add(this.label5, 0, 1);
			this.TLP.Controls.Add(this.label27, 0, 0);
			this.TLP.Controls.Add(this.label8, 2, 1);
			this.TLP.Dock = System.Windows.Forms.DockStyle.Top;
			this.TLP.Location = new System.Drawing.Point(3, 16);
			this.TLP.Name = "TLP";
			this.TLP.RowCount = 2;
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.TLP.Size = new System.Drawing.Size(204, 70);
			this.TLP.TabIndex = 0;
			// 
			// CbDirections
			// 
			this.CbDirections.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TLP.SetColumnSpan(this.CbDirections, 2);
			this.CbDirections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbDirections.FormattingEnabled = true;
			this.CbDirections.Location = new System.Drawing.Point(57, 2);
			this.CbDirections.Margin = new System.Windows.Forms.Padding(2);
			this.CbDirections.Name = "CbDirections";
			this.CbDirections.Size = new System.Drawing.Size(145, 21);
			this.CbDirections.TabIndex = 14;
			this.CbDirections.SelectedIndexChanged += new System.EventHandler(this.CbDirectionsSelectedIndexChanged);
			// 
			// UDQuality
			// 
			this.UDQuality.Location = new System.Drawing.Point(57, 27);
			this.UDQuality.Margin = new System.Windows.Forms.Padding(2);
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
			this.UDQuality.ValueChanged += new System.EventHandler(this.UDQuality_ValueChanged);
			// 
			// CbLinePreview
			// 
			this.CbLinePreview.AutoSize = true;
			this.CbLinePreview.Checked = true;
			this.CbLinePreview.CheckState = System.Windows.Forms.CheckState.Checked;
			this.TLP.SetColumnSpan(this.CbLinePreview, 3);
			this.CbLinePreview.Location = new System.Drawing.Point(2, 51);
			this.CbLinePreview.Margin = new System.Windows.Forms.Padding(2);
			this.CbLinePreview.Name = "CbLinePreview";
			this.CbLinePreview.Size = new System.Drawing.Size(87, 17);
			this.CbLinePreview.TabIndex = 12;
			this.CbLinePreview.Text = "Line Preview";
			this.CbLinePreview.UseVisualStyleBackColor = true;
			this.CbLinePreview.CheckedChanged += new System.EventHandler(this.CbLinePreview_CheckedChanged);
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 30);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(39, 13);
			this.label5.TabIndex = 2;
			this.label5.Text = "Quality";
			// 
			// label27
			// 
			this.label27.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label27.AutoSize = true;
			this.label27.Location = new System.Drawing.Point(3, 6);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(49, 13);
			this.label27.TabIndex = 13;
			this.label27.Text = "Direction";
			// 
			// label8
			// 
			this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(117, 30);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(53, 13);
			this.label8.TabIndex = 15;
			this.label8.Text = "Lines/mm";
			// 
			// groupBox4
			// 
			this.groupBox4.AutoSize = true;
			this.groupBox4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox4.Controls.Add(this.tableLayoutPanel4);
			this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox4.Location = new System.Drawing.Point(3, 240);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(210, 61);
			this.groupBox4.TabIndex = 3;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Conversion Tool";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel4.ColumnCount = 3;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.Controls.Add(this.RbVectorize, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.RbLineToLineTracing, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(204, 42);
			this.tableLayoutPanel4.TabIndex = 1;
			// 
			// RbVectorize
			// 
			this.RbVectorize.AutoSize = true;
			this.tableLayoutPanel4.SetColumnSpan(this.RbVectorize, 3);
			this.RbVectorize.Location = new System.Drawing.Point(2, 23);
			this.RbVectorize.Margin = new System.Windows.Forms.Padding(2);
			this.RbVectorize.Name = "RbVectorize";
			this.RbVectorize.Size = new System.Drawing.Size(164, 17);
			this.RbVectorize.TabIndex = 1;
			this.RbVectorize.Text = "Vectorize! [EXPERIMENTAL]";
			this.RbVectorize.UseVisualStyleBackColor = true;
			this.RbVectorize.CheckedChanged += new System.EventHandler(this.RbVectorize_CheckedChanged);
			// 
			// RbLineToLineTracing
			// 
			this.RbLineToLineTracing.AutoSize = true;
			this.RbLineToLineTracing.Checked = true;
			this.tableLayoutPanel4.SetColumnSpan(this.RbLineToLineTracing, 3);
			this.RbLineToLineTracing.Location = new System.Drawing.Point(2, 2);
			this.RbLineToLineTracing.Margin = new System.Windows.Forms.Padding(2);
			this.RbLineToLineTracing.Name = "RbLineToLineTracing";
			this.RbLineToLineTracing.Size = new System.Drawing.Size(123, 17);
			this.RbLineToLineTracing.TabIndex = 0;
			this.RbLineToLineTracing.TabStop = true;
			this.RbLineToLineTracing.Text = "Line To Line Tracing";
			this.RbLineToLineTracing.UseVisualStyleBackColor = true;
			this.RbLineToLineTracing.CheckedChanged += new System.EventHandler(this.RbLineToLineTracing_CheckedChanged);
			// 
			// TPGCode
			// 
			this.TPGCode.BackColor = System.Drawing.SystemColors.Control;
			this.TPGCode.Controls.Add(this.tableLayoutPanel9);
			this.TPGCode.Location = new System.Drawing.Point(4, 22);
			this.TPGCode.Name = "TPGCode";
			this.TPGCode.Padding = new System.Windows.Forms.Padding(3);
			this.TPGCode.Size = new System.Drawing.Size(222, 559);
			this.TPGCode.TabIndex = 1;
			this.TPGCode.Text = "GCode";
			// 
			// tableLayoutPanel9
			// 
			this.tableLayoutPanel9.ColumnCount = 1;
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel9.Controls.Add(this.groupBox2, 0, 2);
			this.tableLayoutPanel9.Controls.Add(this.groupBox3, 0, 0);
			this.tableLayoutPanel9.Controls.Add(this.groupBox5, 0, 1);
			this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			this.tableLayoutPanel9.RowCount = 4;
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel9.Size = new System.Drawing.Size(216, 553);
			this.tableLayoutPanel9.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.AutoSize = true;
			this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox2.Controls.Add(this.tableLayoutPanel3);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(3, 142);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(212, 61);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Image Size and Position [mm]";
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
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(206, 42);
			this.tableLayoutPanel3.TabIndex = 0;
			// 
			// label9
			// 
			this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label9.AutoSize = true;
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
			this.IIOffsetX.Location = new System.Drawing.Point(61, 24);
			this.IIOffsetX.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IIOffsetX.MaxValue = 1000;
			this.IIOffsetX.MinValue = 0;
			this.IIOffsetX.Name = "IIOffsetX";
			this.IIOffsetX.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIOffsetX.Size = new System.Drawing.Size(55, 15);
			this.IIOffsetX.TabIndex = 17;
			this.IIOffsetX.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIOffsetXYCurrentValueChanged);
			// 
			// IIOffsetY
			// 
			this.IIOffsetY.ForcedText = null;
			this.IIOffsetY.ForceMinMax = false;
			this.IIOffsetY.Location = new System.Drawing.Point(139, 24);
			this.IIOffsetY.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IIOffsetY.MaxValue = 1000;
			this.IIOffsetY.MinValue = 0;
			this.IIOffsetY.Name = "IIOffsetY";
			this.IIOffsetY.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIOffsetY.Size = new System.Drawing.Size(55, 15);
			this.IIOffsetY.TabIndex = 18;
			this.IIOffsetY.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIOffsetXYCurrentValueChanged);
			// 
			// IISizeH
			// 
			this.IISizeH.CurrentValue = 100;
			this.IISizeH.ForcedText = null;
			this.IISizeH.ForceMinMax = false;
			this.IISizeH.Location = new System.Drawing.Point(139, 3);
			this.IISizeH.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IISizeH.MaxValue = 1000;
			this.IISizeH.MinValue = 10;
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
			this.IISizeW.Location = new System.Drawing.Point(61, 3);
			this.IISizeW.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IISizeW.MaxValue = 1000;
			this.IISizeW.MinValue = 10;
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
			this.label6.Location = new System.Drawing.Point(44, 4);
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
			this.label10.Location = new System.Drawing.Point(44, 25);
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
			this.label7.Location = new System.Drawing.Point(122, 4);
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
			this.label11.Location = new System.Drawing.Point(122, 25);
			this.label11.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(14, 13);
			this.label11.TabIndex = 12;
			this.label11.Text = "Y";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox3
			// 
			this.groupBox3.AutoSize = true;
			this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox3.Controls.Add(this.tableLayoutPanel6);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox3.Location = new System.Drawing.Point(3, 3);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(212, 61);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Speed";
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel6.ColumnCount = 3;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.Controls.Add(this.label20, 2, 1);
			this.tableLayoutPanel6.Controls.Add(this.label12, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.label19, 0, 1);
			this.tableLayoutPanel6.Controls.Add(this.IIMarkSpeed, 1, 0);
			this.tableLayoutPanel6.Controls.Add(this.IITravelSpeed, 1, 1);
			this.tableLayoutPanel6.Controls.Add(this.label13, 2, 0);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 2;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size(206, 42);
			this.tableLayoutPanel6.TabIndex = 0;
			// 
			// label20
			// 
			this.label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(118, 25);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(44, 13);
			this.label20.TabIndex = 19;
			this.label20.Text = "mm/min";
			// 
			// label12
			// 
			this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(3, 4);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(45, 13);
			this.label12.TabIndex = 13;
			this.label12.Text = "Mark @";
			// 
			// label19
			// 
			this.label19.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(3, 25);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(51, 13);
			this.label19.TabIndex = 17;
			this.label19.Text = "Travel @";
			// 
			// IIMarkSpeed
			// 
			this.IIMarkSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIMarkSpeed.CurrentValue = 1000;
			this.IIMarkSpeed.ForcedText = null;
			this.IIMarkSpeed.ForceMinMax = false;
			this.IIMarkSpeed.Location = new System.Drawing.Point(57, 3);
			this.IIMarkSpeed.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IIMarkSpeed.MaxValue = 4000;
			this.IIMarkSpeed.MinValue = 1;
			this.IIMarkSpeed.Name = "IIMarkSpeed";
			this.IIMarkSpeed.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIMarkSpeed.Size = new System.Drawing.Size(55, 15);
			this.IIMarkSpeed.TabIndex = 16;
			this.IIMarkSpeed.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIMarkSpeedCurrentValueChanged);
			// 
			// IITravelSpeed
			// 
			this.IITravelSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IITravelSpeed.CurrentValue = 4000;
			this.IITravelSpeed.ForcedText = null;
			this.IITravelSpeed.ForceMinMax = false;
			this.IITravelSpeed.Location = new System.Drawing.Point(57, 24);
			this.IITravelSpeed.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IITravelSpeed.MaxValue = 4000;
			this.IITravelSpeed.MinValue = 1;
			this.IITravelSpeed.Name = "IITravelSpeed";
			this.IITravelSpeed.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IITravelSpeed.Size = new System.Drawing.Size(55, 15);
			this.IITravelSpeed.TabIndex = 18;
			this.IITravelSpeed.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IITravelSpeedCurrentValueChanged);
			// 
			// label13
			// 
			this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(118, 4);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(44, 13);
			this.label13.TabIndex = 15;
			this.label13.Text = "mm/min";
			// 
			// groupBox5
			// 
			this.groupBox5.AutoSize = true;
			this.groupBox5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox5.Controls.Add(this.tableLayoutPanel7);
			this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox5.Location = new System.Drawing.Point(3, 70);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(212, 66);
			this.groupBox5.TabIndex = 6;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Laser Options";
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.AutoSize = true;
			this.tableLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel7.ColumnCount = 4;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.Controls.Add(this.TxtLaserOff, 3, 0);
			this.tableLayoutPanel7.Controls.Add(this.label26, 2, 0);
			this.tableLayoutPanel7.Controls.Add(this.label21, 0, 1);
			this.tableLayoutPanel7.Controls.Add(this.IIMinPower, 1, 1);
			this.tableLayoutPanel7.Controls.Add(this.label25, 2, 1);
			this.tableLayoutPanel7.Controls.Add(this.IIMaxPower, 3, 1);
			this.tableLayoutPanel7.Controls.Add(this.label18, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.TxtLaserOn, 1, 0);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 2;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel7.Size = new System.Drawing.Size(206, 47);
			this.tableLayoutPanel7.TabIndex = 0;
			// 
			// TxtLaserOff
			// 
			this.TxtLaserOff.Location = new System.Drawing.Point(166, 3);
			this.TxtLaserOff.MaxLength = 3;
			this.TxtLaserOff.Name = "TxtLaserOff";
			this.TxtLaserOff.Size = new System.Drawing.Size(35, 20);
			this.TxtLaserOff.TabIndex = 22;
			this.TxtLaserOff.Text = "M5";
			this.TxtLaserOff.TextChanged += new System.EventHandler(this.TxtLaserOffTextChanged);
			// 
			// label26
			// 
			this.label26.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label26.AutoSize = true;
			this.label26.Location = new System.Drawing.Point(104, 6);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(56, 13);
			this.label26.TabIndex = 21;
			this.label26.Text = "Laser OFF";
			// 
			// label21
			// 
			this.label21.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(3, 30);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(27, 13);
			this.label21.TabIndex = 13;
			this.label21.Text = "MIN";
			// 
			// IIMinPower
			// 
			this.IIMinPower.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIMinPower.ForcedText = null;
			this.IIMinPower.ForceMinMax = false;
			this.IIMinPower.Location = new System.Drawing.Point(61, 29);
			this.IIMinPower.MaxValue = 1000;
			this.IIMinPower.MinValue = 0;
			this.IIMinPower.Name = "IIMinPower";
			this.IIMinPower.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIMinPower.Size = new System.Drawing.Size(37, 15);
			this.IIMinPower.TabIndex = 16;
			this.IIMinPower.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIMinPowerCurrentValueChanged);
			// 
			// label25
			// 
			this.label25.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(104, 30);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(30, 13);
			this.label25.TabIndex = 17;
			this.label25.Text = "MAX";
			// 
			// IIMaxPower
			// 
			this.IIMaxPower.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIMaxPower.CurrentValue = 255;
			this.IIMaxPower.ForcedText = null;
			this.IIMaxPower.ForceMinMax = false;
			this.IIMaxPower.Location = new System.Drawing.Point(166, 29);
			this.IIMaxPower.MaxValue = 1000;
			this.IIMaxPower.MinValue = 1;
			this.IIMaxPower.Name = "IIMaxPower";
			this.IIMaxPower.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIMaxPower.Size = new System.Drawing.Size(37, 15);
			this.IIMaxPower.TabIndex = 18;
			this.IIMaxPower.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIMaxPowerCurrentValueChanged);
			// 
			// label18
			// 
			this.label18.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(3, 6);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(52, 13);
			this.label18.TabIndex = 19;
			this.label18.Text = "Laser ON";
			// 
			// TxtLaserOn
			// 
			this.TxtLaserOn.Location = new System.Drawing.Point(61, 3);
			this.TxtLaserOn.MaxLength = 3;
			this.TxtLaserOn.Name = "TxtLaserOn";
			this.TxtLaserOn.Size = new System.Drawing.Size(37, 20);
			this.TxtLaserOn.TabIndex = 20;
			this.TxtLaserOn.Text = "M3";
			this.TxtLaserOn.TextChanged += new System.EventHandler(this.TxtLaserOnTextChanged);
			// 
			// FlipControl
			// 
			this.FlipControl.AutoSize = true;
			this.FlipControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.FlipControl.ColumnCount = 4;
			this.FlipControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.FlipControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.FlipControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.FlipControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.FlipControl.Controls.Add(this.BtFlipV, 3, 0);
			this.FlipControl.Controls.Add(this.BtFlipH, 2, 0);
			this.FlipControl.Controls.Add(this.BtRotateCW, 0, 0);
			this.FlipControl.Controls.Add(this.BtRotateCCW, 1, 0);
			this.FlipControl.Location = new System.Drawing.Point(239, 515);
			this.FlipControl.Name = "FlipControl";
			this.FlipControl.RowCount = 1;
			this.FlipControl.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.FlipControl.Size = new System.Drawing.Size(124, 31);
			this.FlipControl.TabIndex = 8;
			// 
			// BtFlipV
			// 
			this.BtFlipV.AltImage = null;
			this.BtFlipV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtFlipV.Coloration = System.Drawing.Color.Empty;
			this.BtFlipV.Image = ((System.Drawing.Image)(resources.GetObject("BtFlipV.Image")));
			this.BtFlipV.Location = new System.Drawing.Point(96, 3);
			this.BtFlipV.Name = "BtFlipV";
			this.BtFlipV.Size = new System.Drawing.Size(25, 25);
			this.BtFlipV.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtFlipV.TabIndex = 3;
			this.BtFlipV.UseAltImage = false;
			this.BtFlipV.Click += new System.EventHandler(this.BtFlipVClick);
			// 
			// BtFlipH
			// 
			this.BtFlipH.AltImage = null;
			this.BtFlipH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtFlipH.Coloration = System.Drawing.Color.Empty;
			this.BtFlipH.Image = ((System.Drawing.Image)(resources.GetObject("BtFlipH.Image")));
			this.BtFlipH.Location = new System.Drawing.Point(65, 3);
			this.BtFlipH.Name = "BtFlipH";
			this.BtFlipH.Size = new System.Drawing.Size(25, 25);
			this.BtFlipH.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtFlipH.TabIndex = 2;
			this.BtFlipH.UseAltImage = false;
			this.BtFlipH.Click += new System.EventHandler(this.BtFlipHClick);
			// 
			// BtRotateCW
			// 
			this.BtRotateCW.AltImage = null;
			this.BtRotateCW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtRotateCW.Coloration = System.Drawing.Color.Empty;
			this.BtRotateCW.Image = ((System.Drawing.Image)(resources.GetObject("BtRotateCW.Image")));
			this.BtRotateCW.Location = new System.Drawing.Point(3, 3);
			this.BtRotateCW.Name = "BtRotateCW";
			this.BtRotateCW.Size = new System.Drawing.Size(25, 25);
			this.BtRotateCW.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtRotateCW.TabIndex = 1;
			this.BtRotateCW.UseAltImage = false;
			this.BtRotateCW.Click += new System.EventHandler(this.BtRotateCWClick);
			// 
			// BtRotateCCW
			// 
			this.BtRotateCCW.AltImage = null;
			this.BtRotateCCW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtRotateCCW.Coloration = System.Drawing.Color.Empty;
			this.BtRotateCCW.Image = ((System.Drawing.Image)(resources.GetObject("BtRotateCCW.Image")));
			this.BtRotateCCW.Location = new System.Drawing.Point(34, 3);
			this.BtRotateCCW.Name = "BtRotateCCW";
			this.BtRotateCCW.Size = new System.Drawing.Size(25, 25);
			this.BtRotateCCW.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtRotateCCW.TabIndex = 0;
			this.BtRotateCCW.UseAltImage = false;
			this.BtRotateCCW.Click += new System.EventHandler(this.BtRotateCCWClick);
			// 
			// WT
			// 
			this.WT.Interval = 50;
			this.WT.Tick += new System.EventHandler(this.WTTick);
			// 
			// RasterToLaserForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(912, 549);
			this.Controls.Add(this.RightGrid);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RasterToLaserForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Import Raster Image";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RasterToLaserFormFormClosing);
			this.Load += new System.EventHandler(this.RasterToLaserForm_Load);
			this.RightGrid.ResumeLayout(false);
			this.RightGrid.PerformLayout();
			this.TCOriginalPreview.ResumeLayout(false);
			this.TpPreview.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PbConverted)).EndInit();
			this.TpOriginal.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PbOriginal)).EndInit();
			this.TCOptions.ResumeLayout(false);
			this.TPConversion.ResumeLayout(false);
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel8.PerformLayout();
			this.GbVectorizeOptions.ResumeLayout(false);
			this.GbVectorizeOptions.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.UDFillingQuality)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.UDSpotRemoval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.UDSmoothing)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.UDOptimize)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.GbLineToLineOptions.ResumeLayout(false);
			this.GbLineToLineOptions.PerformLayout();
			this.TLP.ResumeLayout(false);
			this.TLP.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.UDQuality)).EndInit();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.TPGCode.ResumeLayout(false);
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			this.FlipControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel RightGrid;
		private System.Windows.Forms.TabControl TCOriginalPreview;
		private System.Windows.Forms.TabPage TpPreview;
		private System.Windows.Forms.PictureBox PbConverted;
		private System.Windows.Forms.TabPage TpOriginal;
		private System.Windows.Forms.PictureBox PbOriginal;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label LblGrayscale;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox CbMode;
		private UserControls.ColorSlider TbBright;
		private UserControls.ColorSlider TbContrast;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown UDQuality;
		private System.Windows.Forms.Button BtnCreate;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private UserControls.IntegerInput.IntegerInputRanged IIMarkSpeed;
		private UserControls.IntegerInput.IntegerInputRanged IISizeW;
		private UserControls.IntegerInput.IntegerInputRanged IISizeH;
		private UserControls.ColorSlider TBRed;
		private System.Windows.Forms.Label LblRed;
		private UserControls.ColorSlider TBGreen;
		private System.Windows.Forms.Label LblGreen;
		private System.Windows.Forms.Label LblBlue;
		private UserControls.ColorSlider TBBlue;
		private System.Windows.Forms.CheckBox CbLinePreview;
		private UserControls.ColorSlider TbThreshold;
		private System.Windows.Forms.CheckBox CbThreshold;
		private System.Windows.Forms.GroupBox GbLineToLineOptions;
		private System.Windows.Forms.TableLayoutPanel TLP;
		private System.Windows.Forms.Label label9;
		private UserControls.IntegerInput.IntegerInputRanged IIOffsetX;
		private UserControls.IntegerInput.IntegerInputRanged IIOffsetY;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.RadioButton RbVectorize;
		private System.Windows.Forms.RadioButton RbLineToLineTracing;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox GbVectorizeOptions;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IITravelSpeed;
		private System.Windows.Forms.NumericUpDown UDSpotRemoval;
		private System.Windows.Forms.NumericUpDown UDSmoothing;
		private System.Windows.Forms.NumericUpDown UDOptimize;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.CheckBox CbSpotRemoval;
		private System.Windows.Forms.CheckBox CbSmoothing;
		private System.Windows.Forms.CheckBox CbOptimize;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.CheckBox CbShowDots;
		private System.Windows.Forms.CheckBox CbShowImage;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label25;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IIMinPower;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IIMaxPower;
		private System.Windows.Forms.TabControl TCOptions;
		private System.Windows.Forms.TabPage TPConversion;
		private System.Windows.Forms.TabPage TPGCode;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.TextBox TxtLaserOn;
		private System.Windows.Forms.TextBox TxtLaserOff;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.ComboBox CbDirections;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox CbResize;
		private System.Windows.Forms.Label label28;
		private LaserGRBL.UserControls.WaitingProgressBar WB;
		private System.Windows.Forms.Timer WT;
		private System.Windows.Forms.TableLayoutPanel FlipControl;
		private LaserGRBL.UserControls.ImageButton BtFlipV;
		private LaserGRBL.UserControls.ImageButton BtFlipH;
		private LaserGRBL.UserControls.ImageButton BtRotateCW;
		private LaserGRBL.UserControls.ImageButton BtRotateCCW;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown UDFillingQuality;
		private System.Windows.Forms.ComboBox CbFillingDirection;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label LblFillingQuality;
		private System.Windows.Forms.Label LblFillingLineLbl;
	}
}