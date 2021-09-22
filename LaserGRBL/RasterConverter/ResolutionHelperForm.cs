//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

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
			UDDesired.Maximum = UDComputed.Maximum = GetMaxQuality();
			BackColor = ColorScheme.FormBackColor;
			ForeColor = ColorScheme.FormForeColor;
			BtnCancel.BackColor = BtnCreate.BackColor = ColorScheme.FormButtonsColor;
		}

		private decimal GetMaxQuality()
		{
			return Settings.GetObject("Raster Hi-Res", false) ? 50 : 20;
		}

		public static double CreateAndShowDialog(Form parent, GrblCore Core, double oldval)
		{
			double rv = oldval;

			using (ResolutionHelperForm f = new ResolutionHelperForm())
			{
				f.UDDesired.Value = (decimal)oldval;
				f.UDHardware.Value = Core.Configuration.ResolutionX;
				f.Compute(null, null);

				if (f.ShowDialog(parent) == DialogResult.OK)
					rv = f.mRetVal;
			}

			return rv;
		}

		private void BtnCreate_Click(object sender, EventArgs e)
		{
			mRetVal = (double)UDComputed.Value;
			Settings.SetObject("Hardware Resolution", UDHardware.Value);
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
		{Tools.Utils.OpenLink(@"https://lasergrbl.com/usage/raster-image-import/setting-reliable-resolution/");}
	}
}
