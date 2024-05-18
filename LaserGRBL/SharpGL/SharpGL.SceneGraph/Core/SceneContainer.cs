using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph.Core
{
    /// <summary>
    /// The Scene Container is the top-level object in a scene graph.
    /// </summary>
    public class SceneContainer : SceneElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SceneContainer"/> class.
        /// </summary>
        public SceneContainer()
        {
            Name = "Scene Container";
        }

        /// <summary>
        /// Gets or sets the parent scene.
        /// </summary>
        /// <value>
        /// The parent scene.
        /// </value>
        [XmlIgnore]
        public Scene ParentScene
        {
            get;
            set;
        }
    }
}
