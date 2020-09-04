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
    /// <summary>
    /// Represents the classification of a token.
    /// </summary>
    public enum TokenKind
    {
        /// <summary>
        /// Represents end of input/file/stream
        /// </summary>
        Eoi,

        /// <summary>
        /// Represents {ident}
        /// </summary>
        Ident,

        /// <summary>
        /// Represents "#" {name}
        /// </summary>
        Hash,

        /// <summary>
        /// Represents "~="
        /// </summary>
        Includes,

        /// <summary>
        /// Represents "|="
        /// </summary>
        DashMatch,

        /// <summary>
        /// Represents "^="
        /// </summary>
        PrefixMatch,

        /// <summary>
        /// Represents "$="
        /// </summary>
        SuffixMatch,

        /// <summary>
        /// Represents "*="
        /// </summary>
        SubstringMatch,
        
        /// <summary>
        /// Represents {string}
        /// </summary>
        String,

        /// <summary>
        /// Represents S* "+"
        /// </summary>
        Plus,

        /// <summary>
        /// Represents S* ">"
        /// </summary>
        Greater,

        /// <summary>
        /// Represents [ \t\r\n\f]+
        /// </summary>
        WhiteSpace,

        /// <summary>
        /// Represents {ident} ")"
        /// </summary>
        Function,

        /// <summary>
        /// Represents [0-9]+
        /// </summary>
        Integer,

        /// <summary>
        /// Represents S* "~"
        /// </summary>
        Tilde,

        /// <summary>
        /// Represents an arbitrary character
        /// </summary>
        Char,
    }
}