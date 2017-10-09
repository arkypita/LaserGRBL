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
			this.GbSize = new System.Windows.Forms.GroupBox();
			this.BtnRepo = new LaserGRBL.UserControls.ImageButton();
			this.TLP = new System.Windows.Forms.TableLayoutPanel();
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
			this.BtnAddBM = new LaserGRBL.UserControls.ImageButton();
			this.GbSize.SuspendLayout();
			this.TLP.SuspendLayout();
			this.SuspendLayout();
			// 
			// GbSize
			// 
			this.GbSize.AutoSize = true;
			this.GbSize.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GbSize.Controls.Add(this.BtnAddBM);
			this.GbSize.Controls.Add(this.BtnRepo);
			this.GbSize.Controls.Add(this.TLP);
			this.GbSize.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbSize.Location = new System.Drawing.Point(0, 0);
			this.GbSize.Name = "GbSize";
			this.GbSize.Size = new System.Drawing.Size(193, 61);
			this.GbSize.TabIndex = 1;
			this.GbSize.TabStop = false;
			this.GbSize.Text = "Image Size and Position [mm]";
			// 
			// BtnRepo
			// 
			this.BtnRepo.AltImage = null;
			this.BtnRepo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnRepo.BackColor = System.Drawing.Color.Transparent;
			this.BtnRepo.Coloration = System.Drawing.Color.Empty;
			this.BtnRepo.Image = ((System.Drawing.Image)(resources.GetObject("BtnRepo.Image")));
			this.BtnRepo.Location = new System.Drawing.Point(171, 0);
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
			this.TLP.ColumnCount = 5;
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP.Controls.Add(this.label9, 0, 1);
			this.TLP.Controls.Add(this.label4, 0, 0);
			this.TLP.Controls.Add(this.IIOffsetX, 2, 1);
			this.TLP.Controls.Add(this.IIOffsetY, 4, 1);
			this.TLP.Controls.Add(this.IISizeH, 4, 0);
			this.TLP.Controls.Add(this.IISizeW, 2, 0);
			this.TLP.Controls.Add(this.label6, 1, 0);
			this.TLP.Controls.Add(this.label10, 1, 1);
			this.TLP.Controls.Add(this.label7, 3, 0);
			this.TLP.Controls.Add(this.label11, 3, 1);
			this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP.Location = new System.Drawing.Point(3, 16);
			this.TLP.Name = "TLP";
			this.TLP.RowCount = 2;
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
			this.TLP.Size = new System.Drawing.Size(187, 42);
			this.TLP.TabIndex = 0;
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
			// BtnAddBM
			// 
			this.BtnAddBM.AltImage = null;
			this.BtnAddBM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnAddBM.BackColor = System.Drawing.Color.Transparent;
			this.BtnAddBM.Coloration = System.Drawing.Color.Empty;
			this.BtnAddBM.Image = ((System.Drawing.Image)(resources.GetObject("BtnAddBM.Image")));
			this.BtnAddBM.Location = new System.Drawing.Point(149, 0);
			this.BtnAddBM.Name = "BtnAddBM";
			this.BtnAddBM.Size = new System.Drawing.Size(17, 17);
			this.BtnAddBM.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnAddBM.TabIndex = 4;
			this.BtnAddBM.UseAltImage = false;
			// 
			// SetupOutput
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.GbSize);
			this.Name = "SetupOutput";
			this.Size = new System.Drawing.Size(193, 61);
			this.GbSize.ResumeLayout(false);
			this.GbSize.PerformLayout();
			this.TLP.ResumeLayout(false);
			this.TLP.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox GbSize;
		private System.Windows.Forms.TableLayoutPanel TLP;
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
		private UserControls.ImageButton BtnRepo;
		private UserControls.ImageButton BtnAddBM;

	}
}
