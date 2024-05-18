namespace SharpGL.RenderContextProviders
{
    using System;

    /// <summary>
    /// Render context provider for working with an external render context
    /// </summary>
    public class ExternalRenderContextProvider : RenderContextProvider
    {
        /// <summary>
        /// The window handle.
        /// </summary>
        protected IntPtr windowHandle = IntPtr.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalRenderContextProvider"/> class.
        /// </summary>
        /// <param name="windowHandle">The existing window handle.</param>
        /// <param name="renderContextHandle">The handle to the existing render context.</param>
        /// <param name="deviceContextHandle">The handle to the existing device context.</param>
        public ExternalRenderContextProvider(IntPtr windowHandle, IntPtr renderContextHandle, IntPtr deviceContextHandle)
        {
            this.windowHandle = windowHandle;
            this.deviceContextHandle = deviceContextHandle;
            this.renderContextHandle = renderContextHandle;
        }

        /// <summary>
        /// Destroys the render context provider instance.
        /// </summary>
        public override void Destroy()
        {
            // Don't destroy the external context!
            // base.Destroy();
        }

        /// <summary>
        /// Blit the rendered data to the supplied device context.
        /// </summary>
        /// <param name="hdc">The HDC.</param>
        public override void Blit(IntPtr hdc)
        {
            // TODO: Should this do something in the case of an external context?
            if (this.deviceContextHandle != IntPtr.Zero || this.windowHandle != IntPtr.Zero)
            {
                //Swap the buffers.
                // Win32.SwapBuffers(deviceContextHandle);
            }
        }

        /// <summary>
        /// Makes the render context current.
        /// </summary>
        public override void MakeCurrent()
        {
            // TODO: Should this have an effect with an external context?
            // if (renderContextHandle != IntPtr.Zero)
            //      Win32.wglMakeCurrent(deviceContextHandle, renderContextHandle);
        }
    }
}