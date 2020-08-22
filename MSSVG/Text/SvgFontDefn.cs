using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    public class SvgFontDefn : IFontDefn
    {
        private SvgFont _font;
        private float _emScale;
        private float _ppi;
        private float _size;
        private Dictionary<string, SvgGlyph> _glyphs;
        private Dictionary<string, SvgKern> _kerning;

        public float Size
        {
            get { return _size; }
        }

        public float SizeInPoints
        {
            get { return _size * 72.0f / _ppi; }
        }

        public SvgFontDefn (SvgFont font, float size, float ppi)
        {
            _font = font;
            _size = size;
            _ppi = ppi;
            var face = _font.Children.OfType<SvgFontFace>().First();
            _emScale = _size / face.UnitsPerEm;
        }

        public float Ascent(ISvgRenderer renderer)
        {
            float ascent = _font.Descendants().OfType<SvgFontFace>().First().Ascent;
            float baselineOffset = this.SizeInPoints * (_emScale / _size) * ascent;
            return renderer.DpiY / 72f * baselineOffset;
        }

        public IList<System.Drawing.RectangleF> MeasureCharacters(ISvgRenderer renderer, string text)
        {
            var result = new List<RectangleF>();
            using (var path = GetPath(renderer, text, result, false)) { }
            return result;
        }

        public System.Drawing.SizeF MeasureString(ISvgRenderer renderer, string text)
        {
            var result = new List<RectangleF>();
            using (var path = GetPath(renderer, text, result, true)) { }
            var nonEmpty = result.Where(r => r != RectangleF.Empty);
            if (!nonEmpty.Any()) return SizeF.Empty;
            return new SizeF(nonEmpty.Last().Right - nonEmpty.First().Left, Ascent(renderer));
        }

        public void AddStringToPath(ISvgRenderer renderer, GraphicsPath path, string text, PointF location)
        {
            var textPath = GetPath(renderer, text, null, false);
            if (textPath.PointCount > 0)
            {
                using (var translate = new Matrix())
                {
                    translate.Translate(location.X, location.Y);
                    textPath.Transform(translate);
                    path.AddPath(textPath, false);
                }
            }
        }

        private GraphicsPath GetPath(ISvgRenderer renderer, string text, IList<RectangleF> ranges, bool measureSpaces)
        {
            EnsureDictionaries();

            RectangleF bounds;
            SvgGlyph glyph;
            SvgKern kern;
            GraphicsPath path;
            SvgGlyph prevGlyph = null;
            Matrix scaleMatrix;
            float xPos = 0;

            var ascent = Ascent(renderer);

            var result = new GraphicsPath();
            if (string.IsNullOrEmpty(text)) return result;

            for (int i = 0; i < text.Length; i++)
            {
                if (!_glyphs.TryGetValue(text.Substring(i, 1), out glyph)) glyph = _font.Descendants().OfType<SvgMissingGlyph>().First();
                if (prevGlyph != null && _kerning.TryGetValue(prevGlyph.GlyphName + "|" + glyph.GlyphName, out kern))
                {
                    xPos -= kern.Kerning * _emScale;
                }
                path = (GraphicsPath)glyph.Path(renderer).Clone();
                scaleMatrix = new Matrix();
                scaleMatrix.Scale(_emScale, -1 * _emScale, MatrixOrder.Append);
                scaleMatrix.Translate(xPos, ascent, MatrixOrder.Append);
                path.Transform(scaleMatrix);
                scaleMatrix.Dispose();

                bounds = path.GetBounds();
                if (ranges != null)
                {
                    if (measureSpaces && bounds == RectangleF.Empty)
                    {
                        ranges.Add(new RectangleF(xPos, 0, glyph.HorizAdvX * _emScale, ascent));
                    }
                    else
                    {
                        ranges.Add(bounds);
                    }
                }
                if (path.PointCount > 0) result.AddPath(path, false);

                xPos += glyph.HorizAdvX * _emScale;
                prevGlyph = glyph;
            }

            return result;
        }

        private void EnsureDictionaries()
        {
            if (_glyphs == null) _glyphs = _font.Descendants().OfType<SvgGlyph>().ToDictionary(g => g.Unicode ?? g.GlyphName ?? g.ID);
            if (_kerning == null) _kerning = _font.Descendants().OfType<SvgKern>().ToDictionary(k => k.Glyph1 + "|" + k.Glyph2);
        }

        public void Dispose()
        {
            _glyphs = null;
            _kerning = null;
        }
    }
}
