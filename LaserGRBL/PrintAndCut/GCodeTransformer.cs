// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LaserGRBL.PrintAndCut
{
	/// <summary>Kind of problem found while rewriting a G-code program.</summary>
	public enum GCodeTransformIssue
	{
		/// <summary>G20: the program is in inches, machine targets are in mm. Blocking.</summary>
		InchUnits,
		/// <summary>G53 move in machine coordinates: left untouched.</summary>
		MachineCoordinates,
		/// <summary>G92 / G10 changes the coordinate system: left untouched.</summary>
		CoordinateOffset,
		/// <summary>G28 / G30 return to a stored position: left untouched.</summary>
		ReturnToStoredPosition,
		/// <summary>G38.x probing move: left untouched.</summary>
		Probe,
		/// <summary>Absolute move with a missing axis while the current position is unknown.</summary>
		UnknownPosition,
	}

	/// <summary>A problem found while rewriting a G-code program.</summary>
	public sealed class GCodeTransformWarning
	{
		internal GCodeTransformWarning(GCodeTransformIssue issue, int lineIndex)
		{
			Issue = issue;
			LineIndex = lineIndex;
		}

		/// <summary>The kind of problem.</summary>
		public GCodeTransformIssue Issue { get; private set; }

		/// <summary>Zero-based index of the first line where the problem was found.</summary>
		public int LineIndex { get; private set; }

		/// <summary>True if the output must not be used.</summary>
		public bool IsBlocking
		{ get { return Issue == GCodeTransformIssue.InchUnits; } }
	}

	/// <summary>Output of <see cref="GCodeTransformer.Transform"/>.</summary>
	public sealed class GCodeTransformResult
	{
		internal GCodeTransformResult(List<string> lines, List<GCodeTransformWarning> warnings)
		{
			Lines = lines;
			Warnings = warnings;
		}

		/// <summary>Rewritten lines, exactly one for each input line (same index).</summary>
		public List<string> Lines { get; private set; }

		/// <summary>Problems found, at most one for each <see cref="GCodeTransformIssue"/>.</summary>
		public List<GCodeTransformWarning> Warnings { get; private set; }

		/// <summary>True if at least one warning is blocking.</summary>
		public bool HasBlockingIssues
		{
			get
			{
				foreach (GCodeTransformWarning w in Warnings)
					if (w.IsBlocking) return true;
				return false;
			}
		}
	}

	/// <summary>
	/// Rewrites a G-code program line by line, applying an <see cref="AlignmentTransform"/> to the XY plane.
	/// <list type="bullet">
	/// <item>Absolute moves (G90): end points are transformed as points; both X and Y are always written.</item>
	/// <item>Relative moves (G91): deltas are rotated and scaled, not translated.</item>
	/// <item>Arcs (G2/G3): I/J are rotated and scaled, R is scaled. Direction is preserved.</item>
	/// <item>S, F, Z and every other word are left untouched, as are comments.</item>
	/// </list>
	/// The output has the same number of lines as the input, so line numbers (job resume) stay valid.
	/// </summary>
	public static class GCodeTransformer
	{
		private const int MotionNone = -1;

		/// <summary>Rewrites the given program. The start position is assumed to be X0 Y0, as in the preview.</summary>
		public static GCodeTransformResult Transform(IList<string> lines, AlignmentTransform transform)
		{
			if (lines == null) throw new ArgumentNullException("lines");
			if (transform == null) throw new ArgumentNullException("transform");

			List<string> output = new List<string>(lines.Count);
			List<GCodeTransformWarning> warnings = new List<GCodeTransformWarning>();
			State state = new State();

			for (int i = 0; i < lines.Count; i++)
				output.Add(TransformLine(lines[i], i, transform, state, warnings));

			return new GCodeTransformResult(output, warnings);
		}

		private sealed class State
		{
			public bool Absolute = true;
			public bool Inches = false;
			public int Motion = MotionNone;
			public double X = 0;
			public double Y = 0;
			public bool PositionKnown = true;
		}

		private static string TransformLine(string line, int index, AlignmentTransform t, State state, List<GCodeTransformWarning> warnings)
		{
			if (line == null) return null;

			string trimmed = line.TrimStart();
			if (trimmed.Length == 0 || trimmed[0] == '$' || trimmed[0] == '%')
				return line; // grbl system commands and program delimiters

			List<Token> tokens = Tokenize(line);

			bool nonModalWithAxes = false;
			bool probe = false;
			foreach (Token tk in tokens)
			{
				if (!tk.IsWord || tk.Letter != 'G') continue;

				double g = tk.Value;
				if (Is(g, 90)) state.Absolute = true;
				else if (Is(g, 91)) state.Absolute = false;
				else if (Is(g, 20)) state.Inches = true;
				else if (Is(g, 21)) state.Inches = false;
				else if (Is(g, 0)) state.Motion = 0;
				else if (Is(g, 1)) state.Motion = 1;
				else if (Is(g, 2)) state.Motion = 2;
				else if (Is(g, 3)) state.Motion = 3;
				else if (Is(g, 80)) state.Motion = MotionNone;
				else if (g >= 38 && g < 39) { probe = true; state.Motion = MotionNone; }
				else if (Is(g, 53)) { nonModalWithAxes = true; AddOnce(warnings, GCodeTransformIssue.MachineCoordinates, index); }
				else if (Is(g, 92) || Is(g, 10)) { nonModalWithAxes = true; AddOnce(warnings, GCodeTransformIssue.CoordinateOffset, index); }
				else if (Is(g, 28) || Is(g, 30)) { nonModalWithAxes = true; AddOnce(warnings, GCodeTransformIssue.ReturnToStoredPosition, index); }
			}

			Token x = Find(tokens, 'X');
			Token y = Find(tokens, 'Y');
			Token iw = Find(tokens, 'I');
			Token jw = Find(tokens, 'J');
			Token r = Find(tokens, 'R');
			bool hasXY = x != null || y != null;
			bool isArc = state.Motion == 2 || state.Motion == 3;
			bool hasIJ = isArc && (iw != null || jw != null);
			bool hasR = isArc && r != null;

			if (probe)
			{
				if (hasXY) { AddOnce(warnings, GCodeTransformIssue.Probe, index); state.PositionKnown = false; }
				return line;
			}

			if (nonModalWithAxes)
			{
				if (hasXY) state.PositionKnown = false;
				return line; // never touch moves or offsets outside the work coordinate system
			}

			if (state.Motion == MotionNone || (!hasXY && !hasIJ && !hasR))
				return line;

			if (state.Inches)
			{
				AddOnce(warnings, GCodeTransformIssue.InchUnits, index);
				return line;
			}

			string newXY = null, newIJ = null, newR = null;

			if (hasXY)
			{
				Vec2 target;
				if (state.Absolute)
				{
					if ((x == null || y == null) && !state.PositionKnown)
						AddOnce(warnings, GCodeTransformIssue.UnknownPosition, index);

					Vec2 p = new Vec2(x != null ? x.Value : state.X, y != null ? y.Value : state.Y);
					state.X = p.X;
					state.Y = p.Y;
					state.PositionKnown = state.PositionKnown || (x != null && y != null);
					target = t.TransformPoint(p);
				}
				else
				{
					Vec2 d = new Vec2(x != null ? x.Value : 0, y != null ? y.Value : 0);
					state.X += d.X;
					state.Y += d.Y;
					target = t.TransformVector(d);
				}
				newXY = "X" + Format(target.X) + " Y" + Format(target.Y);
			}

			if (hasIJ)
			{
				Vec2 ij = t.TransformVector(new Vec2(iw != null ? iw.Value : 0, jw != null ? jw.Value : 0));
				newIJ = "I" + Format(ij.X) + " J" + Format(ij.Y);
			}

			if (hasR)
				newR = "R" + Format(r.Value * t.Scale);

			return Rebuild(tokens, newXY, newIJ, newR);
		}

		private static string Rebuild(List<Token> tokens, string newXY, string newIJ, string newR)
		{
			List<string> parts = new List<string>(tokens.Count);
			bool xyDone = false, ijDone = false;
			foreach (Token tk in tokens)
			{
				if (tk.IsWord && newXY != null && (tk.Letter == 'X' || tk.Letter == 'Y'))
				{
					if (!xyDone) parts.Add(newXY);
					else DropTrailingSpace(parts);
					xyDone = true;
				}
				else if (tk.IsWord && newIJ != null && (tk.Letter == 'I' || tk.Letter == 'J'))
				{
					if (!ijDone) parts.Add(newIJ);
					else DropTrailingSpace(parts);
					ijDone = true;
				}
				else if (tk.IsWord && newR != null && tk.Letter == 'R')
				{
					parts.Add(newR);
				}
				else
				{
					parts.Add(tk.Text);
				}
			}

			StringBuilder sb = new StringBuilder();
			foreach (string p in parts) sb.Append(p);
			return sb.ToString();
		}

		/// <summary>Removes the separator in front of a word that is being dropped.</summary>
		private static void DropTrailingSpace(List<string> parts)
		{
			while (parts.Count > 0 && parts[parts.Count - 1].Trim().Length == 0)
				parts.RemoveAt(parts.Count - 1);
		}

		private static void AddOnce(List<GCodeTransformWarning> warnings, GCodeTransformIssue issue, int index)
		{
			foreach (GCodeTransformWarning w in warnings)
				if (w.Issue == issue) return;
			warnings.Add(new GCodeTransformWarning(issue, index));
		}

		private static bool Is(double value, double code)
		{ return Math.Abs(value - code) < 0.0001; }

		private static Token Find(List<Token> tokens, char letter)
		{
			foreach (Token tk in tokens)
				if (tk.IsWord && tk.Letter == letter) return tk;
			return null;
		}

		/// <summary>Formats a coordinate with at most 3 decimals, invariant culture, never "-0".</summary>
		internal static string Format(double v)
		{
			string s = Math.Round(v, 3).ToString("0.###", CultureInfo.InvariantCulture);
			return s == "-0" ? "0" : s;
		}

		private sealed class Token
		{
			public bool IsWord;
			public char Letter;
			public double Value;
			public string Text;
		}

		/// <summary>
		/// Splits a line into words (letter + number) and raw text (spaces, comments, anything else),
		/// so that the line can be rebuilt with only some words replaced.
		/// </summary>
		private static List<Token> Tokenize(string line)
		{
			List<Token> tokens = new List<Token>();
			int i = 0;
			while (i < line.Length)
			{
				char c = line[i];
				int start = i;

				if (c == ';')
				{
					i = line.Length;
				}
				else if (c == '(')
				{
					int end = line.IndexOf(')', i);
					i = end < 0 ? line.Length : end + 1;
				}
				else if (char.IsLetter(c))
				{
					i++;
					while (i < line.Length && (char.IsDigit(line[i]) || line[i] == '.' || line[i] == '-' || line[i] == '+'))
						i++;

					double value;
					string number = line.Substring(start + 1, i - start - 1);
					if (number.Length > 0 && double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
					{
						tokens.Add(new Token { IsWord = true, Letter = char.ToUpperInvariant(c), Value = value, Text = line.Substring(start, i - start) });
						continue;
					}
				}
				else
				{
					i++;
				}

				tokens.Add(new Token { IsWord = false, Text = line.Substring(start, i - start) });
			}
			return tokens;
		}
	}
}
