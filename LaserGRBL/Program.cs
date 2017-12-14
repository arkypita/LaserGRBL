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
			Tools.TimingBase.TimeFromApplicationStartup();

			Logger.Start();
			GitHub.CleanupOldVersion();
			UsageStats.LoadFile();
			CustomButtons.LoadFile();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)Settings.GetObject("User Language", null);
			if (ci != null)
				System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

			Tools.TaskScheduler.SetClockResolution(1); //use a fast clock
			Application.Run(new MainForm());
			LaserGRBL.GrblEmulator.WebSocketEmulator.Stop();
			Logger.Stop();
		}

	}
}
