namespace LaserGRBL.UI.Forms.RasterConverter
{
	partial class SetupTool
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
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label27 = new System.Windows.Forms.Label();
			this.CbTools = new LaserGRBL.UserControls.EnumComboBox();
			this.GB = new System.Windows.Forms.GroupBox();
			this.PNL = new System.Windows.Forms.Panel();
			this.CbLinePreview = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel2.SuspendLayout();
			this.GB.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.CbLinePreview, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.label27, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.CbTools, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.PNL, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(207, 95);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// label27
			// 
			this.label27.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label27.AutoSize = true;
			this.label27.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label27.Location = new System.Drawing.Point(3, 6);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(28, 13);
			this.label27.TabIndex = 14;
			this.label27.Text = "Tool";
			// 
			// CbTools
			// 
			this.CbTools.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CbTools.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbTools.FormattingEnabled = true;
			this.CbTools.Location = new System.Drawing.Point(36, 2);
			this.CbTools.Margin = new System.Windows.Forms.Padding(2);
			this.CbTools.Name = "CbTools";
			this.CbTools.SelectedItem = null;
			this.CbTools.Size = new System.Drawing.Size(169, 21);
			this.CbTools.TabIndex = 3;
			this.CbTools.SelectedIndexChanged += new System.EventHandler(this.CbTools_SelectedIndexChanged);
			// 
			// GB
			// 
			this.GB.AutoSize = true;
			this.GB.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GB.Controls.Add(this.tableLayoutPanel2);
			this.GB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GB.Location = new System.Drawing.Point(0, 0);
			this.GB.Name = "GB";
			this.GB.Size = new System.Drawing.Size(213, 114);
			this.GB.TabIndex = 1;
			this.GB.TabStop = false;
			this.GB.Text = "Conversion tool";
			// 
			// PNL
			// 
			this.PNL.AutoSize = true;
			this.PNL.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.SetColumnSpan(this.PNL, 2);
			this.PNL.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PNL.Location = new System.Drawing.Point(0, 25);
			this.PNL.Margin = new System.Windows.Forms.Padding(0);
			this.PNL.MinimumSize = new System.Drawing.Size(0, 50);
			this.PNL.Name = "PNL";
			this.PNL.Size = new System.Drawing.Size(207, 50);
			this.PNL.TabIndex = 15;
			// 
			// CbLinePreview
			// 
			this.CbLinePreview.AutoSize = true;
			this.CbLinePreview.Checked = true;
			this.CbLinePreview.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tableLayoutPanel2.SetColumnSpan(this.CbLinePreview, 2);
			this.CbLinePreview.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.CbLinePreview.Location = new System.Drawing.Point(2, 77);
			this.CbLinePreview.Margin = new System.Windows.Forms.Padding(2);
			this.CbLinePreview.Name = "CbLinePreview";
			this.CbLinePreview.Size = new System.Drawing.Size(87, 16);
			this.CbLinePreview.TabIndex = 16;
			this.CbLinePreview.Text = "Line Preview";
			this.CbLinePreview.UseVisualStyleBackColor = true;
			this.CbLinePreview.CheckedChanged += new System.EventHandler(this.CbLinePreview_CheckedChanged);
			// 
			// SetupTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.GB);
			this.Name = "SetupTool";
			this.Size = new System.Drawing.Size(213, 114);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.GB.ResumeLayout(false);
			this.GB.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private UserControls.EnumComboBox CbTools;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.GroupBox GB;
		private System.Windows.Forms.Panel PNL;
		private System.Windows.Forms.CheckBox CbLinePreview;
	}
}
