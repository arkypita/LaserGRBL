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
			this.TbRapid = new System.Windows.Forms.TrackBar();
			this.TbSpeed = new System.Windows.Forms.TrackBar();
			this.TbPower = new System.Windows.Forms.TrackBar();
			this.LblRapid = new System.Windows.Forms.Label();
			this.LblPower = new System.Windows.Forms.Label();
			this.LblSpeed = new System.Windows.Forms.Label();
			this.tlp1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TbRapid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TbSpeed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TbPower)).BeginInit();
			this.SuspendLayout();
			// 
			// tlp1
			// 
			this.tlp1.ColumnCount = 2;
			this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
			this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlp1.Controls.Add(this.TbRapid, 1, 1);
			this.tlp1.Controls.Add(this.TbSpeed, 1, 2);
			this.tlp1.Controls.Add(this.TbPower, 1, 3);
			this.tlp1.Controls.Add(this.LblRapid, 0, 1);
			this.tlp1.Controls.Add(this.LblPower, 0, 3);
			this.tlp1.Controls.Add(this.LblSpeed, 0, 2);
			this.tlp1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlp1.Location = new System.Drawing.Point(0, 0);
			this.tlp1.Name = "tlp1";
			this.tlp1.RowCount = 5;
			this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
			this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
			this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
			this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlp1.Size = new System.Drawing.Size(295, 194);
			this.tlp1.TabIndex = 1;
			// 
			// TbRapid
			// 
			this.TbRapid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TbRapid.LargeChange = 1;
			this.TbRapid.Location = new System.Drawing.Point(88, 20);
			this.TbRapid.Maximum = 2;
			this.TbRapid.Name = "TbRapid";
			this.TbRapid.Size = new System.Drawing.Size(204, 47);
			this.TbRapid.TabIndex = 0;
			this.TbRapid.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.TbRapid.Value = 2;
			this.TbRapid.ValueChanged += new System.EventHandler(this.TbRapid_ValueChanged);
			// 
			// TbSpeed
			// 
			this.TbSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TbSpeed.Location = new System.Drawing.Point(88, 73);
			this.TbSpeed.Maximum = 200;
			this.TbSpeed.Minimum = 10;
			this.TbSpeed.Name = "TbSpeed";
			this.TbSpeed.Size = new System.Drawing.Size(204, 47);
			this.TbSpeed.TabIndex = 1;
			this.TbSpeed.TickFrequency = 10;
			this.TbSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.TbSpeed.Value = 100;
			this.TbSpeed.ValueChanged += new System.EventHandler(this.TbSpeed_ValueChanged);
			// 
			// TbPower
			// 
			this.TbPower.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TbPower.Location = new System.Drawing.Point(88, 126);
			this.TbPower.Maximum = 200;
			this.TbPower.Minimum = 10;
			this.TbPower.Name = "TbPower";
			this.TbPower.Size = new System.Drawing.Size(204, 47);
			this.TbPower.TabIndex = 2;
			this.TbPower.TickFrequency = 10;
			this.TbPower.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.TbPower.Value = 100;
			this.TbPower.ValueChanged += new System.EventHandler(this.TbPower_ValueChanged);
			// 
			// LblRapid
			// 
			this.LblRapid.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblRapid.AutoSize = true;
			this.LblRapid.Location = new System.Drawing.Point(3, 37);
			this.LblRapid.Name = "LblRapid";
			this.LblRapid.Size = new System.Drawing.Size(70, 13);
			this.LblRapid.TabIndex = 3;
			this.LblRapid.Text = "Rapid [1.00x]";
			// 
			// LblPower
			// 
			this.LblPower.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblPower.AutoSize = true;
			this.LblPower.Location = new System.Drawing.Point(3, 143);
			this.LblPower.Name = "LblPower";
			this.LblPower.Size = new System.Drawing.Size(72, 13);
			this.LblPower.TabIndex = 5;
			this.LblPower.Text = "Power [1.00x]";
			// 
			// LblSpeed
			// 
			this.LblSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblSpeed.AutoSize = true;
			this.LblSpeed.Location = new System.Drawing.Point(3, 90);
			this.LblSpeed.Name = "LblSpeed";
			this.LblSpeed.Size = new System.Drawing.Size(73, 13);
			this.LblSpeed.TabIndex = 4;
			this.LblSpeed.Text = "Speed [1.00x]";
			// 
			// OverridesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(295, 194);
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
			((System.ComponentModel.ISupportInitialize)(this.TbRapid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TbSpeed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TbPower)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.TableLayoutPanel tlp1;
		private System.Windows.Forms.TrackBar TbRapid;
		private System.Windows.Forms.TrackBar TbSpeed;
		private System.Windows.Forms.TrackBar TbPower;
		private System.Windows.Forms.Label LblRapid;
		private System.Windows.Forms.Label LblPower;
		private System.Windows.Forms.Label LblSpeed;
	}
}
