//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Threading;
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
			ExceptionManager.RegisterHandler();
			Tools.TimingBase.TimeFromApplicationStartup();

			Logger.Start();
			GitHub.InitUpdate();
			UsageStats.LoadFile();
			CustomButtons.LoadFile();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			System.Globalization.CultureInfo ci = Settings.GetObject<System.Globalization.CultureInfo>("User Language", null);
			if (ci != null) Thread.CurrentThread.CurrentUICulture = ci;
			Tools.TaskScheduler.SetClockResolution(1); //use a fast clock
			
			Application.Run(new MainForm(args));
			
			GrblEmulator.WebSocketEmulator.Stop();
			Autotrace.CleanupTmpFolder();

			ComWrapper.ComLogger.StopLog();
			Logger.Stop();

		}
	}
}
