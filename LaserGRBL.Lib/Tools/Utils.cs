//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;


//abilitare questa macro per poter usare il moltiplicatore di frequenza per fare i test
//#define timedebug

using System;

namespace Tools
{
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