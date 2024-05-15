using System;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Helpers;
using SharpGL.SceneGraph.Assets;
using System.Xml.Serialization;
using SharpGL.SceneGraph.Transformations;

namespace SharpGL.SceneGraph.Quadrics
{
    /// <summary>
    /// Quadric is the base class for all SharpGL quadric objects.
    /// It can be interacted with and it can be rendered.
    /// </summary>
    [Serializable()]
    public abstract class Quadric : 
        SceneElement,
        IHasObjectSpace,
        IHasOpenGLContext,
        IRenderable,
        IHasMaterial
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Quadric"/> class.
        /// </summary>
        public Quadric()
        {
            Name = "Quadric";
        }

        /// <summary>
        /// Create in the context of the supplied OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void CreateInContext(OpenGL gl)
        {
            //  Create the quadric object.
            glQuadric = gl.NewQuadric();
        }

        /// <summary>
        /// Destroy in the context of the supplied OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void DestroyInContext(OpenGL gl)
        {
            //  Delete the quadric object.
            gl.DeleteQuadric(glQuadric);
        }

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public virtual void Render(OpenGL gl, RenderMode renderMode)
        {
            //	Set the quadric properties.
            gl.QuadricDrawStyle(glQuadric, (uint)drawStyle);
            gl.QuadricOrientation(glQuadric, (int)orientation);
            gl.QuadricNormals(glQuadric, (uint)normals);
            gl.QuadricTexture(glQuadric, textureCoords ? 1 : 0);
        }

        /// <summary>
        /// Pushes us into Object Space using the transformation into the specified OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void PushObjectSpace(OpenGL gl)
        {
            //  Use the helper to push us into object space.
            hasObjectSpaceHelper.PushObjectSpace(gl);
        }

        /// <summary>
        /// Pops us from Object Space using the transformation into the specified OpenGL instance.
        /// </summary>
        /// <param name="gl">The gl.</param>
        public void PopObjectSpace(OpenGL gl)
        {
            //  Use the helper to pop us from object space.
            hasObjectSpaceHelper.PopObjectSpace(gl);
        }

        /// <summary>
        /// This is the pointer to the opengl quadric object.
        /// </summary>
        protected IntPtr glQuadric = IntPtr.Zero;

        /// <summary>
        /// The draw style, can be filled, line, silouhette or points.
        /// </summary>
        protected DrawStyle drawStyle = DrawStyle.Fill;
        protected Orientation orientation = Orientation.Outside;
        protected Normals normals = Normals.Smooth;
        protected bool textureCoords = false;

        /// <summary>
        /// The IHasObjectSpace helper.
        /// </summary>
        private HasObjectSpaceHelper hasObjectSpaceHelper = new HasObjectSpaceHelper();

        /// <summary>
        /// Gets or sets the quadric draw style.
        /// </summary>
        /// <value>
        /// The quadric draw style.
        /// </value>
        [Description("Draw Style of the quadric."), Category("Quadric")]
        public DrawStyle QuadricDrawStyle
        {
            get { return drawStyle; }
            set { drawStyle = value; }
        }

        /// <summary>
        /// Gets or sets the normal orientation.
        /// </summary>
        /// <value>
        /// The normal orientation.
        /// </value>
        [Description("What way normals face."), Category("Quadric")]
        public Orientation NormalOrientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        /// <summary>
        /// Gets or sets the normal generation.
        /// </summary>
        /// <value>
        /// The normal generation.
        /// </value>
        [Description("How normals are generated."), Category("Quadric")]
        public Normals NormalGeneration
        {
            get { return normals; }
            set { normals = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [texture coords].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [texture coords]; otherwise, <c>false</c>.
        /// </value>
        [Description("Should OpenGL generate texture coordinates for the Quadric?"), Category("Quadric")]
        public bool TextureCoords
        {
            get { return textureCoords; }
            set { textureCoords = value; }
        }

        /// <summary>
        /// Gets the transformation that pushes us into object space.
        /// </summary>
        [Description("The Quadric Object Space Transformation"), Category("Quadric")]
        public LinearTransformation Transformation
        {
            get { return hasObjectSpaceHelper.Transformation; }
        }

        /// <summary>
        /// Gets the current OpenGL that the object exists in context.
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public OpenGL CurrentOpenGLContext
        {
            get;
            protected set;
        }

        /// <summary>
        /// Material to be used when rendering the quadric in lighted mode.
        /// </summary>
        /// <value>
        /// The material.
        /// </value>
        [XmlIgnore]
        public Material Material 
        { 
            get; 
            set; 
        }
    }
}