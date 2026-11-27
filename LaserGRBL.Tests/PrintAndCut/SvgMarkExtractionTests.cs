using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using LaserGRBL.PrintAndCut;
using LaserGRBL.SvgConverter;
using Xunit;

namespace LaserGRBL.Tests.PrintAndCut
{
	/// <summary>
	/// Runs the real SVG converter (needs WPF, Windows only).
	/// The SVG uses 1 user unit = 1 mm and is 100 mm high, so job Y = 100 - svg Y.
	/// </summary>
	[Collection(SvgConverterCollection.Name)]
	public class SvgMarkExtractionTests
	{
		private const string Svg = @"<svg xmlns='http://www.w3.org/2000/svg' width='200mm' height='100mm' viewBox='0 0 200 100'>
  <rect x='50' y='30' width='100' height='40' style='stroke:#000000;fill:none'/>
  <path d='M 5 10 L 15 10 M 10 5 L 10 15' style='fill:none;stroke:#ff00ff;stroke-width:0.3'/>
  <g style='stroke:#FF00FF'><circle cx='190' cy='90' r='3' fill='none'/></g>
  <circle cx='100' cy='50' r='2' fill='#ff00ff' stroke='none'/>
</svg>";

		private static readonly Color Magenta = Color.FromArgb(255, 0, 255);

		public SvgMarkExtractionTests()
		{
			// the converter reads Settings: keep it away from the user's %AppData%\LaserGRBL
			TestAppData.Isolate();
		}

		private static GCodeFromSVG Convert(Color reserved, ColorFilter filter, out string gcode)
		{
			var converter = new GCodeFromSVG { ReservedColor = reserved };
			gcode = converter.convertFromText(Svg, null, true, filter);
			return converter;
		}

		private static double[] Values(string gcode, char letter)
		{
			return Regex.Matches(gcode, letter + @"(-?[0-9.]+)").Cast<Match>()
				.Select(m => double.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture)).ToArray();
		}

		[Fact]
		public void Convert_ReservedColor_IsExcludedFromGCode()
		{
			string gcode;
			Convert(Magenta, ColorFilter.All, out gcode);

			// only the black rectangle (X 50..150, Y 30..70) must be cut
			Assert.NotEmpty(Values(gcode, 'X'));
			Assert.All(Values(gcode, 'X'), x => Assert.InRange(x, 50 - 0.01, 150 + 0.01));
			Assert.All(Values(gcode, 'Y'), y => Assert.InRange(y, 30 - 0.01, 70 + 0.01));
		}

		[Fact]
		public void Convert_ReservedColor_CollectsOneStrokePerElement()
		{
			string gcode;
			var converter = Convert(Magenta, ColorFilter.All, out gcode);

			Assert.Equal(3, converter.MarkStrokes.Count);
		}

		[Fact]
		public void Convert_Marks_HaveExpectedCenters()
		{
			string gcode;
			var converter = Convert(Magenta, ColorFilter.All, out gcode);
			var marks = MarkDetector.Detect(converter.MarkStrokes);

			Assert.Equal(3, marks.Count);
			// reading order: top first (job Y grows upwards)
			AssertCenter(10, 90, marks[0]);   // cross made of two lines in one path
			AssertCenter(100, 50, marks[1]);  // filled dot
			AssertCenter(190, 10, marks[2]);  // circle inheriting the stroke from its group
			Assert.Equal(6, marks[2].Bounds.Width, 2);
		}

		[Fact]
		public void Convert_MarksAreDetectedEvenWithColorFilter()
		{
			string gcode;
			var converter = Convert(Magenta, ColorFilter.Black, out gcode);

			Assert.Equal(3, converter.MarkStrokes.Count);
			Assert.All(Values(gcode, 'X'), x => Assert.InRange(x, 50 - 0.01, 150 + 0.01));
		}

		[Fact]
		public void Convert_NoReservedColor_CutsEverything()
		{
			string gcode;
			var converter = Convert(Color.Empty, ColorFilter.All, out gcode);

			Assert.Empty(converter.MarkStrokes);
			Assert.Contains(Values(gcode, 'X'), x => x > 180);
		}

		private static void AssertCenter(double x, double y, RegistrationMark mark)
		{
			Assert.Equal(x, mark.Center.X, 2);
			Assert.Equal(y, mark.Center.Y, 2);
		}
	}
}
