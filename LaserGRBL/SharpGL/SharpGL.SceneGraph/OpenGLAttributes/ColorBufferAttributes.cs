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
    public class ColorBufferAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorBufferAttributes"/> class.
        /// </summary>
        public ColorBufferAttributes()
        {
            AttributeFlags = SharpGL.Enumerations.AttributeMask.ColorBuffer;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if(enableAlphaTest.HasValue && alphaTestFunction.HasValue && alphaTestReferenceValue.HasValue) 
            {
                gl.EnableIf(OpenGL.GL_ALPHA_TEST, enableAlphaTest.Value);
                gl.AlphaFunc(alphaTestFunction.Value, alphaTestReferenceValue.Value);
            }
            
            if(enableBlend.HasValue && blendingSourceFactor.HasValue && blendingDestinationFactor.HasValue) 
            {
                gl.EnableIf(OpenGL.GL_BLEND, enableBlend.Value);
                gl.BlendFunc(blendingSourceFactor.Value, blendingDestinationFactor.Value);
            }

            if(enableDither.HasValue) gl.EnableIf(OpenGL.GL_DITHER, enableDither.Value);
            if(drawBufferMode.HasValue) gl.DrawBuffer(drawBufferMode.Value);
            
            if(enableLogicOp.HasValue && logicOp.HasValue) 
            {
                gl.EnableIf(OpenGL.GL_COLOR_LOGIC_OP, enableLogicOp.Value);
                gl.LogicOp(logicOp.Value);
            }
            if(colorModeClearColor != null) gl.ClearColor(colorModeClearColor.R, colorModeClearColor.G, colorModeClearColor.B, colorModeClearColor.A);
            //todowritemask
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
                enableAlphaTest.HasValue ||
                alphaTestFunction.HasValue ||
                alphaTestReferenceValue.HasValue ||
                enableBlend.HasValue ||
                blendingSourceFactor.HasValue ||
                blendingDestinationFactor.HasValue ||
                enableDither.HasValue ||
                drawBufferMode.HasValue ||
                enableLogicOp.HasValue ||
                logicOp.HasValue ||
                colorModeClearColor != null ||
                indexModeClearColor != null ||
                colorModeWriteMask != null ||
                indexModeWriteMask != null;
        }

        private bool? enableAlphaTest;
        private AlphaTestFunction? alphaTestFunction;
        private float? alphaTestReferenceValue;
        private bool? enableBlend;
        private BlendingSourceFactor? blendingSourceFactor;
        private BlendingDestinationFactor? blendingDestinationFactor;
        private bool? enableDither;
        private DrawBufferMode? drawBufferMode;
        private bool? enableLogicOp;
        private LogicOp? logicOp;
        private GLColor colorModeClearColor;
        private GLColor indexModeClearColor;
        private GLColor colorModeWriteMask;
        private GLColor indexModeWriteMask;

        /// <summary>
        /// Gets or sets the enable alpha test.
        /// </summary>
        /// <value>
        /// The enable alpha test.
        /// </value>
        [Description("EnableAlphaTest"), Category("Color Buffer")]
        public bool? EnableAlphaTest
        {
            get { return enableAlphaTest; }
            set { enableAlphaTest = value; }
        }

        /// <summary>
        /// Gets or sets the alpha test function.
        /// </summary>
        /// <value>
        /// The alpha test function.
        /// </value>
        [Description("AlphaTestFunction"), Category("Color Buffer")]
        public AlphaTestFunction? AlphaTestFunction
        {
            get { return alphaTestFunction; }
            set { alphaTestFunction = value; }
        }

        /// <summary>
        /// Gets or sets the alpha test reference value.
        /// </summary>
        /// <value>
        /// The alpha test reference value.
        /// </value>
        [Description("AlphaTestReferenceValue"), Category("Color Buffer")]
        public float? AlphaTestReferenceValue
        {
            get { return alphaTestReferenceValue; }
            set { alphaTestReferenceValue = value; }
        }

        /// <summary>
        /// Gets or sets the enable blend.
        /// </summary>
        /// <value>
        /// The enable blend.
        /// </value>
        [Description("EnableBlend"), Category("Color Buffer")]
        public bool? EnableBlend
        {
            get { return enableBlend; }
            set { enableBlend = value; }
        }

        /// <summary>
        /// Gets or sets the blending source factor.
        /// </summary>
        /// <value>
        /// The blending source factor.
        /// </value>
        [Description("BlendingSourceFactor"), Category("Color Buffer")]
        public BlendingSourceFactor? BlendingSourceFactor
        {
            get { return blendingSourceFactor; }
            set { blendingSourceFactor = value; }
        }

        /// <summary>
        /// Gets or sets the blending destination factor.
        /// </summary>
        /// <value>
        /// The blending destination factor.
        /// </value>
        [Description("BlendingDestinationFactor"), Category("Color Buffer")]
        public BlendingDestinationFactor? BlendingDestinationFactor
        {
            get { return blendingDestinationFactor; }
            set { blendingDestinationFactor = value; }
        }

        /// <summary>
        /// Gets or sets the enable dither.
        /// </summary>
        /// <value>
        /// The enable dither.
        /// </value>
        [Description("EnableDither"), Category("Color Buffer")]
        public bool? EnableDither
        {
            get { return enableDither; }
            set { enableDither = value; }
        }

        /// <summary>
        /// Gets or sets the draw buffer mode.
        /// </summary>
        /// <value>
        /// The draw buffer mode.
        /// </value>
        [Description("DrawBufferMode"), Category("Color Buffer")]
        public DrawBufferMode? DrawBufferMode
        {
            get { return drawBufferMode; }
            set { drawBufferMode = value; }
        }

        /// <summary>
        /// Gets or sets the enable logic op.
        /// </summary>
        /// <value>
        /// The enable logic op.
        /// </value>
        [Description("EnableLogicOp"), Category("Color Buffer")]
        public bool? EnableLogicOp
        {
            get { return enableLogicOp; }
            set { enableLogicOp = value; }
        }

        /// <summary>
        /// Gets or sets the logic op.
        /// </summary>
        /// <value>
        /// The logic op.
        /// </value>
        [Description("LogicOp"), Category("Color Buffer")]
        public LogicOp? LogicOp
        {
            get { return logicOp; }
            set { logicOp = value; }
        }

        /// <summary>
        /// Gets or sets the color of the color mode clear.
        /// </summary>
        /// <value>
        /// The color of the color mode clear.
        /// </value>
        [Description("ColorModeClearColor"), Category("Color Buffer")]
        public GLColor ColorModeClearColor
        {
            get { return colorModeClearColor; }
            set { colorModeClearColor = value; }
        }

        /// <summary>
        /// Gets or sets the color of the index mode clear.
        /// </summary>
        /// <value>
        /// The color of the index mode clear.
        /// </value>
        [Description("IndexModeClearColor"), Category("Color Buffer")]
        public GLColor IndexModeClearColor
        {
            get { return indexModeClearColor; }
            set { indexModeClearColor = value; }
        }

        /// <summary>
        /// Gets or sets the color mode write mask.
        /// </summary>
        /// <value>
        /// The color mode write mask.
        /// </value>
        [Description("ColorModeWriteMask"), Category("Color Buffer")]
        public GLColor ColorModeWriteMask
        {
            get { return colorModeWriteMask; }
            set { colorModeWriteMask = value; }
        }

        /// <summary>
        /// Gets or sets the index mode write mask.
        /// </summary>
        /// <value>
        /// The index mode write mask.
        /// </value>
        [Description("IndexModeWriteMask"), Category("Color Buffer")]
        public GLColor IndexModeWriteMask
        {
            get { return indexModeWriteMask; }
            set { indexModeWriteMask = value; }
        }
	}
}
