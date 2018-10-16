using Svg.DataTypes;
using System.ComponentModel;
using System;

namespace Svg
{
    /// <summary>
    /// Represents an orientation in an Scalable Vector Graphics document.
    /// </summary>
	[TypeConverter(typeof(SvgOrientConverter))]
	public class SvgOrient
    {
        private bool _isAuto = true;
        private float _angle;
  
		public SvgOrient()
		{
			IsAuto = true;
		}

		public SvgOrient(float angle)
		{
			Angle = angle;
		}

        /// <summary>
        /// Gets the value of the unit.
        /// </summary>
        public float Angle
        {
            get { return this._angle; }
            set
            {
            	this._angle = value;
            	this._isAuto = false;
            }
        }

		
        /// <summary>
        /// Gets the value of the unit.
        /// </summary>
        public bool IsAuto
        {
            get { return this._isAuto; }
            set { 
				this._isAuto = value;
            	this._angle = 0f;
			}
        }


        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj.GetType() == typeof (SvgOrient))) return false;

            var unit = (SvgOrient)obj;
            return (unit.IsAuto == this.IsAuto && unit.Angle == this.Angle);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string type = string.Empty;

			if (this.IsAuto)
				return "auto";
			else
				return this.Angle.ToString();
        }

		///// <summary>
		///// Performs an implicit conversion from <see cref="Svg.SvgUnit"/> to <see cref="System.Single"/>.
		///// </summary>
		///// <param name="value">The value.</param>
		///// <returns>The result of the conversion.</returns>
		//public static implicit operator float(SvgOrient value)
		//{
		//    return this.Angle;
		//}

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Single"/> to <see cref="Svg.SvgOrient"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SvgOrient(float value)
        {
            return new SvgOrient(value);
        }

    }

}
