using System;
using System.Windows.Forms;

namespace LaserGRBL
{
	static class Program
	{
		/// <summary>
		/// Punto di ingresso principale dell'applicazione.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Logger.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)Settings.GetObject("User Language", null);
			if (ci != null) System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

			foreach (string arg in args)
			{
				if (arg == "WsEmu")
					LaserGRBL.GrblEmulator.Start();
			}

			Application.Run(new MainForm());

			LaserGRBL.GrblEmulator.Stop();

			Logger.Stop();
		}
	}
}
