using System;
using System.ComponentModel;
using System.Globalization;

namespace Svg.DataTypes
{
	public sealed class SvgOrientConverter : TypeConverter
	{
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value == null)
			{
				return new SvgUnit(SvgUnitType.User, 0.0f);
			}

			if (!(value is string))
			{
				throw new ArgumentOutOfRangeException("value must be a string.");
			}

			switch (value.ToString())
			{
				case "auto":
					return (new SvgOrient());
				default:
					float fTmp = float.MinValue;
					if(!float.TryParse(value.ToString(), out fTmp))
						throw new ArgumentOutOfRangeException("value must be a valid float.");
					return (new SvgOrient(fTmp));
			}
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}

			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
