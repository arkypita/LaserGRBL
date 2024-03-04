using System;
using System.ComponentModel;

using SharpGL.SceneGraph.Evaluators;

namespace SharpGL.SceneGraph.Evaluators
{
	/// <summary>
	/// A NURBS Curve is a one dimensional non uniform B-Spline.
	/// </summary>
	[Serializable()]
	public class NurbsCurve : NurbsBase
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="NurbsCurve"/> class.
        /// </summary>
		public NurbsCurve()
		{
			Name = "NURBS Curve";
			ControlPoints.CreateGrid(4, 1);
		}

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public override void Render(OpenGL gl, Core.RenderMode renderMode)
        {
            //  Call the base.
            base.Render(gl, renderMode);
            
			//	Begin drawing a NURBS curve.
			gl.BeginCurve(nurbsRenderer);

			//	Draw the curve.
			gl.NurbsCurve(nurbsRenderer,		//	The internal nurbs object.
				knots.Length,					//	Number of knots.
				knots,							//	The knots themselves.
				3,								//	The size of a vertex.
				ControlPoints.ToFloatArray(),	//	The control points.
				ControlPoints.Width,			//	The order, i.e degree + 1.
				OpenGL.GL_MAP1_VERTEX_3);			//	Type of data to generate.

			//	End the curve.
			gl.EndCurve(nurbsRenderer);

			//	Draw the control points.
			ControlPoints.Draw(gl, DrawControlPoints, DrawControlGrid);
		}

        /// <summary>
        /// The knots.
        /// </summary>
		private float[] knots = new float [] {0, 0, 0, 0, 1, 1, 1, 1};

        /// <summary>
        /// Gets the knots.
        /// </summary>
        [Description("The knots."), Category("NURBS")]
        public float[] Knots
        {
            get { return knots; }
        }
	}
}
