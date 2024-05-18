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
    public class PixelModeAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="PixelModeAttributes"/> class.
        /// </summary>
        public PixelModeAttributes()
        {
            AttributeFlags = SharpGL.Enumerations.AttributeMask.PixelMode;

            /*
             * GL_RED_BIAS and GL_RED_SCALE settings

GL_GREEN_BIAS and GL_GREEN_SCALE values

GL_BLUE_BIAS and GL_BLUE_SCALE

GL_ALPHA_BIAS and GL_ALPHA_SCALE

GL_DEPTH_BIAS and GL_DEPTH_SCALE

GL_INDEX_OFFSET and GL_INDEX_SHIFT values


GL_ZOOM_X and GL_ZOOM_Y factors

GL_READ_BUFFER setting*/
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (mapColor.HasValue) gl.PixelTransfer(PixelTransferParameterName.MapColor, mapColor.Value);
            if (mapStencil.HasValue) gl.PixelTransfer(PixelTransferParameterName.MapStencil, mapStencil.Value);
            if (indexShift.HasValue) gl.PixelTransfer(PixelTransferParameterName.IndexShift, indexShift.Value);
            if (indexOffset.HasValue) gl.PixelTransfer(PixelTransferParameterName.IndexOffset, indexOffset.Value);
            if (redScale.HasValue) gl.PixelTransfer(PixelTransferParameterName.RedScale, redScale.Value);
            if (greenScale.HasValue) gl.PixelTransfer(PixelTransferParameterName.GreenScale, greenScale.Value);
            if (blueScale.HasValue) gl.PixelTransfer(PixelTransferParameterName.BlueScale, blueScale.Value);
            if (alphaScale.HasValue) gl.PixelTransfer(PixelTransferParameterName.AlphaScale, alphaScale.Value);
            if (depthScale.HasValue) gl.PixelTransfer(PixelTransferParameterName.DepthScale, depthScale.Value);
            if (redBias.HasValue) gl.PixelTransfer(PixelTransferParameterName.RedBias, redBias.Value);
            if (greenBias.HasValue) gl.PixelTransfer(PixelTransferParameterName.GreenBias, greenBias.Value);
            if (blueBias.HasValue) gl.PixelTransfer(PixelTransferParameterName.BlueBias, blueBias.Value);
            if (alphaBias.HasValue) gl.PixelTransfer(PixelTransferParameterName.AlphaBias, alphaBias.Value);
            if (depthBias.HasValue) gl.PixelTransfer(PixelTransferParameterName.DepthBias, depthBias.Value);
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
                mapColor.HasValue || 
                mapStencil.HasValue;
        }

        private bool? mapColor;
        private bool? mapStencil;
        private int? indexShift;
        private int? indexOffset;
        private float? redScale;
        private float? greenScale;
        private float? blueScale;
        private float? alphaScale;
        private float? depthScale;
        private float? redBias;
        private float? greenBias;
        private float? blueBias;
        private float? alphaBias;
        private float? depthBias;


        /// <summary>
        /// Gets or sets the color of the map.
        /// </summary>
        /// <value>
        /// The color of the map.
        /// </value>
        [Description("."), Category("PixelMode")]
        public bool? MapColor
        {
            get { return mapColor; }
            set { mapColor = value; }
        }

        /// <summary>
        /// Gets or sets the map stencil.
        /// </summary>
        /// <value>
        /// The map stencil.
        /// </value>
        [Description("."), Category("PixelMode")]
        public bool? MapStencil
        {
            get { return mapStencil; }
            set { mapStencil = value; }
        }

        /// <summary>
        /// Gets or sets the index shift.
        /// </summary>
        /// <value>
        /// The index shift.
        /// </value>
        [Description("."), Category("PixelMode")]
        public int? IndexShift
        {
            get { return indexShift; }
            set { indexShift = value; }
        }

        /// <summary>
        /// Gets or sets the index offset.
        /// </summary>
        /// <value>
        /// The index offset.
        /// </value>
        [Description("."), Category("PixelMode")]
        public int? IndexOffset
        {
            get { return indexOffset; }
            set { indexOffset = value; }
        }

        /// <summary>
        /// Gets or sets the red scale.
        /// </summary>
        /// <value>
        /// The red scale.
        /// </value>
        [Description("."), Category("PixelMode")]
        public float? RedScale
        {
            get { return redScale; }
            set { redScale = value; }
        }

        /// <summary>
        /// Gets or sets the green scale.
        /// </summary>
        /// <value>
        /// The green scale.
        /// </value>
        [Description("."), Category("PixelMode")]
        public float? GreenScale
        {
            get { return greenScale; }
            set { greenScale = value; }
        }

        /// <summary>
        /// Gets or sets the blue scale.
        /// </summary>
        /// <value>
        /// The blue scale.
        /// </value>
        [Description("."), Category("PixelMode")]
        public float? BlueScale
        {
            get { return blueScale; }
            set { blueScale = value; }
        }

        /// <summary>
        /// Gets or sets the alpha scale.
        /// </summary>
        /// <value>
        /// The alpha scale.
        /// </value>
        [Description("."), Category("PixelMode")]
        public float? AlphaScale
        {
            get { return alphaScale; }
            set { alphaScale = value; }
        }

        /// <summary>
        /// Gets or sets the depth scale.
        /// </summary>
        /// <value>
        /// The depth scale.
        /// </value>
        [Description("."), Category("PixelMode")]
        public float? DepthScale
        {
            get { return depthScale; }
            set { depthScale = value; }
        }

        /// <summary>
        /// Gets or sets the red bias.
        /// </summary>
        /// <value>
        /// The red bias.
        /// </value>
        [Description("."), Category("PixelMode")]
        public float? RedBias
        {
            get { return redBias; }
            set { redBias = value; }
        }

        /// <summary>
        /// Gets or sets the green bias.
        /// </summary>
        /// <value>
        /// The green bias.
        /// </value>
        [Description("."), Category("PixelMode")]
        public float? GreenBias
        {
            get { return greenBias; }
            set { greenBias = value; }
        }

        /// <summary>
        /// Gets or sets the blue bias.
        /// </summary>
        /// <value>
        /// The blue bias.
        /// </value>
        [Description("."), Category("PixelMode")]
        public float? BlueBias
        {
            get { return blueBias; }
            set { blueBias = value; }
        }

        /// <summary>
        /// Gets or sets the alpha bias.
        /// </summary>
        /// <value>
        /// The alpha bias.
        /// </value>
        [Description("."), Category("PixelMode")]
        public float? AlphaBias
        {
            get { return alphaBias; }
            set { alphaBias = value; }
        }

        /// <summary>
        /// Gets or sets the depth bias.
        /// </summary>
        /// <value>
        /// The depth bias.
        /// </value>
        [Description("."), Category("PixelMode")]
        public float? DepthBias
        {
            get { return depthBias; }
            set { depthBias = value; }
        }
	}
}
