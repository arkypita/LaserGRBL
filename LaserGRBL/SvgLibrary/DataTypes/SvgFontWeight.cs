using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    //TODO This should be split out to define an enum for the font face element and text element.
    /// <summary>The weight of a face relative to others in the same font family.</summary>
    [TypeConverter(typeof(SvgFontWeightConverter))]
    [Flags]
    public enum SvgFontWeight
    {
        //TODO All Is not valid for text elements, but is is for font face elements.
        /// <summary>All font weights.</summary>
        All = (W100 | W200 | W300 | W400 | W500 | W600 | W700 | W800 | W900),

        //TODO Inherit Is not valid for font face elements, but is is for text elements.
        /// <summary>The value is inherited from the parent element.</summary>
        Inherit = 0,

        /// <summary>Same as <see cref="W400"/>.</summary>
        Normal = W400,

        /// <summary>Same as <see cref="W700"/>.</summary>
        Bold = W700,

        /// <summary>One font weight darker than the parent element.</summary>
        Bolder = 512,

        /// <summary>One font weight lighter than the parent element.</summary>
        Lighter = 1024,

        /// <summary></summary>
        W100 = 1,

        /// <summary></summary>
        W200 = 2,

        /// <summary></summary>
        W300 = 4,

        /// <summary>Same as <see cref="Normal"/>.</summary>
        W400 = 8,

        /// <summary></summary>
        W500 = 16,

        /// <summary></summary>
        W600 = 32,

        /// <summary>Same as <see cref="Bold"/>.</summary>
        W700 = 64,

        /// <summary></summary>
        W800 = 128,

        /// <summary></summary>
        W900 = 256
    }
}
