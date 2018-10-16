using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    [SvgElement("tref")]
    public class SvgTextRef : SvgTextBase
    {
        private Uri _referencedElement;

        [SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
        public virtual Uri ReferencedElement
        {
            get { return this._referencedElement; }
            set { this._referencedElement = value; }
        }

        internal override IEnumerable<ISvgNode> GetContentNodes()
        {
            var refText = this.OwnerDocument.IdManager.GetElementById(this.ReferencedElement) as SvgTextBase;
            IEnumerable<ISvgNode> contentNodes = null;

            if (refText == null)
            {
                contentNodes = base.GetContentNodes();
            }
            else
            {
                contentNodes = refText.GetContentNodes();
            }

            contentNodes = contentNodes.Where(o => !(o is ISvgDescriptiveElement));

            return contentNodes;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgTextRef>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgTextRef;
            newObj.X = this.X;
            newObj.Y = this.Y;
            newObj.Dx = this.Dx;
            newObj.Dy = this.Dy;
            newObj.Text = this.Text;
            newObj.ReferencedElement = this.ReferencedElement;

            return newObj;
        }


    }
}
