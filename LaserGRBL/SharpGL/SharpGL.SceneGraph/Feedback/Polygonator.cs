using System;
using System.Collections;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Cameras;

namespace SharpGL.SceneGraph.Feedback
{
    /// <summary>
    /// Pass this class any SceneObject and it'll send you back a polygon.
    /// </summary>
    public class Polygonator : Triangulator
    {
        /// <summary>
        /// This is the main function of the class, it'll create a triangulated polygon
        /// from and SceneObject.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="sourceObject">The object to convert.</param>
        /// <param name="guarenteedView">A camera that can see the whole object.</param>
        /// <returns>
        /// A polygon created from 'sourceObject'.
        /// </returns>
        public Polygon CreatePolygon(OpenGL gl, IRenderable sourceObject, Camera guarenteedView)
        {
            //	Save the current camera data.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PushMatrix();

            //	Look through the camera that can see the object.
            guarenteedView.Project(gl);

            //	Start triangulation.
            Begin(gl);

            //	Draw the object.
            sourceObject.Render(gl, RenderMode.Design);

            //	End triangulation.
            End(gl);

            Polygon newPoly = Triangle;
            newPoly.Name = (sourceObject is SceneElement ? ((SceneElement)sourceObject).Name : "Object") + " (Triangulated Poly)";
            return newPoly;
        }
    }
}
