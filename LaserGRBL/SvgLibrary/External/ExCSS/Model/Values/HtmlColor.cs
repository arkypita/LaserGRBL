using System;
using System.Runtime.InteropServices;
using ExCSS.Model;
using ExCSS.Model.Extensions;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class HtmlColor : Term, IEquatable<HtmlColor>
    {
        public byte A;
        public byte R;
        public byte G;
        public byte B;

        public HtmlColor(byte r, byte g, byte b)
        {
            A = 255;
            R = r;
            B = b;
            G = g;
        }

        public HtmlColor(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            B = b;
            G = g;
        }

        public HtmlColor(Double a, byte r, byte g, byte b)
        {
            A = (byte)Math.Max(Math.Min(Math.Ceiling(255 * a), 255), 0);
            R = r;
            B = b;
            G = g;
        }

        public static HtmlColor FromRgba(byte r, byte g, byte b, Single a)
        {
            return new HtmlColor(a, r, g, b);
        }

        public static HtmlColor FromRgba(byte r, byte g, byte b, Double a)
        {
            return new HtmlColor(a, r, g, b);
        }

        public static HtmlColor FromRgb(byte r, byte g, byte b)
        {
            return new HtmlColor(r, g, b);
        }

        public static HtmlColor FromHsl(Single h, Single s, Single l)
        {
            const Single third = 1f / 3f;

            var m2 = l <= 0.5f ? (l * (s + 1f)) : (l + s - l * s);
            var m1 = 2f * l - m2;
            var r = (Byte)Math.Round(255 * HueToRgb(m1, m2, h + third));
            var g = (Byte)Math.Round(255 * HueToRgb(m1, m2, h));
            var b = (Byte)Math.Round(255 * HueToRgb(m1, m2, h - third));
            return new HtmlColor(r, g, b);
        }

        public static HtmlColor FromHex(string color)
        {
            if (color.Length == 3)
            {
                var r = color[0].FromHex();
                r += r * 16;

                var g = color[1].FromHex();
                g += g * 16;
                
                var b = color[2].FromHex();
                b += b * 16;

                return new HtmlColor((byte)r, (byte)g, (byte)b);
            }
           
            if (color.Length == 6)
            {
                var r = 16 * color[0].FromHex();
                var g = 16 * color[2].FromHex();
                var b = 16 * color[4].FromHex();
                
                r += color[1].FromHex();
                g += color[3].FromHex();
                b += color[5].FromHex();

                return new HtmlColor((byte)r, (byte)g, (byte)b);
            }

            throw new ArgumentException("Invalid color code length: " + color, "color");
        }

        public static bool TryFromHex(string color, out HtmlColor htmlColor)
        {
            htmlColor = new HtmlColor(255, 0, 0, 0);

            if (color.Length == 3)
            {
                if (!color[0].IsHex() || !color[1].IsHex() || !color[2].IsHex())
                {
                    return false;
                }

                var r = color[0].FromHex();
                r += r * 16;
                
                var g = color[1].FromHex();
                g += g * 16;
                
                var b = color[2].FromHex();
                b += b * 16;

                htmlColor.R = (byte)r;
                htmlColor.G = (byte)g;
                htmlColor.B = (byte)b;
                
                return true;
            }
            
            if (color.Length == 6)
            {
                if (!color[0].IsHex() || !color[1].IsHex() || !color[2].IsHex() ||
                    !color[3].IsHex() || !color[4].IsHex() || !color[5].IsHex())
                {
                    return false;
                }

                var r = 16 * color[0].FromHex();
                var g = 16 * color[2].FromHex();
                var b = 16 * color[4].FromHex();
                
                r += color[1].FromHex();
                g += color[3].FromHex();
                b += color[5].FromHex();

                htmlColor.R = (byte)r;
                htmlColor.G = (byte)g;
                htmlColor.B = (byte)b;
                
                return true;
            }

            return false;
        }

        public double Alpha
        {
            get { return A / 255.0; }
        }

        public static bool operator ==(HtmlColor a, HtmlColor b)
        {
            return a.GetHashCode() == b.GetHashCode();
        }

        public static bool operator !=(HtmlColor a, HtmlColor b)
        {
            return a.GetHashCode() != b.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            if (obj is HtmlColor)
            {
                return Equals((HtmlColor)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return unchecked(A + (R << 8) + (G << 16) + (B << 24));
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool friendlyFormat, int indentation = 0)
        {
            return ToCss().Indent(friendlyFormat, indentation);
        }

        public string ToString(bool forceLong, bool friendlyFormat, int indentation = 0)
        {
            return ToCss(forceLong).Indent(friendlyFormat, indentation);
        }

        /// <summary>
        /// Return the shortest form possible
        /// </summary>
        string ToCss(bool forceLong = false)
        {
            if (A == 255 && !forceLong && ((R >> 4) == (R & 0x0F)) && ((G >> 4) == (G & 0x0F)) && ((B >> 4) == (B & 0x0F)))
                return "#" + R.ToHexChar() + G.ToHexChar() + B.ToHexChar();

            if (A == 255)
            {
                return "#" + R.ToHex() + G.ToHex() + B.ToHex();
                //return "rgb(" + R + ", " + G + ", " + B + ")";
            }

            return "rgba(" + R + ", " + G + ", " + B + ", " + Alpha.ToString("0.##") + ")";
        }

        public bool Equals(HtmlColor other)
        {
            HtmlColor o = other as HtmlColor;
            if (o == null)
                return false;
            return GetHashCode() == other.GetHashCode();
        }

        private static Single HueToRgb(Single m1, Single m2, Single h)
        {
            const Single sixth = 1f / 6f;
            const Single third2 = 2f / 3f;

            if (h < 0f)
            {
                h += 1f;
            }
            else if (h > 1f)
            {
                h -= 1f;
            }

            if (h < sixth)
            {
                return m1 + (m2 - m1) * h * 6f;
            }
            if (h < 0.5)
            {
                return m2;
            }
            if (h < third2)
            {
                return m1 + (m2 - m1) * (third2 - h) * 6f;
            }

            return m1;
        }
    }
}