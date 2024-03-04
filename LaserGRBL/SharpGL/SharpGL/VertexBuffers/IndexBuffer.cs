namespace SharpGL.VertexBuffers
{
    public class IndexBuffer
    {
        public void Create(OpenGL gl)
        {
            //  Generate the vertex array.
            uint[] ids = new uint[1];
            gl.GenBuffers(1, ids);
            bufferObject = ids[0];
        }

        public void Delete(OpenGL gl)
        {
            gl.DeleteBuffers(1, new uint[] { bufferObject });
        }

        public void SetData(OpenGL gl, ushort[] rawData)
        {
            gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, rawData, OpenGL.GL_STATIC_DRAW);
        }

        public void Bind(OpenGL gl)
        {
            gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, bufferObject);
        }

        public void Unbind(OpenGL gl)
        {
            gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, 0);
        }

        public bool IsCreated() { return bufferObject != 0; }

        /// <summary>
        /// Gets the index buffer object.
        /// </summary>
        public uint IndexBufferObject
        {
            get { return bufferObject; }
        }

        private uint bufferObject;
    }
}