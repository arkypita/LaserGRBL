using System;
using SharpGL.SceneGraph.Assets;

namespace SharpGL.SceneGraph.Core
{
    /// <summary>
    /// The IHasMaterial interface can be implemented by any scene object
    /// to allow a material to be associated with the object.
    /// </summary>
	public interface IHasMaterial
	{
        /// <summary>
        /// Gets or sets the material.
        /// </summary>
        /// <value>
        /// The material.
        /// </value>
	    Material Material 
        { 
            get; 
            set; 
        }
	}
}
