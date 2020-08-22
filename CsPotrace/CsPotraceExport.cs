using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

//Copyright (C) Dileep.M
//E-mail :  m.dileep@gmail.com,


// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
// You should have received a copy of the GNU General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. 

namespace CsPotrace
{
    public partial class Potrace
    {

        /// <summary>
        /// Exports a figure,  created by Potrace from a Bitmap to a svg-formatted string 
        ///It should be SVG-formattted

        /// </summary>
        /// <param name="Fig">Arraylist, which contains vectorinformations about the Curves</param>
        /// <param name="Width">Width of the Bitmap</param>
        /// <param name="Height">Height of the Bitmap</param>
        /// <returns></returns>
        public static void Export2GDIPlus(List<List<Curve>> Fig, Graphics g, Brush fill, Pen border, double inset)
        {
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            GraphicsPath gp = new GraphicsPath();
            foreach (List<Curve> LC in Fig)
            {
                GraphicsPath Current = new GraphicsPath();
                for (int j = 0; j < LC.Count; j++)
                {

                    Curve C = LC[j];
                    if (C.Kind == CurveKind.Line)
                        Current.AddLine(new PointF((float)C.A.X, (float)C.A.Y), new PointF((float)C.B.X, (float)C.B.Y));
                    else
                    {
                        PointF A = new PointF((float)C.A.X, (float)C.A.Y);
                        Current.AddBezier(new PointF((float)C.A.X, (float)C.A.Y), new PointF((float)C.ControlPointA.X, (float)C.ControlPointA.Y), new PointF((float)C.ControlPointB.X, (float)C.ControlPointB.Y), new PointF((float)C.B.X, (float)C.B.Y));
                    }

                }
                gp.AddPath(Current, false);
            }

            if (fill != null)
                g.FillPath(fill, gp);

            if (border != null)
                g.DrawPath(border, gp);


            if (inset > 0)
            {
                using (Pen p = new Pen(Color.White, (float)inset))
                    g.DrawPath(p, gp);
            }

        }
    }
}
