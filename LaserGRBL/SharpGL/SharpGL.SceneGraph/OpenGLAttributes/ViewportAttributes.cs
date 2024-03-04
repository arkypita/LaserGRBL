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
    public class ViewportAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewportAttributes"/> class.
        /// </summary>
        public ViewportAttributes()
        {
            AttributeFlags = AttributeMask.Viewport;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (viewportX.HasValue && viewportY.HasValue && viewportWidth.HasValue && viewportHeight.HasValue)
                gl.Viewport(viewportX.Value, viewportY.Value, viewportWidth.Value, viewportHeight.Value);
            if (depthRangeNear.HasValue && depthRangeFar.HasValue)
                gl.DepthRange(depthRangeNear.Value, depthRangeFar.Value);
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
                viewportX.HasValue ||
                viewportY.HasValue ||
                viewportWidth.HasValue ||
                viewportHeight.HasValue ||
                depthRangeNear.HasValue ||
                depthRangeFar.HasValue;
        }

        private int? viewportX;
        private int? viewportY;
        private int? viewportWidth;
        private int? viewportHeight;
        private double? depthRangeNear;
        private double? depthRangeFar;

        /// <summary>
        /// Gets or sets the viewport X.
        /// </summary>
        /// <value>
        /// The viewport X.
        /// </value>
        [Description("."), Category("Viewport")]
        public int? ViewportX
        {
            get { return viewportX; }
            set { viewportX = value; }
        }

        /// <summary>
        /// Gets or sets the viewport Y.
        /// </summary>
        /// <value>
        /// The viewport Y.
        /// </value>
        [Description("."), Category("Viewport")]
        public int? ViewportY
        {
            get { return viewportY; }
            set { viewportY = value; }
        }

        /// <summary>
        /// Gets or sets the width of the viewport.
        /// </summary>
        /// <value>
        /// The width of the viewport.
        /// </value>
        [Description("."), Category("Viewport")]
        public int? ViewportWidth
        {
            get { return viewportWidth; }
            set { viewportWidth = value; }
        }

        /// <summary>
        /// Gets or sets the height of the viewport.
        /// </summary>
        /// <value>
        /// The height of the viewport.
        /// </value>
        [Description("."), Category("Viewport")]
        public int? ViewportHeight
        {
            get { return viewportHeight; }
            set { viewportHeight = value; }
        }

        /// <summary>
        /// Gets or sets the depth range near.
        /// </summary>
        /// <value>
        /// The depth range near.
        /// </value>
        [Description("."), Category("Viewport")]
        public double? DepthRangeNear
        {
            get { return depthRangeNear; }
            set { depthRangeNear = value; }
        }

        /// <summary>
        /// Gets or sets the depth range far.
        /// </summary>
        /// <value>
        /// The depth range far.
        /// </value>
        [Description("."), Category("Viewport")]
        public double? DepthRangeFar
        {
            get { return depthRangeFar; }
            set { depthRangeFar = value; }
        }
	}
}
