namespace LaserGRBL.UserControls
{
	partial class GrblPanel
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
			this.SuspendLayout();
			// 
			// GrblPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "GrblPanel";
			this.Size = new System.Drawing.Size(548, 400);
			this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.GrblPanel_MouseDoubleClick);
			this.ResumeLayout(false);

		}

		#endregion

	}
}
