//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL.ComWrapper
{
	public enum WrapperType
	{ UsbSerial, UsbSerial2, Telnet, LaserWebESP8266, Emulator }

	public interface IComWrapper
	{
		void Configure(params object[] param);

		void Open();
		void Close(bool auto);

		bool IsOpen { get; }
		
		void Write(byte b);
        void Write(byte[] arr);
        void Write(string text);

		string ReadLineBlocking();

		bool HasData();
	}
}
