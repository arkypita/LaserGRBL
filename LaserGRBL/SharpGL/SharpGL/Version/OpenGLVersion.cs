namespace SharpGL.Version
{
    /// <summary>
    /// Used to specify explictly a version of OpenGL.
    /// </summary>
    public enum OpenGLVersion
    {
        /// <summary>
        /// Version 1.1
        /// </summary>
        [Version(1, 1)] OpenGL1_1,

        /// <summary>
        /// Version 1.2
        /// </summary>
        [Version(1, 2)] OpenGL1_2,

        /// <summary>
        /// Version 1.3
        /// </summary>
        [Version(1, 3)] OpenGL1_3,

        /// <summary>
        /// Version 1.4
        /// </summary>
        [Version(1, 4)] OpenGL1_4,

        /// <summary>
        /// Version 1.5
        /// </summary>
        [Version(1, 5)] OpenGL1_5,

        /// <summary>
        /// OpenGL 2.0
        /// </summary>
        [Version(2, 0)] OpenGL2_0,

        /// <summary>
        /// OpenGL 2.1
        /// </summary>
        [Version(2, 1)] OpenGL2_1,

        /// <summary>
        /// OpenGL 3.0. This is the first version of OpenGL that requires a specially constructed render context.
        /// </summary>
        [Version(3, 0)] OpenGL3_0,

        /// <summary>
        /// OpenGL 3.1
        /// </summary>
        [Version(3, 1)] OpenGL3_1,

        /// <summary>
        /// OpenGL 3.2
        /// </summary>
        [Version(3, 2)] OpenGL3_2,

        /// <summary>
        /// OpenGL 3.3
        /// </summary>
        [Version(3, 3)] OpenGL3_3,

        /// <summary>
        /// OpenGL 4.0
        /// </summary>
        [Version(4, 0)] OpenGL4_0,

        /// <summary>
        /// OpenGL 4.1
        /// </summary>
        [Version(4, 1)] OpenGL4_1,

        /// <summary>
        /// OpenGL 4.2
        /// </summary>
        [Version(4, 2)] OpenGL4_2,

        /// <summary>
        /// OpenGL 4.3
        /// </summary>
        [Version(4, 3)] OpenGL4_3,

        /// <summary>
        /// OpenGL 4.4
        /// </summary>
        [Version(4, 4)] OpenGL4_4
    }
}