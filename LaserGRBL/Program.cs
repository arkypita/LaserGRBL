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
		static void Main()
		{
			Logger.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);


			System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)Settings.GetObject("User Language", null);

			if (ci != null)
				System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

			Application.Run(new MainForm());

			Logger.Stop();
		}
	}
}
