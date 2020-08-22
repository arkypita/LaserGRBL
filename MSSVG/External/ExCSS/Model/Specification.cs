namespace ExCSS.Model
{
    internal static class Specification
    {
        internal const char EndOfFile = (char)0x1a;
        internal const char Tilde = (char)0x7e;
        internal const char Pipe = (char)0x7c;
        internal const char Null = (char)0x0;
        internal const char Ampersand = (char)0x26;
        internal const char Hash = (char)0x23;
        internal const char DollarSign = (char)0x24;
        internal const char Simicolon = (char)0x3b;
        internal const char Asterisk = (char)0x2a;
        internal const char EqualSign = (char)0x3d;
        internal const char PlusSign = (char)0x2b;
        internal const char Comma = (char)0x2c;
        internal const char Period = (char)0x2e;
        internal const char Accent = (char)0x5e;
        internal const char At = (char)0x40;
        internal const char LessThan = (char)0x3c;
        internal const char GreaterThan = (char)0x3e;
        internal const char SingleQuote = (char)0x27;
        internal const char DoubleQuote = (char)0x22;
        internal const char QuestionMark = (char)0x3f;
        internal const char Tab = (char)0x09;
        internal const char LineFeed = (char)0x0a;
        internal const char CarriageReturn = (char)0x0d;
        internal const char FormFeed = (char)0x0c;
        internal const char Space = (char)0x20;
        internal const char Solidus = (char)0x2f;
        internal const char ReverseSolidus = (char)0x5c;
        internal const char Colon = (char)0x3a;
        internal const char Em = (char)0x21;
        internal const char MinusSign = (char)0x2d;
        internal const char Replacement = (char)0xfffd;
        internal const char Underscore = (char)0x5f;
        internal const char ParenOpen = (char)0x28;
        internal const char ParenClose = (char)0x29;
        internal const char Percent = (char)0x25;
        internal const char SquareBracketOpen =(char)0x5b;
        internal const char SquareBracketClose = (char)0x5d;
        internal const char CurlyBraceOpen = (char)0x7b;
        internal const char CurlyBraceClose = (char)0x7d;
        internal const int MaxPoint = 0x10FFFF;/// The maximum allowed codepoint (defined in Unicode).

        internal static bool IsNonPrintable(this char c)
        {
            return (c >= 0x0 && c <= 0x8) || (c >= 0xe && c <= 0x1f) || (c >= 0x7f && c <= 0x9f);
        }

        internal static bool IsLetter(this char c)
        {
            return IsUppercaseAscii(c) || IsLowercaseAscii(c);
        }

        internal static bool IsName(this char c)
        {
            return c >= 0x80 || c.IsLetter() || c == Underscore || c == MinusSign || IsDigit(c);
        }

        internal static bool IsNameStart(this char c)
        {
            return c >= 0x80 || IsUppercaseAscii(c) || IsLowercaseAscii(c) || c == Underscore;
        }

        internal static bool IsLineBreak(this char c)
        {
            //line feed, carriage return
            return c == LineFeed || c == CarriageReturn;
        }

        internal static bool IsSpaceCharacter(this char c)
        {
            //white space, tab, line feed, form feed, carriage return
            return c == Space || c == Tab || c == LineFeed || c == FormFeed || c == CarriageReturn;
        }

        internal static bool IsDigit(this char c)
        {
            return c >= 0x30 && c <= 0x39;
        }

        internal static bool IsUppercaseAscii(this char c)
        {
            return c >= 0x41 && c <= 0x5a;
        }

        internal static bool IsLowercaseAscii(this char c)
        {
            return c >= 0x61 && c <= 0x7a;
        }

        internal static bool IsHex(this char c)
        {
            return IsDigit(c) || (c >= 0x41 && c <= 0x46) || (c >= 0x61 && c <= 0x66);
        }
    }
}