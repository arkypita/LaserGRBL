using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Tools
{
	public class TaskScheduler
	{

		[System.Runtime.InteropServices.DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
		private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);

		public static void SetClockResolution(int msec) //volendo potrebbe gestire anche 0.5 msec ma a noi basta 1-15 msec, quindi usiamo int
		{
			uint DesiredResolution = (uint)msec * 10000;
			bool SetResolution = true;
			uint CurrentResolution = 0;
			NtSetTimerResolution(DesiredResolution, SetResolution, ref CurrentResolution);
		}

	}
}