
using System;
using System.Diagnostics;
using System.Threading;

namespace Tools
{

	public class ThreadObject : ThreadClass
	{

		private System.Threading.ThreadStart _delegatesub;

		private System.Threading.ThreadStart _firsrunsub;
		public ThreadObject(System.Threading.ThreadStart DelegateSub, int SleepTime, bool AutoDispose, string Name, System.Threading.ThreadStart FirstRunSub) : base(SleepTime, AutoDispose, Name)
		{
			_delegatesub = DelegateSub;
			_firsrunsub = FirstRunSub;
		}

		protected override void OnFirstRun()
		{
			if ((_firsrunsub != null)) {
				_firsrunsub();
			}
		}

		protected override void DoTheWork()
		{
			if ((_delegatesub != null)) {
				_delegatesub();
			}
		}

	}


	public abstract class ThreadClass : IDisposable
	{

		protected ManualResetEvent MustExit;
			//checked 26/05/2008
		protected internal Thread TH;

		protected ThreadClass(int SleepTime, bool AutoDispose, string Name)
		{
			this.SleepTime = SleepTime;
			if (AutoDispose)
				System.Windows.Forms.Application.ApplicationExit += this.AutoDispose;
			_Name = Name;
		}


		protected virtual bool MustRun()
		{
			//return true if must run
			return (MustExit != null) && !MustExit.WaitOne(SleepTime, false);
		}



		protected virtual void OnThreadTerminating()
		{
		}

		private void Loop()
		{
			OnFirstRun();
			while (MustRun()) {
				DoTheWork();
			}
			OnThreadTerminating();
		}


		protected virtual void OnFirstRun()
		{
		}

		protected abstract void DoTheWork();

		public virtual void Start()
		{
			if (TH == null) {
				MustExit = new ManualResetEvent(false);
				TH = new System.Threading.Thread(Loop);
				TH.Name = this.Name;
				TH.Start();
			}
		}

		public bool Running {
			get { return (TH != null); }
		}

		private string _Name;
		public string Name {
			get { return _Name; }
		}

		private int _timeout = 5000;
		public int StopWaitTimeout {
			get { return _timeout; }
			set { _timeout = Math.Max(value, 0); }
		}

		private int _sleeptime = 0;
		public int SleepTime {
			get { return _sleeptime; }
			set { _sleeptime = Math.Max(value, 0); }
		}

		public virtual void Stop()
		{
			if (TH != null && TH.ThreadState != System.Threading.ThreadState.Stopped)
			{
				if (MustExit != null)
					MustExit.Set();

				if (!object.ReferenceEquals(System.Threading.Thread.CurrentThread, TH))
				{
					if (TH != null && StopWaitTimeout > 0)
						TH.Join(StopWaitTimeout);

					if (TH != null && TH.ThreadState != System.Threading.ThreadState.Stopped)
					{
						System.Diagnostics.Debug.WriteLine(string.Format("Devo forzare la terminazione del Thread '{0}'", TH.Name));
						TH.Abort();
					}
				}
				else
				{
					System.Diagnostics.Debug.WriteLine(string.Format("ATTENZIONE! Chiamata rientrante a thread stop '{0}'", TH.Name));
				}

				TH = null;
				MustExit = null;
			}
		}

		private void AutoDispose(object sender, System.EventArgs e)
		{
			System.Windows.Forms.Application.ApplicationExit -= this.AutoDispose;
			Dispose();
		}

		protected bool MustExitTH(int WaitDelay)
		{
			return MustExit != null && MustExit.WaitOne(WaitDelay, false);
		}

		public bool MustExitTH()
		{
			return MustExitTH(0);
		}

		public virtual void Dispose()
		{
			this.Stop();
		}






	}

}

