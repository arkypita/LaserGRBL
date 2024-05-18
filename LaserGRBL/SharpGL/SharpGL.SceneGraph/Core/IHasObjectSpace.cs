using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Transformations;

namespace SharpGL.SceneGraph.Core
{
    /// <summary>
    /// A SceneElement can implement IHasObjectSpace to allow it to transform
    /// world space into object space.
    /// </summary>
    public interface IHasObjectSpace
    {
        /// <summary>
        /// Pushes us into Object Space using the transformation into the specified OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        void PushObjectSpace(OpenGL gl);

        /// <summary>
        /// Pops us from Object Space using the transformation into the specified OpenGL instance.
        /// </summary>
        /// <param name="gl">The gl.</param>
        void PopObjectSpace(OpenGL gl);

        /// <summary>
        /// Gets the transformation that pushes us into object space.
        /// </summary>
        LinearTransformation Transformation
        {
            get;
        }
    }
}
