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
			ConsoleAPI.HaveConsole = true;
			srv = new WebSocketServer("ws://127.0.0.1:81");
			srv.AddWebSocketService<GrblWebSocketEmulator>("/");
			Console.WriteLine("Run Grbl emulator");
			srv.Start();
		}

		public static void Stop()
		{
			if (srv != null)
				srv.Stop();
		}


		private class GrblWebSocketEmulator : WebSocketBehavior
		{
			string s = "Idle";
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
					QueueManager.Start();
					SendConnected();
					SendVersion();
					SendStatus();
				}
			}

			private void SendConnected()
			{
				Send("Connected\r\n");
			}

			protected override void OnMessage(MessageEventArgs e)
			{
				if (e.IsBinary)
				{
					if (e.RawData.Length == 1)
					{
						if (e.RawData[0] == 24)
							GrblReset();
						else if (e.RawData[0] == 63)
							SendStatus();
						else
							PrintArray(e.RawData);
					}
					else
					{
						PrintArray(e.RawData);
					}
				}
				else if (e.IsText)
				{
					if (e.Data == "?")
						SendStatus();
					else if (e.Data == "version\n")
						;
					else if (e.Data == "{fb:n}\n")
						;
					else
						EnqueueCommand(e);
				}
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
					Console.Clear();
					Console.WriteLine("Grbl Reset");
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
			{ Send("Grbl 1.1f ['$' for help]\r\n"); }

			private void SendStatus()
			{Send(String.Format(System.Globalization.CultureInfo.InvariantCulture, "<{0}|MPos:{1:0.000},{2:0.000},{3:0.000}>\r\n", s,x,y,z));}

			private void ManageQueue()
			{
				lock (buffer)
				{
					if (buffer.Count > 0)
					{
						string line = buffer.Dequeue();


						LaserGRBL.GrblCommand C = new GrblCommand(line);

						if (C.IsAbsoluteCoord)
							absolute = true;
						else if (C.IsRelativeCoord)
							absolute = false;

						if (C.TrueMovement(x,y, absolute))
						{
							x = C.X != null ? C.X.Number : x;
							y = C.Y != null ? C.Y.Number : y;
							//z = C.Z != null ? C.Z.Number : z;
							System.Threading.Thread.Sleep(30);
						}

						Console.WriteLine(C.Command.Trim("\r\n".ToCharArray()));
						
						Send("OK\r\n");
					}
				}
			}

		}

	}
}
