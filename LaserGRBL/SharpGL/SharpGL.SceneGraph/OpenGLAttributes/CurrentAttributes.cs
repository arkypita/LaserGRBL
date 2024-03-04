using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Core;
using SharpGL.Enumerations;

namespace SharpGL.OpenGLAttributes
{
	/// <summary>
	/// This class has all the settings you can edit for current.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
    public class CurrentAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentAttributes"/> class.
        /// </summary>
        public CurrentAttributes()
        {
            AttributeFlags = AttributeMask.Current;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (currentColor != null) gl.Color(currentColor.R, currentColor.G, currentColor.B, currentColor.A);
            if(currentColorIndex.HasValue) gl.Index(currentColorIndex.Value);
            if (currentNormalVector != null) gl.Normal(currentNormalVector.Value.X, currentNormalVector.Value.Y, currentNormalVector.Value.Z);
            if (currentTextureCoordiate != null) gl.TexCoord(currentTextureCoordiate.Value.U, currentTextureCoordiate.Value.V);
            if (currentRasterPosition != null) gl.RasterPos(currentRasterPosition.Value.X, currentRasterPosition.Value.X);
            //todoif (currentRasterColor != null) gl.(currentColor.R, currentColor.G, currentColor.B, currentColor.A);
            //todoif (currentRasterColorIndex != null) gl.(currentColor.R, currentColor.G, currentColor.B, currentColor.A);
            //todoif (currentRasterTextureCoordiate != null) gl.(currentColor.R, currentColor.G, currentColor.B, currentColor.A);
            if (currentEdgeFlag.HasValue) gl.EdgeFlag(currentEdgeFlag.Value ? (byte)OpenGL.GL_TRUE : (byte)OpenGL.GL_FALSE);
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return (
                    currentColor != null ||
                    currentColorIndex.HasValue ||
                    currentNormalVector != null ||
                    currentTextureCoordiate != null ||
                    currentRasterPosition != null ||
                    currentRasterColor != null ||
                    currentRasterColorIndex.HasValue ||
                    currentRasterTextureCoordiate != null ||
                    currentEdgeFlag.HasValue);

        }

        private GLColor currentColor;
        private int? currentColorIndex;
        private Vertex? currentNormalVector;
        private UV? currentTextureCoordiate;
        private Vertex? currentRasterPosition;
        private GLColor currentRasterColor;
        private int? currentRasterColorIndex;
        private UV? currentRasterTextureCoordiate;
        private bool? currentEdgeFlag;

        /// <summary>
        /// Gets or sets the color of the current.
        /// </summary>
        /// <value>
        /// The color of the current.
        /// </value>
        [Description(""), Category("Current")]
        public GLColor CurrentColor
        {
            get { return currentColor; }
            set { currentColor = value; }
        }

        /// <summary>
        /// Gets or sets the index of the current color.
        /// </summary>
        /// <value>
        /// The index of the current color.
        /// </value>
        [Description(""), Category("Current")]
        public int? CurrentColorIndex
        {
            get { return currentColorIndex; }
            set { currentColorIndex = value; }
        }

        /// <summary>
        /// Gets or sets the current normal vector.
        /// </summary>
        /// <value>
        /// The current normal vector.
        /// </value>
        [Description(""), Category("Current")]
        public Vertex? CurrentNormalVector
        {
            get { return currentNormalVector; }
            set { currentNormalVector = value; }
        }

        /// <summary>
        /// Gets or sets the current texture coordiate.
        /// </summary>
        /// <value>
        /// The current texture coordiate.
        /// </value>
        [Description(""), Category("Current")]
        public UV? CurrentTextureCoordiate
        {
            get { return currentTextureCoordiate; }
            set { currentTextureCoordiate = value; }
        }

        /// <summary>
        /// Gets or sets the current raster position.
        /// </summary>
        /// <value>
        /// The current raster position.
        /// </value>
        [Description(""), Category("Current")]
        public Vertex? CurrentRasterPosition
        {
            get { return currentRasterPosition; }
            set { currentRasterPosition = value; }
        }

        /// <summary>
        /// Gets or sets the color of the current raster.
        /// </summary>
        /// <value>
        /// The color of the current raster.
        /// </value>
        [Description(""), Category("Current")]
        public GLColor CurrentRasterColor
        {
            get { return currentRasterColor; }
            set { currentRasterColor = value; }
        }

        /// <summary>
        /// Gets or sets the index of the current raster color.
        /// </summary>
        /// <value>
        /// The index of the current raster color.
        /// </value>
        [Description(""), Category("Current")]
        public int? CurrentRasterColorIndex
        {
            get { return currentRasterColorIndex; }
            set { currentRasterColorIndex = value; }
        }

        /// <summary>
        /// Gets or sets the current raster texture coordiate.
        /// </summary>
        /// <value>
        /// The current raster texture coordiate.
        /// </value>
        [Description(""), Category("Current")]
        public UV? CurrentRasterTextureCoordiate
        {
            get { return currentRasterTextureCoordiate; }
            set { currentRasterTextureCoordiate = value; }
        }

        /// <summary>
        /// Gets or sets the current edge flag.
        /// </summary>
        /// <value>
        /// The current edge flag.
        /// </value>
        [Description(""), Category("Current")]
        public bool? CurrentEdgeFlag
        {
            get { return currentEdgeFlag; }
            set { currentEdgeFlag = value; }
        }
	}
}
