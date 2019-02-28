using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL.ComWrapper
{
    public class ComLogger
    {
        private string mFileName;
        int logcnt = 0;

        public ComLogger(string filename)
        {
            mFileName = System.IO.Path.Combine(GrblCore.DataPath, filename);
        }

        public void Log(string operation, string line)
        {
            if (GrblCore.WriteComLog)
            {
                try
                {
                    line = line?.Replace("\r", "\\r");
                    line = line?.Replace("\n", "\\n");
                    line = string.Format("{0:00000000}\t{1:00000}\t{2}\t{3}\r\n", Tools.TimingBase.TimeFromApplicationStartup().TotalMilliseconds, logcnt, operation, line);
                    System.Diagnostics.Debug.Write(line);
                    System.IO.File.AppendAllText(mFileName, line); 
                }
                catch { }
                logcnt++;
            }
        }

        internal void Log(string operation, byte b)
        {
            if (GrblCore.WriteComLog)
            {
                Log(operation, string.Format("[{0:X2}]", b));
            }
        }

        internal void Log(string operation, byte[] arr)
        {
            if (GrblCore.WriteComLog)
            {
                StringBuilder sb = new StringBuilder("[");

                for (int i = 0; i < arr.Length; i++)
                {
                    sb.Append(string.Format("{0:X2}", arr[i]));
                    if (i < arr.Length-1) sb.Append(", ");
                }

                sb.Append("]");

                Log(operation, sb.ToString());
            }
        }
    }
}
