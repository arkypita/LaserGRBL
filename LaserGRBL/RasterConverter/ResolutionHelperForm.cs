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

			BackColor = ColorScheme.FormBackColor;
			ForeColor = ColorScheme.FormForeColor;
			BtnCancel.BackColor = BtnCreate.BackColor = ColorScheme.FormButtonsColor;
		}

		public static double CreateAndShowDialog(GrblCore Core, double oldval)
		{
			double rv = oldval;

			using (ResolutionHelperForm f = new ResolutionHelperForm())
			{
				f.UDDesired.Value = (decimal)oldval;
				f.UDHardware.Value = Core.Configuration.ResolutionX;
				f.Compute(null, null);

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
			try
			{
				decimal newRes = UDHardware.Value / Math.Round((UDHardware.Value / UDDesired.Value), 0);

				if (newRes > UDComputed.Maximum)
					UDComputed.Value = MaxRes();
				else if (newRes < UDComputed.Minimum)
					UDComputed.Value = MinRes();
				else
					UDComputed.Value = newRes;
			}
			catch (Exception ex)
			{
				Logger.LogMessage("ResolutionHelper", "Ex with data [{0}]HW [{1}]DV", UDHardware.Value, UDDesired.Value);
				Logger.LogException("ResolutionHelper", ex); 
			}
		}

		private decimal MaxRes()
		{
			decimal maxRes = UDHardware.Value / Math.Ceiling(UDHardware.Value / UDComputed.Maximum);
			return Math.Min(UDComputed.Maximum, maxRes);
		}

		private decimal MinRes()
		{
			decimal minRes = UDHardware.Value / Math.Floor(UDHardware.Value / UDComputed.Minimum);
			return Math.Max(UDComputed.Minimum, minRes);
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/usage/raster-image-import/setting-reliable-resolution/");
		}
	}
}
