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
			srv = new WebSocketServer("ws://127.0.0.1:8181");
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
			protected override void OnClose(CloseEventArgs e)
			{
				Console.WriteLine("Connection lost!");
			}

			protected override void OnOpen()
			{
				Console.WriteLine("Client connected!");
				//Send("Connected");
				Send("Grbl v0.9emu ['$' for help]");
				SendStatus();
			}

			protected override void OnMessage(MessageEventArgs e)
			{
				if (e.IsBinary)
				{
					if (e.RawData.Length == 1)
					{
						if (e.RawData[0] == 63)
							SendStatus();
						else
							Console.WriteLine(BitConverter.ToString(e.RawData));
					}
					else
					{
						Console.WriteLine(BitConverter.ToString(e.RawData));
					}
				}
				else if (e.IsText)
				{
					Console.WriteLine(e.Data);
					System.Threading.Thread.Sleep(10);
					Send("OK");
				}
			}

			private void SendStatus()
			{
				Send("<Idle,MPos:0.000,0.000,0.000,WPos:0.000,0.000,0.000>");
			}
		}

	}
}
