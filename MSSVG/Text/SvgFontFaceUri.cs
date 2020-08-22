using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    [SvgElement("font-face-uri")]
    public class SvgFontFaceUri : SvgElement
    {
        private Uri _referencedElement;

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement
        {
            get { return this._referencedElement; }
            set { this._referencedElement = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFontFaceUri>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgFontFaceUri;
            newObj.ReferencedElement = this.ReferencedElement;

            return newObj;
        }
    }
}
