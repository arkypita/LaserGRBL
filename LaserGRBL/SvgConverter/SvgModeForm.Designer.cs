namespace LaserGRBL.SvgConverter
{
	partial class SvgModeForm
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
			this.PbVector = new System.Windows.Forms.PictureBox();
			this.PbImage = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.LblLoadVector = new System.Windows.Forms.Label();
			this.LblLoadImage = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.PbVector)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PbImage)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// PbVector
			// 
			this.PbVector.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PbVector.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PbVector.Location = new System.Drawing.Point(3, 3);
			this.PbVector.Name = "PbVector";
			this.PbVector.Size = new System.Drawing.Size(354, 396);
			this.PbVector.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PbVector.TabIndex = 0;
			this.PbVector.TabStop = false;
			this.PbVector.Click += new System.EventHandler(this.PbVector_Click);
			// 
			// PbImage
			// 
			this.PbImage.Cursor = System.Windows.Forms.Cursors.Hand;
			this.PbImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PbImage.Location = new System.Drawing.Point(371, 3);
			this.PbImage.Name = "PbImage";
			this.PbImage.Size = new System.Drawing.Size(355, 396);
			this.PbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PbImage.TabIndex = 1;
			this.PbImage.TabStop = false;
			this.PbImage.Click += new System.EventHandler(this.PbImage_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.PbVector, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.PbImage, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.LblLoadVector, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.LblLoadImage, 2, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.04546F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(729, 415);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// LblLoadVector
			// 
			this.LblLoadVector.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.LblLoadVector.AutoSize = true;
			this.LblLoadVector.Location = new System.Drawing.Point(140, 402);
			this.LblLoadVector.Name = "LblLoadVector";
			this.LblLoadVector.Size = new System.Drawing.Size(79, 13);
			this.LblLoadVector.TabIndex = 2;
			this.LblLoadVector.Text = "Load as Vector";
			// 
			// LblLoadImage
			// 
			this.LblLoadImage.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.LblLoadImage.AutoSize = true;
			this.LblLoadImage.Location = new System.Drawing.Point(510, 402);
			this.LblLoadImage.Name = "LblLoadImage";
			this.LblLoadImage.Size = new System.Drawing.Size(77, 13);
			this.LblLoadImage.TabIndex = 3;
			this.LblLoadImage.Text = "Load as Image";
			// 
			// SvgModeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(729, 415);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "SvgModeForm";
			this.Text = "SvgModeForm";
			((System.ComponentModel.ISupportInitialize)(this.PbVector)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PbImage)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox PbVector;
		private System.Windows.Forms.PictureBox PbImage;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label LblLoadVector;
		private System.Windows.Forms.Label LblLoadImage;
	}
}