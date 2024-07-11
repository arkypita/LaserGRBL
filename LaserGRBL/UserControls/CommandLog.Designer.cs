namespace LaserGRBL.UserControls
{
	partial class CommandLog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandLog));
			this.ScrollBar = new System.Windows.Forms.VScrollBar();
			this.TT = new System.Windows.Forms.ToolTip(this.components);
			this.IL = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// ScrollBar
			// 
			this.ScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.ScrollBar.LargeChange = 1;
			this.ScrollBar.Location = new System.Drawing.Point(276, 0);
			this.ScrollBar.Maximum = 0;
			this.ScrollBar.Name = "ScrollBar";
			this.ScrollBar.Size = new System.Drawing.Size(17, 404);
			this.ScrollBar.TabIndex = 0;
			// 
			// IL
			// 
			this.IL.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IL.ImageStream")));
			this.IL.TransparentColor = System.Drawing.Color.Transparent;
			this.IL.Images.SetKeyName(0, "logqueued.png");
			this.IL.Images.SetKeyName(1, "logok.png");
			this.IL.Images.SetKeyName(2, "logko.png");
			this.IL.Images.SetKeyName(3, "loginfo.png");
			this.IL.Images.SetKeyName(4, "warning.png");
			this.IL.Images.SetKeyName(5, "diagnostic.png");
			// 
			// CommandLog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ScrollBar);
			this.Name = "CommandLog";
			this.Size = new System.Drawing.Size(293, 404);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CommandLogMouseMove);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.VScrollBar ScrollBar;
		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.ImageList IL;
	}
}
