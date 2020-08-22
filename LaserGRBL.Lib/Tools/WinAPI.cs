namespace Tools
{
    public class WinAPI
    {
		public static void SignalActvity()
		{
			PInovkes.WinAPI.SignalActvity();
		}
		public static void SignalFree()
		{
			PInovkes.WinAPI.SignalFree();
		}
		public static bool QueryPerformanceCounter(ref long count)
		{
			return PInovkes.WinAPI.QueryPerformanceCounter(ref count);
		}
		public static  bool QueryPerformanceFrequency(ref long timerFrequency)
		{
			return PInovkes.WinAPI.QueryPerformanceFrequency(ref timerFrequency);
		}
		public static long GetTickCount64()
		{
			return PInovkes.WinAPI.GetTickCount64();
		}
	}
}
