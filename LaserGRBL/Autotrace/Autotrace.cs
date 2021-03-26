//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Drawing;
using System.Diagnostics;

namespace LaserGRBL
{
	public class Autotrace
	{
		public static string TempPath { get { return $"{GrblCore.DataPath}\\Autotrace\\"; } }

		public static void CleanupTmpFolder()
		{
			try
			{
				if (System.IO.Directory.Exists(TempPath))
					System.IO.Directory.Delete(TempPath, true);
			}
			catch { }
		}

		public static Svg.SvgDocument BitmapToSvgDocument(Bitmap bmp, bool uct, int ct, bool ult, int lt)
		{
			string content = BitmapToSvgString(bmp, uct, ct, ult, lt);
			return content != null ? Svg.SvgDocument.FromSvg<Svg.SvgDocument>(content) : new Svg.SvgDocument();
		}

		public static string BitmapToSvgString(Bitmap bmp, bool uct, int ct, bool ult, int lt)
		{
			if (!System.IO.Directory.Exists(TempPath))
				System.IO.Directory.CreateDirectory(TempPath);

			string fname = $"{TempPath}{System.IO.Path.GetRandomFileName()}";

			try
			{
				bmp.Save($"{fname}.png", System.Drawing.Imaging.ImageFormat.Png);

				string param = $"-output-fo svg -background-color FFFFFF -centerline";

				if (uct) param += " -corner-t " + ct.ToString(System.Globalization.CultureInfo.InvariantCulture);
				if (ult) param += " -line-t " + (lt / 10.0).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);

				param += $" \"{fname}.png\"";

				return ExecuteAutotrace(param);
			}
			finally
			{
				try
				{
					if (System.IO.File.Exists($"{fname}.png"))
						System.IO.File.Delete($"{fname}.png");
				}
				catch { }
			}
		}

		private static string ToHexString(Color c)
		{
			return $"{c.R:X2}{c.G:X2}{c.B:X2}";
		}

		public static string ExecuteAutotrace(string args)
		{
			ProcessStartInfo procStartInfo = new ProcessStartInfo();

			procStartInfo.FileName = System.IO.Path.Combine(GrblCore.ExePath, "Autotrace\\autotrace.exe");
			procStartInfo.Arguments = args;
			procStartInfo.RedirectStandardOutput = true;
			procStartInfo.UseShellExecute = false;
			procStartInfo.CreateNoWindow = true;

			using (Process process = new Process())
			{
				process.StartInfo = procStartInfo;
				process.Start();
				string result = process.StandardOutput.ReadToEnd();
				process.WaitForExit();
				return result;
			}
		}

	}
}
