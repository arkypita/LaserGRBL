//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System.Drawing;

namespace LaserGRBL
{
    public struct GPoint
	{
		public float X, Y, Z;

		public GPoint(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static GPoint Zero { get { return new GPoint(); } }

		public static bool operator ==(GPoint a, GPoint b)
		{ return a.X == b.X && a.Y == b.Y && a.Z == b.Z; }

		public static bool operator !=(GPoint a, GPoint b)
		{ return !(a == b); }

		public static GPoint operator -(GPoint a, GPoint b)
		{ return new GPoint(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }

		public static GPoint operator +(GPoint a, GPoint b)
		{ return new GPoint(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }

		public override bool Equals(object obj)
		{
			return obj is GPoint && ((GPoint)obj) == this;
		}

		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;
				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				hash = hash * 23 + Z.GetHashCode();
				return hash;
			}
		}

		public PointF ToPointF()
		{
			return new PointF(X, Y);
		}
	}
}
