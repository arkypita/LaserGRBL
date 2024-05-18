using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;
using SharpGL.Enumerations;

namespace SharpGL.OpenGLAttributes
{
    /// <summary>
    /// The polygon settings.
    /// </summary>
    [TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
    [Serializable()]
    public class PolygonAttributes : OpenGLAttributeGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonAttributes"/> class.
        /// </summary>
        public PolygonAttributes()
        {
            AttributeFlags = AttributeMask.Polygon;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (enableCullFace.HasValue) gl.EnableIf(OpenGL.GL_CULL_FACE, enableCullFace.Value);
            if(cullFaceMode.HasValue) gl.CullFace((uint)cullFaceMode.Value);
            if(enableSmooth.HasValue) gl.EnableIf(OpenGL.GL_POLYGON_SMOOTH, enableSmooth.Value);
            if(frontFaceMode.HasValue) gl.FrontFace((uint)frontFaceMode.Value);
            if(polygonMode.HasValue) gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, (uint)polygonMode.Value);
            if(offsetFactor.HasValue && offsetBias.HasValue) gl.PolygonOffset(offsetFactor.Value, offsetBias.Value);
            if(enableOffsetPoint.HasValue) gl.EnableIf(OpenGL.GL_POLYGON_OFFSET_POINT, enableOffsetPoint.Value);
            if(enableOffsetLine.HasValue) gl.EnableIf(OpenGL.GL_POLYGON_OFFSET_LINE, enableOffsetLine.Value);
            if(enableOffsetFill.HasValue) gl.EnableIf(OpenGL.GL_POLYGON_OFFSET_FILL, enableOffsetFill.Value);
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return enableCullFace.HasValue ||
                cullFaceMode.HasValue ||
                enableSmooth.HasValue ||
                frontFaceMode.HasValue ||
                polygonMode.HasValue ||
                offsetFactor.HasValue ||
                offsetBias.HasValue ||
                enableOffsetPoint.HasValue ||
                enableOffsetLine.HasValue ||
                enableOffsetFill.HasValue;
        }

        private bool? enableCullFace;
        private FaceMode? cullFaceMode;
        private bool? enableSmooth;
        private FrontFaceMode? frontFaceMode;
        private PolygonMode? polygonMode;
        private float? offsetFactor;
        private float? offsetBias;
        private bool? enableOffsetPoint;
        private bool? enableOffsetLine;
        private bool? enableOffsetFill;

        /// <summary>
        /// Gets or sets the enable cull face.
        /// </summary>
        /// <value>
        /// The enable cull face.
        /// </value>
        public bool? EnableCullFace
        {
            get { return enableCullFace; }
            set { enableCullFace = value; }
        }

        /// <summary>
        /// Gets or sets the enable smooth.
        /// </summary>
        /// <value>
        /// The enable smooth.
        /// </value>
        public bool? EnableSmooth
        {
            get { return enableSmooth; }
            set { enableSmooth = value; }
        }

        /// <summary>
        /// Gets or sets the cull faces.
        /// </summary>
        /// <value>
        /// The cull faces.
        /// </value>
        public FaceMode? CullFaces
        {
            get { return cullFaceMode; }
            set { cullFaceMode = value; }
        }

        /// <summary>
        /// Gets or sets the front faces.
        /// </summary>
        /// <value>
        /// The front faces.
        /// </value>
        public FrontFaceMode? FrontFaces
        {
            get { return frontFaceMode; }
            set { frontFaceMode = value; }
        }

        /// <summary>
        /// Gets or sets the polygon draw mode.
        /// </summary>
        /// <value>
        /// The polygon draw mode.
        /// </value>
        public PolygonMode? PolygonMode
        {
            get { return polygonMode; }
            set { polygonMode = value; }
        }

        /// <summary>
        /// Gets or sets the offset factor.
        /// </summary>
        /// <value>
        /// The offset factor.
        /// </value>
        public float? OffsetFactor
        {
            get { return offsetFactor; }
            set { offsetFactor = value; }
        }

        /// <summary>
        /// Gets or sets the offset bias.
        /// </summary>
        /// <value>
        /// The offset bias.
        /// </value>
        public float? OffsetBias
        {
            get { return offsetBias; }
            set { offsetBias = value; }
        }

        /// <summary>
        /// Gets or sets the enable offset point.
        /// </summary>
        /// <value>
        /// The enable offset point.
        /// </value>
        public bool? EnableOffsetPoint
        {
            get { return enableOffsetPoint; }
            set { enableOffsetPoint = value; }
        }

        /// <summary>
        /// Gets or sets the enable offset line.
        /// </summary>
        /// <value>
        /// The enable offset line.
        /// </value>
        public bool? EnableOffsetLine
        {
            get { return enableOffsetLine; }
            set { enableOffsetLine = value; }
        }

        /// <summary>
        /// Gets or sets the enable offset fill.
        /// </summary>
        /// <value>
        /// The enable offset fill.
        /// </value>
        public bool? EnableOffsetFill
        {
            get { return enableOffsetFill; }
            set { enableOffsetFill = value; }
        }
    }
}
