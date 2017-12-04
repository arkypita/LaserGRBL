using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace LaserGRBL
{
	class GrblEmulator
	{
		private static WebSocketServer srv;

		public static void Start()
		{
			if (srv == null)
			{
				ConsoleAPI.HaveConsole = true;
				Console.WriteLine("ESP266 Grbl emulator (listening as ws://127.0.0.1:81/)");

				srv = new WebSocketServer("ws://127.0.0.1:81");
				srv.AddWebSocketService<GrblWebSocketEmulator>("/");
				srv.Start();
			}
		}

		public static void Stop()
		{
			if (srv != null)
			{
				srv.Stop();
				srv = null;
			}
		}


		private class GrblWebSocketEmulator : WebSocketBehavior
		{
			private bool mPaused = false;
			decimal curX = 0.0M, curY = 0.0M, curZ = 0.0M, speed = 0.0M;
			bool abs = true;
			TimeSpan toSleep = TimeSpan.Zero;
			private bool mCheck = false;

			private Tools.ThreadObject RX;
			private Tools.ThreadObject TX;
			private Queue<string> rxBuf = new Queue<string>();
			private Queue<string> txBuf = new Queue<string>();

			public GrblWebSocketEmulator()
			{
				RX = new Tools.ThreadObject(ManageRX, 0, true, "WebSocket Emulator RX", null);
				TX = new Tools.ThreadObject(ManageTX, 0, true, "WebSocket Emulator TX", null); 
			}

			protected override void OnClose(CloseEventArgs e)
			{
				lock (rxBuf)
				{
					Console.WriteLine("Connection lost!");
					RX.Stop();
					rxBuf.Clear();

					TX.Stop();
					txBuf.Clear();
				}
			}

			protected override void OnOpen()
			{
				lock (rxBuf)
				{
					Console.WriteLine("Client connected!");
					rxBuf.Clear();
					txBuf.Clear();
					mPaused = false;
					mCheck = false;
					RX.Start();
					TX.Start();
					SendConnected();
					//SendVersion();
					//SendStatus();
				}
			}

			private void SendConnected()
			{ImmediateTX("Connected");}

			protected override void OnMessage(MessageEventArgs e)
			{
				if (e.Data == "?")
					SendStatus();
				else if (e.Data == "version\n")
					;
				else if (e.Data == "{fb:n}\n")
					;
				else if (e.Data == "!")
				{ mPaused = true; SendStatus(); }
				else if (e.Data == "~")
				{ mPaused = false; SendStatus(); }
				else if (e.RawData.Length == 1 && e.RawData[0] == 24)
					GrblReset();
				else
					EnqueueRX(e);
			}

			static System.Text.RegularExpressions.Regex confRegEX = new System.Text.RegularExpressions.Regex(@"^[$](\d+) *= *(\d+\.?\d*)");
			private bool IsSetConf(string p)
			{return confRegEX.IsMatch(p);}

			private void SetConfig(string p)
			{
				try
				{
					System.Threading.Thread.Sleep(10);
					System.Text.RegularExpressions.MatchCollection matches = confRegEX.Matches(p);
					int key = int.Parse(matches[0].Groups[1].Value);

					if (configTable.Keys.Contains(key))
					{
						if (configTable[key] is int)
							configTable[key] = int.Parse(matches[0].Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);
						else if (configTable[key] is decimal)
							configTable[key] = decimal.Parse(matches[0].Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);
						else if (configTable[key] is bool)
							configTable[key] = int.Parse(matches[0].Groups[2].Value) == 0 ? false : true;

						EnqueueTX("ok");
					}
					else
						EnqueueTX("error"); 
				}
				catch(Exception ex)
				{
					EnqueueTX("error"); 
				}
			}

			private Dictionary<int, object> configTable = new Dictionary<int, object> { { 0, 10 }, { 1, 25 }, { 2, 0 }, { 3, 0 }, { 4, false }, { 5, false }, { 6, false }, { 10, 1 }, { 11, 0.010m }, { 12, 0.002m }, { 13, false }, { 20, false }, { 21, false }, { 22, false }, { 23, 0 }, { 24, 25.000m }, { 25, 500.000m }, { 26, 250 }, { 27, 1.000m }, { 30, 1000.0m }, { 31, 0.0m }, { 32, false }, { 100, 250.000m }, { 101, 250.000m }, { 102, 250.000m }, { 110, 500.000m }, { 111, 500.000m }, { 112, 500.000m }, { 120, 10.000m }, { 121, 10.000m }, { 122, 10.000m }, { 130, 200.000m }, { 131, 200.000m }, { 132, 200.000m } };

			private void SendConfig()
			{
				EnqueueTX("ok"); //REPLY TO $$
				foreach (KeyValuePair<int, object> kvp in configTable)
				{
					if (kvp.Value is decimal)
						ImmediateTX(string.Format(System.Globalization.CultureInfo.InvariantCulture, "${0}={1:0.000}", kvp.Key, kvp.Value));
					else if (kvp.Value is bool)
						ImmediateTX(string.Format(System.Globalization.CultureInfo.InvariantCulture, "${0}={1}", kvp.Key, ((bool)kvp.Value) ? 1 : 0));
					else
						ImmediateTX(string.Format(System.Globalization.CultureInfo.InvariantCulture, "${0}={1}", kvp.Key, kvp.Value));
				}
			}

			private void EnqueueRX(MessageEventArgs e)
			{
				lock (rxBuf)
				{ rxBuf.Enqueue(e.Data); }
			}

			private void GrblReset()
			{
				lock (rxBuf)
				{
					rxBuf.Clear();
					System.Threading.Thread.Sleep(50);
					
					mPaused = false;
					curX = curY = curZ = speed = 0.0M;
					abs = true;
					toSleep = TimeSpan.Zero;


					Console.Clear();
					Console.WriteLine("Grbl Reset");
					SendVersion();
				}
			}

			private void PrintArray(byte[] p)
			{
				StringBuilder sb = new StringBuilder("B: ");
				foreach (byte b in p)
				{
					sb.Append(b.ToString());
					sb.Append(" ");
				}
				Console.WriteLine(sb.ToString().Trim());
			}

			private void SendVersion()
			{ ImmediateTX("Grbl 1.1f ['$' for help]"); }

			private void SendStatus()
			{ ImmediateTX(String.Format(System.Globalization.CultureInfo.InvariantCulture, "<{0}|MPos:{1:0.000},{2:0.000},{3:0.000}>\n", Status, curX, curY, curZ)); }

			private string Status
			{
				get 
				{
					if (mCheck)
						return "Check";
					else if (mPaused)
						return "Hold";
					else if (rxBuf.Count > 0)
						return "Run";
					else
						return "Idle";
				}
			}

			private void ManageRX()
			{
				lock (rxBuf)
				{
					if (rxBuf.Count > 0 && !mPaused)
					{
						try
						{
							string line = rxBuf.Dequeue();

							if (line == "$$\n")
								SendConfig();
							else if (IsSetConf(line))
								SetConfig(line);
							else if (line == "$C\n")
								SwapCheck();
							else if (line == "$H\n")
								EmulateHoming();
							else
								EmulateCommand(new GrblCommand(line));
							
							Console.WriteLine(line.Trim("\n".ToCharArray()));
						}
						catch (Exception ex)
						{
						}
					}

					RX.SleepTime = rxBuf.Count > 0 ? 0 : 1;
				}
			}

			private void EmulateHoming()
			{
				System.Threading.Thread.Sleep(2000);
				curX = curY = curZ = 0.0M;
				EnqueueTX("ok");
			}

			private void SwapCheck()
			{
				mCheck = !mCheck;
				EnqueueTX("ok");
				EnqueueTX(mCheck ? "[Enabled]" : "[Disabled]");
				
				if (!mCheck)
					GrblReset();
			}

			private void ManageTX()
			{
				lock (txBuf)
				{
					if (txBuf.Count > 0)
					{
						try
						{
							string line = txBuf.Dequeue();
							Send(line);
						}
						catch (Exception ex)
						{
						}
					}

					TX.SleepTime = txBuf.Count > 0 ? 0 : 1;
				}
			}

			private void EnqueueTX(String response)
			{
				lock (txBuf)
				{ txBuf.Enqueue(response + "\n"); }
			}

			private void ImmediateTX(String response)
			{
				lock (txBuf)
				{ Send(response + "\n"); }
			}

			private void EmulateCommand(GrblCommand cmd)
			{
				if (!mCheck)
				{
					try
					{
						cmd.BuildHelper();

						if (cmd.IsRelativeCoord)
							abs = false;
						if (cmd.IsAbsoluteCoord)
							abs = true;

						if (cmd.F != null)
							speed = cmd.F.Number;

						decimal newX = cmd.X != null ? (abs ? cmd.X.Number : curX + cmd.X.Number) : curX;
						decimal newY = cmd.Y != null ? (abs ? cmd.Y.Number : curY + cmd.Y.Number) : curY;

						decimal distance = 0;

						if (cmd.IsLinearMovement)
							distance = Tools.MathHelper.LinearDistance(curX, curY, newX, newY);
						else if (cmd.IsArcMovement) //arc of given radius
							distance = Tools.MathHelper.ArcDistance(curX, curY, newX, newY, cmd.GetArcRadius());

						if (distance != 0 && speed != 0)
							toSleep += TimeSpan.FromMinutes((double)distance / (double)speed);

						if (toSleep.TotalMilliseconds > 15) //execute sleep
						{
							long start = Tools.HiResTimer.TotalNano;
							System.Threading.Thread.Sleep(toSleep);
							long stop = Tools.HiResTimer.TotalNano;

							toSleep -= TimeSpan.FromMilliseconds((double)(stop - start) / 1000.0 / 1000.0);
						}

						curX = newX;
						curY = newY;
					}
					catch (Exception ex) { throw ex; }
					finally { cmd.DeleteHelper(); }
				}

				EnqueueTX("ok");
			}
		}



	}
}
