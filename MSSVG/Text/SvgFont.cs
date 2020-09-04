using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    [SvgElement("font")]
    public class SvgFont : SvgElement
    {
        [SvgAttribute("horiz-adv-x")]
        public float HorizAdvX
        {
            get { return (this.Attributes["horiz-adv-x"] == null ? 0 : (float)this.Attributes["horiz-adv-x"]); }
            set { this.Attributes["horiz-adv-x"] = value; }
        }
        [SvgAttribute("horiz-origin-x")]
        public float HorizOriginX
        {
            get { return (this.Attributes["horiz-origin-x"] == null ? 0 : (float)this.Attributes["horiz-origin-x"]); }
            set { this.Attributes["horiz-origin-x"] = value; }
        }
        [SvgAttribute("horiz-origin-y")]
        public float HorizOriginY
        {
            get { return (this.Attributes["horiz-origin-y"] == null ? 0 : (float)this.Attributes["horiz-origin-y"]); }
            set { this.Attributes["horiz-origin-y"] = value; }
        }
        [SvgAttribute("vert-adv-y")]
        public float VertAdvY
        {
            get { return (this.Attributes["vert-adv-y"] == null ? this.Children.OfType<SvgFontFace>().First().UnitsPerEm : (float)this.Attributes["vert-adv-y"]); }
            set { this.Attributes["vert-adv-y"] = value; }
        }
        [SvgAttribute("vert-origin-x")]
        public float VertOriginX
        {
            get { return (this.Attributes["vert-origin-x"] == null ? this.HorizAdvX / 2 : (float)this.Attributes["vert-origin-x"]); }
            set { this.Attributes["vert-origin-x"] = value; }
        }
        [SvgAttribute("vert-origin-y")]
        public float VertOriginY
        {
            get { return (this.Attributes["vert-origin-y"] == null ? 
                          (this.Children.OfType<SvgFontFace>().First().Attributes["ascent"] == null ? 0 : this.Children.OfType<SvgFontFace>().First().Ascent) : 
                          (float)this.Attributes["vert-origin-y"]); }
            set { this.Attributes["vert-origin-y"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgFont>();
        }
    }
}
