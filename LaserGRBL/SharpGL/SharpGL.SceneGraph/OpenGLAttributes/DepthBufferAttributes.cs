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
    public class DepthBufferAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="DepthBufferAttributes"/> class.
        /// </summary>
        public DepthBufferAttributes()
        {
            AttributeFlags = SharpGL.Enumerations.AttributeMask.DepthBuffer;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (enableDepthTest.HasValue) gl.EnableIf(OpenGL.GL_DEPTH_TEST, enableDepthTest.Value);
            if (depthFunction.HasValue) gl.DepthFunc(depthFunction.Value);
            if (depthClearValue.HasValue) gl.ClearDepth(depthClearValue.Value);
            if (enableDepthWritemask.HasValue) gl.EnableIf(OpenGL.GL_DEPTH_WRITEMASK, enableDepthWritemask.Value);
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return enableDepthTest.HasValue || 
                depthFunction.HasValue || 
                depthClearValue.HasValue || enableDepthWritemask.HasValue;
        }

        private bool? enableDepthTest;
        private DepthFunction? depthFunction;
        private float? depthClearValue;
        private bool? enableDepthWritemask;
        
        /// <summary>
        /// Gets or sets the enable depth writemask.
        /// </summary>
        /// <value>
        /// The enable depth writemask.
        /// </value>
        [Description(""), Category("Depth Buffer")]
        public bool? EnableDepthWritemask
        {
            get { return enableDepthWritemask; }
            set { enableDepthWritemask = value; }
        }

        /// <summary>
        /// Gets or sets the depth clear value.
        /// </summary>
        /// <value>
        /// The depth clear value.
        /// </value>
        [Description(""), Category("Depth Buffer")]
        public float? DepthClearValue
        {
            get { return depthClearValue; }
            set { depthClearValue = value; }
        }

        /// <summary>
        /// Gets or sets the depth function.
        /// </summary>
        /// <value>
        /// The depth function.
        /// </value>
        [Description(""), Category("Depth Buffer")]
        public DepthFunction? DepthFunction
        {
            get { return depthFunction; }
            set { depthFunction = value; }
        }

        /// <summary>
        /// Gets or sets the enable depth test.
        /// </summary>
        /// <value>
        /// The enable depth test.
        /// </value>
        [Description(""), Category("Depth Buffer")]
        public bool? EnableDepthTest
        {
            get { return enableDepthTest; }
            set { enableDepthTest = value; }
        }
	}
}
