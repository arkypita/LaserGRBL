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
            this.LblSpeed = new System.Windows.Forms.Label();
            this.TbSpeed = new System.Windows.Forms.TrackBar();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.move2DControl1 = new LaserGRBL.UserControls.Move2DControl();
            ((System.ComponentModel.ISupportInitialize)(this.TbSpeed)).BeginInit();
            this.LeftPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpdateFMax
            // 
            this.UpdateFMax.Interval = 1000;
            this.UpdateFMax.Tick += new System.EventHandler(this.UpdateFMax_Tick);
            // 
            // LblSpeed
            // 
            this.LblSpeed.AutoSize = true;
            this.LblSpeed.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LblSpeed.Location = new System.Drawing.Point(0, 112);
            this.LblSpeed.Name = "LblSpeed";
            this.LblSpeed.Size = new System.Drawing.Size(37, 13);
            this.LblSpeed.TabIndex = 17;
            this.LblSpeed.Text = "F1000";
            // 
            // TbSpeed
            // 
            this.TbSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbSpeed.LargeChange = 100;
            this.TbSpeed.Location = new System.Drawing.Point(0, 0);
            this.TbSpeed.Maximum = 4000;
            this.TbSpeed.Minimum = 10;
            this.TbSpeed.Name = "TbSpeed";
            this.TbSpeed.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.TbSpeed.Size = new System.Drawing.Size(45, 112);
            this.TbSpeed.SmallChange = 50;
            this.TbSpeed.TabIndex = 16;
            this.TbSpeed.TickFrequency = 200;
            this.TbSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.TbSpeed.Value = 1000;
            this.TbSpeed.ValueChanged += new System.EventHandler(this.TbSpeed_ValueChanged);
            this.TbSpeed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnSliderMouseUP);
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.TbSpeed);
            this.LeftPanel.Controls.Add(this.LblSpeed);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(42, 125);
            this.LeftPanel.TabIndex = 1;
            // 
            // move2DControl1
            // 
            this.move2DControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.move2DControl1.Location = new System.Drawing.Point(42, 0);
            this.move2DControl1.Name = "move2DControl1";
            this.move2DControl1.Padding = new System.Windows.Forms.Padding(2);
            this.move2DControl1.Size = new System.Drawing.Size(192, 125);
            this.move2DControl1.TabIndex = 2;
            this.move2DControl1.MoveClick += new LaserGRBL.UserControls.Move2DControl.MoveEventHandler(this.Move_Click);
            this.move2DControl1.HomeClick += new System.EventHandler(this.Home_Click);
            // 
            // JogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.move2DControl1);
            this.Controls.Add(this.LeftPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "JogForm";
            this.Size = new System.Drawing.Size(234, 125);
            ((System.ComponentModel.ISupportInitialize)(this.TbSpeed)).EndInit();
            this.LeftPanel.ResumeLayout(false);
            this.LeftPanel.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.Timer UpdateFMax;
		private System.Windows.Forms.Label LblSpeed;
		private System.Windows.Forms.TrackBar TbSpeed;
		private System.Windows.Forms.Panel LeftPanel;
		private UserControls.Move2DControl move2DControl1;
	}
}
