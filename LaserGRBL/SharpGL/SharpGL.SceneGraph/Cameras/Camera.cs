using System;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Lighting;

namespace SharpGL.SceneGraph.Cameras
{

	/// <summary>
	/// The camera class is a base for a set of derived classes for manipulating the
	/// projection matrix.
	/// </summary>
	[Serializable()]
	public abstract class Camera : SceneElement
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
		public Camera()
		{
			Name = "Camera";
		}

		/// <summary>
		///	This function projects through the camera, to OpenGL, ie. it 
		///	creates a projection matrix.
		/// </summary>
		public virtual void Project(OpenGL gl)
		{
			//	Load the projection identity matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
			gl.LoadIdentity();

			//	Perform the projection.
		    TransformProjectionMatrix(gl);

			//	Get the matrix.
			float[] matrix = new float[16];
            gl.GetFloat(OpenGL.GL_PROJECTION_MATRIX, matrix);
			for(int i=0; i<4; i++)
				for(int j=0; j<4; j++)
					projectionMatrix[i,j] = matrix[(i*4) + j];
				
			//	Back to the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
		}


		/// <summary>
		/// This function is for when you simply want to call only the functions that
		/// would transform the projection matrix. Warning, it won't load the identity
		/// first, and it won't set the current matrix to projection, it's really for
		/// people who need to use it for their own projection functions (e.g Picking
		/// uses it to create a composite 'Pick' projection).
		/// </summary>
        public abstract void TransformProjectionMatrix(OpenGL gl);
        
        /// <summary>
        /// The camera position.
        /// </summary>
        private Vertex position = new Vertex(0, 0, 0);
        
		/// <summary>
		/// Every time a camera is used to project, the projection matrix calculated 
		/// and stored here.
		/// </summary>
		private Matrix projectionMatrix = new Matrix(4 ,4);

        /// <summary>
        /// The screen aspect ratio.
        /// </summary>
        private double aspectRatio = 1.0f;

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        [Description("The position of the camera"), Category("Camera")]
        public Vertex Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Gets or sets the aspect.
        /// </summary>
        /// <value>
        /// The aspect.
        /// </value>
        [Description("Screen Aspect Ratio"), Category("Camera")]
        public double AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value; }
        }
    }
}
