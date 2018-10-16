using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    [SvgElement("font-face-src")]
    public class SvgFontFaceSrc : SvgElement
    {
        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgFontFaceSrc>();
        }
    }
}
