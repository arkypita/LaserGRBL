using System.Drawing;
using System.Threading;

namespace LaserGRBL
{
    internal class DoubleBufferBmp
    {
        private Bitmap[] mBmps = new Bitmap[2];
        private int mGetIdx = 0;

        public Bitmap Bitmap
        {
            get
            {
                return mBmps[Thread.VolatileRead(ref mGetIdx)];
            }
            set
            {
                int setIdx = (Thread.VolatileRead(ref mGetIdx) + 1) % 2;
                if (mBmps[setIdx] != null)
                {
                    mBmps[setIdx].Dispose();
                }
                mBmps[setIdx] = value;
                Thread.VolatileWrite(ref mGetIdx, setIdx);
            }
        }

    }
}
