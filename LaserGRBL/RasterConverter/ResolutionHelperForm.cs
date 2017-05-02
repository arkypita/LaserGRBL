using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.RasterConverter
{
	public partial class ResolutionHelperForm : Form
	{
		double mRetVal;

		public ResolutionHelperForm()
		{
			InitializeComponent();
		}

		public static double CreateAndShowDialog(double oldval)
		{
			double rv = oldval;

			using (ResolutionHelperForm f = new ResolutionHelperForm())
			{
				f.UDDesired.Value = (decimal)oldval;
				f.UDHardware.Value = (decimal)Settings.GetObject("Hardware Resolution", 100.0m);


				if (f.ShowDialog() == DialogResult.OK)
					rv = f.mRetVal;
			}

			return rv;
		}

		private void BtnCreate_Click(object sender, EventArgs e)
		{
			mRetVal = (double)UDComputed.Value;
			Settings.SetObject("Hardware Resolution", UDHardware.Value);
			Settings.Save();
		}

		private void Compute(object sender, EventArgs e)
		{
			//decimal newresolution = UDHardware.Value / Math.Round((UDHardware.Value / UDDesired.Value), 0);


			decimal hardwarePitch = 1.0m / UDHardware.Value;
			decimal desiredPitch = 1.0m / UDDesired.Value;

			decimal fullSteps = Math.Round(desiredPitch / hardwarePitch);

			if (Resolution(hardwarePitch, fullSteps) > UDComputed.Maximum)
				UDComputed.Value = UDComputed.Maximum; //todo... use a better computation
			else if (Resolution(hardwarePitch, fullSteps) < UDComputed.Minimum)
				UDComputed.Value = UDComputed.Minimum; //todo... use a better computation
			else
				UDComputed.Value = Resolution(hardwarePitch, fullSteps);
		}

		private decimal Resolution(decimal pitch, decimal steps)
		{
			decimal rv = 1.0m / (pitch * steps);
			return rv;
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/usage/raster-image-import/setting-reliable-resolution/");
		}
	}
}
