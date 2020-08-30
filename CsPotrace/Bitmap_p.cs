using System;
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
        public int w { get; }
        public int h { get; }

        private byte[] _data;
        public byte[] data
        {
            get => _data;
            set
            {
                if (data.Length != value.Length)
                    throw new InvalidOperationException("Lenght mismatch");
                _data = value;
            }
        }

        public Bitmap_p(int w, int h)
        {
            this.w = w;
            this.h = h;
            _data = new byte[size];
        }
        public int size
        {
            get { return w * h; }
        }

        public bool at(int x, int y)
        {
            // check bounds
            if (x < 0 || y < 0) return false;
            if (x >= w || y >= h) return false;
            // check content
            return data[index(x, y)] == 1;
        }
        public Point index(int i)
        {
            int y = i / w;
            return new Point(i - y * w, y);
        }
        public int index(int x, int y)
        {
            return w * y + x;
        }
        public void flip(int x, int y)
        {
            if (at(x, y))
            {
                data[index(x, y)] = 0;
            }
            else
            {
                data[index(x, y)] = 1;
            }
        }
        public Bitmap_p copy()
        {
            Bitmap_p Result = new Bitmap_p(w, h);
            Buffer.BlockCopy(_data, 0, Result._data, 0, _data.Length);
            return Result;
        }
    }
}
