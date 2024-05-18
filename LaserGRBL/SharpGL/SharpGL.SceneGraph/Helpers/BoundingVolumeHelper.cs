using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Helpers
{
    /// <summary>
    /// The bounding helper.
    /// </summary>
    internal class BoundingVolumeHelper
    {
        /// <summary>
        /// The bounding volume.
        /// </summary>
        private BoundingVolume boundingVolume = new BoundingVolume();

        /// <summary>
        /// Gets the bounding volume.
        /// </summary>
        public BoundingVolume BoundingVolume
        {
            get { return boundingVolume; }
        }
    }
}
