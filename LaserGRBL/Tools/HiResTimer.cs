//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;


//abilitare questa macro per poter usare il moltiplicatore di frequenza per fare i test
//#define timedebug

using System;
using Microsoft.VisualBasic;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Tools
{


	//
	// Contiene una logica di correzione della frequenza dell' HiResTimer (QPF) che prende il LowResTimer (GetTickCount64) come riferimento.
	// La frequenza dell'HiResTimer potrebbe essere sbagliata di poco (ie. perché deviata rispetto al LowResTimer) oppure potrebbe essere
	// sbagliata di molto. Abbiamo infatti notato su UNA macchina (portatile DELL VOSTRO 3750 - i5-2450M w7-64bit) che la funzione QPC può dare
	// delle tempistiche incoerenti con la relativa QPF, cambiando la sua velocità anche in corso di esecuzione del programma, senza variazioni di QPF
	//
	public static class HiResTimer
	{

		//16ms è la risoluzione tipica del timer low-res (tra 10 e 16msec)
		//16ms + 8ms di margine -> 24ms
		private const long ErrorThreshold = 24 * NANO_IN_MILLI;

		private const long MILLI_IN_SECOND = 1000;
		private const long NANO_IN_MILLI = 1000 * 1000;
		private const double NANO_IN_SECOND = 1.0e9;
		private const string TimingLock = "Timing-Lock-String";

		private static readonly bool mPerfCounterSupported = true;
		private static long mCurrentFrequency = 0;
		private static long mOriginalFrequency = 0;

		private static TimeReference mLastReference;    //TimeReference dell'ultima chiamata a TotalNano
		private static long mTotalNano;                 //Parzializzatore dei Nanosecondi
		private static TimeReference LongWindow;        //TimeReference di partenza per la finestra di controllo frequenza
		private static int mCorrectionCount = 0;        //contatore di quante volte ho corretto la frequenza

#if timedebug
		//variabili di appoggio necessarie a simulare il cambio di frequenza dell'HiResTimer
		private static DateTime startDT;			//orario a cui viene cambiato il moltiplicatore, così da avere una diagnostica di quanto tempo impiega a convergere
		private static long startQPC = 0;			// performanceCounter al cambio di moltiplicatore
		private static long startEPC = 0;			// emulated-performanceCounter al cambio di moltiplicatore
		private static double testmultiplier = 1.0;	//moltiplicatore di frequenza
#endif


		[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern bool QueryPerformanceCounter(ref long count);
		[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern bool QueryPerformanceFrequency(ref long timerFrequency);
		[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern int GetTickCount();


		//emulo la GetTickCount64 perché non esiste su WindowsXP
		private static long mTickCount64 = 0;
		private static long GetTickCount64()
		{
			long Current = GetTickCount();
			if ((mTickCount64 & 0x80000000) != 0 && (Current & 0x80000000) == 0)
			{
				mTickCount64 += 0x100000000L;
			}

			mTickCount64 = (mTickCount64 & 0x0FFFFFFF00000000L) | (Current & 0x00000000FFFFFFFFL);

			return mTickCount64;
		}

		// 
		// Struttura TimeReference 
		// Contiene i due valori di tempo misurati con l'HiResTimer e con il LowRes timer
		// Può essere usata anche per tenere le differenze tra due intervalli di tempo (tramite la funzione subtract)
		// in tal caso contiene i valori dell'intervallo di tempo misurato con l'HiResTimer e con il LowRes timer
		//
		private struct TimeReference
		{
			private long mLowRes;       //in millisecondi, come tornati dalla GetTickCount64 - con risoluzione 16mS
			private long mHiRes;        //in tick @ frequenza QPF come tornati da QueryPerformanceCounter - con risoluzione dei uS

#if timedebug
			private long mReference;    //preso da hires reale e non faked dal multiplier
			internal long ReferenceNano => (long)(mReference * NANO_IN_SECOND / mOriginalFrequency);
#endif

#if timedebug
			internal static TimeReference Zero
			{ get { return new TimeReference(0, 0, 0); } }
#else
			internal static TimeReference Zero
			{ get { return new TimeReference(0, 0); } }
#endif

			internal static TimeReference Now
			{
				get
				{
#if timedebug
					TimeReference rv = new TimeReference(0,0,0);
#else
					TimeReference rv = new TimeReference(0, 0);
#endif
					bool TaskSwitch = false;
					do
					{
						rv.mLowRes = GetTickCount64();                  //leggi LowResTimer
						if (mPerfCounterSupported)
							QueryPerformanceCounter(ref rv.mHiRes);     //leggi HiResTimer
						else
							rv.mHiRes = rv.mLowRes;                     //emula HiResTimer come LowResTimer

						TaskSwitch = GetTickCount64() != rv.mLowRes;    //verifica che non ci sia stato un Task Switch tra le due letture

						//if (TaskSwitch) System.Diagnostics.Debug.WriteLine("Switch!");
					}
					while (TaskSwitch);                                 //ripeti la lettura se c'è stato un Task Switch

#if timedebug
					rv.mReference = rv.HiRes;
					rv.mHiRes = (long)(startEPC + (rv.mHiRes - startQPC) * testmultiplier);
#endif
					return rv;
				}
			}


#if timedebug
			private TimeReference(long LowRes, long HiRes, long Ref)
			{
				this.mLowRes = LowRes;
				this.mHiRes = HiRes;
				this.mReference = Ref;
			}
#else
			private TimeReference(long LowRes, long HiRes)
			{
				this.mLowRes = LowRes;
				this.mHiRes = HiRes;
			}
#endif

#if timedebug
			internal TimeReference Subtract(TimeReference t)
			{ return new TimeReference(mLowRes - t.mLowRes, mHiRes - t.mHiRes, mReference - t.mReference); }
#else
			internal TimeReference Subtract(TimeReference t)
			{ return new TimeReference(mLowRes - t.mLowRes, mHiRes - t.mHiRes); }
#endif

			internal long LowRes => mLowRes;
			internal long LowResNano => mLowRes * NANO_IN_MILLI;

			internal long HiRes => mHiRes;
			internal long HiResNano => (long)(mHiRes * NANO_IN_SECOND / mCurrentFrequency);

			//da usare su un time-interval calcolato come subtract tra due TimeReference
			internal long CalculateHiResFrequency() => mHiRes * MILLI_IN_SECOND / mLowRes;

			//da usare su un time-interval calcolato come subtract tra due TimeReference
			internal bool IsGoodHiRes()
			{
				if (mCurrentFrequency == 0)
					return false;

				long error = HiResNano - LowResNano;        // calcola error come il delta tra i due orologi
				return Math.Abs(error) < ErrorThreshold;
			}

			internal bool ShouldAdjustFreqency() => mLowRes > 24 && !IsGoodHiRes();
		}

		static HiResTimer() //costruttore static, viene chiamato prima del primo utilizzo della classe
		{
			//verifica se è disponibile l'HiResTimer (true a partire da WinXP) e si fa restituire l' original frequency
			mPerfCounterSupported = QueryPerformanceFrequency(ref mOriginalFrequency);
			//se non è supportato lo emuliamo con il LowRes
			if (!mPerfCounterSupported) mOriginalFrequency = MILLI_IN_SECOND;
			//assegna la frequenza corrente
			mCurrentFrequency = mOriginalFrequency;


			TimeReference now = TimeReference.Now;

#if timedebug
			startQPC = startEPC = now.HiRes;
#endif

			mLastReference = now;
			mTotalNano = now.LowResNano;

			LongWindow = now;               //inizia a monitorare la frequenza da adesso
		}

		public static long TotalNano
		{
			get
			{
				lock (TimingLock) //previene l'accesso multithread alle variabili static
				{
					TimeReference now = TimeReference.Now;

					if (true /*LibConfig.HRTFrequencyCorrection*/)
						ComputeFrequency(now);

					TimeReference delta = now.Subtract(mLastReference);
					long deltaNano = delta.IsGoodHiRes() ? delta.HiResNano : delta.LowResNano;
					long nanoSleepThreshold = 10000000000L; // se passano più di 10s tra due chiamate abbiamo avuto una sleep da non conteggiare! (assicurati di chiamare TotalNano con una frequenza più alta)

					if (deltaNano < 0)
						LaserGRBL.Logger.LogMessage("Issue detector", "Negative delta detected!");
					else if (deltaNano > nanoSleepThreshold)
						LaserGRBL.Logger.LogMessage("Issue detector", "System sleep/hibernation detected ({0}s)", deltaNano / 1000000000L);
					else
						mTotalNano += deltaNano;

					mLastReference = now;

					return mTotalNano;
				}
			}
		}

		public static long TotalMilliseconds => TotalNano / NANO_IN_MILLI;
		public static long CurrentFrequency => mCurrentFrequency;
		public static long OriginalFrequency => mOriginalFrequency;


		private static void ComputeFrequency(TimeReference now)
		{
			TimeReference longWindow = now.Subtract(LongWindow);
			if (longWindow.ShouldAdjustFreqency()) //verifica la bontà dell' ActualFrequency sull'intervallo lungo (orologio derivante)
			{
				ApplicaNuovaFrequenza(longWindow);
				LongWindow = now;
			}
		}


		private static void ApplicaNuovaFrequenza(TimeReference delta)
		{
			long mNewFrequency = delta.CalculateHiResFrequency();

			//applica la media geometrica tra vecchia e nuova frequenza
			//compensa l'oscillazione del valore dovuta al fatto che la mNewFrequency una volta viene in eccesso e una volta viene in difetto
			//ed è ugualmente veloce verso numeri grossi e verso numeri piccoli (a differenza della media aritmetica)
			long mCompFrequency = mCurrentFrequency == 0 ? mNewFrequency : (long)(Math.Sign(mNewFrequency) * Math.Sqrt(Math.Abs((double)mCurrentFrequency * mNewFrequency)));

#if timedebug
			double TimeError = (delta.HiResNano - delta.LowResNano) / (double)NANO_IN_MILLI;
			long TargetFrequency = (long)Math.Round(mOriginalFrequency * testmultiplier);
			long anew = Math.Abs(mCompFrequency);
			long atar = Math.Abs(TargetFrequency);
			double PercError = (anew > atar) ? (anew-atar) * 100.0 / anew : -(atar - anew) * 100.0 / atar;

			if (mCorrectionCount == 0)
				System.Diagnostics.Debug.WriteLine(" TIME \t  ERROR  \t   OLD   \t   NEW   \t   COM   \t  TARGET  \t DELAY \tWINDOW SIZE");

			System.Diagnostics.Debug.WriteLine(string.Format(System.Globalization.CultureInfo.InvariantCulture,
			"{0:00.000}\t{1:+00.00000;-00.00000}\t{2:000000000}\t{3:000000000}\t{4:000000000}\t{5:000000000}\t{6:+00.0;-00.0}ms\t[LR {7}ms HR {8:0.00}ms REF {9:0.00}ms]", DateTime.Now.Subtract(startDT).TotalSeconds, PercError, mCurrentFrequency, mNewFrequency, mCompFrequency, TargetFrequency, TimeError, delta.LowRes, delta.HiResNano / (double)NANO_IN_MILLI, delta.ReferenceNano / (double)NANO_IN_MILLI));
#else
			double TimeError = (delta.HiResNano - delta.LowResNano) / (double)NANO_IN_MILLI;
			System.Diagnostics.Debug.WriteLine($"CLOCK CORRECTION:\t{mCurrentFrequency:000000000}\t{mNewFrequency:000000000}\t{mCompFrequency:000000000}\t{TimeError:0.00}ms\tΔHiRes\t{delta.HiRes}\tΔLowRes\t{delta.LowRes}");
#endif
			mCurrentFrequency = mCompFrequency;
			mCorrectionCount++;
		}




		public static double TestMultiplier //codice per simulare un HiResTimer a frequenza variabile
		{
			get
			{
#if timedebug
				return testmultiplier;
#else
				return 1;
#endif
			}
			set
			{
#if timedebug
				lock (TimingLock) //previene l'accesso multithread alle variabili static
				{
					if (mCorrectionCount > 0) System.Diagnostics.Debug.WriteLine("");

					startDT = DateTime.Now;
					startEPC = TimeReference.Now.HiRes; //memorizza l'HiRes emulato a cui siamo arrivati
					if (mPerfCounterSupported)
						QueryPerformanceCounter(ref startQPC); //memorizza l'HiRes reale a cui siamo arrivati
					else
						startQPC = GetTickCount64(); //memorizza l'HiRes reale a cui siamo arrivati (usa il LowRes se HiRes non supportato)

					testmultiplier = value; //assegna il nuovo moltiplicatore
				}
#endif
			}
		}

	}


	public class Utils
	{
		public static string TimeSpanToString(TimeSpan Span, TimePrecision Precision, TimePrecision MaxFallBackPrecision, string Separator, bool WriteSuffix)
		{
			if (Span >= TimeSpan.MaxValue)
			{
				return "+∞";
			}
			else if (Span <= TimeSpan.MinValue)
			{
				return "-∞";
			}
			else
			{
				DateTime N = DateTime.Now;
				return HumanReadableDateDiff(N, N.Add(Span), Precision, MaxFallBackPrecision, Separator, WriteSuffix);
			}
		}


		public static string HumanReadableDateDiff(DateTime MainDate, DateTime OtherDate, TimePrecision Precision, TimePrecision MaxFallBackPrecision, string Separator, bool WriteSuffix)
		{
			string functionReturnValue = null;

			functionReturnValue = "";

			int years = 0;
			int months = 0;
			int days = 0;
			int hours = 0;
			int minutes = 0;
			int seconds = 0;
			int milliseconds = 0;

			string S_year = "year";
			string S_month = "month";
			string S_day = "day";
			string S_hour = "hour";
			string S_minute = "min";
			string S_second = "sec";
			string S_millisecond = "ms";
			string S_years = "years";
			string S_months = "months";
			string S_days = "days";
			string S_hours = "hours";
			string S_minutes = "min";
			string S_seconds = "sec";
			string S_milliseconds = "ms";
			string S_now = "now";
			string S_ago = "ago";


			DateTime BiggerDate = default(DateTime);
			DateTime SmallestDate = default(DateTime);

			if (MainDate.CompareTo(OtherDate) >= 0)
			{
				BiggerDate = MainDate;
				SmallestDate = OtherDate;
			}
			else
			{
				BiggerDate = OtherDate;
				SmallestDate = MainDate;
			}


			while ((BiggerDate.AddYears(-1).CompareTo(SmallestDate) >= 0))
			{
				years += 1;
				BiggerDate = BiggerDate.AddYears(-1);
			}

			while ((BiggerDate.AddMonths(-1).CompareTo(SmallestDate) >= 0))
			{
				months += 1;
				BiggerDate = BiggerDate.AddMonths(-1);
			}

			while ((BiggerDate.AddDays(-1).CompareTo(SmallestDate) >= 0))
			{
				days += 1;
				BiggerDate = BiggerDate.AddDays(-1);
			}

			TimeSpan diff = BiggerDate.Subtract(SmallestDate);
			hours = diff.Hours;
			minutes = diff.Minutes;
			seconds = diff.Seconds;
			milliseconds = diff.Milliseconds;


			//precision fallback
			if ((Precision == TimePrecision.Years) && (MaxFallBackPrecision > TimePrecision.Years) && (years == 0))
				Precision = TimePrecision.Month;
			if ((Precision == TimePrecision.Month) && (MaxFallBackPrecision > TimePrecision.Month) && (years == 0) && (months == 0))
				Precision = TimePrecision.Day;
			if ((Precision == TimePrecision.Day) && (MaxFallBackPrecision > TimePrecision.Day) && (years == 0) && (months == 0) && (days == 0))
				Precision = TimePrecision.Hour;
			if ((Precision == TimePrecision.Hour) && (MaxFallBackPrecision > TimePrecision.Hour) && (years == 0) && (months == 0) && (days == 0) && (hours == 0))
				Precision = TimePrecision.Minute;
			if ((Precision == TimePrecision.Minute) && (MaxFallBackPrecision > TimePrecision.Minute) && (years == 0) && (months == 0) && (days == 0) && (hours == 0) && (minutes == 0))
				Precision = TimePrecision.Second;
			if ((Precision == TimePrecision.Second) && (MaxFallBackPrecision > TimePrecision.Second) && (years == 0) && (months == 0) && (days == 0) && (hours == 0) && (minutes == 0) && (seconds == 0))
				Precision = TimePrecision.Millisecond;


			if (years > 0)
				functionReturnValue += string.Format("{0} {1}|", years, (years == 1 ? S_year : S_years));
			if (Precision > TimePrecision.Years && months > 0)
				functionReturnValue += string.Format("{0} {1}|", months, (months == 1 ? S_month : S_months));
			if (Precision > TimePrecision.Month && days > 0)
				functionReturnValue += string.Format("{0} {1}|", days, (days == 1 ? S_day : S_days));
			if (Precision > TimePrecision.Day && hours > 0)
				functionReturnValue += string.Format("{0} {1}|", hours, (hours == 1 ? S_hour : S_hours));
			if (Precision > TimePrecision.Hour && minutes > 0)
				functionReturnValue += string.Format("{0} {1}|", minutes, (minutes == 1 ? S_minute : S_minutes));
			if (Precision > TimePrecision.Minute && seconds > 0)
				functionReturnValue += string.Format("{0} {1}|", seconds, (seconds == 1 ? S_second : S_seconds));
			if (Precision > TimePrecision.Second && milliseconds > 0)
				functionReturnValue += string.Format("{0} {1}|", milliseconds, (milliseconds == 1 ? S_millisecond : S_milliseconds));

			if (functionReturnValue == null || string.IsNullOrEmpty(functionReturnValue))
			{
				if (WriteSuffix)
				{
					functionReturnValue = S_now;
				}
				else // write zero + precision
				{
					if (Precision == TimePrecision.Years)
						functionReturnValue = string.Format("0 {0}", S_years);
					if (Precision == TimePrecision.Month)
						functionReturnValue = string.Format("0 {0}", S_months);
					if (Precision == TimePrecision.Day)
						functionReturnValue = string.Format("0 {0}", S_days);
					if (Precision == TimePrecision.Hour)
						functionReturnValue = string.Format("0 {0}", S_hours);
					if (Precision == TimePrecision.Minute)
						functionReturnValue = string.Format("0 {0}", S_minutes);
					if (Precision == TimePrecision.Second)
						functionReturnValue = string.Format("0 {0}", S_seconds);
					if (Precision == TimePrecision.Millisecond)
						functionReturnValue = string.Format("0 {0}", S_milliseconds);


				}
			}
			else
			{
				functionReturnValue = functionReturnValue.Trim(new char[] { '|' });
				functionReturnValue = functionReturnValue.Replace("|", Separator);

				if (WriteSuffix)
				{
					if (MainDate.CompareTo(OtherDate) > 0)
						functionReturnValue = functionReturnValue + " " + S_ago;
				}
			}

			return functionReturnValue;
		}

		internal static void OpenLink(string url)
		{
			string[] list = url.Split('|');
			{
				foreach (string s in list)
				{
					if (s.ToLower().StartsWith("http://") || s.ToLower().StartsWith("https://"))
						Process.Start(string.Format(s, LaserGRBL.UsageStats.GetID()));
				}
			}
		}

		public enum TimePrecision
		{
			Years = 0,
			Month = 1,
			Day = 2,
			Hour = 3,
			Minute = 4,
			Second = 5,
			Millisecond = 6
		}




	}
}