using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Svg.DataTypes;
using System.Text.RegularExpressions;
using System.Linq;

namespace Svg
{
    public partial class SvgElement
    {
        private bool _dirty;
        
        /// <summary>
        /// Gets or sets a value indicating whether this element's <see cref="Path"/> is dirty.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the path is dirty; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool IsPathDirty
        {
            get { return this._dirty; }
            set { this._dirty = value; }
        }

        /// <summary>
        /// Force recreation of the paths for the element and it's children.
        /// </summary>
        public void InvalidateChildPaths()
        {
            this.IsPathDirty = true;
            foreach (SvgElement element in this.Children)
            {
                element.InvalidateChildPaths();
            }
        }

        protected static float FixOpacityValue(float value)
        {
            const float max = 1.0f;
            const float min = 0.0f;
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Gets or sets the fill <see cref="SvgPaintServer"/> of this element.
        /// </summary>
        [SvgAttribute("fill", true)]
        public virtual SvgPaintServer Fill
        {
            get { return ((SvgPaintServer)this.Attributes["fill"] ?? SvgColourServer.NotSet); }
            set { this.Attributes["fill"] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="SvgPaintServer"/> to be used when rendering a stroke around this element.
        /// </summary>
        [SvgAttribute("stroke", true)]
        public virtual SvgPaintServer Stroke
        {
            get { return (SvgPaintServer)this.Attributes["stroke"]; }
            set { this.Attributes["stroke"] = value; }
        }

        [SvgAttribute("fill-rule", true)]
        public virtual SvgFillRule FillRule
        {
            get { return (SvgFillRule)(this.Attributes["fill-rule"] ?? SvgFillRule.NonZero); }
            set { this.Attributes["fill-rule"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of this element's <see cref="Fill"/>.
        /// </summary>
        [SvgAttribute("fill-opacity", true)]
        public virtual float FillOpacity
        {
            get { return (float)(this.Attributes["fill-opacity"] ?? 1.0f); }
            set { this.Attributes["fill-opacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Gets or sets the width of the stroke (if the <see cref="Stroke"/> property has a valid value specified.
        /// </summary>
        [SvgAttribute("stroke-width", true)]
        public virtual SvgUnit StrokeWidth
        {
            get { return (SvgUnit)(this.Attributes["stroke-width"] ?? new SvgUnit(1.0f)); }
            set { this.Attributes["stroke-width"] = value; }
        }

        [SvgAttribute("stroke-linecap", true)]
        public virtual SvgStrokeLineCap StrokeLineCap
        {
            get { return (SvgStrokeLineCap)(this.Attributes["stroke-linecap"] ?? SvgStrokeLineCap.Butt); }
            set { this.Attributes["stroke-linecap"] = value; }
        }

        [SvgAttribute("stroke-linejoin", true)]
        public virtual SvgStrokeLineJoin StrokeLineJoin
        {
            get { return (SvgStrokeLineJoin)(this.Attributes["stroke-linejoin"] ?? SvgStrokeLineJoin.Miter); }
            set { this.Attributes["stroke-linejoin"] = value; }
        }

        [SvgAttribute("stroke-miterlimit", true)]
        public virtual float StrokeMiterLimit
        {
            get { return (float)(this.Attributes["stroke-miterlimit"] ?? 4f); }
            set { this.Attributes["stroke-miterlimit"] = value; }
        }

        [SvgAttribute("stroke-dasharray", true)]
        public virtual SvgUnitCollection StrokeDashArray
        {
            get { return this.Attributes["stroke-dasharray"] as SvgUnitCollection; }
            set { this.Attributes["stroke-dasharray"] = value; }
        }

        [SvgAttribute("stroke-dashoffset", true)]
        public virtual SvgUnit StrokeDashOffset
        {
            get { return (SvgUnit)(this.Attributes["stroke-dashoffset"] ?? SvgUnit.Empty); }
            set { this.Attributes["stroke-dashoffset"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of the stroke, if the <see cref="Stroke"/> property has been specified. 1.0 is fully opaque; 0.0 is transparent.
        /// </summary>
        [SvgAttribute("stroke-opacity", true)]
        public virtual float StrokeOpacity
        {
            get { return (float)(this.Attributes["stroke-opacity"] ?? 1.0f); }
            set { this.Attributes["stroke-opacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Gets or sets the colour of the gradient stop.
        /// </summary>
        /// <remarks>Apparently this can be set on non-sensical elements.  Don't ask; just check the tests.</remarks>
        [SvgAttribute("stop-color", true)]
        [TypeConverter(typeof(SvgPaintServerFactory))]
        public virtual SvgPaintServer StopColor
        {
            get { return this.Attributes["stop-color"] as SvgPaintServer; }
            set { this.Attributes["stop-color"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of the element. 1.0 is fully opaque; 0.0 is transparent.
        /// </summary>
        [SvgAttribute("opacity", true)]
        public virtual float Opacity
        {
            get { return (float)(this.Attributes["opacity"] ?? 1.0f); }
            set { this.Attributes["opacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Refers to the AnitAlias rendering of shapes.
        /// </summary>
        [SvgAttribute("shape-rendering")]
        public virtual SvgShapeRendering ShapeRendering
        {
            get { return this.Attributes.GetInheritedAttribute<SvgShapeRendering>("shape-rendering"); }
            set { this.Attributes["shape-rendering"] = value; }
        }

        /// <summary>
        /// Gets or sets the text anchor.
        /// </summary>
        [SvgAttribute("text-anchor", true)]
        public virtual SvgTextAnchor TextAnchor
        {
            get { return this.Attributes.GetInheritedAttribute<SvgTextAnchor>("text-anchor"); }
            set { this.Attributes["text-anchor"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Specifies dominant-baseline positioning of text.
        /// </summary>
        [SvgAttribute("baseline-shift", true)]
        public virtual string BaselineShift
        {
            get { return this.Attributes.GetInheritedAttribute<string>("baseline-shift"); }
            set { this.Attributes["baseline-shift"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Indicates which font family is to be used to render the text.
        /// </summary>
        [SvgAttribute("font-family", true)]
        public virtual string FontFamily
        {
            get { return this.Attributes["font-family"] as string; }
            set { this.Attributes["font-family"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.
        /// </summary>
        [SvgAttribute("font-size", true)]
        public virtual SvgUnit FontSize
        {
            get { return (SvgUnit)(this.Attributes["font-size"] ?? SvgUnit.Empty); }
            set { this.Attributes["font-size"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Refers to the style of the font.
        /// </summary>
        [SvgAttribute("font-style", true)]
        public virtual SvgFontStyle FontStyle
        {
            get { return (SvgFontStyle)(this.Attributes["font-style"] ?? SvgFontStyle.All); }
            set { this.Attributes["font-style"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Refers to the varient of the font.
        /// </summary>
        [SvgAttribute("font-variant", true)]
        public virtual SvgFontVariant FontVariant
        {
            get { return (SvgFontVariant)(this.Attributes["font-variant"] ?? SvgFontVariant.Inherit); }
            set { this.Attributes["font-variant"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Refers to the boldness of the font.
        /// </summary>
        [SvgAttribute("text-decoration", true)]
        public virtual SvgTextDecoration TextDecoration
        {
            get { return (SvgTextDecoration)(this.Attributes["text-decoration"] ?? SvgTextDecoration.Inherit); }
            set { this.Attributes["text-decoration"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Refers to the boldness of the font.
        /// </summary>
        [SvgAttribute("font-weight", true)]
        public virtual SvgFontWeight FontWeight
        {
            get { return (SvgFontWeight)(this.Attributes["font-weight"] ?? SvgFontWeight.Inherit); }
            set { this.Attributes["font-weight"] = value; this.IsPathDirty = true; }
        }

        private enum FontParseState
        {
            fontStyle,
            fontVariant,
            fontWeight,
            fontSize,
            fontFamilyNext,
            fontFamilyCurr
        }

        /// <summary>
        /// Set all font information.
        /// </summary>
        [SvgAttribute("font", true)]
        public virtual string Font
        {
            get { return ((this.Attributes["font"] ?? string.Empty) as string); }
            set
            {
                var state = FontParseState.fontStyle;
                var parts = value.Split(' ');

                SvgFontStyle fontStyle;
                SvgFontVariant fontVariant;
                SvgFontWeight fontWeight;
                SvgUnit fontSize;

                bool success;
                string[] sizes;
                string part;

                for (int i = 0; i < parts.Length; i++)
                {
                    part = parts[i];
                    success = false;
                    while (!success)
                    {
                        switch (state)
                        {
                            case FontParseState.fontStyle:
                                success = Enums.TryParse<SvgFontStyle>(part, out fontStyle);
                                if (success) this.FontStyle = fontStyle;
                                state++;
                                break;
                            case FontParseState.fontVariant:
                                success = Enums.TryParse<SvgFontVariant>(part, out fontVariant);
                                if (success) this.FontVariant = fontVariant;
                                state++;
                                break;
                            case FontParseState.fontWeight:
                                success = Enums.TryParse<SvgFontWeight>(part, out fontWeight);
                                if (success) this.FontWeight = fontWeight;
                                state++;
                                break;
                            case FontParseState.fontSize:
                                sizes = part.Split('/');
                                try
                                {
                                    fontSize = (SvgUnit)(new SvgUnitConverter().ConvertFromInvariantString(sizes[0]));
                                    success = true;
                                    this.FontSize = fontSize;
                                }
                                catch { }
                                state++;
                                break;
                            case FontParseState.fontFamilyNext:
                                state++;
                                success = true;
                                break;
                        }
                    }

                    switch (state)
                    {
                        case FontParseState.fontFamilyNext:
                            this.FontFamily = string.Join(" ", parts, i + 1, parts.Length - (i + 1));
                            i = int.MaxValue - 2;
                            break;
                        case FontParseState.fontFamilyCurr:
                            this.FontFamily = string.Join(" ", parts, i, parts.Length - (i));
                            i = int.MaxValue - 2;
                            break;
                    }

                }

                this.Attributes["font"] = value;
                this.IsPathDirty = true;
            }
        }
        
        /// <summary>
        /// Get the font information based on data stored with the text object or inherited from the parent.
        /// </summary>
        /// <returns></returns>
        internal IFontDefn GetFont(ISvgRenderer renderer)
        {
            // Get the font-size
            float fontSize;
            var fontSizeUnit = this.FontSize;
            if (fontSizeUnit == SvgUnit.None || fontSizeUnit == SvgUnit.Empty)
            {
                fontSize = 1.0f;
            }
            else
            {
                fontSize = fontSizeUnit.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);
            }

            var family = ValidateFontFamily(this.FontFamily, this.OwnerDocument);
            var sFaces = family as IEnumerable<SvgFontFace>;

            if (sFaces == null)
            {
                var fontStyle = System.Drawing.FontStyle.Regular;

                // Get the font-weight
                switch (this.FontWeight)
                {
                    //Note: Bold is not listed because it is = W700.
                    case SvgFontWeight.Bolder:
                    case SvgFontWeight.W600:
                    case SvgFontWeight.W700:
                    case SvgFontWeight.W800:
                    case SvgFontWeight.W900:
                        fontStyle |= System.Drawing.FontStyle.Bold;
                        break;
                }

                // Get the font-style
                switch (this.FontStyle)
                {
                    case SvgFontStyle.Italic:
                    case SvgFontStyle.Oblique:
                        fontStyle |= System.Drawing.FontStyle.Italic;
                        break;
                }

                // Get the text-decoration
                switch (this.TextDecoration)
                {
                    case SvgTextDecoration.LineThrough:
                        fontStyle |= System.Drawing.FontStyle.Strikeout;
                        break;
                    case SvgTextDecoration.Underline:
                        fontStyle |= System.Drawing.FontStyle.Underline;
                        break;
                }

                var ff = family as FontFamily;
                if (!ff.IsStyleAvailable(fontStyle))
                {
                    // Do Something
                }

                // Get the font-family
                return new GdiFontDefn(new System.Drawing.Font(ff, fontSize, fontStyle, System.Drawing.GraphicsUnit.Pixel));
            }
            else
            {
                var font = sFaces.First().Parent as SvgFont;
                if (font == null)
                {
                    var uri = sFaces.First().Descendants().OfType<SvgFontFaceUri>().First().ReferencedElement;
                    font = OwnerDocument.IdManager.GetElementById(uri) as SvgFont;
                }
                return new SvgFontDefn(font, fontSize, OwnerDocument.Ppi);
            }
        }

        public static System.Drawing.Text.PrivateFontCollection PrivateFonts = new System.Drawing.Text.PrivateFontCollection();
        public static object ValidateFontFamily(string fontFamilyList, SvgDocument doc)
        {
            // Split font family list on "," and then trim start and end spaces and quotes.
            var fontParts = (fontFamilyList ?? string.Empty).Split(new[] { ',' }).Select(fontName => fontName.Trim(new[] { '"', ' ', '\'' }));
            var families = System.Drawing.FontFamily.Families;
            Func<FontFamily, bool> getFamily;
            FontFamily family;
            IEnumerable<SvgFontFace> sFaces;

            // Find a the first font that exists in the list of installed font families.
            //styles from IE get sent through as lowercase.
            foreach (var f in fontParts)
            {
                if (doc.FontDefns().TryGetValue(f, out sFaces)) return sFaces;

                getFamily = new Func<FontFamily, bool>(ff => string.Equals(ff.Name, f, StringComparison.OrdinalIgnoreCase));
                family = families.FirstOrDefault(getFamily);
                if (family != null) return family;
                family = PrivateFonts.Families.FirstOrDefault(getFamily);
                if (family != null) return family;

                switch (f.ToLower())
                {
                    case "serif":
                        return System.Drawing.FontFamily.GenericSerif;
                    case "sans-serif":
                        return System.Drawing.FontFamily.GenericSansSerif;
                    case "monospace":
                        return System.Drawing.FontFamily.GenericMonospace;
                }
            }

            // No valid font family found from the list requested.
            return System.Drawing.FontFamily.GenericSansSerif;
        }
    }
}
