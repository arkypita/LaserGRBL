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
    using System.Collections;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Adds reading semantics to a base <see cref="IEnumerator{T}"/> with the 
    /// option to un-read and insert new elements while consuming the source.
    /// </summary>
    public sealed class Reader<T> : IDisposable, IEnumerable<T>
    {
        private IEnumerator<T> _enumerator;
        private Stack<T> _buffer;

        /// <summary>
        /// Initialize a new <see cref="Reader{T}"/> with a base
        /// <see cref="IEnumerable{T}"/> object.
        /// </summary>
        public Reader(IEnumerable<T> e) :
            this(CheckNonNull(e).GetEnumerator()) { }

        private static IEnumerable<T> CheckNonNull(IEnumerable<T> e)
        {
            if (e == null) throw new ArgumentNullException("e");
            return e;
        }

        /// <summary>
        /// Initialize a new <see cref="Reader{T}"/> with a base
        /// <see cref="IEnumerator{T}"/> object.
        /// </summary>
        public Reader(IEnumerator<T> e)
        {
            if(e == null) throw new ArgumentNullException("e");
            _enumerator = e;
            _buffer = new Stack<T>();
            RealRead();
        }

        /// <summary>
        /// Indicates whether there is, at least, one value waiting to be read or not.
        /// </summary>
        public bool HasMore
        {
            get
            {
                EnsureAlive();
                return _buffer.Count > 0;
            }
        }

        /// <summary>
        /// Pushes back a new value that will be returned on the next read.
        /// </summary>
        public void Unread(T value)
        {
            EnsureAlive();
            _buffer.Push(value);
        }

        /// <summary>
        /// Reads and returns the next value.
        /// </summary>
        public T Read()
        {
            if (!HasMore)
                throw new InvalidOperationException();

            var value = _buffer.Pop();

            if (_buffer.Count == 0)
                RealRead();

            return value;
        }

        /// <summary>
        /// Peeks the next value waiting to be read.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if there is no value waiting to be read.
        /// </exception>
        public T Peek()
        {
            if (!HasMore)
                throw new InvalidOperationException();

            return _buffer.Peek();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the remaining
        /// values to be read.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            EnsureAlive();
            return GetEnumeratorImpl();
        }

        private IEnumerator<T> GetEnumeratorImpl()
        {
            while (HasMore)
                yield return Read();
        }

        private void RealRead()
        {
            EnsureAlive();

            if (_enumerator.MoveNext())
                Unread(_enumerator.Current);
        }

        /// <summary>
        /// Disposes the enumerator used to initialize this object
        /// if that enumerator supports <see cref="IDisposable"/>.
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        void Dispose()
        {
            if(_enumerator == null) 
                return;
            _enumerator.Dispose();
            _enumerator = null;
            _buffer = null;
        }

        private void EnsureAlive()
        {
            if (_enumerator == null)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}