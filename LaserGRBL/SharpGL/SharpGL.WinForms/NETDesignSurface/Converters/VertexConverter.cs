using SharpGL.SceneGraph;
using System;
using System.ComponentModel;
using System.Globalization;

namespace SharpGL.WinForms.NETDesignSurface.Converters
{
	/// <summary>
	/// The VertexConverter class allows you to edit vertices in the propties window.
	/// </summary>
	public class VertexConverter : ExpandableObjectConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
		{
			//	We allow conversion from a string.
			if (t == typeof(string))
				return true;

			return base.CanConvertFrom(context, t);
		}

		public override object ConvertFrom(ITypeDescriptorContext context,
			CultureInfo info, object value)
		{
			//	If it's a string, we'll parse it for coords.
			if (value is string)
			{
				try
				{
					string s = (string)value;

					//	Parse the format (x, y, z).
					int openbracket = s.IndexOf('(');
					int comma = s.IndexOf(',');
					int nextcomma = s.IndexOf(',', comma + 1);
					int closebracket = s.IndexOf(')');

					float xValue, yValue, zValue;

					if (comma != -1 && openbracket != -1)
					{
						//	We have the comma and open bracket, so get x.
						string parsed = s.Substring(openbracket + 1, comma - (openbracket + 1)).Trim();
						xValue = float.Parse(parsed, CultureInfo.InvariantCulture);

						if (comma != -1 && nextcomma != -1)
						{
							parsed = s.Substring(comma + 1, nextcomma - (comma + 1)).Trim();
							yValue = float.Parse(parsed, CultureInfo.InvariantCulture);

							if (nextcomma != -1 && closebracket != -1)
							{
								parsed = s.Substring(nextcomma + 1, closebracket - (nextcomma + 1)).Trim();
								zValue = float.Parse(parsed, CultureInfo.InvariantCulture);

								return new Vertex(xValue, yValue, zValue);
							}
						}
					}
				}
				catch { }
				//	Somehow we couldn't parse it.
				throw new ArgumentException("Can not convert '" + (string)value +
					"' to type Vertex");

			}

			return base.ConvertFrom(context, info, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
			object value, Type destType)
		{
			if (destType == typeof(string) && value is Vertex)
			{
				//	We can easily convert a vertex to a string, format (x, y, z).
				Vertex v = (Vertex)value;

				return "(" + v.X + ", " + v.Y + ", " + v.Z + ")";
			}

			return base.ConvertTo(context, culture, value, destType);
		}
	}
}