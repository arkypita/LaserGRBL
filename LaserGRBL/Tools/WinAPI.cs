using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Tools
{
	public static class ModifyProgressBarColor
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
		static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

		//Note the second parameter in SetState, 1 = normal (green); 2 = error (red); 3 = warning (yellow).
		public static void SetState(this System.Windows.Forms.ProgressBar pBar, int state)
		{
			SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
		}
	}

	class WinAPI
	{


		//SetThreadExecutionState
		//The .NET framework classes don't provide a way to disable the screensaver or sleep mode, so we need to use Platform Invocation Services (P/Invoke) to call a Windows API function named, "SetThreadExecutionState". This function tells the operating system that the thread is in use, even if the user is not interacting with the computer. This can prevent the display from being hidden and stop the machine from being suspended automatically.
		//SetThreadExecutionState uses a series of flags to specify a new state for the current thread. We'll define these flags shortly using an enumeration with the Flags attribute. You can use the logical OR operator to combine several flags and specify multiple behaviours with a single call. The function returns a value made up from the same flags. The return value indicates the state before the changes that you requested, or returns null if there is an error.
		//The four flags that we are interested in are:
		//ES_DISPLAY_REQUIRED. This flag indicates that the display is in use. When passed by itself, the display idle timer is reset to zero once. The timer restarts and the screensaver will be displayed when it next expires.
		//ES_SYSTEM_REQUIRED. This flag indicates that the system is active. When passed alone, the system idle timer is reset to zero once. The timer restarts and the machine will sleep when it expires.
		//ES_CONTINUOUS. This flag is used to specify that the behaviour of the two previous flags is continuous. Rather than resetting the idle timers once, they are disabled until you specify otherwise. Using this flag means that you do not need to call SetThreadExecutionState repeatedly.
		//ES_AWAYMODE_REQUIRED. This flag must be combined with ES_CONTINUOUS. If the machine is configured to allow it, this indicates that the thread requires away mode. When in away mode the computer will appear to sleep as normal. However, the thread will continue to execute even though the computer has partially suspended. As this flag gives the false impression that the computer is in a low power state, you should only use it when absolutely necessary.


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

		public static void SignalActvity()
		{
			try{SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);}
			catch { }
		}

		// Clear EXECUTION_STATE flags to disable away mode and allow the system to idle to sleep normally.
		public static void SignalFree()
		{
			try{SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);}
			catch { }
		}



	}
}
