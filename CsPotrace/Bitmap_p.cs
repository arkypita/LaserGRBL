using System.Drawing;

//Copyright (C) 2001-2016 Peter Selinger
//Copyright (C) 2009-2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
// You should have received a copy of the GNU General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. 

namespace CsPotrace
{
    //Holds the binaray bitmap
    public class Bitmap_p
	{
		public Bitmap_p(int w, int h)
		{
			this.w = w;
			this.h = h;
			data = new byte[size];
		}
		public int w = 0;
		public int h = 0;
		public int size
		{
			get { return w * h; }
		}
		public bool at(int x, int y)
		{
			return (x >= 0)
					&& (x < this.w)
					&& (y >= 0)
					&& (y < this.h)
					&& (this.data[this.w * y + x] == 1);
		}
		public byte[] data = null;
		public Point index(int i)
		{
			int y = i / w;
			return new Point(i - y * w, y);
		}
		public void flip(int x, int y)
		{
			if (this.at(x, y))
			{
				this.data[this.w * y + x] = 0;
			}
			else
			{
				this.data[this.w * y + x] = 1;
			}
		}
		public Bitmap_p copy()
		{
			Bitmap_p Result = new Bitmap_p(w, h);
			for (int i = 0; i < size; i++)
			{
				Result.data[i] = data[i];

			}
			return Result;
		}
	}
}
