using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph.Shaders
{
    /// <summary>
    /// The Vertex Shader object.
    /// </summary>
    public class VertexShader : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexShader"/> class.
        /// </summary>
        public VertexShader()
        {
            Name = "Vertex Shader";
        }

        /// <summary>
        /// Create in the context of the supplied OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void CreateInContext(OpenGL gl)
        {
            //  Create the vertex shader.
            ShaderObject = gl.CreateShader(OpenGL.GL_VERTEX_SHADER);

            //  Store the current context.
            CurrentOpenGLContext = gl;
        }
    }
}
