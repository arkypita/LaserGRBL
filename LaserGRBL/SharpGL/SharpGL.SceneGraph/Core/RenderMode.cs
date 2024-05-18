using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph.Core
{
    /// <summary>
    /// The RenderMode enumeration is used to identify what kind
    /// of rendering is occuring.
    /// </summary>
    public enum RenderMode
    {
        /// <summary>
        /// We are designing.
        /// </summary>
        Design = 0,

        /// <summary>
        /// We are rendering for a hit test.
        /// </summary>
        HitTest = 1,

        /// <summary>
        /// We are rendering.
        /// </summary>
        Render = 2
    }
}
