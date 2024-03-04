using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Effects;
using SharpGL.SceneGraph.Core;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;

namespace SharpGL.SceneGraph.Shaders
{
    /// <summary>
    /// The Shader base class.
    /// </summary>
    public abstract class Shader : 
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
            
        }

        /// <summary>
        /// Pushes the effect onto the specified parent element.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="parentElement">The parent element.</param>
        public override void Push(OpenGL gl, SceneElement parentElement)
        {
        }

        /// <summary>
        /// Create in the context of the supplied OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public abstract void CreateInContext(OpenGL gl);

        /// <summary>
        /// Destroy in the context of the supplied OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void DestroyInContext(OpenGL gl)
        {
            //  Delete the shader.
            gl.DeleteShader(ShaderObject);
            ShaderObject = 0;
            CurrentOpenGLContext = null;
        }

        /// <summary>
        /// Sets the shader source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void SetSource(string source)
        {
            //  Set the shader source.
            CurrentOpenGLContext.ShaderSource(ShaderObject, source);
        }

        /// <summary>
        /// Loads the shader source.
        /// </summary>
        /// <param name="path">The path to the shader file.</param>
        public void LoadSource(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                StreamReader reader = new StreamReader(stream);
                SetSource(reader.ReadToEnd());
                reader.Close();
            }
        }

        /// <summary>
        /// Compiles this instance.
        /// </summary>
        public void Compile()
        {
            //  Compile the shader.
            CurrentOpenGLContext.CompileShader(ShaderObject);
        }

        /// <summary>
        /// The internal shader object.
        /// </summary>
        private uint shaderObject = 0;

        /// <summary>
        /// Gets or sets the shader object.
        /// </summary>
        /// <value>
        /// The shader object.
        /// </value>
        [Category("Shader"), Description("The internal shader object.")]
        [XmlIgnore]
        [Browsable(false)]
        public uint ShaderObject
        {
            get { return shaderObject; }
            protected set { shaderObject = value; }
        }

        /// <summary>
        /// Gets the current OpenGL that the object exists in context.
        /// </summary>
        [Category("Shader"), Description("The current context.")]
        [XmlIgnore]
        [Browsable(false)]
        public OpenGL CurrentOpenGLContext
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the compile status.
        /// </summary>
        [Category("Shader"), Description("The compile status.")]
        public bool? CompileStatus
        {
            get
            {
                if (ShaderObject == 0 || CurrentOpenGLContext == null)
                    return null;

                //  Get the compile info.
                int[] parameters = new int[] { 0 };
                CurrentOpenGLContext.GetShader(ShaderObject, OpenGL.GL_COMPILE_STATUS,
                    parameters);

                return parameters[0] == OpenGL.GL_TRUE;
            }
        }

        /// <summary>
        /// Gets the info log.
        /// </summary>
        [Category("Shader"), Description("The info log.")]
        public string InfoLog
        {
            get
            {
                if (ShaderObject == 0 || CurrentOpenGLContext == null)
                    return null;

                //  Get the info log length.
                int[] infoLength = new int[] { 0 };
                CurrentOpenGLContext.GetShader(ShaderObject,
                    OpenGL.GL_INFO_LOG_LENGTH, infoLength);

                //  Get the compile info.
                StringBuilder il = new StringBuilder(1000);
                CurrentOpenGLContext.GetShaderInfoLog(ShaderObject, 10000,
                   IntPtr.Zero, il);

                return il.ToString();
            }
        }

        /// <summary>
        /// Gets the source code.
        /// </summary>
        [Category("Shader"), Description("Source code.")]
        public string SourceCode
        {
            get
            {
                if (ShaderObject == 0 || CurrentOpenGLContext == null)
                    return null;

                //  Get the info log length.
                int[] infoLength = new int[] { 0 };
                CurrentOpenGLContext.GetShader(ShaderObject,
                    OpenGL.GL_INFO_LOG_LENGTH, infoLength);

                //  Get the source info.
                StringBuilder src = new StringBuilder(1000);
                CurrentOpenGLContext.GetShaderSource(ShaderObject, 10000,
                   IntPtr.Zero, src);

                return src.ToString();
            }
        }
    }
}
