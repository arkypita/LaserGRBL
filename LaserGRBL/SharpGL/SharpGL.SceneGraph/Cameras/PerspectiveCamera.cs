using System;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Lighting;

namespace SharpGL.SceneGraph.Cameras
{
	/// <summary>
	/// This camera contains the data needed to perform a Perspective transformation
	/// to the projection matrix.
	/// </summary>
	[Serializable()]
	public class PerspectiveCamera : Camera
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="PerspectiveCamera"/> class.
        /// </summary>
		public PerspectiveCamera()
		{
			Name = "Camera (Perspective)";
		}

		/// <summary>
		/// This is the class' main function, to override this function and perform a 
		/// perspective transformation.
		/// </summary>
        public override void TransformProjectionMatrix(OpenGL gl)
		{
            //  Perform the perspective transformation.
            //gl.Translate(Position.X, Position.Y, Position.Z);
            gl.Perspective(fieldOfView, AspectRatio, near, far);
            Vertex target = new Vertex(0, 0, 0);
            Vertex upVector = new Vertex(0, 0, 1);
            //  Perform the look at transformation.
            gl.LookAt((double)Position.X, (double)Position.Y, (double)Position.Z,
                (double)target.X, (double)target.Y, (double)target.Z,
                (double)upVector.X, (double)upVector.Y, (double)upVector.Z);
		}

        /// <summary>
        /// The field of view. 
        /// </summary>
		private double fieldOfView = 60.0f;

        /// <summary>
        /// The near clip.
        /// </summary>
		private double near = 0.5f;

        /// <summary>
        /// The far flip.
        /// </summary>
        private double far = 40.0f;

        /// <summary>
        /// Gets or sets the field of view.
        /// </summary>
        /// <value>
        /// The field of view.
        /// </value>
		[Description("The angle of the lense of the camera (60 degrees = human eye)."), Category("Camera (Perspective")]
		public double FieldOfView
		{
			get {return fieldOfView;}
			set {fieldOfView = value;}
		}
		
        /// <summary>
        /// Gets or sets the near.
        /// </summary>
        /// <value>
        /// The near.
        /// </value>
		[Description("The near clipping distance."), Category("Camera (Perspective")]
		public double Near
		{
			get {return near;}
			set {near = value;}
		}

        /// <summary>
        /// Gets or sets the far.
        /// </summary>
        /// <value>
        /// The far.
        /// </value>
		[Description("The far clipping distance."), Category("Camera (Perspective")]
		public double Far
		{
			get {return far;}
			set {far = value;}
		}
	}
}
