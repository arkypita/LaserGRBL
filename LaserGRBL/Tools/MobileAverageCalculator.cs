using System;

namespace Base.Mathematics
{
	public class MobileAverageCalculator
	{
		private int mSize;
		private int mFilledSize;

		private long mSum;
		private long mCur;
		private long mMax;
		private long mMin;
		private long mAvg;
		private bool mReady;


		public MobileAverageCalculator(int size, long init) : this(size)
		{
			EnqueueNewSample(init);
		}

		public MobileAverageCalculator(int size)
		{
			mSize = size;
		}

		public void Reset()
		{
			mSum = 0;
			mCur = 0;
			mMax = 0;
			mMin = 0;
			mAvg = 0;
			mFilledSize = 0;
			mReady = false;
		}

		public void EnqueueNewSample(long value)
		{
			if (mReady)
				mSum = mSum - (mSum / mSize) + value;
			else
			{
				mSum = value * mSize;
				mReady = true;
			}

			if ((mFilledSize < mSize))
				mFilledSize += 1;


			mCur = value;
			mMax = Math.Max(mMax, mCur);
			mMin = Math.Min(mMin, mCur);
			mAvg = mSum / mSize;
		}

		public void Resize(int newSize)
		{
			mSum = mAvg * newSize;
			mSize = newSize;
		}

		public long CurrentValue
		{
			get
			{
				return mCur;
			}
		}

		public long Max
		{
			get
			{
				return mMax;
			}
		}

		public long Min
		{
			get
			{
				return mMin;
			}
		}

		public long Avg
		{
			get
			{
				return mAvg;
			}
		}

		public bool Ready
		{
			get
			{
				return mReady;
			}
		}

		public bool Filled
		{
			get
			{
				return mFilledSize == mSize;
			}
		}
	}


	public class MobileDAverageCalculator
	{
		private int mSize;
		private double mSum;
		private double mCur;
		private double mMax;
		private double mMin;
		private double mAvg;
		private bool mReady;

		public MobileDAverageCalculator(int size, double init) : this(size)
		{
			EnqueueNewSample(init);
		}

		public MobileDAverageCalculator(int size)
		{
			mSize = size;
		}

		public void Reset(double init)
		{
			Reset();
			EnqueueNewSample(init);
		}
		public void Reset()
		{
			mSum = 0;
			mCur = 0;
			mMax = 0;
			mMin = 0;
			mAvg = 0;
			mReady = false;
		}

		public void EnqueueNewSample(double value)
		{
			if (mReady)
				mSum = mSum - (mSum / mSize) + value;
			else
			{
				mSum = value * mSize;
				mReady = true;
			}

			mCur = value;
			mMax = Math.Max(mMax, mCur);
			mMin = Math.Min(mMin, mCur);
			mAvg = mSum / mSize;
		}

		public void Resize(int newSize)
		{
			mSum = mAvg * newSize;
			mSize = newSize;
		}

		public double CurrentValue
		{
			get
			{
				return mCur;
			}
		}

		public double Max
		{
			get
			{
				return mMax;
			}
		}

		public double Min
		{
			get
			{
				return mMin;
			}
		}

		public double Avg
		{
			get
			{
				return mAvg;
			}
		}

		public bool Ready
		{
			get
			{
				return mReady;
			}
		}
	}
}
