//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LaserGRBL
{
	static class Logger
	{
		private static Queue<string> Q;
		private static AutoResetEvent EV;
		private static Thread TH;
		private static string PID;
		private static bool Exit = false;

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
			try
			{
				RotateLog();
				Q = new Queue<string>();
				EV = new AutoResetEvent(false);
				TH = new Thread(DoTheWork);
				PID = System.Diagnostics.Process.GetCurrentProcess().Id.ToString("00000");
				TH.Start();
			}
			catch { }


			Version current = typeof(GitHub).Assembly.GetName().Version;
            bool p64 = Tools.OSHelper.Is64BitProcess;
            bool o64 = Tools.OSHelper.Is64BitOperatingSystem;

            LogMultiLine("Program", String.Format("------- LaserGRBL v{0} [{1}{2}] START -------", current.ToString(3), p64 ? "64bit" : "32bit" , p64 != o64 ? "!" : ""));
        }

		private static void RotateLog()
		{
			try
			{
				if (System.IO.File.Exists(filename))
				{
					int MAXLINE = 1000;
					String tmp = System.IO.Path.GetTempFileName();
					bool written = false;

					using (System.IO.StreamReader reader = new System.IO.StreamReader(new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None)))
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
						if (System.IO.File.Exists(tmp))
							System.IO.File.Delete(tmp);
					}
				}
			}
			catch { }
		}

		internal static void Stop()
		{
			try
			{
				Exit = true;
				LogMultiLine("Program", "---------------- PROGRAM STOP -----------------");
                Enqueue("\r\n");
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
					sb.AppendFormat("{0}.{1}\t{2}\t{3}\r\n", dt, dt.Millisecond, context.PadRight(12, ' '), line);//sb.AppendFormat("{0}.{1}\t{2}\t{3}\t{4}\r\n", dt, dt.Millisecond, pid, context.PadRight(12, ' '), line);

				Enqueue(sb.ToString());
			}
			catch { }
		}

		private static void Enqueue(string s)
		{
			lock (Q)
			{
				if (Q.Count < 100)
					Q.Enqueue(s);	//evita l'aggiunta di elementi se la coda è piena... al peggio non verranno scritti nel log
			}
			
			EV.Set();
		}

		private static void DoTheWork()
		{
			while(!Exit)
			{
				EV.WaitOne();

				while(Q.Count > 0)
				{
					try
					{
						string s;

						lock (Q) { s = Q.Peek(); }

						System.IO.File.AppendAllText(filename, s);
						System.Diagnostics.Debug.Write(s);

						lock (Q){ Q.Dequeue(); }
					}
					catch { Thread.Sleep(1); }
				}

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
