using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Svg.Pathing
{
    public sealed class SvgQuadraticCurveSegment : SvgPathSegment
    {
        private PointF _controlPoint;

        public PointF ControlPoint
        {
            get { return this._controlPoint; }
            set { this._controlPoint = value; }
        }

        private PointF FirstControlPoint
        {
            get
            {
                float x1 = Start.X + (this.ControlPoint.X - Start.X) * 2 / 3;
                float y1 = Start.Y + (this.ControlPoint.Y - Start.Y) * 2 / 3;

                return new PointF(x1, y1);
            }
        }

        private PointF SecondControlPoint
        {
            get
            {
                float x2 = this.ControlPoint.X + (this.End.X - this.ControlPoint.X) / 3;
                float y2 = this.ControlPoint.Y + (this.End.Y - this.ControlPoint.Y) / 3;

                return new PointF(x2, y2);
            }
        }

        public SvgQuadraticCurveSegment(PointF start, PointF controlPoint, PointF end)
        {
            this.Start = start;
            this._controlPoint = controlPoint;
            this.End = end;
        }

        public override void AddToPath(System.Drawing.Drawing2D.GraphicsPath graphicsPath)
        {
            graphicsPath.AddBezier(this.Start, this.FirstControlPoint, this.SecondControlPoint, this.End);
        }
        
        public override string ToString()
		{
			return "Q" + this.ControlPoint.ToSvgString() + " " + this.End.ToSvgString();
		}

    }
}
