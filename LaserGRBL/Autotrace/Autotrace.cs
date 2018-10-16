using System;
using System.Drawing;

namespace LaserGRBL
{
	public class Autotrace
	{
		static System.Text.RegularExpressions.Regex colorRegex = new System.Text.RegularExpressions.Regex("stroke:#([0-9a-fA-F]+);", System.Text.RegularExpressions.RegexOptions.Compiled);

		public static Svg.SvgDocument BitmapToSVG(Bitmap bmp, Color color, bool uct, int ct, bool ult, int lt)
		{
			if (!System.IO.Directory.Exists(".//Autotrace//TempFolder//"))
				System.IO.Directory.CreateDirectory(".//Autotrace//TempFolder//");

			string fname = $".//Autotrace//TempFolder//{System.IO.Path.GetRandomFileName()}";

			try
			{
				bmp.Save($"{fname}.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

				string command = ".//Autotrace//autotrace.exe";
				string param = $"-output-fi {fname}.svg -output-fo svg -centerline -despeckle-l 20 -despeckle-t 0.1";

				if (uct)
					param += " -corner-t " + ct.ToString(System.Globalization.CultureInfo.InvariantCulture);
				if (ult)
					param += " -line-t " + (lt / 10.0).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);

				param += $" {fname}.bmp";


				Tools.CommandLine.ExecuteCommand(command, param);

				string fcontent = System.IO.File.ReadAllText($"{fname}.svg");
				fcontent = colorRegex.Replace(fcontent, $"stroke:#{ToHexString(color)};");

				return Svg.SvgDocument.FromSvg<Svg.SvgDocument>(fcontent);
			}
			catch
			{
				return new Svg.SvgDocument();
			}
			finally
			{
				try
				{
					if (System.IO.File.Exists($"{fname}.bmp"))
						System.IO.File.Delete($"{fname}.bmp");
				}
				catch { }
				try
				{
					if (System.IO.File.Exists($"{fname}.svg"))
						System.IO.File.Delete($"{fname}.svg");
				}
				catch { }
			}
		}

		private static string ToHexString(Color c)
		{
			return $"{c.R:X2}{c.G:X2}{c.B:X2}";
		}
	}
}
