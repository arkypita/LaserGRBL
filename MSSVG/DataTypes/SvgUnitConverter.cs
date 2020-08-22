using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Svg
{
    public sealed class SvgUnitConverter : TypeConverter
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

            // http://www.w3.org/TR/CSS21/syndata.html#values
            // http://www.w3.org/TR/SVG11/coords.html#Units

            string unit = (string)value;
            int identifierIndex = -1;

            if (unit == "none")
                return SvgUnit.None;

            for (int i = 0; i < unit.Length; i++)
            {
                // If the character is a percent sign or a letter which is not an exponent 'e'
                if (unit[i] == '%' || (char.IsLetter(unit[i]) && !((unit[i] == 'e' || unit[i] == 'E') && i < unit.Length - 1 && !char.IsLetter(unit[i + 1]))))
                {
                    identifierIndex = i;
                    break;
                }
            }

            float val = 0.0f;
            float.TryParse((identifierIndex > -1) ? unit.Substring(0, identifierIndex) : unit, NumberStyles.Float, CultureInfo.InvariantCulture, out val);

            if (identifierIndex == -1)
            {
                return new SvgUnit(val);
            }

            switch (unit.Substring(identifierIndex).Trim().ToLower())
            {
                case "mm":
                    return new SvgUnit(SvgUnitType.Millimeter, val);
                case "cm":
                    return new SvgUnit(SvgUnitType.Centimeter, val);
                case "in":
                    return new SvgUnit(SvgUnitType.Inch, val);
                case "px":
                    return new SvgUnit(SvgUnitType.Pixel, val);
                case "pt":
                    return new SvgUnit(SvgUnitType.Point, val);
                case "pc":
                    return new SvgUnit(SvgUnitType.Pica, val);
                case "%":
                    return new SvgUnit(SvgUnitType.Percentage, val);
                case "em":
                    return new SvgUnit(SvgUnitType.Em, val);
                case "ex":
                    return new SvgUnit(SvgUnitType.Ex, val);
                default:
                    throw new FormatException("Unit is in an invalid format '" + unit + "'.");
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
            if (destinationType == typeof(string))
            {
                return ((SvgUnit)value).ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}