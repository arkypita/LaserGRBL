/*  
	This code comes from GRBL-Plotter
	Copyright (C) 2015-2018 Sven Hasemann contact: svenhb@web.de
	Modified for LaserGRBL by Diego Settimi contact: arkypita@bergamo3.it

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
*/

using System;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

namespace LaserGRBL.SvgConverter
{
	class GCodeFromSVG
	{
		private static float factor_In2Px = 96;
		private static float factor_Mm2Px = 96f / 25.4f;
		private static float factor_Cm2Px = 96f / 2.54f;
		private static float factor_Pt2Px = 96f / 72f;
		private static float factor_Pc2Px = 12 * 96f / 72f;
		private static float factor_Em2Px = 150;

		private StringBuilder gcodeString = new StringBuilder();
		private int svgBezierAccuracy = 12;      // applied line segments at bezier curves
		private bool svgScaleApply = false;      // try to scale final GCode if true
		private float svgMaxSize = 100;          // final GCode size (greater dimension) if scale is applied
		private bool svgClosePathExtend = true;  // not working if true move to first and second point of path to get overlap

		private bool svgNodesOnly = false;        // if true only do pen-down -up on given coordinates

		private bool svgPauseElement = false;    // if true insert GCode pause M0 before each element
		private bool svgPausePenDown = false;    // if true insert pause M0 before pen down
		private bool svgComments = false;        // if true insert additional comments into GCode

		private bool gcodeReduce = false;        // if true remove G1 commands if distance is < limit
		private float gcodeReduceVal = 0.1f;     // limit when to remove G1 commands

		public System.Drawing.PointF UserOffset = new System.Drawing.PointF(0,0);
		public float GCodeXYFeed = 2000;        // XY feed to apply for G1
        private double scaledError = 1.0;

		public bool svgConvertToMM = true;
		private float gcodeScale = 1;                    // finally scale with this factor if svgScaleApply and svgMaxSize
		private Matrix[] matrixGroup = new Matrix[10];   // store SVG-Group transformation matrixes
		private Matrix matrixElement = new Matrix();     // store finally applied matrix
		private Matrix oldMatrixElement = new Matrix();     // store finally applied matrix

		/// <summary>
		/// Entrypoint for conversion: apply file-path or file-URL
		/// </summary>
		/// <param name="file">String keeping file-name or URL</param>
		/// <returns>String with GCode of imported data</returns>
		private XElement svgCode;
		private bool importInMM = false;
		//private bool fromText = false;

		Regex RemoveInvalidUnicode = new Regex(@"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-u10FFFF]+", RegexOptions.Compiled);
		public string convertFromText(string text, GrblCore core, bool importMM = false)
		{
			//fromText = true;
			importInMM = importMM;

			// From xml spec valid chars: 
			// #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF] 
			// any Unicode character, excluding the surrogate blocks, FFFE, and FFFF. 
			text = RemoveInvalidUnicode.Replace(text, string.Empty);
			svgCode = XElement.Parse(text, LoadOptions.None);

			return convertSVG(svgCode, core);
		}

		public string convertFromFile(string file, GrblCore core)
		{
			string xml = System.IO.File.ReadAllText(file);
			return convertFromText(xml, core, true);
		}

		private string convertSVG(XElement svgCode, GrblCore core)
		{
			gcode.setup(core);          // initialize GCode creation (get stored settings for export)
			gcode.setRapidNum(Settings.GetObject("Disable G0 fast skip", false) ? 1 : 0);

			gcode.PutInitialCommand(gcodeString);
			startConvert(svgCode);
			gcode.PutFinalCommand(gcodeString);

			return gcodeString.Replace(',', '.').ToString();
		}

		/// <summary>
		/// Set defaults and parse main element of SVG-XML
		/// </summary>
		private void startConvert(XElement svgCode)
		{
			countSubPath = 0;
			startFirstElement = true;
			gcodeScale = 1;
			svgWidthPx = 0; svgHeightPx = 0;
			currentX = 0; currentY = 0;
			offsetX = 0; offsetY = 0;
			firstX = null;
			firstY = null;
			lastX = 0;
			lastY = 0;

			matrixElement.SetIdentity();
			oldMatrixElement.SetIdentity();
			for (int i = 0; i < matrixGroup.Length; i++)
				matrixGroup[i].SetIdentity();

			parseGlobals(svgCode);
			if (!svgNodesOnly)
				parseBasicElements(svgCode, 1);
			parsePath(svgCode, 1);
			parseGroup(svgCode, 1);
			return;
		}

		/// <summary>
		/// Parse SVG dimension (viewbox, width, height)
		/// </summary>
		private XNamespace nspace = "http://www.w3.org/2000/svg";
		private void parseGlobals(XElement svgCode)
		{  
			// One px unit is defined to be equal to one user unit. Thus, a length of "5px" is the same as a length of "5".
			Matrix tmp = new Matrix(1, 0, 0, 1, 0, 0); // m11, m12, m21, m22, offsetx, offsety
			svgWidthPx = 0;
			svgHeightPx = 0;
			float vbOffX = 0;
			float vbOffY = 0;
			float vbWidth = 0;
			float vbHeight = 0;
			float scale = 1;
			string tmpString = "";

			if (svgCode.Attribute("xmlns") != null)
				nspace = svgCode.Attribute("xmlns").Value;
			else
				nspace = "";

			if (svgCode.Attribute("viewBox") != null)   // viewBox unit always in px
			{
				string viewbox = svgCode.Attribute("viewBox").Value;
				viewbox = Regex.Replace(viewbox, @"\s+", " ").Replace(' ', '|');    // remove double space
				var split = viewbox.Split('|');
				vbOffX = -convertToPixel(split[0]);
				vbOffY = -convertToPixel(split[1]);
				vbWidth = convertToPixel(split[2]);
				vbHeight = convertToPixel(split[3].TrimEnd(')'));
				tmp.M11 = 1; tmp.M22 = -1;      // flip Y
				tmp.OffsetY = vbHeight;
			}

			scale = 1;
			if (svgConvertToMM)                 // convert back from px to mm 
				scale = (1 / factor_Mm2Px);
			else                                // convert back from px to inch
				scale = (1 / factor_In2Px);

			//if (unitIsPixel)                    // got svg-text from clipboard perhaps from maker.js
			//	scale = 1;

			if (svgCode.Attribute("width") != null)
			{
				tmpString = svgCode.Attribute("width").Value;
				svgWidthPx = convertToPixel(tmpString);             // convert in px

				tmp.M11 = scale; // get desired scale
								 //if (fromClipboard)
								 //    tmp.M11 = 1 / factor_Mm2Px; // 3.543307;         // https://www.w3.org/TR/SVG/coords.html#Units
				if (vbWidth > 0)
				{
					scaledError = scale * svgWidthPx / vbWidth;
					tmp.M11 = scaledError;
					tmp.OffsetX = vbOffX * scale;   // svgWidthUnit / vbWidth;
				}
			}

			if (svgCode.Attribute("height") != null)
			{
				tmpString = svgCode.Attribute("height").Value;
				svgHeightPx = convertToPixel(tmpString);

				tmp.M22 = -scale;   // get desired scale and flip vertical
				tmp.OffsetY = scale * svgHeightPx;  // svgHeightUnit;

				//if (fromClipboard)
				//{   tmp.M22 = -1 / factor_Mm2Px;// 3.543307;
				//    tmp.OffsetY = svgHeightPx / factor_Mm2Px; // 3.543307;     // https://www.w3.org/TR/SVG/coords.html#Units
				//}
				if (vbHeight > 0)
				{
					tmp.M22 = -scale * svgHeightPx / vbHeight;
					tmp.OffsetY = -vbOffY * svgHeightPx / vbHeight + (svgHeightPx * scale);
				}
			}

			float newWidth = Math.Max(svgWidthPx, vbWidth);     // use value from 'width' or 'viewbox' parameter
			float newHeight = Math.Max(svgHeightPx, vbHeight);
			if ((newWidth > 0) && (newHeight > 0))
			{
				if (SvgScaleApply)
				{
					gcodeScale = SvgMaxSize / Math.Max(newWidth, newHeight);        // calc. factor to get desired max. size
					tmp.Scale((double)gcodeScale, (double)gcodeScale);
					if (svgConvertToMM)                         // https://www.w3.org/TR/SVG/coords.html#Units
						tmp.Scale(factor_Mm2Px, factor_Mm2Px);  // 3.543307, 3.543307);
					else
						tmp.Scale(factor_In2Px, factor_In2Px);
				}
			}
			//else error! dimension not given

			tmp.OffsetX += UserOffset.X; //add user offset for centerline
			tmp.OffsetY += UserOffset.Y; //add user offset for centerline

			for (int i = 0; i < matrixGroup.Length; i++)
			{ matrixGroup[i] = tmp; }
			matrixElement = tmp;

			//var element = svgCode.Element(nspace + "title");
			////if (element != null)
			////    Plotter.DocTitle = element.Value;

			//var xtmp = svgCode.Element(nspace + "metadata");
			//if ((xtmp) != null)
			//{
			//	XNamespace ccspace = "http://creativecommons.org/ns#";
			//	XNamespace rdfspace = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
			//	XNamespace dcspace = "http://purl.org/dc/elements/1.1/";
			//	if ((xtmp = xtmp.Element(rdfspace + "RDF")) != null)
			//	{
			//		if ((xtmp = xtmp.Element(ccspace + "Work")) != null)
			//		{
			//			//if ((xtmp = xtmp.Element(dcspace + "description")) != null)
			//			//                      Plotter.DocDescription = xtmp.Value;
			//		}
			//	}
			//}
			return;
		}

		/// <summary>
		/// Parse Group-Element and included elements
		/// </summary>
		private void parseGroup(XElement svgCode, int level)
		{
			foreach (XElement groupElement in svgCode.Elements(nspace + "g"))
			{
				if (svgComments)
					if (groupElement.Attribute("id") != null)
						gcodeString.Append("\r\n( Group level:" + level.ToString() + " id=" + groupElement.Attribute("id").Value + " )\r\n");
				parseTransform(groupElement, true, level);   // transform will be applied in gcodeMove
				if (!svgNodesOnly)
					parseBasicElements(groupElement, level);
				parsePath(groupElement, level);
				parseGroup(groupElement, level + 1);
			}
			return;
		}

		/// <summary>
		/// Parse Transform information - more information here: http://www.w3.org/TR/SVG/coords.html
		/// transform will be applied in gcodeMove
		/// </summary>
		private bool parseTransform(XElement element, bool isGroup, int level)
		{
			Matrix tmp = new Matrix(1, 0, 0, 1, 0, 0); // m11, m12, m21, m22, offsetx, offsety
			bool transf = false;
			if (element.Attribute("transform") != null)
			{
				transf = true;
				string tstring = element.Attribute("transform").Value;
				if (!string.IsNullOrEmpty(tstring))
				{
					System.Collections.Generic.List<string> tlist = new System.Collections.Generic.List<string>( Regex.Split(tstring, @"(?<=[\)])"));
					tlist.Reverse();
					foreach (string tr in tlist)
					{
						if (tr != null && tr.Trim().Length > 0)
						{
							if (tr.IndexOf("translate") >= 0)
							{
								var coord = getTextBetween(tr, "translate(", ")");
								var split = coord.Split(',');
								if (coord.IndexOf(',') < 0)
									split = coord.Split(' ');

								float ox = floatParse(split[0]);
								float oy = (split.Length > 1) ? floatParse(split[1].TrimEnd(')')) : 0.0f;
								tmp.Translate(ox, oy);

								if (svgComments) gcodeString.Append(string.Format("( SVG-Translate {0} {1} )\r\n", ox, oy));
							}
							else if (tr.IndexOf("scale") >= 0)
							{
								var coord = getTextBetween(tr, "scale(", ")");
								var split = coord.Split(',');
								if (coord.IndexOf(',') < 0)
									split = coord.Split(' ');
								tmp.M11 = floatParse(split[0]);
								if (split.Length > 1)
								{ tmp.M22 = floatParse(split[1]); }
								else
								{
									tmp.M11 = floatParse(coord);
									tmp.M22 = floatParse(coord);
								}
								if (svgComments) gcodeString.Append(string.Format("( SVG-Scale {0} {1} )\r\n", tmp.M11, tmp.M22));
							}
							else if (tr.IndexOf("rotate") >= 0)
							{
								var coord = getTextBetween(tr, "rotate(", ")");
								var split = coord.Split(',');
								if (coord.IndexOf(',') < 0)
									split = coord.Split(' ');

								//Original code from https://github.com/svenhb/GRBL-Plotter
								//float angle = floatParse(split[0]) * (float)Math.PI / 180;
								//tmp.OffsetX = px;
								//tmp.OffsetY = py;
								//tmp.M11 = Math.Cos(angle); tmp.M12 = Math.Sin(angle);
								//tmp.M21 = -Math.Sin(angle); tmp.M22 = Math.Cos(angle);

								//current code from https://github.com/arkypita/LaserGRBL now support rotation with offset
								float angle = floatParse(split[0]); //no need to convert in radiant
								float px = split.Length == 3 ? floatParse(split[1]) : 0.0f; //<--- this read rotation offset point x
								float py = split.Length == 3 ? floatParse(split[2]) : 0.0f; //<--- this read rotation offset point y
								tmp.RotateAt(angle, px, py); // <--- this apply RotateAt matrix

								if (svgComments) gcodeString.Append(string.Format("( SVG-Rotate {0} {1} {2} )\r\n", angle, px, py));
							}
							else if (tr.IndexOf("matrix") >= 0)
							{
								var coord = getTextBetween(tr, "matrix(", ")");
								var split = coord.Split(',');
								if (coord.IndexOf(',') < 0)
									split = coord.Split(' ');
								tmp.M11 = floatParse(split[0]);     // a    scale x         a c e
								tmp.M12 = floatParse(split[1]);     // b                    b d f
								tmp.M21 = floatParse(split[2]);     // c                    0 0 1
								tmp.M22 = floatParse(split[3]);     // d    scale y
								tmp.OffsetX = floatParse(split[4]); // e    offset x
								tmp.OffsetY = floatParse(split[5]); // f    offset y
								if (svgComments) gcodeString.Append(string.Format("\r\n( SVG-Matrix {0} {1} {2} )\r\n", coord.Replace(',', '|'), level, isGroup));
							}
						}
					}
				}



				if (isGroup)
				{
					matrixGroup[level].SetIdentity();
					if (level > 0)
					{
						for (int i = level; i < matrixGroup.Length; i++)
						{ matrixGroup[i] = Matrix.Multiply(tmp, matrixGroup[level - 1]); }
					}
					else
					{ matrixGroup[level] = tmp; }
					matrixElement = matrixGroup[level];
				}
				else
				{
					matrixElement = Matrix.Multiply(tmp, matrixGroup[level]);
				}

				if (svgComments && transf)
				{
					for (int i = 0; i <= level; i++)
						gcodeString.AppendFormat("( gc-Matrix level[{0}] {1} )\r\n", i, matrixGroup[i].ToString());

					if (svgComments) gcodeString.AppendFormat("( gc-Scale {0} {1} )\r\n", matrixElement.M11, matrixElement.M22);
					if (svgComments) gcodeString.AppendFormat("( gc-Offset {0} {1} )\r\n", matrixElement.OffsetX, matrixElement.OffsetY);
				}
			}
			return transf;
		}
		private string getTextBetween(string source, string s1, string s2)
		{
			int start = source.IndexOf(s1) + s1.Length;
			char c;
			for (int i = start; i < source.Length; i++)
			{
				c = source[i];
				if (!(Char.IsNumber(c) || c == '.' || c == ',' || c == ' ' || c == '-' || c == 'e'))    // also exponent
					return source.Substring(start, i - start);
			}
			return source.Substring(start, source.Length - start - 1);
		}

		private float convertToPixel(string str, float ext = 1) => floatParse(str, ext = 1);
		private float floatParse(string str, float ext = 1)
		{
			bool percent = false;
			float factor = 1;   // no unit = px
			if (str.IndexOf("mm") > 0) { factor = factor_Mm2Px; }               // Millimeter
			else if (str.IndexOf("cm") > 0) { factor = factor_Cm2Px; }          // Centimeter
			else if (str.IndexOf("in") > 0) { factor = factor_In2Px; }          // Inch    72, 90 or 96?
			else if (str.IndexOf("pt") > 0) { factor = factor_Pt2Px; }          // Point
			else if (str.IndexOf("pc") > 0) { factor = factor_Pc2Px; }          // Pica
			else if (str.IndexOf("em") > 0) { factor = factor_Em2Px; }          // Font size
			else if (str.IndexOf("%") > 0) { percent = true; }
			string nstr = removeUnit(str);
			str = nstr;
			float test;
			if (str.Length > 0)
			{
				if (percent)
				{
					if (float.TryParse(str, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out test))
					{ return (test * ext / 100); }
				}
				else
				{
					if (float.TryParse(str, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out test))
					{ return (test * factor); }
				}
			}

			//else error!
			return 0f;
		}

		private static string removeUnit(string str)
		{ return Regex.Replace(str, @"[^0-9.\-+]", ""); }

		private string getColor(XElement pathElement)
		{
			string style = "";
			string stroke_color = "000000";        // default=black
			if (pathElement.Attribute("style") != null)
			{
				int start, end;
				style = pathElement.Attribute("style").Value;
				start = style.IndexOf("stroke:#");
				if (start >= 0)
				{
					end = style.IndexOf(';', start);
					if (end > start)
						stroke_color = style.Substring(start + 8, end - start - 8);
				}
				return stroke_color;
			}
			return "";
		}

		/// <summary>
		/// Convert Basic shapes (up to now: line, rect, circle) check: http://www.w3.org/TR/SVG/shapes.html
		/// </summary>
		private void parseBasicElements(XElement svgCode, int level)
		{
			string[] forms = { "rect", "circle", "ellipse", "line", "polyline", "polygon", "text", "image" };
			foreach (var form in forms)
			{
				foreach (var pathElement in svgCode.Elements(nspace + form))
				{
					if (pathElement != null)
					{
						string myColor = getColor(pathElement);

						if (svgComments)
						{
							if (pathElement.Attribute("id") != null)
								gcodeString.Append("\r\n( Basic shape level:" + level.ToString() + " id=" + pathElement.Attribute("id").Value + " )\r\n");
							gcodeString.AppendFormat("( SVG color=#{0})\r\n", myColor);
						}

						if (startFirstElement)
						{ gcodePenUp("1st shape"); startFirstElement = false; }

						offsetX = 0; offsetY = 0;

						oldMatrixElement = matrixElement;
						bool avoidG23 = false;
						parseTransform(pathElement, false, level);  // transform will be applied in gcodeMove

						float x = 0, y = 0, x1 = 0, y1 = 0, x2 = 0, y2 = 0, width = 0, height = 0, rx = 0, ry = 0, cx = 0, cy = 0, r = 0;
						string[] points = { "" };
						if (pathElement.Attribute("x") != null) x = floatParse(pathElement.Attribute("x").Value);
						if (pathElement.Attribute("y") != null) y = floatParse(pathElement.Attribute("y").Value);
						if (pathElement.Attribute("x1") != null) x1 = floatParse(pathElement.Attribute("x1").Value);
						if (pathElement.Attribute("y1") != null) y1 = floatParse(pathElement.Attribute("y1").Value);
						if (pathElement.Attribute("x2") != null) x2 = floatParse(pathElement.Attribute("x2").Value);
						if (pathElement.Attribute("y2") != null) y2 = floatParse(pathElement.Attribute("y2").Value);
						if (pathElement.Attribute("width") != null) width = floatParse(pathElement.Attribute("width").Value, svgWidthPx);
						if (pathElement.Attribute("height") != null) height = floatParse(pathElement.Attribute("height").Value, svgHeightPx);
						if (pathElement.Attribute("rx") != null) rx = floatParse(pathElement.Attribute("rx").Value);
						if (pathElement.Attribute("ry") != null) ry = floatParse(pathElement.Attribute("ry").Value);
						if (pathElement.Attribute("cx") != null) cx = floatParse(pathElement.Attribute("cx").Value);
						if (pathElement.Attribute("cy") != null) cy = floatParse(pathElement.Attribute("cy").Value);
						if (pathElement.Attribute("r") != null) r = floatParse(pathElement.Attribute("r").Value);
						if (pathElement.Attribute("points") != null) points = pathElement.Attribute("points").Value.Split(' ');

						if (svgPauseElement || svgPausePenDown) { /*gcode.Pause(gcodeString, "Pause before path");*/ }
						if (form == "rect")
						{
							if (ry == 0) { ry = rx; }
							else if (rx == 0) { rx = ry; }
							else if (rx != ry) { rx = Math.Min(rx, ry); ry = rx; }   // only same r for x and y are possible
							if (svgComments) gcodeString.AppendFormat("( SVG-Rect x:{0} y:{1} width:{2} height:{3} rx:{4} ry:{5})\r\n", x, y, width, height, rx, ry);
							x += offsetX; y += offsetY;
							gcodeStartPath(x + rx, y + height, form);
							gcodeMoveTo(x + width - rx, y + height, form + " a1");
							if (rx > 0) gcodeArcToCCW(x + width, y + height - ry, 0, -ry, form, avoidG23);  // +ry
							gcodeMoveTo(x + width, y + ry, form + " b1");                        // upper right
							if (rx > 0) gcodeArcToCCW(x + width - rx, y, -rx, 0, form, avoidG23);
							gcodeMoveTo(x + rx, y, form + " a2");                                // upper left
							if (rx > 0) gcodeArcToCCW(x, y + ry, 0, ry, form, avoidG23);                    // -ry
							gcodeMoveTo(x, y + height - ry, form + " b2");                       // lower left
							if (rx > 0)
							{
								gcodeArcToCCW(x + rx, y + height, rx, 0, form, avoidG23);
								gcodeMoveTo(x + rx, y + height, form);  // repeat first point to avoid back draw after last G3
							}
							gcodeStopPath(form);
						}
						else if (form == "circle")
						{
							if (svgComments) gcodeString.AppendFormat("( circle cx:{0} cy:{1} r:{2} )\r\n", cx, cy, r);
							cx += offsetX; cy += offsetY;
							gcodeStartPath(cx + r, cy, form);
							gcodeArcToCCW(cx + r, cy, -r, 0, form, avoidG23);
							gcodeStopPath(form);
						}
						else if (form == "ellipse")
						{
							if (svgComments) gcodeString.AppendFormat("( ellipse cx:{0} cy:{1} rx:{2}  ry:{2})\r\n", cx, cy, rx, ry);
							cx += offsetX; cy += offsetY;
							gcodeStartPath(cx + rx, cy, form);
							isReduceOk = true;
							calcArc(cx + rx, cy, rx, ry, 0, 1, 1, cx - rx, cy);
							calcArc(cx - rx, cy, rx, ry, 0, 1, 1, cx + rx, cy);
							gcodeStopPath(form);

						}
						else if (form == "line")
						{
							if (svgComments) gcodeString.AppendFormat("( SVG-Line x1:{0} y1:{1} x2:{2} y2:{3} )\r\n", x1, y1, x2, y2);
							x1 += offsetX; y1 += offsetY;
							gcodeStartPath(x1, y1, form);
							gcodeMoveTo(x2, y2, form);
							gcodeStopPath(form);
						}
						else if ((form == "polyline") || (form == "polygon"))
						{
							offsetX = 0;// (float)matrixElement.OffsetX;
							offsetY = 0;// (float)matrixElement.OffsetY;
							if (svgComments) gcodeString.AppendFormat("( SVG-Polyline )\r\n");
							int index = 0;
							for (index = 0; index < points.Length; index++)
							{
								if (points[index].Length > 0)
									break;
							}
							if (points[index].IndexOf(",") >= 0)
							{
								string[] coord = points[index].Split(',');
								x = floatParse(coord[0]); y = floatParse(coord[1]);
								x1 = x; y1 = y;
								gcodeStartPath(x, y, form);
								isReduceOk = true;
								for (int i = index + 1; i < points.Length; i++)
								{
									if (points[i].Length > 3)
									{
										coord = points[i].Split(',');
										x = floatParse(coord[0]); y = floatParse(coord[1]);
										x += offsetX; y += offsetY;
										gcodeMoveTo(x, y, form);
									}
								}
								if (form == "polygon")
									gcodeMoveTo(x1, y1, form);
								gcodeStopPath(form);
							}
							else
								gcodeString.AppendLine("( polygon coordinates - missing ',')");
						}
						else if ((form == "text") || (form == "image"))
						{
							gcodeString.AppendLine("( +++++++++++++++++++++++++++++++++ )");
							gcodeString.AppendLine("( ++++++ " + form + " is not supported ++++ )");
							if (form == "text")
							{
								gcodeString.AppendLine("( ++ Convert Object to Path first + )");
							}
							gcodeString.AppendLine("( +++++++++++++++++++++++++++++++++ )");
						}
						else
						{ if (svgComments) gcodeString.Append("( ++++++ Unknown Shape: " + form + " )"); }

						matrixElement = oldMatrixElement;
					}
				}
			}
			return;
		}

		/// <summary>
		/// Convert all Path commands, check: http://www.w3.org/TR/SVG/paths.html
		/// Split command tokens
		/// </summary>
		private void parsePath(XElement svgCode, int level)
		{
			foreach (var pathElement in svgCode.Elements(nspace + "path"))
			{
				if (pathElement != null)
				{
					offsetX = 0;// (float)matrixElement.OffsetX;
					offsetY = 0;// (float)matrixElement.OffsetY;
					currentX = offsetX; currentY = offsetX;
					firstX = null; firstY = null;
					startPath = true;
					startSubPath = true;
					lastX = offsetX; lastY = offsetY;
					string d = pathElement.Attribute("d").Value;
					string id = d;
					if (id.Length > 20)
						id = id.Substring(0, 20);

					string myColor = getColor(pathElement);

					// gcodeString.Append("( Start path )\r\n");
					if (svgComments)
					{
						if (pathElement.Attribute("id") != null)
							gcodeString.Append("\r\n( Path level:" + level.ToString() + " id=" + pathElement.Attribute("id").Value + " )\r\n");
						else
							gcodeString.Append("\r\n( SVG path=" + id + " )\r\n");
						gcodeString.AppendFormat("\r\n(SVG color=#{0})\r\n", myColor);
					}

					if (pathElement.Attribute("id") != null)
						id = pathElement.Attribute("id").Value;

					oldMatrixElement = matrixElement;
					parseTransform(pathElement, false, level);        // transform will be applied in gcodeMove

					if (d.Length > 0)
					{
						// split complete path in to command-tokens
						if (svgPauseElement || svgPausePenDown) { /*gcode.Pause(gcodeString, "Pause before path");*/ }
						string separators = @"(?=[A-Za-z-[e]])";
						var tokens = Regex.Split(d, separators).Where(t => !string.IsNullOrEmpty(t));
						int objCount = 0;
						foreach (string token in tokens)
							objCount += parsePathCommand(token);
					}
					gcodePenUp("End path");

					matrixElement = oldMatrixElement;
				}
			}
			return;
		}

		private bool penIsDown = true;
		private bool startSubPath = true;
		private int countSubPath = 0;
		private bool startPath = true;
		private bool startFirstElement = true;
		private float svgWidthPx, svgHeightPx;
		private float offsetX, offsetY;
		private float currentX, currentY;
		private float? firstX, firstY;
		private float lastX, lastY;
		private float cxMirror = 0, cyMirror = 0;
		private StringBuilder secondMove = new StringBuilder();

		/// <summary>
		/// Convert all Path commands, check: http://www.w3.org/TR/SVG/paths.html
		/// Convert command tokens
		/// </summary>
		private int parsePathCommand(string svgPath)
		{
			var command = svgPath.Take(1).Single();
			char cmd = char.ToUpper(command);
			bool absolute = (cmd == command);
			string remainingargs = svgPath.Substring(1);
			string argSeparators = @"[\s,]|(?=(?<!e)-)";// @"[\s,]|(?=-)|(-{,2})";        // support also -1.2e-3 orig. @"[\s,]|(?=-)"; 
			var splitArgs = Regex
				.Split(remainingargs, argSeparators)
				.Where(t => !string.IsNullOrEmpty(t));
			// get command coordinates
			float[] floatArgs = splitArgs.Select(arg => floatParse(arg)).ToArray();
			int objCount = 0;

			switch (cmd)
			{
				case 'M':       // Start a new sub-path at the given (x,y) coordinate
					for (int i = 0; i < floatArgs.Length; i += 2)
					{
						objCount++;
						if (absolute || startPath)
						{ currentX = floatArgs[i] + offsetX; currentY = floatArgs[i + 1] + offsetY; }
						else
						{ currentX = floatArgs[i] + lastX; currentY = floatArgs[i + 1] + lastY; }
						if (startSubPath)
						{
							if (svgComments) { gcodeString.AppendFormat("( Start new subpath at {0} {1} )\r\n", floatArgs[i], floatArgs[i + 1]); }
							//                            pathCount = 0;
							if (countSubPath++ > 0)
								gcodeStopPath("Stop Path");
							if (svgNodesOnly)
								gcodeDotOnly(currentX, currentY, (command.ToString()));
							else
								gcodeStartPath(currentX, currentY, command.ToString());
							isReduceOk = true;
							firstX = currentX; firstY = currentY;
							startPath = false;
							startSubPath = false;
						}
						else
						{
							if (svgNodesOnly)
								gcodeDotOnly(currentX, currentY, command.ToString());
							else if (i <= 1) // amount of coordinates
							{ gcodeStartPath(currentX, currentY, command.ToString()); }//gcodeMoveTo(currentX, currentY, command.ToString());  // G1
							else
								gcodeMoveTo(currentX, currentY, command.ToString());  // G1
						}
						if (firstX == null) { firstX = currentX; }
						if (firstY == null) { firstY = currentY; }
						lastX = currentX; lastY = currentY;
					}
					cxMirror = currentX; cyMirror = currentY;
					break;

				case 'Z':       // Close the current subpath
					if (!svgNodesOnly)
					{
						if (firstX == null) { firstX = currentX; }
						if (firstY == null) { firstY = currentY; }
						gcodeMoveTo((float)firstX, (float)firstY, command.ToString());    // G1
					}
					lastX = (float)firstX; lastY = (float)firstY;
					firstX = null; firstY = null;
					startSubPath = true;
					if ((svgClosePathExtend) && (!svgNodesOnly))
					{ gcodeString.Append(secondMove); }
					gcodeStopPath("Z");
					break;

				case 'L':       // Draw a line from the current point to the given (x,y) coordinate
					for (int i = 0; i < floatArgs.Length; i += 2)
					{
						objCount++;
						if (absolute)
						{ currentX = floatArgs[i] + offsetX; currentY = floatArgs[i + 1] + offsetY; }
						else
						{ currentX = lastX + floatArgs[i]; currentY = lastY + floatArgs[i + 1]; }
						if (svgNodesOnly)
							gcodeDotOnly(currentX, currentY, command.ToString());
						else
							gcodeMoveTo(currentX, currentY, command.ToString());
						lastX = currentX; lastY = currentY;
						cxMirror = currentX; cyMirror = currentY;
					}
					startSubPath = true;
					break;

				case 'H':       // Draws a horizontal line from the current point (cpx, cpy) to (x, cpy)
					for (int i = 0; i < floatArgs.Length; i++)
					{
						objCount++;
						if (absolute)
						{ currentX = floatArgs[i] + offsetX; currentY = lastY; }
						else
						{ currentX = lastX + floatArgs[i]; currentY = lastY; }
						if (svgNodesOnly)
							gcodeDotOnly(currentX, currentY, command.ToString());
						else
							gcodeMoveTo(currentX, currentY, command.ToString());
						lastX = currentX; lastY = currentY;
						cxMirror = currentX; cyMirror = currentY;
					}
					startSubPath = true;
					break;

				case 'V':       // Draws a vertical line from the current point (cpx, cpy) to (cpx, y)
					for (int i = 0; i < floatArgs.Length; i++)
					{
						objCount++;
						if (absolute)
						{ currentX = lastX; currentY = floatArgs[i] + offsetY; }
						else
						{ currentX = lastX; currentY = lastY + floatArgs[i]; }
						if (svgNodesOnly)
							gcodeDotOnly(currentX, currentY, command.ToString());
						else
							gcodeMoveTo(currentX, currentY, command.ToString());
						lastX = currentX; lastY = currentY;
						cxMirror = currentX; cyMirror = currentY;
					}
					startSubPath = true;
					break;

				case 'A':       // Draws an elliptical arc from the current point to (x, y)
					if (svgComments) { gcodeString.AppendFormat("( Command {0} {1} )\r\n", command.ToString(), ((absolute == true) ? "absolute" : "relative")); }
					for (int rep = 0; rep < floatArgs.Length; rep += 7)
					{
						objCount++;
						if (svgComments) { gcodeString.AppendFormat("( draw arc nr. {0} )\r\n", (1 + rep / 6)); }
						float rx, ry, rot, large, sweep, nx, ny;
						rx = floatArgs[rep]; ry = floatArgs[rep + 1];
						rot = floatArgs[rep + 2];
						large = floatArgs[rep + 3];
						sweep = floatArgs[rep + 4];
						if (absolute)
						{
							nx = floatArgs[rep + 5] + offsetX; ny = floatArgs[rep + 6] + offsetY;
						}
						else
						{
							nx = floatArgs[rep + 5] + lastX; ny = floatArgs[rep + 6] + lastY;
						}
						if (svgNodesOnly)
							gcodeDotOnly(currentX, currentY, command.ToString());
						else
							calcArc(lastX, lastY, rx, ry, rot, large, sweep, nx, ny);
						lastX = nx; lastY = ny;
					}
					startSubPath = true;
					break;

				case 'C':       // Draws a cubic Bézier curve from the current point to (x,y)
					if (svgComments) { gcodeString.AppendFormat("( Command {0} {1} )\r\n", command.ToString(), ((absolute == true) ? "absolute" : "relative")); }
					for (int rep = 0; rep < floatArgs.Length; rep += 6)
					{
						objCount++;
						if (svgComments) { gcodeString.AppendFormat("( draw curve nr. {0} )\r\n", (1 + rep / 6)); }
						if ((rep + 5) < floatArgs.Length)
						{
							float cx1, cy1, cx2, cy2, cx3, cy3;
							if (absolute)
							{
								cx1 = floatArgs[rep] + offsetX; cy1 = floatArgs[rep + 1] + offsetY;
								cx2 = floatArgs[rep + 2] + offsetX; cy2 = floatArgs[rep + 3] + offsetY;
								cx3 = floatArgs[rep + 4] + offsetX; cy3 = floatArgs[rep + 5] + offsetY;
							}
							else
							{
								cx1 = lastX + floatArgs[rep]; cy1 = lastY + floatArgs[rep + 1];
								cx2 = lastX + floatArgs[rep + 2]; cy2 = lastY + floatArgs[rep + 3];
								cx3 = lastX + floatArgs[rep + 4]; cy3 = lastY + floatArgs[rep + 5];
							}
							var points = new Point[4];
							points[0] = new Point(lastX, lastY);
							points[1] = new Point(cx1, cy1);
							points[2] = new Point(cx2, cy2);
							points[3] = new Point(cx3, cy3);
							var b = GetBezierApproximation(points, svgBezierAccuracy);
							if (svgNodesOnly)
							{
								gcodeDotOnly(cx3, cy3, command.ToString());
							}
							else
							{
								for (int i = 1; i < b.Length; i++)
									gcodeMoveTo((float)b[i].X, (float)b[i].Y, command.ToString());
							}
							cxMirror = cx3 - (cx2 - cx3); cyMirror = cy3 - (cy2 - cy3);
							lastX = cx3; lastY = cy3;
						}
						else
						{ gcodeString.AppendFormat("( Missing argument after {0} )\r\n", rep); }
					}
					startSubPath = true;
					break;

				case 'S':       // Draws a cubic Bézier curve from the current point to (x,y)
					if (svgComments) { gcodeString.AppendFormat("( Command {0} {1} )\r\n", command.ToString(), ((absolute == true) ? "absolute" : "relative")); }
					for (int rep = 0; rep < floatArgs.Length; rep += 4)
					{
						objCount++;
						if (svgComments) { gcodeString.AppendFormat("( draw curve nr. {0} )\r\n", (1 + rep / 4)); }
						float cx2, cy2, cx3, cy3;
						if (absolute)
						{
							cx2 = floatArgs[rep] + offsetX; cy2 = floatArgs[rep + 1] + offsetY;
							cx3 = floatArgs[rep + 2] + offsetX; cy3 = floatArgs[rep + 3] + offsetY;
						}
						else
						{
							cx2 = lastX + floatArgs[rep]; cy2 = lastY + floatArgs[rep + 1];
							cx3 = lastX + floatArgs[rep + 2]; cy3 = lastY + floatArgs[rep + 3];
						}
						var points = new Point[4];
						points[0] = new Point(lastX, lastY);
						points[1] = new Point(cxMirror, cyMirror);
						points[2] = new Point(cx2, cy2);
						points[3] = new Point(cx3, cy3);
						var b = GetBezierApproximation(points, svgBezierAccuracy);
						if (svgNodesOnly)
						{
							gcodeDotOnly(cx3, cy3, command.ToString());
						}
						else
						{
							for (int i = 1; i < b.Length; i++)
								gcodeMoveTo((float)b[i].X, (float)b[i].Y, command.ToString());
						}
						cxMirror = cx3 - (cx2 - cx3); cyMirror = cy3 - (cy2 - cy3);
						lastX = cx3; lastY = cy3;
					}
					startSubPath = true;
					break;

				case 'Q':       // Draws a quadratic Bézier curve from the current point to (x,y)
					if (svgComments) { gcodeString.AppendFormat("( Command {0} {1} )\r\n", command.ToString(), ((absolute == true) ? "absolute" : "relative")); }
					for (int rep = 0; rep < floatArgs.Length; rep += 4)
					{
						objCount++;
						if (svgComments) { gcodeString.AppendFormat("( draw curve nr. {0} )\r\n", (1 + rep / 4)); }
						float cx2, cy2, cx3, cy3;
						if (absolute)
						{
							cx2 = floatArgs[rep] + offsetX; cy2 = floatArgs[rep + 1] + offsetY;
							cx3 = floatArgs[rep + 2] + offsetX; cy3 = floatArgs[rep + 3] + offsetY;
						}
						else
						{
							cx2 = lastX + floatArgs[rep]; cy2 = lastY + floatArgs[rep + 1];
							cx3 = lastX + floatArgs[rep + 2]; cy3 = lastY + floatArgs[rep + 3];
						}

						float qpx1 = (cx2 - lastX) * 2 / 3 + lastX;     // shorten control points to 2/3 length to use 
						float qpy1 = (cy2 - lastY) * 2 / 3 + lastY;     // qubic function
						float qpx2 = (cx2 - cx3) * 2 / 3 + cx3;
						float qpy2 = (cy2 - cy3) * 2 / 3 + cy3;
						var points = new Point[4];
						points[0] = new Point(lastX, lastY);
						points[1] = new Point(qpx1, qpy1);
						points[2] = new Point(qpx2, qpy2);
						points[3] = new Point(cx3, cy3);
						cxMirror = cx3 - (cx2 - cx3); cyMirror = cy3 - (cy2 - cy3);
						lastX = cx3; lastY = cy3;
						var b = GetBezierApproximation(points, svgBezierAccuracy);
						if (svgNodesOnly)
						{
							gcodeDotOnly(cx3, cy3, command.ToString());
						}
						else
						{
							for (int i = 1; i < b.Length; i++)
								gcodeMoveTo((float)b[i].X, (float)b[i].Y, command.ToString());
						}
					}
					startSubPath = true;
					break;

				case 'T':       // Draws a quadratic Bézier curve from the current point to (x,y)
					if (svgComments) { gcodeString.AppendFormat("( Command {0} {1} )\r\n", command.ToString(), ((absolute == true) ? "absolute" : "relative")); }
					for (int rep = 0; rep < floatArgs.Length; rep += 2)
					{
						objCount++;
						if (svgComments) { gcodeString.AppendFormat("( draw curve nr. {0} )\r\n", (1 + rep / 2)); }
						float cx3, cy3;
						if (absolute)
						{
							cx3 = floatArgs[rep] + offsetX; cy3 = floatArgs[rep + 1] + offsetY;
						}
						else
						{
							cx3 = lastX + floatArgs[rep]; cy3 = lastY + floatArgs[rep + 1];
						}

						float qpx1 = (cxMirror - lastX) * 2 / 3 + lastX;     // shorten control points to 2/3 length to use 
						float qpy1 = (cyMirror - lastY) * 2 / 3 + lastY;     // qubic function
						float qpx2 = (cxMirror - cx3) * 2 / 3 + cx3;
						float qpy2 = (cyMirror - cy3) * 2 / 3 + cy3;
						var points = new Point[4];
						points[0] = new Point(lastX, lastY);
						points[1] = new Point(qpx1, qpy1);
						points[2] = new Point(qpx2, qpy2);
						points[3] = new Point(cx3, cy3);
						cxMirror = cx3; cyMirror = cy3;
						lastX = cx3; lastY = cy3;
						var b = GetBezierApproximation(points, svgBezierAccuracy);
						if (svgNodesOnly)
						{
							gcodeDotOnly(cx3, cy3, command.ToString());
						}
						else
						{
							for (int i = 1; i < b.Length; i++)
								gcodeMoveTo((float)b[i].X, (float)b[i].Y, command.ToString());
						}
					}
					startSubPath = true;
					break;

				default:
					if (svgComments) gcodeString.Append("( *********** unknown: " + command.ToString() + " ***** )\r\n");
					break;
			}
			return objCount;
		}

		/// <summary>
		/// Calculate Path-Arc-Command - Code from https://github.com/vvvv/SVG/blob/master/Source/Paths/SvgArcSegment.cs
		/// </summary>
		private void calcArc(float StartX, float StartY, float RadiusX, float RadiusY, float Angle, float Size, float Sweep, float EndX, float EndY)
		{
			if (RadiusX == 0.0f && RadiusY == 0.0f)
			{
				//              graphicsPath.AddLine(this.Start, this.End);
				return;
			}
			double sinPhi = Math.Sin(Angle * Math.PI / 180.0);
			double cosPhi = Math.Cos(Angle * Math.PI / 180.0);
			double x1dash = cosPhi * (StartX - EndX) / 2.0 + sinPhi * (StartY - EndY) / 2.0;
			double y1dash = -sinPhi * (StartX - EndX) / 2.0 + cosPhi * (StartY - EndY) / 2.0;
			double root;
			double numerator = RadiusX * RadiusX * RadiusY * RadiusY - RadiusX * RadiusX * y1dash * y1dash - RadiusY * RadiusY * x1dash * x1dash;
			float rx = RadiusX;
			float ry = RadiusY;
			if (numerator < 0.0)
			{
				float s = (float)Math.Sqrt(1.0 - numerator / (RadiusX * RadiusX * RadiusY * RadiusY));

				rx *= s;
				ry *= s;
				root = 0.0;
			}
			else
			{
				root = ((Size == 1 && Sweep == 1) || (Size == 0 && Sweep == 0) ? -1.0 : 1.0) * Math.Sqrt(numerator / (RadiusX * RadiusX * y1dash * y1dash + RadiusY * RadiusY * x1dash * x1dash));
			}
			double cxdash = root * rx * y1dash / ry;
			double cydash = -root * ry * x1dash / rx;
			double cx = cosPhi * cxdash - sinPhi * cydash + (StartX + EndX) / 2.0;
			double cy = sinPhi * cxdash + cosPhi * cydash + (StartY + EndY) / 2.0;
			double theta1 = CalculateVectorAngle(1.0, 0.0, (x1dash - cxdash) / rx, (y1dash - cydash) / ry);
			double dtheta = CalculateVectorAngle((x1dash - cxdash) / rx, (y1dash - cydash) / ry, (-x1dash - cxdash) / rx, (-y1dash - cydash) / ry);
			if (Sweep == 0 && dtheta > 0)
			{
				dtheta -= 2.0 * Math.PI;
			}
			else if (Sweep == 1 && dtheta < 0)
			{
				dtheta += 2.0 * Math.PI;
			}
			int segments = (int)Math.Ceiling((double)Math.Abs(dtheta / (Math.PI / 2.0)));
			double delta = dtheta / segments;
			double t = 8.0 / 3.0 * Math.Sin(delta / 4.0) * Math.Sin(delta / 4.0) / Math.Sin(delta / 2.0);

			double startX = StartX;
			double startY = StartY;

			for (int i = 0; i < segments; ++i)
			{
				double cosTheta1 = Math.Cos(theta1);
				double sinTheta1 = Math.Sin(theta1);
				double theta2 = theta1 + delta;
				double cosTheta2 = Math.Cos(theta2);
				double sinTheta2 = Math.Sin(theta2);

				double endpointX = cosPhi * rx * cosTheta2 - sinPhi * ry * sinTheta2 + cx;
				double endpointY = sinPhi * rx * cosTheta2 + cosPhi * ry * sinTheta2 + cy;

				double dx1 = t * (-cosPhi * rx * sinTheta1 - sinPhi * ry * cosTheta1);
				double dy1 = t * (-sinPhi * rx * sinTheta1 + cosPhi * ry * cosTheta1);

				double dxe = t * (cosPhi * rx * sinTheta2 + sinPhi * ry * cosTheta2);
				double dye = t * (sinPhi * rx * sinTheta2 - cosPhi * ry * cosTheta2);

				var points = new Point[4];
				points[0] = new Point(startX, startY);
				points[1] = new Point((startX + dx1), (startY + dy1));
				points[2] = new Point((endpointX + dxe), (endpointY + dye));
				points[3] = new Point(endpointX, endpointY);
				var b = GetBezierApproximation(points, svgBezierAccuracy);
				for (int k = 1; k < b.Length; k++)
					gcodeMoveTo(b[k], "arc");

				theta1 = theta2;
				startX = (float)endpointX;
				startY = (float)endpointY;
			}
		}
		private double CalculateVectorAngle(double ux, double uy, double vx, double vy)
		{
			double ta = Math.Atan2(uy, ux);
			double tb = Math.Atan2(vy, vx);
			if (tb >= ta)
			{ return tb - ta; }
			return Math.PI * 2 - (ta - tb);
		}
		// helper functions
		private float fsqrt(float x) { return (float)Math.Sqrt(x); }
		private float fvmag(float x, float y) { return fsqrt(x * x + y * y); }
		private float fdistance(float x1, float y1, float x2, float y2) { return fvmag(x2 - x1, y2 - y1); }

		/// <summary>
		/// Calculate Bezier line segments
		/// </summary>
		private Point[] GetBezierApproximation(Point[] controlPoints, int outputSegmentCount)
		{
			if (UseLegacyBezier)
				return GetBezierApproximationOld(controlPoints, outputSegmentCount);
			else
				return BezierTools.FlattenTo(controlPoints, .2 / scaledError).ToArray();
		}

        private Point[] GetBezierApproximationOld(Point[] controlPoints, int outputSegmentCount)
		{
			Point[] points = new Point[outputSegmentCount + 1];
			for (int i = 0; i <= outputSegmentCount; i++)
			{
				double t = (double)i / outputSegmentCount;
				points[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
			}
			return points;
		}
		private Point GetBezierPoint(double t, Point[] controlPoints, int index, int count)
		{
			if (count == 1)
				return controlPoints[index];
			var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
			var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
			double x = (1 - t) * P0.X + t * P1.X;
			return new Point(x, (1 - t) * P0.Y + t * P1.Y);
		}


		// Prepare G-Code

		/// <summary>
		/// Transform XY coordinate using matrix and scale  
		/// </summary>
		/// <param name="pointStart">coordinate to transform</param>
		/// <returns>transformed coordinate</returns>
		private Point translateXY(float x, float y)
		{
			Point coord = new Point(x, y);
			return translateXY(coord);
		}
		private Point translateXY(Point pointStart)
		{
			Point pointResult = matrixElement.Transform(pointStart);
			return pointResult;
		}
		/// <summary>
		/// Transform I,J coordinate using matrix and scale  
		/// </summary>
		/// <param name="pointStart">coordinate to transform</param>
		/// <returns>transformed coordinate</returns>
		private Point translateIJ(float i, float j)
		{
			Point coord = new Point(i, j);
			return translateIJ(coord);
		}
		private Point translateIJ(Point pointStart)
		{
			Point pointResult = pointStart;
			double tmp_i = pointStart.X, tmp_j = pointStart.Y;
			pointResult.X = tmp_i * matrixElement.M11 + tmp_j * matrixElement.M21;  // - tmp
			pointResult.Y = tmp_i * matrixElement.M12 + tmp_j * matrixElement.M22; // tmp_i*-matrix     // i,j are relative - no offset needed, but perhaps rotation
			return pointResult;
		}


		private void gcodeDotOnly(float x, float y, string cmt)
		{
			gcodeStartPath(x, y, cmt);
			gcodePenDown(cmt);
			gcodePenUp(cmt);
		}


		/// <summary>
		/// Insert G0 and Pen down gcode command
		/// </summary>
		private void gcodeStartPath(float x, float y, string cmt)
		{
			Point coord = translateXY(x, y);
			lastGCX = coord.X; lastGCY = coord.Y;
			lastSetGCX = coord.X; lastSetGCY = coord.Y;
			gcodePenUp(cmt);
			gcode.MoveToRapid(gcodeString, coord, cmt);
			if (svgPausePenDown) { /*gcode.Pause(gcodeString, "Pause before Pen Down");*/ }
			penIsDown = false;
			isReduceOk = false;
		}
		/// <summary>
		/// Insert Pen-up gcode command
		/// </summary>
		private void gcodeStopPath(string cmt)
		{
			if (gcodeReduce)
			{
				if ((lastSetGCX != lastGCX) || (lastSetGCY != lastGCY)) // restore last skipped point for accurat G2/G3 use
					gcode.MoveTo(gcodeString, new System.Windows.Point(lastGCX, lastGCY), "restore Point");
			}
			gcodePenUp(cmt);
		}

		/// <summary>
		/// Insert G1 gcode command
		/// </summary>
		private void gcodeMoveTo(float x, float y, string cmt)
		{
			Point coord = new Point(x, y);
			gcodeMoveTo(coord, cmt);
		}

		private bool isReduceOk = false;
		private bool rejectPoint = false;
		private double lastGCX = 0, lastGCY = 0, lastSetGCX = 0, lastSetGCY = 0, distance;
		
		public bool UseLegacyBezier { get; set; }
		public bool SvgScaleApply { get => svgScaleApply; set => svgScaleApply = value; }
		public float SvgMaxSize { get => svgMaxSize; set => svgMaxSize = value; }

		/// <summary>
		/// Insert G1 gcode command
		/// </summary>
		private void gcodeMoveTo(Point orig, string cmt)
		{
			Point coord = translateXY(orig);
			rejectPoint = false;
			gcodePenDown(cmt);
			if (gcodeReduce && isReduceOk)
			{
				distance = Math.Sqrt(((coord.X - lastSetGCX) * (coord.X - lastSetGCX)) + ((coord.Y - lastSetGCY) * (coord.Y - lastSetGCY)));
				if (distance < gcodeReduceVal)      // discard actual G1 movement
				{
					rejectPoint = true;
				}
				else
				{
					lastSetGCX = coord.X; lastSetGCY = coord.Y;
				}
			}
			if (!gcodeReduce || !rejectPoint)       // write GCode
			{
				gcode.MoveTo(gcodeString, coord, cmt);
			}
			lastGCX = coord.X; lastGCY = coord.Y;
		}

		/// <summary>
		/// Insert G2/G3 gcode command
		/// </summary>
		private void gcodeArcToCCW(float x, float y, float i, float j, string cmt, bool avoidG23 = false)
		{
			Point coordxy = translateXY(x, y);
			Point coordij = translateIJ(i, j);
			gcodePenDown(cmt);
			if (gcodeReduce && isReduceOk)      // restore last skipped point for accurat G2/G3 use
			{
				if ((lastSetGCX != lastGCX) || (lastSetGCY != lastGCY))
					gcode.MoveTo(gcodeString, new System.Windows.Point(lastGCX, lastGCY), cmt);
			}
			gcode.Arc(gcodeString, 3, coordxy, coordij, cmt, avoidG23);
		}

		/// <summary>
		/// Insert Pen-up gcode command
		/// </summary>
		private void gcodePenUp(string cmt)
		{
			if (penIsDown)
				gcode.PenUp(gcodeString, cmt);
			penIsDown = false;
		}
		private void gcodePenDown(string cmt)
		{
			if (!penIsDown)
				gcode.PenDown(gcodeString, cmt);
			penIsDown = true;
		}

	}
}