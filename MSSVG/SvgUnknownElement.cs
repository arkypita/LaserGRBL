namespace Svg
{
    public class SvgUnknownElement : SvgElement
    {
        public SvgUnknownElement()
        {
            
        }

        public SvgUnknownElement(string elementName)
        {
            this.ElementName = elementName;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgUnknownElement>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgUnknownElement;

            return newObj;
        }
    }
}