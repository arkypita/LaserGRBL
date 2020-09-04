using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace Svg.Pathing
{
    [TypeConverter(typeof(SvgPathBuilder))]
    public sealed class SvgPathSegmentList : IList<SvgPathSegment>
    {
        internal SvgPath _owner;
        private List<SvgPathSegment> _segments;

        public SvgPathSegmentList()
        {
            this._segments = new List<SvgPathSegment>();
        }

        public SvgPathSegment Last
        {
            get { return this._segments[this._segments.Count-1]; }
        }

        public int IndexOf(SvgPathSegment item)
        {
            return this._segments.IndexOf(item);
        }

        public void Insert(int index, SvgPathSegment item)
        {
            this._segments.Insert(index, item);
            if (this._owner != null)
            {
                this._owner.OnPathUpdated();
            }
        }

        public void RemoveAt(int index)
        {
            this._segments.RemoveAt(index);
            if (this._owner != null)
            {
                this._owner.OnPathUpdated();
            }
        }

        public SvgPathSegment this[int index]
        {
            get { return this._segments[index]; }
            set { this._segments[index] = value; this._owner.OnPathUpdated(); }
        }

        public void Add(SvgPathSegment item)
        {
            this._segments.Add(item);
            if (this._owner != null)
            {
                this._owner.OnPathUpdated();
            }
        }

        public void Clear()
        {
            this._segments.Clear();
        }

        public bool Contains(SvgPathSegment item)
        {
            return this._segments.Contains(item);
        }

        public void CopyTo(SvgPathSegment[] array, int arrayIndex)
        {
            this._segments.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this._segments.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(SvgPathSegment item)
        {
            bool removed = this._segments.Remove(item);

            if (removed)
            {
                if (this._owner != null)
                {
                    this._owner.OnPathUpdated();
                }
            }

            return removed;
        }

        public IEnumerator<SvgPathSegment> GetEnumerator()
        {
            return this._segments.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._segments.GetEnumerator();
        }
    }
}