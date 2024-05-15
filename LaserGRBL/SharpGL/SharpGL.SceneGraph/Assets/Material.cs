using System;
using System.ComponentModel;
using System.Drawing;
using SharpGL.SceneGraph.Lighting;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Assets
{
		/// <summary>
		/// A material object is defined in mathematical terms, i.e it's not exclusivly 
		/// for OpenGL. This means later on, DirectX or custom library functions could
		/// be added.
		/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
	public class Material : Asset, IBindable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Material"/> class.
        /// </summary>
		public Material() 
        {
            Name = "Material";
        }
        
        /// <summary>
        /// Calculates the lighting.
        /// </summary>
        /// <param name="light">The light.</param>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
		public GLColor CalculateLighting(Light light, float angle)
		{
			double angleRadians = angle * 3.14159 / 360.0;
			GLColor reflected = ambient * light.Ambient;
			reflected += diffuse * light.Diffuse * (float)Math.Cos(angleRadians);

			return reflected;
		}

        /// <summary>
        /// Pushes this object into the provided OpenGL instance.
        /// This will generally push elements of the attribute stack
        /// and then set current values.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public virtual void Push(OpenGL gl)
        {
            //  Push lighting and texture attributes.
            gl.PushAttrib(Enumerations.AttributeMask.Lighting | Enumerations.AttributeMask.Texture);

            //  Bind the material.
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
        /// Bind to the specified OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void Bind(OpenGL gl)
        {
            //	Set the material properties.
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_AMBIENT, ambient);
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_DIFFUSE, diffuse);
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_SPECULAR, specular);
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_EMISSION, emission);
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_SHININESS, shininess);

            //  If we have a texture, bind it.
            //  No need to push or pop it as we do that earlier.
            if (texture != null)
                texture.Bind(gl);
        }

        /// <summary>
        /// Ambient color.
        /// </summary>
		private GLColor ambient = new GLColor(0.2f, 0.2f, 0.2f, 1);

        /// <summary>
        /// Diffuse color.
        /// </summary>
        private GLColor diffuse = new GLColor(0.8f, 0.8f, 0.8f, 1);

        /// <summary>
        /// Specular color.
        /// </summary>
        private GLColor specular = new GLColor(0, 0, 0, 1);
       
        /// <summary>
        /// Emission.
        /// </summary>
		private GLColor emission = new GLColor(0.1f, 0.1f, 0.1f, 1);

        /// <summary>
        /// Shininess.
        /// </summary>
        private float shininess = 0;

        /// <summary>
        /// The texture.
        /// </summary>
        private Texture texture = new Texture();
		
        /// <summary>
        /// Gets or sets the ambient.
        /// </summary>
        /// <value>
        /// The ambient.
        /// </value>
        [Description("Ambient color."), Category("Material")]
		public System.Drawing.Color Ambient
		{
			get {return ambient.ColorNET;}
			set {ambient.ColorNET = value;}
		}

        /// <summary>
        /// Gets or sets the diffuse.
        /// </summary>
        /// <value>
        /// The diffuse.
        /// </value>
        [Description("Diffuse color."), Category("Material")]
		public System.Drawing.Color Diffuse
		{
			get {return diffuse.ColorNET;}
			set {diffuse.ColorNET = value;}
		}

        /// <summary>
        /// Gets or sets the specular.
        /// </summary>
        /// <value>
        /// The specular.
        /// </value>
        [Description("Specular color."), Category("Material")]
		public System.Drawing.Color Specular
		{
			get {return specular.ColorNET;}
			set {specular.ColorNET = value;}
		}

        /// <summary>
        /// Gets or sets the emission.
        /// </summary>
        /// <value>
        /// The emission.
        /// </value>
        [Description("Emission color."), Category("Material")]
		public System.Drawing.Color Emission
		{
			get {return emission.ColorNET;}
			set {emission.ColorNET = value;}
		}

        /// <summary>
        /// Gets or sets the shininess.
        /// </summary>
        /// <value>
        /// The shininess.
        /// </value>
        [Description("Shininess."), Category("Material")]
		public float Shininess
		{
			get {return shininess;}
			set {shininess = value;}
		}

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        [Description("The Material's Texture."), Category("Material")]
		public Texture Texture
		{
			get {return texture;}
			set {texture = value;}
		}
    }
	
}

