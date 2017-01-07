using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;
using System.Collections;
using CsPotrace;
using System.Drawing.Drawing2D;
using System.IO;

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
            {

                bool isContour = true;

                for(int i=0;i< Path.Count;i++)
                {
                    Potrace.Curve[] Curves = (Potrace.Curve[])Path[i];
                    rv.AddRange(isContour ? GetContourPathGC(Curves, lOn, lOff) : GetHolePathGC(Curves, lOn, lOff));
                    isContour = false;
                
                }
            }

            return rv;
        }

        private static List<string> GetContourPathGC(Potrace.Curve[] Curves, string lOn, string lOff)
        {
           
        	List<string> rv = new List<string>();
            
            for(int i=0;i<Curves.Length;i++)
            {
                Potrace.Curve Curve = Curves[i];

                
                if (i == 0)
                {
                	//fast go to position
                	rv.Add(String.Format("G0 X{0} Y{1}", Curve.A.X.ToString("0.0", enUsCulture), Curve.A.Y.ToString("0.0", enUsCulture)));
                	//turn on laser
                    rv.Add(lOn);
                }

                if (Curve.Kind == Potrace.CurveKind.Bezier)
                {

                    //rv.Add("C" + Curve.ControlPointA.X.ToString("0.0", enUsCulture) + " " + Curve.ControlPointA.Y.ToString("0.0", enUsCulture) + " " +
                    //                Curve.ControlPointB.X.ToString("0.0", enUsCulture) + " " + Curve.ControlPointB.Y.ToString("0.0", enUsCulture) + " " +
                    //                Curve.B.X.ToString("0.0", enUsCulture) + " " + Curve.B.Y.ToString("0.0", enUsCulture));
                }
                else if (Curve.Kind == Potrace.CurveKind.Line)
                {
                	//trace line
                	rv.Add(String.Format("G1 X{0} Y{1}", Curve.B.X.ToString("0.0", enUsCulture), Curve.B.Y.ToString("0.0", enUsCulture)));
                }
                
                if (i == Curves.Length - 1)
                {
                	//turn off laser
                    rv.Add(lOff);
                }

               
             
            }

            

            return rv;
        }

        private static List<string> GetHolePathGC(Potrace.Curve[] Curves, string lOn, string lOff)
        {
        	List<string> rv = new List<string>();
           
            for(int i=0;i<Curves.Length;i++)//for (int i = Curves.Length-1; i >=0 ; i--)
            {
                Potrace.Curve Curve = Curves[i];


               if (i == 0)
                {
                	//fast go to position
                	rv.Add(String.Format("G0 X{0} Y{1}", Curve.A.X.ToString("0.0", enUsCulture), Curve.A.Y.ToString("0.0", enUsCulture)));
                	//turn on laser
                    rv.Add(lOn);
                }

                if (Curve.Kind == Potrace.CurveKind.Bezier)
                {

                    //rv.Add("C" + Curve.ControlPointA.X.ToString("0.0", enUsCulture) + " " + Curve.ControlPointA.Y.ToString("0.0", enUsCulture) + " " +
                    //                Curve.ControlPointB.X.ToString("0.0", enUsCulture) + " " + Curve.ControlPointB.Y.ToString("0.0", enUsCulture) + " " +
                    //                Curve.B.X.ToString("0.0", enUsCulture) + " " + Curve.B.Y.ToString("0.0", enUsCulture));
                }
                else if (Curve.Kind == Potrace.CurveKind.Line)
                {
                	//trace line
                	rv.Add(String.Format("G1 X{0} Y{1}", Curve.B.X.ToString("0.0", enUsCulture), Curve.B.Y.ToString("0.0", enUsCulture)));
                }
                
                if (i == Curves.Length - 1)
                {
                	//turn off laser
                    rv.Add(lOff);
                }

            }

           

            return rv;
        }
		
		
	}
}
