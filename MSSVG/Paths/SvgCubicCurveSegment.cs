using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Svg.Pathing
{
    public sealed class SvgCubicCurveSegment : SvgPathSegment
    {
        private PointF _firstControlPoint;
        private PointF _secondControlPoint;

        public PointF FirstControlPoint
        {
            get { return this._firstControlPoint; }
            set { this._firstControlPoint = value; }
        }

        public PointF SecondControlPoint
        {
            get { return this._secondControlPoint; }
            set { this._secondControlPoint = value; }
        }

        public SvgCubicCurveSegment(PointF start, PointF firstControlPoint, PointF secondControlPoint, PointF end)
        {
            this.Start = start;
            this.End = end;
            this._firstControlPoint = firstControlPoint;
            this._secondControlPoint = secondControlPoint;
        }

        public override void AddToPath(System.Drawing.Drawing2D.GraphicsPath graphicsPath)
        {
            graphicsPath.AddBezier(this.Start, this.FirstControlPoint, this.SecondControlPoint, this.End);
        }

        public override string ToString()
        {
        	return "C" + this.FirstControlPoint.ToSvgString() + " " + this.SecondControlPoint.ToSvgString() + " " + this.End.ToSvgString();
        }
    }
}
