using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
	public static class SincroStart
	{
		static System.Threading.EventWaitHandle EV;
		static System.Threading.Thread TH;
		static GrblCore C;

		static public void StartListen(GrblCore Core)
		{
			C = Core;
			EV = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, "LaserGRBL syncro event");
			TH = new System.Threading.Thread(DoTheJob);
			TH.Start();
		}

		static public bool Running()
		{ return TH != null; }

		static public void StopListen()
		{
			TH.Abort();
		}

		private static void DoTheJob()
		{
			while (true)
			{
				EV.WaitOne();

				if (C.CanSendFile)
					C.RunProgram();
				else if (C.CanResumeHold)
					C.CycleStartResume(false);

				System.Threading.Thread.Sleep(1000); //do not test the flag for 1 sec
			}
		}

		public static void Signal()
		{
			new Action(SignalEvent).BeginInvoke(null, null);
		}

		private static void SignalEvent()
		{
			EV.Set();
			System.Threading.Thread.Sleep(500);
			EV.Reset();
		}
	}
}
