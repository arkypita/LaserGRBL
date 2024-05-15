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
    public class ScissorAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ScissorAttributes"/> class.
        /// </summary>
        public ScissorAttributes()
        {
            AttributeFlags = AttributeMask.Scissor;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (enableScissorTest.HasValue) 
                gl.EnableIf(OpenGL.GL_SCISSOR_TEST, enableScissorTest.Value);
            if (scissorX.HasValue && scissorY.HasValue && scissorWidth.HasValue && scissorHeight.HasValue) 
                gl.Scissor(scissorX.Value, scissorY.Value, scissorWidth.Value, scissorHeight.Value);

        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return
                enableScissorTest.HasValue ||
                scissorX.HasValue ||
                scissorY.HasValue ||
                scissorWidth.HasValue ||
                scissorHeight.HasValue;
        }

        private bool? enableScissorTest;
        private int? scissorX;
        private int? scissorY;
        private int? scissorWidth;
        private int? scissorHeight;

        /// <summary>
        /// Gets or sets the enable scissor test.
        /// </summary>
        /// <value>
        /// The enable scissor test.
        /// </value>
		[Description("."), Category("Scissor")]
        public bool? EnableScissorTest
		{
            get { return enableScissorTest; }
            set { enableScissorTest = value; }
        }

        /// <summary>
        /// Gets or sets the scissor X.
        /// </summary>
        /// <value>
        /// The scissor X.
        /// </value>
        [Description("."), Category("Scissor")]
        public int? ScissorX
        {
            get { return scissorX; }
            set { scissorX = value; }
        }

        /// <summary>
        /// Gets or sets the scissor Y.
        /// </summary>
        /// <value>
        /// The scissor Y.
        /// </value>
        [Description("."), Category("Scissor")]
        public int? ScissorY
        {
            get { return scissorY; }
            set { scissorY = value; }
        }

        /// <summary>
        /// Gets or sets the width of the scissor.
        /// </summary>
        /// <value>
        /// The width of the scissor.
        /// </value>
        [Description("."), Category("Scissor")]
        public int? ScissorWidth
        {
            get { return scissorWidth; }
            set { scissorWidth = value; }
        }

        /// <summary>
        /// Gets or sets the height of the scissor.
        /// </summary>
        /// <value>
        /// The height of the scissor.
        /// </value>
        [Description("."), Category("Scissor")]
        public int? ScissorHeight
        {
            get { return scissorHeight; }
            set { scissorHeight = value; }
        }
	}
}
