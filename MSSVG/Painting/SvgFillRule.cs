using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Svg
{
	[TypeConverter(typeof(SvgFillRuleConverter))]
    public enum SvgFillRule
    {
        NonZero,
        EvenOdd,
        Inherit
    }
}