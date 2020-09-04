//Copyright (C) 2001-2016 Peter Selinger
//Copyright (C) 2009-2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
// You should have received a copy of the GNU General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. 

namespace CsPotrace
{
    /// <summary>
    /// Holds the coordinates of a Point
    /// </summary>
    public class dPoint
	{
		/// <summary>
		/// x-coordinate
		/// </summary>
		public double X;
		/// <summary>
		/// y-coordinate
		/// </summary>
		public double Y;
		/// <summary>
		/// Creates a point
		/// </summary>
		/// <param name="x">x-coordinate</param>
		/// <param name="y">y-coordinate</param>
		public dPoint(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}
		public dPoint copy()
		{
			return new dPoint(X, Y);
		}
		public dPoint()
		{ }
	}

}