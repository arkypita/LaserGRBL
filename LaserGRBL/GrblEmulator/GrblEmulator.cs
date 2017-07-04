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
			private bool mPaused;
			decimal x = 0.0M, y = 0.0M, z = 0.0M;
			bool absolute;

			private Tools.ThreadObject QueueManager;
			private Queue<string> buffer = new Queue<string>();

			public GrblWebSocketEmulator()
			{ QueueManager = new Tools.ThreadObject(ManageQueue, 1, true, "WebSocket Emulator", null); }

			protected override void OnClose(CloseEventArgs e)
			{
				lock (buffer)
				{
					Console.WriteLine("Connection lost!");
					QueueManager.Stop();
					buffer.Clear();
				}
			}

			protected override void OnOpen()
			{
				lock (buffer)
				{
					Console.WriteLine("Client connected!");
					buffer.Clear();
					mPaused = false;
					QueueManager.Start();
					SendConnected();
					//SendVersion();
					//SendStatus();
				}
			}

			private void SendConnected()
			{
				Send("Connected\n");
			}

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
					EnqueueCommand(e);
			}

			private void EnqueueCommand(MessageEventArgs e)
			{
				lock (buffer)
				{ buffer.Enqueue(e.Data); }
			}

			private void GrblReset()
			{
				lock (buffer)
				{
					buffer.Clear();
					System.Threading.Thread.Sleep(50);
					mPaused = false;
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
			{ Send("Grbl 1.1f ['$' for help]\n"); }

			private void SendStatus()
			{Send(String.Format(System.Globalization.CultureInfo.InvariantCulture, "<{0}|MPos:{1:0.000},{2:0.000},{3:0.000}>\n", Status ,x,y,z));}

			private string Status
			{
				get 
				{
					if (mPaused)
						return "Hold";
					else
						return "Idle";
				}
			}

			private void ManageQueue()
			{
				lock (buffer)
				{
					if (buffer.Count > 0 && !mPaused)
					{
						try
						{
							string line = buffer.Dequeue();


							LaserGRBL.GrblCommand C = new GrblCommand(line);

							if (C.IsAbsoluteCoord)
								absolute = true;
							else if (C.IsRelativeCoord)
								absolute = false;

							if (C.TrueMovement(x, y, absolute))
							{
								x = C.X != null ? C.X.Number : x;
								y = C.Y != null ? C.Y.Number : y;
								//z = C.Z != null ? C.Z.Number : z;
								System.Threading.Thread.Sleep(30);
							}

							Console.WriteLine(C.Command.Trim("\n".ToCharArray()));
							Send("ok\n");
						}
						catch (Exception ex)
						{
						}
					}
				}
			}

		}

	}
}
