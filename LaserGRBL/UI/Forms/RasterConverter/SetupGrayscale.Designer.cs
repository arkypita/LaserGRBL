namespace LaserGRBL.UI.Forms.RasterConverter
{
	partial class SetupGrayscale
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
			this.GbParameters = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.LblGrayscale = new System.Windows.Forms.Label();
			this.CbMode = new LaserGRBL.UserControls.EnumComboBox();
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
			this.TbThreshold = new LaserGRBL.UserControls.ColorSlider();
			this.TBWhiteClip = new LaserGRBL.UserControls.ColorSlider();
			this.label4 = new System.Windows.Forms.Label();
			this.GbParameters.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// GbParameters
			// 
			this.GbParameters.AutoSize = true;
			this.GbParameters.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GbParameters.Controls.Add(this.tableLayoutPanel2);
			this.GbParameters.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GbParameters.Location = new System.Drawing.Point(0, 0);
			this.GbParameters.Name = "GbParameters";
			this.GbParameters.Size = new System.Drawing.Size(222, 212);
			this.GbParameters.TabIndex = 1;
			this.GbParameters.TabStop = false;
			this.GbParameters.Text = "Parameters";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.LblGrayscale, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.CbMode, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.TBRed, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.LblRed, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.LblBlue, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.LblGreen, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.label2, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.TBGreen, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.TbBright, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.TBBlue, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.TbContrast, 1, 5);
			this.tableLayoutPanel2.Controls.Add(this.label3, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.CbThreshold, 0, 7);
			this.tableLayoutPanel2.Controls.Add(this.TbThreshold, 1, 7);
			this.tableLayoutPanel2.Controls.Add(this.TBWhiteClip, 1, 6);
			this.tableLayoutPanel2.Controls.Add(this.label4, 0, 6);
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
			this.tableLayoutPanel2.Size = new System.Drawing.Size(216, 193);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// LblGrayscale
			// 
			this.LblGrayscale.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblGrayscale.AutoSize = true;
			this.LblGrayscale.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblGrayscale.Location = new System.Drawing.Point(3, 6);
			this.LblGrayscale.Name = "LblGrayscale";
			this.LblGrayscale.Size = new System.Drawing.Size(54, 13);
			this.LblGrayscale.TabIndex = 0;
			this.LblGrayscale.Text = "Grayscale";
			// 
			// CbMode
			// 
			this.CbMode.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbMode.FormattingEnabled = true;
			this.CbMode.Location = new System.Drawing.Point(64, 2);
			this.CbMode.Margin = new System.Windows.Forms.Padding(2);
			this.CbMode.Name = "CbMode";
			this.CbMode.SelectedItem = null;
			this.CbMode.Size = new System.Drawing.Size(150, 21);
			this.CbMode.TabIndex = 2;
			this.CbMode.SelectedIndexChanged += new System.EventHandler(this.CbMode_SelectedIndexChanged);
			// 
			// TBRed
			// 
			this.TBRed.BackColor = System.Drawing.Color.Transparent;
			this.TBRed.BarInnerColor = System.Drawing.Color.Firebrick;
			this.TBRed.BarOuterColor = System.Drawing.Color.DarkRed;
			this.TBRed.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TBRed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TBRed.ElapsedInnerColor = System.Drawing.Color.Red;
			this.TBRed.ElapsedOuterColor = System.Drawing.Color.DarkRed;
			this.TBRed.LargeChange = ((uint)(5u));
			this.TBRed.Location = new System.Drawing.Point(63, 26);
			this.TBRed.Margin = new System.Windows.Forms.Padding(1);
			this.TBRed.Maximum = 160;
			this.TBRed.Minimum = 40;
			this.TBRed.Name = "TBRed";
			this.TBRed.Size = new System.Drawing.Size(152, 22);
			this.TBRed.SmallChange = ((uint)(1u));
			this.TBRed.TabIndex = 7;
			this.TBRed.ThumbRoundRectSize = new System.Drawing.Size(4, 4);
			this.TBRed.ThumbSize = 8;
			this.TBRed.Value = 100;
			this.TBRed.Visible = false;
			this.TBRed.ValueChanged += new System.EventHandler(this.TBRed_ValueChanged);
			this.TBRed.DoubleClick += new System.EventHandler(this.OnRGBCBDoubleClick);
			// 
			// LblRed
			// 
			this.LblRed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblRed.AutoSize = true;
			this.LblRed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblRed.Location = new System.Drawing.Point(3, 30);
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
			this.LblBlue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblBlue.Location = new System.Drawing.Point(3, 78);
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
			this.LblGreen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblGreen.Location = new System.Drawing.Point(3, 54);
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
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(3, 102);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Brightness";
			// 
			// TBGreen
			// 
			this.TBGreen.BackColor = System.Drawing.Color.Transparent;
			this.TBGreen.BarInnerColor = System.Drawing.Color.Green;
			this.TBGreen.BarOuterColor = System.Drawing.Color.DarkGreen;
			this.TBGreen.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TBGreen.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TBGreen.LargeChange = ((uint)(5u));
			this.TBGreen.Location = new System.Drawing.Point(63, 50);
			this.TBGreen.Margin = new System.Windows.Forms.Padding(1);
			this.TBGreen.Maximum = 160;
			this.TBGreen.Minimum = 40;
			this.TBGreen.Name = "TBGreen";
			this.TBGreen.Size = new System.Drawing.Size(152, 22);
			this.TBGreen.SmallChange = ((uint)(1u));
			this.TBGreen.TabIndex = 9;
			this.TBGreen.ThumbRoundRectSize = new System.Drawing.Size(4, 4);
			this.TBGreen.ThumbSize = 8;
			this.TBGreen.Value = 100;
			this.TBGreen.Visible = false;
			this.TBGreen.ValueChanged += new System.EventHandler(this.TBGreen_ValueChanged);
			this.TBGreen.DoubleClick += new System.EventHandler(this.OnRGBCBDoubleClick);
			// 
			// TbBright
			// 
			this.TbBright.BackColor = System.Drawing.Color.Transparent;
			this.TbBright.BarInnerColor = System.Drawing.Color.DimGray;
			this.TbBright.BarOuterColor = System.Drawing.Color.Black;
			this.TbBright.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TbBright.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TbBright.ElapsedInnerColor = System.Drawing.Color.White;
			this.TbBright.ElapsedOuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.TbBright.LargeChange = ((uint)(5u));
			this.TbBright.Location = new System.Drawing.Point(63, 98);
			this.TbBright.Margin = new System.Windows.Forms.Padding(1);
			this.TbBright.Maximum = 160;
			this.TbBright.Minimum = 40;
			this.TbBright.Name = "TbBright";
			this.TbBright.Size = new System.Drawing.Size(152, 22);
			this.TbBright.SmallChange = ((uint)(1u));
			this.TbBright.TabIndex = 3;
			this.TbBright.ThumbRoundRectSize = new System.Drawing.Size(4, 4);
			this.TbBright.ThumbSize = 8;
			this.TbBright.Value = 100;
			this.TbBright.ValueChanged += new System.EventHandler(this.TbBright_ValueChanged);
			this.TbBright.DoubleClick += new System.EventHandler(this.OnRGBCBDoubleClick);
			// 
			// TBBlue
			// 
			this.TBBlue.BackColor = System.Drawing.Color.Transparent;
			this.TBBlue.BarInnerColor = System.Drawing.Color.MediumBlue;
			this.TBBlue.BarOuterColor = System.Drawing.Color.DarkBlue;
			this.TBBlue.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TBBlue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TBBlue.ElapsedInnerColor = System.Drawing.Color.DodgerBlue;
			this.TBBlue.ElapsedOuterColor = System.Drawing.Color.SteelBlue;
			this.TBBlue.LargeChange = ((uint)(5u));
			this.TBBlue.Location = new System.Drawing.Point(63, 74);
			this.TBBlue.Margin = new System.Windows.Forms.Padding(1);
			this.TBBlue.Maximum = 160;
			this.TBBlue.Minimum = 40;
			this.TBBlue.Name = "TBBlue";
			this.TBBlue.Size = new System.Drawing.Size(152, 22);
			this.TBBlue.SmallChange = ((uint)(1u));
			this.TBBlue.TabIndex = 10;
			this.TBBlue.ThumbRoundRectSize = new System.Drawing.Size(4, 4);
			this.TBBlue.ThumbSize = 8;
			this.TBBlue.Value = 100;
			this.TBBlue.Visible = false;
			this.TBBlue.ValueChanged += new System.EventHandler(this.TBBlue_ValueChanged);
			this.TBBlue.DoubleClick += new System.EventHandler(this.OnRGBCBDoubleClick);
			// 
			// TbContrast
			// 
			this.TbContrast.BackColor = System.Drawing.Color.Transparent;
			this.TbContrast.BarInnerColor = System.Drawing.Color.DimGray;
			this.TbContrast.BarOuterColor = System.Drawing.Color.Black;
			this.TbContrast.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TbContrast.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TbContrast.ElapsedInnerColor = System.Drawing.Color.White;
			this.TbContrast.ElapsedOuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.TbContrast.LargeChange = ((uint)(5u));
			this.TbContrast.Location = new System.Drawing.Point(63, 122);
			this.TbContrast.Margin = new System.Windows.Forms.Padding(1);
			this.TbContrast.Maximum = 160;
			this.TbContrast.Minimum = 40;
			this.TbContrast.Name = "TbContrast";
			this.TbContrast.Size = new System.Drawing.Size(152, 22);
			this.TbContrast.SmallChange = ((uint)(1u));
			this.TbContrast.TabIndex = 5;
			this.TbContrast.ThumbRoundRectSize = new System.Drawing.Size(4, 4);
			this.TbContrast.ThumbSize = 8;
			this.TbContrast.Value = 100;
			this.TbContrast.ValueChanged += new System.EventHandler(this.TbContrast_ValueChanged);
			this.TbContrast.DoubleClick += new System.EventHandler(this.OnRGBCBDoubleClick);
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(3, 126);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(46, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Contrast";
			// 
			// CbThreshold
			// 
			this.CbThreshold.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.CbThreshold.AutoSize = true;
			this.CbThreshold.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.CbThreshold.Location = new System.Drawing.Point(2, 172);
			this.CbThreshold.Margin = new System.Windows.Forms.Padding(2);
			this.CbThreshold.Name = "CbThreshold";
			this.CbThreshold.Size = new System.Drawing.Size(50, 17);
			this.CbThreshold.TabIndex = 15;
			this.CbThreshold.Text = "B&&W";
			this.CbThreshold.UseVisualStyleBackColor = true;
			this.CbThreshold.CheckedChanged += new System.EventHandler(this.CbThreshold_CheckedChanged);
			// 
			// TbThreshold
			// 
			this.TbThreshold.BackColor = System.Drawing.Color.Transparent;
			this.TbThreshold.BarInnerColor = System.Drawing.Color.DimGray;
			this.TbThreshold.BarOuterColor = System.Drawing.Color.Black;
			this.TbThreshold.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TbThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TbThreshold.ElapsedInnerColor = System.Drawing.Color.White;
			this.TbThreshold.ElapsedOuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.TbThreshold.LargeChange = ((uint)(5u));
			this.TbThreshold.Location = new System.Drawing.Point(63, 170);
			this.TbThreshold.Margin = new System.Windows.Forms.Padding(1);
			this.TbThreshold.Name = "TbThreshold";
			this.TbThreshold.Size = new System.Drawing.Size(152, 22);
			this.TbThreshold.SmallChange = ((uint)(1u));
			this.TbThreshold.TabIndex = 14;
			this.TbThreshold.ThumbRoundRectSize = new System.Drawing.Size(4, 4);
			this.TbThreshold.ThumbSize = 8;
			this.TbThreshold.ValueChanged += new System.EventHandler(this.TbThreshold_ValueChanged);
			this.TbThreshold.DoubleClick += new System.EventHandler(this.OnThresholdDoubleClick);
			// 
			// TBWhiteClip
			// 
			this.TBWhiteClip.BackColor = System.Drawing.Color.Transparent;
			this.TBWhiteClip.BarInnerColor = System.Drawing.Color.DimGray;
			this.TBWhiteClip.BarOuterColor = System.Drawing.Color.Black;
			this.TBWhiteClip.BorderRoundRectSize = new System.Drawing.Size(8, 8);
			this.TBWhiteClip.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TBWhiteClip.ElapsedInnerColor = System.Drawing.Color.White;
			this.TBWhiteClip.ElapsedOuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.TBWhiteClip.LargeChange = ((uint)(5u));
			this.TBWhiteClip.Location = new System.Drawing.Point(63, 146);
			this.TBWhiteClip.Margin = new System.Windows.Forms.Padding(1);
			this.TBWhiteClip.Name = "TBWhiteClip";
			this.TBWhiteClip.Size = new System.Drawing.Size(152, 22);
			this.TBWhiteClip.SmallChange = ((uint)(1u));
			this.TBWhiteClip.TabIndex = 18;
			this.TBWhiteClip.ThumbRoundRectSize = new System.Drawing.Size(4, 4);
			this.TBWhiteClip.ThumbSize = 8;
			this.TBWhiteClip.Value = 5;
			this.TBWhiteClip.ValueChanged += new System.EventHandler(this.TBWhiteClip_ValueChanged);
			this.TBWhiteClip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TBWhiteClip_MouseDown);
			this.TBWhiteClip.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TBWhiteClip_MouseUp);
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label4.Location = new System.Drawing.Point(3, 150);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 13);
			this.label4.TabIndex = 19;
			this.label4.Text = "White Clip";
			// 
			// SetupGrayscale
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.GbParameters);
			this.Name = "SetupGrayscale";
			this.Size = new System.Drawing.Size(222, 212);
			this.GbParameters.ResumeLayout(false);
			this.GbParameters.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox GbParameters;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label LblGrayscale;
		private UserControls.EnumComboBox CbMode;
		private UserControls.ColorSlider TBRed;
		private System.Windows.Forms.Label LblRed;
		private System.Windows.Forms.Label LblBlue;
		private System.Windows.Forms.Label LblGreen;
		private System.Windows.Forms.Label label2;
		private UserControls.ColorSlider TBGreen;
		private UserControls.ColorSlider TbBright;
		private UserControls.ColorSlider TBBlue;
		private UserControls.ColorSlider TbContrast;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox CbThreshold;
		private UserControls.ColorSlider TbThreshold;
		private UserControls.ColorSlider TBWhiteClip;
		private System.Windows.Forms.Label label4;
	}
}
