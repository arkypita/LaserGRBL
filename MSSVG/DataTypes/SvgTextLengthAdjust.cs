using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>Indicates the type of adjustments which the user agent shall make to make the rendered length of the text match the value specified on the ‘textLength’ attribute.</summary>
    /// <remarks>
    ///     <para>The user agent is required to achieve correct start and end positions for the text strings, but the locations of intermediate glyphs are not predictable because user agents might employ advanced algorithms to stretch or compress text strings in order to balance correct start and end positioning with optimal typography.</para>
    ///     <para>Note that, for a text string that contains n characters, the adjustments to the advance values often occur only for n−1 characters (see description of attribute ‘textLength’), whereas stretching or compressing of the glyphs will be applied to all n characters.</para>
    /// </remarks>
    [TypeConverter(typeof(SvgTextLengthAdjustConverter))]
    public enum SvgTextLengthAdjust
    {
        /// <summary>Indicates that only the advance values are adjusted. The glyphs themselves are not stretched or compressed.</summary>
        Spacing,

        /// <summary>Indicates that the advance values are adjusted and the glyphs themselves stretched or compressed in one axis (i.e., a direction parallel to the inline-progression-direction).</summary>
        SpacingAndGlyphs
    }
}
