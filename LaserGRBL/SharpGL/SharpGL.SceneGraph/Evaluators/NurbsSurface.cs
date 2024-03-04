using System;
using System.ComponentModel;

using SharpGL.SceneGraph.Evaluators;

namespace SharpGL.SceneGraph.Evaluators
{
	/// <summary>
	/// A NURBS Surface is a two dimensional non uniform B-Spline.
	/// </summary>
	[Serializable()]
	public class NurbsSurface : NurbsBase
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="NurbsSurface"/> class.
        /// </summary>
		public NurbsSurface()
		{
			Name = "NURBS Surface";
			ControlPoints.CreateGrid(4, 4);
		}

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public override void Render(OpenGL gl, Core.RenderMode renderMode)
        {
            base.Render(gl, renderMode);

            //	Begin drawing a NURBS surface.
            gl.BeginSurface(nurbsRenderer);

            //	Draw the surface.
            gl.NurbsSurface(nurbsRenderer,		//	The internal nurbs object.
                sKnots.Length,					//	Number of s-knots.
                sKnots,							//	The s-knots themselves.
                tKnots.Length,					//	The number of t-knots.
                tKnots,							//	The t-knots themselves.
                ControlPoints.Width * 3,		//	The size of a row of control points.
                3,								//	The size of a control points.
                ControlPoints.ToFloatArray(),	//	The control points.
                ControlPoints.Width,			//	The order, i.e degree + 1.
                ControlPoints.Height,			//	The order, i.e degree + 1.
                OpenGL.GL_MAP2_VERTEX_3);			//	Type of data to generate.

            //	End the surface.
            gl.EndSurface(nurbsRenderer);

            //	Draw the control points.
            ControlPoints.Draw(gl, DrawControlPoints, DrawControlGrid);
        }

        /// <summary>
        /// The s knots.
        /// </summary>
		private float[] sKnots = new float [] {0, 0, 0, 0, 1, 1, 1, 1};

        /// <summary>
        /// The t knots.
        /// </summary>
        private float[] tKnots = new float[] { 0, 0, 0, 0, 1, 1, 1, 1 };

        /// <summary>
        /// Gets the knots.
        /// </summary>
        [Description("The S knots."), Category("NURBS")]
        public float[] SKnots
        {
            get { return sKnots; }
        }

        /// <summary>
        /// Gets the knots.
        /// </summary>
        [Description("The T knots."), Category("NURBS")]
        public float[] TKnots
        {
            get { return tKnots; }
        }
	}
}
