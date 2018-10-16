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
    using System.Globalization;
    using System.Linq;

    using TokenSpec = Either<TokenKind, Token>;

    #endregion

    /// <summary>
    /// Semantic parser for CSS selector grammar.
    /// </summary>
    public sealed class Parser
    {
        private readonly Reader<Token> _reader;
        private readonly ISelectorGenerator _generator;

        private Parser(Reader<Token> reader, ISelectorGenerator generator)
        {
            Debug.Assert(reader != null);
            Debug.Assert(generator != null);
            _reader = reader;
            _generator = generator;
        }

        /// <summary>
        /// Parses a CSS selector group and generates its implementation.
        /// </summary>
        public static TGenerator Parse<TGenerator>(string selectors, TGenerator generator)
                where TGenerator : ISelectorGenerator
        {
            return Parse(selectors, generator, g => g);
        }

        /// <summary>
        /// Parses a CSS selector group and generates its implementation.
        /// </summary>
        public static T Parse<TGenerator, T>(string selectors, TGenerator generator, Func<TGenerator, T> resultor)
            where TGenerator : ISelectorGenerator
        {
            if (selectors == null) throw new ArgumentNullException("selectors");
            if (selectors.Length == 0) throw new ArgumentException(null, "selectors");

            return Parse(Tokener.Tokenize(selectors), generator, resultor);
        }

        /// <summary>
        /// Parses a tokenized stream representing a CSS selector group and 
        /// generates its implementation.
        /// </summary>
        public static TGenerator Parse<TGenerator>(IEnumerable<Token> tokens, TGenerator generator)
                where TGenerator : ISelectorGenerator
        {
            return Parse(tokens, generator, g => g);
        }

        /// <summary>
        /// Parses a tokenized stream representing a CSS selector group and 
        /// generates its implementation.
        /// </summary>
        public static T Parse<TGenerator, T>(IEnumerable<Token> tokens, TGenerator generator, Func<TGenerator, T> resultor)
            where TGenerator : ISelectorGenerator
        {
            if (tokens == null) throw new ArgumentNullException("tokens");
            if (resultor == null) throw new ArgumentNullException("resultor");

            new Parser(new Reader<Token>(tokens.GetEnumerator()), generator).Parse();
            return resultor(generator);
        }

        private void Parse()
        {
            _generator.OnInit();
            SelectorGroup();
            _generator.OnClose();
        }

        private void SelectorGroup()
        {
            //selectors_group
            //  : selector [ COMMA S* selector ]*
            //  ;

            Selector();
            while (TryRead(ToTokenSpec(Token.Comma())) != null)
            {
                TryRead(ToTokenSpec(TokenKind.WhiteSpace));
                Selector();
            }

            Read(ToTokenSpec(TokenKind.Eoi));
        }

        private void Selector()
        {
            _generator.OnSelector();

            //selector
            //  : simple_selector_sequence [ combinator simple_selector_sequence ]*
            //  ;

            SimpleSelectorSequence();
            while (TryCombinator())
                SimpleSelectorSequence();
        }

        private bool TryCombinator()
        {
            //combinator
            //  /* combinators can be surrounded by whitespace */
            //  : PLUS S* | GREATER S* | TILDE S* | S+
            //  ;

            var token = TryRead(ToTokenSpec(TokenKind.Plus), ToTokenSpec(TokenKind.Greater), ToTokenSpec(TokenKind.Tilde), ToTokenSpec(TokenKind.WhiteSpace));

            if (token == null)
                return false;

            if (token.Value.Kind == TokenKind.WhiteSpace)
            {
                _generator.Descendant();
            }
            else
            {
                switch (token.Value.Kind)
                {
                    case TokenKind.Tilde: _generator.GeneralSibling(); break;
                    case TokenKind.Greater: _generator.Child(); break;
                    case TokenKind.Plus: _generator.Adjacent(); break;
                }

                TryRead(ToTokenSpec(TokenKind.WhiteSpace));
            }

            return true;
        }

        private void SimpleSelectorSequence()
        {
            //simple_selector_sequence
            //  : [ type_selector | universal ]
            //    [ HASH | class | attrib | pseudo | negation ]*
            //  | [ HASH | class | attrib | pseudo | negation ]+
            //  ;

            var named = false;
            for (var modifiers = 0; ; modifiers++)
            {
                var token = TryRead(ToTokenSpec(TokenKind.Hash), ToTokenSpec(Token.Dot()), ToTokenSpec(Token.LeftBracket()), ToTokenSpec(Token.Colon()));

                if (token == null)
                {
                    if (named || modifiers > 0)
                        break;
                    TypeSelectorOrUniversal();
                    named = true;
                }
                else
                {
                    if (modifiers == 0 && !named)
                        _generator.Universal(NamespacePrefix.None); // implied

                    if (token.Value.Kind == TokenKind.Hash)
                    {
                        _generator.Id(token.Value.Text);
                    }
                    else
                    {
                        Unread(token.Value);
                        switch (token.Value.Text[0])
                        {
                            case '.': Class(); break;
                            case '[': Attrib(); break;
                            case ':': Pseudo(); break;
                            default: throw new Exception("Internal error.");
                        }
                    }
                }
            }
        }

        private void Pseudo()
        {
            //pseudo
            //  /* '::' starts a pseudo-element, ':' a pseudo-class */
            //  /* Exceptions: :first-line, :first-letter, :before and :after. */
            //  /* Note that pseudo-elements are restricted to one per selector and */
            //  /* occur only in the last simple_selector_sequence. */
            //  : ':' ':'? [ IDENT | functional_pseudo ]
            //  ;

            PseudoClass(); // We do pseudo-class only for now
        }

        private void PseudoClass()
        {
            //pseudo
            //  : ':' [ IDENT | functional_pseudo ]
            //  ;

            Read(ToTokenSpec(Token.Colon()));
            if (!TryFunctionalPseudo())
            {
                var clazz = Read(ToTokenSpec(TokenKind.Ident)).Text;
                switch (clazz)
                {
                    case "first-child": _generator.FirstChild(); break;
                    case "last-child": _generator.LastChild(); break;
                    case "only-child": _generator.OnlyChild(); break;
                    case "empty": _generator.Empty(); break;
                    default:
                        {
                            throw new FormatException(string.Format(
                                "Unknown pseudo-class '{0}'. Use either first-child, last-child, only-child or empty.", clazz));
                        }
                }
            }
        }

        private bool TryFunctionalPseudo()
        {
            //functional_pseudo
            //  : FUNCTION S* expression ')'
            //  ;

            var token = TryRead(ToTokenSpec(TokenKind.Function));
            if (token == null)
                return false;

            TryRead(ToTokenSpec(TokenKind.WhiteSpace));

            var func = token.Value.Text;
            switch (func)
            {
                case "nth-child": Nth(); break;
                case "nth-last-child": NthLast(); break;
                default:
                    {
                        throw new FormatException(string.Format(
                            "Unknown functional pseudo '{0}'. Only nth-child and nth-last-child are supported.", func));
                    }
            }

            Read(ToTokenSpec(Token.RightParenthesis()));
            return true;
        }

        private void Nth()
        {
            //nth
            //  : S* [ ['-'|'+']? INTEGER? {N} [ S* ['-'|'+'] S* INTEGER ]? |
            //         ['-'|'+']? INTEGER | {O}{D}{D} | {E}{V}{E}{N} ] S*
            //  ;

            // TODO Add support for the full syntax
            // At present, only INTEGER is allowed

            _generator.NthChild(1, NthB());
        }

        private void NthLast()
        {
            //nth
            //  : S* [ ['-'|'+']? INTEGER? {N} [ S* ['-'|'+'] S* INTEGER ]? |
            //         ['-'|'+']? INTEGER | {O}{D}{D} | {E}{V}{E}{N} ] S*
            //  ;

            // TODO Add support for the full syntax
            // At present, only INTEGER is allowed

            _generator.NthLastChild(1, NthB());
        }

        private int NthB()
        {
            return int.Parse(Read(ToTokenSpec(TokenKind.Integer)).Text, CultureInfo.InvariantCulture);
        }

        private void Attrib()
        {
            //attrib
            //  : '[' S* [ namespace_prefix ]? IDENT S*
            //        [ [ PREFIXMATCH |
            //            SUFFIXMATCH |
            //            SUBSTRINGMATCH |
            //            '=' |
            //            INCLUDES |
            //            DASHMATCH ] S* [ IDENT | STRING ] S*
            //        ]? ']'
            //  ;

            Read(ToTokenSpec(Token.LeftBracket()));
            var prefix = TryNamespacePrefix() ?? NamespacePrefix.None;
            var name = Read(ToTokenSpec(TokenKind.Ident)).Text;

            var hasValue = false;
            while (true)
            {
                var op = TryRead(
                    ToTokenSpec(Token.Equals()),
                    ToTokenSpec(TokenKind.Includes),
                    ToTokenSpec(TokenKind.DashMatch),
                    ToTokenSpec(TokenKind.PrefixMatch),
                    ToTokenSpec(TokenKind.SuffixMatch),
                    ToTokenSpec(TokenKind.SubstringMatch));

                if (op == null)
                    break;

                hasValue = true;
                var value = Read(ToTokenSpec(TokenKind.String), ToTokenSpec(TokenKind.Ident)).Text;

                if (op.Value == Token.Equals())
                {
                    _generator.AttributeExact(prefix, name, value);
                }
                else
                {
                    switch (op.Value.Kind)
                    {
                        case TokenKind.Includes: _generator.AttributeIncludes(prefix, name, value); break;
                        case TokenKind.DashMatch: _generator.AttributeDashMatch(prefix, name, value); break;
                        case TokenKind.PrefixMatch: _generator.AttributePrefixMatch(prefix, name, value); break;
                        case TokenKind.SuffixMatch: _generator.AttributeSuffixMatch(prefix, name, value); break;
                        case TokenKind.SubstringMatch: _generator.AttributeSubstring(prefix, name, value); break;
                    }
                }
            }

            if (!hasValue)
                _generator.AttributeExists(prefix, name);

            Read(ToTokenSpec(Token.RightBracket()));
        }

        private void Class()
        {
            //class
            //  : '.' IDENT
            //  ;

            Read(ToTokenSpec(Token.Dot()));
            _generator.Class(Read(ToTokenSpec(TokenKind.Ident)).Text);
        }

        private NamespacePrefix? TryNamespacePrefix()
        {
            //namespace_prefix
            //  : [ IDENT | '*' ]? '|'
            //  ;

            var pipe = Token.Pipe();
            var token = TryRead(ToTokenSpec(TokenKind.Ident), ToTokenSpec(Token.Star()), ToTokenSpec(pipe));

            if (token == null)
                return null;

            if (token.Value == pipe)
                return NamespacePrefix.Empty;

            var prefix = token.Value;
            if (TryRead(ToTokenSpec(pipe)) == null)
            {
                Unread(prefix);
                return null;
            }

            return prefix.Kind == TokenKind.Ident
                 ? new NamespacePrefix(prefix.Text)
                 : NamespacePrefix.Any;
        }

        private void TypeSelectorOrUniversal()
        {
            //type_selector
            //  : [ namespace_prefix ]? element_name
            //  ;
            //element_name
            //  : IDENT
            //  ;
            //universal
            //  : [ namespace_prefix ]? '*'
            //  ;

            var prefix = TryNamespacePrefix() ?? NamespacePrefix.None;
            var token = Read(ToTokenSpec(TokenKind.Ident), ToTokenSpec(Token.Star()));
            if (token.Kind == TokenKind.Ident)
                _generator.Type(prefix, token.Text);
            else
                _generator.Universal(prefix);
        }

        private Token Peek()
        {
            return _reader.Peek();
        }

        private Token Read(TokenSpec spec)
        {
            var token = TryRead(spec);
            if (token == null)
            {
                throw new FormatException(
                    string.Format(@"Unexpected token {{{0}}} where {{{1}}} was expected.",
                    Peek().Kind, spec));
            }
            return token.Value;
        }

        private Token Read(params TokenSpec[] specs)
        {
            var token = TryRead(specs);
            if (token == null)
            {
                throw new FormatException(string.Format(
                    @"Unexpected token {{{0}}} where one of [{1}] was expected.",
                    Peek().Kind, string.Join(", ", specs.Select(k => k.ToString()).ToArray())));
            }
            return token.Value;
        }

        private Token? TryRead(params TokenSpec[] specs)
        {
            foreach (var kind in specs)
            {
                var token = TryRead(kind);
                if (token != null)
                    return token;
            }
            return null;
        }

        private Token? TryRead(TokenSpec spec)
        {
            var token = Peek();
            if (!spec.Fold(a => a == token.Kind, b => b == token))
                return null;
            _reader.Read();
            return token;
        }

        private void Unread(Token token)
        {
            _reader.Unread(token);
        }

        private static TokenSpec ToTokenSpec(TokenKind kind)
        {
            return TokenSpec.A(kind);
        }

        private static TokenSpec ToTokenSpec(Token token)
        {
            return TokenSpec.B(token);
        }
    }
}
