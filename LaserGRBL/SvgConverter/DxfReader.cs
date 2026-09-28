//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Point = System.Windows.Point;

namespace LaserGRBL.SvgConverter
{
	/// <summary>
	/// Error in the dxf file that must be shown to the user (binary dxf, nothing to import)
	/// </summary>
	public class DxfImportException : Exception
	{
		public DxfImportException(string message) : base(message) { }
	}

	/// <summary>
	/// Read an ASCII dxf file as lines and true arcs in mm, one color for each dxf color,
	/// to be imported with the color layers without intermediate formats
	/// </summary>
	public static class DxfReader
	{
		private const int MaxBlockDepth = 32;       // protection against blocks that insert themselves
		private const double ArcTolerance = 0.02;    // max distance (mm) between an arc and its approximation
		private const double SplineStep = 0.1;       // length (mm) of the segments used to approximate splines

		public static VectorDrawing Read(string filename)
		{
			return Read(filename, Settings.GetObject(ArcFitter.ToleranceSetting, ArcFitter.DefaultTolerance));
		}

		/// <param name="splineTolerance">splines are converted to arcs within this distance (mm), 0 to keep them as short lines</param>
		public static VectorDrawing Read(string filename, double splineTolerance)
		{
			if (IsBinary(filename))
				throw new DxfImportException(Strings.DxfBinaryNotSupported);

			DxfDocument doc = DxfDocument.Load(File.ReadAllLines(filename));
			VectorDrawing drawing = new DrawingBuilder(doc, splineTolerance).Build();
			if (drawing == null)
				throw new DxfImportException(Strings.DxfNoEntities);
			return drawing;
		}

		private static bool IsBinary(string filename)
		{
			const string signature = "AutoCAD Binary DXF";
			byte[] head = new byte[signature.Length];
			using (FileStream fs = File.OpenRead(filename))
			{
				int read = fs.Read(head, 0, head.Length);
				return read == head.Length && Encoding.ASCII.GetString(head) == signature;
			}
		}

		#region Dxf structure

		// an entity (or table record) as the list of its code/value pairs
		private class DxfEntity
		{
			public string Type;
			public List<KeyValuePair<int, string>> Pairs = new List<KeyValuePair<int, string>>();
			public List<DxfEntity> Vertices = new List<DxfEntity>(); // VERTEX of a POLYLINE
			public bool SequenceEnded;                                // SEQEND found after the vertices

			public DxfEntity(string type)
			{ Type = type; }

			public bool Has(int code)
			{
				foreach (KeyValuePair<int, string> p in Pairs)
					if (p.Key == code) return true;
				return false;
			}

			public string GetString(int code, string def)
			{
				foreach (KeyValuePair<int, string> p in Pairs)
					if (p.Key == code) return p.Value;
				return def;
			}

			public double GetDouble(int code, double def)
			{
				foreach (KeyValuePair<int, string> p in Pairs)
					if (p.Key == code) return ParseDouble(p.Value);
				return def;
			}

			public int GetInt(int code, int def)
			{
				foreach (KeyValuePair<int, string> p in Pairs)
					if (p.Key == code) return (int)ParseLong(p.Value);
				return def;
			}

			public List<double> GetAll(int code)
			{
				List<double> rv = new List<double>();
				foreach (KeyValuePair<int, string> p in Pairs)
					if (p.Key == code) rv.Add(ParseDouble(p.Value));
				return rv;
			}

			// the Z of the extrusion direction: -1 means that the object coordinate system is mirrored on X
			public bool MirroredOCS
			{ get { return GetDouble(230, 1) < 0; } }
		}

		private class DxfBlock
		{
			public string Name;
			public double BaseX, BaseY;
			public List<DxfEntity> Entities = new List<DxfEntity>();
		}

		private class DxfLayer
		{
			public string Color;
			public bool Hidden; // off or frozen
		}

		private class DxfDocument
		{
			public double UnitToMM = 1;
			public int Units = 0;
			public Dictionary<string, DxfLayer> Layers = new Dictionary<string, DxfLayer>(StringComparer.OrdinalIgnoreCase);
			public Dictionary<string, DxfBlock> Blocks = new Dictionary<string, DxfBlock>(StringComparer.OrdinalIgnoreCase);
			public List<DxfEntity> Entities = new List<DxfEntity>();

			public static DxfDocument Load(string[] lines)
			{
				DxfDocument doc = new DxfDocument();

				// split the file in records, each one starts with a code 0 pair
				List<DxfEntity> records = new List<DxfEntity>();
				DxfEntity current = null;
				for (int i = 0; i + 1 < lines.Length; i += 2)
				{
					string codeText = lines[i].Trim();
					if (codeText.Length == 0 && i + 2 >= lines.Length) break; // empty line at the end of the file
					int code = int.Parse(codeText, CultureInfo.InvariantCulture);
					string value = lines[i + 1].Trim();
					if (code == 0)
					{
						current = new DxfEntity(value.ToUpperInvariant());
						records.Add(current);
					}
					else if (current != null)
					{
						current.Pairs.Add(new KeyValuePair<int, string>(code, value));
					}
				}

				string section = null;
				DxfBlock block = null;
				foreach (DxfEntity r in records)
				{
					if (r.Type == "SECTION")
					{
						section = r.GetString(2, "").ToUpperInvariant();
						if (section == "HEADER")
							doc.ReadHeader(r);
					}
					else if (r.Type == "ENDSEC")
					{
						section = null;
					}
					else if (section == "TABLES" && r.Type == "LAYER")
					{
						DxfLayer layer = new DxfLayer();
						int aci = r.GetInt(62, 7);
						layer.Color = r.Has(420) ? TrueColor(r.GetInt(420, 0)) : AciColor(Math.Abs(aci));
						layer.Hidden = aci < 0 || (r.GetInt(70, 0) & 1) != 0;
						doc.Layers[r.GetString(2, "0")] = layer;
					}
					else if (section == "BLOCKS")
					{
						if (r.Type == "BLOCK")
						{
							block = new DxfBlock();
							block.Name = r.GetString(2, "");
							block.BaseX = r.GetDouble(10, 0);
							block.BaseY = r.GetDouble(20, 0);
							doc.Blocks[block.Name] = block;
						}
						else if (r.Type == "ENDBLK")
						{
							block = null;
						}
						else if (block != null)
						{
							AddEntity(block.Entities, r);
						}
					}
					else if (section == "ENTITIES")
					{
						AddEntity(doc.Entities, r);
					}
				}

				return doc;
			}

			private void ReadHeader(DxfEntity header)
			{
				for (int i = 0; i + 1 < header.Pairs.Count; i++)
				{
					if (header.Pairs[i].Key == 9 && header.Pairs[i].Value.ToUpperInvariant() == "$INSUNITS")
					{
						Units = (int)ParseLong(header.Pairs[i + 1].Value);
						UnitToMM = UnitsToMM(Units);
					}
				}
			}

			// VERTEX records belong to the POLYLINE before them, until SEQEND
			private static void AddEntity(List<DxfEntity> list, DxfEntity r)
			{
				DxfEntity last = list.Count > 0 ? list[list.Count - 1] : null;
				bool inSequence = last != null && last.Type == "POLYLINE" && !last.SequenceEnded;

				if (r.Type == "VERTEX" && inSequence)
					last.Vertices.Add(r);
				else if (r.Type == "SEQEND")
				{
					if (inSequence) last.SequenceEnded = true;
				}
				else if (r.Type != "ATTRIB") // attributes of an INSERT (text)
					list.Add(r);
			}
		}

		private static double UnitsToMM(int units)
		{
			switch (units)
			{
				case 1: return 25.4;      // inches
				case 2: return 304.8;     // feet
				case 4: return 1;         // millimeters
				case 5: return 10;        // centimeters
				case 6: return 1000;      // meters
				case 8: return 0.0000254; // microinches
				case 9: return 0.0254;    // mils
				case 10: return 914.4;    // yards
				case 13: return 0.001;    // microns
				case 14: return 100;      // decimeters
				default: return 1;        // unitless or unknown: assume mm
			}
		}

		private static double ParseDouble(string value)
		{ return double.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture); }

		private static long ParseLong(string value)
		{ return long.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture); }

		#endregion

		#region Colors

		// standard AutoCAD Color Index palette (0 is ByBlock and 256 is ByLayer, they are not real colors)
		private static readonly int[] AciPalette = new int[] {
			0x000000, 0xFF0000, 0xFFFF00, 0x00FF00, 0x00FFFF, 0x0000FF, 0xFF00FF, 0xFFFFFF,
			0x808080, 0xC0C0C0, 0xFF0000, 0xFF7F7F, 0xCC0000, 0xCC6666, 0x990000, 0x994C4C,
			0x7F0000, 0x7F3F3F, 0x4C0000, 0x4C2626, 0xFF3F00, 0xFF9F7F, 0xCC3300, 0xCC7F66,
			0x992600, 0x995F4C, 0x7F1F00, 0x7F4F3F, 0x4C1300, 0x4C2F26, 0xFF7F00, 0xFFBF7F,
			0xCC6600, 0xCC9966, 0x994C00, 0x99724C, 0x7F3F00, 0x7F5F3F, 0x4C2600, 0x4C3926,
			0xFFBF00, 0xFFDF7F, 0xCC9900, 0xCCB266, 0x997200, 0x99854C, 0x7F5F00, 0x7F6F3F,
			0x4C3900, 0x4C4226, 0xFFFF00, 0xFFFF7F, 0xCCCC00, 0xCCCC66, 0x999900, 0x99994C,
			0x7F7F00, 0x7F7F3F, 0x4C4C00, 0x4C4C26, 0xBFFF00, 0xDFFF7F, 0x99CC00, 0xB2CC66,
			0x729900, 0x85994C, 0x5F7F00, 0x6F7F3F, 0x394C00, 0x424C26, 0x7FFF00, 0xBFFF7F,
			0x66CC00, 0x99CC66, 0x4C9900, 0x72994C, 0x3F7F00, 0x5F7F3F, 0x264C00, 0x394C26,
			0x3FFF00, 0x9FFF7F, 0x33CC00, 0x7FCC66, 0x269900, 0x5F994C, 0x1F7F00, 0x4F7F3F,
			0x134C00, 0x2F4C26, 0x00FF00, 0x7FFF7F, 0x00CC00, 0x66CC66, 0x009900, 0x4C994C,
			0x007F00, 0x3F7F3F, 0x004C00, 0x264C26, 0x00FF3F, 0x7FFF9F, 0x00CC33, 0x66CC7F,
			0x009926, 0x4C995F, 0x007F1F, 0x3F7F4F, 0x004C13, 0x264C2F, 0x00FF7F, 0x7FFFBF,
			0x00CC66, 0x66CC99, 0x00994C, 0x4C9972, 0x007F3F, 0x3F7F5F, 0x004C26, 0x264C39,
			0x00FFBF, 0x7FFFDF, 0x00CC99, 0x66CCB2, 0x009972, 0x4C9985, 0x007F5F, 0x3F7F6F,
			0x004C39, 0x264C42, 0x00FFFF, 0x7FFFFF, 0x00CCCC, 0x66CCCC, 0x009999, 0x4C9999,
			0x007F7F, 0x3F7F7F, 0x004C4C, 0x264C4C, 0x00BFFF, 0x7FDFFF, 0x0099CC, 0x66B2CC,
			0x007299, 0x4C8599, 0x005F7F, 0x3F6F7F, 0x00394C, 0x26424C, 0x007FFF, 0x7FBFFF,
			0x0066CC, 0x6699CC, 0x004C99, 0x4C7299, 0x003F7F, 0x3F5F7F, 0x00264C, 0x26394C,
			0x003FFF, 0x7F9FFF, 0x0033CC, 0x667FCC, 0x002699, 0x4C5F99, 0x001F7F, 0x3F4F7F,
			0x00134C, 0x262F4C, 0x0000FF, 0x7F7FFF, 0x0000CC, 0x6666CC, 0x000099, 0x4C4C99,
			0x00007F, 0x3F3F7F, 0x00004C, 0x26264C, 0x3F00FF, 0x9F7FFF, 0x3300CC, 0x7F66CC,
			0x260099, 0x5F4C99, 0x1F007F, 0x4F3F7F, 0x13004C, 0x2F264C, 0x7F00FF, 0xBF7FFF,
			0x6600CC, 0x9966CC, 0x4C0099, 0x724C99, 0x3F007F, 0x5F3F7F, 0x26004C, 0x39264C,
			0xBF00FF, 0xDF7FFF, 0x9900CC, 0xB266CC, 0x720099, 0x854C99, 0x5F007F, 0x6F3F7F,
			0x39004C, 0x42264C, 0xFF00FF, 0xFF7FFF, 0xCC00CC, 0xCC66CC, 0x990099, 0x994C99,
			0x7F007F, 0x7F3F7F, 0x4C004C, 0x4C264C, 0xFF00BF, 0xFF7FDF, 0xCC0099, 0xCC66B2,
			0x990072, 0x994C85, 0x7F005F, 0x7F3F6F, 0x4C0039, 0x4C2642, 0xFF007F, 0xFF7FBF,
			0xCC0066, 0xCC6699, 0x99004C, 0x994C72, 0x7F003F, 0x7F3F5F, 0x4C0026, 0x4C2639,
			0xFF003F, 0xFF7F9F, 0xCC0033, 0xCC667F, 0x990026, 0x994C5F, 0x7F001F, 0x7F3F4F,
			0x4C0013, 0x4C262F, 0x333333, 0x505050, 0x696969, 0x828282, 0xBEBEBE, 0xFFFFFF };

		private static string AciColor(int index)
		{
			// 7 is white on a black background and black on a white one: for the laser it is black
			if (index <= 0 || index == 7 || index >= AciPalette.Length)
				return "#000000";
			return TrueColor(AciPalette[index]);
		}

		private static string TrueColor(int rgb)
		{ return "#" + (rgb & 0xFFFFFF).ToString("X6"); }

		#endregion

		#region Geometry

		// affine transformation: x' = A*x + C*y + E, y' = B*x + D*y + F
		private class Transform
		{
			public double A = 1, B = 0, C = 0, D = 1, E = 0, F = 0;

			public static Transform Translation(double x, double y)
			{ Transform t = new Transform(); t.E = x; t.F = y; return t; }

			public static Transform Scaling(double sx, double sy)
			{ Transform t = new Transform(); t.A = sx; t.D = sy; return t; }

			public static Transform Rotation(double degrees)
			{
				double a = degrees * Math.PI / 180;
				Transform t = new Transform();
				t.A = Math.Cos(a); t.B = Math.Sin(a); t.C = -Math.Sin(a); t.D = Math.Cos(a);
				return t;
			}

			// apply "other" first, then this
			public Transform Then(Transform other)
			{
				Transform t = new Transform();
				t.A = A * other.A + C * other.B;
				t.B = B * other.A + D * other.B;
				t.C = A * other.C + C * other.D;
				t.D = B * other.C + D * other.D;
				t.E = A * other.E + C * other.F + E;
				t.F = B * other.E + D * other.F + F;
				return t;
			}

			public double[] Apply(double x, double y)
			{ return new double[] { A * x + C * y + E, B * x + D * y + F }; }

			public double Determinant
			{ get { return A * D - B * C; } }

			public double Scale
			{ get { return Math.Sqrt(Math.Abs(Determinant)); } }

			// circles remain circles (uniform scale, rotation, mirror)
			public bool IsSimilarity
			{
				get
				{
					double sx = A * A + B * B, sy = C * C + D * D;
					return Math.Abs(sx - sy) <= 1e-9 * Math.Max(sx, sy) && Math.Abs(A * C + B * D) <= 1e-9 * Math.Max(sx, sy);
				}
			}
		}

		// state inherited from the INSERT when drawing the entities of a block
		private class Context
		{
			public Transform Transform;
			public string BlockLayer; // entities on layer "0" take the layer of the INSERT
			public string BlockColor; // color used by ByBlock entities
			public int Depth;
		}

		private class DrawingBuilder
		{
			private DxfDocument mDoc;
			private VectorDrawing mDrawing = new VectorDrawing();
			private SortedDictionary<string, int> mIgnored = new SortedDictionary<string, int>();
			private int mHidden;
			private int mCount;

			// path under construction
			private VectorPath mPath;
			private double mTolerance; // arc tolerance in drawing units
			private double mSplineTolerance; // mm, 0 = splines as lines

			public DrawingBuilder(DxfDocument doc, double splineTolerance)
			{
				mDoc = doc;
				mSplineTolerance = splineTolerance;
			}

			public VectorDrawing Build()
			{
				Context ctx = new Context();
				ctx.Transform = new Transform();
				ctx.BlockLayer = "0";
				ctx.BlockColor = "#000000";

				foreach (DxfEntity e in mDoc.Entities)
					if (e.GetInt(67, 0) == 0) // skip paper space
						DrawEntity(e, ctx);

				foreach (KeyValuePair<string, int> kv in mIgnored)
					Logger.LogMessage("DxfImport", "Ignored {0} {1} entities", kv.Value, kv.Key);
				if (mHidden > 0)
					Logger.LogMessage("DxfImport", "Ignored {0} entities on hidden layers", mHidden);
				Logger.LogMessage("DxfImport", "Imported {0} entities, units {1} ({2} mm)", mCount, mDoc.Units, mDoc.UnitToMM);

				if (mCount == 0)
					return null;

				mDrawing.MoveToOrigin();
				return mDrawing;
			}

			private void Ignore(string type)
			{
				int count;
				mIgnored.TryGetValue(type, out count);
				mIgnored[type] = count + 1;
			}

			private string EffectiveLayer(DxfEntity e, Context ctx)
			{
				string layer = e.GetString(8, "0");
				return layer == "0" && ctx.Depth > 0 ? ctx.BlockLayer : layer;
			}

			private string LayerColor(string name)
			{
				DxfLayer layer;
				return mDoc.Layers.TryGetValue(name, out layer) ? layer.Color : "#000000";
			}

			private string EntityColor(DxfEntity e, Context ctx)
			{
				if (e.Has(420))
					return TrueColor(e.GetInt(420, 0));
				int aci = e.GetInt(62, 256);
				if (aci == 0) return ctx.BlockColor;                     // ByBlock
				if (aci == 256) return LayerColor(EffectiveLayer(e, ctx)); // ByLayer
				return AciColor(Math.Abs(aci));
			}

			private void DrawEntity(DxfEntity e, Context ctx)
			{
				if (e.GetInt(60, 0) == 1) // invisible
					return;

				DxfLayer layer;
				if (mDoc.Layers.TryGetValue(EffectiveLayer(e, ctx), out layer) && layer.Hidden)
				{
					mHidden++;
					return;
				}

				string color = EntityColor(e, ctx);
				Transform t = ctx.Transform;
				// the object coordinate system of 2D entities is mirrored when the extrusion direction points down
				Transform ocs = e.MirroredOCS ? t.Then(Transform.Scaling(-1, 1)) : t;
				mTolerance = ArcTolerance / mDoc.UnitToMM / Math.Max(t.Scale, 1e-9);

				switch (e.Type)
				{
					case "LINE":
						BeginPath();
						MoveTo(t, e.GetDouble(10, 0), e.GetDouble(20, 0));
						LineTo(t, e.GetDouble(11, 0), e.GetDouble(21, 0));
						EndPath(color, false);
						break;
					case "LWPOLYLINE":
						DrawLwPolyline(e, ocs, color);
						break;
					case "POLYLINE":
						DrawPolyline(e, t, ocs, color);
						break;
					case "CIRCLE":
						DrawCircle(ocs, e.GetDouble(10, 0), e.GetDouble(20, 0), e.GetDouble(40, 0), color);
						break;
					case "ARC":
						{
							double a1 = e.GetDouble(50, 0), a2 = e.GetDouble(51, 360);
							double sweep = a2 - a1;
							while (sweep <= 0) sweep += 360;
							while (sweep > 360) sweep -= 360;
							BeginPath();
							ArcTo(ocs, e.GetDouble(10, 0), e.GetDouble(20, 0), e.GetDouble(40, 0), a1 * Math.PI / 180, sweep * Math.PI / 180, true);
							EndPath(color, false);
							break;
						}
					case "ELLIPSE":
						DrawEllipse(e, t, color);
						break;
					case "SPLINE":
						DrawSpline(e, t, color);
						break;
					case "INSERT":
						DrawInsert(e, ctx, color);
						return; // counted by its entities
					default:
						Ignore(e.Type);
						return;
				}
			}

			#region Entities

			private void DrawInsert(DxfEntity e, Context ctx, string color)
			{
				DxfBlock block;
				if (!mDoc.Blocks.TryGetValue(e.GetString(2, ""), out block))
				{
					Logger.LogMessage("DxfImport", "Block {0} not found", e.GetString(2, ""));
					return;
				}
				if (ctx.Depth >= MaxBlockDepth)
				{
					Logger.LogMessage("DxfImport", "Block {0} nested too deep", block.Name);
					return;
				}

				Transform parent = e.MirroredOCS ? ctx.Transform.Then(Transform.Scaling(-1, 1)) : ctx.Transform;
				Transform position = Transform.Translation(e.GetDouble(10, 0), e.GetDouble(20, 0)).Then(Transform.Rotation(e.GetDouble(50, 0)));
				Transform local = Transform.Scaling(e.GetDouble(41, 1), e.GetDouble(42, 1)).Then(Transform.Translation(-block.BaseX, -block.BaseY));

				// MINSERT: grid of copies, spacing along the rotated axes
				int cols = Math.Max(1, e.GetInt(70, 1)), rows = Math.Max(1, e.GetInt(71, 1));
				double dx = e.GetDouble(44, 0), dy = e.GetDouble(45, 0);

				for (int r = 0; r < rows; r++)
				{
					for (int c = 0; c < cols; c++)
					{
						Context child = new Context();
						child.Transform = parent.Then(position).Then(Transform.Translation(c * dx, r * dy)).Then(local);
						child.BlockLayer = EffectiveLayer(e, ctx);
						child.BlockColor = color;
						child.Depth = ctx.Depth + 1;

						foreach (DxfEntity be in block.Entities)
							DrawEntity(be, child);
					}
				}
			}

			private void DrawLwPolyline(DxfEntity e, Transform t, string color)
			{
				List<double[]> vertices = new List<double[]>(); // x, y, bulge
				foreach (KeyValuePair<int, string> p in e.Pairs)
				{
					if (p.Key == 10)
						vertices.Add(new double[] { ParseDouble(p.Value), 0, 0 });
					else if (p.Key == 20 && vertices.Count > 0)
						vertices[vertices.Count - 1][1] = ParseDouble(p.Value);
					else if (p.Key == 42 && vertices.Count > 0)
						vertices[vertices.Count - 1][2] = ParseDouble(p.Value);
				}
				DrawVertices(vertices, (e.GetInt(70, 0) & 1) != 0, t, color);
			}

			private void DrawPolyline(DxfEntity e, Transform wcs, Transform ocs, string color)
			{
				int flags = e.GetInt(70, 0);
				if ((flags & (16 | 64)) != 0) // polygon mesh, polyface mesh
				{
					Ignore("POLYLINE mesh");
					return;
				}

				List<double[]> vertices = new List<double[]>();
				foreach (DxfEntity v in e.Vertices)
					if ((v.GetInt(70, 0) & 16) == 0) // skip spline frame control points
						vertices.Add(new double[] { v.GetDouble(10, 0), v.GetDouble(20, 0), v.GetDouble(42, 0) });

				bool is3D = (flags & 8) != 0; // 3D polyline: world coordinates, no arcs
				DrawVertices(vertices, (flags & 1) != 0, is3D ? wcs : ocs, color);
			}

			private void DrawVertices(List<double[]> v, bool closed, Transform t, string color)
			{
				if (v.Count == 0) return;

				BeginPath();
				MoveTo(t, v[0][0], v[0][1]);
				int segments = closed ? v.Count : v.Count - 1;
				for (int i = 0; i < segments; i++)
				{
					double[] a = v[i], b = v[(i + 1) % v.Count];
					if (Math.Abs(a[2]) < 1e-9)
						LineTo(t, b[0], b[1]);
					else
						BulgeTo(t, a[0], a[1], b[0], b[1], a[2]);
				}
				EndPath(color, closed);
			}

			// arc from (x1,y1) to (x2,y2): bulge is tan(angle/4), positive when counterclockwise
			private void BulgeTo(Transform t, double x1, double y1, double x2, double y2, double bulge)
			{
				double dx = x2 - x1, dy = y2 - y1;
				double chord = Math.Sqrt(dx * dx + dy * dy);
				if (chord < 1e-12)
					return;

				double angle = 4 * Math.Atan(bulge);
				double radius = chord / (2 * Math.Sin(Math.Abs(angle) / 2));
				double offset = chord / 2 / Math.Tan(angle / 2); // signed distance of the center from the chord, on the left side
				double cx = (x1 + x2) / 2 - dy / chord * offset;
				double cy = (y1 + y2) / 2 + dx / chord * offset;
				ArcTo(t, cx, cy, radius, Math.Atan2(y1 - cy, x1 - cx), angle, false);
			}

			private void DrawCircle(Transform t, double cx, double cy, double r, string color)
			{
				if (r <= 0) return;
				// with a non uniform scale in a block it becomes an ellipse, approximated with lines by ArcTo
				BeginPath();
				ArcTo(t, cx, cy, r, 0, 2 * Math.PI, true);
				EndPath(color, true);
			}

			private void DrawEllipse(DxfEntity e, Transform t, string color)
			{
				double cx = e.GetDouble(10, 0), cy = e.GetDouble(20, 0);
				double mx = e.GetDouble(11, 0), my = e.GetDouble(21, 0);
				double ratio = e.GetDouble(40, 1);
				double nz = e.GetDouble(230, 1) < 0 ? -1 : 1;
				// minor axis = ratio * (extrusion x major axis)
				double nx = -nz * my * ratio, ny = nz * mx * ratio;

				double p1 = e.GetDouble(41, 0), p2 = e.GetDouble(42, 2 * Math.PI);
				double sweep = p2 - p1;
				while (sweep <= 1e-9) sweep += 2 * Math.PI;
				while (sweep > 2 * Math.PI + 1e-9) sweep -= 2 * Math.PI;
				bool full = Math.Abs(sweep - 2 * Math.PI) < 1e-6;

				double major = Math.Sqrt(mx * mx + my * my);
				int segments = Segments(major, sweep);

				BeginPath();
				for (int i = 0; i <= segments; i++)
				{
					if (full && i == segments) break; // closed with Z
					double a = p1 + sweep * i / segments;
					double x = cx + Math.Cos(a) * mx + Math.Sin(a) * nx;
					double y = cy + Math.Cos(a) * my + Math.Sin(a) * ny;
					if (i == 0) MoveTo(t, x, y); else LineTo(t, x, y);
				}
				EndPath(color, full);
			}

			private void DrawSpline(DxfEntity e, Transform t, string color)
			{
				int degree = e.GetInt(71, 3);
				List<double> knots = e.GetAll(40);
				List<double> weights = e.GetAll(41);
				List<double> xs = e.GetAll(10), ys = e.GetAll(20);
				bool closed = (e.GetInt(70, 0) & 1) != 0;

				List<double[]> points = new List<double[]>();
				if (xs.Count > degree && ys.Count == xs.Count && knots.Count == xs.Count + degree + 1)
				{
					if (weights.Count != xs.Count)
						weights = Enumerable.Repeat(1.0, xs.Count).ToList();
					points = EvaluateSpline(degree, knots, weights, xs, ys, t.Scale);
				}
				else
				{
					// no usable control points: go through the fit points
					List<double> fx = e.GetAll(11), fy = e.GetAll(21);
					for (int i = 0; i < fx.Count && i < fy.Count; i++)
						points.Add(new double[] { fx[i], fy[i] });
				}

				if (points.Count < 2)
				{
					Ignore("SPLINE (invalid)");
					return;
				}

				BeginPath();
				MoveTo(t, points[0][0], points[0][1]);
				if (mSplineTolerance > 0)
				{
					// fitted on the final coordinates: correct also inside scaled or rotated blocks
					List<Point> drawing = points.Select(q => ToDrawing(t, q[0], q[1])).ToList();
					mPath.Segments.AddRange(ArcFitter.Fit(drawing, mSplineTolerance));
				}
				else
				{
					for (int i = 1; i < points.Count; i++)
						LineTo(t, points[i][0], points[i][1]);
				}
				EndPath(color, closed);
			}

			// sample the (rational) b-spline span by span, with about SplineStep mm segments
			private List<double[]> EvaluateSpline(int p, List<double> knots, List<double> weights, List<double> xs, List<double> ys, double scale)
			{
				List<double[]> rv = new List<double[]>();
				int n = xs.Count;
				for (int span = p; span < n; span++)
				{
					double u0 = knots[span], u1 = knots[span + 1];
					if (u1 - u0 <= 1e-12)
						continue;

					// the curve in this span is inside the control polygon of the p+1 points that define it
					double len = 0;
					for (int k = span - p; k < span; k++)
						len += Math.Sqrt(Math.Pow(xs[k + 1] - xs[k], 2) + Math.Pow(ys[k + 1] - ys[k], 2));
					int segments = Math.Min(10000, Math.Max(4, (int)Math.Ceiling(len * scale * mDoc.UnitToMM / SplineStep)));

					for (int i = rv.Count == 0 ? 0 : 1; i <= segments; i++)
						rv.Add(DeBoor(p, knots, weights, xs, ys, span, u0 + (u1 - u0) * i / segments));
				}
				return rv;
			}

			private static double[] DeBoor(int p, List<double> knots, List<double> weights, List<double> xs, List<double> ys, int span, double u)
			{
				// homogeneous coordinates: x*w, y*w, w
				double[,] d = new double[p + 1, 3];
				for (int j = 0; j <= p; j++)
				{
					int i = j + span - p;
					d[j, 0] = xs[i] * weights[i];
					d[j, 1] = ys[i] * weights[i];
					d[j, 2] = weights[i];
				}
				for (int r = 1; r <= p; r++)
				{
					for (int j = p; j >= r; j--)
					{
						int i = j + span - p;
						double den = knots[i + p - r + 1] - knots[i];
						double alpha = den == 0 ? 0 : (u - knots[i]) / den;
						for (int k = 0; k < 3; k++)
							d[j, k] = (1 - alpha) * d[j - 1, k] + alpha * d[j, k];
					}
				}
				return new double[] { d[p, 0] / d[p, 2], d[p, 1] / d[p, 2] };
			}

			#endregion

			#region Path output

			private void BeginPath()
			{ mPath = null; }

			private void EndPath(string color, bool closed)
			{
				if (mPath == null || mPath.Segments.Count == 0)
					return;
				if (closed && VectorSegment.Distance(mPath.End, mPath.Start) > 1e-9)
					mPath.Segments.Add(VectorSegment.Line(mPath.Start));
				mPath.Color = color;
				mPath.Closed = closed;
				mDrawing.Paths.Add(mPath);
				mPath = null;
				mCount++;
			}

			// from local coordinates to the drawing: block transformations, then units to mm
			private Point ToDrawing(Transform t, double x, double y)
			{
				double[] p = t.Apply(x, y);
				return new Point(p[0] * mDoc.UnitToMM, p[1] * mDoc.UnitToMM);
			}

			private void MoveTo(Transform t, double x, double y)
			{ mPath = new VectorPath(null, ToDrawing(t, x, y)); }

			private void LineTo(Transform t, double x, double y)
			{ mPath.Segments.Add(VectorSegment.Line(ToDrawing(t, x, y))); }

			// circular arc in local coordinates, sweep in radians (positive = counterclockwise)
			private void ArcTo(Transform t, double cx, double cy, double r, double start, double sweep, bool move)
			{
				if (r <= 0) return;
				if (move)
					MoveTo(t, cx + r * Math.Cos(start), cy + r * Math.Sin(start));

				if (t.IsSimilarity)
				{
					// a true arc (mirrored blocks invert the direction); a full circle in two halves, start and end must differ
					bool ccw = (sweep > 0) ^ (t.Determinant < 0);
					Point center = ToDrawing(t, cx, cy);
					int pieces = Math.Abs(sweep) > 2 * Math.PI - 1e-9 ? 2 : 1;
					for (int i = 1; i <= pieces; i++)
					{
						double a = start + sweep * i / pieces;
						mPath.Segments.Add(VectorSegment.Arc(ToDrawing(t, cx + r * Math.Cos(a), cy + r * Math.Sin(a)), center, ccw));
					}
				}
				else
				{
					int segments = Segments(r, Math.Abs(sweep));
					for (int i = 1; i <= segments; i++)
					{
						double a = start + sweep * i / segments;
						LineTo(t, cx + r * Math.Cos(a), cy + r * Math.Sin(a));
					}
				}
			}

			// number of segments needed to approximate an arc of the given radius (drawing units) within the arc tolerance
			private int Segments(double radius, double sweep)
			{
				double step = radius > mTolerance ? 2 * Math.Acos(1 - mTolerance / radius) : Math.PI / 4;
				return Math.Max(4, Math.Min(10000, (int)Math.Ceiling(sweep / Math.Min(step, Math.PI / 4))));
			}

			#endregion
		}

		#endregion
	}
}
