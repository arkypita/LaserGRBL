
//Copyright (C) 2001-2016 Peter Selinger
//Copyright (C) 2009-2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
// You should have received a copy of the GNU General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. 

namespace CsPotrace
{
    public class privcurve
	{
		public int n;                   /* number of segments */
		public int[] tag;                /* tag[n]: POTRACE_CORNER or POTRACE_CURVETO */
		//     public dPoint[,] ControlPoints;  /* c[n][i]: control points. 
		//                      c[n][0] is unused for tag[n]=POTRACE_CORNER */
		public dPoint[] vertex;          /* for POTRACE_CORNER, this equals c[1] */
		/* (*c)[3]; /* c[n][i]: control points. 
		c[n][0] is unused for tag[n]=POTRACE_CORNER */
		public dPoint[] c = null;        // HelpPoint
		public double[] alpha;           /* only for POTRACE_CURVETO */
		public double[] alpha0;          /* "uncropped" alpha parameter - for debug output only */
		public double[] beta;
		public int alphacurve = 0;
		public privcurve(int Count)
		{
			n = Count;
			tag = new int[n];
			// ControlPoints = new dPoint[n, 3];
			vertex = new dPoint[n];
			alpha = new double[n];
			alpha0 = new double[n];
			beta = new double[n];
			c = new dPoint[n * 3];
		}
	}
}
