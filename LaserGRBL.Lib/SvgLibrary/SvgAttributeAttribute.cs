using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Svg
{
    /// <summary>
    /// Specifies the SVG attribute name of the associated property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Event)]
    public class SvgAttributeAttribute : System.Attribute
    {
		/// <summary>
		/// Gets a <see cref="string"/> containing the XLink namespace (http://www.w3.org/1999/xlink).
		/// </summary>
		public const string SvgNamespace = "http://www.w3.org/2000/svg";
		public const string XLinkPrefix = "xlink";
		public const string XLinkNamespace = "http://www.w3.org/1999/xlink";
        public const string XmlNamespace = "http://www.w3.org/XML/1998/namespace";

		public static readonly List<KeyValuePair<string, string>> Namespaces = new List<KeyValuePair<string, string>>()
    	                                                                	{
																				new KeyValuePair<string, string>("", SvgNamespace),
																				new KeyValuePair<string, string>(XLinkPrefix, XLinkNamespace),
																				new KeyValuePair<string, string>("xml", XmlNamespace)
		                                                                    };
        private bool _inAttrDictionary;
        private string _name;
        private string _namespace;

        /// <summary>
        /// When overridden in a derived class, returns a value that indicates whether this instance equals a specified object.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object"/> to compare with this instance of <see cref="T:System.Attribute"/>.</param>
        /// <returns>
        /// true if this instance equals <paramref name="obj"/>; otherwise, false.
        /// </returns>
        public override bool Match(object obj)
        {
            SvgAttributeAttribute indicator = obj as SvgAttributeAttribute;

            if (indicator == null)
                return false;

            // Always match if either value is String.Empty (wildcard)
            if (indicator.Name == String.Empty)
                return false;

            return String.Compare(indicator.Name, this.Name) == 0;
        }

		/// <summary>
		/// Gets the name of the SVG attribute.
		/// </summary>
		public string NamespaceAndName
		{
			get
			{
				if (_namespace == SvgNamespace)
					return _name;
				return Namespaces.First(x => x.Value == _namespace).Key + ":" + _name;
			}
		}


        /// <summary>
        /// Gets the name of the SVG attribute.
        /// </summary>
        public string Name
        {
            get { return this._name; }
        }

        /// <summary>
        /// Gets the namespace of the SVG attribute.
        /// </summary>
        public string NameSpace
        {
            get { return this._namespace; }
        }

        public bool InAttributeDictionary
        {
            get { return this._inAttrDictionary; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgAttributeAttribute"/> class.
        /// </summary>
        internal SvgAttributeAttribute()
        {
            this._name = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgAttributeAttribute"/> class with the specified attribute name.
        /// </summary>
        /// <param name="name">The name of the SVG attribute.</param>
        internal SvgAttributeAttribute(string name)
        {
            this._name = name;
            this._namespace = SvgNamespace;
        }

        internal SvgAttributeAttribute(string name, bool inAttrDictionary)
        {
            this._name = name;
            this._namespace = SvgNamespace;
            this._inAttrDictionary = inAttrDictionary;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgAttributeAttribute"/> class with the specified SVG attribute name and namespace.
        /// </summary>
        /// <param name="name">The name of the SVG attribute.</param>
        /// <param name="nameSpace">The namespace of the SVG attribute (e.g. http://www.w3.org/2000/svg).</param>
        public SvgAttributeAttribute(string name, string nameSpace)
        {
            this._name = name;
            this._namespace = nameSpace;
        }
    }
}