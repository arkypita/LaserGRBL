using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;
using SharpGL.Enumerations;

namespace SharpGL.OpenGLAttributes
{
	/// <summary>
	/// This class has the light settings.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
	public class LightingAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="LightingAttributes"/> class.
        /// </summary>
        public LightingAttributes()
        {
            AttributeFlags = SharpGL.Enumerations.AttributeMask.Lighting;        
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (enable.HasValue) gl.EnableIf(OpenGL.GL_LIGHTING, enable.Value);
            if (ambientLight != null) gl.LightModel(LightModelParameter.Ambient, ambientLight);
            if (localViewer.HasValue) gl.LightModel(LightModelParameter.LocalViewer, localViewer.Value == true ? 1 : 0);
            if (twoSided.HasValue) gl.LightModel(LightModelParameter.TwoSide, twoSided.Value == true ? 1 : 0);
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
                ambientLight != null ||
                localViewer.HasValue ||
                twoSided.HasValue;
        }

        private GLColor ambientLight;
        private bool? localViewer;
        private bool? twoSided;
        private bool? enable;
		
		[Description("The ambient light for the entire scene."), Category("Lighting")]
        public GLColor AmbientLight
		{
			get {return ambientLight;}
			set {ambientLight = value;}
		}
		[Description("Does the scene get light depending on camera position?"), Category("Lighting")]
		public bool? LocalViewer
		{
			get {return localViewer;}
			set {localViewer = value;}
		}
		[Description("Are both sides of a polygon lit?"), Category("Lighting")]
		public bool? TwoSided
		{
			get {return twoSided;}
			set {twoSided = value;}
		}
		[Description("Is lighting enabled in the scene?"), Category("Lighting")]
		public bool? Enable
		{
			get {return enable;}
			set {enable = value;}
		}
	}
}
