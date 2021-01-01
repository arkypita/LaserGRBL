//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

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
