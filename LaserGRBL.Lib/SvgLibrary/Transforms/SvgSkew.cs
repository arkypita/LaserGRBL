﻿using System;
﻿using System.Drawing.Drawing2D;
using System.Globalization;

namespace Svg.Transforms
{
    /// <summary>
    /// The class which applies the specified skew vector to this Matrix.
    /// </summary>
    public sealed class SvgSkew : SvgTransform
    {
        public float AngleX { get; set; }

        public float AngleY { get; set; }

        public override Matrix Matrix
        {
            get
            {
                var matrix = new Matrix();
                matrix.Shear(
                    (float)Math.Tan(AngleX/180*Math.PI),
                    (float)Math.Tan(AngleY/180*Math.PI));
                return matrix;
            }
        }

        public override string WriteToString()
        {
            if (this.AngleY == 0)
            {
                return string.Format(CultureInfo.InvariantCulture, "skewX({0})", this.AngleX);
            }
            else
            {
                return string.Format(CultureInfo.InvariantCulture, "skewY({0})", this.AngleY);
            }
        }

        public SvgSkew(float x, float y)
        {
            AngleX = x;
            AngleY = y;
        }


		public override object Clone()
		{
			return new SvgSkew(this.AngleX, this.AngleY);
		}
    }
}