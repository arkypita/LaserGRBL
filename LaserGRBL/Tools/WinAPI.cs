using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Tools
{
	class WinAPI
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Auto,SetLastError = true)]
		private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

		[FlagsAttribute]
		private enum EXECUTION_STATE :uint
		{
			 ES_AWAYMODE_REQUIRED = 0x00000040,
			 ES_CONTINUOUS = 0x80000000,
			 ES_DISPLAY_REQUIRED = 0x00000002,
			 ES_SYSTEM_REQUIRED = 0x00000001
			 // Legacy flag, should not be used.
			 // ES_USER_PRESENT = 0x00000004
		}



		public static void SignalActvity() // Enable away mode and prevent the sleep idle time-out.
		{SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);}

		// Clear EXECUTION_STATE flags to disable away mode and allow the system to idle to sleep normally.
		public static void SignalFree()
		{ SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS); }





	}
}
