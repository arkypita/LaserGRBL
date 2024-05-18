using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph.Core
{
    /// <summary>
    /// A Scene Element can implement this interface to enable rendering
    /// functionality. Note that many scene elements (materials etc) are
    /// not actually drawn.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        void Render(OpenGL gl, RenderMode renderMode);
    }
}
