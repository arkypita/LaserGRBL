using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;  

namespace LaserGRBL
{
	class ConsoleAPI
	{
		[DllImport("kernel32.dll")] private static extern int AllocConsole();

		[DllImport("kernel32.dll")] private static extern int FreeConsole();

		internal const UInt32 SC_CLOSE = 61536;
		internal const UInt32 MF_ENABLED = 0;
		internal const UInt32 MF_GRAYED = 1;
		internal const UInt32 MF_DISABLED = 2;
		internal const System.UInt32 MF_BYCOMMAND = 0;

		[DllImport("user32.dll")] private static extern bool EnableMenuItem(IntPtr hMenu, System.UInt32 uIDEnableItem, System.UInt32 uEnable);
		[DllImport("user32.dll")] private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
		[DllImport("kernel32.dll")] private static extern IntPtr GetConsoleWindow();

		private static bool _haveconsole = false;
		public static bool HaveConsole
		{
			get { return _haveconsole; }
			set
			{
				if (!(value == _haveconsole))
				{
					try
					{
						if (value)
						{
							AllocConsole();
							EnableConsoleCloseButton(false);
						}
						else
						{
							FreeConsole();
						}
					}
					catch (Exception ex)
					{
					}
					_haveconsole = value;
				}
			}
		}

		public static void EnableConsoleCloseButton(bool bEnabled)
		{
			IntPtr hSystemMenu = GetSystemMenu(GetConsoleWindow(), false);
			if (bEnabled)
				EnableMenuItem(hSystemMenu, SC_CLOSE, (MF_ENABLED));
			else
				EnableMenuItem(hSystemMenu, SC_CLOSE, (MF_ENABLED | MF_GRAYED));
		}


	}
}
