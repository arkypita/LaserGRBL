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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupTool));
			this.TLP = new System.Windows.Forms.TableLayoutPanel();
			this.label27 = new System.Windows.Forms.Label();
			this.CbTools = new LaserGRBL.UserControls.EnumComboBox();
			this.PNL = new System.Windows.Forms.Panel();
			this.GB = new System.Windows.Forms.GroupBox();
			this.BtnRepo = new LaserGRBL.UserControls.ImageButton();
			this.TLP.SuspendLayout();
			this.GB.SuspendLayout();
			this.SuspendLayout();
			// 
			// TLP
			// 
			this.TLP.AutoSize = true;
			this.TLP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP.ColumnCount = 2;
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLP.Controls.Add(this.label27, 0, 0);
			this.TLP.Controls.Add(this.CbTools, 1, 0);
			this.TLP.Controls.Add(this.PNL, 0, 1);
			this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLP.Location = new System.Drawing.Point(3, 16);
			this.TLP.Name = "TLP";
			this.TLP.RowCount = 2;
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TLP.Size = new System.Drawing.Size(207, 75);
			this.TLP.TabIndex = 0;
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
			// PNL
			// 
			this.PNL.AutoSize = true;
			this.PNL.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TLP.SetColumnSpan(this.PNL, 2);
			this.PNL.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PNL.Location = new System.Drawing.Point(0, 25);
			this.PNL.Margin = new System.Windows.Forms.Padding(0);
			this.PNL.MinimumSize = new System.Drawing.Size(0, 50);
			this.PNL.Name = "PNL";
			this.PNL.Size = new System.Drawing.Size(207, 50);
			this.PNL.TabIndex = 15;
			// 
			// GB
			// 
			this.GB.AutoSize = true;
			this.GB.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GB.Controls.Add(this.BtnRepo);
			this.GB.Controls.Add(this.TLP);
			this.GB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GB.Location = new System.Drawing.Point(0, 0);
			this.GB.Name = "GB";
			this.GB.Size = new System.Drawing.Size(213, 94);
			this.GB.TabIndex = 1;
			this.GB.TabStop = false;
			this.GB.Text = "Conversion tool";
			// 
			// BtnRepo
			// 
			this.BtnRepo.AltImage = null;
			this.BtnRepo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnRepo.BackColor = System.Drawing.Color.Transparent;
			this.BtnRepo.Coloration = System.Drawing.Color.Empty;
			this.BtnRepo.Image = ((System.Drawing.Image)(resources.GetObject("BtnRepo.Image")));
			this.BtnRepo.Location = new System.Drawing.Point(191, 0);
			this.BtnRepo.Name = "BtnRepo";
			this.BtnRepo.Size = new System.Drawing.Size(17, 17);
			this.BtnRepo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnRepo.TabIndex = 2;
			this.BtnRepo.UseAltImage = false;
			// 
			// SetupTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.GB);
			this.Name = "SetupTool";
			this.Size = new System.Drawing.Size(213, 94);
			this.TLP.ResumeLayout(false);
			this.TLP.PerformLayout();
			this.GB.ResumeLayout(false);
			this.GB.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TLP;
		private UserControls.EnumComboBox CbTools;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.GroupBox GB;
		private System.Windows.Forms.Panel PNL;
		private UserControls.ImageButton BtnRepo;
	}
}
