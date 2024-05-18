using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;

namespace SharpGL.OpenGLAttributes
{
	/// <summary>
	/// This class has all the settings you can edit for fog.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
    public class PolygonStippleAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonStippleAttributes"/> class.
        /// </summary>
        public PolygonStippleAttributes()
        {
            AttributeFlags = SharpGL.Enumerations.AttributeMask.PolygonStipple;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (polygonStipple != null) gl.PolygonStipple(polygonStipple);
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return polygonStipple != null;
        }

        private byte[] polygonStipple;

        /// <summary>
        /// Gets or sets the polygon stipple.
        /// </summary>
        /// <value>
        /// The polygon stipple.
        /// </value>
		[Description("."), Category("Polygon Stipple")]
        public byte[] PolygonStipple
		{
            get { return polygonStipple; }
            set { polygonStipple = value; }
		}
	}
}
