using System;
using System.Collections;
using System.ComponentModel;

using SharpGL.SceneGraph.Collections;
using System.Collections.Generic;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Evaluators
{
	/// <summary>
	/// This is a 2D evaluator, i.e a bezier patch.
	/// </summary>
	[Serializable()]
	public class Evaluator2D : Evaluator
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Evaluator2D"/> class.
        /// </summary>
		public Evaluator2D()
		{
			ControlPoints.CreateGrid(4, 4);
			Name = "2D Evaluator (Bezier Patch)";
			Transformation.RotateX = 180;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Evaluator2D"/> class.
        /// </summary>
        /// <param name="u">The u.</param>
        /// <param name="v">The v.</param>
		public Evaluator2D(int u, int v)
		{
			ControlPoints.CreateGrid(u, v);
			Name = "2D Evaluator (Bezier Patch)";
            Transformation.RotateX = 180;
		}

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public override void Render(OpenGL gl, RenderMode renderMode)
		{			
			//	Create the evaluator.
            gl.Map2(OpenGL.GL_MAP2_VERTEX_3,//	Use and produce 3D points.
				0,								//	Low order value of 'u'.
				1,								//	High order value of 'u'.
				3,								//	Size (bytes) of a control point.
				ControlPoints.Width,			//	Order (i.e degree plus one).
				0,								//	Low order value of 'v'.
				1,								//	High order value of 'v'
				ControlPoints.Width * 3,		//	Size in bytes of a 'row' of points.
				ControlPoints.Height,			//	Order (i.e degree plus one).
				ControlPoints.ToFloatArray());	//	The control points.

            gl.Enable(OpenGL.GL_MAP2_VERTEX_3);
            gl.Enable(OpenGL.GL_AUTO_NORMAL);
			gl.MapGrid2(20, 0, 1, 20, 0, 1);
			
			//	Now draw it.
            gl.EvalMesh2(OpenGL.GL_FILL, 0, 20, 0, 20);

			//	Draw the control points.
            ControlPoints.Draw(gl, DrawControlPoints, DrawControlGrid);
		}

        /// <summary>
        /// The segments.
        /// </summary>
		private int segments = 30;

        /// <summary>
        /// Gets or sets the segments.
        /// </summary>
        /// <value>
        /// The segments.
        /// </value>
        [Description("The number of segments."), Category("Evaluator")]
		public int Segments
		{
			get {return segments;}
			set {segments = value; }
		}
	}
}