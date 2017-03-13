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
	public partial class NewVersionForm : Form
	{
		public NewVersionForm()
		{
			InitializeComponent();
		}

		internal static void CreateAndShowDialog(Version current, Version latest, string name, string url)
		{
			using (NewVersionForm f = new NewVersionForm())
			{
				f.LblCurrentVersion.Text = current.ToString(3);
				f.LblLatestVersion.Text = latest.ToString(3);
				if (f.ShowDialog() == DialogResult.OK)
					System.Diagnostics.Process.Start(url);
			}

			
		}

	}
}
