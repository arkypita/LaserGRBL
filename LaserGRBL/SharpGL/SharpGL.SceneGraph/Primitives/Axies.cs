using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Core;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph.Primitives
{
    /// <summary>
    /// The axies objects are design time rendered primitives
    /// that show an RGB axies at the origin of the scene.
    /// </summary>
    public class Axies : SceneElement, IRenderable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Axies"/> class.
        /// </summary>
        public Axies()
        {
            Name = "Design Time Axies";
        }
        
        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public void Render(OpenGL gl, RenderMode renderMode)
        {
            //  Design time primitives render only in design mode.
            if (renderMode != RenderMode.Design)
                return;

            //  If we do not have the display list, we must create it.
            //  Otherwise, we can simple call the display list.
            if (displayList == null)
                CreateDisplayList(gl);
            else
                displayList.Call(gl);
        }

        /// <summary>
        /// Creates the display list. This function draws the
        /// geometry as well as compiling it.
        /// </summary>
        private void CreateDisplayList(OpenGL gl)
        {
            //  Create the display list. 
            displayList = new DisplayList();

            //  Generate the display list and 
            displayList.Generate(gl);
            displayList.New(gl, DisplayList.DisplayListMode.CompileAndExecute);

            //  Push all attributes, disable lighting and depth testing.
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT | OpenGL.GL_ENABLE_BIT |
                OpenGL.GL_LINE_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.DepthFunc(OpenGL.GL_ALWAYS);

            //  Set a nice fat line width.
            gl.LineWidth(1.50f);

            //  Draw the axies.
            gl.Begin(OpenGL.GL_LINES);
            gl.Color(1f, 0f, 0f, 1f);
            gl.Vertex(0, 0, 0);
            gl.Vertex(3, 0, 0);
            gl.Color(0f, 1f, 0f, 1f);
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 3, 0);
            gl.Color(0f, 0f, 1f, 1f);
            gl.Vertex(0, 0, 0);
            gl.Vertex(0, 0, 3);
            gl.End();

            //  Restore attributes.
            gl.PopAttrib();

            //  End the display list.
            displayList.End(gl);
        }

        /// <summary>
        /// The internal display list.
        /// </summary>
        [XmlIgnore]
        private DisplayList displayList;
    }
}
