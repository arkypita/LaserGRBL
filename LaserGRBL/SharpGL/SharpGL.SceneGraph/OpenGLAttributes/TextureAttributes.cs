using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;
using SharpGL.Enumerations;

namespace SharpGL.OpenGLAttributes
{
	/// <summary>
	/// This class has all the settings you can edit for fog.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
    public class TextureAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureAttributes"/> class.
        /// </summary>
        public TextureAttributes()
        {
            AttributeFlags = AttributeMask.Texture;

            /*TODO
             * Enable bits for the four texture coordinates

Border color for each texture image

Minification function for each texture image

Magnification function for each texture image

Texture coordinates and wrap mode for each texture image

Color and mode for each texture environment

Enable bits GL_TEXTURE_GEN_x; x is S, T, R, and Q

GL_TEXTURE_GEN_MODE setting for S, T, R, and Q

glTexGen plane equations for S, T, R, and Q*/
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (enableAutoNormal.HasValue) gl.EnableIf(OpenGL.GL_AUTO_NORMAL, enableAutoNormal.Value);
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return enableAutoNormal.HasValue;
        }

        private bool? enableAutoNormal;

		[Description("."), Category("Eval")]
        public bool? EnableAutoNormal
		{
            get { return enableAutoNormal; }
            set { enableAutoNormal = value; }
		}
	}
}
