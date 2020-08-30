using System;
using System.Numerics;

//Copyright (C) 2001-2016 Peter Selinger
//Copyright (C) 2009-2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
// You should have received a copy of the GNU General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. 

namespace CsPotrace.BezierToBiarc
{
    /// <summary>
    /// Definition of an Arc. It contains redundant information
    /// </summary>
    public struct Arc
    {
        /// <summary>
        /// Center point
        /// </summary>
        public readonly Vector2 C;
        /// <summary>
        /// Radius
        /// </summary>
        public readonly float r;
        /// <summary>
        /// Start angle in radian
        /// </summary>
        public readonly float startAngle;
        /// <summary>
        /// Sweep angle in radian
        /// </summary>
        public readonly float sweepAngle;
        /// <summary>
        /// Start point of the arc
        /// </summary>
        public readonly Vector2 P1;
        /// <summary>
        /// End point of the arc
        /// </summary>
        public readonly Vector2 P2;

        public Arc(Vector2 C, float r, float startAngle, float sweepAngle, Vector2 P1, Vector2 P2)
        {
            this.C = C;
            this.r = r;
            this.startAngle = startAngle;
            this.sweepAngle = sweepAngle;
            this.P1 = P1;
            this.P2 = P2;
        }

        /// <summary>
        /// Orientation of the arc
        /// </summary>
        public bool IsClockwise
        {
            get { return sweepAngle > 0; }
        }

        /// <summary>
        /// Implement the parametric equation.
        /// </summary>
        /// <param name="t">Parameter of the curve. Must be in [0,1]</param>
        /// <returns></returns>
        public Vector2 PointAt(float t)
        {
            double x = C.X + r * Math.Cos(startAngle + t * sweepAngle);
            double y = C.Y + r * Math.Sin(startAngle + t * sweepAngle);
            return new Vector2((float)x, (float)y);
        }

        public float Length
        {
            get { return r * Math.Abs(sweepAngle); }
        }

        public float LinearLength
        {
            get
            {
                float dX = P2.X - P1.X;
                float dY = P2.Y - P2.Y;
                return (float)Math.Sqrt(dX * dX + dY * dY);
            }
        }
    }
}
