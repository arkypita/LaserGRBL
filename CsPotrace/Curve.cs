using System;


//Copyright (C) 2001-2016 Peter Selinger
//Copyright (C) 2009-2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
// You should have received a copy of the GNU General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. 

namespace CsPotrace
#region auxiliary classes
{
    /// <summary>
    /// Holds the information about der produced curves
    /// </summary>
    /// 
    public struct Curve
	{
		/// <summary>
		/// Bezier or Line
		/// </summary>
		public CurveKind Kind;
		/// <summary>
		/// Startpoint
		/// </summary>
		public dPoint A;
		/// <summary>
		/// ControlPoint
		/// </summary>
		public dPoint ControlPointA;
		/// <summary>
		/// ControlPoint
		/// </summary>
		public dPoint ControlPointB;
		/// <summary>
		/// Endpoint
		/// </summary>
		public dPoint B;
		/// <summary>
		/// Creates a curve
		/// </summary>
		/// <param name="Kind"></param>
		/// <param name="A">Startpoint</param>
		/// <param name="ControlPointA">Controlpoint</param>
		/// <param name="ControlPointB">Controlpoint</param>
		/// <param name="B">Endpoint</param>
		public Curve(CurveKind Kind, dPoint A, dPoint ControlPointA, dPoint ControlPointB, dPoint B)
		{

			this.Kind = Kind;
			this.A = A;
			this.B = B;
			this.ControlPointA = ControlPointA;
			this.ControlPointB = ControlPointB;

		}

		public double LinearLenght
		{
			get
			{
				double dX = B.X - A.X;
				double dY = B.Y - A.Y;
				return Math.Sqrt(dX * dX + dY * dY);
			}
		}
	}

}