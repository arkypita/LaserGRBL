using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SharpGL.SceneGraph.Assets
{
    /// <summary>
    /// An Asset is something which is used in the scene, but is not in the scene 
    /// tree. An example of an asset is a material, which there may be one instance
    /// of which is shared between many objects.
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return GetType().Name + ": " + Name;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [Description("The unique ID of the Asset."), Category("Asset")]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Description("The name of the Asset."), Category("Material")]
        public string Name
        {
            get;
            set;
        }
    }
}
