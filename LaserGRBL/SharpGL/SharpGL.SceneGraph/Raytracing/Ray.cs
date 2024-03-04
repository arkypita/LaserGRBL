using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Raytracing
{
    /// <summary>
    /// A Ray.
    /// </summary>
    public class Ray
    {
        /// <summary>
        /// The light.
        /// </summary>
        public GLColor light = new GLColor(0, 0, 0, 0);

        /// <summary>
        /// The origin.
        /// </summary>
        public Vertex origin = new Vertex();

        /// <summary>
        /// The direction.
        /// </summary>
        public Vertex direction = new Vertex();
    }
}
