/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 05/12/2016
 * Time: 23:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace LaserGRBL
{
	partial class PreviewForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private LaserGRBL.UserControls.GrblPanel Preview;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.Preview = new LaserGRBL.UserControls.GrblPanel();
			this.SuspendLayout();
			// 
			// Preview
			// 
			this.Preview.BackColor = System.Drawing.SystemColors.Info;
			this.Preview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Preview.Location = new System.Drawing.Point(0, 0);
			this.Preview.Name = "Preview";
			this.Preview.Size = new System.Drawing.Size(409, 363);
			this.Preview.TabIndex = 0;
			// 
			// PreviewForm
			// 
			this.ClientSize = new System.Drawing.Size(409, 363);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.Preview);
			this.DockAreas = LaserGRBL.UserControls.DockingManager.DockAreas.Document;
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "PreviewForm";
			this.Text = "Preview";
			this.ToolTipText = "Preview";
			this.ResumeLayout(false);

		}
	}
}
