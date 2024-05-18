
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph.Transformations
{
    /// <summary>
    /// Base class for transformations.
    /// </summary>
    public abstract class Transformation
    {
        /// <summary>
        /// Performs the transformation on the current matrix.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public abstract void Transform(OpenGL gl);
    }
}
