using System;
using System.Drawing;
using System.ComponentModel;
using System.Linq;

using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Core;
using SharpGL.OpenGLAttributes;
using SharpGL.Enumerations;

namespace SharpGL.SceneGraph.Effects
{
    /// <summary>
    /// The OpenGLAttributes are an effect that can set 
    /// any OpenGL attributes.
    /// </summary>
    public class OpenGLAttributesEffect : Effect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLAttributesEffect"/> class.
        /// </summary>
        public OpenGLAttributesEffect()
        {
            Name = "OpenGL Attributes";
        }

        /// <summary>
        /// Pushes the effect onto the specified parent element.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="parentElement">The parent element.</param>
        public override void Push(OpenGL gl, SceneElement parentElement)
        {
            //  Create a combined mask.
            AttributeMask attributeFlags = AttributeMask.None;
            attributeFlags |= accumBufferAttributes.AreAnyAttributesSet() ? accumBufferAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= colorBufferAttributes.AreAnyAttributesSet() ? colorBufferAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= currentAttributes.AreAnyAttributesSet() ? currentAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= depthBufferAttributes.AreAnyAttributesSet() ? depthBufferAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= enableAttributes.AreAnyAttributesSet() ? enableAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= fogAttributes.AreAnyAttributesSet() ? fogAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= lightingAttributes.AreAnyAttributesSet() ? lightingAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= lineAttributes.AreAnyAttributesSet() ? lineAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= pointAttributes.AreAnyAttributesSet() ? pointAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= polygonAttributes.AreAnyAttributesSet() ? polygonAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= evalAttributes.AreAnyAttributesSet() ? evalAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= hintAttributes.AreAnyAttributesSet() ? hintAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= listAttributes.AreAnyAttributesSet() ? listAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= pixelModeAttributes.AreAnyAttributesSet() ? pixelModeAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= polygonStippleAttributes.AreAnyAttributesSet() ? polygonStippleAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= scissorAttributes.AreAnyAttributesSet() ? scissorAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= stencilBufferAttributes.AreAnyAttributesSet() ? stencilBufferAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= textureAttributes.AreAnyAttributesSet() ? textureAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= transformAttributes.AreAnyAttributesSet() ? transformAttributes.AttributeFlags : AttributeMask.None;
            attributeFlags |= viewportAttributes.AreAnyAttributesSet() ? viewportAttributes.AttributeFlags : AttributeMask.None;

            //  Push the attribute stack.
            gl.PushAttrib((uint)attributeFlags);

            //  Set the attributes.
            accumBufferAttributes.SetAttributes(gl);
            colorBufferAttributes.SetAttributes(gl);
            currentAttributes.SetAttributes(gl);
            depthBufferAttributes.SetAttributes(gl);
            enableAttributes.SetAttributes(gl);
            fogAttributes.SetAttributes(gl);
            lightingAttributes.SetAttributes(gl);
            lineAttributes.SetAttributes(gl);
            pointAttributes.SetAttributes(gl);
            polygonAttributes.SetAttributes(gl);            
            evalAttributes.SetAttributes(gl);
            hintAttributes.SetAttributes(gl);
            listAttributes.SetAttributes(gl);
            pixelModeAttributes.SetAttributes(gl);
            polygonStippleAttributes.SetAttributes(gl);
            scissorAttributes.SetAttributes(gl);
            stencilBufferAttributes.SetAttributes(gl);
            textureAttributes.SetAttributes(gl);
            transformAttributes.SetAttributes(gl);
            viewportAttributes.SetAttributes(gl);

        }

        /// <summary>
        /// Pops the effect off the specified parent element.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="parentElement">The parent element.</param>
        public override void Pop(OpenGL gl, SceneElement parentElement)
        {
            //  Pop the attribute stack.
            gl.PopAttrib();
        }

        private AccumBufferAttributes accumBufferAttributes = new AccumBufferAttributes();
        private ColorBufferAttributes colorBufferAttributes = new ColorBufferAttributes();
        private CurrentAttributes currentAttributes = new CurrentAttributes();
        private DepthBufferAttributes depthBufferAttributes = new DepthBufferAttributes();
        private EnableAttributes enableAttributes = new EnableAttributes();
        private EvalAttributes evalAttributes = new EvalAttributes();
        private FogAttributes fogAttributes = new FogAttributes();
        private HintAttributes hintAttributes = new HintAttributes();
        private LightingAttributes lightingAttributes = new LightingAttributes();
        private LineAttributes lineAttributes = new LineAttributes();
        private ListAttributes listAttributes = new ListAttributes();
        private PixelModeAttributes pixelModeAttributes = new PixelModeAttributes();
        private PointAttributes pointAttributes = new PointAttributes();
        private PolygonAttributes polygonAttributes = new PolygonAttributes();
        private PolygonStippleAttributes polygonStippleAttributes = new PolygonStippleAttributes();
        private ScissorAttributes scissorAttributes = new ScissorAttributes();
        private StencilBufferAttributes stencilBufferAttributes = new StencilBufferAttributes();
        private TextureAttributes textureAttributes = new TextureAttributes();
        private TransformAttributes transformAttributes = new TransformAttributes();
        private ViewportAttributes viewportAttributes = new ViewportAttributes();

        /// <summary>
        /// Gets or sets the hint attributes.
        /// </summary>
        /// <value>
        /// The hint attributes.
        /// </value>
        [Description(""), Category("OpenGL Attributes")]
        public HintAttributes HintAttributes
        {
            get { return hintAttributes; }
            set { hintAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the list attributes.
        /// </summary>
        /// <value>
        /// The list attributes.
        /// </value>
        [Description(""), Category("OpenGL Attributes")]
        public ListAttributes ListAttributes
        {
            get { return listAttributes; }
            set { listAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the pixel mode attributes.
        /// </summary>
        /// <value>
        /// The pixel mode attributes.
        /// </value>
        [Description(""), Category("OpenGL Attributes")]
        public PixelModeAttributes PixelModeAttributes
        {
            get { return pixelModeAttributes; }
            set { pixelModeAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the polygon stipple attributes.
        /// </summary>
        /// <value>
        /// The polygon stipple attributes.
        /// </value>
        [Description(""), Category("OpenGL Attributes")]
        public PolygonStippleAttributes PolygonStippleAttributes
        {
            get { return polygonStippleAttributes; }
            set { polygonStippleAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the scissor attributes.
        /// </summary>
        /// <value>
        /// The scissor attributes.
        /// </value>
        [Description(""), Category("OpenGL Attributes")]
        public ScissorAttributes ScissorAttributes
        {
            get { return scissorAttributes; }
            set { scissorAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the stencil buffer attributes.
        /// </summary>
        /// <value>
        /// The stencil buffer attributes.
        /// </value>
        [Description(""), Category("OpenGL Attributes")]
        public StencilBufferAttributes StencilBufferAttributes
        {
            get { return stencilBufferAttributes; }
            set { stencilBufferAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the texture attributes.
        /// </summary>
        /// <value>
        /// The texture attributes.
        /// </value>
        [Description(""), Category("OpenGL Attributes")]
        public TextureAttributes TextureAttributes
        {
            get { return textureAttributes; }
            set { textureAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the transform attributes.
        /// </summary>
        /// <value>
        /// The transform attributes.
        /// </value>
        [Description(""), Category("OpenGL Attributes")]
        public TransformAttributes TransformAttributes
        {
            get { return transformAttributes; }
            set { transformAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the viewport attributes.
        /// </summary>
        /// <value>
        /// The viewport attributes.
        /// </value>
        [Description(""), Category("OpenGL Attributes")]
        public ViewportAttributes ViewportAttributes
        {
            get { return viewportAttributes; }
            set { viewportAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the eval attributes.
        /// </summary>
        /// <value>
        /// The eval attributes.
        /// </value>
        [Description(""), Category("OpenGL Attributes")]
        public EvalAttributes EvalAttributes
        {
            get { return evalAttributes; }
            set { evalAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the accum buffer attributes.
        /// </summary>
        /// <value>
        /// The accum buffer attributes.
        /// </value>
        [Description("AccumBuffer attributes"), Category("OpenGL Attributes")]
        public AccumBufferAttributes AccumBufferAttributes
        {
            get { return accumBufferAttributes; }
            set { accumBufferAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the color buffer attributes.
        /// </summary>
        /// <value>
        /// The color buffer attributes.
        /// </value>
        [Description("ColorBuffer attributes"), Category("OpenGL Attributes")]
        public ColorBufferAttributes ColorBufferAttributes
        {
            get { return colorBufferAttributes; }
            set { colorBufferAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the current attributes.
        /// </summary>
        /// <value>
        /// The current buffer.
        /// </value>
        [Description("Current attributes"), Category("OpenGL Attributes")]
        public CurrentAttributes CurrentAttributes
        {
            get { return currentAttributes; }
            set { currentAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the depth buffer attributes.
        /// </summary>
        /// <value>
        /// The depth buffer attributes.
        /// </value>
        [Description("DepthBuffer attributes"), Category("OpenGL Attributes")]
        public DepthBufferAttributes DepthBufferAttributes
        {
            get { return depthBufferAttributes; }
            set { depthBufferAttributes = value; }
        }

        /// <summary>
        /// Gets or sets the enable attributes.
        /// </summary>
        /// <value>
        /// The enable attributes.
        /// </value>
        [Description("Enable attributes"), Category("OpenGL Attributes")]
        public EnableAttributes EnableAttributes
        {
            get { return enableAttributes; }
            set { enableAttributes = value; }
        }

        /// <summary>
        /// Gets the fog attributes.
        /// </summary>
        [Description("Fog attributes"), Category("OpenGL Attributes")]
        public FogAttributes FogAttributes
        {
            get { return fogAttributes; }
            set { fogAttributes = value; }
        }

        /// <summary>
        /// Gets the lighting attributes.
        /// </summary>
        [Description("Lighting attributes"), Category("OpenGL Attributes")]
        public LightingAttributes LightingAttributes
        {
            get { return lightingAttributes; }
            set { lightingAttributes = value; }
        }

        /// <summary>
        /// Gets the line attributes.
        /// </summary>
        [Description("Line attributes"), Category("OpenGL Attributes")]
        public LineAttributes LineAttributes
        {
            get { return lineAttributes; }
            set { lineAttributes = value; }
        }

        /// <summary>
        /// Gets the point attributes.
        /// </summary>
        [Description("Point attributes"), Category("OpenGL Attributes")]
        public PointAttributes PointAttributes
        {
            get { return pointAttributes; }
            set { pointAttributes = value; }
        }

        /// <summary>
        /// Gets the polygon attributes.
        /// </summary>
        [Description("Polygon attributes"), Category("OpenGL Attributes")]
        public PolygonAttributes PolygonAttributes
        {
            get { return polygonAttributes; }
            set { polygonAttributes = value; }
        }

        
    }
}
