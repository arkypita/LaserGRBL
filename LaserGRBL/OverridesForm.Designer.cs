namespace LaserGRBL
{
	partial class OverridesForm
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
			this.components = new System.ComponentModel.Container();
			this.TT = new System.Windows.Forms.ToolTip(this.components);
			this.tlp1 = new System.Windows.Forms.TableLayoutPanel();
			this.OvRapid = new System.Windows.Forms.TrackBar();
			this.OvFeed = new System.Windows.Forms.TrackBar();
			this.OvLaser = new System.Windows.Forms.TrackBar();
			this.tlp1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.OvRapid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.OvFeed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.OvLaser)).BeginInit();
			this.SuspendLayout();
			// 
			// tlp1
			// 
			this.tlp1.ColumnCount = 1;
			this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlp1.Controls.Add(this.OvLaser, 0, 2);
			this.tlp1.Controls.Add(this.OvRapid, 0, 0);
			this.tlp1.Controls.Add(this.OvFeed, 0, 1);
			this.tlp1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlp1.Location = new System.Drawing.Point(0, 0);
			this.tlp1.Name = "tlp1";
			this.tlp1.RowCount = 3;
			this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlp1.Size = new System.Drawing.Size(269, 193);
			this.tlp1.TabIndex = 1;
			// 
			// OvRapid
			// 
			this.OvRapid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.OvRapid.Enabled = false;
			this.OvRapid.LargeChange = 100;
			this.OvRapid.Location = new System.Drawing.Point(3, 23);
			this.OvRapid.Maximum = 2000;
			this.OvRapid.Minimum = 10;
			this.OvRapid.Name = "OvRapid";
			this.OvRapid.Size = new System.Drawing.Size(263, 45);
			this.OvRapid.SmallChange = 50;
			this.OvRapid.TabIndex = 17;
			this.OvRapid.TickFrequency = 100;
			this.OvRapid.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.OvRapid.Value = 1000;
			// 
			// OvFeed
			// 
			this.OvFeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.OvFeed.Enabled = false;
			this.OvFeed.LargeChange = 100;
			this.OvFeed.Location = new System.Drawing.Point(3, 74);
			this.OvFeed.Maximum = 2000;
			this.OvFeed.Minimum = 10;
			this.OvFeed.Name = "OvFeed";
			this.OvFeed.Size = new System.Drawing.Size(263, 45);
			this.OvFeed.SmallChange = 50;
			this.OvFeed.TabIndex = 18;
			this.OvFeed.TickFrequency = 100;
			this.OvFeed.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.OvFeed.Value = 1000;
			// 
			// OvLaser
			// 
			this.OvLaser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.OvLaser.Enabled = false;
			this.OvLaser.LargeChange = 100;
			this.OvLaser.Location = new System.Drawing.Point(3, 125);
			this.OvLaser.Maximum = 2000;
			this.OvLaser.Minimum = 10;
			this.OvLaser.Name = "OvLaser";
			this.OvLaser.Size = new System.Drawing.Size(263, 45);
			this.OvLaser.SmallChange = 50;
			this.OvLaser.TabIndex = 19;
			this.OvLaser.TickFrequency = 100;
			this.OvLaser.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.OvLaser.Value = 1000;
			// 
			// OverridesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(269, 193);
			this.Controls.Add(this.tlp1);
			this.DockAreas = ((LaserGRBL.UserControls.DockingManager.DockAreas)(((LaserGRBL.UserControls.DockingManager.DockAreas.Float | LaserGRBL.UserControls.DockingManager.DockAreas.DockLeft) 
			| LaserGRBL.UserControls.DockingManager.DockAreas.DockRight)));
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HideOnClose = true;
			this.Name = "OverridesForm";
			this.ShowHint = LaserGRBL.UserControls.DockingManager.DockState.Float;
			this.Text = "Overrides";
			this.ToolTipText = "Overrides";
			this.tlp1.ResumeLayout(false);
			this.tlp1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.OvRapid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.OvFeed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.OvLaser)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.TableLayoutPanel tlp1;
		private System.Windows.Forms.TrackBar OvRapid;
		private System.Windows.Forms.TrackBar OvFeed;
		private System.Windows.Forms.TrackBar OvLaser;
	}
}
