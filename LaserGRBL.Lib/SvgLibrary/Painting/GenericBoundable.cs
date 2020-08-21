using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Svg
{
    internal class GenericBoundable : ISvgBoundable
    {
        private RectangleF _rect;

        public GenericBoundable(RectangleF rect)
        {
            _rect = rect;
        }
        public GenericBoundable(float x, float y, float width, float height)
        {
            _rect = new RectangleF(x, y, width, height);
        }

        public System.Drawing.PointF Location
        {
            get { return _rect.Location; }
        }

        public System.Drawing.SizeF Size
        {
            get { return _rect.Size; }
        }

        public System.Drawing.RectangleF Bounds
        {
            get { return _rect; }
        }
    }
}
