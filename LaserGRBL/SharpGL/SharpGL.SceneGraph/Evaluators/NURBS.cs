using System;
using System.ComponentModel;

using SharpGL.SceneGraph.Evaluators;
using SharpGL.SceneGraph.Core;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph.Evaluators
{
	/// <summary>
	/// The NURBS class is the base for NURBS objects, such as curves and surfaces.
	/// </summary>
	[Serializable()]
	public abstract class NurbsBase : 
        Evaluator,
        IHasOpenGLContext
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="NurbsBase"/> class.
        /// </summary>
        public NurbsBase()
        {
            Name = "NURBS Object";
        }

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public override void Render(OpenGL gl, Core.RenderMode renderMode)
        {
			//	Set the properties.
            gl.NurbsProperty(nurbsRenderer, (int)OpenGL.GLU_DISPLAY_MODE, (float)displayMode);
        }

        /// <summary>
        /// Create in the context of the supplied OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void CreateInContext(OpenGL gl)
        {
            //  Create the NURBS renderer.
            nurbsRenderer = gl.NewNurbsRenderer();
            CurrentOpenGLContext = gl;
        }

        /// <summary>
        /// Destroy in the context of the supplied OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void DestroyInContext(OpenGL gl)
        {
            //  Destroy the NURBS renderer.
            gl.DeleteNurbsRenderer(nurbsRenderer);
            CurrentOpenGLContext = null;
        }

		public enum NurbsDisplayMode : uint
		{
            OutlinePatch = OpenGL.GLU_OUTLINE_PATCH,
            OutlinePolygon = OpenGL.GLU_OUTLINE_POLYGON,
			Fill = OpenGL.GLU_FILL,
		}

		/// <summary>
		/// This is the pointer to the underlying NURBS object.
		/// </summary>
		protected IntPtr nurbsRenderer = IntPtr.Zero;

        /// <summary>
        /// The display mode.
        /// </summary>
		private NurbsDisplayMode displayMode = NurbsDisplayMode.OutlinePolygon;

        /// <summary>
        /// Gets or sets the display mode.
        /// </summary>
        /// <value>
        /// The display mode.
        /// </value>
		[Description("The way that the NURBS object is rendered."), Category("NURBS")]
		public NurbsDisplayMode DisplayMode
		{
			get {return displayMode;}
			set {displayMode = value;}
		}

        /// <summary>
        /// Gets the current OpenGL that the object exists in context.
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public OpenGL CurrentOpenGLContext
        {
            get;
            protected set;
        }
    }
}
