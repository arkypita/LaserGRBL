using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    [TypeConverter(typeof(SvgFontVariantConverter))]
    public enum SvgFontVariant
    {
        Normal,
        Smallcaps,
        Inherit
    }
}
