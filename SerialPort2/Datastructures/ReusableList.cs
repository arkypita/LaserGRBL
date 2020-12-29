namespace RJCP.Datastructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Array of elements which can be reused.
    /// </summary>
    /// <typeparam name="T">The element type that is to be kept.</typeparam>
    /// <remarks>
    /// Behave like a list, that when we convert to an array we don't allocate new
    /// objects on the heap like List{T}.
    /// </remarks>
    internal class ReusableList<T> : IList<T> where T : class
    {
        private int m_Count;
        private int m_MinCapacity;
        private T[] m_ReusableList;
        private T[][] m_ReusableListCache;

        public ReusableList(int minCapacity, int maxCapacity)
        {
            if (minCapacity < 1) throw new ArgumentOutOfRangeException("minCapacity", "Minimum capacity must be 1 or greater");
            if (maxCapacity < 1) throw new ArgumentOutOfRangeException("maxCapacity", "Maximum capacity must be 1 or greater");
            if (maxCapacity < minCapacity) throw new ArgumentException("Maximum capacity must be greater/equal than Minimum capacity");

            m_ReusableList = new T[maxCapacity];
            m_ReusableListCache = new T[maxCapacity - minCapacity + 1][];
            m_MinCapacity = minCapacity;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0) throw new ArgumentOutOfRangeException("index", "Index is less than zero");
                if (index >= Count) throw new ArgumentOutOfRangeException("index", "Index is more than Count");

                return m_ReusableList[index];
            }
            set
            {
                if (index < 0) throw new ArgumentOutOfRangeException("index", "Index is less than zero");
                if (index >= Count) throw new ArgumentOutOfRangeException("index", "Index is more than Count");

                m_ReusableList[index] = value;
            }
        }

        public int Count { get { return m_Count; } }

        public bool IsReadOnly { get { return false; } }

        public void Add(T item)
        {
            if (m_Count == m_ReusableList.Length) throw new InvalidOperationException("List is full");
            m_ReusableList[m_Count] = item;
            m_Count++;
        }

        public void Clear()
        {
            m_Count = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < m_Count; i++) {
                if (item == null) return m_ReusableList[i] == null;
                if (item.Equals(m_ReusableList[i])) return true;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than lower bound of array");
            if (array.Rank != 1) throw new ArgumentException("array is multidimensional", "array");
            if (m_Count + arrayIndex > array.Length) throw new ArgumentException("The number of elements in source array is greater than available elements from index to the end of the destination array");

            for (int i = 0; i < m_Count; i++) {
                array[i + arrayIndex] = m_ReusableList[i];
            }
        }

        private sealed class ArrayEnumerator : IEnumerator<T>
        {
            private ReusableList<T> m_Parent;
            private int m_Index;

            public ArrayEnumerator(ReusableList<T> parent)
            {
                m_Parent = parent;
            }

            public T Current { get; private set; }

            object IEnumerator.Current { get { return Current; } }

            public bool MoveNext()
            {
                if (m_Index >= m_Parent.m_Count) return false;
                Current = m_Parent.m_ReusableList[m_Index];
                m_Index++;
                return true;
            }

            public void Reset()
            {
                m_Index = 0;
            }

            public void Dispose()
            {
                /* Nothing to dispose */
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ArrayEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < m_Count; i++) {
                if (item == null && m_ReusableList[i] == null) return i;
                if (item != null && item.Equals(m_ReusableList[i])) return i;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (m_Count == m_ReusableList.Length) throw new InvalidOperationException("List is full");
            if (index > m_Count) throw new ArgumentOutOfRangeException("index", "Index greater than Count");
            for (int i = m_Count; i > index; i--) {
                m_ReusableList[i] = m_ReusableList[i - 1];
            }
            m_ReusableList[index] = item;
            m_Count++;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            if (index >= Count) throw new ArgumentOutOfRangeException("index", "Index is more than Count");
            for (int i = index; i < m_Count; i++) {
                m_ReusableList[i] = m_ReusableList[i + 1];
            }
            m_Count--;
        }

        public T[] ToArray()
        {
            T[] result;
            if (m_Count < m_MinCapacity) {
                result = new T[m_Count];
            } else {
                int index = m_Count - m_MinCapacity;
                if (m_ReusableListCache[index] == null) {
                    result = new T[m_Count];
                    m_ReusableListCache[index] = result;
                } else {
                    result = m_ReusableListCache[index];
                }
            }

            for (int i = 0; i < m_Count; i++) {
                result[i] = m_ReusableList[i];
            }
            return result;
        }
    }
}
