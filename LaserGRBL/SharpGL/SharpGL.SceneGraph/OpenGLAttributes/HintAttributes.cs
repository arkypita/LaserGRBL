using System;
using System.Drawing;
using System.ComponentModel;

using SharpGL.SceneGraph;
using SharpGL.Enumerations;

namespace SharpGL.OpenGLAttributes
{
	/// <summary>
	/// This class has all the settings you can edit for hints.
	/// </summary>
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
	[Serializable()]
    public class HintAttributes : OpenGLAttributeGroup
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="HintAttributes"/> class.
        /// </summary>
        public HintAttributes()
        {
            AttributeFlags = SharpGL.Enumerations.AttributeMask.Hint;
        }

        /// <summary>
        /// Sets the attributes.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void SetAttributes(OpenGL gl)
        {
            if (perspectiveCorrectionHint.HasValue) gl.Hint(HintTarget.PerspectiveCorrection, perspectiveCorrectionHint.Value);
            if (pointSmoothHint.HasValue) gl.Hint(HintTarget.LineSmooth, pointSmoothHint.Value);
            if (lineSmoothHint.HasValue) gl.Hint(HintTarget.PointSmooth, lineSmoothHint.Value);
            if (polygonSmoothHint.HasValue) gl.Hint(HintTarget.PolygonSmooth, polygonSmoothHint.Value);
            if (fogHint.HasValue) gl.Hint(HintTarget.Fog, fogHint.Value);
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
                perspectiveCorrectionHint.HasValue ||
                pointSmoothHint.HasValue ||
                lineSmoothHint.HasValue ||
                polygonSmoothHint.HasValue ||
                fogHint.HasValue;
        }

        private HintMode? perspectiveCorrectionHint;
        private HintMode? pointSmoothHint;
        private HintMode? lineSmoothHint;
        private HintMode? polygonSmoothHint;
        private HintMode? fogHint;

        /// <summary>
        /// Gets or sets the perspective correction hint.
        /// </summary>
        /// <value>
        /// The perspective correction hint.
        /// </value>
        [Description("."), Category("Hint")]
        public HintMode? PerspectiveCorrectionHint
        {
            get { return perspectiveCorrectionHint; }
            set { perspectiveCorrectionHint = value; }
        }

        /// <summary>
        /// Gets or sets the point smooth hint.
        /// </summary>
        /// <value>
        /// The point smooth hint.
        /// </value>
        [Description("."), Category("Hint")]
        public HintMode? PointSmoothHint
        {
            get { return pointSmoothHint; }
            set { pointSmoothHint = value; }
        }

        /// <summary>
        /// Gets or sets the line smooth hint.
        /// </summary>
        /// <value>
        /// The line smooth hint.
        /// </value>
        [Description("."), Category("Hint")]
        public HintMode? LineSmoothHint
        {
            get { return lineSmoothHint; }
            set { lineSmoothHint = value; }
        }

        /// <summary>
        /// Gets or sets the polygon smooth hint.
        /// </summary>
        /// <value>
        /// The polygon smooth hint.
        /// </value>
        [Description("."), Category("Hint")]
        public HintMode? PolygonSmoothHint
        {
            get { return polygonSmoothHint; }
            set { polygonSmoothHint = value; }
        }

        /// <summary>
        /// Gets or sets the fog hint.
        /// </summary>
        /// <value>
        /// The fog hint.
        /// </value>
        [Description("."), Category("Hint")]
        public HintMode? FogHint
        {
            get { return fogHint; }
            set { fogHint = value; }
        }
	}
}
