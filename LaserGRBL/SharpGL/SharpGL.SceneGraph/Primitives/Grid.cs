using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Core;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph.Primitives
{
    /// <summary>
    /// The Grid design time primitive is displays a grid in the scene.
    /// </summary>
    public class Grid : SceneElement, IRenderable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class.
        /// </summary>
        public Grid()
        {
            Name = "Design Time Grid";
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

            //  Push attributes, set the color.
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT | OpenGL.GL_ENABLE_BIT |
                OpenGL.GL_LINE_BIT);
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.LineWidth(1.0f);

            //  Draw the grid lines.
            gl.Begin(OpenGL.GL_LINES);
            for (int i = -10; i <= 10; i++)
            {
                float fcol = ((i % 10) == 0) ? 0.3f : 0.15f;
                gl.Color(fcol, fcol, fcol);
                gl.Vertex(i, -10, 0);
                gl.Vertex(i, 10, 0);
                gl.Vertex(-10, i, 0);
                gl.Vertex(10, i, 0);
            }
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
