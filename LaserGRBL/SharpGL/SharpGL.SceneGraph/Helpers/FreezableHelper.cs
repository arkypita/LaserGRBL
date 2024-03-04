using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Helpers
{
    internal class FreezableHelper
    {
        /// <summary>
        /// Freezes the specified instance.
        /// </summary>
        /// <param name="gl">The gl instance.</param>
        /// <param name="renderable">The renderable.</param>
        public void Freeze(OpenGL gl, IRenderable renderable)
        {
            //  Handle the trivial case.
            if (isFrozen)
                return;

            //  Create the display list.
            if (displayList != null)
                displayList.Delete(gl);
            displayList = new DisplayList();
            displayList.Generate(gl);

            //  Start the display list.
            displayList.New(gl, DisplayList.DisplayListMode.Compile);

            //  Render the object.
            renderable.Render(gl, RenderMode.Design);

            //  End the display list.
            displayList.End(gl);

            //  We are now frozen.
            isFrozen = true;
        }

        /// <summary>
        /// Renders the specified gl.
        /// </summary>
        /// <param name="gl">The gl.</param>
        public void Render(OpenGL gl)
        {
            if (isFrozen)
                displayList.Call(gl);
        }

        /// <summary>
        /// Unfreezes the specified gl.
        /// </summary>
        /// <param name="gl">The gl.</param>
        public void Unfreeze(OpenGL gl)
        {
            if (isFrozen)
            {
                displayList.Delete(gl);
                isFrozen = false;
            }
        }

        /// <summary>
        /// The display list internally.
        /// </summary>
        private DisplayList displayList = null;

        /// <summary>
        /// If true, we're frozen.
        /// </summary>
        private bool isFrozen = false;

        /// <summary>
        /// Gets a value indicating whether this instance is frozen.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is frozen; otherwise, <c>false</c>.
        /// </value>
        public bool IsFrozen
        {
            get { return isFrozen; }
        }
    }
}
