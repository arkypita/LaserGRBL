using System.Drawing;

namespace Svg
{
    public interface ISvgBoundable
    {
        PointF Location
        {
            get;
        }

        SizeF Size
        {
            get;
        }

        RectangleF Bounds
        {
            get;
        } 
    }
}