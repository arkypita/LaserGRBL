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
			this.label11 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.TbSizeH = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.TbSizeW = new System.Windows.Forms.TextBox();
			this.UDQuality = new System.Windows.Forms.NumericUpDown();
			this.TbOffsettX = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.TbOffsetY = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.CbMode = new System.Windows.Forms.ComboBox();
			this.TbBright = new System.Windows.Forms.TrackBar();
			this.TbContrast = new System.Windows.Forms.TrackBar();
			this.label3 = new System.Windows.Forms.Label();
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
			((System.ComponentModel.ISupportInitialize)(this.TbBright)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TbContrast)).BeginInit();
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
			this.tableLayoutPanel1.Size = new System.Drawing.Size(854, 545);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.groupBox2);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(220, 492);
			this.panel1.TabIndex = 2;
			// 
			// groupBox2
			// 
			this.groupBox2.AutoSize = true;
			this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox2.Controls.Add(this.tableLayoutPanel3);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point(0, 148);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(220, 97);
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
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.label11, 4, 2);
			this.tableLayoutPanel3.Controls.Add(this.label9, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.label8, 2, 1);
			this.tableLayoutPanel3.Controls.Add(this.label7, 4, 0);
			this.tableLayoutPanel3.Controls.Add(this.label6, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.TbSizeH, 3, 0);
			this.tableLayoutPanel3.Controls.Add(this.label5, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.TbSizeW, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.UDQuality, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.TbOffsettX, 1, 2);
			this.tableLayoutPanel3.Controls.Add(this.label10, 2, 2);
			this.tableLayoutPanel3.Controls.Add(this.TbOffsetY, 3, 2);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 3;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(214, 78);
			this.tableLayoutPanel3.TabIndex = 0;
			// 
			// label11
			// 
			this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(190, 58);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(14, 13);
			this.label11.TabIndex = 12;
			this.label11.Text = "Y";
			// 
			// label9
			// 
			this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(3, 58);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(35, 13);
			this.label9.TabIndex = 9;
			this.label9.Text = "Offset";
			// 
			// label8
			// 
			this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label8.AutoSize = true;
			this.tableLayoutPanel3.SetColumnSpan(this.label8, 3);
			this.label8.Location = new System.Drawing.Point(109, 32);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(49, 13);
			this.label8.TabIndex = 8;
			this.label8.Text = "lines/mm";
			// 
			// label7
			// 
			this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(190, 6);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(23, 13);
			this.label7.TabIndex = 6;
			this.label7.Text = "mm";
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(110, 6);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(12, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "x";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TbSizeH
			// 
			this.TbSizeH.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TbSizeH.Location = new System.Drawing.Point(129, 3);
			this.TbSizeH.Name = "TbSizeH";
			this.TbSizeH.Size = new System.Drawing.Size(55, 20);
			this.TbSizeH.TabIndex = 4;
			this.TbSizeH.TextChanged += new System.EventHandler(this.TbSizeHTextChanged);
			this.TbSizeH.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GoodInput);
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 32);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(39, 13);
			this.label5.TabIndex = 2;
			this.label5.Text = "Quality";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 6);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(27, 13);
			this.label4.TabIndex = 1;
			this.label4.Text = "Size";
			// 
			// TbSizeW
			// 
			this.TbSizeW.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TbSizeW.Location = new System.Drawing.Point(48, 3);
			this.TbSizeW.Name = "TbSizeW";
			this.TbSizeW.Size = new System.Drawing.Size(55, 20);
			this.TbSizeW.TabIndex = 3;
			this.TbSizeW.TextChanged += new System.EventHandler(this.TbSizeWTextChanged);
			this.TbSizeW.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GoodInput);
			// 
			// UDQuality
			// 
			this.UDQuality.Location = new System.Drawing.Point(48, 29);
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
			1,
			0,
			0,
			0});
			// 
			// TbOffsettX
			// 
			this.TbOffsettX.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TbOffsettX.Location = new System.Drawing.Point(48, 55);
			this.TbOffsettX.Name = "TbOffsettX";
			this.TbOffsettX.Size = new System.Drawing.Size(55, 20);
			this.TbOffsettX.TabIndex = 10;
			this.TbOffsettX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GoodInput);
			// 
			// label10
			// 
			this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(109, 58);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(14, 13);
			this.label10.TabIndex = 11;
			this.label10.Text = "X";
			// 
			// TbOffsetY
			// 
			this.TbOffsetY.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TbOffsetY.Location = new System.Drawing.Point(129, 55);
			this.TbOffsetY.Name = "TbOffsetY";
			this.TbOffsetY.Size = new System.Drawing.Size(55, 20);
			this.TbOffsetY.TabIndex = 12;
			this.TbOffsetY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GoodInput);
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(220, 148);
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
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.CbMode, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.TbBright, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.TbContrast, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(214, 129);
			this.tableLayoutPanel2.TabIndex = 0;
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
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Brightness";
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
			this.CbMode.SelectedIndexChanged += new System.EventHandler(this.CbMode_SelectedIndexChanged);
			// 
			// TbBright
			// 
			this.TbBright.AutoSize = false;
			this.TbBright.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TbBright.Location = new System.Drawing.Point(65, 30);
			this.TbBright.Maximum = 160;
			this.TbBright.Minimum = 40;
			this.TbBright.Name = "TbBright";
			this.TbBright.Size = new System.Drawing.Size(146, 45);
			this.TbBright.TabIndex = 3;
			this.TbBright.TickFrequency = 10;
			this.TbBright.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.TbBright.Value = 100;
			this.TbBright.Scroll += new System.EventHandler(this.TbBright_Scroll);
			// 
			// TbContrast
			// 
			this.TbContrast.AutoSize = false;
			this.TbContrast.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TbContrast.Location = new System.Drawing.Point(65, 81);
			this.TbContrast.Maximum = 160;
			this.TbContrast.Minimum = 40;
			this.TbContrast.Name = "TbContrast";
			this.TbContrast.Size = new System.Drawing.Size(146, 45);
			this.TbContrast.TabIndex = 5;
			this.TbContrast.TickFrequency = 10;
			this.TbContrast.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.TbContrast.Value = 100;
			this.TbContrast.Scroll += new System.EventHandler(this.TbContrast_Scroll);
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 97);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(46, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Contrast";
			// 
			// Tp
			// 
			this.Tp.Controls.Add(this.TpPreview);
			this.Tp.Controls.Add(this.TpOriginal);
			this.Tp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Tp.Location = new System.Drawing.Point(229, 3);
			this.Tp.Name = "Tp";
			this.Tp.SelectedIndex = 0;
			this.Tp.Size = new System.Drawing.Size(622, 492);
			this.Tp.TabIndex = 3;
			// 
			// TpPreview
			// 
			this.TpPreview.Controls.Add(this.PbConverted);
			this.TpPreview.Location = new System.Drawing.Point(4, 22);
			this.TpPreview.Name = "TpPreview";
			this.TpPreview.Padding = new System.Windows.Forms.Padding(3);
			this.TpPreview.Size = new System.Drawing.Size(614, 466);
			this.TpPreview.TabIndex = 0;
			this.TpPreview.Text = "Preview";
			this.TpPreview.UseVisualStyleBackColor = true;
			// 
			// PbConverted
			// 
			this.PbConverted.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PbConverted.Location = new System.Drawing.Point(3, 3);
			this.PbConverted.Name = "PbConverted";
			this.PbConverted.Size = new System.Drawing.Size(608, 460);
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
			this.TpOriginal.Size = new System.Drawing.Size(614, 466);
			this.TpOriginal.TabIndex = 1;
			this.TpOriginal.Text = "Original";
			this.TpOriginal.UseVisualStyleBackColor = true;
			// 
			// PbOriginal
			// 
			this.PbOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PbOriginal.Location = new System.Drawing.Point(3, 3);
			this.PbOriginal.Name = "PbOriginal";
			this.PbOriginal.Size = new System.Drawing.Size(608, 460);
			this.PbOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PbOriginal.TabIndex = 0;
			this.PbOriginal.TabStop = false;
			// 
			// BtnCreate
			// 
			this.BtnCreate.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.BtnCreate.Location = new System.Drawing.Point(744, 501);
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
			this.ClientSize = new System.Drawing.Size(854, 545);
			this.Controls.Add(this.tableLayoutPanel1);
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
			((System.ComponentModel.ISupportInitialize)(this.TbBright)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TbContrast)).EndInit();
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
		private System.Windows.Forms.TextBox TbSizeH;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox TbSizeW;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown UDQuality;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox TbOffsettX;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox TbOffsetY;
		private System.Windows.Forms.Button BtnCreate;
	}
}