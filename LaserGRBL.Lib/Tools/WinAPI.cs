using System;

namespace Tools
{
    public class WinAPI
    {
		// Prevent Sleep
		public static void SignalActvity()
		{
			PInovkes.WinAPI.SignalActvity();
		}
		public static void SignalFree()
		{
			PInovkes.WinAPI.SignalFree();
		}
		// HiRes timer
		public static bool QueryPerformanceCounter(ref long count)
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				count = DateTime.Now.Ticks;
				return true;
			}
			else
			{
				return PInovkes.WinAPI.QueryPerformanceCounter(ref count);
			}
		}
		public static bool QueryPerformanceFrequency(ref long timerFrequency)
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				timerFrequency = 10000000;
				return true;
			}
			else
			{
				return PInovkes.WinAPI.QueryPerformanceFrequency(ref timerFrequency);
			}
		}
		public static long GetTickCount64()
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				return DateTime.Now.Ticks;
			}
			else
			{
				return PInovkes.WinAPI.GetTickCount64();
			}
		}

		public static void SetClockResolution(int msec) //volendo potrebbe gestire anche 0.5 msec ma a noi basta 1-15 msec, quindi usiamo int
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				// Does nothing
			}
			else
			{
				uint DesiredResolution = (uint)msec * 10000;
				bool SetResolution = true;
				uint CurrentResolution = 0;
				PInovkes.WinAPI.NtSetTimerResolution(DesiredResolution, SetResolution, ref CurrentResolution);
			}
		}
	}
}
