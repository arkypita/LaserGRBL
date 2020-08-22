using System;

namespace LaserGRBL
{
    public abstract class ColorSegment
	{
		public int mColor { get; set; }
		protected int mPixLen;

		public ColorSegment(int col, int len, bool rev)
		{
			mColor = col;
			mPixLen = rev ? -len : len;
		}

		public virtual bool IsSeparator
		{ get { return false; } }

		public bool Fast(L2LConf c)
		{ return c.pwm ? mColor == 0 : mColor <= 125; }

		public string formatnumber(int number, float offset, L2LConf c)
		{
			double dval = Math.Round(number / (c.vectorfilling ? c.fres : c.res) + offset, 3);
			return dval.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}

		// Format laser power value
		// grbl                    with pwm : color can be between 0 and configured SMax - S128
		// smoothiware             with pwm : Value between 0.00 and 1.00    - S0.50
		// Marlin : Laser power can not be defined as switch (Add in comment hard coded changes)
		public string FormatLaserPower(int color, L2LConf c)
		{
			if (c.firmwareType == Firmware.Smoothie)
				return string.Format(System.Globalization.CultureInfo.InvariantCulture, "S{0:0.00}", color / 255.0); //maybe scaling to UI maxpower VS config maxpower instead of fixed / 255.0 ?
																													 //else if (c.firmwareType == Firmware.Marlin)
																													 //	return "";
			else
				return string.Format(System.Globalization.CultureInfo.InvariantCulture, "S{0}", color);
		}

		public abstract string ToGCodeNumber(ref int cumX, ref int cumY, L2LConf c);
	}
}
