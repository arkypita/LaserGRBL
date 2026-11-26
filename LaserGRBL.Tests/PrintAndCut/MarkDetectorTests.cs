using System.Collections.Generic;
using LaserGRBL.PrintAndCut;
using Xunit;

namespace LaserGRBL.Tests.PrintAndCut
{
	public class MarkDetectorTests
	{
		private static Bounds Line(double x1, double y1, double x2, double y2) { return new Bounds(x1, y1, x2, y2); }

		[Fact]
		public void Detect_CrossOfTwoLines_IsOneMark()
		{
			var strokes = new List<Bounds> { Line(5, 10, 15, 10), Line(10, 5, 10, 15) };

			var marks = MarkDetector.Detect(strokes);

			Assert.Single(marks);
			Assert.Equal(2, marks[0].StrokeCount);
			Assert.Equal(10, marks[0].Center.X, 9);
			Assert.Equal(10, marks[0].Center.Y, 9);
		}

		[Fact]
		public void Detect_TwoSeparateMarks_AreNumberedInReadingOrder()
		{
			// bottom-right and top-left (Y grows upwards)
			var strokes = new List<Bounds> { new Bounds(190, 0, 200, 10), new Bounds(0, 290, 10, 300) };

			var marks = MarkDetector.Detect(strokes);

			Assert.Equal(2, marks.Count);
			Assert.Equal(1, marks[0].Number);
			Assert.Equal(5, marks[0].Center.X, 9);    // top-left first
			Assert.Equal(295, marks[0].Center.Y, 9);
			Assert.Equal(2, marks[1].Number);
			Assert.Equal(195, marks[1].Center.X, 9);
		}

		[Fact]
		public void Detect_SameRow_LeftFirst()
		{
			var strokes = new List<Bounds> { new Bounds(100, 0, 110, 10), new Bounds(0, 0, 10, 10) };

			var marks = MarkDetector.Detect(strokes);

			Assert.Equal(5, marks[0].Center.X, 9);
			Assert.Equal(105, marks[1].Center.X, 9);
		}

		[Fact]
		public void Detect_CircleWithDotInside_IsOneMark()
		{
			var strokes = new List<Bounds> { new Bounds(0, 0, 6, 6), new Bounds(2.9, 2.9, 3.1, 3.1) };

			var marks = MarkDetector.Detect(strokes);

			Assert.Single(marks);
			Assert.Equal(3, marks[0].Center.X, 9);
		}

		[Fact]
		public void Detect_StrokesWithinTolerance_AreMerged()
		{
			var strokes = new List<Bounds> { new Bounds(0, 0, 4, 4), new Bounds(4.5, 0, 8, 4) };

			Assert.Single(MarkDetector.Detect(strokes, 1.0, 20));
			Assert.Equal(2, MarkDetector.Detect(strokes, 0.2, 20).Count);
		}

		[Fact]
		public void Detect_ChainOfStrokes_IsMergedTransitively()
		{
			// A touches B, B touches C, A does not touch C
			var strokes = new List<Bounds> { new Bounds(0, 0, 2, 2), new Bounds(20, 0, 22, 2), new Bounds(2, 0, 20, 0) };

			var marks = MarkDetector.Detect(strokes, 0.1, 50);

			Assert.Single(marks);
			Assert.Equal(3, marks[0].StrokeCount);
		}

		[Fact]
		public void Detect_MergedMarksTooBig_AreFlaggedOversized()
		{
			// two crosses 5 mm apart: the tolerance merges them into a 25 mm wide mark
			var strokes = new List<Bounds> { new Bounds(0, 0, 10, 10), new Bounds(10.5, 0, 25, 10) };

			var marks = MarkDetector.Detect(strokes, 1.0, 20);

			Assert.Single(marks);
			Assert.True(marks[0].IsOversized);
		}

		[Fact]
		public void Detect_NormalMark_IsNotOversized()
		{
			var marks = MarkDetector.Detect(new List<Bounds> { new Bounds(0, 0, 5, 5) });
			Assert.False(marks[0].IsOversized);
		}

		[Fact]
		public void Detect_NoStrokes_ReturnsEmptyList()
		{
			Assert.Empty(MarkDetector.Detect(new List<Bounds>()));
		}

		[Fact]
		public void BoundsFromPoints_ComputesBox()
		{
			var b = Bounds.FromPoints(new[] { new Vec2(3, -1), new Vec2(-2, 4), new Vec2(1, 1) });

			Assert.Equal(-2, b.MinX);
			Assert.Equal(-1, b.MinY);
			Assert.Equal(3, b.MaxX);
			Assert.Equal(4, b.MaxY);
		}
	}
}
