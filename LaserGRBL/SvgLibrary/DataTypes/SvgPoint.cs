using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Svg
{
    public struct SvgPoint
    {
        private SvgUnit x;
        private SvgUnit y;

        public SvgUnit X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public SvgUnit Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public PointF ToDeviceValue(ISvgRenderer renderer, SvgElement owner)
        {
            return SvgUnit.GetDevicePoint(this.X, this.Y, renderer, owner);
        }

        public bool IsEmpty()
        {
            return (this.X.Value == 0.0f && this.Y.Value == 0.0f);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (!(obj.GetType() == typeof(SvgPoint))) return false;

            var point = (SvgPoint)obj;
            return (point.X.Equals(this.X) && point.Y.Equals(this.Y));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public SvgPoint(string x, string y)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(SvgUnit));

            this.x = (SvgUnit)converter.ConvertFrom(x);
            this.y = (SvgUnit)converter.ConvertFrom(y);
        }

        public SvgPoint(SvgUnit x, SvgUnit y)
        {
            this.x = x;
            this.y = y;
        }
    }
}