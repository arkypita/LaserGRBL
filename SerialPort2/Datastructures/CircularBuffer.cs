// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.Datastructures
{
    using System;
    using System.Diagnostics;
    using System.Text;

    /// <summary>
    /// A simple data structure to manage an array as a circular buffer.
    /// </summary>
    /// <remarks>
    /// This class provides simple methods for abstracting a circular buffer. A circular buffer allows for faster access
    /// of data by avoiding potential copy operations for data that is at the beginning.
    /// <para>
    /// Stream data structures can benefit from this data structure by allocating a single block on the heap of an
    /// arbitrary size. If the stream is long-lived the benefits are larger. In the .NET framework (4.0 and earlier),
    /// all allocations of data structures that are 80kb and larger are automatically allocated on the heap. The heap is
    /// not garbage collected like smaller objects. Instead, new elements are added to the heap in an incremental
    /// fashion. It is theoretically possible to exhaust all memory in an application by allocating and deallocating
    /// regularly on a heap if such a new heap element requires space and there is not a single block large enough. By
    /// using the <see cref="CircularBuffer{T}"/> with the type <c>T</c> as <c>byte</c>, you can preallocate a buffer
    /// for a stream of any reasonable size (as a simple example 5MB). That block is allocated once and remains for the
    /// lifetime of the stream. No time will be allocated for compacting or garbage collection.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">Type to use for the array.</typeparam>
    [DebuggerDisplay("Start = {Start}; Length = {Length}; Free = {Free}")]
    internal class CircularBuffer<T>
    {
        /// <summary>
        /// Circular buffer itself. Exposed by property "Array".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T[] m_Array;

        /// <summary>
        /// Start index into the buffer. Exposed by property "Start".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_Start;

        /// <summary>
        /// Length of data in circular buffer. Exposed by property "Length".
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_Count;

        /// <summary>
        /// Allocate an Array of type T[] of particular capacity.
        /// </summary>
        /// <param name="capacity">Size of array to allocate.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> must be positive.</exception>
        public CircularBuffer(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity), "capacity must be positive");
            m_Array = new T[capacity];
            m_Start = 0;
            m_Count = 0;
        }

        /// <summary>
        /// Circular buffer based on an already allocated array.
        /// </summary>
        /// <param name="array">Array (zero indexed) to allocate.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="array"/> must have at least one element;</exception>
        /// <remarks>
        /// The array is used as the storage for the circular buffer. No copy of the array is made. The initial index in
        /// the circular buffer is index 0 in the array. The array is assumed to be completely used (i.e. it is
        /// initialized with zero bytes Free).
        /// </remarks>
        public CircularBuffer(T[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) throw new ArgumentException("Array must have at least one element", nameof(array));
            m_Array = array;
            m_Start = 0;
            m_Count = array.Length;
        }

        /// <summary>
        /// Circular buffer based on an already allocated array.
        /// </summary>
        /// <param name="array">Array (zero indexed) to allocate.</param>
        /// <param name="count">Length of data in array, beginning from offset 0.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Initial <paramref name="count"/> must be within range of <paramref name="array"/>
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="array"/> must have at least one element;</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> may not be <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// The array is used as the storage for the circular buffer. No copy of the array is made, only a reference.
        /// The initial index in the array is 0. The value <paramref name="count"/> sets the initial length of the
        /// array. So an initial <paramref name="count"/> of zero would imply an empty circular buffer.
        /// </remarks>
        public CircularBuffer(T[] array, int count)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) throw new ArgumentException("Array must have at least one element", nameof(array));
            if (count < 0 || count > array.Length)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be within range of the array");

            m_Array = array;
            m_Start = 0;
            m_Count = count;
        }

        /// <summary>
        /// Circular buffer based on an already allocated array.
        /// </summary>
        /// <param name="array">Array (zero indexed) to allocate.</param>
        /// <param name="offset">Offset of first byte in the array.</param>
        /// <param name="count">
        /// Length of data in <paramref name="array"/>, wrapping to the start of the <paramref name="array"/>.
        /// </param>
        /// <exception cref="ArgumentException"><paramref name="array"/> must have at least one element;</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> must be within range of <paramref name="array"/>;
        /// <para>- or -</para>
        /// <paramref name="offset"/> exceeds the <paramref name="array"/> boundaries.
        /// </exception>
        /// <remarks>
        /// The array is used as the storage for the circular buffer. No copy of the array is made, only a reference.
        /// The <paramref name="offset"/> is defined to be the first entry in the circular buffer. This may be any value
        /// from zero to the last index ( <c>Array.Length - 1</c>). The value <paramref name="count"/> is the amount of
        /// data in the array, and it may cause wrapping (so that by setting offset near the end, a value of count may
        /// be set so that data can be considered at the end and beginning of the array given).
        /// </remarks>
        public CircularBuffer(T[] array, int offset, int count)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length == 0) throw new ArgumentException("Array must have at least one element", nameof(array));
            if (count < 0 || count > array.Length)
                throw new ArgumentOutOfRangeException(nameof(count), "must be within range of the array");
            if (offset < 0 || offset >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), "exceeds array boundaries");

            m_Array = array;
            m_Start = offset;
            m_Count = count;
        }

        /// <summary>
        /// Get start index into array where data begins.
        /// </summary>
        public int Start { get { return m_Start; } }

        /// <summary>
        /// Get end index into array where data ends.
        /// </summary>
        /// <remarks>
        /// This property is useful to know from what element in the underlying array that data can be written to.
        /// </remarks>
        public int End { get { return (m_Start + m_Count) % m_Array.Length; } }

        /// <summary>
        /// Get total length of data in array.
        /// </summary>
        /// <remarks>
        /// Returns the amount of allocated data in the circular buffer. The following rule applies:
        /// <see cref="Length"/> + <see cref="Free"/> = <see cref="Capacity"/>.
        /// </remarks>
        public int Length { get { return m_Count; } }

        /// <summary>
        /// Get total free data in array.
        /// </summary>
        /// <remarks>
        /// Returns the total amount of free elements in the circular buffer. The following rule applies:
        /// <see cref="Length"/> + <see cref="Free"/> = <see cref="Capacity"/>.
        /// </remarks>
        public int Free { get { return m_Array.Length - m_Count; } }

        /// <summary>
        /// Get the total capacity of the array.
        /// </summary>
        /// <remarks>
        /// Get the total number of elements allocated for the underlying array of the circular buffer. The following
        /// rule applies: <see cref="Length"/> + <see cref="Free"/> = <see cref="Capacity"/>.
        /// </remarks>
        public int Capacity { get { return m_Array.Length; } }

        /// <summary>
        /// Convert an index from the start of the data to read to an array index.
        /// </summary>
        /// <param name="index">
        /// Index in circular buffer, where an index of 0 is equivalent to the <see cref="Start"/> property.
        /// </param>
        /// <returns>Index in array that can be used in array based operations.</returns>
        public int ToArrayIndex(int index) { return (m_Start + index) % m_Array.Length; }

        /// <summary>
        /// Get length of continuous available space from the current position to the end of the array or until the
        /// buffer is full.
        /// </summary>
        /// <remarks>
        /// This function is useful if you need to pass the array to another function that will then fill the contents
        /// of the buffer. You would pass <see cref="End"/> as the offset for where writing the data should start, and
        /// <b>WriteLength</b> as the length of buffer space available until the end of the array buffer. After the read
        /// operation that writes in to your buffer, the array is completely full, or until the end of the array.
        /// <para>
        /// Such a property is necessary in case that the free space wraps around the buffer. Where below <c>X</c> is
        /// your stream you wish to read from, <c>b</c> is the circular buffer instantiated as the type
        /// <c>CircularBuffer{T}</c>.
        /// <code language="csharp">
        /// <![CDATA[
        ///  c = X.Read(b.Array, b.End, b.WriteLength);
        ///  b.Produce(c);]]>
        /// </code>
        /// If the property <b>WriteLength</b> is not zero, then there is space in the buffer to read data.
        /// </para>
        /// </remarks>
        public int WriteLength
        {
            get
            {
                if (m_Start + m_Count >= m_Array.Length) return m_Array.Length - m_Count;
                return m_Array.Length - m_Start - m_Count;
            }
        }

        /// <summary>
        /// Get the length of the continuous amount of data that can be read in a single copy operation from the start
        /// of the buffer data.
        /// </summary>
        /// <remarks>
        /// This function is useful if you need to pass the array to another function that will use the contents of the
        /// array. You would pass <see cref="Start"/> as the offset for reading data and <see cref="ReadLength"/> as the
        /// count. Then based on the amount of data operated on, you would free space with
        /// <c><see cref="Consume"/>(ReadLength).</c>
        /// </remarks>
        public int ReadLength
        {
            get
            {
                if (m_Start + m_Count >= m_Array.Length) return m_Array.Length - m_Start;
                return m_Count;
            }
        }

        /// <summary>
        /// Given an offset, calculate the length of data that can be read until the end of the block.
        /// </summary>
        /// <param name="offset">The offset into the circular buffer to test for the read length.</param>
        /// <returns>Length of the block that can be read from <paramref name="offset"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="offset"/> may not be negative.</exception>
        /// <remarks>
        /// Similar to the property <c>ReadLength</c>, this function takes an argument <c>offset</c> which is used to
        /// determine the length of data that can be read from that offset, until either the end of the block, or the
        /// end of the buffer.
        /// <para>
        /// This function is useful if you want to read a block of data, not starting from the offset 0 (and you don't
        /// want to consume the data before hand to reach an offset of zero).
        /// </para>
        /// <para>
        /// The example below, will calculate a checksum from the third byte in the block for the length of data. If the
        /// block to read from offset 3 can be done in one operation, it will do so. Else it must be done in two
        /// operations, first from offset 3 to the end, then from offset 0 for the remaining data.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code language="csharp">
        /// <![CDATA[
        /// short crc;
        /// if (buffer.GetReadBlock(3) >= length - 3) {
        ///   crc = crc16.Compute(buffer.Array, buffer.ToArrayIndex(3), length - 3);
        /// } else {
        ///   crc = crc16.Compute(buffer.Array, buffer.ToArrayIndex(3), buffer.ReadLength - 3);
        ///   crc = crc16.Compute(crc, buffer.Array, 0, length - buffer.ReadLength);
        /// }]]>
        /// </code>
        /// </example>
        public int GetReadBlock(int offset)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "may not be negative");
            if (offset >= m_Count) return 0;

            int s = (m_Start + offset) % m_Array.Length;
            int c = m_Count - offset;

            if (s + c >= m_Array.Length) return m_Array.Length - s;
            return c;
        }

        /// <summary>
        /// Consume array elements (freeing space from the beginning) updating pointers in the circular buffer.
        /// </summary>
        /// <param name="length">Amount of data to consume.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> is negative, or cannot consume more data than exists.
        /// </exception>
        /// <remarks>
        /// This method advances the internal pointers for <i>Start</i> based on the <i>length</i> that should be
        /// consumed. The pointer <i>End</i> does not change. It is important that this method does not <i>Reset()</i>
        /// the buffer in case that all data is consumed. A common scenario with Streams is to write into the buffer
        /// using asynchronous I/O. If a <i>Reset()</i> occurs during an asynchronous I/O <i>ReadFile()</i>, the
        /// <i>End</i> pointer is also changed, so that when a <i>Produce()</i> occurs on completion of the
        /// <i>ReadFile()</i> operation, the pointers are updated, but not using the pointers before the <i>Reset()</i>.
        /// No crash would occur (so long as the underlying array is pinned), but data corruption would occur if this
        /// method were not used in this particular scenario.
        /// </remarks>
        public void Consume(int length)
        {
            if (length < 0 || length > m_Count)
                throw new ArgumentOutOfRangeException(nameof(length), "Cannot consume negative length, or more data than exists");

            // Note, some implementations may rely on the pointers being correctly advanced also in
            // the case that data is consumed.
            m_Count -= length;
            m_Start = (m_Start + length) % m_Array.Length;
        }

        /// <summary>
        /// Produce bytes (allocating space at the end) updating pointers in the circular buffer.
        /// </summary>
        /// <param name="length">
        /// The number of bytes to indicate that have been added from the index <see cref="End"/> to the end of the
        /// array and possibly again from the start of the array if overlapped.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Cannot produce negative <paramref name="length"/>, or producing <paramref name="length"/> exceeds
        /// <see cref="Free"/>.
        /// </exception>
        public void Produce(int length)
        {
            if (length < 0 || length > m_Array.Length - m_Count)
                throw new ArgumentOutOfRangeException(nameof(length), "Cannot produce negative length, or exceed buffer free");

            m_Count += length;
        }

        /// <summary>
        /// Revert elements produced to the end of the circular buffer.
        /// </summary>
        /// <param name="length">
        /// The number of bytes to remove from the end of the array, moving the <see cref="End"/> property to the left,
        /// leaving the <see cref="Start"/> property untouched.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="length"/> must be positive and not exceed the number of elements in the circular buffer.
        /// </exception>
        /// <remarks>
        /// This method can be used to remove data that has been added to the end of the circular buffer. When using
        /// this data structure for streams, you would not use this property to ensure consistency of your stream (your
        /// <c>Read</c> operation would consume from your circular buffer and <c>Write</c> would produce data to your
        /// circular buffer.
        /// </remarks>
        public void Revert(int length)
        {
            if (length < 0 || length >= m_Count)
                throw new ArgumentOutOfRangeException(nameof(length),
                    "must be positive and not exceed the number of elements in the circular buffer");

            m_Count -= length;
        }

        /// <summary>
        /// Reset the pointers in the circular buffer, effectively noting the circular buffer as empty.
        /// </summary>
        public void Reset()
        {
            m_Count = 0;
            m_Start = 0;
        }

        /// <summary>
        /// Get the reference to the array that's allocated.
        /// </summary>
        /// <remarks>
        /// This property allows you to access the content of the data in the circular buffer in an efficient manner.
        /// You can then use this property along with <see cref="Start"/>, <see cref="ReadLength"/>, <see cref="End"/>
        /// and <see cref="WriteLength"/> for knowing where in the buffer to read and write.
        /// </remarks>
        public T[] Array { get { return m_Array; } }

        /// <summary>
        /// Access an element in the array using the Start as index 0.
        /// </summary>
        /// <param name="index">Index into the array referenced from <see cref="Start"/>.</param>
        /// <returns>Contents of the array.</returns>
        public T this[int index]
        {
            get
            {
#if DEBUG
                if (index >= Length) throw new ArgumentOutOfRangeException(nameof(index), "Index exceeded Buffer Length");
#endif
                return m_Array[(m_Start + index) % m_Array.Length];
            }
            set
            {
#if DEBUG
                if (index >= Length) throw new ArgumentOutOfRangeException(nameof(index), "Index exceeded Buffer Length");
#endif
                m_Array[(m_Start + index) % m_Array.Length] = value;
            }
        }

        /// <summary>
        /// Copy data from array to the end of this circular buffer and update the length.
        /// </summary>
        /// <param name="array">Array to copy from.</param>
        /// <returns>Number of bytes copied.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> may not be <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// Data is copied to the end of the Circular Buffer. The amount of data that could be copied is dependent on
        /// the amount of free space. The result is the number of elements from the <c>buffer</c> array that is copied
        /// into the Circular Buffer. Pointers in the circular buffer are updated appropriately.
        /// </remarks>
        public int Append(T[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            return Append(array, 0, array.Length);
        }

        /// <summary>
        /// Copy data from array to the end of this circular buffer and update the length.
        /// </summary>
        /// <param name="array">Array to copy from.</param>
        /// <param name="offset">Offset to copy data from.</param>
        /// <param name="count">Length of data to copy.</param>
        /// <returns>Number of bytes copied.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> or <paramref name="count"/> may not be negative.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="offset"/> and <paramref name="count"/> exceed <paramref name="array"/> boundaries.
        /// </exception>
        /// <remarks>
        /// Data is copied to the end of the Circular Buffer. The amount of data that could be copied is dependent on
        /// the amount of free space. The result is the number of elements from the <c>buffer</c> array that is copied
        /// into the Circular Buffer. Pointers in the circular buffer are updated appropriately.
        /// </remarks>
        public int Append(T[] array, int offset, int count)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "may not be negative");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "may not be negative");
            if (offset > array.Length - count) throw new ArgumentException("Parameters exceed array boundary");
            if (m_Count == Capacity) return 0;
            if (count == 0) return 0;

            if (count <= WriteLength) {
                System.Array.Copy(array, offset, m_Array, End, count);
                Produce(count);
                return count;
            } else {
                count = Math.Min(Free, count);
                System.Array.Copy(array, offset, m_Array, End, WriteLength);
                System.Array.Copy(array, offset + WriteLength, m_Array, 0, count - WriteLength);
                Produce(count);
                return count;
            }
        }

        /// <summary>
        /// Copy data from the circular buffer to the end of this circular buffer.
        /// </summary>
        /// <param name="buffer">Buffer to append.</param>
        /// <returns>Amount of data appended.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> may not be <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// Data is copied to the end of the Circular Buffer. The amount of data that could be copied is dependent on
        /// the amount of free space. The result is the number of elements from the <c>buffer</c> array that is copied
        /// into the Circular Buffer. Pointers in the circular buffer are updated appropriately.
        /// </remarks>
        public int Append(CircularBuffer<T> buffer)
        {
            return Append(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Copy data from the circular buffer to the end of this circular buffer.
        /// </summary>
        /// <param name="buffer">Buffer to append.</param>
        /// <param name="count">Number of bytes to append.</param>
        /// <returns>Amount of data appended.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="count"/> would exceed boundaries of <paramref name="buffer"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> may not be negative.</exception>
        /// <remarks>
        /// Data is copied to the end of the Circular Buffer. The amount of data that could be copied is dependent on
        /// the amount of free space. The result is the number of elements from the <c>buffer</c> array that is copied
        /// into the Circular Buffer. Pointers in the circular buffer are updated appropriately.
        /// </remarks>
        public int Append(CircularBuffer<T> buffer, int count)
        {
            return Append(buffer, 0, count);
        }

        /// <summary>
        /// Copy data from the circular buffer to the end of this circular buffer.
        /// </summary>
        /// <param name="buffer">Buffer to append.</param>
        /// <param name="count">Number of bytes to append.</param>
        /// <param name="offset">Offset into the buffer to start appending.</param>
        /// <returns>Amount of data appended.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> may not be negative;
        /// <para>- or -</para>
        /// <paramref name="count"/> may not be negative.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="offset"/> and <paramref name="count"/> would exceed boundaries of <paramref name="buffer"/>.
        /// </exception>
        /// <remarks>
        /// Data is copied to the end of the Circular Buffer. The amount of data that could be copied is dependent on
        /// the amount of free space. The result is the number of elements from the <c>buffer</c> array that is copied
        /// into the Circular Buffer. Pointers in the circular buffer are updated appropriately.
        /// </remarks>
        public int Append(CircularBuffer<T> buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "may not be negative");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "may not be negative");
            if (offset > buffer.Length - count) throw new ArgumentException("Parameters exceed buffer boundary");
            if (m_Count == Capacity) return 0;
            if (count == 0) return 0;

            int o = (buffer.Start + offset) % buffer.Capacity;
            int c = Math.Min(Free, count);
            int r = c;

            while (r > 0) {
                int rl = (o + r >= buffer.Capacity) ? (buffer.Capacity - o) : r;
                int cp = Math.Min(r, WriteLength);
                cp = Math.Min(cp, rl);
                System.Array.Copy(buffer.Array, o, m_Array, End, cp);
                Produce(cp);
                r -= cp;
                o = (o + cp) % buffer.Capacity;
            }
            return c;
        }

        /// <summary>
        /// Append a single element to the end of the Circular Buffer.
        /// </summary>
        /// <param name="element">The element to add at the end of the buffer.</param>
        /// <returns>Amount of data appended. 1 if successful, 0 if no space available.</returns>
        public int Append(T element)
        {
            if (m_Count == Capacity) return 0;

            m_Array[End] = element;
            Produce(1);
            return 1;
        }

        /// <summary>
        /// Retrieve a single element from the Circular buffer and consume it.
        /// </summary>
        /// <returns>The value at index 0.</returns>
        /// <exception cref="InvalidOperationException">Circular buffer is empty.</exception>
        public T Pop()
        {
            if (m_Count == 0) throw new InvalidOperationException("Circular Buffer is empty");
            T result = m_Array[m_Start];
            Consume(1);
            return result;
        }

        /// <summary>
        /// Copy data from the circular buffer to the array and then consume the data from the circular buffer.
        /// </summary>
        /// <param name="array">The array to copy the data to.</param>
        /// <returns>The number of bytes that were moved.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> may not be <see langword="null"/>.
        /// </exception>
        /// <remarks>Data is copied to the first element in the array, up to the length of the array.</remarks>
        public int MoveTo(T[] array)
        {
            int l = CopyTo(array);
            Consume(l);
            return l;
        }

        /// <summary>
        /// Copy data from the circular buffer to the array and then consume the data from the circular buffer.
        /// </summary>
        /// <param name="array">The array to copy the data to.</param>
        /// <param name="offset">Offset into the array to copy to.</param>
        /// <param name="count">Amount of data to copy to.</param>
        /// <returns>The number of bytes that were moved.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> may not be negative;
        /// <para>- or -</para>
        /// <paramref name="offset"/> may not be negative.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="offset"/> and <paramref name="count"/> would exceed <paramref name="array"/> length.
        /// </exception>
        /// <remarks>
        /// This method is very similar to the <see cref="CopyTo(T[], int, int)"/> method, but it will also consume the
        /// data that was copied also.
        /// </remarks>
        public int MoveTo(T[] array, int offset, int count)
        {
            int l = CopyTo(array, offset, count);
            Consume(l);
            return l;
        }

        /// <summary>
        /// Copy data from the circular buffer to the array.
        /// </summary>
        /// <param name="array">The array to copy the data to.</param>
        /// <returns>The number of bytes that were copied.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> may not be <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// Data is copied from the first element in the array, up to the length of the array. The data from the
        /// Circular Buffer is <i>not</i> consumed. You must do this yourself. Else use the MoveTo() method.
        /// </remarks>
        public int CopyTo(T[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            return CopyTo(array, 0, array.Length);
        }

        /// <summary>
        /// Copy data from the circular buffer to the array.
        /// </summary>
        /// <param name="array">The array to copy the data to.</param>
        /// <param name="offset">Offset into the array to copy to.</param>
        /// <param name="count">Amount of data to copy to.</param>
        /// <returns>The number of bytes that were copied.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> may not be negative;
        /// <para>- or -</para>
        /// <paramref name="offset"/> may not be negative.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="offset"/> and <paramref name="count"/> would exceed <paramref name="array"/> length.
        /// </exception>
        /// <remarks>
        /// Data is copied from the circular buffer into the array specified, at the offset given. The data from the
        /// Circular Buffer is <i>not</i> consumed. You must do this yourself. Else use the MoveTo() method.
        /// </remarks>
        public int CopyTo(T[] array, int offset, int count)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (count == 0) return 0;
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "may not be negative");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "may not be negative");
            if (offset > array.Length - count) throw new ArgumentException("Offset and count exceed boundary length");

            int length = Math.Min(count, Length);
            if (ReadLength >= count) {
                // The block of data is one continuous block to copy
                System.Array.Copy(m_Array, Start, array, offset, length);
                return length;
            } else {
                // The block of data wraps over
                System.Array.Copy(m_Array, Start, array, offset, ReadLength);
                System.Array.Copy(m_Array, 0, array, offset + ReadLength, length - ReadLength);
                return length;
            }
        }
    }

    /// <summary>
    /// A set of useful extensions to the CircularBuffer for specific data types.
    /// </summary>
    internal static class CircularBufferExtensions
    {
        /// <summary>
        /// Convert the contents of the circular buffer into a string.
        /// </summary>
        /// <param name="buff">The circular buffer based on char.</param>
        /// <returns>A string containing the contents of the circular buffer.</returns>
        /// <remarks>This method will not consume the data in the CircularBuffer{char}.</remarks>
        public static string GetString(this CircularBuffer<char> buff)
        {
            if (buff == null) return null;
            return buff.GetString(buff.Length);
        }

        /// <summary>
        /// Convert the contents of the circular buffer into a string.
        /// </summary>
        /// <param name="buff">The circular buffer based on char.</param>
        /// <param name="length">Number of characters to convert to a string.</param>
        /// <returns>A string of up to length characters.</returns>
        /// <remarks>This method will not consume the data in the CircularBuffer{char}.</remarks>
        public static string GetString(this CircularBuffer<char> buff, int length)
        {
            if (buff == null) return null;
            if (length == 0) return string.Empty;
            if (length > buff.Length) length = buff.Length;
            if (buff.Start + length > buff.Capacity) {
                StringBuilder sb = new StringBuilder(length);
                sb.Append(buff.Array, buff.Start, buff.Capacity - buff.Start);
                sb.Append(buff.Array, 0, length + buff.Start - buff.Capacity);
                return sb.ToString();
            }
            return new string(buff.Array, buff.Start, length);
        }

        /// <summary>
        /// Convert the contents of the circular buffer into a string.
        /// </summary>
        /// <param name="buff">The circular buffer based on char.</param>
        /// <param name="offset">The offset into the circular buffer.</param>
        /// <param name="length">Number of characters to convert to a string.</param>
        /// <returns>
        /// A string of up to length characters, from the circular buffer starting at the offset specified..
        /// </returns>
        /// <remarks>This method will not consume the data in the CircularBuffer{char}.</remarks>
        public static string GetString(this CircularBuffer<char> buff, int offset, int length)
        {
            if (buff == null) return null;
            if (length == 0) return string.Empty;
            if (offset > buff.Length) return string.Empty;
            if (offset + length > buff.Length) length = buff.Length - offset;

            int start = (buff.Start + offset) % buff.Capacity;
            if (start + length > buff.Capacity) {
                StringBuilder sb = new StringBuilder(length);
                sb.Append(buff.Array, start, buff.Capacity - start);
                sb.Append(buff.Array, 0, length + start - buff.Capacity);
                return sb.ToString();
            } else {
                return new string(buff.Array, start, length);
            }
        }

        /// <summary>
        /// Use a decoder to convert from a Circular Buffer of bytes into a char array.
        /// </summary>
        /// <param name="decoder">The decoder to do the conversion.</param>
        /// <param name="bytes">The circular buffer of bytes to convert from.</param>
        /// <param name="chars">An array to store the converted characters.</param>
        /// <param name="charIndex">The first element of <i>chars</i> in which data is stored.</param>
        /// <param name="charCount">Maximum number of characters to write.</param>
        /// <param name="flush">
        /// <see langword="true"/> to indicate that no further data is to be converted; otherwise,
        /// <see langword="false"/>.
        /// </param>
        /// <param name="bytesUsed">
        /// When this method returns, contains the number of bytes that were used in the conversion. This parameter is
        /// passed uninitialized.
        /// </param>
        /// <param name="charsUsed">
        /// When this method returns, contains the number of characters from chars that were produced by the conversion.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <param name="completed">
        /// When this method returns, contains <see langword="true"/> if all the characters specified by byteCount were
        /// converted; otherwise, <see langword="false"/>. This parameter is passed uninitialized.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The output buffer <paramref name="chars"/> is too small to contain any of the converted input.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> or <paramref name="chars"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="charIndex"/> may not be negative;
        /// <para>- or -</para>
        /// <paramref name="charCount"/> may not be negative.
        /// </exception>
        /// <remarks>
        /// This method should behave the same as the decoder for an array of bytes of equal size.
        /// <para>
        /// The <i>completed</i> output parameter indicates whether all the data in the input buffer was converted and
        /// stored in the output buffer. This parameter is set to <see langword="false"/> if the number of bytes
        /// specified by the <i>bytes.Length</i> parameter cannot be converted without exceeding the number of
        /// characters specified by the charCount parameter.
        /// </para>
        /// <para>
        /// The completed parameter can also be set to <see langword="false"/>, even though the all bytes were consumed.
        /// This situation occurs if there is still data in the Decoder object that has not been stored in the bytes
        /// buffer.
        /// </para>
        /// <para>
        /// There are a few noted deviations from using the Decoder on an array of bytes, instead of a Circular Buffer.
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// When converting a sequence of bytes to multiple chars, if those sequences result in the minimum number of
        /// characters being written as 2 or more characters, slight discrepancies occur. A UTF8 decoder would convert
        /// the sequence F3 A0 82 84 to the two characters DB40 DC84. The UTF8 decoder would not consume any of the 4
        /// bytes if all 4 bytes are immediately available to a single call to the Decoder.Convert() function and
        /// instead raise an exception. This Convert() function may consume some of these bytes and indicate success, if
        /// the byte sequence wraps over from the end of the array to the beginning of the array. The number of bytes
        /// consumed (bytesUsed) is correct and characters produced (charsUsed) is also correct. There is no error found
        /// according to the MS documentation. The next call will result in an exception instead. So this function may:
        /// consume more bytes than expected (but with the correct results); and may not raise an exception immediately
        /// if those bytes were consumed.
        /// </item>
        /// </list>
        /// </remarks>
        public static void Convert(this Decoder decoder, CircularBuffer<byte> bytes, char[] chars, int charIndex, int charCount, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (chars == null) throw new ArgumentNullException(nameof(chars));
            if (charIndex < 0) throw new ArgumentOutOfRangeException(nameof(charIndex), "may not be negative");
            if (charCount < 0) throw new ArgumentOutOfRangeException(nameof(charCount), "may not be negative");
            if (chars.Length - charIndex < charCount) throw new ArgumentException("charIndex and charCount exceed char buffer boundaries");

            bytesUsed = 0;
            charsUsed = 0;
            completed = true;
            bool outFlush = false;

            int rl = bytes.ReadLength;
            while (rl > 0 && charCount > 0) {
                int bu;
                int cu;
                if (rl == bytes.Length) outFlush = flush;
                try {
                    decoder.Convert(bytes.Array, bytes.Start, rl,
                        chars, charIndex, charCount,
                        outFlush, out bu, out cu, out completed);
                } catch (ArgumentException e) {
                    if (e.ParamName == null || !e.ParamName.Equals("chars")) throw;

                    // NOTE: While a decoder may not consume anything, using the CircularBuffer extension may, if the
                    // bytes need to be passed to the decoder twice. This is because we can't know what bytes may cause
                    // the error. The same kind of behavior would occur if you feed one byte at a time to the decoder
                    // yourself. It will be passed twice if the byte sequence is split between the end and the start of
                    // the circular queue.

                    if (bytesUsed == 0) throw;
                    completed = false;
                    return;
                }
                bytes.Consume(bu);
                bytesUsed += bu;
                charCount -= cu;
                charsUsed += cu;
                charIndex += cu;
                rl = bytes.ReadLength;
            }
        }

        /// <summary>
        /// Use a decoder to convert from a Circular Buffer of bytes into a Circular Buffer of chars.
        /// </summary>
        /// <param name="decoder">The decoder to do the conversion.</param>
        /// <param name="bytes">The circular buffer of bytes to convert from.</param>
        /// <param name="chars">The circular buffer of chars to convert to.</param>
        /// <param name="charCount">Maximum number of characters to write.</param>
        /// <param name="flush">
        /// <see langword="true"/> to indicate that no further data is to be converted; otherwise,
        /// <see langword="false"/>.
        /// </param>
        /// <param name="bytesUsed">
        /// When this method returns, contains the number of bytes that were used in the conversion. This parameter is
        /// passed uninitialized.
        /// </param>
        /// <param name="charsUsed">
        /// When this method returns, contains the number of characters from chars that were produced by the conversion.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <param name="completed">
        /// When this method returns, contains <see langword="true"/> if all the characters specified by byteCount were
        /// converted; otherwise, <see langword="false"/>. This parameter is passed uninitialized.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The output buffer <paramref name="chars"/> is too small to contain any of the converted input.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> or <paramref name="chars"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="charCount"/> may not be negative.</exception>
        public static void Convert(this Decoder decoder, CircularBuffer<byte> bytes, CircularBuffer<char> chars, int charCount, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (chars == null) throw new ArgumentNullException(nameof(chars));

            charCount = Math.Min(chars.Free, charCount);
            bytesUsed = 0;
            charsUsed = 0;
            completed = true;
            bool outFlush = false;

            int rl = bytes.ReadLength;
            while (rl > 0 && charCount > 0) {
                int bu;
                int cu;
                if (rl == bytes.Length) outFlush = flush;
                try {
                    decoder.Convert(bytes.Array, bytes.Start, rl,
                        chars.Array, chars.End, Math.Min(chars.WriteLength, charCount),
                        outFlush, out bu, out cu, out completed);
                    bytes.Consume(bu);
                    chars.Produce(cu);
                    rl = bytes.ReadLength;
                } catch (ArgumentException e) {
                    if (e.ParamName == null || !e.ParamName.Equals("chars")) throw;

                    // Decoder tried to write bytes, but not enough free space. We need to write to a temp array, then
                    // copy into the circular buffer. We assume that the underlying decoder hasn't changed state.
                    if (charCount <= chars.WriteLength) {
                        // There's no free space left, so we raise the same exception as the decoder
                        if (bytesUsed == 0) throw;
                        completed = false;
                        return;
                    }

                    int tmpLen = Math.Min(16, charCount);
                    char[] tmp = new char[tmpLen];
                    try {
                        decoder.Convert(bytes.Array, bytes.Start, rl,
                            tmp, 0, tmp.Length, outFlush, out bu, out cu, out completed);
                    } catch (ArgumentException e2) {
                        if (e2.ParamName == null || !e2.ParamName.Equals("chars")) throw;
                        if (bytesUsed == 0) throw;
                        completed = false;
                        return;
                    }
                    bytes.Consume(bu);
                    chars.Append(tmp, 0, cu);
                    rl = bytes.ReadLength;
                }
                bytesUsed += bu;
                charCount -= cu;
                charsUsed += cu;
            }
        }

        /// <summary>
        /// Use a decoder to convert from a Circular Buffer of bytes into a Circular Buffer of chars.
        /// </summary>
        /// <param name="decoder">The decoder to do the conversion.</param>
        /// <param name="bytes">The circular buffer of bytes to convert from.</param>
        /// <param name="chars">The circular buffer of chars to convert to.</param>
        /// <param name="flush">
        /// <see langword="true"/> to indicate that no further data is to be converted; otherwise,
        /// <see langword="false"/>.
        /// </param>
        /// <param name="bytesUsed">
        /// When this method returns, contains the number of bytes that were used in the conversion. This parameter is
        /// passed uninitialized.
        /// </param>
        /// <param name="charsUsed">
        /// When this method returns, contains the number of characters from chars that were produced by the conversion.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <param name="completed">
        /// When this method returns, contains <see langword="true"/> if all the characters specified by byteCount were
        /// converted; otherwise, <see langword="false"/>. This parameter is passed uninitialized.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The output buffer <paramref name="chars"/> is too small to contain any of the converted input.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> or <paramref name="chars"/> may not be <see langword="null"/>.
        /// </exception>
        public static void Convert(this Decoder decoder, CircularBuffer<byte> bytes, CircularBuffer<char> chars, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (chars == null) throw new ArgumentNullException(nameof(chars));
            decoder.Convert(bytes, chars, chars.Free, flush, out bytesUsed, out charsUsed, out completed);
        }

        /// <summary>
        /// Use a decoder to convert from an array of bytes into a char CircularBuffer.
        /// </summary>
        /// <param name="decoder">The decoder to do the conversion.</param>
        /// <param name="bytes">The array of bytes to convert.</param>
        /// <param name="byteIndex">Start index in bytes array.</param>
        /// <param name="byteCount">Number of bytes to convert in the byte array.</param>
        /// <param name="chars">The circular buffer of chars to convert to.</param>
        /// <param name="flush">
        /// <see langword="true"/> to indicate that no further data is to be converted; otherwise,
        /// <see langword="false"/>.
        /// </param>
        /// <param name="bytesUsed">
        /// When this method returns, contains the number of bytes that were used in the conversion. This parameter is
        /// passed uninitialized.
        /// </param>
        /// <param name="charsUsed">
        /// When this method returns, contains the number of characters from chars that were produced by the conversion.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <param name="completed">
        /// When this method returns, contains <see langword="true"/> if all the characters specified by byteCount were
        /// converted; otherwise, <see langword="false"/>. This parameter is passed uninitialized.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The output buffer <paramref name="bytes"/> is too small to contain any of the converted input.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> or <paramref name="chars"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="byteIndex"/> may not be negative;
        /// <para>- or -</para>
        /// <paramref name="byteCount"/> may not be negative.
        /// </exception>
        public static void Convert(this Decoder decoder, byte[] bytes, int byteIndex, int byteCount, CircularBuffer<char> chars, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (chars == null) throw new ArgumentNullException(nameof(chars));
            if (byteIndex < 0) throw new ArgumentOutOfRangeException(nameof(byteIndex), "may not be negative");
            if (byteCount < 0) throw new ArgumentOutOfRangeException(nameof(byteCount), "may not be negative");
            if (bytes.Length - byteIndex < byteCount) throw new ArgumentException("byteIndex and byteCount exceed byte buffer boundaries");

            bytesUsed = 0;
            charsUsed = 0;
            completed = true;
            if (byteCount == 0) return;

            do {
                int bu;
                int cu;
                try {
                    if (bytesUsed != 0 && chars.WriteLength == 0) {
                        completed = false;
                        return;
                    }
                    decoder.Convert(bytes, byteIndex, byteCount,
                        chars.Array, chars.End, chars.WriteLength,
                        flush, out bu, out cu, out completed);
                    byteCount -= bu;
                    bytesUsed += bu;
                    byteIndex += bu;
                    chars.Produce(cu);
                    charsUsed += cu;
                } catch (ArgumentException e) {
                    if (e.ParamName == null || !e.ParamName.Equals("chars")) throw;

                    // Decoder tried to write bytes, but not enough free space. We need to write to a temp array, then
                    // copy into the circular buffer. We assume that the underlying decoder hasn't changed state.
                    if (chars.WriteLength == chars.Free) {
                        // There's no free space left, so we raise the same exception as the decoder
                        if (bytesUsed == 0) throw;
                        completed = false;
                        return;
                    }

                    int tempLen = Math.Min(16, chars.Free);
                    char[] tmp = new char[tempLen];
                    try {
                        decoder.Convert(bytes, byteIndex, byteCount,
                            tmp, 0, tmp.Length, flush, out bu, out cu, out completed);
                    } catch (ArgumentException e2) {
                        // There still isn't enough space, so abort
                        if (e2.ParamName == null || !e2.ParamName.Equals("chars")) throw;
                        if (bytesUsed == 0) throw;
                        completed = false;
                        return;
                    }
                    byteCount -= bu;
                    bytesUsed += bu;
                    byteIndex += bu;
                    chars.Append(tmp, 0, cu);
                    charsUsed += cu;
                }
            } while (!completed);
        }

        /// <summary>
        /// Converts an array of Unicode characters to a byte sequence storing the result in a circular buffer.
        /// </summary>
        /// <param name="encoder">The encoder to use for the conversion.</param>
        /// <param name="chars">An array of characters to convert.</param>
        /// <param name="charIndex">The first element of <i>chars</i> to convert.</param>
        /// <param name="charCount">The number of elements of <i>chars</i> to convert.</param>
        /// <param name="bytes">Circular buffer where converted bytes are stored.</param>
        /// <param name="flush">
        /// <see langword="true"/> to indicate no further data is to be converted; otherwise, <see langword="false"/>
        /// </param>
        /// <param name="charsUsed">
        /// When this method returns, contains the number of characters from chars that were produced by the conversion.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <param name="bytesUsed">
        /// When this method returns, contains the number of bytes that were used in the conversion. This parameter is
        /// passed uninitialized.
        /// </param>
        /// <param name="completed">
        /// When this method returns, contains <see langword="true"/> if all the characters specified by byteCount were
        /// converted; otherwise, <see langword="false"/>. This parameter is passed uninitialized.
        /// </param>
        /// <exception cref="ArgumentException">
        /// The output buffer <paramref name="chars"/> is too small to contain any of the converted input.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> or <paramref name="chars"/> may not be <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="charIndex"/> may not be negative;
        /// <para>- or -</para>
        /// <paramref name="charCount"/> may not be negative.
        /// </exception>
        public static void Convert(this Encoder encoder, char[] chars, int charIndex, int charCount, CircularBuffer<byte> bytes, bool flush, out int charsUsed, out int bytesUsed, out bool completed)
        {
            // The code here is the same as the "Decoder" version as they do the same thing. Unfortunately, .NET doesn't
            // have a base class for this, so we need two separate encoder/decoder methods.

            if (chars == null) throw new ArgumentNullException(nameof(chars));
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (charIndex < 0) throw new ArgumentOutOfRangeException(nameof(charIndex), "may not be negative");
            if (charCount < 0) throw new ArgumentOutOfRangeException(nameof(charCount), "may not be negative");
            if (chars.Length - charIndex < charCount) throw new ArgumentException("charIndex and charCount exceed char buffer boundaries");

            bytesUsed = 0;
            charsUsed = 0;
            completed = true;
            if (charCount == 0) return;

            do {
                int bu;
                int cu;
                try {
                    if (charsUsed != 0 && bytes.WriteLength == 0) {
                        completed = false;
                        return;
                    }

                    encoder.Convert(chars, charIndex, charCount,
                        bytes.Array, bytes.End, bytes.WriteLength,
                        flush, out cu, out bu, out completed);
                    charCount -= cu;
                    charsUsed += cu;
                    charIndex += cu;
                    bytes.Produce(bu);
                    bytesUsed += bu;
                } catch (ArgumentException e) {
                    if (e.ParamName == null || !e.ParamName.Equals("bytes")) throw;

                    // Encoder tried to write chars, but not enough free space. We need to write to a temp array, then
                    // copy into the circular buffer. We assume that the underlying encoder hasn't changed state.
                    if (bytes.WriteLength == bytes.Free) {
                        // There's no free space left, so we raise the same exception as the decoder
                        if (charsUsed == 0) throw;
                        completed = false;
                        return;
                    }

                    int tempLen = Math.Min(16, bytes.Free);
                    byte[] tmp = new byte[tempLen];
                    try {
                        encoder.Convert(chars, charIndex, charCount,
                            tmp, 0, tmp.Length, flush, out cu, out bu, out completed);
                    } catch (ArgumentException e2) {
                        // There still isn't enough space, so abort
                        if (e2.ParamName == null || !e2.ParamName.Equals("bytes")) throw;
                        if (charsUsed == 0) throw;
                        completed = false;
                        return;
                    }
                    charCount -= cu;
                    charsUsed += cu;
                    charIndex += cu;
                    bytes.Append(tmp, 0, bu);
                    bytesUsed += bu;
                }
            } while (!completed);
        }
    }
}
