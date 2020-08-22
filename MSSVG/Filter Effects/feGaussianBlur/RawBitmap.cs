using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Svg.FilterEffects
{
    internal sealed class RawBitmap : IDisposable
    {
        private Bitmap _originBitmap;
        private BitmapData _bitmapData;
        private IntPtr _ptr;
        private int _bytes;
        private byte[] _argbValues;

        public RawBitmap(Bitmap originBitmap)
        {
            _originBitmap = originBitmap;
            _bitmapData = _originBitmap.LockBits(new Rectangle(0, 0, _originBitmap.Width, _originBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            _ptr = _bitmapData.Scan0;
            _bytes = this.Stride * _originBitmap.Height;
            _argbValues = new byte[_bytes];
            Marshal.Copy(_ptr, _argbValues, 0, _bytes);
        }

        #region IDisposable Members

        public void Dispose()
        {
            _originBitmap.UnlockBits(_bitmapData);
        }

        #endregion

        public int Stride
        {
            get { return _bitmapData.Stride; }
        }

        public int Width
        {
            get { return _bitmapData.Width; }
        }

        public int Height
        {
            get { return _bitmapData.Height; }
        }

        public byte[] ArgbValues
        {
            get { return _argbValues; }
            set
            {
                _argbValues = value;
            }
        }

        public Bitmap Bitmap
        {
            get
            {
                Marshal.Copy(_argbValues, 0, _ptr, _bytes);
                return _originBitmap;
            }
        }

    }
}