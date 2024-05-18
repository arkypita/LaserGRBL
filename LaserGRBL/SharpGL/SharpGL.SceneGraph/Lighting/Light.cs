using System;
using System.Collections;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Helpers;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph.Lighting
{
	/// <summary>
	/// A Light is defined purely mathematically, but works well with OpenGL.
    /// A light is a scene element, it can be interacted with using the mouse
    /// and it is bindable.
	/// </summary>
	[Serializable()]
	public class Light : SceneElement, IBindable, IVolumeBound
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Light"/> class.
        /// </summary>
		public Light()
		{
			Name = "Light";
            Position = new Vertex(0, 3, 0);
		}

        /// <summary>
        /// Pushes this object into the provided OpenGL instance.
        /// This will generally push elements of the attribute stack
        /// and then set current values.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public virtual void Push(OpenGL gl)
        {
            //  Push lighting attributes.
            gl.PushAttrib(Enumerations.AttributeMask.Lighting);

            //  Bind the light.
            Bind(gl);
        }

        /// <summary>
        /// Pops this object from the provided OpenGL instance.
        /// This will generally pop elements of the attribute stack,
        /// restoring previous attribute values.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public virtual void Pop(OpenGL gl)
        {
            //  Pop lighting attributes.
            gl.PopAttrib();
        }

        /// <summary>
        /// This function sets all of the lights parameters into OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
		public virtual void Bind(OpenGL gl)
		{
			if(on)
			{
				//	Enable this light.
                gl.Enable(OpenGL.GL_LIGHTING);
				gl.Enable(glCode);

				//	The light is on, so set it's properties.
                gl.Light(glCode, OpenGL.GL_AMBIENT, ambient);
                gl.Light(glCode, OpenGL.GL_DIFFUSE, diffuse);
                gl.Light(glCode, OpenGL.GL_SPECULAR, specular);
                gl.Light(glCode, OpenGL.GL_POSITION, new float[] { position.X, position.Y, position.Z, 1.0f });

                //  180 degree cutoff gives an omnidirectional light.
                gl.Light(GLCode, OpenGL.GL_SPOT_CUTOFF, 180.0f);
            }
			else
				gl.Disable(glCode);
		}

		/// <summary>
		/// This is the OpenGL code for the light.
		/// </summary>
		private uint glCode = 0;

		/// <summary>
		/// The ambient colour of the light.
		/// </summary>
		private GLColor ambient = new GLColor(0, 0, 0, 1);
			
		/// <summary>
		/// The diffuse color of the light.
		/// </summary>
		private GLColor diffuse = new GLColor(1, 1, 1, 1);
			
		/// <summary>
		/// The specular colour of the light.
		/// </summary>
		private GLColor specular = new GLColor(1, 1, 1, 1);
			
		/// <summary>
		/// The colour of the shadow created by this light.
		/// </summary>
		private GLColor shadowColor = new GLColor(0, 0, 0, 0.4f);

		/// <summary>
		/// True when the light is on.
		/// </summary>
		private bool on = false;

        /// <summary>
        /// The position of the light.
        /// </summary>
        private Vertex position = new Vertex(0, 0, 0);

		/// <summary>
		/// Should the light cast a shadow?
		/// </summary>
		private bool castShadow = true;

        /// <summary>
        /// Used to aid in the implementation of IVolumeBound.
        /// </summary>
        private BoundingVolumeHelper boundingVolumeHelper = new BoundingVolumeHelper();

        /// <summary>
        /// Gets the bounding volume.
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public BoundingVolume BoundingVolume
        {
            get { return boundingVolumeHelper.BoundingVolume; }
        }

        /// <summary>
        /// Gets or sets the GL code.
        /// </summary>
        /// <value>
        /// The GL code.
        /// </value>
		[Browsable(false)]
		public uint GLCode
		{
			get {return glCode;}
			set {glCode = value; }
		}

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        [Category("Light"), Description("The position.")]
        public Vertex Position
        {
            get { return position; }
            set 
            { 
                position = value;
                BoundingVolume.FromSphericalVolume(position, 0.3f);
            }
        }

        /// <summary>
        /// Gets or sets the ambient.
        /// </summary>
        /// <value>
        /// The ambient.
        /// </value>
        [Category("Light"), Description("The ambient value.")]
		public System.Drawing.Color Ambient
		{
			get {return ambient.ColorNET;}
			set {ambient.ColorNET = value; }
		}

        /// <summary>
        /// Gets or sets the diffuse.
        /// </summary>
        /// <value>
        /// The diffuse.
        /// </value>
        [Category("Light"), Description("The diffuse value.")]
		public System.Drawing.Color Diffuse
		{
			get {return diffuse.ColorNET;}
			set {diffuse.ColorNET = value; }
		}

        /// <summary>
        /// Gets or sets the specular.
        /// </summary>
        /// <value>
        /// The specular.
        /// </value>
        [Category("Light"), Description("The specular value.")]
		public System.Drawing.Color Specular
		{
			get {return specular.ColorNET;}
			set {specular.ColorNET = value; }
		}

        /// <summary>
        /// Gets or sets the color of the shadow.
        /// </summary>
        /// <value>
        /// The color of the shadow.
        /// </value>
        [Category("Light"), Description("The shadow color.")]
		public GLColor ShadowColor
		{
			get {return shadowColor;}
			set {shadowColor = value; }
		}

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Light"/> is on.
        /// </summary>
        /// <value>
        ///   <c>true</c> if on; otherwise, <c>false</c>.
        /// </value>
		[Description("Is the light turned on?"), Category("Light")]
		public bool On
		{
			get {return on;}
			set {on = value;}
		}

        /// <summary>
        /// Gets or sets a value indicating whether [cast shadow].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cast shadow]; otherwise, <c>false</c>.
        /// </value>
        [Category("Light"), Description("If true, a shadow is cast.")]
		public bool CastShadow 
		{
			get {return castShadow;}
			set {castShadow = value; }
		}
    }
	
}
