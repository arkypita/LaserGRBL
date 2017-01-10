
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
	// La logica di correzione usa una finestra temporare ampia (now - LastReset) per il calcolo fine della frequenza sul lungo periodo
	// ed una stretta finestra temporale mobile (now - MobileWindow) per intercettare il cambio di frequenza a runtime, e resettare la finestra lunga
	//
	public static class HiResTimer
	{

		//16ms è la risoluzione tipica del timer low-res (tra 10 e 16msec)
		private const int LowResResolution = 16;
		private const int SafeTrigger = LowResResolution + 8; //aggiungi 8ms di margine -> 24

		private const long MILLI_IN_SECOND = 1000;
		private const long NANO_IN_MILLI = 1000 * 1000;
		private const double NANO_IN_SECOND = 1.0e9;
		private static string TimingLock = "Timing-Lock-String";

		private static bool mPerfCounterSupported = true;
		private static long mCurrentFrequency = 0;
		private static long mOriginalFrequency = 0;

		private static TimeReference LastReset; //TimeReference di partenza per la finestra ampia
		private static TimeReference MobileWindow; //TimeReference di partenza per la finestra stretta

		private static long cambioNano; //nanoSecondi misurati all'ultimo cambio di frequenza
		private static long cambioCount; //performanceCounter raggiunto all'ultimo cambio di frequenza

#if timedebug
		//variabili di appoggio necessarie a simulare il cambio di frequenza dell'HiResTimer
		private static long startQPC = 0; // performanceCounter al cambio di moltiplicatore
		private static long startEPC = 0; // emulated-performanceCounter al cambio di moltiplicatore
		private static double testmultiplier = 1.0; //moltiplicatore di frequenza
		public static double MaxCycleError = 0;
		public static double TotalError = 0;
		public static int Samples = 0;
		public static long mDebugFrequency = 0;
		public static int AlignmentCount = 0;
		public static int ShortWTrigger = 0;
#endif

		[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern bool QueryPerformanceCounter(ref long count);
		[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern bool QueryPerformanceFrequency(ref long timerFrequency);
		[DllImport("Kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern int GetTickCount();


		private static long mTickCount64 = 0;
		private static long GetTickCount64() //emulo la GetTickCount64 perché non esiste su WindowsXP
		{
			long Current = GetTickCount();
			if ((mTickCount64 & 0x80000000) != 0 && (Current & 0x80000000) == 0)
				mTickCount64 += 0x100000000L;
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
			private long mLowRes; //in millisecondi, come tornati dalla GetTickCount64 - con risoluzione 16mS
			private long mHiRes; //in tick @ frequenza QPF come tornati da QueryPerformanceCounter - con risoluzione dei uS

			internal static TimeReference Now
			{
				get
				{
					TimeReference rv;

					long hr = 0;

					long lr = GetTickCount64(); //leggi LowResTimer
					if (mPerfCounterSupported)
						QueryPerformanceCounter(ref hr); //leggi HiResTimer
					else
						hr = lr; //emula HiResTimer come LowResTimer

					rv.mLowRes = lr;

#if timedebug
					rv.mHiRes = (long)(startEPC + (hr - startQPC) * testmultiplier);
#else
					rv.mHiRes = hr;
#endif

					return rv;
				}
			}

			private TimeReference(long LowRes, long HiRes)
			{
				this.mLowRes = LowRes;
				this.mHiRes = HiRes;
			}

			internal TimeReference Subtract(TimeReference t)
			{ return new TimeReference(mLowRes - t.mLowRes, mHiRes - t.mHiRes); }

			internal long LowRes
			{ get { return mLowRes; } }

			internal long HiRes
			{ get { return mHiRes; } }

			private double HiResMsecD() //converte un intervallo tra due tempi, espressi in HiRes @ freq - nel rispettivo tempo in mSec (per il confronto con LowRes)
			{ return (double)mHiRes / (double)CurrentFrequency * MILLI_IN_SECOND; }

			internal long CalculateHiResFrequency() //da usare su un time-interval calcolato come subtract tra due TimeReference
			{ return (mHiRes * MILLI_IN_SECOND + mLowRes / 2) / mLowRes; }

			internal bool IsLowResZero //da usare su un time-interval calcolato come subtract tra due TimeReference
			{ get { return mLowRes == 0; } } //evita che due chiamate successive diano un Delta-LowRes pari a Zero

			internal bool InError() //da usare su un time-interval calcolato come subtract tra due TimeReference
			{
				double TimeError = Error; // calcola il TimeError come il delta tra i due orologi, sul periodo a cui si riferisce

#if timedebug
				if (Math.Abs(TimeError) > Math.Abs(MaxCycleError))
					MaxCycleError = TimeError;
#endif

				return !IsLowResZero && (Math.Abs(TimeError) > SafeTrigger); //se si è cumulato TimeError > SafeTrigger allora siamo sicuri che deriva: si deve fare una correzione di frequenza
			}

			internal double Error
			{ get { return HiResMsecD() - LowRes; } }

		}

		static HiResTimer() //costruttore static, viene chiamato prima del primo utilizzo della classe
		{
			mPerfCounterSupported = QueryPerformanceFrequency(ref mOriginalFrequency); //verifica se è disponibile l'HiResTimer (true a partire da WinXP)
			if (!mPerfCounterSupported) //se non è supportato lo emuliamo con il LowRes
				mOriginalFrequency = MILLI_IN_SECOND;

#if timedebug
			mCurrentFrequency = (long)Math.Round(mOriginalFrequency * testmultiplier);
#else
			mCurrentFrequency = mOriginalFrequency;
#endif

			// per testare il caso più complicato di allinemento a cambio lowres
			//long old = GetTickCount64();
			//while (old == GetTickCount64()) // allineati all'inizio della variazione
			//	;

			TimeReference now = TimeReference.Now;

#if timedebug
			startQPC = startEPC = now.HiRes;
#endif

			LastReset = MobileWindow = now; //avvia le finestre fissa e mobile a partire da adesso
			cambioNano = 0; // assegna il cambioNano iniziale - nessun cambio
			cambioCount = 0; // assegna il cambioCount iniziale - nessun cambio
		}

#if timedebug
		public static long TotalMillisecondsLR
		{ get { return TimeReference.Now.LowRes; } }
#endif

		public static long TotalMilliseconds
		{ get { return TotalNano / NANO_IN_MILLI; } }

		public static long TotalNano
		{
			get
			{
				lock (TimingLock) //previene l'accesso multithread alle variabili static
				{
					TimeReference now = TimeReference.Now;

					if (true/*LibConfig.HRTFrequencyCorrection*/)
						ComputeFrequency(now);

					return cambioNano + (long)((now.HiRes - cambioCount) * NANO_IN_SECOND / mCurrentFrequency);
				}
			}
		}

		public static long CurrentFrequency
		{ get { return HiResTimer.mCurrentFrequency; } }
		public static long OriginalFrequency
		{ get { return HiResTimer.mOriginalFrequency; } }

		private static void ComputeFrequency(TimeReference now)
		{
			TimeReference longWindow = now.Subtract(LastReset);
			TimeReference smallWindow = now.Subtract(MobileWindow);

#if timedebug
			MaxCycleError = 0;
#endif

#if timedebug
			CalcolaFrequenzaDebug(longWindow, now);
#endif

			if (longWindow.InError()) //verifica la bontà dell' ActualFrequency sull'intervallo lungo (orologio derivante)
			{
				ApplicaNuovaFrequenza(longWindow, now);
			}

			if (smallWindow.LowRes >= 2000) //verifica la bontà dell' ActualFrequency sull'intervallo piccolo (orologio impazzito) - tolleranza 24ms/2000ms = 1.2%
			{
				if (smallWindow.InError())
				{
#if timedebug
					ShortWTrigger++;
#endif

					System.Diagnostics.Debug.Write("SWC: ");
					ApplicaNuovaFrequenza(smallWindow, now);
					LastReset = now;
				}
				MobileWindow = now;
			}

#if timedebug
			TotalError += MaxCycleError;
			Samples++;
#endif

		}


		private static void ApplicaNuovaFrequenza(TimeReference elapsed, TimeReference now)
		{
			long newfreq = elapsed.CalculateHiResFrequency();

#if timedebug
			long target = (long)Math.Round(mOriginalFrequency * testmultiplier);
			System.Diagnostics.Debug.WriteLine(String.Format("{0} -> {1} target {2} err {3:+0.00000;-0.00000}% trigger with {4:+00.000;-00.000}", mCurrentFrequency, newfreq, target, (newfreq - target) * 100.0 / target, elapsed.Error));
#else
			System.Diagnostics.Debug.WriteLine(String.Format("{0} -> {1}", mCurrentFrequency, newfreq));
#endif

			AssignFrequency(newfreq, now.HiRes);

#if timedebug
			AlignmentCount++;
#endif
		}

		private static void AssignFrequency(long newfreq, long countHR)
		{
			cambioNano = cambioNano + (long)((countHR - cambioCount) * NANO_IN_SECOND / mCurrentFrequency);
			cambioCount = countHR;

			mCurrentFrequency = newfreq;

#if timedebug
			TotalError = 0;
			Samples = 0;
#endif
		}


#if timedebug
		private static void CalcolaFrequenzaDebug(TimeReference elapsed, TimeReference now)
		{
			if (!elapsed.IsLowResZero)
				mDebugFrequency = elapsed.CalculateHiResFrequency();
		}
#endif

#if timedebug
		public static double TestMultiplier //codice per simulare un HiResTimer a frequenza variabile
		{
			get
			{
				return testmultiplier;
			}
			set
			{
				startEPC = TimeReference.Now.HiRes; //memorizza l'HiRes emulato a cui siamo arrivati
				if (mPerfCounterSupported)
					QueryPerformanceCounter(ref startQPC); //memorizza l'HiRes reale a cui siamo arrivati
				else
					startQPC = GetTickCount64(); //memorizza l'HiRes reale a cui siamo arrivati (usa il LowRes se HiRes non supportato)

				testmultiplier = value; //assegna il nuovo moltiplicatore
			}
		}
#endif

#if timedebug
		public static int ErrorThreshold
		{ get { return SafeTrigger; } }
#endif
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
					functionReturnValue = S_now;
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