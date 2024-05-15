using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph.Core
{
    /// <summary>
    /// Any element or asset which has an OpenGL context has some 
    /// associated OpenGL resource. This means that when it is loaded
    /// from file, it may need to be re-created, or if it is moved
    /// between OpenGL contexts it may need to be re-created.
    /// Any object that has an OpenGL context has a Create method, a 
    /// Destroy Method and a CurrentContext property.
    /// </summary>
    public interface IHasOpenGLContext
    {
        /// <summary>
        /// Create in the context of the supplied OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        void CreateInContext(OpenGL gl);

        /// <summary>
        /// Destroy in the context of the supplied OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        void DestroyInContext(OpenGL gl);

        /// <summary>
        /// Gets the current OpenGL that the object exists in context.
        /// </summary>
        OpenGL CurrentOpenGLContext
        {
            get;
        }
    }
}
