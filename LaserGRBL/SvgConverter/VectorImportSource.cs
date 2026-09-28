//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Point = System.Windows.Point;

namespace LaserGRBL.SvgConverter
{
	/// <summary>
	/// A vector file imported with the color layers dialog: what the dialog needs to show it and to create the gcode
	/// </summary>
	public abstract class VectorImportSource
	{
		/// <summary>
		/// One layer for each color, in order of first appearance
		/// </summary>
		public abstract List<SvgColorLayer> ScanLayers();

		/// <summary>
		/// The shapes of each layer as polylines in mm, in the same position of the gcode (preview and filling)
		/// </summary>
		public abstract Dictionary<string, List<List<Point>>> CaptureLayers(GrblCore core, List<SvgColorLayer> layers);

		public abstract string CreateGCode(GrblCore core, List<SvgColorLayer> layers);
	}

	/// <summary>
	/// Svg file, converted by GCodeFromSVG
	/// </summary>
	public class SvgImportSource : VectorImportSource
	{
		private XElement mSvg;

		public SvgImportSource(XElement svg)
		{ mSvg = svg; }

		private static GCodeFromSVG CreateConverter()
		{
			GCodeFromSVG converter = new GCodeFromSVG();
			converter.GCodeXYFeed = Settings.GetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", 1000);
			converter.UseLegacyBezier = !Settings.GetObject("Vector.UseSmartBezier", true);
			return converter;
		}

		public override List<SvgColorLayer> ScanLayers()
		{ return SvgColorLayer.Scan(mSvg); }

		public override Dictionary<string, List<List<Point>>> CaptureLayers(GrblCore core, List<SvgColorLayer> layers)
		{ return CreateConverter().CaptureLayers(mSvg, core, layers); }

		public override string CreateGCode(GrblCore core, List<SvgColorLayer> layers)
		{ return CreateConverter().convertFromXml(mSvg, core, layers); }
	}

	/// <summary>
	/// Dxf file, read as lines and true arcs: no intermediate format, the arcs become G2/G3
	/// </summary>
	public class DxfImportSource : VectorImportSource
	{
		private VectorDrawing mDrawing;

		public DxfImportSource(VectorDrawing drawing)
		{ mDrawing = drawing; }

		public override List<SvgColorLayer> ScanLayers()
		{
			List<SvgColorLayer> rv = new List<SvgColorLayer>();
			foreach (string color in mDrawing.Colors())
			{
				SvgColorLayer layer = new SvgColorLayer(color);
				layer.ElementCount = mDrawing.Paths.Count(p => p.Color == color);
				rv.Add(layer);
			}
			return rv;
		}

		public override Dictionary<string, List<List<Point>>> CaptureLayers(GrblCore core, List<SvgColorLayer> layers)
		{
			Dictionary<string, List<List<Point>>> rv = new Dictionary<string, List<List<Point>>>();
			foreach (SvgColorLayer layer in layers)
				rv[layer.Color] = mDrawing.Paths.Where(p => p.Color == layer.Color).Select(p => p.ToPolyline(VectorGCode.FillTolerance)).ToList();
			return rv;
		}

		public override string CreateGCode(GrblCore core, List<SvgColorLayer> layers)
		{ return VectorGCode.Create(mDrawing, core, layers); }
	}
}
