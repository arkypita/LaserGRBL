using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Effects
{
    /// <summary>
    /// An effect is something that can be applied to a scene element which 
    /// then changes everything in the tree below it. It can be pushed, to apply it
    /// and popped, to restore OpenGL back to the state without the effect.
    /// </summary>
    public abstract class Effect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Effect"/> class.
        /// </summary>
        public Effect()
        {
            Name = "Effect";
            IsEnabled = true;
        }

        /// <summary>
        /// Pushes the effect onto the specified parent element.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="parentElement">The parent element.</param>
        public abstract void Push(OpenGL gl, SceneElement parentElement);

        /// <summary>
        /// Pops the effect off the specified parent element.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="parentElement">The parent element.</param>
        public abstract void Pop(OpenGL gl, SceneElement parentElement);

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled
        {
            get;
            set;
        }
    }
}
