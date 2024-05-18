using System;
using System.Collections;
using System.ComponentModel;

using SharpGL.SceneGraph.Collections;
using System.Collections.Generic;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Helpers;
using SharpGL.SceneGraph.Transformations;

namespace SharpGL.SceneGraph.Evaluators
{
	/// <summary>
	/// This is the base class of all evaluators, 1D, 2D etc. It is also the base class
	/// for the NURBS, as they share alot of common code, such as the VertexGrid.
	/// </summary>
	[Serializable()]
	public abstract class Evaluator : 
        SceneElement, 
        IHasObjectSpace, 
        IRenderable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Evaluator"/> class.
        /// </summary>
		public Evaluator()
		{
			Name = "Evaluator";
        }

        /// <summary>
        /// Pushes us into Object Space using the transformation into the specified OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void PushObjectSpace(OpenGL gl)
        {
            //  Use the helper to push us into object space.
            hasObjectSpaceHelper.PushObjectSpace(gl);
        }

        /// <summary>
        /// Pops us from Object Space using the transformation into the specified OpenGL instance.
        /// </summary>
        /// <param name="gl">The gl.</param>
        public void PopObjectSpace(OpenGL gl)
        {
            //  Use the helper to pop us from object space.
            hasObjectSpaceHelper.PopObjectSpace(gl);
        }

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public abstract void Render(OpenGL gl, RenderMode renderMode);

        /// <summary>
        /// The control points.
        /// </summary>
		private VertexGrid controlPoints = new VertexGrid();

        /// <summary>
        /// Draw points flag.
        /// </summary>
        private bool drawPoints = true;

        /// <summary>
        /// Draw lines flag.
        /// </summary>
        private bool drawLines = true;

        /// <summary>
        /// The IHasObjectSpace helper.
        /// </summary>
        private HasObjectSpaceHelper hasObjectSpaceHelper = new HasObjectSpaceHelper();

        /// <summary>
        /// Gets or sets the control points.
        /// </summary>
        /// <value>
        /// The control points.
        /// </value>
        [Description("The control points."), Category("Evaluator")]
        public VertexGrid ControlPoints
		{
			get {return controlPoints;}
			set {controlPoints = value; }
		}

        /// <summary>
        /// Gets or sets a value indicating whether [draw control points].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [draw control points]; otherwise, <c>false</c>.
        /// </value>
        [Description("Should the control points be drawn?"), Category("Evaluator")]
		public bool DrawControlPoints
		{
			get {return drawPoints;}
			set {drawPoints = value; }
		}

        /// <summary>
        /// Gets or sets a value indicating whether [draw control grid].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [draw control grid]; otherwise, <c>false</c>.
        /// </value>
        [Description("Should the control grid be drawn?"), Category("Evaluator")]
		public bool DrawControlGrid
		{
			get {return drawLines;}
			set {drawLines = value; }
		}

        /// <summary>
        /// Gets the transformation that pushes us into object space.
        /// </summary>
        [Description("The Quadric Object Space Transformation"), Category("Evaluator")]
        public LinearTransformation Transformation
        {
            get { return hasObjectSpaceHelper.Transformation; }
        }
	}
}