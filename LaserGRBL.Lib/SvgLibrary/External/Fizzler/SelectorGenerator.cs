#region Copyright and License
// 
// Fizzler - CSS Selector Engine for Microsoft .NET Framework
// Copyright (c) 2009 Atif Aziz, Colin Ramsay. All rights reserved.
// 
// This library is free software; you can redistribute it and/or modify it under 
// the terms of the GNU Lesser General Public License as published by the Free 
// Software Foundation; either version 3 of the License, or (at your option) 
// any later version.
// 
// This library is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more 
// details.
// 
// You should have received a copy of the GNU Lesser General Public License 
// along with this library; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
// 
#endregion

namespace Fizzler
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    /// <summary>
    /// A selector generator implementation for an arbitrary document/element system.
    /// </summary>
    public class SelectorGenerator<TElement> : ISelectorGenerator
    {
        private readonly IEqualityComparer<TElement> _equalityComparer;
        private readonly Stack<Selector<TElement>> _selectors;

        /// <summary>
        /// Initializes a new instance of this object with an instance
        /// of <see cref="IElementOps{TElement}"/> and the default equality
        /// comparer that is used for determining if two elements are equal.
        /// </summary>
        public SelectorGenerator(IElementOps<TElement> ops) : this(ops, null) {}

        /// <summary>
        /// Initializes a new instance of this object with an instance
        /// of <see cref="IElementOps{TElement}"/> and an equality comparer
        /// used for determining if two elements are equal.
        /// </summary>
        public SelectorGenerator(IElementOps<TElement> ops, IEqualityComparer<TElement> equalityComparer)
        {
            if(ops == null) throw new ArgumentNullException("ops");
            Ops = ops;
            _equalityComparer = equalityComparer ?? EqualityComparer<TElement>.Default;
            _selectors = new Stack<Selector<TElement>>();
        }

        /// <summary>
        /// Gets the selector implementation.
        /// </summary>
        /// <remarks>
        /// If the generation is not complete, this property returns the 
        /// last generated selector.
        /// </remarks>
        public Selector<TElement> Selector { get; private set; }

        /// <summary>
        /// Gets the <see cref="IElementOps{TElement}"/> instance that this object
        /// was initialized with.
        /// </summary>
        public IElementOps<TElement> Ops { get; private set; }

        /// <summary>
        /// Returns the collection of selector implementations representing 
        /// a group.
        /// </summary>
        /// <remarks>
        /// If the generation is not complete, this method return the 
        /// selectors generated so far in a group.
        /// </remarks>
        public IEnumerable<Selector<TElement>> GetSelectors()
        {
            var selectors = _selectors;
            var top = Selector;
            return top == null 
                 ? selectors.Select(s => s) 
                 : selectors.Concat(Enumerable.Repeat(top, 1));
        }

        /// <summary>
        /// Adds a generated selector.
        /// </summary>
        protected void Add(Selector<TElement> selector)
        {
            if(selector == null) throw new ArgumentNullException("selector");
            
            var top = Selector;
            Selector = top == null ? selector : (elements => selector(top(elements)));
        }

        /// <summary>
        /// Delimits the initialization of a generation.
        /// </summary>
        public virtual void OnInit()
        {
            _selectors.Clear();
            Selector = null;
        }

        /// <summary>
        /// Delimits a selector generation in a group of selectors.
        /// </summary>
        public virtual void OnSelector()
        {
            if (Selector != null)
                _selectors.Push(Selector);
            Selector = null;
        }

        /// <summary>
        /// Delimits the closing/conclusion of a generation.
        /// </summary>
        public virtual void OnClose()
        {
            var sum = GetSelectors().Aggregate((a, b) => (elements => a(elements).Concat(b(elements))));
            var normalize = Ops.Descendant();
            Selector = elements => sum(normalize(elements)).Distinct(_equalityComparer);
            _selectors.Clear();
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#Id-selectors">ID selector</a>,
        /// which represents an element instance that has an identifier that 
        /// matches the identifier in the ID selector.
        /// </summary>
        public virtual void Id(string id)
        {
            Add(Ops.Id(id));
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#class-html">class selector</a>,
        /// which is an alternative <see cref="ISelectorGenerator.AttributeIncludes"/> when 
        /// representing the <c>class</c> attribute. 
        /// </summary>
        public virtual void Class(string clazz)
        {
            Add(Ops.Class(clazz));
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#type-selectors">type selector</a>,
        /// which represents an instance of the element type in the document tree. 
        /// </summary>
        public virtual void Type(NamespacePrefix prefix, string type)
        {
            Add(Ops.Type(prefix, type));
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#universal-selector">universal selector</a>,
        /// any single element in the document tree in any namespace 
        /// (including those without a namespace) if no default namespace 
        /// has been specified for selectors. 
        /// </summary>
        public virtual void Universal(NamespacePrefix prefix)
        {
            Add(Ops.Universal(prefix));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the given attribute <paramref name="name"/>
        /// whatever the values of the attribute.
        /// </summary>
        public virtual void AttributeExists(NamespacePrefix prefix, string name)
        {
            Add(Ops.AttributeExists(prefix, name));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the given attribute <paramref name="name"/>
        /// and whose value is exactly <paramref name="value"/>.
        /// </summary>
        public virtual void AttributeExact(NamespacePrefix prefix, string name, string value)
        {
            Add(Ops.AttributeExact(prefix, name, value));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the given attribute <paramref name="name"/>
        /// and whose value is a whitespace-separated list of words, one of 
        /// which is exactly <paramref name="value"/>.
        /// </summary>
        public virtual void AttributeIncludes(NamespacePrefix prefix, string name, string value)
        {
            Add(Ops.AttributeIncludes(prefix, name, value));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the given attribute <paramref name="name"/>,
        /// its value either being exactly <paramref name="value"/> or beginning 
        /// with <paramref name="value"/> immediately followed by "-" (U+002D).
        /// </summary>
        public virtual void AttributeDashMatch(NamespacePrefix prefix, string name, string value)
        {
            Add(Ops.AttributeDashMatch(prefix, name, value));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the attribute <paramref name="name"/> 
        /// whose value begins with the prefix <paramref name="value"/>.
        /// </summary>
        public void AttributePrefixMatch(NamespacePrefix prefix, string name, string value)
        {
            Add(Ops.AttributePrefixMatch(prefix, name, value));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the attribute <paramref name="name"/> 
        /// whose value ends with the suffix <paramref name="value"/>.
        /// </summary>
        public void AttributeSuffixMatch(NamespacePrefix prefix, string name, string value)
        {
            Add(Ops.AttributeSuffixMatch(prefix, name, value));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the attribute <paramref name="name"/> 
        /// whose value contains at least one instance of the substring <paramref name="value"/>.
        /// </summary>
        public void AttributeSubstring(NamespacePrefix prefix, string name, string value)
        {
            Add(Ops.AttributeSubstring(prefix, name, value));
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents an element that is the first child of some other element.
        /// </summary>
        public virtual void FirstChild()
        {
            Add(Ops.FirstChild());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents an element that is the last child of some other element.
        /// </summary>
        public virtual void LastChild()
        {
            Add(Ops.LastChild());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents an element that is the N-th child of some other element.
        /// </summary>
        public virtual void NthChild(int a, int b)
        {
            Add(Ops.NthChild(a, b));
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents an element that has a parent element and whose parent 
        /// element has no other element children.
        /// </summary>
        public virtual void OnlyChild()
        {
            Add(Ops.OnlyChild());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents an element that has no children at all.
        /// </summary>
        public virtual void Empty()
        {
            Add(Ops.Empty());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
        /// which represents a childhood relationship between two elements.
        /// </summary>
        public virtual void Child()
        {
            Add(Ops.Child());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
        /// which represents a relationship between two elements where one element is an 
        /// arbitrary descendant of some ancestor element.
        /// </summary>
        public virtual void Descendant()
        {
            Add(Ops.Descendant());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
        /// which represents elements that share the same parent in the document tree and 
        /// where the first element immediately precedes the second element.
        /// </summary>
        public virtual void Adjacent()
        {
            Add(Ops.Adjacent());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
        /// which separates two sequences of simple selectors. The elements represented
        /// by the two sequences share the same parent in the document tree and the
        /// element represented by the first sequence precedes (not necessarily
        /// immediately) the element represented by the second one.
        /// </summary>
        public virtual void GeneralSibling()
        {
            Add(Ops.GeneralSibling());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents an element that is the N-th child from bottom up of some other element.
        /// </summary>
        public void NthLastChild(int a, int b)
        {
            Add(Ops.NthLastChild(a, b));
        }
    }
}