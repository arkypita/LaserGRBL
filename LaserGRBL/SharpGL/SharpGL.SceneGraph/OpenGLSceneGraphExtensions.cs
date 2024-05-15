
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SharpGL.SceneGraph
{
    /// <summary>
    /// Extensions to the OpenGL class for use with the Scene Graph types (allowing
    /// vertices, GLColors etc to be used).
    /// </summary>
    public static class OpenGLSceneGraphExtensions
    {
        /// <summary>
        /// Set the current color.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="color">The color.</param>
        public static void Color(this OpenGL gl, GLColor color)
        {
            gl.Color(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// This is a SharpGL helper version, that projects the vertex passed, using the
        /// current matrixes.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="vertex">The object coordinates.</param>
        /// <returns>
        /// The screen coords.
        /// </returns>
        public static Vertex Project(this OpenGL gl, Vertex vertex)
        {
            //	THIS CODE MUST BE TESTED
            double[] modelview = new double[16];
            double[] projection = new double[16];
            int[] viewport = new int[4];
            gl.GetDouble(OpenGL.GL_MODELVIEW_MATRIX, modelview);
            gl.GetDouble(OpenGL.GL_PROJECTION_MATRIX, projection);
            gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);
            double[] x = new double[1];	//	kludgy
            double[] y = new double[1];
            double[] z = new double[1];
            gl.Project(vertex.X, vertex.Y, vertex.Z,
                modelview, projection, viewport, x, y, z);
            
            return new Vertex((float)x[0], (float)y[0], (float)z[0]);
        }

        /// <summary>
        /// Gets the model view matrix.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <returns></returns>
        public static Matrix GetModelViewMatrix(this OpenGL gl)
        {
                //  Get the matrix.
                double[] matrix = new double[16];
                gl.GetDouble(OpenGL.GL_MODELVIEW_MATRIX, matrix);
                return Matrix.FromColumnMajorArray(matrix, 4, 4);
        }

        /// <summary>
        /// Gets the projection matrix.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <returns></returns>
        public static Matrix GetProjectionMatrix(this OpenGL gl)
        {
                //  Get the matrix.
                double[] matrix = new double[16];
                gl.GetDouble(OpenGL.GL_PROJECTION_MATRIX, matrix);
                return Matrix.FromColumnMajorArray(matrix, 4, 4);
        }

        /// <summary>
        /// Gets the texture matrix.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <returns></returns>
        public static Matrix GetTextureMatrix(this OpenGL gl)
        {
                //  Get the matrix.
                double[] matrix = new double[16];
                gl.GetDouble(OpenGL.GL_TEXTURE_MATRIX, matrix);
                return Matrix.FromColumnMajorArray(matrix, 4, 4);
        }

        /// <summary>
        /// Vertexes the pointer.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="size">The size.</param>
        /// <param name="type">The type.</param>
        /// <param name="stride">The stride.</param>
        /// <param name="pointer">The pointer.</param>
        public static void VertexPointer(this OpenGL gl, int size, uint type, int stride, Vertex[] pointer)
        {
            //  TODO debug this.
            var handle = GCHandle.Alloc(pointer, GCHandleType.Pinned);
            gl.VertexPointer(size, type, stride, handle.AddrOfPinnedObject());
            handle.Free();
        }
    }
}
