using System.Collections;
using System.Collections.Generic;
using ExCSS.Model.Extensions;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class MediaTypeList : IEnumerable<string>
    {
        private readonly List<string> _media;
        private string _buffer;

        internal MediaTypeList()
        {
            _buffer = string.Empty;
            _media = new List<string>();
        }

        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= _media.Count)
                {
                    return null;
                }

                return _media[index];
            }
            set
            {
                _media[index] = value;
            }
        }

        public int Count
        {
            get { return _media.Count; }
        }

        public string MediaType
        {
            get { return _buffer; }
            set
            {
                _buffer = string.Empty;
                _media.Clear();

                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                var entries = value.SplitOnCommas();

                foreach (var t in entries)
                {
                    AppendMedium(t);
                }
            }
        }

        internal MediaTypeList AppendMedium(string newMedium)
        {
            if (_media.Contains(newMedium))
            {
                return this;
            }

            _media.Add(newMedium);
            _buffer += (string.IsNullOrEmpty(_buffer) ? string.Empty : ",") + newMedium;

            return this;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool friendlyFormat, int indentation = 0)
        {
            return friendlyFormat
                ? string.Join(", ", _media.ToArray())
                : string.Join(",", _media.ToArray());
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>) _media).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}