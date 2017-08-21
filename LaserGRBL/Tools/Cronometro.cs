using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
	public class TimingBase
	{

		private static long mStartup;

		static TimingBase()
		{
			mStartup = HiResTimer.TotalNano;
		}

		public static TimeSpan TimeFromApplicationStartup()
		{
			// Un singolo Tick rappresenta cento nanosecondi 
			return TimeSpan.FromTicks((HiResTimer.TotalNano - mStartup) / 100);
		}

	}

	public class OneShootCrono
	{
		//necessario xè non è sufficiente usare starttime
		private bool started = false;

		private TimeSpan starttime = TimeSpan.Zero;
		public void Start()
		{
			starttime = TimingBase.TimeFromApplicationStartup();
			started = true;
		}

		public TimeSpan TempoTrascorso
		{
			get
			{
				//non inizializzato
				if (!started)
				{
					return TimeSpan.Zero;
				}
				else
				{
					return TimingBase.TimeFromApplicationStartup() - starttime;
				}
			}
		}

		public virtual void Reset()
		{
			starttime = TimeSpan.Zero;
			started = false;
		}

		public bool Running
		{
			get { return started; }
		}

	}

	public class Cronometro : OneShootCrono
	{


		private System.Collections.Generic.List<TimeSpan> times = new System.Collections.Generic.List<TimeSpan>();
		public void SalvaIntermedio()
		{
			times.Add(TempoTrascorso);
		}

		public override void Reset()
		{
			base.Reset();
			times.Clear();
		}

		public System.Collections.Generic.List<TimeSpan> Intermedi
		{
			get { return times; }
		}

	}

	public class AutoResetTimer
	{
		private TimeSpan m_period;
		private OneShootCrono crono;

		private long m_periodcount;
		public AutoResetTimer(TimeSpan Period, bool start = false)
		{
			m_period = Period;
			crono = new OneShootCrono();

			if (start)
				Start();
		}

		public void Start()
		{
			crono.Start();
		}

		public bool Running
		{
			get { return crono.Running; }
		}

		public TimeSpan Period
		{
			get { return m_period; }
		}

		public bool Expired
		{
			get
			{
				bool rv = false;
				long newperiod = crono.TempoTrascorso.Ticks / Period.Ticks;
				rv = newperiod > m_periodcount;
				m_periodcount = newperiod;
				return rv;
			}
		}

		public long NumPeriod
		{
			get { return m_periodcount; }
		}

	}

}
