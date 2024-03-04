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
    public class TransformAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="TransformAttributes"/> class.
        /// </summary>
        public TransformAttributes()
        {
            AttributeFlags = SharpGL.Enumerations.AttributeMask.Transform;

            /*TODO
             * Coefficients of the six clipping planes

Enable bits for the user-definable clipping planes
*/
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (enableNormalize.HasValue) gl.EnableIf(OpenGL.GL_NORMALIZE, enableNormalize.Value);
            if (matrixMode.HasValue) gl.MatrixMode(matrixMode.Value);
        }

        /// <summary>
        /// Returns true if any attributes are set.
        /// </summary>
        /// <returns>
        /// True if any attributes are set
        /// </returns>
        public override bool AreAnyAttributesSet()
        {
            return 
                enableNormalize.HasValue ||
                matrixMode.HasValue;
        }

        private bool? enableNormalize;
        private MatrixMode? matrixMode;

        /// <summary>
        /// Gets or sets the enable normalize.
        /// </summary>
        /// <value>
        /// The enable normalize.
        /// </value>
		[Description("."), Category("Transform")]
        public bool? EnableNormalize
		{
            get { return enableNormalize; }
            set { enableNormalize = value; }
		}

        /// <summary>
        /// Gets or sets the matrix mode.
        /// </summary>
        /// <value>
        /// The matrix mode.
        /// </value>
        [Description("."), Category("Transform")]
        public MatrixMode? MatrixMode
        {
            get { return matrixMode; }
            set { matrixMode = value; }
        }
	}
}
