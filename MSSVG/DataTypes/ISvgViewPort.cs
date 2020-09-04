using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Provides properties and methods to be implemented by view port elements.
    /// </summary>
    public interface ISvgViewPort
    {
        /// <summary>
        /// Gets or sets the viewport of the element.
        /// </summary>
        SvgViewBox ViewBox { get; set; }
        SvgAspectRatio AspectRatio { get; set; }
		SvgOverflow Overflow { get; set; }
    }
}
