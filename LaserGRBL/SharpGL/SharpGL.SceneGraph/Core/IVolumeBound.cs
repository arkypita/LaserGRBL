using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph.Core
{
    /// <summary>
    /// A Scene Element can be volumne bound meaning that it can
    /// participate in hit testing and various optimisations.
    /// </summary>
    public interface IVolumeBound
    {
        /// <summary>
        /// Gets the bounding volume.
        /// </summary>
        BoundingVolume BoundingVolume
        {
            get;
        }
    }
}
