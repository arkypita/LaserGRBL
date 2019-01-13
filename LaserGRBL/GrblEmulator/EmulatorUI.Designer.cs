namespace LaserGRBL.GrblEmulator
{
	partial class EmulatorUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmulatorUI));
            this.RT = new System.Windows.Forms.Timer(this.components);
            this.RTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // RT
            // 
            this.RT.Enabled = true;
            this.RT.Tick += new System.EventHandler(this.RT_Tick);
            // 
            // RTB
            // 
            this.RTB.BackColor = System.Drawing.Color.Black;
            this.RTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RTB.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RTB.ForeColor = System.Drawing.Color.White;
            this.RTB.Location = new System.Drawing.Point(0, 0);
            this.RTB.Multiline = true;
            this.RTB.Name = "RTB";
            this.RTB.ReadOnly = true;
            this.RTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RTB.Size = new System.Drawing.Size(566, 225);
            this.RTB.TabIndex = 0;
            // 
            // EmulatorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 225);
            this.Controls.Add(this.RTB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "EmulatorUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EmulatorUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EmulatorUI_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Timer RT;
		private System.Windows.Forms.TextBox RTB;

	}
}