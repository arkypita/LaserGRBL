using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
	class Logger
	{

		private static string LockString = "--- LOCK LOGGER CALL ---";

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
				int MAXLINE = 1000;
				String tmp = System.IO.Path.GetTempFileName();
				bool written = false;

				using (System.IO.StreamReader reader = new System.IO.StreamReader("sessionlog.txt"))
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
				}

				if (written)
				{
					System.IO.File.Delete("sessionlog.txt");
					System.IO.File.Move(tmp, "sessionlog.txt");
				}
				else
				{
					System.IO.File.Delete(tmp); 
				}
			}
			catch { }



			LogMultiLine("Program", "------------ PROGRAM START ------------");
		}

		internal static void Stop()
		{
			lock (LockString)
			{
				try
				{
					LogMultiLine("Program", "------------ PROGRAM STOP ------------");
					System.IO.File.AppendAllText("sessionlog.txt", "\r\n");
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
						sb.AppendFormat("{0}\t{1}\t{2}\r\n", dt, context.PadRight(12, ' '), line);

					System.IO.File.AppendAllText("sessionlog.txt", sb.ToString());
				}
				catch { }
			}
		}


	}
}
