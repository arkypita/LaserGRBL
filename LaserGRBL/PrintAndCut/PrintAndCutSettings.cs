// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA.

using System.Drawing;

namespace LaserGRBL.PrintAndCut
{
	/// <summary>Persisted Print and Cut settings.</summary>
	public static class PrintAndCutSettings
	{
		/// <summary>Default color of the registration marks: magenta.</summary>
		public const string DefaultReservedColor = "#FF00FF";

		private const string ReservedColorKey = "PrintAndCut.ReservedColor";

		/// <summary>
		/// Color reserved for registration marks in imported SVG files. Elements of this color
		/// are never cut nor engraved. Color.Empty if the setting is invalid.
		/// </summary>
		public static Color ReservedColor
		{
			get
			{
				try { return ColorTranslator.FromHtml(Settings.GetObject(ReservedColorKey, DefaultReservedColor)); }
				catch { return ColorTranslator.FromHtml(DefaultReservedColor); }
			}
			set { Settings.SetObject(ReservedColorKey, ColorTranslator.ToHtml(Color.FromArgb(value.R, value.G, value.B))); }
		}
	}
}
