using System;
using System.Collections;
using System.ComponentModel;

using SharpGL.SceneGraph.Collections;
using System.Collections.Generic;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Evaluators
{
	/// <summary>
	/// This is a 1D evaluator, i.e a bezier curve.
	/// </summary>
	[Serializable()]
	public class Evaluator1D : Evaluator
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Evaluator1D"/> class.
        /// </summary>
		public Evaluator1D()
		{
			//	Create a single line of points.
			ControlPoints.CreateGrid(4, 1);

			Transformation.RotateX = 180;

			Name = "1D Evaluator (Bezier Curve)";
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Evaluator1D"/> class.
        /// </summary>
        /// <param name="points">The points.</param>
		public Evaluator1D(int points)
		{
			//	Create a single line of points.
			ControlPoints.CreateGrid(points, 1);
			Name = "1D Evaluator (Bezier Curve)";
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
            gl.Map1(OpenGL.GL_MAP1_VERTEX_3,//	Use and produce 3D points.
                0,								//	Low order value of 'u'.
                1,								//	High order value of 'u'.
                3,								//	Size (bytes) of a control point.
                ControlPoints.Width,			//	Order (i.e degree plus one).
                ControlPoints.ToFloatArray());	//	The control points.

            //	Enable the type of evaluator we wish to use.
            gl.Enable(OpenGL.GL_MAP1_VERTEX_3);

            //	Beging drawing a line strip.
            gl.Begin(OpenGL.GL_LINE_STRIP);

            //	Now draw it.
            for (int i = 0; i <= segments; i++)
                gl.EvalCoord1((float)i / segments);

            gl.End();

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