using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>The ‘overflow’ property applies to elements that establish new viewports (e.g., ‘svg’ elements), ‘pattern’ elements and ‘marker’ elements. For all other elements, the property has no effect (i.e., a clipping rectangle is not created).</summary>
    /// <remarks>
    ///     <para>The ‘overflow’ property has the same parameter values and has the same meaning as defined in CSS2 ([CSS2], section 11.1.1); however, the following additional points apply:</para>
    ///     <para>The ‘overflow’ property applies to elements that establish new viewports (e.g., ‘svg’ elements), ‘pattern’ elements and ‘marker’ elements. For all other elements, the property has no effect (i.e., a clipping rectangle is not created).</para>
    ///     <para>For those elements to which the ‘overflow’ property can apply, if the ‘overflow’ property has the value hidden or scroll, the effect is that a new clipping path in the shape of a rectangle is created. The result is equivalent to defining a ‘clipPath’ element whose content is a ‘rect’ element which defines the equivalent rectangle, and then specifying the <uri> of this ‘clipPath’ element on the ‘clip-path’ property for the given element.</para>
    ///     <para>If the ‘overflow’ property has a value other than hidden or scroll, the property has no effect (i.e., a clipping rectangle is not created).</para>
    ///     <para>Within SVG content, the value auto is equivalent to the value visible.</para>
    ///     <para>When an outermost svg element is embedded inline within a parent XML grammar which uses CSS layout ([CSS2], chapter 9) or XSL formatting [XSL], if the ‘overflow’ property has the value hidden or scroll, then the user agent will establish an initial clipping path equal to the bounds of the initial viewport; otherwise, the initial clipping path is set according to the clipping rules as defined in CSS2 ([CSS2], section 11.1.1).</para>
    ///     <para>When an outermost svg element is stand-alone or embedded inline within a parent XML grammar which does not use CSS layout or XSL formatting, the ‘overflow’ property on the outermost svg element is ignored for the purposes of visual rendering and the initial clipping path is set to the bounds of the initial viewport.</para>
    ///     <para>The initial value for ‘overflow’ as defined in [CSS2-overflow] is 'visible', and this applies also to the root ‘svg’ element; however, for child elements of an SVG document, SVG's user agent style sheet overrides this initial value and sets the ‘overflow’ property on elements that establish new viewports (e.g., ‘svg’ elements), ‘pattern’ elements and ‘marker’ elements to the value 'hidden'.</para>
    ///     <para>As a result of the above, the default behavior of SVG user agents is to establish a clipping path to the bounds of the initial viewport and to establish a new clipping path for each element which establishes a new viewport and each ‘pattern’ and ‘marker’ element.</para>
    /// </remarks>
    [TypeConverter(typeof(SvgOverflowConverter))]
	public enum SvgOverflow
    {
        /// <summary>Overflow is not rendered.</summary>
        Hidden,

        /// <summary>The value is inherited from the parent element.</summary>
		Inherit,

        /// <summary>The overflow is rendered - same as "visible".</summary>
        Auto,

        /// <summary>Overflow is rendered.</summary>
        Visible,

        /// <summary>Overflow causes a scrollbar to appear (horizontal, vertical or both).</summary>
		Scroll
	}
}
