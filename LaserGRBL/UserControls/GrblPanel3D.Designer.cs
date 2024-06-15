namespace LaserGRBL.UserControls
{
    partial class GrblPanel3D
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GrblPanel3D));
			this.TimDetectIssue = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// TimDetectIssue
			// 
			this.TimDetectIssue.Interval = 10000;
			this.TimDetectIssue.Tick += new System.EventHandler(this.TimDetectIssue_Tick);
			// 
			// GrblPanel3D
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.DoubleBuffered = true;
			this.Name = "GrblPanel3D";
			this.Size = new System.Drawing.Size(834, 460);
			this.Load += new System.EventHandler(this.GrblPanel3D_Load);
			this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.Timer TimDetectIssue;
	}
}
