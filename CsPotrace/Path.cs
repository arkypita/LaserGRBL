using System.Collections.Generic;
using System.Drawing;

//Copyright (C) 2001-2016 Peter Selinger
//Copyright (C) 2009-2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
// You should have received a copy of the GNU General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. 

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
