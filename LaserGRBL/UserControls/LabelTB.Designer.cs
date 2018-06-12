namespace LaserGRBL.UserControls
{
	partial class LabelTB
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelTB));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.TB = new System.Windows.Forms.TrackBar();
			this.Lbl = new System.Windows.Forms.Label();
			this.BtnReset = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TB)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.BtnReset)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.TB, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.Lbl, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.BtnReset, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(280, 51);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// TB
			// 
			this.TB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TB.AutoSize = false;
			this.TB.Location = new System.Drawing.Point(99, 3);
			this.TB.MinimumSize = new System.Drawing.Size(150, 0);
			this.TB.Name = "TB";
			this.TB.Size = new System.Drawing.Size(150, 45);
			this.TB.TabIndex = 6;
			this.TB.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.TB.ValueChanged += new System.EventHandler(this.TB_ValueChanged);
			this.TB.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TB_MouseUp);
			// 
			// Lbl
			// 
			this.Lbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.Lbl.AutoSize = true;
			this.Lbl.Location = new System.Drawing.Point(3, 19);
			this.Lbl.MinimumSize = new System.Drawing.Size(90, 0);
			this.Lbl.Name = "Lbl";
			this.Lbl.Size = new System.Drawing.Size(90, 13);
			this.Lbl.TabIndex = 7;
			this.Lbl.Text = "Text";
			// 
			// BtnReset
			// 
			this.BtnReset.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BtnReset.Image = ((System.Drawing.Image)(resources.GetObject("BtnReset.Image")));
			this.BtnReset.Location = new System.Drawing.Point(255, 3);
			this.BtnReset.Name = "BtnReset";
			this.BtnReset.Size = new System.Drawing.Size(22, 45);
			this.BtnReset.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.BtnReset.TabIndex = 8;
			this.BtnReset.TabStop = false;
			this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
			// 
			// LabelTB
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "LabelTB";
			this.Size = new System.Drawing.Size(280, 51);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.TB)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.BtnReset)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TrackBar TB;
		private System.Windows.Forms.Label Lbl;

		#endregion

		private System.Windows.Forms.PictureBox BtnReset;
	}
}
