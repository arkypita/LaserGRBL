﻿//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System;

namespace LaserGRBL
{
    public class Autotrace
	{
		public static string AutotracePath = null;
		public static string TempPath { get { return Path.Combine(Settings.DataPath, "Autotrace"); } }

		public static void CleanupTmpFolder()
		{
			try
			{
				if (Directory.Exists(TempPath))
					Directory.Delete(TempPath, true);
			}
			catch { }
		}

		public static object BitmapToSvgDocument(Bitmap bmp, bool uct, int ct, bool ult, int lt)
		{
			throw new NotImplementedException();
			//string content = BitmapToSvgString(bmp, uct, ct, ult, lt);
			//return content != null ? Svg.SvgDocument.FromSvg<Svg.SvgDocument>(content) : new Svg.SvgDocument();
		}

		public static string BitmapToSvgString(Bitmap bmp, bool uct, int ct, bool ult, int lt)
		{
			if (!Directory.Exists(TempPath))
				 Directory.CreateDirectory(TempPath);

			string fname = $"{TempPath}{Path.GetRandomFileName()}";

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
					if (File.Exists($"{fname}.png"))
						File.Delete($"{fname}.png");
				}
				catch { }
			}
		}

		public static string ExecuteAutotrace(string args)
		{
			ProcessStartInfo procStartInfo = new ProcessStartInfo();

			if (AutotracePath == null)
			{
				// Use the old way, executable in LGRBL folder
				var currDir = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "Autotrace");
				procStartInfo.FileName = Path.Combine(currDir, "autotrace.exe");
			}
			else
			{
				// Use specified location
				procStartInfo.FileName = AutotracePath;
			}

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
