using System;
using System.Collections.Generic;
using System.Globalization;
using LaserGRBL.PrintAndCut;
using Xunit;

namespace LaserGRBL.Tests.PrintAndCut
{
	public class GCodeTransformerTests
	{
		// translation by (10, 20)
		private static readonly AlignmentTransform Shift = AlignmentTransform.FromTargets(
			new Vec2(0, 0), new Vec2(100, 0), new Vec2(10, 20), new Vec2(110, 20), false);

		// +90° around the origin
		private static readonly AlignmentTransform Rot90 = AlignmentTransform.FromTargets(
			new Vec2(0, 0), new Vec2(100, 0), new Vec2(0, 0), new Vec2(0, 100), false);

		// x2 scale, no rotation, no translation
		private static readonly AlignmentTransform Double = AlignmentTransform.FromTargets(
			new Vec2(0, 0), new Vec2(50, 0), new Vec2(0, 0), new Vec2(100, 0), true);

		private static List<string> Run(AlignmentTransform t, params string[] lines)
		{
			return GCodeTransformer.Transform(lines, t).Lines;
		}

		[Fact]
		public void Transform_KeepsLineCount()
		{
			var input = new[] { "G90", "", "M3 S100", "G1 X1 Y1 F1000", "(comment)", "M5" };
			Assert.Equal(input.Length, GCodeTransformer.Transform(input, Shift).Lines.Count);
		}

		[Fact]
		public void Transform_AbsoluteMove_TransformsPoint()
		{
			var output = Run(Shift, "G90", "G1 X5 Y5 F1000 S500");
			Assert.Equal("G1 X15 Y25 F1000 S500", output[1]);
		}

		[Fact]
		public void Transform_AbsoluteMoveWithOneAxis_WritesBothAxes()
		{
			var output = Run(Rot90, "G90", "G0 X10 Y0", "G1 X20", "Y5");

			Assert.Equal("G0 X0 Y10", output[1]);
			Assert.Equal("G1 X0 Y20", output[2]);   // Y was 0 -> (20,0) -> (0,20)
			Assert.Equal("X-5 Y20", output[3]);      // modal G1, X was 20 -> (20,5) -> (-5,20)
		}

		[Fact]
		public void Transform_RelativeMove_RotatesWithoutTranslating()
		{
			var output = Run(Shift, "G91", "G1 X5 Y0");
			Assert.Equal("G1 X5 Y0", output[1]);

			output = Run(Rot90, "G91", "G1 X5");
			Assert.Equal("G1 X0 Y5", output[1]);
		}

		[Fact]
		public void Transform_RelativeThenAbsolute_TracksPosition()
		{
			// after G91 moves the position is (3,4); the absolute X-only move keeps Y=4
			var output = Run(Shift, "G90", "G0 X1 Y1", "G91", "G1 X2 Y3", "G90", "G1 X10");
			Assert.Equal("G1 X20 Y24", output[5]);
		}

		[Fact]
		public void Transform_ArcIJ_RotatesOffsetsAndKeepsDirection()
		{
			// quarter circle CCW from (10,0) to (0,10) around the origin
			var output = Run(Rot90, "G90", "G0 X10 Y0", "G3 X0 Y10 I-10 J0");
			Assert.Equal("G3 X-10 Y0 I0 J-10", output[2]);
		}

		[Fact]
		public void Transform_ArcIJ_NotTranslated()
		{
			var output = Run(Shift, "G90", "G0 X10 Y0", "G2 X0 Y-10 I-10 J0");
			Assert.Equal("G2 X10 Y10 I-10 J0", output[2]);
		}

		[Fact]
		public void Transform_FullCircleWithoutEndPoint_TransformsOnlyIJ()
		{
			var output = Run(Rot90, "G90", "G0 X10 Y0", "G3 I-10 J0");
			Assert.Equal("G3 I0 J-10", output[2]);
		}

		[Fact]
		public void Transform_ArcR_IsScaled()
		{
			var output = Run(Double, "G90", "G0 X0 Y0", "G2 X10 Y0 R5");
			Assert.Equal("G2 X20 Y0 R10", output[2]);
		}

		[Fact]
		public void Transform_ArcCenterIsTransformedConsistently()
		{
			// arc center = start + IJ must be the transformed center
			var t = AlignmentTransform.FromTargets(new Vec2(3, 4), new Vec2(80, 60), new Vec2(20, 15), new Vec2(70, 110), true);
			var output = Run(t, "G90", "G0 X10 Y5", "G2 X30 Y5 I10 J-3");

			var start = ParseXY(output[1]);
			var end = ParseXY(output[2]);
			var ij = ParseIJ(output[2]);
			var expectedCenter = t.TransformPoint(new Vec2(20, 2));

			Assert.Equal(expectedCenter.X, start.X + ij.X, 2);
			Assert.Equal(expectedCenter.Y, start.Y + ij.Y, 2);
			// same radius from both ends (within the 3 decimal rounding)
			Assert.Equal(Vec2.Distance(expectedCenter, start), Vec2.Distance(expectedCenter, end), 2);
		}

		[Fact]
		public void Transform_KeepsCommentsAndOtherWords()
		{
			var output = Run(Shift, "G90", "G1 X1 Y2 Z-1 F600 S1000 (cut) ; end");
			Assert.Equal("G1 X11 Y22 Z-1 F600 S1000 (cut) ; end", output[1]);
		}

		[Fact]
		public void Transform_CompactLineWithoutSpaces()
		{
			var output = Run(Shift, "G90", "G1X1Y2F600");
			Assert.Equal("G1X11 Y22F600", output[1]);
		}

		[Fact]
		public void Transform_LinesWithoutXY_AreUntouched()
		{
			var input = new[] { "G90 G21", "M3 S0", "G1 F1000", "G4 P0.5", "M5", "$H", "%", "(X10 Y10 in a comment)" };
			var output = GCodeTransformer.Transform(input, Shift).Lines;
			Assert.Equal(input, output);
		}

		[Fact]
		public void Transform_RasterStyleLines_StayConsistent()
		{
			// typical LaserGRBL raster: horizontal lines with only X and S changing
			var output = Run(Rot90, "G90", "G0 X0 Y0", "G1 X1 S100", "X2 S0", "X3 S255", "G0 Y0.1");
			Assert.Equal("G1 X0 Y1 S100", output[2]);
			Assert.Equal("X0 Y2 S0", output[3]);
			Assert.Equal("X0 Y3 S255", output[4]);
			Assert.Equal("G0 X-0.1 Y3", output[5]);
		}

		[Fact]
		public void Transform_NeverWritesNegativeZero()
		{
			var output = Run(Rot90, "G90", "G1 X10 Y0");
			Assert.DoesNotContain("-0 ", output[1] + " ");
		}

		[Fact]
		public void Transform_Inches_IsBlocking()
		{
			var result = GCodeTransformer.Transform(new[] { "G20", "G1 X1 Y1" }, Shift);

			Assert.True(result.HasBlockingIssues);
			Assert.Equal(GCodeTransformIssue.InchUnits, result.Warnings[0].Issue);
			Assert.Equal(1, result.Warnings[0].LineIndex);
		}

		[Theory]
		[InlineData("G53 G0 X0 Y0", GCodeTransformIssue.MachineCoordinates)]
		[InlineData("G92 X0 Y0", GCodeTransformIssue.CoordinateOffset)]
		[InlineData("G10 L20 P1 X0 Y0", GCodeTransformIssue.CoordinateOffset)]
		[InlineData("G28 X0 Y0", GCodeTransformIssue.ReturnToStoredPosition)]
		[InlineData("G38.2 X10 F100", GCodeTransformIssue.Probe)]
		public void Transform_SpecialMoves_AreUntouchedAndReported(string line, GCodeTransformIssue issue)
		{
			var result = GCodeTransformer.Transform(new[] { "G90", line }, Shift);

			Assert.Equal(line, result.Lines[1]);
			Assert.Contains(result.Warnings, w => w.Issue == issue);
			Assert.False(result.HasBlockingIssues);
		}

		[Fact]
		public void Transform_MissingAxisAfterG53_ReportsUnknownPosition()
		{
			var result = GCodeTransformer.Transform(new[] { "G90", "G53 G0 X0 Y0", "G1 X10" }, Shift);
			Assert.Contains(result.Warnings, w => w.Issue == GCodeTransformIssue.UnknownPosition);
		}

		[Fact]
		public void Transform_WarningsAreReportedOnce()
		{
			var result = GCodeTransformer.Transform(new[] { "G92 X0 Y0", "G92 X1 Y1" }, Shift);
			Assert.Single(result.Warnings);
		}

		[Fact]
		public void Transform_LowerCaseInput()
		{
			var output = Run(Shift, "g90", "g1 x1 y2");
			Assert.Equal("g1 X11 Y22", output[1]);
		}

		private static Vec2 ParseXY(string line) { return new Vec2(Word(line, 'X'), Word(line, 'Y')); }
		private static Vec2 ParseIJ(string line) { return new Vec2(Word(line, 'I'), Word(line, 'J')); }

		private static double Word(string line, char letter)
		{
			foreach (string part in line.Split(' '))
				if (part.Length > 1 && part[0] == letter)
					return double.Parse(part.Substring(1), CultureInfo.InvariantCulture);
			throw new InvalidOperationException(letter + " not found in " + line);
		}
	}
}
