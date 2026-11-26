using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using LaserGRBL.PrintAndCut;
using LaserGRBL.SvgConverter;
using Xunit;

namespace LaserGRBL.Tests.PrintAndCut
{
	/// <summary>
	/// End to end, without UI: SVG with registration marks imported with <see cref="GrblFile.LoadImportedSVG"/>,
	/// G-code appended with <see cref="GrblFile.LoadFile"/>, targets measured on a rotated and shifted sheet,
	/// and the lines that <see cref="AlignedJobCache"/> gives to the sender (needs WPF, Windows only).
	/// </summary>
	[Collection(SvgConverterCollection.Name)]
	public class PrintAndCutJobTests : IDisposable
	{
		// 1 user unit = 1 mm, 100 mm high: job Y = 100 - svg Y.
		// Cut: rectangle X 50..150, Y 30..70. Marks (crosses): job (10, 90) and (190, 10).
		private const string Svg = @"<svg xmlns='http://www.w3.org/2000/svg' width='200mm' height='100mm' viewBox='0 0 200 100'>
  <rect x='50' y='30' width='100' height='40' style='stroke:#000000;fill:none'/>
  <path d='M 7 10 L 13 10 M 10 7 L 10 13' style='fill:none;stroke:#FF00FF;stroke-width:0.3'/>
  <path d='M 187 90 L 193 90 M 190 87 L 190 93' style='fill:none;stroke:#FF00FF;stroke-width:0.3'/>
</svg>";

		// appended job (as an engraving would be): a segment inside the rectangle
		private const string Appended = "G90\nM3 S0\nG0 X100 Y50\nG1 X120 Y50 F1000 S500\nX120 Y60\nM5 S0\n";

		private const double Tolerance = 0.002; // the rewriter writes 3 decimals

		private static readonly Vec2 Wco = new Vec2(-310.5, -205.25);   // MPos = WPos + WCO
		private static readonly Vec2 Shift = new Vec2(37.5, 12.25);     // sheet placement on the bed (work coordinates)
		private const double SheetAngle = 30;                           // degrees, counterclockwise

		private readonly string mFolder;

		public PrintAndCutJobTests()
		{
			TestAppData.Isolate();
			mFolder = Path.Combine(Path.GetTempPath(), "LaserGRBL.Tests", "job-" + Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(mFolder);
		}

		public void Dispose()
		{
			try { Directory.Delete(mFolder, true); }
			catch (IOException) { }
			catch (UnauthorizedAccessException) { }
		}

		[Fact]
		public void Import_FindsBothMarks_AndKeepsThemWhenGCodeIsAppended()
		{
			GrblFile file = LoadJob();

			Assert.Equal(2, file.RegistrationMarks.Count);
			AssertNear(10, 90, file.RegistrationMarks[0].Center);
			AssertNear(190, 10, file.RegistrationMarks[1].Center);
			// the appended job is part of the file
			Assert.Contains(file.GetLines(), l => l.StartsWith("G1 X120 Y50", StringComparison.Ordinal));
		}

		[Fact]
		public void Import_MarksAreNotCut()
		{
			GrblFile file = LoadJob();

			foreach (Vec2 p in Positions(file.GetLines()).Where(p => p.HasValue).Select(p => p.Value))
			{
				Assert.InRange(p.X, 50 - Tolerance, 150 + Tolerance);
				Assert.InRange(p.Y, 30 - Tolerance, 70 + Tolerance);
			}
		}

		// a drawing that really uses magenta, once
		private const string SvgOneMagenta = @"<svg xmlns='http://www.w3.org/2000/svg' width='200mm' height='100mm' viewBox='0 0 200 100'>
  <rect x='50' y='30' width='100' height='40' style='stroke:#000000;fill:none'/>
  <rect x='170' y='5' width='20' height='10' style='stroke:#FF00FF;fill:none'/>
</svg>";

		[Fact]
		public void Import_SingleMagentaElement_IsCutLikeAnyOtherColor()
		{
			GrblFile file = LoadSvg(SvgOneMagenta);

			Assert.Empty(file.RegistrationMarks);
			// the magenta rectangle (X 170..190) is part of the job
			Assert.Contains(Positions(file.GetLines()).Where(p => p.HasValue).Select(p => p.Value).ToList(), p => p.X > 180);
		}

		[Fact]
		public void Import_TwoMagentaMarks_AreNotCut()
		{
			GrblFile file = LoadSvg(Svg);

			Assert.Equal(2, file.RegistrationMarks.Count);
			Assert.DoesNotContain(Positions(file.GetLines()).Where(p => p.HasValue).Select(p => p.Value).ToList(), p => p.X < 49 || p.X > 151);
		}

		[Fact]
		public void Import_MarksSplitInTwoFiles_AreRegisteredWhenAppended()
		{
			string a = @"<svg xmlns='http://www.w3.org/2000/svg' width='200mm' height='100mm' viewBox='0 0 200 100'>
  <rect x='50' y='30' width='100' height='40' style='stroke:#000000;fill:none'/>
  <path d='M 7 10 L 13 10 M 10 7 L 10 13' style='fill:none;stroke:#FF00FF;stroke-width:0.3'/>
  <path d='M 187 90 L 193 90 M 190 87 L 190 93' style='fill:none;stroke:#FF00FF;stroke-width:0.3'/>
</svg>";
			string b = @"<svg xmlns='http://www.w3.org/2000/svg' width='200mm' height='100mm' viewBox='0 0 200 100'>
  <rect x='80' y='40' width='20' height='10' style='stroke:#000000;fill:none'/>
  <path d='M 97 50 L 103 50 M 100 47 L 100 53' style='fill:none;stroke:#FF00FF;stroke-width:0.3'/>
</svg>";
			var file = new GrblFile();
			Load(file, f => f.LoadImportedSVG(WriteSvg("a.svg", a), false, null, ColorFilter.All));
			Load(file, f => f.LoadImportedSVG(WriteSvg("b.svg", b), true, null, ColorFilter.All));

			// the two marks of the first file make the third (one only in its own file) a mark too
			Assert.Equal(3, file.RegistrationMarks.Count);
		}

		[Fact]
		public void AlignNoScaling_RotatedAndShiftedSheet_MovesWholeJob()
		{
			GrblFile file = LoadJob();
			var manager = new PrintAndCutManager();
			SetTargets(manager, file, 1.0);
			manager.Align(false, Wco);

			GCodeTransformResult result = new AlignedJobCache().GetLines(file, manager, Wco);

			Assert.NotNull(result);
			Assert.False(result.HasBlockingIssues);
			AssertMapped(file.GetLines(), result.Lines, p => Sheet(p, 1.0));
		}

		[Fact]
		public void AlignWithScaling_StretchedPrint_ScalesAroundFirstTarget()
		{
			const double printScale = 1.01;
			GrblFile file = LoadJob();
			var manager = new PrintAndCutManager();
			SetTargets(manager, file, printScale);
			AlignmentTransform transform = manager.Align(true, Wco);

			GCodeTransformResult result = new AlignedJobCache().GetLines(file, manager, Wco);

			Assert.Equal(printScale, transform.Scale, 9);
			AssertMapped(file.GetLines(), result.Lines, p => Sheet(p, printScale));
		}

		[Fact]
		public void Align_MarksLandOnMeasuredPositions()
		{
			GrblFile file = LoadJob();
			var manager = new PrintAndCutManager();
			SetTargets(manager, file, 1.0);

			AlignmentTransform transform = manager.Align(false, Wco);

			foreach (RegistrationMark mark in file.RegistrationMarks)
			{
				Vec2 expected = MachinePosition(mark.Center, 1.0) - Wco;
				Vec2 actual = transform.TransformPoint(mark.Center);
				Assert.Equal(expected.X, actual.X, 6);
				Assert.Equal(expected.Y, actual.Y, 6);
			}
		}

		[Fact]
		public void AnalyzedCopy_RangeFollowsTheAlignedJob_AndOriginalIsUntouched()
		{
			GrblFile file = LoadJob();
			List<string> original = file.GetLines();
			var manager = new PrintAndCutManager();
			SetTargets(manager, file, 1.0);
			manager.Align(false, Wco);

			GrblFile aligned = new AlignedJobCache().GetAnalyzedCopy(file, manager, Wco);

			Assert.NotNull(aligned);
			Assert.Equal(original, file.GetLines());
			Assert.Equal(original.Count, aligned.GetLines().Count);

			// the drawing is the rectangle (the appended segment is inside it)
			ProgramRange.XYRange source = file.Range.DrawingRange;
			Assert.Equal(50m, source.X.Min);
			Assert.Equal(150m, source.X.Max);
			Assert.Equal(30m, source.Y.Min);
			Assert.Equal(70m, source.Y.Max);

			// aligned drawing range (used by framing) = box of the rotated corners
			Vec2[] corners = { new Vec2(50, 30), new Vec2(150, 30), new Vec2(150, 70), new Vec2(50, 70) };
			Vec2[] moved = corners.Select(c => Sheet(c, 1.0)).ToArray();
			ProgramRange.XYRange range = aligned.Range.DrawingRange;
			Assert.True(range.ValidRange);
			Assert.InRange((double)range.X.Min, moved.Min(c => c.X) - Tolerance, moved.Min(c => c.X) + Tolerance);
			Assert.InRange((double)range.X.Max, moved.Max(c => c.X) - Tolerance, moved.Max(c => c.X) + Tolerance);
			Assert.InRange((double)range.Y.Min, moved.Min(c => c.Y) - Tolerance, moved.Min(c => c.Y) + Tolerance);
			Assert.InRange((double)range.Y.Max, moved.Max(c => c.Y) - Tolerance, moved.Max(c => c.Y) + Tolerance);
		}

		[Fact]
		public void Reset_SendsTheOriginalJobAgain()
		{
			GrblFile file = LoadJob();
			var manager = new PrintAndCutManager();
			SetTargets(manager, file, 1.0);
			manager.Align(false, Wco);
			var cache = new AlignedJobCache();
			Assert.NotNull(cache.GetLines(file, manager, Wco));

			manager.Reset();

			Assert.Null(cache.GetLines(file, manager, Wco));
			Assert.Null(cache.GetAnalyzedCopy(file, manager, Wco));
		}

		// --- helpers ---

		/// <summary>Where a design point is on the bed, in work coordinates (the printed sheet was rotated, shifted and scaled).</summary>
		private static Vec2 Sheet(Vec2 design, double printScale)
		{
			double a = SheetAngle * Math.PI / 180.0;
			double x = design.X * printScale, y = design.Y * printScale;
			return new Vec2(x * Math.Cos(a) - y * Math.Sin(a) + Shift.X, x * Math.Sin(a) + y * Math.Cos(a) + Shift.Y);
		}

		/// <summary>The machine position (MPos) the user reads after jogging onto a design point.</summary>
		private static Vec2 MachinePosition(Vec2 design, double printScale)
		{
			return Sheet(design, printScale) + Wco;
		}

		private static void SetTargets(PrintAndCutManager manager, GrblFile file, double printScale)
		{
			RegistrationMark first = file.RegistrationMarks[0], second = file.RegistrationMarks[1];
			manager.SetFirstTarget(first, MachinePosition(first.Center, printScale));
			manager.SetSecondTarget(second, MachinePosition(second.Center, printScale));
		}

		private string WriteSvg(string name, string content)
		{
			string path = Path.Combine(mFolder, name);
			File.WriteAllText(path, content);
			return path;
		}

		private GrblFile LoadSvg(string svg)
		{
			var file = new GrblFile();
			string path = WriteSvg("only.svg", svg);
			Load(file, f => f.LoadImportedSVG(path, false, null, ColorFilter.All));
			return file;
		}

		private GrblFile LoadJob()
		{
			string svg = Path.Combine(mFolder, "design.svg");
			string nc = Path.Combine(mFolder, "engrave.nc");
			File.WriteAllText(svg, Svg);
			File.WriteAllText(nc, Appended);

			var file = new GrblFile();
			Load(file, f => f.LoadImportedSVG(svg, false, null, ColorFilter.All));
			Load(file, f => f.LoadFile(nc, true));
			return file;
		}

		/// <summary>GrblFile loads on its own thread: waits until it is done.</summary>
		private static void Load(GrblFile file, Action<GrblFile> load)
		{
			using (var loaded = new ManualResetEvent(false))
			{
				GrblFile.OnFileLoadedDlg handler = (elapsed, name) => loaded.Set();
				file.OnFileLoaded += handler;
				try
				{
					load(file);
					Assert.True(loaded.WaitOne(TimeSpan.FromSeconds(30)), "the file was not loaded (see the session log)");
				}
				finally
				{
					file.OnFileLoaded -= handler;
				}
			}

			// the loading thread clears its flag right after raising the event
			DateTime limit = DateTime.UtcNow.AddSeconds(10);
			while (file.CheckInUse(false))
			{
				Assert.True(DateTime.UtcNow < limit, "the loading thread did not finish");
				Thread.Sleep(10);
			}
		}

		/// <summary>Checks that every position reached by <paramref name="output"/> is the mapped position of <paramref name="input"/>.</summary>
		private static void AssertMapped(IList<string> input, IList<string> output, Func<Vec2, Vec2> map)
		{
			Assert.Equal(input.Count, output.Count);
			Vec2?[] before = Positions(input), after = Positions(output);
			int checkedMoves = 0;
			for (int i = 0; i < input.Count; i++)
			{
				if (!before[i].HasValue)
					continue;
				Assert.True(after[i].HasValue, "line " + i + " lost its position: '" + output[i] + "'");
				Vec2 expected = map(before[i].Value);
				Assert.True(Math.Abs(expected.X - after[i].Value.X) <= Tolerance && Math.Abs(expected.Y - after[i].Value.Y) <= Tolerance,
					string.Format(CultureInfo.InvariantCulture, "line {0}: '{1}' -> '{2}', expected X{3:0.###} Y{4:0.###}", i, input[i], output[i], expected.X, expected.Y));
				checkedMoves++;
			}
			Assert.True(checkedMoves > 8, "too few moves checked: " + checkedMoves);
		}

		private static readonly Regex Word = new Regex(@"([A-Z])\s*([-+]?[0-9]*\.?[0-9]+)", RegexOptions.IgnoreCase);

		/// <summary>Absolute XY position after each line, once both axes are known (the jobs used here are G90 only).</summary>
		private static Vec2?[] Positions(IList<string> lines)
		{
			var rv = new Vec2?[lines.Count];
			double? x = null, y = null;
			for (int i = 0; i < lines.Count; i++)
			{
				string code = Regex.Replace(lines[i], @"\(.*?\)|;.*$", "");
				Assert.DoesNotContain("G91", code.ToUpperInvariant());
				foreach (Match m in Word.Matches(code))
				{
					char letter = char.ToUpperInvariant(m.Groups[1].Value[0]);
					double value = double.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture);
					if (letter == 'X') x = value;
					else if (letter == 'Y') y = value;
				}
				if (x.HasValue && y.HasValue)
					rv[i] = new Vec2(x.Value, y.Value);
			}
			return rv;
		}

		private static void AssertNear(double x, double y, Vec2 actual)
		{
			Assert.Equal(x, actual.X, 2);
			Assert.Equal(y, actual.Y, 2);
		}
	}
}
