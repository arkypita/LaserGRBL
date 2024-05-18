using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;
using SharpGL.Enumerations;

namespace SharpGL.OpenGLAttributes
{
	/// <summary>
	/// The line attributes.
	/// </summary>
    [TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    [Serializable()]
    public class LineAttributes : OpenGLAttributeGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineAttributes"/> class.
        /// </summary>
        public LineAttributes()
        {
            AttributeFlags = AttributeMask.Line;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if(width.HasValue) gl.LineWidth(width.Value);
            if(smooth.HasValue) gl.EnableIf(OpenGL.GL_LINE_SMOOTH, smooth.Value);
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return width.HasValue || smooth.HasValue;
        }

        private float? width;
        private bool? smooth;

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public float? Width
        {
            get { return width; }
            set { width = value; }
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
