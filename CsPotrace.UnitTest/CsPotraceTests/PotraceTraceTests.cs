using System.Drawing;
using System.Linq;
using Xunit;

namespace CsPotrace.UnitTest.CsPotraceTests
{
    // A lot of this tests were implemented long after the library was in-use,
    // My main first goal is to asssure that what was woking will continue working the SAME way
    // I any bug appears, it will be frozen broken in this tests and later fixed

    public class PotraceTraceTests
    {
        [Fact]
        public void CsPotrace_PotraceTrace_LoadBitmap_CurveCount()
        {
            using var bmp = getSquare();
            Potrace po = new Potrace();
            var result = po.PotraceTrace(bmp);
            Assert.Single(result); // External List
            Assert.Equal(4, result[0].Count);
        }
        [Fact]
        public void CsPotrace_PotraceTrace_LoadBitmap_CurveCountInverted()
        {
            using var bmp = getSquareInverted();
            Potrace po = new Potrace();
            var result = po.PotraceTrace(bmp);

            var points = result[0].SelectMany(p => new dPoint[] { p.A, p.B }).ToArray();
            Assert.Equal(16, points.Length);
        }
        [Fact]
        public void CsPotrace_PotraceTrace_LoadBitmap_CurveCountCircle()
        {
            using var bmp = getCircle();
            Potrace po = new Potrace();
            var result = po.PotraceTrace(bmp);

            var points = result[0].SelectMany(p => new dPoint[] { p.A, p.B }).ToArray();
            Assert.Equal(8, points.Length);
        }

        [Fact]
        public void CsPotrace_PotraceTrace_LoadBitmap_Points()
        {
            using var bmp = getSquare();
            Potrace po = new Potrace();
            var result = po.PotraceTrace(bmp);

            var points = result[0].SelectMany(p => new dPoint[] { p.A, p.B }).ToArray();
            Assert.Equal(8, points.Length);

            Assert.Equal(2, points[0].X);
            Assert.Equal(5, points[0].Y);
            Assert.Equal(5, points[1].X);
            Assert.Equal(8, points[1].Y);
            Assert.Equal(5, points[2].X);
            Assert.Equal(8, points[2].Y);
            Assert.Equal(8, points[3].X);
            Assert.Equal(5, points[3].Y);
            Assert.Equal(8, points[4].X);
            Assert.Equal(5, points[4].Y);
            Assert.Equal(5, points[5].X);
            Assert.Equal(2, points[5].Y);
            Assert.Equal(5, points[6].X);
            Assert.Equal(2, points[6].Y);
            Assert.Equal(2, points[7].X);
            Assert.Equal(5, points[7].Y);
        }



        internal static Bitmap getCircle()
        {
            var bmp = new Bitmap(10, 10);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.FillEllipse(Brushes.Black, new Rectangle(2, 2, 6, 6));
            return bmp;
        }
        internal static Bitmap getSquare()
        {
            var bmp = new Bitmap(10, 10);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.FillRectangle(Brushes.Black, new Rectangle(2, 2, 6, 6));
            return bmp;
        }
        internal static Bitmap getSquareInverted()
        {
            var bmp = new Bitmap(10, 10);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.FillRectangle(Brushes.White, new Rectangle(2, 2, 6, 6));
            return bmp;
        }
    }
}
