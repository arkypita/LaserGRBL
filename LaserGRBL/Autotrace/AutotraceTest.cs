//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace LaserGRBL
{
	public partial class AutotraceTest : Form
	{
		public AutotraceTest()
		{
			InitializeComponent();
			System.Threading.Thread thread = new System.Threading.Thread(DoTheWork);
			thread.Start();
			CheckForIllegalCrossThreadCalls = false;
		}

		private Image source;

		private delegate void SetImage(Bitmap bmp);

		private bool exiting = false;
		private bool refresh = false;
		private void DoTheWork()
		{
			while (!exiting)
			{
				if (source != null && refresh)
				{
					refresh = false;
					Bitmap clone = (Bitmap)source.Clone();
					PreviewCenterline(clone);

					BeginInvoke(new SetImage(DoSetImage), clone);
				}
				else
				{
					System.Threading.Thread.Sleep(10);
				}
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			exiting = true;
			base.OnClosing(e);
		}

		private void DoSetImage(Bitmap bmp)
		{
			pictureBox2.Image = bmp;
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();

			System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.DialogResult.Cancel;
			try
			{
				dialogResult = ofd.ShowDialog(this);
			}
			catch (System.Runtime.InteropServices.COMException)
			{
				ofd.AutoUpgradeEnabled = false;
				dialogResult = ofd.ShowDialog(this);
			}

			if (dialogResult == DialogResult.OK)
			{
				source = Bitmap.FromFile(ofd.FileName);
				pictureBox1.Image = (Image)source.Clone();
				refresh = true;
			}
		}

		private void TrackChange(object sender, EventArgs e)
		{
			refresh = true;
		}

		System.Text.RegularExpressions.Regex colorRegex = new System.Text.RegularExpressions.Regex("stroke:#([0-9a-fA-F]+);", System.Text.RegularExpressions.RegexOptions.Compiled);
		private void PreviewCenterline(Bitmap bmp)
		{

			if (!System.IO.Directory.Exists(".//Autotrace//TempFolder//"))
				System.IO.Directory.CreateDirectory(".//Autotrace//TempFolder//");

			string fname = $".//Autotrace//TempFolder//{System.IO.Path.GetRandomFileName()}";

			try
			{
				bmp.Save($"{fname}.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

				System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
				nfi.NumberDecimalSeparator = ".";

				string dst = (despeckletightness.Value / 10.0).ToString("0.00", nfi);
				string ieth = (trackBar6.Value / 10.0).ToString("0.00", nfi);
				string liner = (linereversionthreshold.Value / 10.0).ToString("0.00", nfi);
				string linet = (linethreshold.Value / 10.0).ToString("0.00", nfi);

				string command = ".//Autotrace//autotrace.exe";
				string rparam = $"-corner-a {corneralwaysthreshold.Value} -corner-t {cornerthreshold.Value} -corner-s {cornersurround.Value} -despeckle-l {despecklelevel.Value} -despeckle-t {dst} -filter-i {filteriterations.Value} -line-r {liner} -line-t {linet} -tangent-s {tangentsurround.Value}";

				if (preservewidth.Checked)
					rparam = rparam + " -preserve-width";
				if (removeadjacentcorners.Checked)
					rparam = rparam + " -remove-adjacent-corners";

				string param = $"-output-fi {fname}.svg -output-fo svg -centerline {rparam} {fname}.bmp";
				label1.Text = rparam;
				//Debug.WriteLine(param);
				ExecuteCommand(command, param);

				string fcontent = System.IO.File.ReadAllText($"{fname}.svg");
				fcontent = colorRegex.Replace(fcontent, "stroke:#FF0000;");

				Svg.SvgDocument svg = Svg.SvgDocument.FromSvg<Svg.SvgDocument>(fcontent);

				using (Graphics g = Graphics.FromImage(bmp))
				{
					g.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.White)), g.ClipBounds);

					svg.Draw(g);
				}
			}
			catch(Exception)
			{


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

		private bool ExecuteCommand(string exeDir, string args)
		{
			try
			{
				ProcessStartInfo procStartInfo = new ProcessStartInfo();

				procStartInfo.FileName = exeDir;
				procStartInfo.Arguments = args;
				procStartInfo.RedirectStandardOutput = true;
				procStartInfo.UseShellExecute = false;
				procStartInfo.CreateNoWindow = true;

				using (Process process = new Process())
				{
					process.StartInfo = procStartInfo;

					//System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
					//stopwatch.Start();
					process.Start();
					process.WaitForExit();
					//System.Diagnostics.Debug.WriteLine(stopwatch.ElapsedMilliseconds);

					string result = process.StandardOutput.ReadToEnd();
					Console.WriteLine(result);
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("*** Error occured executing the following commands.");
				Console.WriteLine(exeDir);
				Console.WriteLine(args);
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		private void CheckedChanged(object sender, EventArgs e)
		{
			refresh = true;
		}
	}
}
