using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph.Raytracing
{
    /// <summary>
    /// A SceneElement can implement IRayTracable to allow it to
    /// be raytraced.
    /// </summary>
    public interface IRayTracable
    {
        /// <summary>
        /// Raytraces the specified ray. If an intersection is found, it is returned,
        /// otherwise null is returned.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="scene">The scene.</param>
        /// <returns>The intersection with the object, or null.</returns>
        Intersection Raytrace(Ray ray, Scene scene);
    }
}
