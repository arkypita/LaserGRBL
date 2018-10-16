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
    using System.Diagnostics;

    #endregion

    /// <summary>
    /// Implementation for a selectors compiler that supports caching.
    /// </summary>
    /// <remarks>
    /// This class is primarily targeted for developers of selection
    /// over an arbitrary document model.
    /// </remarks>
    public static class SelectorsCachingCompiler
    {
        /// <summary>
        /// Creates a caching selectors compiler on top on an existing compiler.
        /// </summary>
        public static Func<string, T> Create<T>(Func<string, T> compiler)
        {
            return Create(compiler, null);
        }

        /// <summary>
        /// Creates a caching selectors compiler on top on an existing compiler.
        /// An addition parameter specified a dictionary to use as the cache.
        /// </summary>
        /// <remarks>
        /// If <paramref name="cache"/> is <c>null</c> then this method uses a
        /// the <see cref="Dictionary{TKey,TValue}"/> implementation with an 
        /// ordinally case-insensitive selectors text comparer.
        /// </remarks>
        public static Func<string, T> Create<T>(Func<string, T> compiler, IDictionary<string, T> cache)
        {
            if(compiler == null) throw new ArgumentNullException("compiler");
            return CreateImpl(compiler, cache ?? new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase));
        }

        private static Func<string, T> CreateImpl<T>(Func<string, T> compiler, IDictionary<string, T> cache)
        {
            Debug.Assert(compiler != null);
            Debug.Assert(cache != null);

            return selector =>
            {
                T compiled;
                return cache.TryGetValue(selector, out compiled) 
                     ? compiled 
                     : cache[selector] = compiler(selector);
            };
        }
    }
}