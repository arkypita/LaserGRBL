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
    using System;

    /// <summary>
    /// An <see cref="ISelectorGenerator"/> implementation that delegates
    /// to two other <see cref="ISelectorGenerator"/> objects, which
    /// can be useful for doing work in a single pass.
    /// </summary>
    public sealed class SelectorGeneratorTee : ISelectorGenerator
    {
        /// <summary>
        /// Gets the first generator used to initialize this generator.
        /// </summary>
        public ISelectorGenerator Primary { get; private set; }

        /// <summary>
        /// Gets the second generator used to initialize this generator.
        /// </summary>
        public ISelectorGenerator Secondary { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="SelectorGeneratorTee"/>
        /// with the two other <see cref="ISelectorGenerator"/> objects
        /// it delegates to.
        /// </summary>
        public SelectorGeneratorTee(ISelectorGenerator primary, ISelectorGenerator secondary)
        {
            if (primary == null) throw new ArgumentNullException("primary");
            if (secondary == null) throw new ArgumentNullException("secondary");

            Primary = primary;
            Secondary = secondary;
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void OnInit()
        {
            Primary.OnInit();
            Secondary.OnInit();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void OnClose()
        {
            Primary.OnClose();
            Secondary.OnClose();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void OnSelector()
        {
            Primary.OnSelector();
            Secondary.OnSelector();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Type(NamespacePrefix prefix, string type)
        {
            Primary.Type(prefix, type);
            Secondary.Type(prefix, type);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Universal(NamespacePrefix prefix)
        {
            Primary.Universal(prefix);
            Secondary.Universal(prefix);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Id(string id)
        {
            Primary.Id(id);
            Secondary.Id(id);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Class(string clazz)
        {
            Primary.Class(clazz);
            Secondary.Class(clazz);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributeExists(NamespacePrefix prefix, string name)
        {
            Primary.AttributeExists(prefix, name);
            Secondary.AttributeExists(prefix, name);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributeExact(NamespacePrefix prefix, string name, string value)
        {
            Primary.AttributeExact(prefix, name, value);
            Secondary.AttributeExact(prefix, name, value);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributeIncludes(NamespacePrefix prefix, string name, string value)
        {
            Primary.AttributeIncludes(prefix, name, value);
            Secondary.AttributeIncludes(prefix, name, value);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributeDashMatch(NamespacePrefix prefix, string name, string value)
        {
            Primary.AttributeDashMatch(prefix, name, value);
            Secondary.AttributeDashMatch(prefix, name, value);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributePrefixMatch(NamespacePrefix prefix, string name, string value)
        {
            Primary.AttributePrefixMatch(prefix, name, value);
            Secondary.AttributePrefixMatch(prefix, name, value);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributeSuffixMatch(NamespacePrefix prefix, string name, string value)
        {
            Primary.AttributeSuffixMatch(prefix, name, value);
            Secondary.AttributeSuffixMatch(prefix, name, value);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributeSubstring(NamespacePrefix prefix, string name, string value)
        {
            Primary.AttributeSubstring(prefix, name, value);
            Secondary.AttributeSubstring(prefix, name, value);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void FirstChild()
        {
            Primary.FirstChild();
            Secondary.FirstChild();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void LastChild()
        {
            Primary.LastChild();
            Secondary.LastChild();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void NthChild(int a, int b)
        {
            Primary.NthChild(a, b);
            Secondary.NthChild(a, b);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void OnlyChild()
        {
            Primary.OnlyChild();
            Secondary.OnlyChild();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Empty()
        {
            Primary.Empty();
            Secondary.Empty();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Child()
        {
            Primary.Child();
            Secondary.Child();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Descendant()
        {
            Primary.Descendant();
            Secondary.Descendant();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Adjacent()
        {
            Primary.Adjacent();
            Secondary.Adjacent();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void GeneralSibling()
        {
            Primary.GeneralSibling();
            Secondary.GeneralSibling();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void NthLastChild(int a, int b)
        {
            Primary.NthLastChild(a, b);
            Secondary.NthLastChild(a, b);
        }
    }
}
