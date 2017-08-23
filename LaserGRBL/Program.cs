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
			if (System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath).ToLower() == GitHub.UpdaterExeName.ToLower())
				RunAsUpdater(args);
			else
				RunNormally();
		}

		private static void RunAsUpdater(string[] args)
		{
			if (args.Length < 3 || args[0] != "AU") //check for correct call (AU PID EXEPATH)
				return;

			if (!IsRunAsAdmin())
			{
				System.Windows.Forms.MessageBox.Show("Update require administrator privilege!", "Cannot update LaserGRBL", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			GitHub.CleanupOldVersion();

			int PID = int.Parse(args[1]);
			string lasergrblpath = args[2];

			try
			{
				System.Diagnostics.Process P = System.Diagnostics.Process.GetProcessById(PID);
				if (P != null && !P.WaitForExit(1000)) //wait for process exit
					P.Kill();
			}
			catch { }


			try
			{
				foreach (System.Diagnostics.Process Pn in System.Diagnostics.Process.GetProcessesByName("LaserGRBL")) //if other instance - kill them!
				{
					if (Pn.Id != System.Diagnostics.Process.GetCurrentProcess().Id) //do not kill myself!!
					{
						try { Pn.Kill(); }
						catch { }
					}
				}
			}
			catch { }

			if (GitHub.ApplyUpdateS2()) //at this moment LaserGRBL is not running!
				System.Windows.Forms.MessageBox.Show("Update success!", "Update result", MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
				System.Windows.Forms.MessageBox.Show("Automatic update failed!\r\nPlease manually download the new version from lasergrbl site.", "Update result", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

			if (System.IO.File.Exists(lasergrblpath))
			{
				System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo { UseShellExecute = true, WorkingDirectory = Environment.CurrentDirectory, FileName = lasergrblpath };
				try { System.Diagnostics.Process.Start(p); }
				catch { }
			}
		}

		private static void RunNormally()
		{
			Logger.Start();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)Settings.GetObject("User Language", null);
			if (ci != null) System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

			Application.Run(new MainForm());

			LaserGRBL.GrblEmulator.Stop();
			Logger.Stop();
		}


		private static bool IsRunAsAdmin()
		{
			var Principle = new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent());
			return Principle.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
		}
	}
}
