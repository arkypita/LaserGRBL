using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;
using SharpGL.Enumerations;

namespace SharpGL.OpenGLAttributes
{
    /// <summary>
    /// The point settings.
    /// </summary>
    [TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    [Serializable()]
    public class PointAttributes : OpenGLAttributeGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PointAttributes"/> class.
        /// </summary>
        public PointAttributes()
        {
            AttributeFlags = AttributeMask.Point;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if(size.HasValue) gl.PointSize(size.Value);
            if(smooth.HasValue) gl.EnableIf(OpenGL.GL_POINT_SMOOTH, smooth.Value);
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return size.HasValue || smooth.HasValue;
        }

        /// <summary>
        /// The size.
        /// </summary>
        private float? size;

        /// <summary>
        /// The smooth flag.
        /// </summary>
        private bool? smooth;

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public float? Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// Gets or sets the smooth.
        /// </summary>
        /// <value>
        /// The smooth.
        /// </value>
        public bool? Smooth
        {
            get { return smooth; }
            set { smooth = value; }
        }
    }
}
