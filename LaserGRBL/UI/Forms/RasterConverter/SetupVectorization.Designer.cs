namespace LaserGRBL.UI.Forms.RasterConverter
{
	partial class SetupVectorization
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupVectorization));
			this.TLP = new System.Windows.Forms.TableLayoutPanel();
			this.label22 = new System.Windows.Forms.Label();
			this.UDSpotRemoval = new System.Windows.Forms.NumericUpDown();
			this.CbSpotRemoval = new System.Windows.Forms.CheckBox();
			this.label24 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.UDOptimize = new System.Windows.Forms.NumericUpDown();
			this.UDSmoothing = new System.Windows.Forms.NumericUpDown();
			this.CbOptimize = new System.Windows.Forms.CheckBox();
			this.CbSmoothing = new System.Windows.Forms.CheckBox();
			this.label14 = new System.Windows.Forms.Label();
			this.CbFillingDirection = new LaserGRBL.UserControls.EnumComboBox();
			this.LblFillingQuality = new System.Windows.Forms.Label();
			this.UDFillingQuality = new System.Windows.Forms.NumericUpDown();
			this.LblFillingLineLbl = new System.Windows.Forms.Label();
			this.UDDownSample = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.CbDownSample = new System.Windows.Forms.CheckBox();
			this.BtnFillingQualityInfo = new LaserGRBL.UserControls.ImageButton();
			this.TLP.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UDSpotRemoval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.UDOptimize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.UDSmoothing)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.UDFillingQuality)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.UDDownSample)).BeginInit();
			this.SuspendLayout();
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
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			this.TLP.Controls.Add(this.label22, 0, 0);
			this.TLP.Controls.Add(this.UDSpotRemoval, 1, 0);
			this.TLP.Controls.Add(this.CbSpotRemoval, 2, 0);
			this.TLP.Controls.Add(this.label24, 0, 2);
			this.TLP.Controls.Add(this.label23, 0, 1);
			this.TLP.Controls.Add(this.UDOptimize, 1, 2);
			this.TLP.Controls.Add(this.UDSmoothing, 1, 1);
			this.TLP.Controls.Add(this.CbOptimize, 2, 2);
			this.TLP.Controls.Add(this.CbSmoothing, 2, 1);
			this.TLP.Controls.Add(this.label14, 0, 4);
			this.TLP.Controls.Add(this.CbFillingDirection, 1, 4);
			this.TLP.Controls.Add(this.LblFillingQuality, 0, 5);
			this.TLP.Controls.Add(this.UDFillingQuality, 1, 5);
			this.TLP.Controls.Add(this.LblFillingLineLbl, 2, 5);
			this.TLP.Controls.Add(this.UDDownSample, 1, 3);
			this.TLP.Controls.Add(this.label1, 0, 3);
			this.TLP.Controls.Add(this.CbDownSample, 2, 3);
			this.TLP.Controls.Add(this.BtnFillingQualityInfo, 3, 5);
			this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP.Location = new System.Drawing.Point(0, 0);
			this.TLP.Name = "TLP";
			this.TLP.RowCount = 6;
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
			this.TLP.Size = new System.Drawing.Size(223, 145);
			this.TLP.TabIndex = 1;
			// 
			// label22
			// 
			this.label22.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label22.AutoSize = true;
			this.label22.ImeMode = System.Windows.Forms.ImeMode.NoControl;
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
			this.UDSpotRemoval.Location = new System.Drawing.Point(84, 2);
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
			// CbSpotRemoval
			// 
			this.CbSpotRemoval.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.CbSpotRemoval.AutoSize = true;
			this.CbSpotRemoval.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.CbSpotRemoval.Location = new System.Drawing.Point(143, 5);
			this.CbSpotRemoval.Margin = new System.Windows.Forms.Padding(2);
			this.CbSpotRemoval.Name = "CbSpotRemoval";
			this.CbSpotRemoval.Size = new System.Drawing.Size(15, 14);
			this.CbSpotRemoval.TabIndex = 25;
			this.CbSpotRemoval.UseVisualStyleBackColor = true;
			this.CbSpotRemoval.CheckedChanged += new System.EventHandler(this.CbSpotRemoval_CheckedChanged);
			// 
			// label24
			// 
			this.label24.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label24.AutoSize = true;
			this.label24.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label24.Location = new System.Drawing.Point(3, 53);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(47, 13);
			this.label24.TabIndex = 24;
			this.label24.Text = "Optimize";
			// 
			// label23
			// 
			this.label23.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label23.AutoSize = true;
			this.label23.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label23.Location = new System.Drawing.Point(3, 29);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(57, 13);
			this.label23.TabIndex = 23;
			this.label23.Text = "Smoothing";
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
			this.UDOptimize.Location = new System.Drawing.Point(84, 50);
			this.UDOptimize.Margin = new System.Windows.Forms.Padding(2);
			this.UDOptimize.Maximum = new decimal(new int[] {
            50,
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
			this.UDSmoothing.Location = new System.Drawing.Point(84, 26);
			this.UDSmoothing.Margin = new System.Windows.Forms.Padding(2);
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
			// CbOptimize
			// 
			this.CbOptimize.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.CbOptimize.AutoSize = true;
			this.CbOptimize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.CbOptimize.Location = new System.Drawing.Point(143, 53);
			this.CbOptimize.Margin = new System.Windows.Forms.Padding(2);
			this.CbOptimize.Name = "CbOptimize";
			this.CbOptimize.Size = new System.Drawing.Size(15, 14);
			this.CbOptimize.TabIndex = 27;
			this.CbOptimize.UseVisualStyleBackColor = true;
			this.CbOptimize.CheckedChanged += new System.EventHandler(this.CbOptimize_CheckedChanged);
			// 
			// CbSmoothing
			// 
			this.CbSmoothing.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.CbSmoothing.AutoSize = true;
			this.CbSmoothing.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.CbSmoothing.Location = new System.Drawing.Point(143, 29);
			this.CbSmoothing.Margin = new System.Windows.Forms.Padding(2);
			this.CbSmoothing.Name = "CbSmoothing";
			this.CbSmoothing.Size = new System.Drawing.Size(15, 14);
			this.CbSmoothing.TabIndex = 26;
			this.CbSmoothing.UseVisualStyleBackColor = true;
			this.CbSmoothing.CheckedChanged += new System.EventHandler(this.CbSmoothing_CheckedChanged);
			// 
			// label14
			// 
			this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label14.AutoSize = true;
			this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label14.Location = new System.Drawing.Point(3, 102);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(33, 13);
			this.label14.TabIndex = 33;
			this.label14.Text = "Filling";
			// 
			// CbFillingDirection
			// 
			this.CbFillingDirection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TLP.SetColumnSpan(this.CbFillingDirection, 3);
			this.CbFillingDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbFillingDirection.FormattingEnabled = true;
			this.CbFillingDirection.Location = new System.Drawing.Point(84, 98);
			this.CbFillingDirection.Margin = new System.Windows.Forms.Padding(2);
			this.CbFillingDirection.Name = "CbFillingDirection";
			this.CbFillingDirection.SelectedItem = null;
			this.CbFillingDirection.Size = new System.Drawing.Size(137, 21);
			this.CbFillingDirection.TabIndex = 31;
			this.CbFillingDirection.SelectedIndexChanged += new System.EventHandler(this.CbFillingDirection_SelectedIndexChanged);
			// 
			// LblFillingQuality
			// 
			this.LblFillingQuality.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFillingQuality.AutoSize = true;
			this.LblFillingQuality.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblFillingQuality.Location = new System.Drawing.Point(3, 126);
			this.LblFillingQuality.Name = "LblFillingQuality";
			this.LblFillingQuality.Size = new System.Drawing.Size(68, 13);
			this.LblFillingQuality.TabIndex = 34;
			this.LblFillingQuality.Text = "Filling Quality";
			// 
			// UDFillingQuality
			// 
			this.UDFillingQuality.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.UDFillingQuality.DecimalPlaces = 3;
			this.UDFillingQuality.Location = new System.Drawing.Point(84, 123);
			this.UDFillingQuality.Margin = new System.Windows.Forms.Padding(2);
			this.UDFillingQuality.Maximum = new decimal(new int[] {
            20,
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
			// LblFillingLineLbl
			// 
			this.LblFillingLineLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblFillingLineLbl.AutoSize = true;
			this.LblFillingLineLbl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblFillingLineLbl.Location = new System.Drawing.Point(144, 126);
			this.LblFillingLineLbl.Name = "LblFillingLineLbl";
			this.LblFillingLineLbl.Size = new System.Drawing.Size(49, 13);
			this.LblFillingLineLbl.TabIndex = 35;
			this.LblFillingLineLbl.Text = "lines/mm";
			// 
			// UDDownSample
			// 
			this.UDDownSample.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.UDDownSample.DecimalPlaces = 1;
			this.UDDownSample.Enabled = false;
			this.UDDownSample.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.UDDownSample.Location = new System.Drawing.Point(84, 74);
			this.UDDownSample.Margin = new System.Windows.Forms.Padding(2);
			this.UDDownSample.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.UDDownSample.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.UDDownSample.Name = "UDDownSample";
			this.UDDownSample.Size = new System.Drawing.Size(54, 20);
			this.UDDownSample.TabIndex = 37;
			this.UDDownSample.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.UDDownSample.ValueChanged += new System.EventHandler(this.UDDownSample_ValueChanged);
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(3, 77);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 13);
			this.label1.TabIndex = 36;
			this.label1.Text = "Downsampling";
			// 
			// CbDownSample
			// 
			this.CbDownSample.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.CbDownSample.AutoSize = true;
			this.CbDownSample.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.CbDownSample.Location = new System.Drawing.Point(143, 77);
			this.CbDownSample.Margin = new System.Windows.Forms.Padding(2);
			this.CbDownSample.Name = "CbDownSample";
			this.CbDownSample.Size = new System.Drawing.Size(15, 14);
			this.CbDownSample.TabIndex = 38;
			this.CbDownSample.UseVisualStyleBackColor = true;
			this.CbDownSample.CheckedChanged += new System.EventHandler(this.CbDownSample_CheckedChanged);
			// 
			// BtnFillingQualityInfo
			// 
			this.BtnFillingQualityInfo.AltImage = null;
			this.BtnFillingQualityInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.BtnFillingQualityInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnFillingQualityInfo.Coloration = System.Drawing.Color.Empty;
			this.BtnFillingQualityInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnFillingQualityInfo.Image")));
			this.BtnFillingQualityInfo.Location = new System.Drawing.Point(199, 124);
			this.BtnFillingQualityInfo.Name = "BtnFillingQualityInfo";
			this.BtnFillingQualityInfo.Size = new System.Drawing.Size(17, 17);
			this.BtnFillingQualityInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnFillingQualityInfo.TabIndex = 39;
			this.BtnFillingQualityInfo.UseAltImage = false;
			// 
			// SetupVectorization
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.TLP);
			this.Name = "SetupVectorization";
			this.Size = new System.Drawing.Size(223, 145);
			this.TLP.ResumeLayout(false);
			this.TLP.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.UDSpotRemoval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.UDOptimize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.UDSmoothing)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.UDFillingQuality)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.UDDownSample)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TLP;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.NumericUpDown UDSpotRemoval;
		private System.Windows.Forms.CheckBox CbSpotRemoval;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.NumericUpDown UDOptimize;
		private System.Windows.Forms.NumericUpDown UDSmoothing;
		private System.Windows.Forms.CheckBox CbOptimize;
		private System.Windows.Forms.CheckBox CbSmoothing;
		private System.Windows.Forms.Label label14;
		private UserControls.EnumComboBox CbFillingDirection;
		private System.Windows.Forms.Label LblFillingQuality;
		private System.Windows.Forms.NumericUpDown UDFillingQuality;
		private System.Windows.Forms.Label LblFillingLineLbl;
		private System.Windows.Forms.NumericUpDown UDDownSample;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox CbDownSample;
		private UserControls.ImageButton BtnFillingQualityInfo;
	}
}
