using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;

namespace SharpGL.OpenGLAttributes
{
	/// <summary>
	/// This class has all the settings you can edit for fog.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
    public class EnableAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="EnableAttributes"/> class.
        /// </summary>
        public EnableAttributes()
        {
            AttributeFlags = SharpGL.Enumerations.AttributeMask.AccumBuffer;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if(enableAlphaTest.HasValue) gl.EnableIf(OpenGL.GL_ALPHA_TEST,enableAlphaTest.Value);
            if(enableAutoNormal.HasValue) gl.EnableIf(OpenGL.GL_AUTO_NORMAL,enableAutoNormal.Value);
            if(enableBlend.HasValue) gl.EnableIf(OpenGL.GL_BLEND,enableBlend.Value);
            if(enableCullFace.HasValue) gl.EnableIf(OpenGL.GL_CULL_FACE,enableCullFace.Value);
            if(enableDepthTest.HasValue) gl.EnableIf(OpenGL.GL_DEPTH_TEST,enableDepthTest.Value);
            if(enableDither.HasValue) gl.EnableIf(OpenGL.GL_DITHER,enableDither.Value);
            if(enableFog.HasValue) gl.EnableIf(OpenGL.GL_FOG,enableFog.Value);
            if(enableLighting.HasValue) gl.EnableIf(OpenGL.GL_LIGHTING,enableLighting.Value);
            if(enableLineSmooth.HasValue) gl.EnableIf(OpenGL.GL_LINE_SMOOTH,enableLineSmooth.Value);
            if(enableLineStipple.HasValue) gl.EnableIf(OpenGL.GL_LINE_STIPPLE,enableLineStipple.Value);
            if(enableColorLogicOp.HasValue) gl.EnableIf(OpenGL.GL_COLOR_LOGIC_OP,enableColorLogicOp.Value);
            if(enableIndexLogicOp.HasValue) gl.EnableIf(OpenGL.GL_INDEX_LOGIC_OP,enableIndexLogicOp.Value);
            if(enableNormalize.HasValue) gl.EnableIf(OpenGL.GL_NORMALIZE,enableNormalize.Value);
            if(enablePointSmooth.HasValue) gl.EnableIf(OpenGL.GL_POINT_SMOOTH,enablePointSmooth.Value);
            if(enablePolygonOffsetLine.HasValue) gl.EnableIf(OpenGL.GL_POLYGON_OFFSET_LINE,enablePolygonOffsetLine.Value);
            if(enablePolygonOffsetFill.HasValue) gl.EnableIf(OpenGL.GL_POLYGON_OFFSET_FILL,enablePolygonOffsetFill.Value);
            if(enablePolygonOffsetPoint.HasValue) gl.EnableIf(OpenGL.GL_POLYGON_OFFSET_POINT,enablePolygonOffsetPoint.Value);
            if(enablePolygonSmooth.HasValue) gl.EnableIf(OpenGL.GL_POLYGON_SMOOTH,enablePolygonSmooth.Value);
            if(enablePolygonStipple.HasValue) gl.EnableIf(OpenGL.GL_POLYGON_STIPPLE,enablePolygonStipple.Value);
            if(enableScissorTest.HasValue) gl.EnableIf(OpenGL.GL_SCISSOR_TEST,enableScissorTest.Value);
            if(enableStencilTest.HasValue) gl.EnableIf(OpenGL.GL_STENCIL,enableStencilTest.Value);
            if(enableTexture1D.HasValue) gl.EnableIf(OpenGL.GL_TEXTURE_1D,enableTexture1D.Value);
            if (enableTexture2D.HasValue) gl.EnableIf(OpenGL.GL_TEXTURE_2D, enableTexture2D.Value);
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
                enableAutoNormal.HasValue ||
                enableBlend.HasValue ||
                enableCullFace.HasValue ||
                enableDepthTest.HasValue ||
                enableDither.HasValue ||
                enableFog.HasValue ||
                enableLighting.HasValue ||
                enableLineSmooth.HasValue ||
                enableLineStipple.HasValue ||
                enableColorLogicOp.HasValue ||
                enableIndexLogicOp.HasValue ||
                enableNormalize.HasValue ||
                enablePointSmooth.HasValue ||
                enablePolygonOffsetLine.HasValue ||
                enablePolygonOffsetFill.HasValue ||
                enablePolygonOffsetPoint.HasValue ||
                enablePolygonSmooth.HasValue ||
                enablePolygonStipple.HasValue ||
                enableScissorTest.HasValue ||
                enableStencilTest.HasValue ||
                enableTexture1D.HasValue ||
                enableTexture2D.HasValue;

        }

        private bool? enableAlphaTest;
        private bool? enableAutoNormal;
        private bool? enableBlend;
        private bool? enableCullFace;
        private bool? enableDepthTest;
        private bool? enableDither;
        private bool? enableFog;
        private bool? enableLighting;
        private bool? enableLineSmooth;
        private bool? enableLineStipple;
        private bool? enableColorLogicOp;
        private bool? enableIndexLogicOp;
        private bool? enableNormalize;
        private bool? enablePointSmooth;
        private bool? enablePolygonOffsetLine;
        private bool? enablePolygonOffsetFill;
        private bool? enablePolygonOffsetPoint;
        private bool? enablePolygonSmooth;
        private bool? enablePolygonStipple;
        private bool? enableScissorTest;
        private bool? enableStencilTest;
        private bool? enableTexture1D;
        private bool? enableTexture2D;

        /// <summary>
        /// Gets or sets the enable alpha test.
        /// </summary>
        /// <value>
        /// The enable alpha test.
        /// </value>
        public bool? EnableAlphaTest
        {
            get { return enableAlphaTest; }
            set { enableAlphaTest = value; }
        }

        /// <summary>
        /// Gets or sets the enable auto normal.
        /// </summary>
        /// <value>
        /// The enable auto normal.
        /// </value>
        public bool? EnableAutoNormal
        {
            get { return enableAutoNormal; }
            set { enableAutoNormal = value; }
        }

        /// <summary>
        /// Gets or sets the enable blend.
        /// </summary>
        /// <value>
        /// The enable blend.
        /// </value>
        public bool? EnableBlend
        {
            get { return enableBlend; }
            set { enableBlend = value; }
        }

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
        /// Gets or sets the enable depth test.
        /// </summary>
        /// <value>
        /// The enable depth test.
        /// </value>
        public bool? EnableDepthTest
        {
            get { return enableDepthTest; }
            set { enableDepthTest = value; }
        }

        /// <summary>
        /// Gets or sets the enable dither.
        /// </summary>
        /// <value>
        /// The enable dither.
        /// </value>
        public bool? EnableDither
        {
            get { return enableDither; }
            set { enableDither = value; }
        }

        /// <summary>
        /// Gets or sets the enable fog.
        /// </summary>
        /// <value>
        /// The enable fog.
        /// </value>
        public bool? EnableFog
        {
            get { return enableFog; }
            set { enableFog = value; }
        }

        /// <summary>
        /// Gets or sets the enable lighting.
        /// </summary>
        /// <value>
        /// The enable lighting.
        /// </value>
        public bool? EnableLighting
        {
            get { return enableLighting; }
            set { enableLighting = value; }
        }

        /// <summary>
        /// Gets or sets the enable line smooth.
        /// </summary>
        /// <value>
        /// The enable line smooth.
        /// </value>
        public bool? EnableLineSmooth
        {
            get { return enableLineSmooth; }
            set { enableLineSmooth = value; }
        }

        /// <summary>
        /// Gets or sets the enable line stipple.
        /// </summary>
        /// <value>
        /// The enable line stipple.
        /// </value>
        public bool? EnableLineStipple
        {
            get { return enableLineStipple; }
            set { enableLineStipple = value; }
        }

        /// <summary>
        /// Gets or sets the enable color logic op.
        /// </summary>
        /// <value>
        /// The enable color logic op.
        /// </value>
        public bool? EnableColorLogicOp
        {
            get { return enableColorLogicOp; }
            set { enableColorLogicOp = value; }
        }

        /// <summary>
        /// Gets or sets the enable index logic op.
        /// </summary>
        /// <value>
        /// The enable index logic op.
        /// </value>
        public bool? EnableIndexLogicOp
        {
            get { return enableIndexLogicOp; }
            set { enableIndexLogicOp = value; }
        }

        /// <summary>
        /// Gets or sets the enable normalize.
        /// </summary>
        /// <value>
        /// The enable normalize.
        /// </value>
        public bool? EnableNormalize
        {
            get { return enableNormalize; }
            set { enableNormalize = value; }
        }

        /// <summary>
        /// Gets or sets the enable point smooth.
        /// </summary>
        /// <value>
        /// The enable point smooth.
        /// </value>
        public bool? EnablePointSmooth
        {
            get { return enablePointSmooth; }
            set { enablePointSmooth = value; }
        }

        /// <summary>
        /// Gets or sets the enable polygon offset line.
        /// </summary>
        /// <value>
        /// The enable polygon offset line.
        /// </value>
        public bool? EnablePolygonOffsetLine
        {
            get { return enablePolygonOffsetLine; }
            set { enablePolygonOffsetLine = value; }
        }

        /// <summary>
        /// Gets or sets the enable polygon offset fill.
        /// </summary>
        /// <value>
        /// The enable polygon offset fill.
        /// </value>
        public bool? EnablePolygonOffsetFill
        {
            get { return enablePolygonOffsetFill; }
            set { enablePolygonOffsetFill = value; }
        }

        /// <summary>
        /// Gets or sets the enable polygon offset point.
        /// </summary>
        /// <value>
        /// The enable polygon offset point.
        /// </value>
        public bool? EnablePolygonOffsetPoint
        {
            get { return enablePolygonOffsetPoint; }
            set { enablePolygonOffsetPoint = value; }
        }

        /// <summary>
        /// Gets or sets the enable polygon smooth.
        /// </summary>
        /// <value>
        /// The enable polygon smooth.
        /// </value>
        public bool? EnablePolygonSmooth
        {
            get { return enablePolygonSmooth; }
            set { enablePolygonSmooth = value; }
        }

        /// <summary>
        /// Gets or sets the enable polygon stipple.
        /// </summary>
        /// <value>
        /// The enable polygon stipple.
        /// </value>
        public bool? EnablePolygonStipple
        {
            get { return enablePolygonStipple; }
            set { enablePolygonStipple = value; }
        }

        /// <summary>
        /// Gets or sets the enable scissor test.
        /// </summary>
        /// <value>
        /// The enable scissor test.
        /// </value>
        public bool? EnableScissorTest
        {
            get { return enableScissorTest; }
            set { enableScissorTest = value; }
        }

        /// <summary>
        /// Gets or sets the enable stencil test.
        /// </summary>
        /// <value>
        /// The enable stencil test.
        /// </value>
        public bool? EnableStencilTest
        {
            get { return enableStencilTest; }
            set { enableStencilTest = value; }
        }

        /// <summary>
        /// Gets or sets the enable texture1 D.
        /// </summary>
        /// <value>
        /// The enable texture1 D.
        /// </value>
        public bool? EnableTexture1D
        {
            get { return enableTexture1D; }
            set { enableTexture1D = value; }
        }

        /// <summary>
        /// Gets or sets the enable texture2 D.
        /// </summary>
        /// <value>
        /// The enable texture2 D.
        /// </value>
        public bool? EnableTexture2D
        {
            get { return enableTexture2D; }
            set { enableTexture2D = value; }
        }
	}
}
