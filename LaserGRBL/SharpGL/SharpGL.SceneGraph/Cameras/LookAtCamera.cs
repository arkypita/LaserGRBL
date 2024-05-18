using System;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Lighting;

namespace SharpGL.SceneGraph.Cameras
{
	/// <summary>
	/// The LookAt camera is a camera that does a 'look at' transformation.
	/// </summary>
	[Serializable()]
	public class LookAtCamera : PerspectiveCamera
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="LookAtCamera"/> class.
        /// </summary>
        public LookAtCamera()
		{
			Name = "Camera (Look At)";
		}

        /// <summary>
        /// This is the class' main function, to override this function and perform a 
        /// perspective transformation.
        /// </summary>
        public override void TransformProjectionMatrix(OpenGL gl)
        {
            //  Perform the look at transformation.
            gl.Perspective(FieldOfView, AspectRatio, Near, Far);
            gl.LookAt((double)Position.X, (double)Position.Y, (double)Position.Z,
                (double)target.X, (double)target.Y, (double)target.Z,
                (double)upVector.X, (double)upVector.Y, (double)upVector.Z);
        }

		/// <summary>
		/// This is the point in the scene that the camera is pointed at.
		/// </summary>
		protected Vertex target = new Vertex(0, 0, 0);
        
		/// <summary>
		/// This is a vector that describes the 'up' direction (normally 0, 0, 1).
		/// Use this to tilt the camera.
		/// </summary>
		protected Vertex upVector = new Vertex(0, 0, 1);

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
		[Description("The target of the camera (the point it's looking at"), Category("Camera")]
		public Vertex Target
		{
			get {return target;}
			set {target = value;}
		}

        /// <summary>
        /// Gets or sets up vector.
        /// </summary>
        /// <value>
        /// Up vector.
        /// </value>
		[Description("The up direction, relative to camera. (Controls tilt)."), Category("Camera")]
		public Vertex UpVector
		{
			get {return upVector;}
			set {upVector = value;}
		}
    }
}
