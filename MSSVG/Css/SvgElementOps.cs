using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fizzler;

namespace Svg.Css
{
    internal class SvgElementOps : IElementOps<SvgElement>
    {
	    private readonly SvgElementFactory _elementFactory;

	    public SvgElementOps(SvgElementFactory elementFactory)
	    {
		    _elementFactory = elementFactory;
	    }

	    public Selector<SvgElement> Type(NamespacePrefix prefix, string name)
        {
            SvgElementFactory.ElementInfo type = null;
            if (_elementFactory.AvailableElements.TryGetValue(name, out type)) 
            {
                return nodes => nodes.Where(n => n.GetType() == type.ElementType);
            }
            return nodes => Enumerable.Empty<SvgElement>();
        }

        public Selector<SvgElement> Universal(NamespacePrefix prefix)
        {
            return nodes => nodes;
        }

        public Selector<SvgElement> Id(string id)
        {
            return nodes => nodes.Where(n => n.ID == id);
        }

        public Selector<SvgElement> Class(string clazz)
        {
            return AttributeIncludes(NamespacePrefix.None, "class", clazz);
        }

        public Selector<SvgElement> AttributeExists(NamespacePrefix prefix, string name)
        {
            return nodes => nodes.Where(n => n.ContainsAttribute(name));
        }

        public Selector<SvgElement> AttributeExact(NamespacePrefix prefix, string name, string value)
        {
            return nodes => nodes.Where(n =>
            {
                string val = null;
                return (n.TryGetAttribute(name, out val) && val == value);
            });
        }

        public Selector<SvgElement> AttributeIncludes(NamespacePrefix prefix, string name, string value)
        {
            return nodes => nodes.Where(n =>
            {
                string val = null;
                return (n.TryGetAttribute(name, out val) && val.Split(' ').Contains(value));
            });
        }

        public Selector<SvgElement> AttributeDashMatch(NamespacePrefix prefix, string name, string value)
        {
            return string.IsNullOrEmpty(value)
                 ? (Selector<SvgElement>)(nodes => Enumerable.Empty<SvgElement>())
                 : (nodes => nodes.Where(n =>
                    {
                        string val = null;
                        return (n.TryGetAttribute(name, out val) && val.Split('-').Contains(value));
                    }));
        }

        public Selector<SvgElement> AttributePrefixMatch(NamespacePrefix prefix, string name, string value)
        {
            return string.IsNullOrEmpty(value)
                 ? (Selector<SvgElement>)(nodes => Enumerable.Empty<SvgElement>())
                 : (nodes => nodes.Where(n =>
                     {
                         string val = null;
                         return (n.TryGetAttribute(name, out val) && val.StartsWith(value));
                     }));
        }

        public Selector<SvgElement> AttributeSuffixMatch(NamespacePrefix prefix, string name, string value)
        {
            return string.IsNullOrEmpty(value)
                 ? (Selector<SvgElement>)(nodes => Enumerable.Empty<SvgElement>())
                 : (nodes => nodes.Where(n =>
                 {
                     string val = null;
                     return (n.TryGetAttribute(name, out val) && val.EndsWith(value));
                 }));
        }

        public Selector<SvgElement> AttributeSubstring(NamespacePrefix prefix, string name, string value)
        {
            return string.IsNullOrEmpty(value)
                 ? (Selector<SvgElement>)(nodes => Enumerable.Empty<SvgElement>())
                 : (nodes => nodes.Where(n =>
                 {
                     string val = null;
                     return (n.TryGetAttribute(name, out val) && val.Contains(value));
                 }));
        }

        public Selector<SvgElement> FirstChild()
        {
            return nodes => nodes.Where(n => n.Parent == null || n.Parent.Children.First() == n);
        }

        public Selector<SvgElement> LastChild()
        {
            return nodes => nodes.Where(n => n.Parent == null || n.Parent.Children.Last() == n);
        }

        private IEnumerable<T> GetByIds<T>(IList<T> items, IEnumerable<int> indices)
        {
            foreach (var i in indices)
            {
                if (i >= 0 && i < items.Count) yield return items[i];
            }
        }

        public Selector<SvgElement> NthChild(int a, int b)
        {
            return nodes => nodes.Where(n => n.Parent != null && GetByIds(n.Parent.Children, (from i in Enumerable.Range(0, n.Parent.Children.Count / a) select a * i + b)).Contains(n));
        }

        public Selector<SvgElement> OnlyChild()
        {
            return nodes => nodes.Where(n => n.Parent == null || n.Parent.Children.Count == 1);
        }

        public Selector<SvgElement> Empty()
        {
            return nodes => nodes.Where(n => n.Children.Count == 0);
        }

        public Selector<SvgElement> Child()
        {
            return nodes => nodes.SelectMany(n => n.Children);
        }

        public Selector<SvgElement> Descendant()
        {
            return nodes => nodes.SelectMany(n => Descendants(n));
        }

        private IEnumerable<SvgElement> Descendants(SvgElement elem)
        {
            foreach (var child in elem.Children)
            {
                yield return child;
                foreach (var descendant in child.Descendants())
                {
                    yield return descendant;
                }
            }
        }

        public Selector<SvgElement> Adjacent()
        {
            return nodes => nodes.SelectMany(n => ElementsAfterSelf(n).Take(1));
        }

        public Selector<SvgElement> GeneralSibling()
        {
            return nodes => nodes.SelectMany(n => ElementsAfterSelf(n));
        }

        private IEnumerable<SvgElement> ElementsAfterSelf(SvgElement self)
        {
            return (self.Parent == null ? Enumerable.Empty<SvgElement>() : self.Parent.Children.Skip(self.Parent.Children.IndexOf(self) + 1));
        }

        public Selector<SvgElement> NthLastChild(int a, int b)
        {
            throw new NotImplementedException();
        }
    }
}
