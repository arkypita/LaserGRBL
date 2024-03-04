using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.Version;

namespace SharpGL.RenderContextProviders
{
    public class HiddenWindowRenderContextProvider : RenderContextProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HiddenWindowRenderContextProvider"/> class.
        /// </summary>
        public HiddenWindowRenderContextProvider()
        {
            //  We can layer GDI drawing on top of open gl drawing.
            GDIDrawingEnabled = true;
        }

        private Win32.WNDCLASSEX wndClass;

        /// <summary>
        /// Creates the render context provider. Must also create the OpenGL extensions.
        /// </summary>
        /// <param name="openGLVersion">The desired OpenGL version.</param>
        /// <param name="gl">The OpenGL context.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bitDepth">The bit depth.</param>
        /// <param name="parameter">The parameter</param>
        /// <returns></returns>
        public override bool Create(OpenGLVersion openGLVersion, OpenGL gl, int width, int height, int bitDepth, object parameter)
        {
            //  Call the base.
            base.Create(openGLVersion, gl, width, height, bitDepth, parameter);

            //	Create a new window class, as basic as possible.                
            wndClass = new Win32.WNDCLASSEX();
            wndClass.Init();
		    wndClass.style			= Win32.ClassStyles.HorizontalRedraw | Win32.ClassStyles.VerticalRedraw | Win32.ClassStyles.OwnDC;
            wndClass.lpfnWndProc    = wndProcDelegate;
		    wndClass.cbClsExtra		= 0;
		    wndClass.cbWndExtra		= 0;
		    wndClass.hInstance		= IntPtr.Zero;
		    wndClass.hIcon			= IntPtr.Zero;
		    wndClass.hCursor		= IntPtr.Zero;
		    wndClass.hbrBackground	= IntPtr.Zero;
		    wndClass.lpszMenuName	= null;
		    wndClass.lpszClassName	= "SharpGLRenderWindow";
		    wndClass.hIconSm		= IntPtr.Zero;
		    Win32.RegisterClassEx(ref wndClass);
            	
		    //	Create the window. Position and size it.
		    windowHandle = Win32.CreateWindowEx(0,
					      "SharpGLRenderWindow",
					      "",
					      Win32.WindowStyles.WS_CLIPCHILDREN | Win32.WindowStyles.WS_CLIPSIBLINGS | Win32.WindowStyles.WS_POPUP,
					      0, 0, width, height,
					      IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

		    //	Get the window device context.
		    deviceContextHandle = Win32.GetDC(windowHandle);

		    //	Setup a pixel format.
		    Win32.PIXELFORMATDESCRIPTOR pfd = new Win32.PIXELFORMATDESCRIPTOR();
            pfd.Init();
		    pfd.nVersion = 1;
		    pfd.dwFlags = Win32.PFD_DRAW_TO_WINDOW | Win32.PFD_SUPPORT_OPENGL | Win32.PFD_DOUBLEBUFFER;
		    pfd.iPixelType = Win32.PFD_TYPE_RGBA;
		    pfd.cColorBits = (byte)bitDepth;
		    pfd.cDepthBits = 16;
		    pfd.cStencilBits = 8;
		    pfd.iLayerType = Win32.PFD_MAIN_PLANE;
		
		    //	Match an appropriate pixel format 
		    int iPixelformat;
		    if((iPixelformat = Win32.ChoosePixelFormat(deviceContextHandle, pfd)) == 0 )
			    return false;

		    //	Sets the pixel format
            if (Win32.SetPixelFormat(deviceContextHandle, iPixelformat, pfd) == 0)
		    {
			    return false;
		    }

		    //	Create the render context.
            renderContextHandle = Win32.wglCreateContext(deviceContextHandle);
            
            //  Make the context current.
            MakeCurrent();

            //  Update the context if required.
            UpdateContextVersion(gl);

            //  Return success.
            return true;
        }

        private static Win32.WndProc wndProcDelegate = new Win32.WndProc(WndProc);

        static private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            return Win32.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        /// <summary>
        /// Destroys the render context provider instance.
        /// </summary>
	    public override void Destroy()
	    {
		    //	Release the device context.
		    Win32.ReleaseDC(windowHandle, deviceContextHandle);

		    //	Destroy the window.
		    Win32.DestroyWindow(windowHandle);

            //  Unregister Class
            Win32.UnregisterClass(wndClass.lpszClassName, wndClass.hInstance);

		    //	Call the base, which will delete the render context handle.
            base.Destroy();
	    }

        /// <summary>
        /// Sets the dimensions of the render context provider.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
	    public override void SetDimensions(int width, int height)
	    {
            //  Call the base.
            base.SetDimensions(width, height);

		    //	Set the window size.
            Win32.SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, Width, Height, 
                Win32.SetWindowPosFlags.SWP_NOACTIVATE | 
                Win32.SetWindowPosFlags.SWP_NOCOPYBITS | 
                Win32.SetWindowPosFlags.SWP_NOMOVE | 
                Win32.SetWindowPosFlags.SWP_NOOWNERZORDER);
	    }

        /// <summary>
        /// Blit the rendered data to the supplied device context.
        /// </summary>
        /// <param name="hdc">The HDC.</param>
	    public override void Blit(IntPtr hdc)
	    {
		    if(deviceContextHandle != IntPtr.Zero || windowHandle != IntPtr.Zero)
		    {
			    //	Swap the buffers.
                Win32.SwapBuffers(deviceContextHandle);
			    
			    //  Get the HDC for the graphics object.
                Win32.BitBlt(hdc, 0, 0, Width, Height, deviceContextHandle, 0, 0, Win32.SRCCOPY);
		    }
	    }

        /// <summary>
        /// Makes the render context current.
        /// </summary>
	    public override void MakeCurrent()
	    {
		    if(renderContextHandle != IntPtr.Zero)
			    Win32.wglMakeCurrent(deviceContextHandle, renderContextHandle);
	    }

        /// <summary>
        /// The window handle.
        /// </summary>
        protected IntPtr windowHandle = IntPtr.Zero;
    }
}
