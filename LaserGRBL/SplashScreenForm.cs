//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class SplashScreenForm : Form
	{
		private string mVersion;

		public SplashScreenForm()
		{
			InitializeComponent();
			this.Size = BackgroundImage.Size;
			this.DoubleBuffered = true;
			mVersion = "v" + Program.CurrentVersion.ToString(3);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			SizeF s = e.Graphics.MeasureString(mVersion, Font);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawString(mVersion, Font, Brushes.Black,  Width - s.Width -3 , Height - s.Height - 3);
			//e.Graphics.DrawRectangle(Pens.DarkGray, 2, 2, Width - 5, Height - 5);
		}

		private void SplashScreenForm_Load(object sender, EventArgs e)
		{
			timer1.Start();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			CloseAndGoMain();
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			CloseAndGoMain();
		}
		
		private void CloseAndGoMain()
		{
			timer1.Enabled = false;
			Close();
			
		}
	}
}
