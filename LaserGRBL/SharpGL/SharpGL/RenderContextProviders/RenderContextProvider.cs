using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.Version;

namespace SharpGL.RenderContextProviders
{
    public abstract class RenderContextProvider : IRenderContextProvider
    {
        /// <summary>
        /// Creates the render context provider. Must also create the OpenGL extensions.
        /// </summary>
        /// <param name="openGLVersion">The desired OpenGL version.</param>
        /// <param name="gl">The OpenGL context.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bitDepth">The bit depth.</param>
        /// <param name="parameter">The extra parameter.</param>
        /// <returns></returns>
        public virtual bool Create(OpenGLVersion openGLVersion, OpenGL gl, int width, int height, int bitDepth, object parameter)
        {
	        //  Set the width, height and bit depth.
            Width = width;
            Height = height;
            BitDepth = bitDepth;

            //  For now, assume we're going to be able to create the requested OpenGL version.
            requestedOpenGLVersion = openGLVersion;
            createdOpenGLVersion = openGLVersion;

            return true;
        }

        /// <summary>
        /// Destroys the render context provider instance.
        /// </summary>
        public virtual void Destroy()
        {
            //  If we have a render context, destroy it.
            if(renderContextHandle != IntPtr.Zero)
            {
                Win32.wglDeleteContext(renderContextHandle);
                renderContextHandle = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Sets the dimensions of the render context provider.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public virtual void SetDimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Makes the render context current.
        /// </summary>
        public abstract void MakeCurrent();

        /// <summary>
        /// Blit the rendered data to the supplied device context.
        /// </summary>
        /// <param name="hdc">The HDC.</param>
        public abstract void Blit(IntPtr hdc);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            //  Destroy the context provider.
            Destroy();
        }

        /// <summary>
        /// Only valid to be called after the render context is created, this function attempts to
        /// move the render context to the OpenGL version originally requested. If this is &gt; 2.1, this
        /// means building a new context. If this fails, we'll have to make do with 2.1.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        protected void UpdateContextVersion(OpenGL gl)
        {
            //  If the request version number is anything up to and including 2.1, standard render contexts
            //  will provide what we need (as long as the graphics card drivers are up to date).
            var requestedVersionNumber = VersionAttribute.GetVersionAttribute(requestedOpenGLVersion);
            if (requestedVersionNumber.IsAtLeastVersion(3, 0) == false)
            {
                createdOpenGLVersion = requestedOpenGLVersion;
                return;
            }

            //  Now the none-trivial case. We must use the WGL_ARB_create_context extension to 
            //  attempt to create a 3.0+ context.
            try
            {
                int[] attributes = 
                {
                    OpenGL.WGL_CONTEXT_MAJOR_VERSION_ARB, requestedVersionNumber.Major,  
                    OpenGL.WGL_CONTEXT_MINOR_VERSION_ARB, requestedVersionNumber.Minor,
                    OpenGL.WGL_CONTEXT_FLAGS_ARB, OpenGL.WGL_CONTEXT_FORWARD_COMPATIBLE_BIT_ARB,
                    0
                };
                var hrc = gl.CreateContextAttribsARB(IntPtr.Zero, attributes);
                Win32.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero);
                Win32.wglDeleteContext(renderContextHandle);
                Win32.wglMakeCurrent(deviceContextHandle, hrc);
                renderContextHandle = hrc; 
            }
            catch(Exception)
            {
                //  TODO: can we actually get the real version?
                createdOpenGLVersion = OpenGLVersion.OpenGL2_1;
            }
        }

        /// <summary>
        /// Gets the render context handle.
        /// </summary>
        public IntPtr RenderContextHandle
        {
            get { return renderContextHandle; }
            protected set { renderContextHandle = value; }
        }

        /// <summary>
        /// Gets the device context handle.
        /// </summary>
        public IntPtr DeviceContextHandle
        {
            get { return deviceContextHandle; }
            protected set { deviceContextHandle = value; }
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
            get { return height; }
            protected set { height = value; } 
        }

        /// <summary>
        /// Gets or sets the bit depth.
        /// </summary>
        /// <value>The bit depth.</value>
        public int BitDepth
        {
            get { return bitDepth; }
            protected set { bitDepth = value; } 
        }

        /// <summary>
        /// Gets a value indicating whether GDI drawing is enabled for this type of render context.
        /// </summary>
        /// <value><c>true</c> if GDI drawing is enabled; otherwise, <c>false</c>.</value>
        public bool GDIDrawingEnabled
        {
            get { return gdiDrawingEnabled; }
            protected set { gdiDrawingEnabled = value; }
        }

        /// <summary>
        /// Gets the OpenGL version that was requested when creating the render context.
        /// </summary>
        public OpenGLVersion RequestedOpenGLVersion
        {
            get { return requestedOpenGLVersion; }
        }

        /// <summary>
        /// Gets the OpenGL version that is supported by the render context, compare to <see cref="RequestedOpenGLVersion"/>.
        /// </summary>
        public OpenGLVersion CreatedOpenGLVersion
        {
            get { return createdOpenGLVersion; }
        }

        /// <summary>
        /// The render context handle.
        /// </summary>
        protected IntPtr renderContextHandle = IntPtr.Zero;

        /// <summary>
        /// The device context handle.
        /// </summary>
        protected IntPtr deviceContextHandle = IntPtr.Zero;

        /// <summary>
        /// The width.
        /// </summary>
        protected int width = 0;

        /// <summary>
        /// The height.
        /// </summary>
        protected int height = 0;

        /// <summary>
        /// The bit depth.
        /// </summary>
        protected int bitDepth = 0;

        /// <summary>
        /// Is gdi drawing enabled?
        /// </summary>
        protected bool gdiDrawingEnabled = true;

        /// <summary>
        /// The version of OpenGL that was requested when creating the render context.
        /// </summary>
        protected OpenGLVersion requestedOpenGLVersion;

        /// <summary>
        /// The actual version of OpenGL that is supported by the render context.
        /// </summary>
        protected OpenGLVersion createdOpenGLVersion;

    }
}
