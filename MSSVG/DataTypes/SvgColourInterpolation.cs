using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Svg.DataTypes
{
    /// <summary>Specifies the color space for gradient interpolations, color animations and alpha compositing.</summary>
    /// <remarks>When a child element is blended into a background, the value of the ‘color-interpolation’ property on the child determines the type of blending, not the value of the ‘color-interpolation’ on the parent. For gradients which make use of the ‘xlink:href’ attribute to reference another gradient, the gradient uses the ‘color-interpolation’ property value from the gradient element which is directly referenced by the ‘fill’ or ‘stroke’ property. When animating colors, color interpolation is performed according to the value of the ‘color-interpolation’ property on the element being animated.</remarks>
    [TypeConverter(typeof(SvgColourInterpolationConverter))]
	public enum SvgColourInterpolation
	{
        /// <summary>Indicates that the user agent can choose either the sRGB or linearRGB spaces for color interpolation. This option indicates that the author doesn't require that color interpolation occur in a particular color space.</summary>
		Auto,

        /// <summary>Indicates that color interpolation should occur in the sRGB color space.</summary>
        SRGB,

        /// <summary>Indicates that color interpolation should occur in the linearized RGB color space as described above.</summary>
        LinearRGB,

        /// <summary>The value is inherited from the parent element.</summary>
		Inherit
	}
}
