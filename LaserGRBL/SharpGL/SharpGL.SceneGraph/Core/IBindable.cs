using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.SceneGraph.Core
{
    /// <summary>
    /// An object that is Bindable is able to set itself into
    /// the current OpenGL instance. This can be lights, materials,
    /// attributes and so on.
    /// Bindable objects must be able to be used without interfering
    /// with later rendering, so as well as simply being bound directly, 
    /// they must be able to be pushed and popped.
    /// </summary>
    public interface IBindable
    {
        /// <summary>
        /// Pushes this object into the provided OpenGL instance.
        /// This will generally push elements of the attribute stack
        /// and then set current values.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        void Push(OpenGL gl);

        /// <summary>
        /// Pops this object from the provided OpenGL instance.
        /// This will generally pop elements of the attribute stack,
        /// restoring previous attribute values.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        void Pop(OpenGL gl);

        /// <summary>
        /// Bind to the specified OpenGL instance.
        /// Remember, this will not push or pop the attribute
        /// stack so will affect ALL subsequent rendering.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        void Bind(OpenGL gl);
    }
}
