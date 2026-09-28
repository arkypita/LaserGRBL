//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace LaserGRBL.SvgConverter
{
	/// <summary>
	/// How the elements of a color layer are processed
	/// </summary>
	public enum SvgLayerMode
	{
		Line,        // engrave the outline (low power)
		Fill,        // engrave the area inside the shapes with a hatch pattern
		FillAndLine, // fill, then engrave the outline for a clean edge
		Cut,         // cut through the outline (high power, multiple passes), always done last
		Ignore       // skip this color
	}

	/// <summary>
	/// A group of SVG elements sharing the same color, with its own laser parameters (like LightBurn layers)
	/// </summary>
	public class SvgColorLayer
	{
		public string Color { get; private set; }  // normalized "#RRGGBB"
		public int ElementCount { get; set; }
		public SvgLayerMode Mode { get; set; }
		public int Speed { get; set; }
		public int Power { get; set; }
		public int Passes { get; set; }
		public RasterConverter.ImageProcessor.Direction FillDirection { get; set; }
		public double LinesPerMM { get; set; }

		public SvgColorLayer(string color)
		{
			Color = color;
			Mode = SvgLayerMode.Line;
			Passes = 1;
			FillDirection = RasterConverter.ImageProcessor.Direction.NewHorizontal;
			LinesPerMM = 10;
		}

		public bool HasFill
		{ get { return Mode == SvgLayerMode.Fill || Mode == SvgLayerMode.FillAndLine; } }

		public bool HasOutline
		{ get { return Mode == SvgLayerMode.Line || Mode == SvgLayerMode.FillAndLine || Mode == SvgLayerMode.Cut; } }

		public Color DrawingColor
		{ get { return ColorTranslator.FromHtml(Color); } }

		// the order in which layers are executed: fill first, then lines, cut must be last otherwise the piece may move before the engraving is done
		public int ExecutionOrder
		{
			get
			{
				switch (Mode)
				{
					case SvgLayerMode.Fill:
					case SvgLayerMode.FillAndLine: return 0;
					case SvgLayerMode.Line: return 1;
					default: return 2;
				}
			}
		}

		#region Settings persistence

		private string SettingKey
		{ get { return "SvgColorLayer." + Color.TrimStart('#'); } }

		public bool LoadSettings()
		{
			string value = Settings.GetObject<string>(SettingKey, null);
			if (value == null)
				return false;

			try
			{
				string[] parts = value.Split('|');
				Mode = (SvgLayerMode)Enum.Parse(typeof(SvgLayerMode), parts[0]);
				Speed = int.Parse(parts[1], CultureInfo.InvariantCulture);
				Power = int.Parse(parts[2], CultureInfo.InvariantCulture);
				Passes = Math.Max(1, int.Parse(parts[3], CultureInfo.InvariantCulture));
				if (parts.Length >= 6) //fill parameters added later
				{
					FillDirection = (RasterConverter.ImageProcessor.Direction)Enum.Parse(typeof(RasterConverter.ImageProcessor.Direction), parts[4]);
					LinesPerMM = double.Parse(parts[5], CultureInfo.InvariantCulture);
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public void SaveSettings()
		{
			Settings.SetObject(SettingKey, string.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}|{4}|{5}", Mode, Speed, Power, Passes, FillDirection, LinesPerMM));
		}

		#endregion

		#region Color detection

		public const string DefaultColor = "#000000"; // SVG default fill is black

		// elements converted by GCodeFromSVG (text and image are not supported)
		private static readonly string[] ConvertedElements = { "path", "rect", "circle", "ellipse", "line", "polyline", "polygon" };

		/// <summary>
		/// Scan the svg and return one layer for each color found, in order of first appearance
		/// </summary>
		public static List<SvgColorLayer> Scan(XElement svgRoot)
		{
			Dictionary<string, SvgColorLayer> layers = new Dictionary<string, SvgColorLayer>();
			List<SvgColorLayer> rv = new List<SvgColorLayer>();
			ScanContainer(svgRoot, svgRoot.Name.Namespace, layers, rv);
			return rv;
		}

		// walk the same elements visited by GCodeFromSVG: root and nested groups
		private static void ScanContainer(XElement container, XNamespace ns, Dictionary<string, SvgColorLayer> layers, List<SvgColorLayer> rv)
		{
			foreach (XElement element in container.Elements())
			{
				if (element.Name == ns + "g")
				{
					ScanContainer(element, ns, layers, rv);
				}
				else if (element.Name.Namespace == ns && ConvertedElements.Contains(element.Name.LocalName))
				{
					string color = ResolveColor(element);
					SvgColorLayer layer;
					if (!layers.TryGetValue(color, out layer))
					{
						layer = new SvgColorLayer(color);
						layers.Add(color, layer);
						rv.Add(layer);
					}
					layer.ElementCount++;
				}
			}
		}

		/// <summary>
		/// Return the color used to group the element: stroke color if any, fill color otherwise (inherited from parent groups)
		/// </summary>
		public static string ResolveColor(XElement element)
		{
			string color = NormalizeColor(GetInheritedProperty(element, "stroke"));
			if (color == null)
				color = NormalizeColor(GetInheritedProperty(element, "fill"));
			return color ?? DefaultColor;
		}

		// value from style attribute or presentation attribute, searching in the element and its ancestors
		private static string GetInheritedProperty(XElement element, string name)
		{
			for (XElement current = element; current != null; current = current.Parent)
			{
				string value = GetProperty(current, name);
				if (value != null && value != "inherit")
					return value;
			}
			return null;
		}

		private static string GetProperty(XElement element, string name)
		{
			XAttribute style = element.Attribute("style");
			if (style != null)
			{
				foreach (string declaration in style.Value.Split(';'))
				{
					int sep = declaration.IndexOf(':');
					if (sep > 0 && declaration.Substring(0, sep).Trim() == name)
						return declaration.Substring(sep + 1).Replace("!important", "").Trim();
				}
			}

			XAttribute attribute = element.Attribute(name);
			return attribute != null ? attribute.Value.Trim() : null;
		}

		private static readonly Regex RgbFunction = new Regex(@"^rgba?\(\s*([\d.]+)(%?)\s*[,\s]\s*([\d.]+)(%?)\s*[,\s]\s*([\d.]+)(%?)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		/// <summary>
		/// Convert an svg color value to "#RRGGBB", null for "none" or unsupported values (gradients, currentColor...)
		/// </summary>
		public static string NormalizeColor(string value)
		{
			if (string.IsNullOrEmpty(value) || value == "none" || value.StartsWith("url(") || value == "currentColor")
				return null;

			try
			{
				Color c;
				Match m = RgbFunction.Match(value);
				if (m.Success)
					c = System.Drawing.Color.FromArgb(RgbComponent(m.Groups[1].Value, m.Groups[2].Value), RgbComponent(m.Groups[3].Value, m.Groups[4].Value), RgbComponent(m.Groups[5].Value, m.Groups[6].Value));
				else if (value.StartsWith("#") && value.Length == 4) // short hex form #RGB
					c = ColorTranslator.FromHtml(string.Format("#{0}{0}{1}{1}{2}{2}", value[1], value[2], value[3]));
				else
					c = ColorTranslator.FromHtml(value);

				if (c.IsEmpty)
					return null;

				return string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
			}
			catch
			{
				return null;
			}
		}

		private static int RgbComponent(string number, string percent)
		{
			double v = double.Parse(number, CultureInfo.InvariantCulture);
			if (percent == "%")
				v = v * 255 / 100;
			return (int)Math.Max(0, Math.Min(255, Math.Round(v)));
		}

		#endregion
	}
}
