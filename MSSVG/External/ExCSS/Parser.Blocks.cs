using System;
using System.Text;
using System.Collections.Generic;
using ExCSS.Model;
using ExCSS.Model.TextBlocks;

namespace ExCSS
{
    public partial class Parser
    {
        private bool ParseTokenBlock(Block token)
        {
            switch (_parsingContext)
            {
                case ParsingContext.DataBlock:
                    return ParseSymbol(token);

                case ParsingContext.InSelector:
                    return ParseSelector(token);

                case ParsingContext.InDeclaration:
                    return ParseDeclaration(token);

                case ParsingContext.AfterProperty:
                    return ParsePostProperty(token);

                case ParsingContext.BeforeValue:
                    return ParseValue(token);

                case ParsingContext.InValuePool:
                    return ParseValuePool(token);

                case ParsingContext.InValueList:
                    return ParseValueList(token);

                case ParsingContext.InSingleValue:
                    return ParseSingleValue(token);

                case ParsingContext.ValueImportant:
                    return ParseImportant(token);

                case ParsingContext.AfterValue:
                    return ParsePostValue(token);

                case ParsingContext.InMediaList:
                    return ParseMediaList(token);

                case ParsingContext.InMediaValue:
                    return ParseMediaValue(token);

                case ParsingContext.BeforeImport:
                    return ParseImport(token);

                case ParsingContext.AfterInstruction:
                    return ParsePostInstruction(token);

                case ParsingContext.BeforeCharset:
                    return ParseCharacterSet(token);

                case ParsingContext.BeforeNamespacePrefix:
                    return ParseLeadingPrefix(token);

                case ParsingContext.AfterNamespacePrefix:
                    return ParseNamespace(token);

                case ParsingContext.InCondition:
                    return ParseCondition(token);

                case ParsingContext.InUnknown:
                    return ParseUnknown(token);

                case ParsingContext.InKeyframeText:
                    return ParseKeyframeText(token);

                case ParsingContext.BeforePageSelector:
                    return ParsePageSelector(token);

                case ParsingContext.BeforeDocumentFunction:
                    return ParsePreDocumentFunction(token);

                case ParsingContext.InDocumentFunction:
                    return ParseDocumentFunction(token);

                case ParsingContext.AfterDocumentFunction:
                    return ParsePostDocumentFunction(token);

                case ParsingContext.BetweenDocumentFunctions:
                    return ParseDocumentFunctions(token);

                case ParsingContext.BeforeKeyframesName:
                    return ParseKeyframesName(token);

                case ParsingContext.BeforeKeyframesData:
                    return ParsePreKeyframesData(token);

                case ParsingContext.KeyframesData:
                    return ParseKeyframesData(token);

                case ParsingContext.BeforeFontFace:
                    return ParseFontface(token);

                case ParsingContext.InHexValue:
                    return ParseHexValue(token);

                case ParsingContext.InFunction:

                    return ParseValueFunction(token);
                default:
                    return false;
            }
        }

        private bool ParseSymbol(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.AtRule)
            {
                switch (((SymbolBlock)token).Value)
                {
                    case RuleTypes.Media:
                        {
                            AddRuleSet(new MediaRule());
                            SetParsingContext(ParsingContext.InMediaList);
                            break;
                        }
                    case RuleTypes.Page:
                        {
                            AddRuleSet(new PageRule());
                            //SetParsingContext(ParsingContext.InSelector);
                            SetParsingContext(ParsingContext.BeforePageSelector);
                            break;
                        }
                    case RuleTypes.Import:
                        {
                            AddRuleSet(new ImportRule());
                            SetParsingContext(ParsingContext.BeforeImport);
                            break;
                        }
                    case RuleTypes.FontFace:
                        {
                            AddRuleSet(new FontFaceRule());
                            //SetParsingContext(ParsingContext.InDeclaration);
                            SetParsingContext(ParsingContext.BeforeFontFace);
                            break;
                        }
                    case RuleTypes.CharacterSet:
                        {
                            AddRuleSet(new CharacterSetRule());
                            SetParsingContext(ParsingContext.BeforeCharset);
                            break;
                        }
                    case RuleTypes.Namespace:
                        {
                            AddRuleSet(new NamespaceRule());
                            SetParsingContext(ParsingContext.BeforeNamespacePrefix);
                            break;
                        }
                    case RuleTypes.Supports:
                        {
                            _buffer = new StringBuilder();
                            AddRuleSet(new SupportsRule());
                            SetParsingContext(ParsingContext.InCondition);
                            break;
                        }
                    case RuleTypes.Keyframes:
                        {
                            AddRuleSet(new KeyframesRule());
                            SetParsingContext(ParsingContext.BeforeKeyframesName);
                            break;
                        }
                    case RuleTypes.Document:
                        {
                            AddRuleSet(new DocumentRule());
                            SetParsingContext(ParsingContext.BeforeDocumentFunction);
                            break;
                        }
                    default:
                        {
                            _buffer = new StringBuilder();
                            AddRuleSet(new GenericRule());
                            SetParsingContext(ParsingContext.InUnknown);
                            ParseUnknown(token);
                            break;
                        }
                }

                return true;
            }

            if (token.GrammarSegment == GrammarSegment.CurlyBracketClose)
            {
                return FinalizeRule();
            }

            AddRuleSet(new StyleRule());
            SetParsingContext(ParsingContext.InSelector);
            ParseSelector(token);
            return true;

        }

        private bool ParseUnknown(Block token)
        {
            switch (token.GrammarSegment)
            {
                case GrammarSegment.Semicolon:
                    CastRuleSet<GenericRule>().SetInstruction(_buffer.ToString());
                    SetParsingContext(ParsingContext.DataBlock);

                    return FinalizeRule();

                case GrammarSegment.CurlyBraceOpen:
                    CastRuleSet<GenericRule>().SetCondition(_buffer.ToString());
                    SetParsingContext(ParsingContext.DataBlock);
                    break;

                default:
                    _buffer.Append(token);
                    break;
            }

            return true;
        }

        private bool ParseSelector(Block token)
        {
            switch (token.GrammarSegment)
            {
                case GrammarSegment.CurlyBraceOpen:
                    {
                        var rule = CurrentRule as ISupportsSelector;

                        if (rule != null)
                        {
                            rule.Selector = _selectorFactory.GetSelector();
                        }

                        SetParsingContext(CurrentRule is StyleRule
                            ? ParsingContext.InDeclaration
                            : ParsingContext.DataBlock);
                    }
                    break;

                case GrammarSegment.CurlyBracketClose:
                    return false;

                default:
                    _selectorFactory.Apply(token);
                    break;
            }

            return true;
        }

        private bool ParseDeclaration(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.CurlyBracketClose)
            {
                FinalizeProperty();
                SetParsingContext(CurrentRule is KeyframeRule ? ParsingContext.KeyframesData : ParsingContext.DataBlock);
                return FinalizeRule();
            }

            if (token.GrammarSegment != GrammarSegment.Ident)
            {
                return false;
            }

            AddProperty(new Property(((SymbolBlock)token).Value));
            SetParsingContext(ParsingContext.AfterProperty);
            return true;
        }

        private bool ParsePostInstruction(Block token)
        {
            if (token.GrammarSegment != GrammarSegment.Semicolon)
            {
                return false;
            }

            SetParsingContext(ParsingContext.DataBlock);

            return FinalizeRule();
        }

        private bool ParseCondition(Block token)
        {
            switch (token.GrammarSegment)
            {
                case GrammarSegment.CurlyBraceOpen:
                    CastRuleSet<SupportsRule>().Condition = _buffer.ToString();
                    SetParsingContext(ParsingContext.DataBlock);
                    break;

                default:
                    _buffer.Append(token);
                    break;
            }

            return true;
        }

        private bool ParseLeadingPrefix(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.Ident)
            {
                CastRuleSet<NamespaceRule>().Prefix = ((SymbolBlock)token).Value;
                SetParsingContext(ParsingContext.AfterNamespacePrefix);

                return true;
            }

            if (token.GrammarSegment == GrammarSegment.String || token.GrammarSegment == GrammarSegment.Url)
            {
                CastRuleSet<NamespaceRule>().Uri = ((StringBlock)token).Value;
                return true;
            }

            SetParsingContext(ParsingContext.AfterInstruction);

            return ParsePostInstruction(token);
        }

        private bool ParsePostProperty(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.Colon)
            {
                _isFraction = false;
                SetParsingContext(ParsingContext.BeforeValue);
                return true;
            }

            if (token.GrammarSegment == GrammarSegment.Semicolon || token.GrammarSegment == GrammarSegment.CurlyBracketClose)
            {
                ParsePostValue(token);
            }

            return false;
        }

        private bool ParseValue(Block token)
        {
            switch (token.GrammarSegment)
            {
                case GrammarSegment.Semicolon:
                    SetParsingContext(ParsingContext.InDeclaration);
                    break;

                case GrammarSegment.CurlyBracketClose:
                    ParseDeclaration(token);
                    break;

                default:
                    SetParsingContext(ParsingContext.InSingleValue);
                    return ParseSingleValue(token);
            }

            return false;
        }

        private bool ParseSingleValue(Block token)
        {
            switch (token.GrammarSegment)
            {
                case GrammarSegment.Dimension: // "3px"
                    return AddTerm(new PrimitiveTerm(((UnitBlock)token).Unit, ((UnitBlock)token).Value));

                case GrammarSegment.Hash:// "#ffffff"
                    return ParseSingleValueHexColor(((SymbolBlock)token).Value);

                case GrammarSegment.Delimiter: // "#"
                    return ParseValueDelimiter((DelimiterBlock)token);

                case GrammarSegment.Ident: // "auto"
                    return ParseSingleValueIdent((SymbolBlock)token);

                case GrammarSegment.String:// "'some value'"
                    return AddTerm(new PrimitiveTerm(UnitType.String, ((StringBlock)token).Value));

                case GrammarSegment.Url:// "url('http://....')"
                    return AddTerm(new PrimitiveTerm(UnitType.Uri, ((StringBlock)token).Value));

                case GrammarSegment.Percentage: // "10%"
                    return AddTerm(new PrimitiveTerm(UnitType.Percentage, ((UnitBlock)token).Value));

                case GrammarSegment.Number: // "123"
                    return AddTerm(new PrimitiveTerm(UnitType.Number, ((NumericBlock)token).Value));

                case GrammarSegment.Whitespace: // " "
                    _terms.AddSeparator(GrammarSegment.Whitespace);
                    SetParsingContext(ParsingContext.InValueList);
                    return true;

                case GrammarSegment.Function: // rgba(...)
                    _functionBuffers.Push(new FunctionBuffer(((SymbolBlock)token).Value));
                    SetParsingContext(ParsingContext.InFunction);
                    return true;

                case GrammarSegment.Comma: // ","
                    _terms.AddSeparator(GrammarSegment.Comma);
                    SetParsingContext(ParsingContext.InValuePool);
                    return true;

                case GrammarSegment.Semicolon: // ";"
                case GrammarSegment.CurlyBracketClose: // "}"
                    return ParsePostValue(token);

                default:
                    return false;
            }
        }

        private bool ParseValueFunction(Block token)
        {
            switch (token.GrammarSegment)
            {
                case GrammarSegment.ParenClose:
                    SetParsingContext(ParsingContext.InSingleValue);
                    return AddTerm(_functionBuffers.Pop().Done());

                case GrammarSegment.Comma:
                    _functionBuffers.Peek().Include();
                    return true;

                default:
                    return ParseSingleValue(token);
            }
        }

        private bool ParseValueList(Block token)
        {
            switch (token.GrammarSegment)
            {
                case GrammarSegment.CurlyBracketClose:
                case GrammarSegment.Semicolon:
                    ParsePostValue(token);
                    break;

                case GrammarSegment.Comma:
                    SetParsingContext(ParsingContext.InValuePool);
                    break;

                default:
                    SetParsingContext(ParsingContext.InSingleValue);
                    return ParseSingleValue(token);
            }

            return true;
        }

        private bool ParseValuePool(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.Semicolon || token.GrammarSegment == GrammarSegment.CurlyBracketClose)
            {
                ParsePostValue(token);
            }
            else
            {
                SetParsingContext(ParsingContext.InSingleValue);
                return ParseSingleValue(token);
            }

            return false;
        }

        private bool ParseHexValue(Block token)
        {
            switch (token.GrammarSegment)
            {
                case GrammarSegment.Number:
                case GrammarSegment.Dimension:
                case GrammarSegment.Ident:
                    var rest = token.ToString();

                    if (_buffer.Length + rest.Length <= 6)
                    {
                        _buffer.Append(rest);
                        return true;
                    }

                    break;
            }

            ParseSingleValueHexColor(_buffer.ToString());
            SetParsingContext(ParsingContext.InSingleValue);
            return ParseSingleValue(token);
        }

        private bool ParsePostValue(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.Semicolon)
            {
                FinalizeProperty();
                SetParsingContext(ParsingContext.InDeclaration);
                return true;
            }

            if (token.GrammarSegment == GrammarSegment.CurlyBracketClose)
            {
                return ParseDeclaration(token);
            }

            return false;
        }

        private bool ParseImportant(Block token)
        {
            if (token.GrammarSegment != GrammarSegment.Ident || ((SymbolBlock)token).Value != "important")
            {
                return ParsePostValue(token);
            }

            SetParsingContext(ParsingContext.AfterValue);
            _property.Important = true;

            return true;
        }

        private bool ParseValueDelimiter(DelimiterBlock token)
        {
            switch (token.Value)
            {
                case Specification.Em:
                    SetParsingContext(ParsingContext.ValueImportant);
                    return true;

                case Specification.Hash:
                    _buffer = new StringBuilder();
                    SetParsingContext(ParsingContext.InHexValue);
                    return true;

                case Specification.Solidus:
                    _isFraction = true;
                    return true;

                default:
                    return false;
            }
        }

        private bool ParseSingleValueIdent(SymbolBlock token)
        {
            if (token.Value != "inherit")
            {
                return AddTerm(new PrimitiveTerm(UnitType.Ident, token.Value));
            }
            _terms.AddTerm(Term.Inherit);
            SetParsingContext(ParsingContext.AfterValue);
            return true;
        }

        private bool ParseSingleValueHexColor(string color)
        {
            HtmlColor htmlColor;

            if(HtmlColor.TryFromHex(color, out htmlColor))
                return AddTerm(htmlColor);
            return false;
        }

        #region Namespace
        private bool ParseNamespace(Block token)
        {
            SetParsingContext(ParsingContext.AfterInstruction);

            if (token.GrammarSegment != GrammarSegment.String)
            {
                return ParsePostInstruction(token);
            }

            CastRuleSet<NamespaceRule>().Uri = ((StringBlock)token).Value;

            return true;
        }
        #endregion

        #region Charset
        private bool ParseCharacterSet(Block token)
        {
            SetParsingContext(ParsingContext.AfterInstruction);

            if (token.GrammarSegment != GrammarSegment.String)
            {
                return ParsePostInstruction(token);
            }

            CastRuleSet<CharacterSetRule>().Encoding = ((StringBlock)token).Value;

            return true;
        }
        #endregion

        #region Import
        private bool ParseImport(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.String || token.GrammarSegment == GrammarSegment.Url)
            {
                CastRuleSet<ImportRule>().Href = ((StringBlock)token).Value;
                SetParsingContext(ParsingContext.InMediaList);
                return true;
            }

            SetParsingContext(ParsingContext.AfterInstruction);

            return false;
        }
        #endregion

        #region Font Face

        private bool ParseFontface(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.CurlyBraceOpen)
            {
                SetParsingContext(ParsingContext.InDeclaration);
                return true;
            }

            return false;
        }
        #endregion

        #region Keyframes
        private bool ParseKeyframesName(Block token)
        {
            //SetParsingContext(ParsingContext.BeforeKeyframesData);

            if (token.GrammarSegment == GrammarSegment.Ident)
            {
                CastRuleSet<KeyframesRule>().Identifier = ((SymbolBlock)token).Value;
                return true;
            }

            if (token.GrammarSegment == GrammarSegment.CurlyBraceOpen)
            {
                SetParsingContext(ParsingContext.KeyframesData);
                return true;
            }

            return false;
        }

        private bool ParsePreKeyframesData(Block token)
        {
            if (token.GrammarSegment != GrammarSegment.CurlyBraceOpen)
            {
                return false;
            }

            SetParsingContext(ParsingContext.BeforeKeyframesData);
            return true;
        }

        private bool ParseKeyframesData(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.CurlyBracketClose)
            {
                SetParsingContext(ParsingContext.DataBlock);
                return FinalizeRule();
            }

            _buffer = new StringBuilder();
         
            return ParseKeyframeText(token);
        }

        private bool ParseKeyframeText(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.CurlyBraceOpen)
            {
                SetParsingContext(ParsingContext.InDeclaration);
                return true;
            }

            if (token.GrammarSegment == GrammarSegment.CurlyBracketClose)
            {
                ParseKeyframesData(token);

                return false;
            }

            var frame = new KeyframeRule
            {
                Value = token.ToString()
            };


            CastRuleSet<KeyframesRule>().Declarations.Add(frame);
            _activeRuleSets.Push(frame);

            return true;
        }
        #endregion

        #region Page

        private bool ParsePageSelector(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.Colon || token.GrammarSegment == GrammarSegment.Whitespace)
            {
                return true;
            }

            if (token.GrammarSegment == GrammarSegment.Ident)
            {
                CastRuleSet<PageRule>().Selector = new SimpleSelector(token.ToString());
                return true;
            }

            if (token.GrammarSegment == GrammarSegment.CurlyBraceOpen)
            {
                SetParsingContext(ParsingContext.InDeclaration);
                return true;
            }

            return false;
        }
        
        #endregion

        #region Document
        private bool ParsePreDocumentFunction(Block token)
        {
            switch (token.GrammarSegment)
            {
                case GrammarSegment.Url:
                    CastRuleSet<DocumentRule>().Conditions.Add(new KeyValuePair<DocumentFunction, string>(DocumentFunction.Url, ((StringBlock)token).Value));
                    break;

                case GrammarSegment.UrlPrefix:
                    CastRuleSet<DocumentRule>().Conditions.Add(new KeyValuePair<DocumentFunction, string>(DocumentFunction.UrlPrefix, ((StringBlock)token).Value));
                    break;

                case GrammarSegment.Domain:
                    CastRuleSet<DocumentRule>().Conditions.Add(new KeyValuePair<DocumentFunction, string>(DocumentFunction.Domain, ((StringBlock)token).Value));
                    break;

                case GrammarSegment.Function:
                    if (string.Compare(((SymbolBlock)token).Value, "regexp", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        SetParsingContext(ParsingContext.InDocumentFunction);
                        return true;
                    }
                    SetParsingContext(ParsingContext.AfterDocumentFunction);
                    return false;

                default:
                    SetParsingContext(ParsingContext.DataBlock);
                    return false;
            }

            SetParsingContext(ParsingContext.BetweenDocumentFunctions);
            return true;
        }

        private bool ParseDocumentFunction(Block token)
        {
            SetParsingContext(ParsingContext.AfterDocumentFunction);

            if (token.GrammarSegment != GrammarSegment.String) return false;
            CastRuleSet<DocumentRule>().Conditions.Add(new KeyValuePair<DocumentFunction, string>(DocumentFunction.RegExp, ((StringBlock)token).Value));
            return true;
        }

        private bool ParsePostDocumentFunction(Block token)
        {
            SetParsingContext(ParsingContext.BetweenDocumentFunctions);
            return token.GrammarSegment == GrammarSegment.ParenClose;
        }

        private bool ParseDocumentFunctions(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.Comma)
            {
                SetParsingContext(ParsingContext.BeforeDocumentFunction);
                return true;
            }

            if (token.GrammarSegment == GrammarSegment.CurlyBraceOpen)
            {
                SetParsingContext(ParsingContext.DataBlock);
                return true;
            }

            SetParsingContext(ParsingContext.DataBlock);
            return false;
        }
        #endregion

        #region Media
        private bool ParseMediaList(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.Semicolon)
            {
                FinalizeRule();
                SetParsingContext(ParsingContext.DataBlock);
                return true;
            }

            _buffer = new StringBuilder();
            SetParsingContext(ParsingContext.InMediaValue);
            return ParseMediaValue(token);
        }

        private bool ParseMediaValue(Block token)
        {
            switch (token.GrammarSegment)
            {
                case GrammarSegment.CurlyBraceOpen:
                case GrammarSegment.Semicolon:
                    {
                        var container = CurrentRule as ISupportsMedia;
                        var medium = _buffer.ToString();

                        if (container != null)
                        {
                            container.Media.AppendMedium(medium);
                        }

                        if (CurrentRule is ImportRule)
                        {
                            return ParsePostInstruction(token);
                        }

                        SetParsingContext(ParsingContext.DataBlock);
                        return token.GrammarSegment == GrammarSegment.CurlyBraceOpen;
                    }
                case GrammarSegment.Comma:
                    {
                        var container = CurrentRule as ISupportsMedia;

                        if (container != null)
                        {
                            container.Media.AppendMedium(_buffer.ToString());
                        }

                        _buffer.Length = 0;
                        return true;
                    }
                case GrammarSegment.Whitespace:
                    {
                        _buffer.Append(' ');
                        return true;
                    }
                default:
                    {
                        _buffer.Append(token);
                        return true;
                    }
            }
        }
        #endregion
    }
}
