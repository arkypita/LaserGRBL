namespace LaserGRBL
{
	partial class JogForm
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
			this.UpdateFMax = new System.Windows.Forms.Timer(this.components);
			this.move2DControl1 = new LaserGRBL.UserControls.Move2DControl();
			this.SuspendLayout();
			// 
			// UpdateFMax
			// 
			this.UpdateFMax.Interval = 1000;
			this.UpdateFMax.Tick += new System.EventHandler(this.UpdateFMax_Tick);
			// 
			// move2DControl1
			// 
			this.move2DControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.move2DControl1.Location = new System.Drawing.Point(0, 0);
			this.move2DControl1.Name = "move2DControl1";
			this.move2DControl1.Padding = new System.Windows.Forms.Padding(2);
			this.move2DControl1.Size = new System.Drawing.Size(234, 125);
			this.move2DControl1.SpeedMaximum = 4000;
			this.move2DControl1.SpeedMinimum = 10;
			this.move2DControl1.SpeedValue = 1000;
			this.move2DControl1.TabIndex = 2;
			this.move2DControl1.MoveClick += new LaserGRBL.UserControls.Move2DControl.MoveEventHandler(this.Move_Click);
			this.move2DControl1.HomeClick += new LaserGRBL.UserControls.Move2DControl.HomeEventHandler(this.Home_Click);
			this.move2DControl1.SpeedChanged += new LaserGRBL.UserControls.Move2DControl.SpeedEventHandler(this.SpeedValueChanged);
			// 
			// JogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.move2DControl1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "JogForm";
			this.Size = new System.Drawing.Size(234, 125);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.Timer UpdateFMax;
		private UserControls.Move2DControl move2DControl1;
	}
}
