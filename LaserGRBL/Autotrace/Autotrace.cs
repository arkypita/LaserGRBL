using System;
using System.Drawing;
using System.Diagnostics;

namespace LaserGRBL
{
	public class Autotrace
	{
		public static string TempPath { get => $"{GrblCore.DataPath}\\Autotrace\\"; }

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

			procStartInfo.FileName = ".\\Autotrace\\autotrace.exe";
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
