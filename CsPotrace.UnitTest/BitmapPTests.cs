using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Schema;
using Xunit;

namespace CsPotrace.UnitTest
{
    public class BitmapPTests
    {
        [Fact]
        public void CsPotrace_BitmapP_Ctor()
        {
            Bitmap_p b = new Bitmap_p(9, 11);
            Assert.Equal(9, b.w);
            Assert.Equal(11, b.h);
            Assert.Equal(99, b.size);
            Assert.Equal(99, b.data.Length);
        }

        [Fact]
        public void CsPotrace_BitmapP_atOutBounds()
        {
            Bitmap_p b = new Bitmap_p(9, 11);
            for (int i = 0; i < b.data.Length; i++) b.data[i] = 1;

            Assert.False(b.at(-1, 0));
            Assert.False(b.at(0, -1));

            Assert.False(b.at(9, 0));
            Assert.False(b.at(0, 11));
        }
        [Fact]
        public void CsPotrace_BitmapP_atInBounds()
        {
            Bitmap_p b = new Bitmap_p(9, 11);
            for (int i = 0; i < b.data.Length; i++) b.data[i] = 1;

            Assert.True(b.at(0, 0));
            Assert.True(b.at(8, 10));
        }
        [Fact]
        public void CsPotrace_BitmapP_indexPoint()
        {
            Bitmap_p b = new Bitmap_p(9, 11);

            Assert.Equal(new Point(0, 0), b.index(0)); // zero
            Assert.Equal(new Point(1, 0), b.index(1)); // first
            Assert.Equal(new Point(8, 0), b.index(8)); // last of first row
            Assert.Equal(new Point(0, 1), b.index(9));

            // Last rows
            Assert.Equal(new Point(8, 9), b.index(89));
            Assert.Equal(new Point(0, 10), b.index(90));
            Assert.Equal(new Point(8, 10), b.index(98));
        }
        [Fact]
        public void CsPotrace_BitmapP_indexInt()
        {
            Bitmap_p b = new Bitmap_p(9, 11);

            Assert.Equal(0, b.index(0, 0)); // zero
            Assert.Equal(1, b.index(1, 0)); // first
            Assert.Equal(8, b.index(8, 0)); // last of first row
            Assert.Equal(9, b.index(0, 1));

            // Last rows
            Assert.Equal(89, b.index(8, 9));
            Assert.Equal(90, b.index(0, 10));
            Assert.Equal(98, b.index(8, 10));
        }
        [Fact]
        public void CsPotrace_BitmapP_flip()
        {
            Bitmap_p b = new Bitmap_p(2, 2);
            b.data = new byte[] { 0, 1, 1, 0 };
            b.flip(0, 0);
            Assert.Equal(new byte[] { 1, 1, 1, 0 }, b.data);
            b.flip(1, 0);
            Assert.Equal(new byte[] { 1, 0, 1, 0 }, b.data);
        }
        [Fact]
        public void CsPotrace_BitmapP_flipAll()
        {
            Bitmap_p b = new Bitmap_p(3, 3);
            b.data = new byte[9] { 0, 1, 0,
                                   1, 0, 1,
                                   0, 1, 0 };

            var inverted = new byte[9] { 1, 0, 1,
                                         0, 1, 0,
                                         1, 0, 1 };
            // Flip all
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    b.flip(x, y);

            Assert.Equal(inverted, b.data);
        }

        [Fact]
        public void CsPotrace_BitmapP_copy()
        {
            Bitmap_p b = new Bitmap_p(9, 11);
            for (int i = 0; i < b.data.Length; i++) b.data[i] = (byte)i;

            var bCopy = b.copy();

            Assert.Equal(b.w, bCopy.w);
            Assert.Equal(b.h, bCopy.h);

            Assert.Equal(b.data, bCopy.data);
        }
    }
}