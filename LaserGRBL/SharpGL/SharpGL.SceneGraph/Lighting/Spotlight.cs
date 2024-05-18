using System;
using System.Collections;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Helpers;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph.Lighting
{
	/// <summary>
	/// A spotlight has a direction and a spot cutoff.
	/// </summary>
	[Serializable()]
	public class Spotlight : Light
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Spotlight"/> class.
        /// </summary>
        public Spotlight()
		{
            Name = "Spotlight";
            Position = new Vertex(0, 3, 0);
		}

        /// <summary>
        /// This function sets all of the lights parameters into OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
		public override void Bind(OpenGL gl)
		{
            //  Call the base (setting ambient etc).
            base.Bind(gl);

            //  Is the light on?
			if(On)
			{
                //  Set the spot parameters.
			    gl.Light(GLCode, OpenGL.GL_SPOT_CUTOFF, spotCutoff);
                gl.Light(GLCode, OpenGL.GL_SPOT_DIRECTION, direction);
			}
		}

		/// <summary>
		/// The spotlight cutoff value (between 0-90 for spotlights, or 180 for a 
		/// simple light).
		/// </summary>
		private float spotCutoff = 45.0f;
			
		/// <summary>
		/// A Vector describing the direction of the spotlight.
		/// </summary>
		private Vertex direction = new Vertex(0, 1, 0);

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        [Category("Light"), Description("The spotlight direction.")]
		public Vertex Direction
		{
			get {return direction;}
			set {direction = value; }
		}

        /// <summary>
        /// Gets or sets the spot cutoff.
        /// </summary>
        /// <value>
        /// The spot cutoff.
        /// </value>
        [Category("Light"), Description("The spotlight cutoff.")]
		public float SpotCutoff
		{
			get {return spotCutoff;}
			set {spotCutoff = value; }
		}
    }
	
}
