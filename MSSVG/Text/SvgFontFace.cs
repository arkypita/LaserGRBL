using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    [SvgElement("font-face")]
    public class SvgFontFace : SvgElement
    {
        [SvgAttribute("alphabetic")]
        public float Alphabetic
        {
            get { return (this.Attributes["alphabetic"] == null ? 0 : (float)this.Attributes["alphabetic"]); }
            set { this.Attributes["alphabetic"] = value; }
        }

        [SvgAttribute("ascent")]
        public float Ascent
        {
            get 
            { 
                if (this.Attributes["ascent"] == null) 
                {
                    var font = this.Parent as SvgFont;
                    return (font == null ? 0 : this.UnitsPerEm - font.VertOriginY);
                }
                else
                {
                    return (float)this.Attributes["ascent"];
                }
            }
            set { this.Attributes["ascent"] = value; }
        }

        [SvgAttribute("ascent-height")]
        public float AscentHeight
        {
            get { return (this.Attributes["ascent-height"] == null ? this.Ascent : (float)this.Attributes["ascent-height"]); }
            set { this.Attributes["ascent-height"] = value; }
        }

        [SvgAttribute("descent")]
        public float Descent
        {
            get 
            { 
                if (this.Attributes["descent"] == null) 
                {
                    var font = this.Parent as SvgFont;
                    return (font == null ? 0 : font.VertOriginY);
                }
                else 
                {
                    return (float)this.Attributes["descent"];
                }
            }
            set { this.Attributes["descent"] = value; }
        }

        /// <summary>
        /// Indicates which font family is to be used to render the text.
        /// </summary>
        [SvgAttribute("font-family")]
        public override string FontFamily
        {
            get { return this.Attributes["font-family"] as string; }
            set { this.Attributes["font-family"] = value; }
        }

        /// <summary>
        /// Refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.
        /// </summary>
        [SvgAttribute("font-size")]
        public override SvgUnit FontSize
        {
            get { return (this.Attributes["font-size"] == null) ? SvgUnit.Empty : (SvgUnit)this.Attributes["font-size"]; }
            set { this.Attributes["font-size"] = value; }
        }

        /// <summary>
        /// Refers to the style of the font.
        /// </summary>
        [SvgAttribute("font-style")]
        public override SvgFontStyle FontStyle
        {
            get { return (this.Attributes["font-style"] == null) ? SvgFontStyle.All : (SvgFontStyle)this.Attributes["font-style"]; }
            set { this.Attributes["font-style"] = value; }
        }

        /// <summary>
        /// Refers to the varient of the font.
        /// </summary>
        [SvgAttribute("font-variant")]
        public override SvgFontVariant FontVariant
        {
            get { return (this.Attributes["font-variant"] == null) ? SvgFontVariant.Inherit : (SvgFontVariant)this.Attributes["font-variant"]; }
            set { this.Attributes["font-variant"] = value; }
        }

        /// <summary>
        /// Refers to the boldness of the font.
        /// </summary>
        [SvgAttribute("font-weight")]
        public override SvgFontWeight FontWeight
        {
            get { return (this.Attributes["font-weight"] == null) ? SvgFontWeight.Inherit : (SvgFontWeight)this.Attributes["font-weight"]; }
            set { this.Attributes["font-weight"] = value; }
        }

        [SvgAttribute("panose-1")]
        public string Panose1
        {
            get { return this.Attributes["panose-1"] as string; }
            set { this.Attributes["panose-1"] = value; }
        }

        [SvgAttribute("units-per-em")]
        public float UnitsPerEm
        {
            get { return (this.Attributes["units-per-em"] == null ? 1000 : (float)this.Attributes["units-per-em"]); }
            set { this.Attributes["units-per-em"] = value; }
        }

        [SvgAttribute("x-height")]
        public float XHeight
        {
            get { return (this.Attributes["x-height"] == null ? float.MinValue : (float)this.Attributes["x-height"]); }
            set { this.Attributes["x-height"] = value; }
        }


        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgFontFace>();
        }
    }
}
