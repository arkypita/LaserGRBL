using System.ComponentModel;
using System.Collections.Generic;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Assets;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
    [XmlInclude(typeof(PerspectiveCamera))]
    [XmlInclude(typeof(OrthographicCamera))]
    [XmlInclude(typeof(FrustumCamera))]
    [XmlInclude(typeof(LookAtCamera))]
    public class Scene : IHasOpenGLContext
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class.
        /// </summary>
		public Scene()
        {
            RenderBoundingVolumes = true;

            //  The SceneContainer must have it's parent scene set.
            SceneContainer.ParentScene = this;
        }

        /// <summary>
        /// Performs a hit test on the scene. All elements that implement IVolumeBound will
        /// be hit tested.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The elements hit.</returns>
		public virtual IEnumerable<SceneElement> DoHitTest(int x, int y)
		{
            //  Create a result set.
            List<SceneElement> resultSet = new List<SceneElement>();

            //  Create a hitmap.
            Dictionary<uint, SceneElement> hitMap = new Dictionary<uint, SceneElement>();

			//	If we don't have a current camera, we cannot hit test.
            if (CurrentCamera == null)
                return resultSet;

			//	Create an array that will be the viewport.
			int[] viewport = new int[4];
			
			//	Get the viewport, then convert the mouse point to an opengl point.
			gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);
			y = viewport[3] - y;

			//	Create a select buffer.
			uint[] selectBuffer = new uint[512];
			gl.SelectBuffer(512, selectBuffer);
			
			//	Enter select mode.
			gl.RenderMode(OpenGL.GL_SELECT);
                        
			//	Initialise the names, and add the first name.
			gl.InitNames();
			gl.PushName(0);
		
			//	Push matrix, set up projection, then load matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
			gl.PushMatrix();
			gl.LoadIdentity();
			gl.PickMatrix(x, y, 4, 4, viewport);
			CurrentCamera.TransformProjectionMatrix(gl);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
			gl.LoadIdentity();

            //  Create the name.
            uint currentName = 1;

            //  Render the root for hit testing.
            RenderElementForHitTest(SceneContainer, hitMap, ref currentName);
													
			//	Pop matrix and flush commands.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
			gl.PopMatrix();
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
			gl.Flush();

			//	End selection.
            int hits = gl.RenderMode(OpenGL.GL_RENDER);
			uint posinarray = 0;

            //  Go through each name.
            for (int hit = 0; hit < hits; hit++)
            {
                uint nameCount = selectBuffer[posinarray++];
                uint unused = selectBuffer[posinarray++];
                uint unused1 = selectBuffer[posinarray++];

                if (nameCount == 0)
                    continue;

                //	Add each hit element to the result set to the array.
                for (int name = 0; name < nameCount; name++)
                {
                    uint hitName = selectBuffer[posinarray++];
                    resultSet.Add(hitMap[hitName]);
                }
            }

            //  Return the result set.
			return resultSet;
		}

		/// <summary>
		/// This function draws all of the objects in the scene (i.e. every quadric
		/// in the quadrics arraylist etc).
		/// </summary>
		public virtual void Draw(Camera camera = null)
		{
            //  TODO: we must decide what to do about drawing - are 
            //  cameras completely outside of the responsibility of the scene?
            //  If no camera has been provided, use the current one.
            if (camera == null)
                camera = CurrentCamera;

			//	Set the clear color.
			float[] clear = ClearColour;
			gl.ClearColor(clear[0], clear[1], clear[2], clear[3]);

            //  Reproject.
            if (camera != null)
                camera.Project(gl);

			//	Clear.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT |
                OpenGL.GL_STENCIL_BUFFER_BIT);

            //gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);

            //  Render the root element, this will then render the whole
            //  of the scene tree.
            RenderElement(SceneContainer, RenderMode.Design);

            //  TODO: Adding this code here re-enables textures- it should work without it but it
            //  doesn't, look into this.
            //gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
            //gl.Enable(OpenGL.GL_TEXTURE_2D);
                        
			gl.Flush();
		}

	    /// <summary>
	    /// Renders the element.
	    /// </summary>
	    /// <param name="sceneElement">The secene element to render.</param>
	    /// <param name="renderMode">The render mode.</param>
	    public void RenderElement(SceneElement sceneElement, RenderMode renderMode)
        {
            //  If the element is disabled, we're done.
            if (sceneElement.IsEnabled == false)
                return;

            //  Push each effect.
            foreach (var effect in sceneElement.Effects)
                if(effect.IsEnabled)
                    effect.Push(gl, sceneElement);

            //  If the element can be bound, bind it.
            if (sceneElement is IBindable)
                ((IBindable)sceneElement).Bind(gl);

            //  If the element has an object space, transform into it.
            if (sceneElement is IHasObjectSpace)
                ((IHasObjectSpace)sceneElement).PushObjectSpace(gl);

            //  If the element has a material, push it.
            if (sceneElement is IHasMaterial && ((IHasMaterial)sceneElement).Material != null)
                ((IHasMaterial)sceneElement).Material.Push(gl);

            //  If the element can be rendered, render it.
            if (sceneElement is IRenderable)
                ((IRenderable)sceneElement).Render(gl, renderMode);

            //  If the element has a material, pop it.
            if (sceneElement is IHasMaterial && ((IHasMaterial)sceneElement).Material != null)
                ((IHasMaterial)sceneElement).Material.Pop(gl);

            //  IF the element is volume bound and we are rendering volumes, render the volume.
            if (RenderBoundingVolumes && sceneElement is IVolumeBound)
                ((IVolumeBound)sceneElement).BoundingVolume.Render(gl, renderMode);

            //  Recurse through the children.
            foreach (var childElement in sceneElement.Children)
                RenderElement(childElement, renderMode);

            //  If the element has an object space, transform out of it.
            if (sceneElement is IHasObjectSpace)
                ((IHasObjectSpace)sceneElement).PopObjectSpace(gl);

            //  Pop each effect.
            for (int i = sceneElement.Effects.Count - 1; i >= 0; i--)
                if(sceneElement.Effects[i].IsEnabled)
                    sceneElement.Effects[i].Pop(gl, sceneElement);
        }

        /// <summary>
        /// Renders the element for hit test.
        /// </summary>
        /// <param name="sceneElement">The scene element.</param>
        /// <param name="hitMap">The hit map.</param>
        /// <param name="currentName">Current hit name.</param>
        private void RenderElementForHitTest(SceneElement sceneElement, 
            Dictionary<uint, SceneElement> hitMap, ref uint currentName)
        {
            //  If the element is disabled, we're done.
            //  Also, never hit test the current camera.
            if (sceneElement.IsEnabled == false || sceneElement == CurrentCamera)
                return;

            //  Push each effect.
            foreach (var effect in sceneElement.Effects)
                if (effect.IsEnabled)
                    effect.Push(gl, sceneElement);

            //  If the element has an object space, transform into it.
            if (sceneElement is IHasObjectSpace)
                ((IHasObjectSpace)sceneElement).PushObjectSpace(gl);

            //  If the element is volume bound, render the volume.
            if (sceneElement is IVolumeBound)
            {
                //  Load and map the name.
                gl.LoadName(currentName);
                hitMap[currentName] = sceneElement;

                //  Render the bounding volume.
                ((IVolumeBound)sceneElement).BoundingVolume.Render(gl, RenderMode.HitTest);

                //  Increment the name.
                currentName++;
            }

            //  Recurse through the children.
            foreach (var childElement in sceneElement.Children)
                RenderElementForHitTest(childElement, hitMap, ref currentName);

            //  If the element has an object space, transform out of it.
            if (sceneElement is IHasObjectSpace)
                ((IHasObjectSpace)sceneElement).PopObjectSpace(gl);

            //  Pop each effect.
            for (int i = sceneElement.Effects.Count - 1; i >= 0; i--)
                if (sceneElement.Effects[i].IsEnabled)
                    sceneElement.Effects[i].Pop(gl, sceneElement);
        }

		/// <summary>
		/// Use this function to resize the scene window, and also to look through
		/// the current camera.
		/// </summary>
		/// <param name="width">Width of the screen.</param>
		/// <param name="height">Height of the screen.</param>
		public virtual void Resize(int width, int height)
		{
			if(width != -1 && height != -1 && gl != null)
			{
                //	Resize.
				gl.Viewport(0, 0, width, height);
				
                if (CurrentCamera != null)
                {
                    //  Set aspect ratio.
                    CurrentCamera.AspectRatio = width / (float)height;

				    //	Then project.
                    CurrentCamera.Project(gl);
                }
			}
		}
        
       
		/// <summary>
		/// This is the OpenGL class, use it to call OpenGL functions.
		/// Only set when the context is set.
		/// </summary>
        private OpenGL gl;

	    /// <summary>
		/// This is the colour of the background of the scene. 
        /// CHANGED FOR LASERGRBL
		/// </summary>
		public GLColor ClearColour { get; set; } = new GLColor(0, 0, 0, 0);

	    /// <summary>
        /// Gets or sets the scene container.
        /// </summary>
        /// <value>
        /// The scene container.
        /// </value>
        [Description("The top-level object in the Scene Tree"), Category("Scene")]
        public SceneContainer SceneContainer { get; set; } = new SceneContainer();

	    /// <summary>
        /// Gets the assets.
        /// </summary>
        [Description("The scene assets."), Category("Scene")]
        public ObservableCollection<Asset> Assets { get; } = new ObservableCollection<Asset>();

	    /// <summary>
        /// Gets or sets the current camera.
        /// </summary>
        /// <value>
        /// The current camera.
        /// </value>
		[Description("The current camera being used to view the scene."), Category("Scene")]
		public Camera CurrentCamera { get; set; }

	    /// <summary>
        /// Gets or sets a value indicating whether [render bounding volumes].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [render bounding volumes]; otherwise, <c>false</c>.
        /// </value>
        //todo tidy up (into render options?)
        public bool RenderBoundingVolumes
        {
            get;
            set;
        }

	    public void CreateInContext(OpenGL gl)
	    {
            //  Set our current context.
	        this.gl = gl;

	        //  Create every scene element.
	        var openGLContextElements = SceneContainer.Traverse<SceneElement>(
	            se => se is IHasOpenGLContext);
	        foreach (var openGLContextElement in openGLContextElements)
	            ((IHasOpenGLContext)openGLContextElement).CreateInContext(gl);
	        
        }

	    public void DestroyInContext(OpenGL gl)
	    {
	        //  Create every scene element.
	        var openGLContextElements = SceneContainer.Traverse<SceneElement>(
	            se => se is IHasOpenGLContext);
	        foreach (var openGLContextElement in openGLContextElements)
	            ((IHasOpenGLContext)openGLContextElement).CreateInContext(gl);
        }

	    public OpenGL CurrentOpenGLContext => gl;
	}
}
