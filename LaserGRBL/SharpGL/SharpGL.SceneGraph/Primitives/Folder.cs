using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Primitives
{
    /// <summary>
    /// A Folder is used to organise scene elements.
    /// </summary>
    public class Folder : SceneElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        public Folder()
        {
            Name = "Folder";
        }
    }
}
