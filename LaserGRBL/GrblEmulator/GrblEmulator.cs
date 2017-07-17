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

			private Tools.ThreadObject rxThread;
			private Tools.ThreadObject txThread;
			private Queue<string> rxBuf = new Queue<string>();
			private Queue<string> txBuf = new Queue<string>();

			public GrblWebSocketEmulator()
			{
				rxThread = new Tools.ThreadObject(ManageRX, 1, true, "WebSocket Emulator RX", null);
				txThread = new Tools.ThreadObject(ManageTX, 1, true, "WebSocket Emulator TX", null); 
			}

			protected override void OnClose(CloseEventArgs e)
			{
				lock (rxBuf)
				{
					Console.WriteLine("Connection lost!");
					rxThread.Stop();
					rxBuf.Clear();

					txThread.Stop();
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
					rxThread.Start();
					txThread.Start();
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
					{mPaused = true; SendStatus();}
				else if (e.Data == "~")
					{ mPaused = false; SendStatus(); }
				else if (e.RawData.Length == 1 && e.RawData[0] == 24)
					GrblReset();
				else
					EnqueueRX(e);
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
					if (mPaused)
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


							LaserGRBL.GrblCommand C = new GrblCommand(line);
							EmulateCommand(C);

							Console.WriteLine(C.Command.Trim("\n".ToCharArray()));

							EnqueueTX("ok");
						}
						catch (Exception ex)
						{
						}
					}
				}
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
		}



	}
}
