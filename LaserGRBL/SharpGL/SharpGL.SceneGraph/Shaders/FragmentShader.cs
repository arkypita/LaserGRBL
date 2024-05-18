using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph.Shaders
{
    /// <summary>
    /// The Fragment Shader.
    /// </summary>
    public class FragmentShader : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FragmentShader"/> class.
        /// </summary>
        public FragmentShader()
        {
            Name = "Fragment Shader";
        }

        /// <summary>
        /// Create in the context of the supplied OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void CreateInContext(OpenGL gl)
        {
            //  Create the fragment shader.
            ShaderObject = gl.CreateShader(OpenGL.GL_FRAGMENT_SHADER);

            //  Store the current context.
            CurrentOpenGLContext = gl;
        }
    }
}
