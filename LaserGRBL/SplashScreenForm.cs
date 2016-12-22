using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class SplashScreenForm : Form
	{
		public SplashScreenForm()
		{
			InitializeComponent();
			this.DoubleBuffered = true;
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
