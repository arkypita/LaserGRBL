using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Effects;
using SharpGL.SceneGraph.Core;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph.Shaders
{
    /// <summary>
    /// The Shader base class.
    /// </summary>
    public class ShaderProgram : 
        Effect,
        IHasOpenGLContext
    {
        /// <summary>
        /// Pops the effect off the specified parent element.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="parentElement">The parent element.</param>
        public override void Pop(OpenGL gl, SceneElement parentElement)
        {
            //  Un-use this shader object.
            gl.UseProgram(0);
        }

        /// <summary>
        /// Pushes the effect onto the specified parent element.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="parentElement">The parent element.</param>
        public override void Push(OpenGL gl, SceneElement parentElement)
        {
            //  Use this shader object.
            gl.UseProgram(ProgramObject);
        }

        /// <summary>
        /// Create in the context of the supplied OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void CreateInContext(OpenGL gl)
        {
            //  Create the program.
            ProgramObject = gl.CreateProgram();
            CurrentOpenGLContext = gl;
        }

        /// <summary>
        /// Destroy in the context of the supplied OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void DestroyInContext(OpenGL gl)
        {
            //  Delete the shader.
            gl.DeleteProgram(ProgramObject);
            ProgramObject = 0;
            CurrentOpenGLContext = null;
        }

        /// <summary>
        /// Attaches a shader.
        /// </summary>
        /// <param name="shader">The shader.</param>
        public void AttachShader(Shader shader)
        {
            //  Attach the shader.
            CurrentOpenGLContext.AttachShader(ProgramObject, shader.ShaderObject);

            //  Add it to the list.
            attachedShaders.Add(shader);
        }

        /// <summary>
        /// Detaches the shader.
        /// </summary>
        /// <param name="shader">The shader.</param>
        public void DetachShader(Shader shader)
        {
            //  Detach the shader.
            CurrentOpenGLContext.DetachShader(ProgramObject, shader.ShaderObject);

            //  Remove it from the list.
            attachedShaders.Remove(shader);
        }

        /// <summary>
        /// Links this instance.
        /// </summary>
        public void Link()
        {
            //  Link the program.
            CurrentOpenGLContext.LinkProgram(ProgramObject);
        }

        /// <summary>
        /// Gets the uniform location.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public int GetUniformLocation(string name)
        {
            //  Get the uniform location.
            return CurrentOpenGLContext.GetUniformLocation(ProgramObject, name);
        }

        /// <summary>
        /// Sets the full shader source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void SetFullShaderSource(string source)
        {
            //  To compile a full shader, we must have a file like this:
            //  -- Vertex
            //  <a vertex shader>
            //  -- Fragment
            //  <a fragment shader>

            //  Prepare the source code.
            string vertexShader = string.Empty;
            string fragmentShader = string.Empty;

            //  Keep track of what we're building, 0 is nothing
            //  1 is the vertex shader, 2 is the fragment shader.
            int code = 0;

            //  Go through the source line by line.
            var lines = source.Split('\n');
            foreach (var line in lines)
            {
                //  If the line is a vertex shader, switch to vertex mode.
                if (line.StartsWith("--") && line.Contains("Vertex"))
                {
                    code = 1;
                }
                else if (line.StartsWith("--") && line.Contains("Fragment"))
                {
                    code = 2;
                }
                else
                {
                    //  Add the line to the appropriate shader.
                    switch (code)
                    {
                        case 1:
                            vertexShader += line + Environment.NewLine;
                            break;
                        case 2:
                            fragmentShader += line + Environment.NewLine;
                            break;
                    }
                }
            }

            //  If we have a vertex shader, build it.
            if (string.IsNullOrEmpty(vertexShader) == false)
            {
                VertexShader shader = new VertexShader();
                shader.CreateInContext(CurrentOpenGLContext);
                shader.SetSource(vertexShader);
                AttachShader(shader);
            }
            if (string.IsNullOrEmpty(fragmentShader) == false)
            {
                FragmentShader shader = new FragmentShader();
                shader.CreateInContext(CurrentOpenGLContext);
                shader.SetSource(fragmentShader);
                AttachShader(shader);
            }
        }

        /*todo uniform1,2,3,4
        public void Uniform(float float1)
        {
            CurrentOpenGLContext.Uniform1(
        }*/
        
        /// <summary>
        /// The set of attached shaders.
        /// </summary>
        private List<Shader> attachedShaders = new List<Shader>();

        /// <summary>
        /// Gets or sets the shader object.
        /// </summary>
        /// <value>
        /// The shader object.
        /// </value>
        [Category("Program"), Description("The internal program object.")]
        [XmlIgnore]
        [Browsable(false)]
        public uint ProgramObject
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the current OpenGL that the object exists in context.
        /// </summary>
        [Category("Program"), Description("The current context.")]
        [XmlIgnore]
        [Browsable(false)]
        public OpenGL CurrentOpenGLContext
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the attached shaders.
        /// </summary>
        [Category("Program"), Description("The attached shaders.")]
        [XmlIgnore]
        public IEnumerable<Shader> AttachedShaders
        {
            get { return attachedShaders; }
        }

        /// <summary>
        /// Gets the compile status.
        /// </summary>
        [Category("Program"), Description("The link status.")]
        public bool? LinkStatus
        {
            get
            {
                if (ProgramObject == 0 || CurrentOpenGLContext == null)
                    return null;

                //  Get the compile info.
                int[] parameters = new int[] { 0 };
                CurrentOpenGLContext.GetProgram(ProgramObject, OpenGL.GL_LINK_STATUS,
                    parameters);

                return parameters[0] == OpenGL.GL_TRUE;
            }
        }

        /// <summary>
        /// Gets the info log.
        /// </summary>
        [Category("Program"), Description("The info log.")]
        public string InfoLog
        {
            get
            {
                if (ProgramObject == 0 || CurrentOpenGLContext == null)
                    return null;

                //  Get the info log length.
                int[] infoLength = new int[] { 0 };
                CurrentOpenGLContext.GetProgram(ProgramObject,
                    OpenGL.GL_INFO_LOG_LENGTH, infoLength);

                //  Get the compile info.
                StringBuilder il = new StringBuilder(1000);
                CurrentOpenGLContext.GetProgramInfoLog(ProgramObject, 10000,
                    IntPtr.Zero, il);

                return il.ToString();
            }
        }
    }
}
