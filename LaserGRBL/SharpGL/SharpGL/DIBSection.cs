using System;
using System.Runtime.InteropServices;

namespace SharpGL
{
    /// <summary>
    /// 
    /// </summary>
	public class DIBSection : IDisposable
	{
        /// <summary>
        /// Creates the specified width.
        /// </summary>
        /// <param name="hDC">The handle to the device context.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bitCount">The bit count.</param>
        /// <returns></returns>
		public virtual unsafe bool Create(IntPtr hDC, int width, int height, int bitCount)
		{
			this.width = width;
			this.height = height;
            parentDC = hDC;

			//	Destroy existing objects.
			Destroy();
			
			//	Create a bitmap info structure.
            Win32.BITMAPINFO info = new Win32.BITMAPINFO();
            info.Init();

			//	Set the data.
			info.biBitCount = (short)bitCount;
			info.biPlanes = 1;
			info.biWidth = width;
			info.biHeight = height;

			//	Create the bitmap.
			hBitmap = Win32.CreateDIBSection(hDC, ref info, Win32.DIB_RGB_COLORS,
				out bits, IntPtr.Zero, 0);

            Win32.SelectObject(hDC, hBitmap);
			
			//	Set the OpenGL pixel format.
			SetPixelFormat(hDC, bitCount);

			return true;
		}

    /// <summary>
    /// Resizes the section.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="bitCount">The bit count.</param>
        public void Resize(int width, int height, int bitCount)
        {
            //	Destroy existing objects.
            Destroy();

            //  Set parameters.
            Width = width;
            Height = height;

            //	Create a bitmap info structure.
            Win32.BITMAPINFO info = new Win32.BITMAPINFO();
            info.Init();

            //	Set the data.
            info.biBitCount = (short)bitCount;
            info.biPlanes = 1;
            info.biWidth = width;
            info.biHeight = height;

            //	Create the bitmap.
            hBitmap = Win32.CreateDIBSection(parentDC, ref info, Win32.DIB_RGB_COLORS,
                out bits, IntPtr.Zero, 0);

            Win32.SelectObject(parentDC, hBitmap);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Destroy();
        }

		/// <summary>
		/// This function sets the pixel format of the underlying bitmap.
		/// </summary>
        /// <param name="hDC">The handle to the device context.</param>
		/// <param name="bitCount">The bitcount.</param>
		protected virtual bool SetPixelFormat(IntPtr hDC, int bitCount)
		{
			//	Create the big lame pixel format majoo.
            Win32.PIXELFORMATDESCRIPTOR pixelFormat = new Win32.PIXELFORMATDESCRIPTOR();
            pixelFormat.Init();

			//	Set the values for the pixel format.
			pixelFormat.nVersion  = 1;
            pixelFormat.dwFlags = (Win32.PFD_DRAW_TO_BITMAP | Win32.PFD_SUPPORT_OPENGL | Win32.PFD_SUPPORT_GDI);
            pixelFormat.iPixelType = Win32.PFD_TYPE_RGBA;
			pixelFormat.cColorBits = (byte)bitCount;
			pixelFormat.cDepthBits = 32;
            pixelFormat.iLayerType = Win32.PFD_MAIN_PLANE;

			//	Match an appropriate pixel format 
			int iPixelformat;
			if((iPixelformat = Win32.ChoosePixelFormat(hDC, pixelFormat)) == 0 )
				return false;

			//	Sets the pixel format
            if (Win32.SetPixelFormat(hDC, iPixelformat, pixelFormat) == 0)
			{
                //  Clear the error and fail.
				int _ = Marshal.GetLastWin32Error();
				return false;
			}

			return true;
		}

    /// <summary>
    /// Destroys this instance.
    /// </summary>
		public virtual void Destroy()
		{
            //	Destroy the bitmap.
			if(hBitmap != IntPtr.Zero)
			{
                Win32.DeleteObject(hBitmap);
				hBitmap = IntPtr.Zero;
            }
		}

        /// <summary>
        /// The parent dc.
        /// </summary>
        protected IntPtr parentDC = IntPtr.Zero;

        /// <summary>
        /// The bitmap handle.
        /// </summary>
        protected IntPtr hBitmap = IntPtr.Zero;

        /// <summary>
        /// The bits.
        /// </summary>
        protected IntPtr bits = IntPtr.Zero;

        /// <summary>
        /// The width.
        /// </summary>
		protected int width = 0;
        
        /// <summary>
        /// The height.
        /// </summary>
		protected int height = 0;

        /// <summary>
        /// Gets the handle to the bitmap.
        /// </summary>
        /// <value>The handle to the bitmap.</value>
		public IntPtr HBitmap
		{
			get {return hBitmap;}
		}

        /// <summary>
        /// Gets the bits.
        /// </summary>
        public IntPtr Bits
        {
            get { return bits; }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
		public int Width
		{
			get { return width; }
            protected set { width = value; }
		}

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
		public int Height
		{
			get {return height;}
            protected set { height = value; }
		}
    }
}