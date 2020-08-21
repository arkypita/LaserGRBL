using System.Collections.Generic;
using System.IO;
using System.Text;
using ExCSS.Model;

namespace ExCSS
{
    internal class StylesheetReader
    {
        private int _insertion;
        private readonly Stack<int> _collengths;
        private TextReader _reader;
        private readonly StringBuilder _buffer;
        private bool _lineWithReturn;

        StylesheetReader()
        {
            _buffer = new StringBuilder();
            _collengths = new Stack<int>();
            Column = 1;
            Line = 1;
        }

        internal StylesheetReader(string styleText) : this()
        {
            _reader = new StringReader(styleText);
            ReadCurrent();
        }

        internal StylesheetReader(Stream styleStream) : this()
        {
            _reader = new StreamReader(styleStream, true);
            ReadCurrent();
        }

        internal bool IsBeginning
        {
            get { return _insertion < 2; }
        }

        internal int Line { get; private set; }

        internal int Column { get; private set; }

        internal bool IsEnded { get; private set; }

        internal bool IsEnding
        {
            get { return Current == Specification.EndOfFile; }
        }

        internal char Current { get; private set; }

        internal char Next
        {
            get
            {
                Advance();

                return Current;
            }
        }

        internal char Previous
        {
            get
            {
                Back();

                return Current;
            }
        }

        internal void Advance()
        {
            if (!IsEnding)
            {
                AdvanceUnsafe();
            }
            else if (!IsEnded)
            {
                IsEnded = true;
            }
        }

        internal void Advance(int positions)
        {
            while (positions-- > 0 && !IsEnding)
            {
                AdvanceUnsafe();
            }
        }

        internal void Back()
        {
            IsEnded = false;

            if (!IsBeginning)
            {
                BackUnsafe();
            }
        }

        internal void Back(int positions)
        {
            IsEnded = false;

            while (positions-- > 0 && !IsBeginning)
            {
                BackUnsafe();
            }
        }

        private void ReadCurrent()
        {
            if (_insertion < _buffer.Length)
            {
                Current = _buffer[_insertion];
                _insertion++;
                return;
            }

            var nextPosition = _reader.Read();
            Current = nextPosition == -1 ? Specification.EndOfFile : (char)nextPosition;

            if (Current == Specification.CarriageReturn)
            {
                Current = Specification.LineFeed;
                _lineWithReturn = true;
            }
            else if (_lineWithReturn)
            {
                _lineWithReturn = false;

                if (Current == Specification.LineFeed)
                {
                    ReadCurrent();
                    return;
                }
            }

            _buffer.Append(Current);
            _insertion++;
        }

        private void AdvanceUnsafe()
        {
            if (Current.IsLineBreak())
            {
                _collengths.Push(Column);
                Column = 1;
                Line++;
            }
            else
            {
                Column++;
            }

            ReadCurrent();
        }

        private void BackUnsafe()
        {
            _insertion--;

            if (_insertion == 0)
            {
                Column = 0;
                Current = Specification.Null;
                return;
            }

            Current = _buffer[_insertion - 1];

            if (Current.IsLineBreak())
            {
                Column = _collengths.Count != 0 ? _collengths.Pop() : 1;
                Line--;
            }
            else
            {
                Column--;
            }
        }
    }
}