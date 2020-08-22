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
    /// Represent a type or attribute name.
    /// </summary>
    [Serializable]
    public struct NamespacePrefix
    {
        /// <summary>
        /// Represents a name from either the default or any namespace 
        /// in a target document, depending on whether a default namespace is
        /// in effect or not.
        /// </summary>
        public static readonly NamespacePrefix None = new NamespacePrefix(null);

        /// <summary>
        /// Represents an empty namespace.
        /// </summary>
        public static readonly NamespacePrefix Empty = new NamespacePrefix(string.Empty);

        /// <summary>
        /// Represents any namespace.
        /// </summary>
        public static readonly NamespacePrefix Any = new NamespacePrefix("*");

        /// <summary>
        /// Initializes an instance with a namespace prefix specification.
        /// </summary>
        public NamespacePrefix(string text) : this()
        {
            Text = text;
        }

        /// <summary>
        /// Gets the raw text value of this instance.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Indicates whether this instance represents a name
        /// from either the default or any namespace in a target
        /// document, depending on whether a default namespace is
        /// in effect or not.
        /// </summary>
        public bool IsNone { get { return Text == null; } }

        /// <summary>
        /// Indicates whether this instance represents a name
        /// from any namespace (including one without one)
        /// in a target document.
        /// </summary>
        public bool IsAny
        {
            get { return !IsNone && Text.Length == 1 && Text[0] == '*'; }
        }

        /// <summary>
        /// Indicates whether this instance represents a name
        /// without a namespace in a target document.
        /// </summary>
        public bool IsEmpty { get { return !IsNone && Text.Length == 0; } }

        /// <summary>
        /// Indicates whether this instance represents a name from a 
        /// specific namespace or not.
        /// </summary>
        public bool IsSpecific { get {return !IsNone && !IsAny; } }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is NamespacePrefix && Equals((NamespacePrefix) obj);
        }

        /// <summary>
        /// Indicates whether this instance and another are equal.
        /// </summary>
        public bool Equals(NamespacePrefix other)
        {
            return Text == other.Text;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return IsNone ? 0 : Text.GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return IsNone ? "(none)" : Text;
        }

        /// <summary>
        /// Formats this namespace together with a name.
        /// </summary>
        public string Format(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException(null, "name");

            return Text + (IsNone ? null : "|") + name;
        }
    }
}
