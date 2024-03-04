using System;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Quadrics
{
    /// <summary>
    /// The Quadric draw style.
    /// </summary>
    public enum DrawStyle : uint
    {
        /// <summary>
        /// Points.
        /// </summary>
        Point = OpenGL.GLU_POINT,

        /// <summary>
        /// Lines.
        /// </summary>
        Line = OpenGL.GLU_LINE,

        /// <summary>
        /// Silhouette.
        /// </summary>
        Silhouette = OpenGL.GLU_SILHOUETTE,

        /// <summary>
        /// Fill.
        /// </summary>
        Fill = OpenGL.GLU_FILL
    }
}