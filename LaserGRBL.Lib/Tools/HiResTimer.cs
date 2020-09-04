//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System.Diagnostics;

namespace Tools
{
    // https://github.com/arkypita/LaserGRBL/issues/5
    public static class HiResTimer
	{
		private static Stopwatch watch = new Stopwatch();

		static HiResTimer() //costruttore static, viene chiamato prima del primo utilizzo della classe
		{ watch.Start(); }

		public static long TotalMilliseconds => watch.ElapsedMilliseconds;

		public static long TotalNano
		{
			get
			{
				return (long)((double)watch.ElapsedTicks / (double)Stopwatch.Frequency * 1000000000.0);
			}
		}
	}
}