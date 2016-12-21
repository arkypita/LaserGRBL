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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.CbMode = new System.Windows.Forms.ComboBox();
			this.TbBright = new System.Windows.Forms.TrackBar();
			this.Tp = new System.Windows.Forms.TabControl();
			this.TpPreview = new System.Windows.Forms.TabPage();
			this.PbConverted = new System.Windows.Forms.PictureBox();
			this.TpOriginal = new System.Windows.Forms.TabPage();
			this.PbOriginal = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.TbContrast = new System.Windows.Forms.TrackBar();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TbBright)).BeginInit();
			this.Tp.SuspendLayout();
			this.TpPreview.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PbConverted)).BeginInit();
			this.TpOriginal.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PbOriginal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TbContrast)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.Tp, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
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
			this.panel1.Size = new System.Drawing.Size(212, 472);
			this.panel1.TabIndex = 2;
			// 
			// groupBox2
			// 
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point(0, 148);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(212, 217);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Gcode generation";
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(212, 148);
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
			this.tableLayoutPanel2.Size = new System.Drawing.Size(206, 129);
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
			this.CbMode.Size = new System.Drawing.Size(138, 21);
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
			this.TbBright.Size = new System.Drawing.Size(138, 45);
			this.TbBright.TabIndex = 3;
			this.TbBright.TickFrequency = 10;
			this.TbBright.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.TbBright.Value = 100;
			this.TbBright.Scroll += new System.EventHandler(this.TbBright_Scroll);
			// 
			// Tp
			// 
			this.Tp.Controls.Add(this.TpPreview);
			this.Tp.Controls.Add(this.TpOriginal);
			this.Tp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Tp.Location = new System.Drawing.Point(221, 3);
			this.Tp.Name = "Tp";
			this.Tp.SelectedIndex = 0;
			this.Tp.Size = new System.Drawing.Size(622, 472);
			this.Tp.TabIndex = 3;
			// 
			// TpPreview
			// 
			this.TpPreview.Controls.Add(this.PbConverted);
			this.TpPreview.Location = new System.Drawing.Point(4, 22);
			this.TpPreview.Name = "TpPreview";
			this.TpPreview.Padding = new System.Windows.Forms.Padding(3);
			this.TpPreview.Size = new System.Drawing.Size(614, 446);
			this.TpPreview.TabIndex = 0;
			this.TpPreview.Text = "Preview";
			this.TpPreview.UseVisualStyleBackColor = true;
			// 
			// PbConverted
			// 
			this.PbConverted.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PbConverted.Location = new System.Drawing.Point(3, 3);
			this.PbConverted.Name = "PbConverted";
			this.PbConverted.Size = new System.Drawing.Size(608, 440);
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
			this.TpOriginal.Size = new System.Drawing.Size(614, 446);
			this.TpOriginal.TabIndex = 1;
			this.TpOriginal.Text = "Original";
			this.TpOriginal.UseVisualStyleBackColor = true;
			// 
			// PbOriginal
			// 
			this.PbOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PbOriginal.Location = new System.Drawing.Point(3, 3);
			this.PbOriginal.Name = "PbOriginal";
			this.PbOriginal.Size = new System.Drawing.Size(608, 440);
			this.PbOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PbOriginal.TabIndex = 0;
			this.PbOriginal.TabStop = false;
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
			// TbContrast
			// 
			this.TbContrast.AutoSize = false;
			this.TbContrast.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TbContrast.Location = new System.Drawing.Point(65, 81);
			this.TbContrast.Maximum = 160;
			this.TbContrast.Minimum = 40;
			this.TbContrast.Name = "TbContrast";
			this.TbContrast.Size = new System.Drawing.Size(138, 45);
			this.TbContrast.TabIndex = 5;
			this.TbContrast.TickFrequency = 10;
			this.TbContrast.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.TbContrast.Value = 100;
			this.TbContrast.Scroll += new System.EventHandler(this.TbContrast_Scroll);
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
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.TbBright)).EndInit();
			this.Tp.ResumeLayout(false);
			this.TpPreview.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PbConverted)).EndInit();
			this.TpOriginal.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PbOriginal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TbContrast)).EndInit();
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
	}
}