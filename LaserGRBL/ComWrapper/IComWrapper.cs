using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL.ComWrapper
{
	public enum WrapperType
	{ UsbSerial, Ethernet }

	interface IComWrapper
	{
		void Configure(params object[] param);

		void Open();
		void Close(bool auto);

		bool IsOpen { get; }
		
		void Write(byte b);
		void WriteLine(string text);

		string ReadLine();
	}
}
