using System.Linq;
using Svg.Pathing;
using System.Drawing.Drawing2D;

namespace Svg
{
    [SvgElement("glyph")]
    public class SvgGlyph : SvgPathBasedElement
    {
        private GraphicsPath _path;

        /// <summary>
        /// Gets or sets a <see cref="SvgPathSegmentList"/> of path data.
        /// </summary>
        [SvgAttribute("d", true)]
        public SvgPathSegmentList PathData
        {
            get { return this.Attributes.GetAttribute<SvgPathSegmentList>("d"); }
            set { this.Attributes["d"] = value; }
        }

        [SvgAttribute("glyph-name", true)]
        public virtual string GlyphName
        {
            get { return this.Attributes["glyph-name"] as string; }
            set { this.Attributes["glyph-name"] = value; }
        }
        [SvgAttribute("horiz-adv-x", true)]
        public float HorizAdvX
        {
            get { return (this.Attributes["horiz-adv-x"] == null ? this.Parents.OfType<SvgFont>().First().HorizAdvX : (float)this.Attributes["horiz-adv-x"]); }
            set { this.Attributes["horiz-adv-x"] = value; }
        }
        [SvgAttribute("unicode", true)]
        public string Unicode
        {
            get { return this.Attributes["unicode"] as string; }
            set { this.Attributes["unicode"] = value; }
        }
        [SvgAttribute("vert-adv-y", true)]
        public float VertAdvY
        {
            get { return (this.Attributes["vert-adv-y"] == null ? this.Parents.OfType<SvgFont>().First().VertAdvY : (float)this.Attributes["vert-adv-y"]); }
            set { this.Attributes["vert-adv-y"] = value; }
        }
        [SvgAttribute("vert-origin-x", true)]
        public float VertOriginX
        {
            get { return (this.Attributes["vert-origin-x"] == null ? this.Parents.OfType<SvgFont>().First().VertOriginX : (float)this.Attributes["vert-origin-x"]); }
            set { this.Attributes["vert-origin-x"] = value; }
        }
        [SvgAttribute("vert-origin-y", true)]
        public float VertOriginY
        {
            get { return (this.Attributes["vert-origin-y"] == null ? this.Parents.OfType<SvgFont>().First().VertOriginY : (float)this.Attributes["vert-origin-y"]); }
            set { this.Attributes["vert-origin-y"] = value; }
        }


        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (this._path == null || this.IsPathDirty)
            {
                _path = new GraphicsPath();

                foreach (SvgPathSegment segment in this.PathData)
                {
                    segment.AddToPath(_path);
                }

                this.IsPathDirty = false;
            }
            return _path;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgGlyph"/> class.
        /// </summary>
        public SvgGlyph()
        {
            var pathData = new SvgPathSegmentList();
            this.Attributes["d"] = pathData;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGlyph>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgGlyph;
            foreach (var pathData in this.PathData)
                newObj.PathData.Add(pathData.Clone());
            return newObj;

        }
    }
}
