namespace Tools
{
    public class TaskScheduler
	{
		public static void SetClockResolution(int msec) //volendo potrebbe gestire anche 0.5 msec ma a noi basta 1-15 msec, quindi usiamo int
		{
			WinAPI.SetClockResolution(msec);
		}

	}
}