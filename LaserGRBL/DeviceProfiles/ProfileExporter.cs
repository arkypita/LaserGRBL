using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LaserGRBL.DeviceProfiles
{
	internal sealed class ExportMove
	{
		internal decimal X, Y, Feed;
		internal int Power;
		internal bool Rapid, Curved;
	}

	internal sealed class ExportLayer
	{
		internal IEnumerable<string> Lines;
		internal int Passes = 1;
		internal int? OperationPower;
		internal decimal? WorkFeed;
		internal bool ApplyOverscan;
	}

	internal sealed class ExportedJob
	{
		internal byte[] Bytes;
		internal IList<ExportMove> Moves;
	}

	internal static class ProfileExporter
	{
		private const int MoveLimit = 2000000;

		internal static ExportedJob Create(DeviceProfile profile, IEnumerable<ExportLayer> layers,
			int sourceMax, bool frame, bool pause)
		{
			profile.Validate();
			if (sourceMax < 1 || sourceMax > 100000)
				throw new InvalidDataException("Algtöö võimsusvahemik peab olema 1–100 000.");
			if (frame && profile.PreviewPasses <= 0)
				throw new InvalidDataException("Sellel profiilil puudub raamimise seadistus.");
			if (pause && string.IsNullOrEmpty(profile.PauseCommand))
				throw new InvalidDataException("Sellel profiilil puudub pausikäsk.");

			var job = new Writer(profile);
			job.Begin();
			var work = new List<ExportMove>();
			foreach (ExportLayer layer in layers)
			{
				ValidateLayer(profile, layer);
				var panel = SourceProgram.Parse(layer.Lines, sourceMax, layer.WorkFeed ?? profile.WorkFeed);
				var moves = Plan(profile, layer, panel, sourceMax);
				for (int pass = 0; pass < layer.Passes; pass++)
				{
					if (work.Count > MoveLimit - moves.Count)
						throw new InvalidDataException("Töö sisaldab liiga palju liigutusi.");
					work.AddRange(moves);
				}
			}
			if (work.Count == 0)
				throw new InvalidDataException("Eksporditav töö on tühi.");

			if (frame)
				Frame(profile, job, work);
			if (pause)
				job.Pause();
			foreach (ExportMove move in work)
				job.Move(move);
			return job.Finish();
		}

		private static void ValidateLayer(DeviceProfile profile, ExportLayer layer)
		{
			if (layer.Passes < 1 || layer.Passes > 100)
				throw new InvalidDataException("Läbimiste arv peab olema 1–100.");
			if (layer.OperationPower.HasValue && (layer.OperationPower < 0 || layer.OperationPower > profile.MaxPower ||
				(layer.OperationPower > 0 && layer.OperationPower < profile.MinPower)))
				throw new InvalidDataException("Materjali võimsus jääb profiili piiridest välja.");
			if (layer.WorkFeed.HasValue && (layer.WorkFeed <= 0 || layer.WorkFeed > profile.RapidFeed))
				throw new InvalidDataException("Materjali kiirus jääb profiili piiridest välja.");
		}

		private static List<ExportMove> Plan(DeviceProfile profile, ExportLayer layer,
			IList<ExportMove> source, int sourceMax)
		{
			var result = new List<ExportMove>();
			if (source.Count == 0)
				return result;

			// Every pass begins at its own logical zero, including programs starting with a work move.
			result.Add(Map(profile, 0, 0, 0, profile.RapidFeed, true));
			decimal previousX = 0, previousY = 0;
			for (int i = 0; i < source.Count; i++)
			{
				ExportMove current = source[i];
				int direction = Math.Sign(current.X - previousX);
				if (layer.ApplyOverscan && profile.Overscan > 0 && !current.Rapid &&
					!current.Curved && current.Y == previousY && direction != 0)
				{
					int end = i;
					bool burns = MapPower(profile, layer, current.Power, sourceMax) > 0;
					while (end + 1 < source.Count && !source[end + 1].Rapid && !source[end + 1].Curved &&
						source[end + 1].Y == current.Y && Math.Sign(source[end + 1].X - source[end].X) == direction)
					{
						end++;
						burns |= MapPower(profile, layer, source[end].Power, sourceMax) > 0;
					}
					if (burns)
					{
						decimal runIn = previousX - direction * profile.Overscan;
						decimal runOut = source[end].X + direction * profile.Overscan;
						result.Add(Map(profile, runIn, previousY, 0, profile.RapidFeed, true));
						result.Add(Map(profile, previousX, previousY, 0, Feed(profile, layer, current), false));
						for (; i <= end; i++)
							result.Add(MapSource(profile, layer, source[i], sourceMax));
						i = end;
						current = source[i];
						result.Add(Map(profile, runOut, current.Y, 0, Feed(profile, layer, current), false));
						// A following G1 segment must still begin at the original endpoint.
						if (i + 1 < source.Count && !source[i + 1].Rapid)
							result.Add(Map(profile, current.X, current.Y, 0, profile.RapidFeed, true));
						previousX = current.X;
						previousY = current.Y;
						continue;
					}
				}
				result.Add(MapSource(profile, layer, current, sourceMax));
				previousX = current.X;
				previousY = current.Y;
			}
			if (result.Count > MoveLimit)
				throw new InvalidDataException("Töö sisaldab liiga palju liigutusi.");
			return result;
		}

		private static ExportMove MapSource(DeviceProfile profile, ExportLayer layer, ExportMove move, int sourceMax)
		{
			return Map(profile, move.X, move.Y, move.Rapid ? 0 : MapPower(profile, layer, move.Power, sourceMax),
				Feed(profile, layer, move), move.Rapid);
		}

		private static int MapPower(DeviceProfile profile, ExportLayer layer, int sourcePower, int sourceMax)
		{
			if (sourcePower == 0 || layer.OperationPower == 0)
				return 0;
			int maximum = layer.OperationPower ?? profile.MaxPower;
			int scaled = (int)Math.Round(sourcePower * (decimal)maximum / sourceMax, MidpointRounding.AwayFromZero);
			return Math.Max(profile.MinPower, scaled);
		}

		private static decimal Feed(DeviceProfile profile, ExportLayer layer, ExportMove source)
		{
			return source.Rapid ? profile.RapidFeed : layer.WorkFeed ?? source.Feed;
		}

		private static ExportMove Map(DeviceProfile profile, decimal x, decimal y, int power, decimal feed, bool rapid)
		{
			x = profile.OriginX + (profile.ReverseX ? -x : x);
			y = profile.OriginY + (profile.ReverseY ? -y : y);
			if (x < 0 || x > profile.Width || y < 0 || y > profile.Height)
				throw new InvalidDataException(string.Format(CultureInfo.InvariantCulture,
					"Liikumine X{0:0.####} Y{1:0.####} ületab tööala {2} × {3} mm. Kontrolli paigutust ja kiirendusvaru.",
					x, y, profile.Width, profile.Height));
			if (feed <= 0 || feed > profile.RapidFeed)
				throw new InvalidDataException("Liikumiskiirus jääb profiili piiridest välja.");
			return new ExportMove { X = x, Y = y, Feed = feed, Power = power, Rapid = rapid };
		}

		private static void Frame(DeviceProfile profile, Writer writer, IList<ExportMove> work)
		{
			decimal left = profile.Width, right = 0, bottom = profile.Height, top = 0;
			bool found = false;
			for (int i = 1; i < work.Count; i++)
			{
				if (work[i].Power <= 0)
					continue;
				left = Math.Min(left, Math.Min(work[i - 1].X, work[i].X));
				right = Math.Max(right, Math.Max(work[i - 1].X, work[i].X));
				bottom = Math.Min(bottom, Math.Min(work[i - 1].Y, work[i].Y));
				top = Math.Max(top, Math.Max(work[i - 1].Y, work[i].Y));
				found = true;
			}
			if (!found)
				throw new InvalidDataException("Raamimiseks peab töö sisaldama aktiivseid graveerimisliigutusi.");

			writer.Move(new ExportMove { X = left, Y = bottom, Feed = profile.RapidFeed, Rapid = true });
			for (int pass = 0; pass < profile.PreviewPasses; pass++)
			{
				FrameMove(profile, writer, right, bottom);
				FrameMove(profile, writer, right, top);
				FrameMove(profile, writer, left, top);
				FrameMove(profile, writer, left, bottom);
				writer.Off();
			}
		}

		private static void FrameMove(DeviceProfile profile, Writer writer, decimal x, decimal y)
		{
			writer.Move(new ExportMove { X = x, Y = y, Feed = profile.RapidFeed, Power = profile.PreviewPower });
		}

		private sealed class Writer
		{
			private readonly DeviceProfile profile;
			private readonly StringBuilder text = new StringBuilder();
			private readonly List<ExportMove> moves = new List<ExportMove>();
			private readonly string newline;
			private int power;

			internal Writer(DeviceProfile profile)
			{
				this.profile = profile;
				newline = profile.LineEndings == "LF" ? "\n" : "\r\n";
			}

			private void Line(string line) { text.Append(line).Append(newline); }

			internal void Begin()
			{
				Line(profile.LaserOff);
				string[] setup = DeviceProfile.Commands(profile.StartCommands);
				if (Array.IndexOf(setup, "G21") < 0) Line("G21");
				if (Array.IndexOf(setup, "G90") < 0) Line("G90");
				foreach (string command in setup)
					Line(command);
			}

			internal void Off() { SetPower(0); }

			internal void Pause()
			{
				Off();
				Line(profile.PauseCommand);
			}

			private void SetPower(int next)
			{
				if (power == next)
					return;
				if (!string.IsNullOrEmpty(profile.SynchronizeCommand))
					Line(profile.SynchronizeCommand);
				Line(next == 0 ? profile.LaserOff : profile.LaserOn.Replace("{power}", next.ToString(CultureInfo.InvariantCulture)));
				power = next;
			}

			internal void Move(ExportMove move)
			{
				// Quantization could turn a tiny powered segment into a stationary burn.
				decimal x = TruncatePosition(move.X), y = TruncatePosition(move.Y);
				decimal feed = Math.Floor(move.Feed * 1000) / 1000;
				if (!move.Rapid && moves.Count > 0 && moves[moves.Count - 1].X == x && moves[moves.Count - 1].Y == y)
					return;
				if (feed <= 0)
					throw new InvalidDataException("Liikumiskiirus on väljundvormingus liiga väike.");
				SetPower(move.Rapid ? 0 : move.Power);
				Line(string.Format(CultureInfo.InvariantCulture, "{0} X{1:0.####} Y{2:0.####} F{3:0.###}",
					move.Rapid ? "G0" : "G1", x, y, feed));
				moves.Add(new ExportMove { X = x, Y = y, Feed = feed, Power = move.Power, Rapid = move.Rapid });
			}

			private static decimal TruncatePosition(decimal value) { return Math.Floor(value * 10000) / 10000; }

			internal ExportedJob Finish()
			{
				if (power != 0)
					Off();
				else
					Line(profile.LaserOff);
				foreach (string command in DeviceProfile.Commands(profile.EndCommands))
					Line(command);
				return new ExportedJob { Bytes = Encoding.ASCII.GetBytes(text.ToString()), Moves = moves.AsReadOnly() };
			}
		}
	}
}
