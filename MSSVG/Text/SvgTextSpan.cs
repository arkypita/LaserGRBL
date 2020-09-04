using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Svg
{
    [SvgElement("tspan")]
    public class SvgTextSpan : SvgTextBase
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTextSpan>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgTextSpan;
            newObj.X = this.X;
            newObj.Y = this.Y;
            newObj.Dx = this.Dx;
            newObj.Dy = this.Dy;
            newObj.Text = this.Text;

            return newObj;
        }


    }
}