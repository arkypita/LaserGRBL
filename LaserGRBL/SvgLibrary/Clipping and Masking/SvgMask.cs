using System;
using System.Collections.Generic;
using System.Text;

namespace Svg
{
    public class SvgMask : SvgElement
    {


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgMask>();
		}

    }
}