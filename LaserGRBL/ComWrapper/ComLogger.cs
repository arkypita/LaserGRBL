using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL.ComWrapper
{
    public static class ComLogger
    {
		private static string LockString = "--- LOCK COMLOGGER CALL ---";
		private static int logcnt = 0;
		private static string mFileName = null;

		public static string FileName
		{
			get
			{
				return mFileName;
			}
			set
			{
				lock (LockString)
				{
					if (value != FileName)
					{
						if (mFileName != null && value == null)
							Log("log", $"Recording session stopped @ {DateTime.Now}");

						logcnt = 0;
						mFileName = value;

						if (mFileName != null)
							Log("log", $"Recording session started @ {DateTime.Now}");
					}
				}
			}
		}

		public static bool Enabled => FileName != null; 
		public static void Log(string operation, string line)
        {
			lock (LockString)
			{
				if (FileName != null)
				{
					try
					{
						line = line?.Replace("\r", "\\r");
						line = line?.Replace("\n", "\\n");
						line = string.Format("{0:00000}\t{1:00000000}\t{2}\t{3}\r\n", logcnt, Tools.TimingBase.TimeFromApplicationStartup().TotalMilliseconds, operation, line);
						System.Diagnostics.Debug.Write(line);
						System.IO.File.AppendAllText(FileName, line);
					}
					catch { }
					logcnt++;
				}
			}
        }

        internal static void Log(string operation, byte b)
        {
			if (Enabled)
				Log(operation, string.Format("[{0:X2}]", b));
        }

        internal static void Log(string operation, byte[] arr)
        {
			if (Enabled)
			{
				StringBuilder sb = new StringBuilder("[");

				for (int i = 0; i < arr.Length; i++)
				{
					sb.Append(string.Format("{0:X2}", arr[i]));
					if (i < arr.Length - 1) sb.Append(", ");
				}

				sb.Append("]");

				Log(operation, sb.ToString());
			}
        }
    }
}
