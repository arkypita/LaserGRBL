using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Svg
{
    /// <summary>
    /// A wrapper for a paint server has a fallback if the primary server doesn't work.
    /// </summary>
    public class SvgFallbackPaintServer : SvgPaintServer
    {
        private IEnumerable<SvgPaintServer> _fallbacks;
        private SvgPaintServer _primary;

        public SvgFallbackPaintServer() : base() { }
        public SvgFallbackPaintServer(SvgPaintServer primary, IEnumerable<SvgPaintServer> fallbacks) : this()
        {
            _fallbacks = fallbacks;
            _primary = primary;
        }

        public override Brush GetBrush(SvgVisualElement styleOwner, ISvgRenderer renderer, float opacity, bool forStroke = false)
        {
            try
            {
                _primary.GetCallback = () => _fallbacks.FirstOrDefault();
                return _primary.GetBrush(styleOwner, renderer, opacity, forStroke);
            }
            finally
            {
                _primary.GetCallback = null;
            }
        }

        public override SvgElement DeepCopy()
        {
            return base.DeepCopy<SvgFallbackPaintServer>();
        }
        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgFallbackPaintServer;
            newObj._fallbacks = this._fallbacks;
            newObj._primary = this._primary;
            return newObj;
        }
    }
}
