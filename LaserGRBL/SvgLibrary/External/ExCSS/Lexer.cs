using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ExCSS.Model;
using ExCSS.Model.TextBlocks;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    sealed class Lexer
    {
        private readonly StringBuilder _buffer;
        private readonly StylesheetReader _stylesheetReader;
        private bool _ignoreWhitespace;
        private bool _ignoreComments;
        internal Action<ParserError, string> ErrorHandler { get; set; }

        internal Lexer(StylesheetReader source)
        {
            _buffer = new StringBuilder();
            _stylesheetReader = source;

            ErrorHandler = (err, msg) => { };
        }

        private Block DataBlock(char current)
        {
            switch (current)
            {
                case Specification.LineFeed:
                case Specification.CarriageReturn:
                case Specification.Tab:
                case Specification.Space:
                    do
                    {
                        current = _stylesheetReader.Next;
                    }
                    while (current.IsSpaceCharacter());

                    if (_ignoreWhitespace)
                    {
                        return DataBlock(current);
                    }

                    _stylesheetReader.Back();
                    return SpecialCharacter.Whitespace;

                case Specification.DoubleQuote:
                    return DoubleQuoteString(_stylesheetReader.Next);

                case Specification.Hash:
                    return HashStart(_stylesheetReader.Next);

                case Specification.DollarSign:
                    current = _stylesheetReader.Next;

                    return current == Specification.EqualSign
                        ? MatchBlock.Suffix
                        : Block.Delim(_stylesheetReader.Previous);

                case Specification.SingleQuote:
                    return SingleQuoteString(_stylesheetReader.Next);

                case Specification.ParenOpen:
                    return BracketBlock.OpenRound;

                case Specification.ParenClose:
                    return BracketBlock.CloseRound;

                case Specification.Asterisk:
                    current = _stylesheetReader.Next;

                    return current == Specification.EqualSign
                        ? MatchBlock.Substring
                        : Block.Delim(_stylesheetReader.Previous);

                case Specification.PlusSign:
                    {
                        var nextFirst = _stylesheetReader.Next;

                        if (nextFirst == Specification.EndOfFile)
                        {
                            _stylesheetReader.Back();
                        }
                        else
                        {
                            var nextSEcond = _stylesheetReader.Next;
                            _stylesheetReader.Back(2);

                            if (nextFirst.IsDigit() || (nextFirst == Specification.Period && nextSEcond.IsDigit()))
                            {
                                return NumberStart(current);
                            }
                        }

                        return Block.Delim(current);
                    }

                case Specification.Comma:
                    return SpecialCharacter.Comma;

                case Specification.Period:
                    {
                        var c = _stylesheetReader.Next;

                        return c.IsDigit()
                            ? NumberStart(_stylesheetReader.Previous)
                            : Block.Delim(_stylesheetReader.Previous);
                    }

                case Specification.MinusSign:
                    {
                        var nextFirst = _stylesheetReader.Next;

                        if (nextFirst == Specification.EndOfFile)
                        {
                            _stylesheetReader.Back();
                        }
                        else
                        {
                            var nextSecond = _stylesheetReader.Next;
                            _stylesheetReader.Back(2);

                            if (nextFirst.IsDigit() || (nextFirst == Specification.Period && nextSecond.IsDigit()))
                            {
                                return NumberStart(current);
                            }
                            if (nextFirst.IsNameStart())
                            {
                                return IdentStart(current);
                            }
                            if (nextFirst == Specification.ReverseSolidus && !nextSecond.IsLineBreak() && nextSecond != Specification.EndOfFile)
                            {
                                return IdentStart(current);
                            }

                            if (nextFirst != Specification.MinusSign || nextSecond != Specification.GreaterThan)
                            {
                                return Block.Delim(current);
                            }
                            _stylesheetReader.Advance(2);

                            return _ignoreComments
                                ? DataBlock(_stylesheetReader.Next)
                                : CommentBlock.Close;
                        }

                        return Block.Delim(current);
                    }

                case Specification.Solidus:

                    current = _stylesheetReader.Next;

                    return current == Specification.Asterisk
                        ? Comment(_stylesheetReader.Next)
                        : Block.Delim(_stylesheetReader.Previous);

                case Specification.ReverseSolidus:
                    current = _stylesheetReader.Next;

                    if (current.IsLineBreak() || current == Specification.EndOfFile)
                    {
                        ErrorHandler(current == Specification.EndOfFile
                            ? ParserError.EndOfFile
                            : ParserError.UnexpectedLineBreak,
                            ErrorMessages.LineBreakEof);

                        return Block.Delim(_stylesheetReader.Previous);
                    }

                    return IdentStart(_stylesheetReader.Previous);

                case Specification.Colon:
                    return SpecialCharacter.Colon;

                case Specification.Simicolon:
                    return SpecialCharacter.Semicolon;

                case Specification.LessThan:
                    current = _stylesheetReader.Next;

                    if (current == Specification.Em)
                    {
                        current = _stylesheetReader.Next;

                        if (current == Specification.MinusSign)
                        {
                            current = _stylesheetReader.Next;

                            if (current == Specification.MinusSign)
                            {
                                return _ignoreComments
                                    ? DataBlock(_stylesheetReader.Next)
                                    : CommentBlock.Open;
                            }

                            current = _stylesheetReader.Previous;
                        }

                        current = _stylesheetReader.Previous;
                    }

                    return Block.Delim(_stylesheetReader.Previous);

                case Specification.At:
                    return AtKeywordStart(_stylesheetReader.Next);

                case Specification.SquareBracketOpen:
                    return BracketBlock.OpenSquare;

                case Specification.SquareBracketClose:
                    return BracketBlock.CloseSquare;

                case Specification.Accent:
                    current = _stylesheetReader.Next;

                    return current == Specification.EqualSign
                        ? MatchBlock.Prefix
                        : Block.Delim(_stylesheetReader.Previous);

                case Specification.CurlyBraceOpen:
                    return BracketBlock.OpenCurly;

                case Specification.CurlyBraceClose:
                    return BracketBlock.CloseCurly;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return NumberStart(current);

                case 'U':
                case 'u':
                    current = _stylesheetReader.Next;

                    if (current == Specification.PlusSign)
                    {
                        current = _stylesheetReader.Next;

                        if (current.IsHex() || current == Specification.QuestionMark)
                            return UnicodeRange(current);

                        current = _stylesheetReader.Previous;
                    }

                    return IdentStart(_stylesheetReader.Previous);

                case Specification.Pipe:
                    current = _stylesheetReader.Next;

                    if (current == Specification.EqualSign)
                    {
                        return MatchBlock.Dash;
                    }
                    if (current == Specification.Pipe)
                    {
                        return Block.Column;
                    }

                    return Block.Delim(_stylesheetReader.Previous);

                case Specification.Tilde:
                    current = _stylesheetReader.Next;

                    if (current == Specification.EqualSign)
                    {
                        return MatchBlock.Include;
                    }

                    return Block.Delim(_stylesheetReader.Previous);

                case Specification.EndOfFile:
                    return null;

                case Specification.Em:
                    current = _stylesheetReader.Next;

                    return current == Specification.EqualSign
                        ? MatchBlock.Not
                        : Block.Delim(_stylesheetReader.Previous);

                default:
                    return current.IsNameStart()
                        ? IdentStart(current)
                        : Block.Delim(current);
            }
        }

        private Block DoubleQuoteString(char current)
        {
            while (true)
            {
                switch (current)
                {
                    case Specification.DoubleQuote:
                    case Specification.EndOfFile:
                        return StringBlock.Plain(FlushBuffer());

                    case Specification.FormFeed:
                    case Specification.LineFeed:
                        ErrorHandler(ParserError.UnexpectedLineBreak, ErrorMessages.DoubleQuotedString);
                        _stylesheetReader.Back();
                        return StringBlock.Plain(FlushBuffer(), true);

                    case Specification.ReverseSolidus:
                        current = _stylesheetReader.Next;

                        if (current.IsLineBreak())
                        {
                            _buffer.AppendLine();
                        }
                        else if (current != Specification.EndOfFile)
                        {
                            _buffer.Append(ConsumeEscape(current));
                        }
                        else
                        {
                            ErrorHandler(ParserError.EndOfFile, ErrorMessages.DoubleQuotedStringEof);
                            _stylesheetReader.Back();
                            return StringBlock.Plain(FlushBuffer(), true);
                        }

                        break;

                    default:
                        _buffer.Append(current);
                        break;
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block SingleQuoteString(char current)
        {
            while (true)
            {
                switch (current)
                {
                    case Specification.SingleQuote:
                    case Specification.EndOfFile:
                        return StringBlock.Plain(FlushBuffer());

                    case Specification.FormFeed:
                    case Specification.LineFeed:
                        ErrorHandler(ParserError.UnexpectedLineBreak, ErrorMessages.SingleQuotedString);
                        _stylesheetReader.Back();
                        return (StringBlock.Plain(FlushBuffer(), true));

                    case Specification.ReverseSolidus:
                        current = _stylesheetReader.Next;

                        if (current.IsLineBreak())
                        {
                            _buffer.AppendLine();
                        }
                        else if (current != Specification.EndOfFile)
                        {
                            _buffer.Append(ConsumeEscape(current));
                        }
                        else
                        {
                            ErrorHandler(ParserError.EndOfFile, ErrorMessages.SingleQuotedStringEof);
                            _stylesheetReader.Back();
                            return (StringBlock.Plain(FlushBuffer(), true));
                        }

                        break;

                    default:
                        _buffer.Append(current);
                        break;
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block HashStart(char current)
        {
            if (current.IsNameStart())
            {
                _buffer.Append(current);
                return HashRest(_stylesheetReader.Next);
            }

            if (IsValidEscape(current))
            {
                current = _stylesheetReader.Next;
                _buffer.Append(ConsumeEscape(current));
                return HashRest(_stylesheetReader.Next);
            }

            if (current != Specification.ReverseSolidus)
            {
                _stylesheetReader.Back();
                return Block.Delim(Specification.Hash);
            }

            ErrorHandler(ParserError.InvalidCharacter, ErrorMessages.InvalidCharacterAfterHash);
            return Block.Delim(Specification.Hash);
        }

        private Block HashRest(char current)
        {
            while (true)
            {
                if (current.IsName())
                {
                    _buffer.Append(current);
                }
                else if (IsValidEscape(current))
                {
                    current = _stylesheetReader.Next;
                    _buffer.Append(ConsumeEscape(current));
                }
                else if (current == Specification.ReverseSolidus)
                {
                    ErrorHandler(ParserError.InvalidCharacter, ErrorMessages.InvalidCharacterAfterHash);

                    _stylesheetReader.Back();
                    return SymbolBlock.Hash(FlushBuffer());
                }
                else
                {
                    _stylesheetReader.Back();
                    return SymbolBlock.Hash(FlushBuffer());
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block Comment(char current)
        {
            while (true)
            {
                switch (current)
                {
                    case Specification.Asterisk:
                        current = _stylesheetReader.Next;
                        if (current == Specification.Solidus)
                        {
                            return DataBlock(_stylesheetReader.Next);
                        }
                        break;
                    case Specification.Solidus:
                        {
                            if (_stylesheetReader.Previous == Specification.Asterisk)
                            {
                                return DataBlock(_stylesheetReader.Next);
                            }
                            current = _stylesheetReader.Next;
                            break;
                        }
                    case Specification.EndOfFile:

                        ErrorHandler(ParserError.EndOfFile, ErrorMessages.ExpectedCommentEnd);

                        return DataBlock(current);
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block AtKeywordStart(char current)
        {
            if (current == Specification.MinusSign)
            {
                current = _stylesheetReader.Next;

                if (current.IsNameStart() || IsValidEscape(current))
                {
                    _buffer.Append(Specification.MinusSign);
                    return AtKeywordRest(current);
                }

                _stylesheetReader.Back(2);

                return Block.Delim(Specification.At);
            }

            if (current.IsNameStart())
            {
                _buffer.Append(current);
                return AtKeywordRest(_stylesheetReader.Next);
            }

            if (IsValidEscape(current))
            {
                current = _stylesheetReader.Next;
                _buffer.Append(ConsumeEscape(current));
                return AtKeywordRest(_stylesheetReader.Next);
            }

            _stylesheetReader.Back();
            return Block.Delim(Specification.At);

        }

        private Block AtKeywordRest(char current)
        {
            while (true)
            {
                if (current.IsName())
                {
                    _buffer.Append(current);
                }
                else if (IsValidEscape(current))
                {
                    current = _stylesheetReader.Next;
                    _buffer.Append(ConsumeEscape(current));
                }
                else
                {
                    _stylesheetReader.Back();
                    return SymbolBlock.At(FlushBuffer());
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block IdentStart(char current)
        {
            if (current == Specification.MinusSign)
            {
                current = _stylesheetReader.Next;

                if (current.IsNameStart() || IsValidEscape(current))
                {
                    _buffer.Append(Specification.MinusSign);
                    return IdentRest(current);
                }

                _stylesheetReader.Back();
                return Block.Delim(Specification.MinusSign);
            }

            if (current.IsNameStart())
            {
                _buffer.Append(current);
                return IdentRest(_stylesheetReader.Next);
            }

            if (current != Specification.ReverseSolidus)
            {
                return DataBlock(current);
            }

            if (!IsValidEscape(current))
            {
                return DataBlock(current);
            }

            current = _stylesheetReader.Next;
            _buffer.Append(ConsumeEscape(current));
            return IdentRest(_stylesheetReader.Next);
        }

        private Block IdentRest(char current)
        {
            while (true)
            {
                if (current.IsName())
                {
                    _buffer.Append(current);
                }
                else if (IsValidEscape(current))
                {
                    current = _stylesheetReader.Next;
                    _buffer.Append(ConsumeEscape(current));
                }
                else if (current == Specification.ParenOpen)
                {
                    switch (_buffer.ToString().ToLower())
                    {
                        case "url":
                            _buffer.Length = 0;
                            return UrlStart(_stylesheetReader.Next);//, GrammarSegment.Url);

                        case "domain":
                            _buffer.Length = 0;
                            return UrlStart(_stylesheetReader.Next);//, GrammarSegment.Domain);

                        case "url-prefix":
                            _buffer.Length = 0;
                            return UrlStart(_stylesheetReader.Next);//, GrammarSegment.UrlPrefix);

                        default:
                            return SymbolBlock.Function(FlushBuffer());
                    }

                }
                else
                {
                    _stylesheetReader.Back();
                    return SymbolBlock.Ident(FlushBuffer());
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block NumberStart(char current)
        {
            while (true)
            {
                switch (current)
                {
                    case Specification.MinusSign:
                    case Specification.PlusSign:
                        _buffer.Append(current);
                        current = _stylesheetReader.Next;
                        if (current == Specification.Period)
                        {
                            _buffer.Append(current);
                            _buffer.Append(_stylesheetReader.Next);

                            return NumberFraction(_stylesheetReader.Next);
                        }
                        _buffer.Append(current);
                        return NumberRest(_stylesheetReader.Next);

                    case Specification.Period:
                        _buffer.Append(current);
                        _buffer.Append(_stylesheetReader.Next);
                        return NumberFraction(_stylesheetReader.Next);

                    default:
                        if (current.IsDigit())
                        {
                            _buffer.Append(current);
                            return NumberRest(_stylesheetReader.Next);
                        }
                        break;
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block NumberRest(char current)
        {
            while (true)
            {
                if (current.IsDigit())
                {
                    _buffer.Append(current);
                }
                else if (current.IsNameStart())
                {
                    var number = FlushBuffer();
                    _buffer.Append(current);
                    return Dimension(_stylesheetReader.Next, number);
                }
                else if (IsValidEscape(current))
                {
                    current = _stylesheetReader.Next;
                    var number = FlushBuffer();
                    _buffer.Append(ConsumeEscape(current));
                    return Dimension(_stylesheetReader.Next, number);
                }
                else
                {
                    break;
                }

                current = _stylesheetReader.Next;
            }

            switch (current)
            {
                case Specification.Period:
                    current = _stylesheetReader.Next;

                    if (current.IsDigit())
                    {
                        _buffer.Append(Specification.Period).Append(current);
                        return NumberFraction(_stylesheetReader.Next);
                    }

                    _stylesheetReader.Back();
                    return Block.Number(FlushBuffer());

                case '%':
                    return UnitBlock.Percentage(FlushBuffer());

                case 'e':
                case 'E':
                    return NumberExponential(current);

                case Specification.MinusSign:
                    return NumberDash(current);

                default:
                    _stylesheetReader.Back();
                    return Block.Number(FlushBuffer());
            }
        }

        private Block NumberFraction(char current)
        {
            while (true)
            {
                if (current.IsDigit())
                {
                    _buffer.Append(current);
                }
                else if ("eE%-".IndexOf(current) >= 0)
                {
                    break;
                }
                else if (current.IsNameStart())
                {
                    var number = FlushBuffer();
                    _buffer.Append(current);

                    return Dimension(_stylesheetReader.Next, number);
                }
                else if (IsValidEscape(current))
                {
                    current = _stylesheetReader.Next;
                    var number = FlushBuffer();
                    _buffer.Append(ConsumeEscape(current));

                    return Dimension(_stylesheetReader.Next, number);
                }
                else
                {
                    break;
                }

                current = _stylesheetReader.Next;
            }

            switch (current)
            {
                case 'e':
                case 'E':
                    return NumberExponential(current);

                case '%':
                    return UnitBlock.Percentage(FlushBuffer());

                case Specification.MinusSign:
                    return NumberDash(current);

                default:
                    _stylesheetReader.Back();
                    return Block.Number(FlushBuffer());
            }
        }

        private Block Dimension(char current, string number)
        {
            while (true)
            {
                if (current.IsName())
                {
                    _buffer.Append(current);
                }
                else if (IsValidEscape(current))
                {
                    current = _stylesheetReader.Next;
                    _buffer.Append(ConsumeEscape(current));
                }
                else
                {
                    _stylesheetReader.Back();
                    return UnitBlock.Dimension(number, FlushBuffer());
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block SciNotation(char current)
        {
            while (true)
            {
                if (current.IsDigit())
                {
                    _buffer.Append(current);
                }
                else
                {
                    _stylesheetReader.Back();
                    return Block.Number(FlushBuffer());
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block UrlStart(char current)
        {
            while (current.IsSpaceCharacter())
            {
                current = _stylesheetReader.Next;
            }

            switch (current)
            {
                case Specification.EndOfFile:
                    ErrorHandler(ParserError.EndOfFile, ErrorMessages.InvalidUrlEnd);
                    return StringBlock.Url(string.Empty, true);

                case Specification.DoubleQuote:
                    return DoubleQuotedUrl(_stylesheetReader.Next);

                case Specification.SingleQuote:
                    return SingleQuoteUrl(_stylesheetReader.Next);

                case ')':
                    return StringBlock.Url(string.Empty);

                default:
                    return UnquotedUrl(current);
            }
        }

        private Block DoubleQuotedUrl(char current)
        {
            while (true)
            {
                if (current.IsLineBreak())
                {
                    ErrorHandler(ParserError.UnexpectedLineBreak, ErrorMessages.InvalidUrlEnd);
                    return BadUrl(_stylesheetReader.Next);
                }

                if (Specification.EndOfFile == current)
                {
                    return StringBlock.Url(FlushBuffer());
                }

                if (current == Specification.DoubleQuote)
                {
                    return UrlEnd(_stylesheetReader.Next);
                }

                if (current == Specification.ReverseSolidus)
                {
                    current = _stylesheetReader.Next;

                    if (current == Specification.EndOfFile)
                    {
                        _stylesheetReader.Back(2);
                        ErrorHandler(ParserError.EndOfFile, ErrorMessages.InvalidUrlEnd);
                        return StringBlock.Url(FlushBuffer(), true);
                    }

                    if (current.IsLineBreak())
                    {
                        _buffer.AppendLine();
                    }
                    else
                    {
                        _buffer.Append(ConsumeEscape(current));
                    }
                }
                else
                {
                    _buffer.Append(current);
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block SingleQuoteUrl(char current)
        {
            while (true)
            {
                if (current.IsLineBreak())
                {
                    ErrorHandler(ParserError.UnexpectedLineBreak, ErrorMessages.SingleQuotedString);
                    return BadUrl(_stylesheetReader.Next);
                }

                if (Specification.EndOfFile == current)
                {
                    return StringBlock.Url(FlushBuffer());
                }

                if (current == Specification.SingleQuote)
                {
                    return UrlEnd(_stylesheetReader.Next);
                }

                if (current == Specification.ReverseSolidus)
                {
                    current = _stylesheetReader.Next;

                    if (current == Specification.EndOfFile)
                    {
                        _stylesheetReader.Back(2);
                        ErrorHandler(ParserError.EndOfFile, ErrorMessages.SingleQuotedString);
                        return StringBlock.Url(FlushBuffer(), true);
                    }

                    if (current.IsLineBreak())
                    {
                        _buffer.AppendLine();
                    }
                    else
                    {
                        _buffer.Append(ConsumeEscape(current));
                    }
                }
                else
                {
                    _buffer.Append(current);
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block UnquotedUrl(char current)
        {
            while (true)
            {
                if (current.IsSpaceCharacter())
                {
                    return UrlEnd(_stylesheetReader.Next);
                }

                if (current == Specification.ParenClose || current == Specification.EndOfFile)
                {
                    return StringBlock.Url(FlushBuffer());
                }

                if (current == Specification.DoubleQuote || current == Specification.SingleQuote ||
                    current == Specification.ParenOpen || current.IsNonPrintable())
                {
                    ErrorHandler(ParserError.InvalidCharacter, ErrorMessages.InvalidUrlQuote);
                    return BadUrl(_stylesheetReader.Next);
                }

                if (current == Specification.ReverseSolidus)
                {
                    if (IsValidEscape(current))
                    {
                        current = _stylesheetReader.Next;
                        _buffer.Append(ConsumeEscape(current));
                    }
                    else
                    {
                        ErrorHandler(ParserError.InvalidCharacter, ErrorMessages.InvalidUrlCharacter);
                        return BadUrl(_stylesheetReader.Next);
                    }
                }
                else
                {
                    _buffer.Append(current);
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block UrlEnd(char current)
        {
            while (true)
            {
                if (current == Specification.ParenClose)
                {
                    return StringBlock.Url(FlushBuffer());
                }

                if (!current.IsSpaceCharacter())
                {
                    ErrorHandler(ParserError.InvalidCharacter, ErrorMessages.InvalidUrlCharacter);
                    return BadUrl(current);
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block BadUrl(char current)
        {
            while (true)
            {
                if (current == Specification.EndOfFile)
                {
                    ErrorHandler(ParserError.EndOfFile, ErrorMessages.InvalidUrlEnd);

                    return StringBlock.Url(FlushBuffer(), true);
                }

                if (current == Specification.ParenClose)
                {
                    return StringBlock.Url(FlushBuffer(), true);
                }

                if (IsValidEscape(current))
                {
                    current = _stylesheetReader.Next;
                    _buffer.Append(ConsumeEscape(current));
                }

                current = _stylesheetReader.Next;
            }
        }

        private Block UnicodeRange(char current)
        {
            for (var i = 0; i < 6; i++)
            {
                if (!current.IsHex())
                {
                    break;
                }

                _buffer.Append(current);
                current = _stylesheetReader.Next;
            }

            if (_buffer.Length != 6)
            {
                for (var i = 0; i < 6 - _buffer.Length; i++)
                {
                    if (current != Specification.QuestionMark)
                    {
                        current = _stylesheetReader.Previous;
                        break;
                    }

                    _buffer.Append(current);
                    current = _stylesheetReader.Next;
                }

                var range = FlushBuffer();
                var start = range.Replace(Specification.QuestionMark, '0');
                var end = range.Replace(Specification.QuestionMark, 'F');
                return Block.Range(start, end);
            }

            if (current == Specification.MinusSign)
            {
                current = _stylesheetReader.Next;

                if (current.IsHex())
                {
                    var start = _buffer.ToString();
                    _buffer.Length = 0;

                    for (var i = 0; i < 6; i++)
                    {
                        if (!current.IsHex())
                        {
                            current = _stylesheetReader.Previous;
                            break;
                        }

                        _buffer.Append(current);
                        current = _stylesheetReader.Next;
                    }

                    var end = FlushBuffer();
                    return Block.Range(start, end);
                }

                _stylesheetReader.Back(2);
                return Block.Range(FlushBuffer(), null);

            }

            _stylesheetReader.Back();
            return Block.Range(FlushBuffer(), null);
        }

        private string FlushBuffer()
        {
            var value = _buffer.ToString();
            _buffer.Length = 0;
            return value;
        }

        private Block NumberExponential(char current)
        {
            current = _stylesheetReader.Next;

            if (current.IsDigit())
            {
                _buffer.Append('e').Append(current);
                return SciNotation(_stylesheetReader.Next);
            }

            if (current == Specification.PlusSign || current == Specification.MinusSign)
            {
                var op = current;
                current = _stylesheetReader.Next;

                if (current.IsDigit())
                {
                    _buffer.Append('e').Append(op).Append(current);
                    return SciNotation(_stylesheetReader.Next);
                }

                _stylesheetReader.Back();
            }

            current = _stylesheetReader.Previous;
            var number = FlushBuffer();
            _buffer.Append(current);

            return Dimension(_stylesheetReader.Next, number);
        }

        private Block NumberDash(char current)
        {
            current = _stylesheetReader.Next;

            if (current.IsNameStart())
            {
                var number = FlushBuffer();
                _buffer.Append(Specification.MinusSign).Append(current);
                return Dimension(_stylesheetReader.Next, number);
            }

            if (IsValidEscape(current))
            {
                current = _stylesheetReader.Next;
                var number = FlushBuffer();
                _buffer.Append(Specification.MinusSign).Append(ConsumeEscape(current));
                return Dimension(_stylesheetReader.Next, number);
            }

            _stylesheetReader.Back(2);
            return Block.Number(FlushBuffer());
        }

        private string ConsumeEscape(char current)
        {
            if (!current.IsHex())
            {
                return current.ToString(CultureInfo.InvariantCulture);
            }

            var escape = new List<Char>();

            for (var i = 0; i < 6; i++)
            {
                escape.Add(current);
                current = _stylesheetReader.Next;

                if (!current.IsHex())
                {
                    break;
                }
            }

            current = _stylesheetReader.Previous;
            var code = int.Parse(new string(escape.ToArray()), NumberStyles.HexNumber);
            return Char.ConvertFromUtf32(code);
        }

        private bool IsValidEscape(char current)
        {
            if (current != Specification.ReverseSolidus)
            {
                return false;
            }

            current = _stylesheetReader.Next;
            _stylesheetReader.Back();

            if (current == Specification.EndOfFile)
            {
                return false;
            }

            return !current.IsLineBreak();
        }

        internal bool IgnoreWhitespace
        {
            get { return _ignoreWhitespace; }
            set { _ignoreWhitespace = value; }
        }

        internal bool IgnoreComments
        {
            get { return _ignoreComments; }
            set { _ignoreComments = value; }
        }

        internal StylesheetReader Stream
        {
            get { return _stylesheetReader; }
        }

        internal IEnumerable<Block> Tokens
        {
            get
            {
                while (true)
                {
                    var token = DataBlock(_stylesheetReader.Current);

                    if (token == null)
                    {
                        yield break;
                    }

                    _stylesheetReader.Advance();

                    yield return token;
                }
            }
        }
    }
}
