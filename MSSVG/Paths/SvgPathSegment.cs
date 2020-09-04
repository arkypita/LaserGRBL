using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Pathing
{
    public abstract class SvgPathSegment
    {
        private PointF _start;
        private PointF _end;

        public PointF Start
        {
            get { return this._start; }
            set { this._start = value; }
        }

        public PointF End
        {
            get { return this._end; }
            set { this._end = value; }
        }

        protected SvgPathSegment()
        {
        }

        protected SvgPathSegment(PointF start, PointF end)
        {
            this.Start = start;
            this.End = end;
        }

        public abstract void AddToPath(GraphicsPath graphicsPath);

		public SvgPathSegment Clone()
		{
			return this.MemberwiseClone() as SvgPathSegment;
		}
    }
}
