using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace LaserGRBL.GrblEmulator
{
	class WebSocketEmulator
	{
		private static WebSocketServer srv;

		public static void Start()
		{
			if (srv == null)
			{
				EmulatorUI.ShowUI("ESP266 Grbl emulator (listening as ws://127.0.0.1:81/)");
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

				EmulatorUI.HideUI();
			}
		}


		private class GrblWebSocketEmulator : WebSocketBehavior
		{
			private Grblv11Emulator emu;
			public GrblWebSocketEmulator()
			{
				emu = new Grblv11Emulator(SendData);
			}

			private void SendData(string message)
			{
				Send(message);
			}

			protected override void OnClose(CloseEventArgs e)
			{
				emu.CloseCom();
			}

			protected override void OnOpen()
			{
				emu.OpenCom();
			}

			protected override void OnMessage(MessageEventArgs e)
			{
				emu.ManageMessage(e.RawData);
			}


		}



	}
}
