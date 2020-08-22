using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LaserGRBL.Lib.SvgConverter
{
	//
	// System.Windows.Media.Matrix struct
	//
	// Contact:
	//   Moonlight List (moonlight-list@lists.ximian.com)
	//
	// Copyright (C) 2007 Novell, Inc (http://www.novell.com)
	//
	// Permission is hereby granted, free of charge, to any person obtaining
	// a copy of this software and associated documentation files (the
	// "Software"), to deal in the Software without restriction, including
	// without limitation the rights to use, copy, modify, merge, publish,
	// distribute, sublicense, and/or sell copies of the Software, and to
	// permit persons to whom the Software is furnished to do so, subject to
	// the following conditions:
	//
	// The above copyright notice and this permission notice shall be
	// included in all copies or substantial portions of the Software.
	//
	// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
	// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
	// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
	// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
	// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
	// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
	// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
	//

	public struct Matrix
	{

		private float m_11;
		private float m_12;
		private float m_21;
		private float m_22;
		private float offset_x;
		private float offset_y;
		// we can't have an empty ctor for a struct (CS0568) but the default matrix is identity
		private bool init;

		public Matrix(float m11, float m12, float m21, float m22, float offsetX, float offsetY)
		{
			m_11 = m11;
			m_12 = m12;
			m_21 = m21;
			m_22 = m22;
			offset_x = offsetX;
			offset_y = offsetY;
			init = true;
		}

		public float M11
		{
			get
			{
				if (!init)
					SetIdentity();
				return m_11;
			}
			set
			{
				if (!init)
					SetIdentity();
				m_11 = value;
			}
		}

		public float M12
		{
			get
			{
				if (!init)
					SetIdentity();
				return m_12;
			}
			set
			{
				if (!init)
					SetIdentity();
				m_12 = value;
			}
		}

		public float M21
		{
			get
			{
				if (!init)
					SetIdentity();
				return m_21;
			}
			set
			{
				if (!init)
					SetIdentity();
				m_21 = value;
			}
		}

		public float M22
		{
			get
			{
				if (!init)
					SetIdentity();
				return m_22;
			}
			set
			{
				if (!init)
					SetIdentity();
				m_22 = value;
			}
		}

		public float OffsetX
		{
			get
			{
				if (!init)
					SetIdentity();
				return offset_x;
			}
			set
			{
				if (!init)
					SetIdentity();
				offset_x = value;
			}
		}

		public float OffsetY
		{
			get
			{
				if (!init)
					SetIdentity();
				return offset_y;
			}
			set
			{
				if (!init)
					SetIdentity();
				offset_y = value;
			}
		}

		public bool IsIdentity
		{
			get
			{
				if (!init)
					return true;

				return ((m_11 == 1.0) && (m_12 == 0.0) && (m_21 == 0.0) && (m_22 == 1.0) &&
					(offset_x == 0.0) && (offset_y == 0.0));
			}
		}


		public void SetIdentity()
		{
			m_11 = 1.0F;
			m_12 = 0.0F;
			m_21 = 0.0F;
			m_22 = 1.0F;
			offset_x = 0.0F;
			offset_y = 0.0F;
			init = true;
		}


		public override int GetHashCode()
		{
			if (IsIdentity)
				return 0;

			return m_11.GetHashCode() ^ m_12.GetHashCode() ^ m_21.GetHashCode() ^ m_22.GetHashCode() ^
				offset_x.GetHashCode() ^ offset_y.GetHashCode();
		}

		public PointF Transform(PointF point)
		{
			if (!init)
				return point;

			float x = (m_11 * point.X) + (m_21 * point.Y) + offset_x;
			float y = (m_22 * point.Y) + (m_12 * point.X) + offset_y;

			return new PointF(x, y);
		}

		public override string ToString()
		{
			if (IsIdentity)
				return "Identity";
			return String.Format("{0},{1},{2},{3},{4},{5}", m_11, m_12, m_21, m_22, offset_x, offset_y);
		}

		public override bool Equals(object o)
		{
			if ((o == null) || (!(o is Matrix)))
				return false;
			return Equals((Matrix)o);
		}

		public bool Equals(Matrix value)
		{
			return (this == value);
		}

		public static bool operator ==(Matrix matrix1, Matrix matrix2)
		{
			if (!matrix1.init)
			{
				if (!matrix2.init)
					return true;
				matrix1.SetIdentity();
			}
			if (!matrix2.init)
			{
				matrix2.SetIdentity();
			}

			return ((matrix1.m_11 == matrix2.m_11) && (matrix1.m_12 == matrix2.m_12) &&
				(matrix1.m_21 == matrix2.m_21) && (matrix1.m_22 == matrix2.m_22) &&
				(matrix1.offset_x == matrix2.offset_x) && (matrix1.offset_y == matrix2.offset_y));
		}

		public static bool operator !=(Matrix matrix1, Matrix matrix2)
		{
			return !(matrix1 == matrix2);
		}

		public static Matrix Identity
		{
			get { return new Matrix(); }
		}
		public void Append(Matrix matrix)
		{
			float _m11;
			float _m21;
			float _m12;
			float _m22;
			float _offsetX;
			float _offsetY;

			_m11 = this.m_11 * matrix.M11 + this.m_12 * matrix.M21;
			_m12 = this.m_11 * matrix.M12 + this.m_12 * matrix.M22;
			_m21 = this.m_21 * matrix.M11 + this.m_22 * matrix.M21;
			_m22 = this.m_21 * matrix.M12 + this.m_22 * matrix.M22;

			_offsetX = this.offset_x * matrix.M11 + this.offset_y * matrix.M21 + matrix.OffsetX;
			_offsetY = this.offset_x * matrix.M12 + this.offset_y * matrix.M22 + matrix.OffsetY;

			this.m_11 = _m11;
			this.m_12 = _m12;
			this.m_21 = _m21;
			this.m_22 = _m22;
			this.offset_x = _offsetX;
			this.offset_y = _offsetY;
		}
		public void Scale(float scaleX, float scaleY)
		{
			Matrix scale = new Matrix(scaleX, 0, 0, scaleY, 0, 0);
			Append(scale);
		}
		public void Translate(float offsetX, float offsetY)
		{
			this.offset_x += offsetX;
			this.offset_y += offsetY;
		}
		public void Rotate(float angle)
		{
			// R_theta==[costheta -sintheta; sintheta costheta],	
			double theta = angle * Math.PI / 180;
			Matrix r_theta = new Matrix((float)Math.Cos(theta), (float)Math.Sin(theta),
										(float)-Math.Sin(theta), (float)Math.Cos(theta),
										0, 0);

			Append(r_theta);
		}
		public void RotateAt(float angle, float centerX, float centerY)
		{
			Translate(-centerX, -centerY);
			Rotate(angle);
			Translate(centerX, centerY);
		}

		public static Matrix Multiply(Matrix trans1, Matrix trans2)
		{
			Matrix m = trans1;
			m.Append(trans2);
			return m;
		}
	}

}
