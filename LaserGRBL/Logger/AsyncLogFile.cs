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
	public class AsyncLogFile
	{
		private Queue<string> Q;
		private AutoResetEvent EV;
		private Thread TH;
		private string PID;
		private bool Exit = false;
		private string mFilename;

		public AsyncLogFile(string filename, int maxline = int.MaxValue)
		{
			mFilename = filename;
			Start(maxline);
		}

		private void Start(int maxline)
		{
			try
			{
				RotateLog(maxline);
				Q = new Queue<string>();
				EV = new AutoResetEvent(false);
				TH = new Thread(DoTheWork);
				PID = System.Diagnostics.Process.GetCurrentProcess().Id.ToString("00000");
				TH.Name = $"AsyncLog {mFilename}";
				TH.Start();
			}
			catch { }
		}

		private void RotateLog(int maxline)
		{
			try
			{
				if (System.IO.File.Exists(mFilename) && maxline != int.MaxValue)
				{
					String tmp = System.IO.Path.GetTempFileName();
					bool written = false;

					using (System.IO.StreamReader reader = new System.IO.StreamReader(new System.IO.FileStream(mFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None)))
					{
						int linecount = 0;
						while (reader.ReadLine() != null)
							linecount++;

						reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

						if (linecount > maxline)
						{
							int lines_to_delete = linecount - maxline;
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
						System.IO.File.Delete(mFilename);
						System.IO.File.Move(tmp, mFilename);
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

		internal void Stop()
		{
			Exit = true;
			EV.Set();
		}

		public void Log(string text)
		{
			try { Enqueue(text); }
			catch { }
		}

		private void Enqueue(string s)
		{
			lock (Q)
			{
				if (Q.Count < 10000)
					Q.Enqueue(s);   //evita l'aggiunta di elementi se la coda è piena... al peggio non verranno scritti nel log
			}

			EV.Set();
		}

		private void DoTheWork()
		{
			while (!Exit)
			{
				EV.WaitOne();

				while (Q.Count > 0)
				{
					try
					{
						string s;

						lock (Q) { s = Q.Peek(); }

						System.IO.File.AppendAllText(mFilename, s);
						System.Diagnostics.Debug.Write(s);

						lock (Q) { Q.Dequeue(); }
					}
					catch { Thread.Sleep(1); }
				}

			}
		}

		public bool ExistLog
		{ get { return System.IO.File.Exists(mFilename); } }

		public void ShowLog()
		{System.Diagnostics.Process.Start(mFilename);}

	}
}
