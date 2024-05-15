using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;
using SharpGL.Enumerations;

namespace SharpGL.OpenGLAttributes
{
	/// <summary>
	/// This class has all the settings you can edit for fog.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
    public class EvalAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="EvalAttributes"/> class.
        /// </summary>
        public EvalAttributes()
        {
            AttributeFlags = AttributeMask.Eval;

            /*
             * 
             * GL_MAP1_x enable bits, where x is a map type

GL_MAP2_x enable bits, where x is a map type

1-D grid endpoints and divisions

2-D grid endpoints and divisions

GL_AUTO_NORMAL enable bit*/
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (enableAutoNormal.HasValue) gl.EnableIf(OpenGL.GL_AUTO_NORMAL, enableAutoNormal.Value);
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return enableAutoNormal.HasValue;
        }

        private bool? enableAutoNormal;

		[Description("."), Category("Eval")]
        public bool? EnableAutoNormal
		{
            get { return enableAutoNormal; }
            set { enableAutoNormal = value; }
		}
	}
}
