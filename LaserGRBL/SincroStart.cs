using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
	public static class SincroStart
	{
		static System.Threading.ManualResetEvent EX;	//Exit flag
		static System.Threading.EventWaitHandle EV;		//Event flag
		static System.Threading.Thread TH;
		static GrblCore C;

		static public void StartListen(GrblCore Core)
		{
			C = Core;
			EX = new System.Threading.ManualResetEvent(false);
			EV = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset, "LaserGRBL syncro event");
			TH = new System.Threading.Thread(DoTheJob);
			TH.Start();
		}

		static public bool Running()
		{ return TH != null; }

		static public void StopListen()
		{
			EX.Set();
		}

		private static void DoTheJob()
		{
			while (!EX.WaitOne(0))
			{
				int handle = System.Threading.WaitHandle.WaitAny(new System.Threading.WaitHandle[] { EV, EX });

				if (handle == 0)
				{
					if (C.CanSendFile)
						C.RunProgram(null);
					else if (C.CanResumeHold)
						C.CycleStartResume(false);
					else if (C.CanFeedHold)
						C.FeedHold(false);

					System.Threading.Thread.Sleep(1000); //do not test the flag for 1 sec
				}
			}
		}

		public static void Signal()
		{
			new Action(SignalEvent).BeginInvoke(null, null);
		}

		private static void SignalEvent()
		{
			EV.Set();								//setta l'evento
			System.Threading.Thread.Sleep(500);		//aspetta 500ms così da assicurarsi che tutti lo vedano
			EV.Reset();								//resetta l'evento
		}
	}
}
