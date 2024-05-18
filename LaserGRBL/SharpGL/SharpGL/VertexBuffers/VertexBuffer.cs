using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.VertexBuffers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Very useful reference for management of VBOs and VBAs:
    /// http://stackoverflow.com/questions/8704801/glvertexattribpointer-clarification
    /// </remarks>
    public class VertexBuffer
    {
        public void Create(OpenGL gl)
        {
            //  Generate the vertex array.
            uint[] ids = new uint[1];
            gl.GenBuffers(1, ids);
            vertexBufferObject = ids[0];
        }

        public void Delete(OpenGL gl)
        {
            gl.DeleteBuffers(1, new uint[] { vertexBufferObject });
        }

        public void SetData(OpenGL gl, uint attributeIndex, float[] rawData, bool isNormalised, int stride)
        {
            //  Set the data, specify its shape and assign it to a vertex attribute (so shaders can bind to it).
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, rawData, OpenGL.GL_STATIC_DRAW);
            gl.VertexAttribPointer(attributeIndex, stride, OpenGL.GL_FLOAT, isNormalised, 0, IntPtr.Zero);
            gl.EnableVertexAttribArray(attributeIndex);
        }

        public void Bind(OpenGL gl)
        {
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vertexBufferObject);
        }

        public void Unbind(OpenGL gl)
        {
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, 0);
        }

        public bool IsCreated() { return vertexBufferObject != 0; }

        /// <summary>
        /// Gets the vertex buffer object.
        /// </summary>
        public uint VertexBufferObject
        {
            get { return vertexBufferObject; }
        }

        private uint vertexBufferObject;
    }
}
