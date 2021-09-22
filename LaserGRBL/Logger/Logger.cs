//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Text;

namespace LaserGRBL
{
	static class Logger
	{
		private static AsyncLogFile file = new AsyncLogFile(System.IO.Path.Combine(GrblCore.DataPath, "sessionlog.txt"), 1000);

		public static void LogException(string context, Exception ex)
		{
			try { LogMultiLine(context, ex.ToString()); }
			catch { }
		}

		public static void LogMessage(string context, string format, params object[] args)
		{
			try { LogMultiLine(context, string.Format(format, args)); }
			catch { }
		}

		internal static void Start()
		{
            bool p64 = Tools.OSHelper.Is64BitProcess;
            bool o64 = Tools.OSHelper.Is64BitOperatingSystem;

            LogMultiLine("Program", String.Format("------- LaserGRBL v{0} [{1}{2}] START -------", Program.CurrentVersion.ToString(3), p64 ? "64bit" : "32bit" , p64 != o64 ? "!" : ""));
        }
		
		internal static void Stop()
		{
			try
			{
				LogMultiLine("Program", "---------------- PROGRAM STOP -----------------");
				Log("\r\n");
				file.Stop();
			}
			catch { }
		}

		private static void LogMultiLine(string context, string text)
		{
			try
			{
				DateTime dt = DateTime.Now;

				StringBuilder sb = new StringBuilder();
				foreach (string line in text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
					sb.AppendFormat("{0:dd/MM/yyyy HH:mm:ss.fff}\t{1}\t{2}\r\n", dt, context.PadRight(12, ' '), line);//sb.AppendFormat("{0}.{1}\t{2}\t{3}\t{4}\r\n", dt, dt.Millisecond, pid, context.PadRight(12, ' '), line);

				Log(sb.ToString());
			}
			catch { }
		}

		private static void Log(string s)
		{
			file.Log(s);
		}

		public static bool ExistLog
		{ get { return file.ExistLog; } }


		internal static void ShowLog()
		{
			file.ShowLog();
		}
	}
}
