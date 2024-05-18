using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph.Core
{
    /// <summary>
    /// Scene Elements can be marked as Freezeable. If scene objects
    /// are freezable, they can be frozen, meaning that they are locked.
    /// Generally this means compiling the object's geometry into
    /// a display list.
    /// </summary>
    public interface IFreezable
    {
        /// <summary>
        /// Freezes this instance using the provided OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        void Freeze(OpenGL gl);

        /// <summary>
        /// Unfreezes this instance using the provided OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        void Unfreeze(OpenGL gl);

        /// <summary>
        /// Gets a value indicating whether this instance is frozen.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is frozen; otherwise, <c>false</c>.
        /// </value>
        bool IsFrozen
        {
            get;
        }
    }
}
