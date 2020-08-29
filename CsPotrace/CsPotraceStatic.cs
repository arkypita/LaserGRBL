using CsPotrace.BezierToBiarc;
using System.Drawing;
using System.Globalization;

namespace CsPotrace
{
    public partial class Potrace
    {

		private static string formatnumber(double number, double scale)
		{
			double num = number / scale;
			if (!double.IsNaN(num))
				return num.ToString("0.###", CultureInfo.InvariantCulture);
			else
				return "0";
		}

		public static PointF AsPointF(Vector2 v)
		{
			return new PointF(v.X, v.Y);
		}

	}
}
