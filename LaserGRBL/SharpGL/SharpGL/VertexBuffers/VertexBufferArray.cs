using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.VertexBuffers
{
    /// <summary>
    /// A VertexBufferArray is a logical grouping of VertexBuffers. Vertex Buffer Arrays
    /// allow us to use a set of vertex buffers for vertices, indicies, normals and so on,
    /// without having to use more complicated interleaved arrays.
    /// </summary>
    public class VertexBufferArray
    {
        public void Create(OpenGL gl)
        {
            //  Generate the vertex array.
            uint[] ids = new uint[1];
            gl.GenVertexArrays(1, ids);
            vertexArrayObject = ids[0];
        }

        public void Delete(OpenGL gl)
        {
            gl.DeleteVertexArrays(1, new uint[] { vertexArrayObject });
        }

        public void Bind(OpenGL gl)
        {
            gl.BindVertexArray(vertexArrayObject);
        }

        public void Unbind(OpenGL gl)
        {
            gl.BindVertexArray(0);
        }

        /// <summary>
        /// Gets the vertex buffer array object.
        /// </summary>
        public uint VertexBufferArrayObject
        {
            get { return vertexArrayObject; }
        }

        private uint vertexArrayObject;
    }
}
