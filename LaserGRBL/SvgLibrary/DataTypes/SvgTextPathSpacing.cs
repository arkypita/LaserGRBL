using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>Indicates how the user agent should determine the spacing between glyphs that are to be rendered along a path.</summary>
    [TypeConverter(typeof(SvgTextPathSpacingConverter))]
    public enum SvgTextPathSpacing
    {
        /// <summary>Indicates that the glyphs should be rendered exactly according to the spacing rules as specified in Text on a path layout rules.</summary>
        Exact,

        /// <summary>Indicates that the user agent should use text-on-a-path layout algorithms to adjust the spacing between glyphs in order to achieve visually appealing results.</summary>
        Auto
    }
}
