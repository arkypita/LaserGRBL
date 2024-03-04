using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph.Helpers
{
    /// <summary>
    /// The OpenGL Helper is a class that provides some helper functions for working with OpenGL.
    /// </summary>
    public class OpenGLHelper
    {
        /// <summary>
        /// Initialises the supplied OpenGL instance for high quality rendering.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public static void InitialiseHighQuality(OpenGL gl)
        {
            //	Set parameters that give us some high quality settings.
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_NORMALIZE);
            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.ShadeModel(OpenGL.GL_SMOOTH);
            gl.LightModel(OpenGL.GL_LIGHT_MODEL_TWO_SIDE, OpenGL.GL_TRUE);
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
        }
    }
}
