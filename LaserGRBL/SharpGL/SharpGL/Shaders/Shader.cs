using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.Shaders
{
    /// <summary>
    /// This is the base class for all shaders (vertex and fragment). It offers functionality
    /// which is core to all shaders, such as file loading and binding.
    /// </summary>
    public class Shader
    {
        public void Create(OpenGL gl, uint shaderType, string source)
        {
            //  Create the OpenGL shader object.
            shaderObject = gl.CreateShader(shaderType);

            //  Set the shader source.
            gl.ShaderSource(shaderObject, source);

            //  Compile the shader object.
            gl.CompileShader(shaderObject);

            //  Now that we've compiled the shader, check it's compilation status. If it's not compiled properly, we're
            //  going to throw an exception.
            if (GetCompileStatus(gl) == false)
            {
                throw new ShaderCompilationException(string.Format("Failed to compile shader with ID {0}.", shaderObject), GetInfoLog(gl));
            }
        }

        public void Delete(OpenGL gl)
        {
            gl.DeleteShader(shaderObject);
            shaderObject = 0;
        }

        public bool GetCompileStatus(OpenGL gl)
        {
            int[] parameters = new int[] { 0 };
            gl.GetShader(shaderObject, OpenGL.GL_COMPILE_STATUS, parameters);
            return parameters[0] == OpenGL.GL_TRUE;
        }

        public string GetInfoLog(OpenGL gl)
        {
            //  Get the info log length.
            int[] infoLength = new int[] { 0 };
            gl.GetShader(ShaderObject,
                OpenGL.GL_INFO_LOG_LENGTH, infoLength);
            int bufSize = infoLength[0];

            //  Get the compile info.
            StringBuilder il = new StringBuilder(bufSize);
            gl.GetShaderInfoLog(shaderObject, bufSize, IntPtr.Zero, il);

            return il.ToString();
        }

        /// <summary>
        /// The OpenGL shader object.
        /// </summary>
        private uint shaderObject;

        /// <summary>
        /// Gets the shader object.
        /// </summary>
        public uint ShaderObject
        {
            get { return shaderObject; }
        }
    }

    
}
