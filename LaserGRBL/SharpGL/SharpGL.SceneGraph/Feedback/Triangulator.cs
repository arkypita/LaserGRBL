using System;
using System.Collections;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Feedback;

namespace SharpGL.SceneGraph.Feedback
{
    public class Triangulator : Feedback
    {
        /// <summary>
        /// This takes the feedback data and turns it into triangles.
        /// </summary>
        /// <param name="gl"></param>
        /// <param name="values">The number of triangles.</param>
        protected override void ParseData(OpenGL gl, int values)
        {
            int count = values;

            //	Create the triangle.
            triangle = new Polygon();
            triangle.Name = "Triangulated Polygon";

            //	For every value in the buffer...
            while (count != 0)
            {
                //	Get the token.
                float token = feedbackBuffer[values - count];

                //	Decrement count (move to the next token).
                count--;

                //	Check the type of the token.
                switch ((int)token)
                {
                    case (int)OpenGL.GL_PASS_THROUGH_TOKEN:
                        count--;
                        break;
                    case (int)OpenGL.GL_POINT_TOKEN:
                        //	We use only polygons, skip this single vertex (11 floats).
                        count -= 11;
                        break;
                    case (int)OpenGL.GL_LINE_TOKEN:
                        //	We use only polygons, skip this vertex pair (22 floats).
                        count -= 22;
                        break;
                    case (int)OpenGL.GL_LINE_RESET_TOKEN:
                        //	We use only polygons, skip this vertex pair (22 floats).
                        count -= 22;
                        break;
                    case (int)OpenGL.GL_POLYGON_TOKEN:

                        //	Get the number of vertices.
                        int vertexCount = (int)feedbackBuffer[values - count--];

                        //	Create an array of vertices.
                        Vertex[] vertices = new Vertex[vertexCount];

                        //	Parse them.
                        for (int i = 0; i < vertexCount; i++)
                        {
                            vertices[i] = new Vertex();
                            double x = (double)feedbackBuffer[values - count--];
                            double y = (double)feedbackBuffer[values - count--];
                            double z = (double)feedbackBuffer[values - count--];
                            double[] coords = gl.UnProject(x, y, z);
                            vertices[i].X = (float)coords[0];
                            vertices[i].Y = (float)coords[1];
                            vertices[i].Z = (float)coords[2];

                            //	Ignore the four r,g,b,a values and four material coords.
                            count -= 8;
                        }

                        //	Add a new face to the current polygon.
                        triangle.AddFaceFromVertexData(vertices);

                        break;
                }
            }

            //	Triangulate it.
            triangle.Triangulate();
        }

        protected Polygon triangle;

        #region Properties
        public Polygon Triangle
        {
            get { return triangle; }
            set { triangle = value; }
        }
        #endregion
    }
}