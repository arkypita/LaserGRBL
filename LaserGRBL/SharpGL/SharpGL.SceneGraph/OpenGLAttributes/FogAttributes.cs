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
    public class FogAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="FogAttributes"/> class.
        /// </summary>
        public FogAttributes()
        {
            AttributeFlags = SharpGL.Enumerations.AttributeMask.Fog;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (enable.HasValue) gl.EnableIf(OpenGL.GL_FOG, enable.Value);
            if (mode.HasValue) gl.Fog(OpenGL.GL_FOG_MODE, (int)mode.Value);
            if (color != null) gl.Fog(OpenGL.GL_FOG_COLOR, color);
            if (density.HasValue) gl.Fog(OpenGL.GL_FOG_DENSITY, density.Value);
            if (start.HasValue) gl.Fog(OpenGL.GL_FOG_START, start.Value);
            if (end.HasValue) gl.Fog(OpenGL.GL_FOG_END, end.Value);
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return enable.HasValue ||
                mode.HasValue ||
                color != null ||
                density.HasValue ||
                start.HasValue ||
                end.HasValue;
        }

        private bool? enable;
        private FogMode? mode;
        private GLColor color;
        private float? density;
        private float? start;
        private float? end;

		[Description("Use OpenGL Fog."), Category("Fog")]
		public bool? Enable
		{
			get {return enable;}
			set {enable = value;}
		}
		[Description("Fog mode (how the fog density is calculated)."), Category("Fog")]
		public FogMode? Mode
		{
			get {return mode;}
			set {mode = value;}
		}
		[Description("Fog color."), Category("Fog")]
		public GLColor Color
		{
			get {return color;}
			set {color = value;}
		}
		[Description("Fog density (how thick the fog is)."), Category("Fog")]
		public float? Density
		{
			get {return density;}
			set {density = value;}
		}
		[Description("How close to the camera the fog starts."), Category("Fog")]
		public float? Start
		{
			get {return start;}
			set {start = value;}
		}
		[Description("How far away from the camera you get complete fog."), Category("Fog")]
		public float? End
		{
			get {return end;}
			set {end = value;}
		}
	}
}
