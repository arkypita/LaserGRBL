namespace LaserGRBL.PSHelper
{
	partial class PSHelperForm
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
			this.psHelperControl1 = new LaserGRBL.PSHelper.PSHelperControl();
			this.SuspendLayout();
			// 
			// psHelperControl1
			// 
			this.psHelperControl1.AutoSize = true;
			this.psHelperControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.psHelperControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.psHelperControl1.Location = new System.Drawing.Point(0, 0);
			this.psHelperControl1.Name = "psHelperControl1";
			this.psHelperControl1.Size = new System.Drawing.Size(377, 148);
			this.psHelperControl1.TabIndex = 0;
			// 
			// PSHelperForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(377, 148);
			this.Controls.Add(this.psHelperControl1);
			this.Name = "PSHelperForm";
			this.Text = "PSHelperForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private PSHelperControl psHelperControl1;
	}
}