using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LaserGRBL.DeviceProfiles
{
	internal static class SourceProgram
	{
		private const int MaximumLines = 10000000;
		private const int MaximumMoves = 4000000;
		private const int MaximumLineLength = 8192;
		private const decimal MaximumCoordinate = 1000000m;
		private const decimal MaximumFeed = 1000000m;
		private const double MaximumArcRadius = 100000.0;
		private const int MaximumArcSegments = 16000;
		private const double ArcTolerance = 0.01;
		private const double FullTurn = 2.0 * Math.PI;

		internal static IList<ExportMove> Parse(IEnumerable<string> lines, int sourceMax, decimal defaultWorkFeed)
		{
			if (lines == null)
				throw new ArgumentNullException("lines");
			if (sourceMax <= 0)
				throw new ArgumentOutOfRangeException("sourceMax");
			if (defaultWorkFeed <= 0 || defaultWorkFeed > MaximumFeed)
				throw new ArgumentOutOfRangeException("defaultWorkFeed");

			var moves = new List<ExportMove>();
			var state = new ModalState { Feed = defaultWorkFeed, Units = 1m };
			int lineNumber = 0;
			foreach (string line in lines)
			{
				lineNumber++;
				try
				{
					if (lineNumber > MaximumLines)
						throw new InvalidDataException("The source contains too many lines.");
					string words = RemoveCommentsAndWhitespace(line);
					if (words.Length == 0)
						continue;
					if (state.Ended)
						throw new InvalidDataException("A command follows the program end.");

					Block block = ReadBlock(words);
					ApplyBlock(block, ref state, moves, sourceMax);
				}
				catch (InvalidDataException error)
				{
					throw new InvalidDataException(
						string.Format(CultureInfo.InvariantCulture, "Source G-code line {0}: {1}", lineNumber, error.Message), error);
				}
			}
			return moves;
		}

		private struct ModalState
		{
			internal decimal X, Y, Feed, Units;
			internal int Motion, Power;
			internal bool Incremental, LaserEnabled, Ended;
		}

		private struct Block
		{
			internal decimal? X, Y, I, J, R, F, S, N;
			internal int? Motion, Spindle;
			internal bool? Incremental, Inches;
			internal bool ArcIncremental, Plane, FeedMode;
		}

		private static string RemoveCommentsAndWhitespace(string line)
		{
			if (line == null || line.Length > MaximumLineLength)
				throw new InvalidDataException("The source line is missing or too long.");
			if (line.IndexOf('\r') >= 0 || line.IndexOf('\n') >= 0)
				throw new InvalidDataException("Expected one command block per line.");

			var words = new StringBuilder(line.Length);
			bool inComment = false;
			foreach (char c in line)
			{
				if (c == ';' && !inComment)
					break;
				if (c == '(' && !inComment)
					inComment = true;
				else if (c == ')' && inComment)
					inComment = false;
				else if (c == '(' || c == ')')
					throw new InvalidDataException("Unexpected comment delimiter.");
				else if (!inComment && c != ' ' && c != '\t')
					words.Append(c >= 'a' && c <= 'z' ? (char)(c - ('a' - 'A')) : c);
			}
			if (inComment)
				throw new InvalidDataException("Unclosed parenthesis comment.");
			return words.ToString();
		}

		private static Block ReadBlock(string words)
		{
			var block = new Block();
			int index = 0;
			while (index < words.Length)
			{
				char letter = words[index++];
				if (letter < 'A' || letter > 'Z')
					throw new InvalidDataException("Expected a G-code word.");

				int start = index;
				if (index < words.Length && (words[index] == '+' || words[index] == '-'))
					index++;
				while (index < words.Length && ((words[index] >= '0' && words[index] <= '9') || words[index] == '.'))
					index++;

				decimal value;
				if (!decimal.TryParse(words.Substring(start, index - start), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
					CultureInfo.InvariantCulture, out value) || value < -int.MaxValue || value > int.MaxValue)
					throw new InvalidDataException("Invalid number for " + letter + ".");

				switch (letter)
				{
					case 'G': ReadGCode(ref block, value); break;
					case 'M': ReadMCode(ref block, value); break;
					case 'X': SetOnce(ref block.X, value, "X"); break;
					case 'Y': SetOnce(ref block.Y, value, "Y"); break;
					case 'I': SetOnce(ref block.I, value, "I"); break;
					case 'J': SetOnce(ref block.J, value, "J"); break;
					case 'R': SetOnce(ref block.R, value, "R"); break;
					case 'F': SetOnce(ref block.F, value, "F"); break;
					case 'S': SetOnce(ref block.S, value, "S"); break;
					case 'N':
						if (value < 0 || value != decimal.Truncate(value))
							throw new InvalidDataException("Line numbers must be nonnegative integers.");
						SetOnce(ref block.N, value, "N");
						break;
					default: throw new InvalidDataException("Unsupported word " + letter + ".");
				}
			}
			return block;
		}

		private static void SetOnce<T>(ref T? field, T value, string word) where T : struct
		{
			if (field.HasValue)
				throw new InvalidDataException("Multiple " + word + " words in one block.");
			field = value;
		}

		private static void ReadGCode(ref Block block, decimal code)
		{
			switch (code)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					SetOnce(ref block.Motion, (int)code, "motion");
					break;
				case 17:
					SetFlag(ref block.Plane, "G17");
					break;
				case 20:
				case 21:
					SetOnce(ref block.Inches, code == 20, "units");
					break;
				case 90:
				case 91:
					SetOnce(ref block.Incremental, code == 91, "distance mode");
					break;
				case 91.1m:
					SetFlag(ref block.ArcIncremental, "G91.1");
					break;
				case 94:
					SetFlag(ref block.FeedMode, "G94");
					break;
				default:
					throw new InvalidDataException("Unsupported source command G" + code.ToString(CultureInfo.InvariantCulture) + ".");
			}
		}

		private static void SetFlag(ref bool field, string code)
		{
			if (field)
				throw new InvalidDataException("Repeated command " + code + ".");
			field = true;
		}

		private static void ReadMCode(ref Block block, decimal code)
		{
			switch (code)
			{
				case 2:
				case 3:
				case 4:
				case 5:
				case 30:
					SetOnce(ref block.Spindle, (int)code, "spindle or program end");
					break;
				default:
					throw new InvalidDataException("Unsupported source command M" + code.ToString(CultureInfo.InvariantCulture) + ".");
			}
		}

		private static void ApplyBlock(Block block, ref ModalState state, List<ExportMove> moves, int sourceMax)
		{
			if (block.Inches.HasValue)
				state.Units = block.Inches.Value ? 25.4m : 1m;
			if (block.Incremental.HasValue)
				state.Incremental = block.Incremental.Value;
			if (block.Motion.HasValue)
				state.Motion = block.Motion.Value;
			if (block.F.HasValue)
			{
				state.Feed = block.F.Value * state.Units;
				if (state.Feed <= 0 || state.Feed > MaximumFeed)
					throw new InvalidDataException("Feed must be positive and within the supported range.");
			}
			if (block.S.HasValue)
			{
				decimal power = block.S.Value;
				if (power < 0 || power > sourceMax)
					throw new InvalidDataException("Power is outside the selected source power range.");
				state.Power = (int)decimal.Round(power, 0, MidpointRounding.AwayFromZero);
			}

			bool hasAxes = block.X.HasValue || block.Y.HasValue;
			bool hasArcCenter = block.I.HasValue || block.J.HasValue || block.R.HasValue;
			switch (block.Spindle)
			{
				case 2:
				case 30:
					if (hasAxes || hasArcCenter || block.Motion.HasValue)
						throw new InvalidDataException("A program end command cannot include motion.");
					state.Ended = true;
					return;
				case 3:
				case 4:
					state.LaserEnabled = true;
					break;
				case 5:
					state.LaserEnabled = false;
					break;
			}

			if (!hasAxes && !hasArcCenter)
				return;

			decimal x = AxisPosition(block.X, state.X, state);
			decimal y = AxisPosition(block.Y, state.Y, state);
			bool rapid = state.Motion == 0;
			int powerForMotion = !rapid && state.LaserEnabled ? state.Power : 0;
			if (state.Motion >= 2)
				AppendArc(moves, state, block, x, y, powerForMotion);
			else
			{
				if (hasArcCenter)
					throw new InvalidDataException("Arc center words require G2 or G3.");
				if (x != state.X || y != state.Y)
					AppendMove(moves, x, y, state.Feed, powerForMotion, rapid);
			}
			state.X = x;
			state.Y = y;
		}

		private static decimal AxisPosition(decimal? word, decimal current, ModalState state)
		{
			decimal axis = word.HasValue ? word.Value * state.Units : 0m;
			if (!word.HasValue || state.Incremental)
				axis += current;
			ValidateCoordinate(axis);
			return axis;
		}

		private static void ValidateCoordinate(decimal value)
		{
			if (value < -MaximumCoordinate || value > MaximumCoordinate)
				throw new InvalidDataException("Coordinate is outside the supported range.");
		}

		private static void AppendMove(List<ExportMove> moves, decimal x, decimal y, decimal feed, int power, bool rapid, bool curved = false)
		{
			if (moves.Count >= MaximumMoves)
				throw new InvalidDataException("The source generates too many moves.");
			ValidateCoordinate(x);
			ValidateCoordinate(y);
			moves.Add(new ExportMove { X = x, Y = y, Feed = feed, Power = power, Rapid = rapid, Curved = curved });
		}

		private static void AppendArc(List<ExportMove> moves, ModalState state, Block block, decimal x, decimal y, int power)
		{
			double startX = (double)state.X;
			double startY = (double)state.Y;
			double endX = (double)x;
			double endY = (double)y;
			bool clockwise = state.Motion == 2;
			bool fullCircle = state.X == x && state.Y == y;
			double centerX;
			double centerY;
			double radius;

			if (block.R.HasValue)
			{
				if (block.I.HasValue || block.J.HasValue || fullCircle)
					throw new InvalidDataException("An R arc needs a distinct endpoint and no I or J words.");
				double signedRadius = (double)(block.R.Value * state.Units);
				radius = Math.Abs(signedRadius);
				double dx = endX - startX;
				double dy = endY - startY;
				double halfChord = Distance(dx, dy) / 2.0;
				if (halfChord == 0)
					throw new InvalidDataException("Arc endpoints are too close to resolve.");
				if (radius < halfChord)
					throw new InvalidDataException("Arc radius is smaller than half the endpoint distance.");
				double perpendicular = Math.Sqrt((radius - halfChord) * (radius + halfChord));
				if (clockwise == (signedRadius > 0))
					perpendicular = -perpendicular;
				centerX = startX + dx / 2.0 - dy * perpendicular / (2.0 * halfChord);
				centerY = startY + dy / 2.0 + dx * perpendicular / (2.0 * halfChord);
			}
			else
			{
				if (!block.I.HasValue && !block.J.HasValue)
					throw new InvalidDataException("An arc needs I/J center offsets or R.");
				double i = (double)(block.I.GetValueOrDefault() * state.Units);
				double j = (double)(block.J.GetValueOrDefault() * state.Units);
				radius = Distance(i, j);
				centerX = startX + i;
				centerY = startY + j;
				double radiusDifference = Math.Abs(Distance(endX - centerX, endY - centerY) - radius);
				// GRBL permits a small difference because source coordinates are rounded.
				if (radiusDifference > 0.005 && (radiusDifference > 0.5 || radiusDifference > 0.001 * radius))
					throw new InvalidDataException("Arc endpoint does not match its center offsets.");
			}

			if (radius <= 0 || radius > MaximumArcRadius)
				throw new InvalidDataException("Arc radius is outside the supported range.");

			double startAngle = Math.Atan2(startY - centerY, startX - centerX);
			double endAngle = Math.Atan2(endY - centerY, endX - centerX);
			double direction = clockwise ? -1.0 : 1.0;
			double sweep = fullCircle ? FullTurn : PositiveAngle(direction * (endAngle - startAngle));
			if (sweep < 1e-12)
				throw new InvalidDataException("Arc has no angular travel.");

			double angleStep = Math.Min(Math.PI / 12.0, 2.0 * Math.Acos(Math.Max(-1.0, 1.0 - ArcTolerance / radius)));
			int segments = (int)Math.Ceiling(sweep / angleStep);
			if (segments > MaximumArcSegments)
				throw new InvalidDataException("Arc needs too many linear segments.");

			var stops = new List<double>(segments + 4);
			for (int k = 1; k < segments; k++)
				stops.Add(sweep * k / segments);
			// Include all extrema so work-area validation sees the full arc envelope.
			for (int k = 0; k < 4; k++)
			{
				double delta = PositiveAngle(direction * (k * Math.PI / 2.0 - startAngle));
				if (delta > 1e-12 && delta < sweep - 1e-12)
					stops.Add(delta);
			}
			stops.Sort();

			decimal previousX = state.X;
			decimal previousY = state.Y;
			foreach (double stop in stops)
			{
				double angle = startAngle + direction * stop;
				decimal px = (decimal)Math.Round(centerX + radius * Math.Cos(angle), 6);
				decimal py = (decimal)Math.Round(centerY + radius * Math.Sin(angle), 6);
				if (px != previousX || py != previousY)
					AppendMove(moves, px, py, state.Feed, power, false, true);
				previousX = px;
				previousY = py;
			}
			if (x != previousX || y != previousY)
				AppendMove(moves, x, y, state.Feed, power, false, true);
		}

		private static double Distance(double x, double y)
		{
			return Math.Sqrt(x * x + y * y);
		}

		private static double PositiveAngle(double angle)
		{
			angle %= FullTurn;
			return angle >= 0 ? angle : angle + FullTurn;
		}
	}
}
