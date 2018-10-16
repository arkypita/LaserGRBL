namespace Svg
{
    public class NonSvgElement : SvgElement
    {
        public NonSvgElement()
        {
        }

        public NonSvgElement(string elementName)
        {
            this.ElementName = elementName;
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<NonSvgElement>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as NonSvgElement;

            return newObj;
        }

        /// <summary>
        /// Publish the element name to be able to differentiate non-svg elements.
        /// </summary>
        public string Name
        {
            get
            {
                return ElementName;
            }
        }
    }
}