using System;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Lighting;

namespace SharpGL.SceneGraph.Cameras
{
	/// <summary>
	/// The ArcBall camera supports arcball projection, making it ideal for use with a mouse.
	/// </summary>
	[Serializable()]
	public class ArcBallCamera : PerspectiveCamera
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="PerspectiveCamera"/> class.
        /// </summary>
        public ArcBallCamera()
		{
			Name = "Camera (ArcBall)";
		}

		/// <summary>
		/// This is the class' main function, to override this function and perform a 
		/// perspective transformation.
		/// </summary>
        public override void TransformProjectionMatrix(OpenGL gl)
        {
            int[] viewport = new int[4];
            gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);

            //  Perform the perspective transformation.
            arcBall.SetBounds(viewport[2], viewport[3]);
            gl.Perspective(FieldOfView, AspectRatio, Near, Far);
            Vertex target = new Vertex(0, 0, 0);
            Vertex upVector = new Vertex(0, 0, 1);

            //  Perform the look at transformation.
            gl.LookAt((double)Position.X, (double)Position.Y, (double)Position.Z,
                (double)target.X, (double)target.Y, (double)target.Z,
                (double)upVector.X, (double)upVector.Y, (double)upVector.Z);

            arcBall.TransformMatrix(gl);
		}

        /// <summary>
        /// The arcball used for rotating.
        /// </summary>
        private ArcBall arcBall = new ArcBall();

        /// <summary>
        /// Gets the arc ball.
        /// </summary>
        public ArcBall ArcBall
        {
            get { return arcBall; }
        }
	}
}
