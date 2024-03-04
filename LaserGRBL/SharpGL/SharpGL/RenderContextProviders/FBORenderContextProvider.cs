using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.Version;

namespace SharpGL.RenderContextProviders
{
    public class FBORenderContextProvider : HiddenWindowRenderContextProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FBORenderContextProvider"/> class.
        /// </summary>
        public FBORenderContextProvider()
        {
            //  We can layer GDI drawing on top of open gl drawing.
            GDIDrawingEnabled = true;
        }

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
            this.gl = gl;

            //  Call the base class. 	        
            base.Create(openGLVersion, gl, width, height, bitDepth, parameter);

            CreateDBOs(openGLVersion, gl, width, height, bitDepth, parameter);

            return true;
        }

        /// <summary>
        /// Creates the render context provider with sharing lists of another render context. 
        /// Must also create the OpenGL extensions.
        /// </summary>
        /// <param name="openGLVersion">The desired OpenGL version.</param>
        /// <param name="gl">The OpenGL context.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bitDepth">The bit depth.</param>
        /// <param name="parameter">The parameter</param>
        /// <param name="hrc">Render context handle to share lists with</param>
        /// <returns></returns>
        public bool CreateWithShareLists(OpenGLVersion openGLVersion, OpenGL gl, int width, int height, int bitDepth, object parameter, IntPtr hrc)
        {
            this.gl = gl;

            //  Call the base class. 	        
            base.Create(openGLVersion, gl, width, height, bitDepth, parameter);

            bool shareStatus = Win32.wglShareLists(hrc, this.RenderContextHandle);

            CreateDBOs(openGLVersion, gl, width, height, bitDepth, parameter);

            return shareStatus;
        }

        private void CreateDBOs(OpenGLVersion openGLVersion, OpenGL gl, int width, int height, int bitDepth, object parameter)
        {
            uint[] ids = new uint[1];

            //  First, create the frame buffer and bind it.
            ids = new uint[1];
            gl.GenFramebuffersEXT(1, ids);
            frameBufferID = ids[0];
            gl.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, frameBufferID);

            //	Create the colour render buffer and bind it, then allocate storage for it.
            gl.GenRenderbuffersEXT(1, ids);
            colourRenderBufferID = ids[0];
            gl.BindRenderbufferEXT(OpenGL.GL_RENDERBUFFER_EXT, colourRenderBufferID);
            gl.RenderbufferStorageEXT(OpenGL.GL_RENDERBUFFER_EXT, OpenGL.GL_RGBA, width, height);

            //	Create the depth render buffer and bind it, then allocate storage for it.
            gl.GenRenderbuffersEXT(1, ids);
            depthRenderBufferID = ids[0];
            gl.BindRenderbufferEXT(OpenGL.GL_RENDERBUFFER_EXT, depthRenderBufferID);
            gl.RenderbufferStorageEXT(OpenGL.GL_RENDERBUFFER_EXT, OpenGL.GL_DEPTH_COMPONENT24, width, height);

            //  Set the render buffer for colour and depth.
            gl.FramebufferRenderbufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, OpenGL.GL_COLOR_ATTACHMENT0_EXT,
                OpenGL.GL_RENDERBUFFER_EXT, colourRenderBufferID);
            gl.FramebufferRenderbufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, OpenGL.GL_DEPTH_ATTACHMENT_EXT,
                OpenGL.GL_RENDERBUFFER_EXT, depthRenderBufferID);

            dibSectionDeviceContext = Win32.CreateCompatibleDC(deviceContextHandle);

            //  Create the DIB section.
            dibSection.Create(dibSectionDeviceContext, width, height, bitDepth);
        }

        private void DestroyFramebuffers()
        {
            //  Delete the render buffers.
            gl.DeleteRenderbuffersEXT(2, new uint[] { colourRenderBufferID, depthRenderBufferID });

            //	Delete the framebuffer.
            gl.DeleteFramebuffersEXT(1, new uint[] { frameBufferID });

            //  Reset the IDs.
            colourRenderBufferID = 0;
            depthRenderBufferID = 0;
            frameBufferID = 0;
        }

        public override void Destroy()
        {
            //  Delete the render buffers.
            DestroyFramebuffers();

            //  Destroy the internal dc.
            Win32.DeleteDC(dibSectionDeviceContext);

            //	Call the base, which will delete the render context handle and window.
            base.Destroy();
        }

        public override void SetDimensions(int width, int height)
        {
            //  Call the base.
            base.SetDimensions(width, height);

            //	Resize dib section.
            dibSection.Resize(width, height, BitDepth);

            DestroyFramebuffers();

            //  TODO: We should be able to just use the code below - however we 
            //  get invalid dimension issues at the moment, so recreate for now.

            /*
            //  Resize the render buffer storage.
            gl.BindRenderbufferEXT(OpenGL.GL_RENDERBUFFER_EXT, colourRenderBufferID);
            gl.RenderbufferStorageEXT(OpenGL.GL_RENDERBUFFER_EXT, OpenGL.GL_RGBA, width, height);
            gl.BindRenderbufferEXT(OpenGL.GL_RENDERBUFFER_EXT, depthRenderBufferID);
            gl.RenderbufferStorageEXT(OpenGL.GL_RENDERBUFFER_EXT, OpenGL.GL_DEPTH_ATTACHMENT_EXT, width, height);
            var complete = gl.CheckFramebufferStatusEXT(OpenGL.GL_FRAMEBUFFER_EXT);
            */

            uint[] ids = new uint[1];

            //  First, create the frame buffer and bind it.
            ids = new uint[1];
            gl.GenFramebuffersEXT(1, ids);
            frameBufferID = ids[0];
            gl.BindFramebufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, frameBufferID);

            //	Create the colour render buffer and bind it, then allocate storage for it.
            gl.GenRenderbuffersEXT(1, ids);
            colourRenderBufferID = ids[0];
            gl.BindRenderbufferEXT(OpenGL.GL_RENDERBUFFER_EXT, colourRenderBufferID);
            gl.RenderbufferStorageEXT(OpenGL.GL_RENDERBUFFER_EXT, OpenGL.GL_RGBA, width, height);

            //	Create the depth render buffer and bind it, then allocate storage for it.
            gl.GenRenderbuffersEXT(1, ids);
            depthRenderBufferID = ids[0];
            gl.BindRenderbufferEXT(OpenGL.GL_RENDERBUFFER_EXT, depthRenderBufferID);
            gl.RenderbufferStorageEXT(OpenGL.GL_RENDERBUFFER_EXT, OpenGL.GL_DEPTH_COMPONENT24, width, height);

            //  Set the render buffer for colour and depth.
            gl.FramebufferRenderbufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, OpenGL.GL_COLOR_ATTACHMENT0_EXT,
                OpenGL.GL_RENDERBUFFER_EXT, colourRenderBufferID);
            gl.FramebufferRenderbufferEXT(OpenGL.GL_FRAMEBUFFER_EXT, OpenGL.GL_DEPTH_ATTACHMENT_EXT,
                OpenGL.GL_RENDERBUFFER_EXT, depthRenderBufferID);
        }

        public override void Blit(IntPtr hdc)
        {
            if (deviceContextHandle != IntPtr.Zero)
            {
                //  Set the read buffer.
                gl.ReadBuffer(OpenGL.GL_COLOR_ATTACHMENT0_EXT);

                //	Read the pixels into the DIB section.
                gl.ReadPixels(0, 0, Width, Height, OpenGL.GL_BGRA, 
                    OpenGL.GL_UNSIGNED_BYTE, dibSection.Bits);

                //	Blit the DC (containing the DIB section) to the target DC.
                Win32.BitBlt(hdc, 0, 0, Width, Height,
                    dibSectionDeviceContext, 0, 0, Win32.SRCCOPY);
            }
        }

        protected uint colourRenderBufferID = 0;
        protected uint depthRenderBufferID = 0;
        protected uint frameBufferID = 0;
        protected IntPtr dibSectionDeviceContext = IntPtr.Zero;
        protected DIBSection dibSection = new DIBSection();
        protected OpenGL gl;

        /// <summary>
        /// Gets the internal DIB section.
        /// </summary>
        /// <value>The internal DIB section.</value>
        public DIBSection InternalDIBSection
        {
            get { return dibSection; }
        }
    }
}
