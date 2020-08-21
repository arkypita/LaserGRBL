using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// Indicates the algorithm which is to be used to determine the clipping region.
    /// </summary>
    /// <remarks>
    ///     <para>This rule determines the "insideness" of a point on the canvas by drawing a ray from 
    ///     that point to infinity in any direction and then examining the places where a segment of the 
    ///     shape crosses the ray.</para>
    /// </remarks>
    [TypeConverter(typeof(SvgClipRuleConverter))]
    public enum SvgClipRule
    {
        /// <summary>
        /// This rule determines the "insideness" of a point on the canvas by drawing a ray from that point to infinity in any direction and then examining the places where a segment of the shape crosses the ray. Starting with a count of zero, add one each time a path segment crosses the ray from left to right and subtract one each time a path segment crosses the ray from right to left. After counting the crossings, if the result is zero then the point is outside  the path. Otherwise, it is inside.
        /// </summary>
        NonZero,
        /// <summary>
        /// This rule determines the "insideness" of a point on the canvas by drawing a ray from that point to infinity in any direction and counting the number of path segments from the given shape that the ray crosses. If this number is odd, the point is inside; if even, the point is outside.
        /// </summary>
        EvenOdd
    }
}