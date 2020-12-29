using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;
using System.Collections;
using CsPotrace;
using System.Drawing.Drawing2D;
using System.IO;

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


//		/// <summary>
//		/// Exports a figure, created by Potrace from a Bitmap to a svg-formatted string
//		/// </summary>
//		/// <param name="Fig">Arraylist, which contains vectorinformations about the Curves</param>
//		/// <param name="Width">Width of the exportd cvg-File</param>
//		/// <param name="Height">Height of the exportd cvg-File</param>
//		/// <returns></returns>
//		public static string Export2SVG(ArrayList Fig, int Width, int Height)
//		{
            

//			StringBuilder svg = new StringBuilder();
//			string head = String.Format(@"<?xml version='1.0' standalone='no'?>
//<!DOCTYPE svg PUBLIC '-//W3C//DTD SVG 20010904//EN' 
//'http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd'>
//<svg version='1.0' xmlns='http://www.w3.org/2000/svg' preserveAspectRatio='xMidYMid meet'
//width='{0}' height='{1}'  viewBox='0 0 {0} {1}'>
//<g>", Width, Height);
//			svg.AppendLine(head);

//			foreach (ArrayList Path in Fig)
//			{

//				bool isContour = true;
//				svg.Append("<path d='");
//				for(int i=0;i< Path.Count;i++)
//				{
//					CsPotrace.Curve[] Curves = (CsPotrace.Curve[])Path[i];
                   
//					string curve_path = isContour ? GetContourPath(Curves) : GetHolePath(Curves);

                   

//					if (i == Path.Count-1)
//					{
//					   svg.Append(curve_path);
//					}
//					else
//					{
//					 svg.AppendLine(curve_path);
//					}

//					isContour = false;
                   
//				}
//				svg.AppendLine("' />");

//			}



//			string foot = @"</g>
//</svg>";
//			svg.AppendLine(foot);
//			svg.Replace("'", "\"");


//			return svg.ToString();
//		}



        //static System.Globalization.CultureInfo enUsCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");


		//public static GraphicsPath Shrink(GraphicsPath path, float width)
		//{
		//	using (GraphicsPath p = new GraphicsPath())
		//	{
		//		p.AddPath(path, false);
		//		p.CloseAllFigures();
		//		p.Widen(new Pen(Color.Black, width * 2));

		//		int position = 0;
		//		GraphicsPath result = new GraphicsPath();
		//		while (position < p.PointCount)
		//		{
		//			// skip outer edge
		//			position += CountNextFigure(p.PathData, position);
		//			// count inner edge
		//			int figureCount = CountNextFigure(p.PathData, position);
		//			PointF[] points = new PointF[figureCount];
		//			byte[] types = new byte[figureCount];

		//			Array.Copy(p.PathPoints, position, points, 0, figureCount);
		//			Array.Copy(p.PathTypes, position, types, 0, figureCount);
		//			position += figureCount;
		//			result.AddPath(new GraphicsPath(points, types), false);
		//		}
		//		path.Reset();
		//		path.AddPath(result, false);
		//		return path;
		//	}
		//}

		//static int CountNextFigure(PathData data, int position)
		//{
		//	int count = 0;
		//	for (int i = position; i < data.Types.Length; i++)
		//	{
		//		count++;
		//		if (0 != (data.Types[i] & (int)PathPointType.CloseSubpath))
		//		{
		//			return count;
		//		}
		//	}
		//	return count;
		//}


        //private static string  GetContourPath(CsPotrace.Curve[] Curves)
        //{
           
        //    StringBuilder path = new StringBuilder();
            
        //    for(int i=0;i<Curves.Length;i++)
        //    {
        //        CsPotrace.Curve Curve = Curves[i];

                
        //        if (i == 0)
        //        {
        //            path.AppendLine("M" + Curve.A.X.ToString("0.0", enUsCulture) + " " + Curve.A.Y.ToString("0.0", enUsCulture));
        //        }

        //        if (Curve.Kind == CsPotrace.CurveKind.Bezier)
        //        {

        //            path.Append("C" + Curve.ControlPointA.X.ToString("0.0", enUsCulture) + " " + Curve.ControlPointA.Y.ToString("0.0", enUsCulture) + " " +
        //                            Curve.ControlPointB.X.ToString("0.0", enUsCulture) + " " + Curve.ControlPointB.Y.ToString("0.0", enUsCulture) + " " +
        //                            Curve.B.X.ToString("0.0", enUsCulture) + " " + Curve.B.Y.ToString("0.0", enUsCulture));




        //        }
        //        if (Curve.Kind == CsPotrace.CurveKind.Line)
        //        {
        //            path.Append("L" + Curve.B.X.ToString("0.0", enUsCulture) + " " + Curve.B.Y.ToString("0.0", enUsCulture));

        //        }
        //        if (i == Curves.Length - 1)
        //        {
        //            path.Append("Z");
        //        }
        //        else
        //        {
        //            path.AppendLine("");
        //        }
               
             
        //    }

            

        //    return path.ToString();
        //}

        //private static string  GetHolePath(CsPotrace.Curve[] Curves)
        //{
        //    StringBuilder path = new StringBuilder();
           
        //    for (int i = Curves.Length-1; i >=0 ; i--)
        //    {
        //        CsPotrace.Curve Curve = Curves[i];


        //        if (i == Curves.Length - 1)
        //        {
        //            path.AppendLine("M" + Curve.B.X.ToString("0.0", enUsCulture) + " " + Curve.B.Y.ToString("0.0", enUsCulture));
        //        }

        //        if (Curve.Kind == CsPotrace.CurveKind.Bezier)
        //        {



        //            path.Append("C" + Curve.ControlPointB.X.ToString("0.0", enUsCulture) + " " + Curve.ControlPointB.Y.ToString("0.0", enUsCulture) + " " +
        //                          Curve.ControlPointA.X.ToString("0.0", enUsCulture) + " " + Curve.ControlPointA.Y.ToString("0.0", enUsCulture) + " " +
        //                          Curve.A.X.ToString("0.0", enUsCulture) + " " + Curve.A.Y.ToString("0.0", enUsCulture));



        //        }
        //        if (Curve.Kind == CsPotrace.CurveKind.Line)
        //        {
        //            path.Append("L" + Curve.B.X.ToString("0.0", enUsCulture) + " " + Curve.B.Y.ToString("0.0", enUsCulture));

        //        }
        //        if (i == 0)
        //        {
        //            path.Append("Z");
        //        }
        //        else
        //        {
        //            path.AppendLine("");
        //        }

        //    }

           

        //    return path.ToString();
        //}
    }
}
