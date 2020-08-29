using System.Drawing;

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
