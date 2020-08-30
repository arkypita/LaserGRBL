
//Copyright (C) 2001-2016 Peter Selinger
//Copyright (C) 2009-2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
// You should have received a copy of the GNU General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. 

namespace CsPotrace
{
    public struct Sum
	{
		public Sum(double x, double y, double xy, double x2, double y2)
		{
			this.x = x;
			this.y = y;
			this.xy = xy;
			this.x2 = x2;
			this.y2 = y2;
		}
		public double x, y, xy, x2, y2;
	}
}
