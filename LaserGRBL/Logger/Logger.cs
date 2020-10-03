//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
	static class Logger
	{

		private static string LockString = "--- LOCK LOGGER CALL ---";
		private static string pid;

		public static void LogException(string context, Exception ex)
		{
			try
			{
				//System.Diagnostics.Debug.WriteLine(ex.ToString());
				LogMultiLine(context, ex.ToString());
			}
			catch { }
		}

		public static void LogMessage(string context, string format, params object[] args)
		{
			try
			{
				//System.Diagnostics.Debug.WriteLine(string.Format(format, args));
				LogMultiLine(context, string.Format(format, args));
			}
			catch { }
		}


		internal static void Start()
		{
			try
			{
				pid = System.Diagnostics.Process.GetCurrentProcess().Id.ToString("00000");
				if (System.IO.File.Exists(filename))
				{
					int MAXLINE = 1000;
					String tmp = System.IO.Path.GetTempFileName();
					bool written = false;

					using (System.IO.StreamReader reader = new System.IO.StreamReader(filename))
					{
						int linecount = 0;
						while (reader.ReadLine() != null)
							linecount++;

						reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

						if (linecount > MAXLINE)
						{
							int lines_to_delete = linecount - MAXLINE;
							using (System.IO.StreamWriter writer = new System.IO.StreamWriter(tmp))
							{
								string line;
								while (lines_to_delete-- > 0)
									reader.ReadLine();
								while ((line = reader.ReadLine()) != null)
									writer.WriteLine(line);
							}
							written = true;
						}

						reader.Close();
					}

					if (written)
					{
						System.IO.File.Delete(filename);
						System.IO.File.Move(tmp, filename);
					}
					else
					{
						System.IO.File.Delete(tmp);
					}
				}
			}
			catch { }


			Version current = typeof(GitHub).Assembly.GetName().Version;
            bool p64 = Tools.OSHelper.Is64BitProcess;
            bool o64 = Tools.OSHelper.Is64BitOperatingSystem;

            LogMultiLine("Program", String.Format("------- LaserGRBL v{0} [{1}{2}] START -------", current.ToString(3), p64 ? "64bit" : "32bit" , p64 != o64 ? "!" : ""));

        }

		internal static void Stop()
		{
			lock (LockString)
			{
				try
				{
					LogMultiLine("Program", "---------------- PROGRAM STOP -----------------");
                    System.IO.File.AppendAllText(filename, "\r\n");
				}
				catch { }
			}
		}

		private static void LogMultiLine(string context, string text)
		{
			lock (LockString)
			{
				try
				{
					DateTime dt = DateTime.Now;

					StringBuilder sb = new StringBuilder();
					foreach (string line in text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
						sb.AppendFormat("{0}.{1}\t{2}\t{3}\r\n", dt, dt.Millisecond, context.PadRight(12, ' '), line);//sb.AppendFormat("{0}.{1}\t{2}\t{3}\t{4}\r\n", dt, dt.Millisecond, pid, context.PadRight(12, ' '), line);

					System.IO.File.AppendAllText(filename, sb.ToString());
					System.Diagnostics.Debug.Write(sb.ToString());
				}
				catch { }
			}
		}

		static string filename
		{get{return System.IO.Path.Combine(GrblCore.DataPath, "sessionlog.txt");}}

		public static bool ExistLog
		{ get { return System.IO.File.Exists(filename); } }


		internal static void ShowLog()
		{
			System.Diagnostics.Process.Start(filename);
		}
	}
}
