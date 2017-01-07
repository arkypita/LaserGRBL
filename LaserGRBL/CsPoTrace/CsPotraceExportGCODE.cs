using System;
using System.Collections.Generic;
using CsPotrace.BezierToBiarc;
using System.Collections;
using CsPotrace;
using System.Numerics;

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
        public static List<string> Export2GCode(ArrayList Fig, int Width, int Height, string lOn, string lOff)
        {
        	List<string> rv = new List<string>();

            foreach (ArrayList Path in Fig)
                foreach(Potrace.Curve[] Curves in Path)
                    rv.AddRange(GetPathGC(Curves, lOn, lOff));

            return rv;
        }

        private static List<string> GetPathGC(Potrace.Curve[] Curves, string lOn, string lOff)
        {
           
        	List<string> rv = new List<string>();
            
            for(int i=0;i<Curves.Length;i++)
            {
                Potrace.Curve Curve = Curves[i];

                
                if (i == 0)
                {
                	//fast go to position
                	rv.Add(String.Format("G0 X{0} Y{1}", formatnumber(Curve.A.X), formatnumber(Curve.A.Y)));
                	//turn on laser
                    rv.Add(lOn);
                }

                if (Curve.Kind == Potrace.CurveKind.Bezier)
                {
                	CubicBezier cb = new CubicBezier(new Vector2((float)Curve.A.X, (float)Curve.A.Y),
                	                                 new Vector2((float)Curve.ControlPointA.X, (float)Curve.ControlPointA.Y),
                	                                 new Vector2((float)Curve.ControlPointB.X, (float)Curve.ControlPointB.Y),
                	                                 new Vector2((float)Curve.B.X, (float)Curve.B.Y));
                	
                	List<BiArc> bal = Algorithm.ApproxCubicBezier(cb, 5, 1);
                	
                	foreach (BiArc ba in bal)
		            { 
                		rv.Add(GetArcGC(ba.A1));
                		rv.Add(GetArcGC(ba.A2));
                		
//		                g.DrawArc(new Pen(Color.Red, 1),
//		                    ba.A1.C.X - ba.A1.r, ba.A1.C.Y - ba.A1.r, 2 * ba.A1.r, 2 * ba.A1.r, 
//		                    ba.A1.startAngle * 180.0f / (float)Math.PI, ba.A1.sweepAngle * 180.0f / (float)Math.PI);
//		                g.DrawArc(new Pen(Color.Red, 1),
//		                    ba.A2.C.X - ba.A2.r, ba.A2.C.Y - ba.A2.r, 2 * ba.A2.r, 2 * ba.A2.r, 
//		                    ba.A2.startAngle * 180.0f / (float)Math.PI, ba.A2.sweepAngle * 180.0f / (float)Math.PI);
		            }    

                }
                else if (Curve.Kind == Potrace.CurveKind.Line)
                {
                	//trace line
                	rv.Add(String.Format("G1 X{0} Y{1}", formatnumber(Curve.B.X), formatnumber(Curve.B.Y)));
                }
                
                if (i == Curves.Length - 1)
                {
                	//turn off laser
                    rv.Add(lOff);
                }

               
             
            }

            

            return rv;
        }

        private static string GetArcGC(Arc arc)
        {
        	//http://www.cnccookbook.com/CCCNCGCodeArcsG02G03.htm
        	//https://www.tormach.com/g02_g03.html
        	
        	return String.Format("G1 X{0} Y{1}", formatnumber(arc.P2.X), formatnumber(arc.P2.Y));
        	
        	//if (!arc.IsClockwise)
        	//	return String.Format("G2 X{0} Y{1} I{2} J{3}", formatnumber(arc.P1.X), formatnumber(arc.P1.Y), formatnumber(arc.C.X - arc.P1.X), formatnumber(arc.C.Y - arc.P1.Y));
        	//else
        	//    return String.Format("G3 X{0} Y{1} I{2} J{3}", formatnumber(arc.P1.X), formatnumber(arc.P1.Y), formatnumber(arc.P1.X - arc.C.X), formatnumber(arc.P1.Y - arc.C.Y));
        }
		
        private static string formatnumber(double number)
		{ return number.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture); }
		
	}
}
