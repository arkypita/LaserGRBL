using System.Collections.Generic;
using System.Drawing;

namespace CsPotrace
{
    public class Path
	{
		public int m = 0;
		public int area = 0;
		public int len = 0;
		public string sign = "?";
		// curve 
		public List<Point> pt = new List<Point>();
		public int minX = 100000;
		public int minY = 100000;
		public int maxX = -1;
		public int maxY = -1;
		public double x0;
		public double y0;
		public int[] po;
		public int[] lon = null;
		public List<Sum> sums = new List<Sum>();
		public privcurve curve = null;

	}
}
