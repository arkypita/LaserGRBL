using System;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Lighting;

namespace SharpGL.SceneGraph.Cameras
{
	/// <summary>
	/// This camera contains the data needed to perform an orthographic transformation
	/// to the projection matrix.
	/// </summary>
	[Serializable()]
	public class OrthographicCamera : Camera
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="OrthographicCamera"/> class.
        /// </summary>
		public OrthographicCamera()
		{
			Name = "Camera (Orthographic)";
		}

		/// <summary>
		/// This is the main function of the class, to perform a specialised projection
		/// in this case, an orthographic one.
		/// </summary>
        public override void TransformProjectionMatrix(OpenGL gl)
        {
            //  Perform the transformation.
            gl.Translate(Position.X, Position.Y, Position.Z); 
			gl.Ortho(left, right, bottom, top, near, far);
		}

        /// <summary>
        /// The left pos.
        /// </summary>
        private double left = 0.0f;

        /// <summary>
        /// The right pos.
        /// </summary>
        private double right = 1.0f;

        /// <summary>
        /// The top pos.
        /// </summary>
        private double top = 0.0f;

        /// <summary>
        /// The bottom pos.
        /// </summary>
        private double bottom = 1.0f;

        /// <summary>
        /// The near pos.
        /// </summary>
        private double near = 0.0f;

        /// <summary>
        /// The far pos.
        /// </summary>
        private double far = 100.0f;

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        [Description("The left clip"), Category("Camera")]
        public double Left
        {
            get { return left; }
            set { left = value; }
        }

        /// <summary>
        /// Gets or sets the right.
        /// </summary>
        /// <value>
        /// The right.
        /// </value>
        [Description("The right clip"), Category("Camera")]
        public double Right
        {
            get { return right; }
            set { right = value; }
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        [Description("The top clip"), Category("Camera")]
        public double Top
        {
            get { return top; }
            set { top = value; }
        }

        /// <summary>
        /// Gets or sets the bottom.
        /// </summary>
        /// <value>
        /// The bottom.
        /// </value>
        [Description("The bottom clip"), Category("Camera")]
        public double Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }

        /// <summary>
        /// Gets or sets the near.
        /// </summary>
        /// <value>
        /// The near.
        /// </value>
        [Description("The near clip"), Category("Camera")]
        public double Near
        {
            get { return near; }
            set { near = value; }
        }

        /// <summary>
        /// Gets or sets the far.
        /// </summary>
        /// <value>
        /// The far.
        /// </value>
        [Description("The far clip"), Category("Camera")]
        public double Far
        {
            get { return far; }
            set { far = value; }
        }
	}
}
