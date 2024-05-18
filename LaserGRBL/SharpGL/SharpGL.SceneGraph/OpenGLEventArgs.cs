using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph
{
    /// <summary>
    /// The OpenGLEventArgs class.
    /// </summary>
    public class OpenGLEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLEventArgs"/> class.
        /// </summary>
        /// <param name="gl">The gl.</param>
        public OpenGLEventArgs(OpenGL gl)
        {
            OpenGL = gl;
        }

        /// <summary>
        /// The OpenGL instance.
        /// </summary>
        private OpenGL gl = null;

        /// <summary>
        /// Gets or sets the open GL.
        /// </summary>
        /// <value>The open GL.</value>
        public OpenGL OpenGL
        {
            get { return gl; }
            private set { gl = value; }
        }
    }

    /// <summary>
    /// The OpenGL Event Handler delegate.
    /// </summary>
    public delegate void OpenGLEventHandler(object sender, OpenGLEventArgs args);
}
