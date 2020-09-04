using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    /// <summary>Specifies the shape to be used at the end of open subpaths when they are stroked.</summary>
    [TypeConverter(typeof(SvgStrokeLineCapConverter))]
    public enum SvgStrokeLineCap
    {
        /// <summary>The value is inherited from the parent element.</summary>
        Inherit,

        /// <summary>The ends of the subpaths are square but do not extend past the end of the subpath.</summary>
        Butt,
        
        /// <summary>The ends of the subpaths are rounded.</summary>
        Round,

        /// <summary>The ends of the subpaths are square.</summary>
        Square
    }
}
