using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.Enumerations;

namespace SharpGL.OpenGLAttributes
{
    /// <summary>
    /// The OpenGLAttributeGroup is the base for groups of opengl attributes.
    /// </summary>
    public abstract class OpenGLAttributeGroup
    {
        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public abstract void SetAttributes(OpenGL gl);

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>True if any attributes are set</returns>
        public abstract bool AreAnyAttributesSet();

        /// <summary>
        /// Pushes the attributes onto the specified OpenGL attribute stack.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void Push(OpenGL gl)
        {
            gl.PushAttrib((uint)AttributeFlags);
        }

        /// <summary>
        /// Pops the attributes off the specified OpenGL attribute stack.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void Pop(OpenGL gl)
        {
            gl.PopAttrib();
        }

        /// <summary>
        /// The attribute flags for the group.
        /// </summary>
        private AttributeMask attributeFlags = AttributeMask.None;

        /// <summary>
        /// Gets the attribute flags.
        /// todo use get only, xmlignore and don't store them - return them on the fly.
        /// </summary>
        public AttributeMask AttributeFlags
        {
            get { return attributeFlags; }
            set { attributeFlags = value; }
        }
    }
}
