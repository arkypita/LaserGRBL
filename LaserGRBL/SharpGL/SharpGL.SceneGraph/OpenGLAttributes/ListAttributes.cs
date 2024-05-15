using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;
using SharpGL.Enumerations;

namespace SharpGL.OpenGLAttributes
{
	/// <summary>
	/// This class has all the settings you can edit for lists.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
    public class ListAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ListAttributes"/> class.
        /// </summary>
        public ListAttributes()
        {
            AttributeFlags = AttributeMask.List;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (listBase.HasValue) gl.ListBase(listBase.Value);
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return listBase.HasValue;
        }

        /// <summary>
        /// The list base.
        /// </summary>
        private uint? listBase;

        /// <summary>
        /// Gets or sets the list base.
        /// </summary>
        /// <value>
        /// The list base.
        /// </value>
		[Description("."), Category("List")]
        public uint? ListBase
		{
            get { return listBase; }
            set { listBase = value; }
		}
	}
}
