﻿//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using CsPotrace.BezierToBiarc;
using System.Drawing;
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
		/// <param name="list">Arraylist, which contains vectorinformations about the Curves</param>
		/// <param name="Width">Width of the exportd cvg-File</param>
		/// <param name="Height">Height of the exportd cvg-File</param>
		/// <returns></returns>
		public static List<string> Export2GCode(List<List<Curve>> list, float oX, float oY, double scale, string lOn, string lOff, Size originalImageSize)
		{
			bool debug = false;

			Bitmap bmp = null;
			Graphics g = null;

			if (debug)
			{
				bmp = new Bitmap(originalImageSize.Width, originalImageSize.Height);
				g = Graphics.FromImage(bmp);
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.Clear(Color.White);
			}

			List<string> rv = new List<string>();

			list.Reverse();
			foreach (List<Curve> Curves in list)
				rv.AddRange(GetPathGC(Curves, lOn, lOff, oX * scale, oY * scale, scale, g));

			if (debug)
			{
				bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
				bmp.Save("preview.png");
				g.Dispose();
				bmp.Dispose();
			}
			return rv;
		}

		private static List<string> GetPathGC(List<Curve> Curves, string lOn, string lOff, double oX, double oY, double scale, Graphics g)
		{
			List<string> rv = new List<string>();

			OnPathBegin(Curves, lOn, oX, oY, scale, rv);

			foreach (Curve Curve in Curves)
				OnPathSegment(Curve, oX, oY, scale, rv, g);

			OnPathEnd(Curves, lOff, oX, oY, scale, rv);

			return rv;
		}

		private static void OnPathSegment(Curve Curve, double oX, double oY, double scale, List<string> rv, Graphics g)
		{
			if (double.IsNaN(Curve.LinearLenght)) // problem?
				return;

			if (Curve.Kind == CurveKind.Line)
			{
				//trace line
				if (g != null)
					g.DrawLine(Pens.DarkGray, (float)Curve.A.X, (float)Curve.A.Y, (float)Curve.B.X, (float)Curve.B.Y);

				rv.Add(string.Format("G1 X{0} Y{1}", formatnumber(Curve.B.X + oX, scale), formatnumber(Curve.B.Y + oY, scale)));
			}

			if (Curve.Kind == CurveKind.Bezier)
			{
				CubicBezier cb = new CubicBezier(new Vector2((float)Curve.A.X, (float)Curve.A.Y),
												 new Vector2((float)Curve.ControlPointA.X, (float)Curve.ControlPointA.Y),
												 new Vector2((float)Curve.ControlPointB.X, (float)Curve.ControlPointB.Y),
												 new Vector2((float)Curve.B.X, (float)Curve.B.Y));

				if (g != null) g.DrawBezier(Pens.Green,
								AsPointF(cb.P1),
								AsPointF(cb.C1),
								AsPointF(cb.C2),
								AsPointF(cb.P2));

				try
				{
					List<BiArc> bal = Algorithm.ApproxCubicBezier(cb, 5, 1);
					foreach (BiArc ba in bal)
					{
						if (!double.IsNaN(ba.A1.Length) && !double.IsNaN(ba.A1.LinearLength))
							rv.Add(GetArcGC(ba.A1, oX, oY, scale, g));

						if (!double.IsNaN(ba.A2.Length) && !double.IsNaN(ba.A2.LinearLength))
							rv.Add(GetArcGC(ba.A2, oX, oY, scale, g));
					}
				}
				catch
				{
					if (g != null) g.DrawLine(Pens.DarkGray, (float)Curve.A.X, (float)Curve.A.Y, (float)Curve.B.X, (float)Curve.B.Y);
					rv.Add(string.Format("G1 X{0} Y{1}", formatnumber(Curve.B.X + oX, scale), formatnumber(Curve.B.Y + oY, scale)));
				}

			}
		}

		private static void OnPathBegin(List<CsPotrace.Curve> Curves, string lOn, double oX, double oY, double scale, List<string> rv)
		{
			if (Curves.Count > 0)
			{
				//fast go to position
				rv.Add(String.Format("G0 X{0} Y{1}", formatnumber(Curves[0].A.X + oX, scale), formatnumber(Curves[0].A.Y + oY, scale)));
				//turn on laser
				rv.Add(lOn);
			}
		}

		private static void OnPathEnd(List<CsPotrace.Curve> Curves, string lOff, double oX, double oY, double scale, List<string> rv)
		{
			//turn off laser
			if (Curves.Count > 0)
				rv.Add(lOff);
		}

		private static string GetArcGC(Arc arc, double oX, double oY, double scale, Graphics g)
		{
			//http://www.cnccookbook.com/CCCNCGCodeArcsG02G03.htm
			//https://www.tormach.com/g02_g03.html

			if (arc.LinearLength > 2) //if not a small arc
			{
				if (g != null) g.DrawArc(Pens.Red, arc.C.X - arc.r, arc.C.Y - arc.r, 2 * arc.r, 2 * arc.r, arc.startAngle * 180.0f / (float)Math.PI, arc.sweepAngle * 180.0f / (float)Math.PI);
				return String.Format("G{0} X{1} Y{2} I{3} J{4}", !arc.IsClockwise ? 2 : 3, formatnumber(arc.P2.X + oX, scale), formatnumber(arc.P2.Y + oY, scale), formatnumber(arc.C.X - arc.P1.X, scale), formatnumber(arc.C.Y - arc.P1.Y, scale));
			}
			else //approximate with a line
			{
				if (g != null) g.DrawLine(Pens.DarkGray, (float)arc.P1.X, (float)arc.P1.Y, (float)arc.P2.X, (float)arc.P2.Y);
				return String.Format("G1 X{0} Y{1}", formatnumber(arc.P2.X + oX, scale), formatnumber(arc.P2.Y + oY, scale));
			}


		}

	}
}
