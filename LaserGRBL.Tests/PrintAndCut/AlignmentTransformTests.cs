using System;
using LaserGRBL.PrintAndCut;
using Xunit;

namespace LaserGRBL.Tests.PrintAndCut
{
	public class AlignmentTransformTests
	{
		private const int Precision = 9;

		private static void AssertPoint(double x, double y, Vec2 actual)
		{
			Assert.Equal(x, actual.X, Precision);
			Assert.Equal(y, actual.Y, Precision);
		}

		[Fact]
		public void FromTargets_SameTargets_IsIdentity()
		{
			var t = AlignmentTransform.FromTargets(new Vec2(10, 10), new Vec2(110, 10), new Vec2(10, 10), new Vec2(110, 10), true);

			Assert.Equal(0, t.Angle, Precision);
			Assert.Equal(1, t.Scale, Precision);
			AssertPoint(37.5, -4.25, t.TransformPoint(new Vec2(37.5, -4.25)));
		}

		[Fact]
		public void FromTargets_PureTranslation_MovesEveryPoint()
		{
			var t = AlignmentTransform.FromTargets(new Vec2(0, 0), new Vec2(100, 0), new Vec2(5, 7), new Vec2(105, 7), false);

			AssertPoint(5, 7, t.TransformPoint(new Vec2(0, 0)));
			AssertPoint(55, 57, t.TransformPoint(new Vec2(50, 50)));
			AssertPoint(50, 50, t.TransformVector(new Vec2(50, 50))); // vectors are not translated
		}

		[Fact]
		public void FromTargets_Rotation90_RotatesCounterClockwise()
		{
			// design: D1=(0,0) D2=(100,0); machine: M1=(10,10) M2=(10,110) -> +90°
			var t = AlignmentTransform.FromTargets(new Vec2(0, 0), new Vec2(100, 0), new Vec2(10, 10), new Vec2(10, 110), false);

			Assert.Equal(90, t.AngleDegrees, Precision);
			AssertPoint(10, 110, t.TransformPoint(new Vec2(100, 0)));
			AssertPoint(0, 10, t.TransformPoint(new Vec2(0, 10)));
			AssertPoint(-10, 0, t.TransformVector(new Vec2(0, 10)));
		}

		[Fact]
		public void FromTargets_TargetsMapExactly_WithScaling()
		{
			var d1 = new Vec2(12.3, 45.6);
			var d2 = new Vec2(187.1, 210.4);
			var m1 = new Vec2(40.2, 30.9);
			var m2 = new Vec2(180.7, 260.1);

			var t = AlignmentTransform.FromTargets(d1, d2, m1, m2, true);

			AssertPoint(m1.X, m1.Y, t.TransformPoint(d1));
			AssertPoint(m2.X, m2.Y, t.TransformPoint(d2));
			Assert.Equal(0, t.SecondTargetError, Precision);
		}

		[Fact]
		public void FromTargets_NoScaling_KeepsDistancesAndReportsResidual()
		{
			// machine distance is 101 mm instead of 100 mm
			var t = AlignmentTransform.FromTargets(new Vec2(0, 0), new Vec2(100, 0), new Vec2(0, 0), new Vec2(101, 0), false);

			Assert.Equal(1.0, t.Scale, Precision);
			Assert.Equal(1.01, t.MeasuredScale, Precision);
			AssertPoint(100, 0, t.TransformPoint(new Vec2(100, 0)));
			Assert.Equal(1.0, t.SecondTargetError, Precision);
		}

		[Fact]
		public void FromTargets_Scaling_AppliesMeasuredScale()
		{
			var t = AlignmentTransform.FromTargets(new Vec2(0, 0), new Vec2(100, 0), new Vec2(0, 0), new Vec2(102, 0), true);

			Assert.Equal(1.02, t.Scale, Precision);
			AssertPoint(51, 51, t.TransformPoint(new Vec2(50, 50)));
		}

		[Theory]
		[InlineData(1.00, false)]
		[InlineData(1.02, false)]
		[InlineData(0.98, false)]
		[InlineData(1.05, true)]
		[InlineData(0.90, true)]
		public void IsScaleSuspicious_DependsOnMeasuredScale(double scale, bool suspicious)
		{
			var t = AlignmentTransform.FromTargets(new Vec2(0, 0), new Vec2(100, 0), new Vec2(0, 0), new Vec2(100 * scale, 0), false);

			Assert.Equal(suspicious, t.IsScaleSuspicious);
		}

		[Fact]
		public void FromTargets_AngleIsNormalized()
		{
			// design vector points at +170°, machine vector at -170°: the rotation is +20°, not -340°
			double a1 = 170 * Math.PI / 180, a2 = -170 * Math.PI / 180;
			var t = AlignmentTransform.FromTargets(new Vec2(0, 0), new Vec2(100 * Math.Cos(a1), 100 * Math.Sin(a1)),
				new Vec2(0, 0), new Vec2(100 * Math.Cos(a2), 100 * Math.Sin(a2)), false);

			Assert.Equal(20, t.AngleDegrees, 6);
		}

		[Fact]
		public void FromTargets_DesignTargetsTooClose_Throws()
		{
			Assert.Throws<ArgumentException>(() =>
				AlignmentTransform.FromTargets(new Vec2(0, 0), new Vec2(0.5, 0), new Vec2(0, 0), new Vec2(100, 0), true));
		}

		[Fact]
		public void FromTargets_MachineTargetsTooClose_Throws()
		{
			Assert.Throws<ArgumentException>(() =>
				AlignmentTransform.FromTargets(new Vec2(0, 0), new Vec2(100, 0), new Vec2(3, 3), new Vec2(3, 3), true));
		}
	}
}
