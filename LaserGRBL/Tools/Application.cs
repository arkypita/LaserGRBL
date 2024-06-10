using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
	public static class Application2
	{
		public static void RestartNoCommandLine()
		{
			ProcessStartInfo startInfo = Process.GetCurrentProcess().StartInfo;
			startInfo.FileName = Application.ExecutablePath;
			var exit = typeof(Application).GetMethod("ExitInternal",
								System.Reflection.BindingFlags.NonPublic |
								System.Reflection.BindingFlags.Static);
			exit.Invoke(null, null);
			Process.Start(startInfo);
		}
	}
}
