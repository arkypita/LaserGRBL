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
	public partial class LicenseForm : Form
	{
		public LicenseForm()
		{
			InitializeComponent();
		}

		private void BtnContinue_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://paypal.me/pools/c/8cQ1Lo4sRA");
		}

		internal static void CreateAndShowDialog()
		{
			using (LicenseForm f = new LicenseForm())
				f.ShowDialog();
		}

		private void RTBLinkClick(object sender, LinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.LinkText);
		}
	}
}
