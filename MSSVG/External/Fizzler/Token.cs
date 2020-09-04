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
    using System;

    /// <summary>
    /// Represent a token and optionally any text associated with it.
    /// </summary>
    public struct Token : IEquatable<Token>
    {
        /// <summary>
        /// Gets the kind/type/class of the token.
        /// </summary>
        public TokenKind Kind { get; private set; }

        /// <summary>
        /// Gets text, if any, associated with the token.
        /// </summary>
        public string Text { get; private set; }

        private Token(TokenKind kind) : this(kind, null) {}

        private Token(TokenKind kind, string text) : this()
        {
            Kind = kind;
            Text = text;
        }

        /// <summary>
        /// Creates an end-of-input token.
        /// </summary>
        public static Token Eoi()
        {
            return new Token(TokenKind.Eoi);
        }

        private static readonly Token _star = Char('*');
        private static readonly Token _dot = Char('.');
        private static readonly Token _colon = Char(':');
        private static readonly Token _comma = Char(',');
        private static readonly Token _rightParenthesis = Char(')');
        private static readonly Token _equals = Char('=');
        private static readonly Token _pipe = Char('|');
        private static readonly Token _leftBracket = Char('[');
        private static readonly Token _rightBracket = Char(']');

        /// <summary>
        /// Creates a star token.
        /// </summary>
        public static Token Star()
        {
            return _star;
        }

        /// <summary>
        /// Creates a dot token.
        /// </summary>
        public static Token Dot()
        {
            return _dot;
        }

        /// <summary>
        /// Creates a colon token.
        /// </summary>
        public static Token Colon()
        {
            return _colon;
        }

        /// <summary>
        /// Creates a comma token.
        /// </summary>
        public static Token Comma()
        {
            return _comma;
        }

        /// <summary>
        /// Creates a right parenthesis token.
        /// </summary>
        public static Token RightParenthesis()
        {
            return _rightParenthesis;
        }

        /// <summary>
        /// Creates an equals token.
        /// </summary>
        public static Token Equals()
        {
            return _equals;
        }

        /// <summary>
        /// Creates a left bracket token.
        /// </summary>
        public static Token LeftBracket()
        {
            return _leftBracket;
        }

        /// <summary>
        /// Creates a right bracket token.
        /// </summary>
        public static Token RightBracket()
        {
            return _rightBracket;
        }

        /// <summary>
        /// Creates a pipe (vertical line) token.
        /// </summary>
        public static Token Pipe()
        {
            return _pipe;
        }

        /// <summary>
        /// Creates a plus token.
        /// </summary>
        public static Token Plus()
        {
            return new Token(TokenKind.Plus);
        }

        /// <summary>
        /// Creates a greater token.
        /// </summary>
        public static Token Greater()
        {
            return new Token(TokenKind.Greater);
        }

        /// <summary>
        /// Creates an includes token.
        /// </summary>
        public static Token Includes()
        {
            return new Token(TokenKind.Includes);
        }

        /// <summary>
        /// Creates a dash-match token.
        /// </summary>
        public static Token DashMatch()
        {
            return new Token(TokenKind.DashMatch);
        }

        /// <summary>
        /// Creates a prefix-match token.
        /// </summary>
        public static Token PrefixMatch()
        {
            return new Token(TokenKind.PrefixMatch);
        }

        /// <summary>
        /// Creates a suffix-match token.
        /// </summary>
        public static Token SuffixMatch()
        {
            return new Token(TokenKind.SuffixMatch);
        }

        /// <summary>
        /// Creates a substring-match token.
        /// </summary>
        public static Token SubstringMatch()
        {
            return new Token(TokenKind.SubstringMatch);
        }

        /// <summary>
        /// Creates a general sibling token.
        /// </summary>
        public static Token Tilde()
        {
            return new Token(TokenKind.Tilde);
        }

        /// <summary>
        /// Creates an identifier token.
        /// </summary>
        public static Token Ident(string text)
        {
            ValidateTextArgument(text);
            return new Token(TokenKind.Ident, text);
        }

        /// <summary>
        /// Creates an integer token.
        /// </summary>
        public static Token Integer(string text)
        {
            ValidateTextArgument(text);
            return new Token(TokenKind.Integer, text);
        }

        /// <summary>
        /// Creates a hash-name token.
        /// </summary>
        public static Token Hash(string text)
        {
            ValidateTextArgument(text);
            return new Token(TokenKind.Hash, text);
        }

        /// <summary>
        /// Creates a white-space token.
        /// </summary>
        public static Token WhiteSpace(string space)
        {
            ValidateTextArgument(space);
            return new Token(TokenKind.WhiteSpace, space);
        }

        /// <summary>
        /// Creates a string token.
        /// </summary>
        public static Token String(string text)
        {
            return new Token(TokenKind.String, text ?? string.Empty);
        }

        /// <summary>
        /// Creates a function token.
        /// </summary>
        public static Token Function(string text)
        {
            ValidateTextArgument(text);
            return new Token(TokenKind.Function, text);
        }

        /// <summary>
        /// Creates an arbitrary character token.
        /// </summary>
        public static Token Char(char ch)
        {
            return new Token(TokenKind.Char, ch.ToString());
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj != null && obj is Token && Equals((Token) obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return Text == null ? Kind.GetHashCode() : Kind.GetHashCode() ^ Text.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public bool Equals(Token other)
        {
            return Kind == other.Kind && Text == other.Text;
        }

        /// <summary>
        /// Gets a string representation of the token.
        /// </summary>
        public override string ToString()
        {
            return Text == null ? Kind.ToString() : Kind + ": " + Text;
        }
        /// <summary>
        /// Performs a logical comparison of the two tokens to determine 
        /// whether they are equal. 
        /// </summary>
        public static bool operator==(Token a, Token b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Performs a logical comparison of the two tokens to determine 
        /// whether they are inequal. 
        /// </summary>
        public static bool operator !=(Token a, Token b)
        {
            return !a.Equals(b);
        }

        private static void ValidateTextArgument(string text)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (text.Length == 0) throw new ArgumentException(null, "text");
        }
    }
}