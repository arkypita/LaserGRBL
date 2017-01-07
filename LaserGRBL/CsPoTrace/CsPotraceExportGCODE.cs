using System;
using System.Collections.Generic;
using CsPotrace.BezierToBiarc;
using System.Collections;
using CsPotrace;
using System.Numerics;
using System.Drawing;

namespace CsPotrace
{
	/// <summary>
	/// Description of CsPotraceExportGCODE.
	/// </summary>
	public partial class Potrace
	{

		        /// <summary>
        /// Exports a figure, created by Potrace from a Bitmap to a svg-formatted string
        /// </summary>
        /// <param name="Fig">Arraylist, which contains vectorinformations about the Curves</param>
        /// <param name="Width">Width of the exportd cvg-File</param>
        /// <param name="Height">Height of the exportd cvg-File</param>
        /// <returns></returns>
        public static List<string> Export2GCode(ArrayList Fig, int oX, int oY, double scale , string lOn, string lOff)
        {
//       	System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
//        	g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
//        	g.Clear(System.Drawing.Color.White);
        		
	        	List<string> rv = new List<string>();
	
	            foreach (ArrayList Path in Fig)
	                foreach(Potrace.Curve[] Curves in Path)
	            		rv.AddRange(GetPathGC(Curves, lOn, lOff, oX * scale, oY * scale, scale));
	
//	            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
//	            bmp.Save("preview.bmp");
	            return rv;
        }

        private static List<string> GetPathGC(Potrace.Curve[] Curves, string lOn, string lOff, double oX, double oY, double scale)
        {
           
        	List<string> rv = new List<string>();
            
            for(int i=0;i<Curves.Length;i++)
            {
                Potrace.Curve Curve = Curves[i];
               
                if (i == 0)
                {
                	//fast go to position
                	rv.Add(String.Format("G0 X{0} Y{1}", formatnumber(Curve.A.X + oX, scale), formatnumber(Curve.A.Y + oY, scale)));
                	
                	//turn on laser
                    rv.Add(lOn);
                }

                if (Curve.Kind == Potrace.CurveKind.Bezier)
                {
                	CubicBezier cb = new CubicBezier(new Vector2((float)Curve.A.X, (float)Curve.A.Y),
                	                                 new Vector2((float)Curve.ControlPointA.X, (float)Curve.ControlPointA.Y),
                	                                 new Vector2((float)Curve.ControlPointB.X, (float)Curve.ControlPointB.Y),
                	                                 new Vector2((float)Curve.B.X, (float)Curve.B.Y));
                	
//                	g.DrawBezier(Pens.Green, 
//                	             	AsPointF(cb.P1),
//                	             	AsPointF(cb.C1),
//                	             	AsPointF(cb.C2),
//                	             	AsPointF(cb.P2));
                	
                	List<BiArc> bal = Algorithm.ApproxCubicBezier(cb, 5, 1);
                	
                	foreach (BiArc ba in bal)
		            { 
                		rv.Add(GetArcGC(ba.A1, oX, oY, scale));
                		rv.Add(GetArcGC(ba.A2, oX, oY, scale));
		            }    

                }
                else if (Curve.Kind == Potrace.CurveKind.Line)
                {
                	//trace line
                	rv.Add(String.Format("G1 X{0} Y{1}", formatnumber(Curve.B.X + oX, scale), formatnumber(Curve.B.Y + oY, scale)));
                	//g.DrawLine(Pens.DarkGray, (float)Curve.A.X, (float)Curve.A.Y, (float)Curve.B.X, (float)Curve.B.Y);
                }
                
                if (i == Curves.Length - 1)
                {
                	//turn off laser
                    rv.Add(lOff);
                }

               
             
            }

            

            return rv;
        }

        private static string GetArcGC(Arc arc, double oX, double oY, double scale)
        {
        	//http://www.cnccookbook.com/CCCNCGCodeArcsG02G03.htm
        	//https://www.tormach.com/g02_g03.html
        	
//	        g.DrawArc(Pens.Red,
//            arc.C.X - arc.r, arc.C.Y - arc.r, 2 * arc.r, 2 * arc.r, 
//            arc.startAngle * 180.0f / (float)Math.PI, arc.sweepAngle * 180.0f / (float)Math.PI);


			//cw-ccw work inverted cause of my coord system has Y axes reversed
			return String.Format("G{0} X{1} Y{2} I{3} J{4}", !arc.IsClockwise ? 2 : 3, formatnumber(arc.P2.X + oX, scale), formatnumber(arc.P2.Y + oY, scale), formatnumber(arc.C.X - arc.P1.X, scale), formatnumber(arc.C.Y - arc.P1.Y, scale));
        }
		
        private static string formatnumber(double number, double scale)
        { return (number/scale).ToString("0.###", System.Globalization.CultureInfo.InvariantCulture); }
		
        public static PointF AsPointF(Vector2 v)
        {
            return new PointF(v.X, v.Y);
        }
        
	}
}
