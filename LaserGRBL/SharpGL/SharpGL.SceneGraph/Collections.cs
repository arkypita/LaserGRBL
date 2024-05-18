using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Collections
{
    /// <summary>
    /// Utility class to perform a search on vertices.
    /// </summary>
    public static class VertexSearch
    {
        /// <summary>
        /// Search a set of vertices.
        /// </summary>
        /// <param name="vertices">The set of vertices to search through.</param>
        /// <param name="start">The starting vertex.</param>
        /// <param name="vertex">The target of the search.</param>
        /// <param name="accuracy">The threshhold of the distance from each dimension of the vertex for the search.</param>
        /// <returns></returns>
        public static int Search(List<Vertex> vertices, int start, Vertex vertex, float accuracy)
        {
            //  Go through the verticies.
            for (int i = start; i < vertices.Count; i++)
            {
                if ((vertices[i].X > (vertex.X - accuracy) && vertices[i].X < (vertex.X + accuracy))
                    && (vertices[i].Y > (vertex.Y - accuracy) && vertices[i].Y < (vertex.Y + accuracy))
                    && (vertices[i].Z > (vertex.Z - accuracy) && vertices[i].Z < (vertex.Z + accuracy)))
                    return i;
            }
            return -1;
        }
    }
}
