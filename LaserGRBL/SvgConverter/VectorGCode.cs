//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Point = System.Windows.Point;

namespace LaserGRBL.SvgConverter
{
	/// <summary>
	/// Create the gcode of a vector drawing layer by layer, like GCodeFromSVG does for the svg files,
	/// but emitting the arcs as G2/G3 instead of short lines
	/// </summary>
	public class VectorGCode
	{
		public const double FillTolerance = 0.01;   // mm, arcs approximation used to compute the filling
		private const double MinArcLength = 0.05;   // mm, shorter arcs are emitted as lines
		private const double MinArcSagitta = 0.001; // mm, arcs closer than this to their chord (the gcode precision) are lines

		private StringBuilder mGCode = new StringBuilder();
		private bool mPenDown;

		public static string Create(VectorDrawing drawing, GrblCore core, List<SvgColorLayer> layers)
		{
			return new VectorGCode().Convert(drawing, core, layers);
		}

		private string Convert(VectorDrawing drawing, GrblCore core, List<SvgColorLayer> layers)
		{
			gcode.setup(core);
			gcode.setRapidNum(Settings.GetObject("Disable G0 fast skip", false) ? 1 : 0);
			gcode.PutInitialCommand(mGCode);

			// stable sort: keep user order, but fill first and cut layers always last
			var ordered = layers.Where(l => l.Mode != SvgLayerMode.Ignore).Select((l, i) => new { l, i }).OrderBy(x => x.l.ExecutionOrder).ThenBy(x => x.i).Select(x => x.l);
			foreach (SvgColorLayer layer in ordered)
			{
				List<VectorPath> paths = drawing.Paths.Where(p => p.Color == layer.Color).ToList();
				List<List<Point>> filling = layer.HasFill ? SvgFilling.Build(paths.Select(p => p.ToPolyline(FillTolerance)).ToList(), layer.FillDirection, layer.LinesPerMM) : null;

				for (int pass = 1; pass <= layer.Passes; pass++)
				{
					mGCode.AppendFormat("(Layer {0} {1} pass {2}/{3})\r\n", layer.Color, layer.Mode, pass, layer.Passes);
					gcode.SetLayerParams(mGCode, layer.Speed, layer.Power);
					if (filling != null)
						foreach (List<Point> polyline in filling)
							EmitPolyline(polyline);
					if (layer.HasOutline)
						foreach (VectorPath path in paths)
							EmitPath(path);
					PenUp();
				}
			}

			gcode.PutFinalCommand(mGCode);
			return mGCode.Replace(',', '.').ToString();
		}

		private void EmitPolyline(List<Point> polyline)
		{
			PenUp();
			gcode.MoveToRapid(mGCode, polyline[0]);
			for (int i = 1; i < polyline.Count; i++)
			{
				PenDown();
				gcode.MoveTo(mGCode, polyline[i]);
			}
		}

		private void EmitPath(VectorPath path)
		{
			PenUp();
			gcode.MoveToRapid(mGCode, path.Start);
			Point current = path.Start;
			foreach (VectorSegment s in path.Segments)
			{
				PenDown();
				if (IsTrueArc(s, current))
					gcode.Arc(mGCode, s.CCW ? 3 : 2, s.End, new Point(s.Center.X - current.X, s.Center.Y - current.Y));
				else
					gcode.MoveTo(mGCode, s.End);
				current = s.End;
			}
		}

		// almost straight arcs have huge radii that the controller may reject: a line is as precise
		private static bool IsTrueArc(VectorSegment s, Point start)
		{
			if (!s.IsArc)
				return false;
			double sweep = Math.Abs(s.Sweep(start)), r = s.Radius;
			double sagitta = sweep >= Math.PI ? r : r * (1 - Math.Cos(sweep / 2));
			return sweep * r >= MinArcLength && sagitta >= MinArcSagitta;
		}

		private void PenUp()
		{
			if (mPenDown)
				gcode.PenUp(mGCode);
			mPenDown = false;
		}

		private void PenDown()
		{
			if (!mPenDown)
				gcode.PenDown(mGCode);
			mPenDown = true;
		}
	}
}
