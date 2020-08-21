using System;

namespace LaserGRBL
{
    [Serializable]
	public class ThreadingMode
	{
		public readonly int StatusQuery;
		public readonly int TxLong;
		public readonly int TxShort;
		public readonly int RxLong;
		public readonly int RxShort;
		private readonly string Name;

		public ThreadingMode(int query, int txlong, int txshort, int rxlong, int rxshort, string name)
		{
			StatusQuery = query;
			TxLong = txlong; 
			TxShort = txshort;
			RxLong = rxlong; 
			RxShort = rxshort; 
			Name = name;
		}

        public static ThreadingMode Slow => new ThreadingMode(2000, 15, 4, 2, 1, "Slow");

        public static ThreadingMode Quiet => new ThreadingMode(1000, 10, 2, 1, 1, "Quiet");

        public static ThreadingMode Fast => new ThreadingMode(500, 5, 1, 1, 0, "Fast");

        public static ThreadingMode UltraFast => new ThreadingMode(200, 1, 0, 0, 0, "UltraFast");

        public static ThreadingMode Insane => new ThreadingMode(100, 1, 0, 0, 0, "Insane");

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj is ThreadingMode && ((ThreadingMode)obj).Name == Name;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
