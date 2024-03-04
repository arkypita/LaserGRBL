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
    public class AccumBufferAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="AccumBufferAttributes"/> class.
        /// </summary>
        public AccumBufferAttributes()
        {
            AttributeFlags = AttributeMask.AccumBuffer;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (clear != null) gl.ClearAccum(clear.R, clear.G, clear.B, clear.A);
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return clear != null;
        }

        private GLColor clear;

		[Description("Clear color."), Category("Fog")]
		public GLColor Clear
		{
            get { return clear; }
            set { clear = value; }
		}
	}
}
