using System;
using System.Globalization;
using ExCSS.Model;
using ExCSS.Model.TextBlocks;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    internal sealed class SelectorFactory
    {
        private SelectorOperation _selectorOperation;
        private BaseSelector _currentSelector;
        private AggregateSelectorList _aggregateSelectorList;
        private ComplexSelector _complexSelector;
        private bool _hasCombinator;
        private Combinator _combinator;
        private SelectorFactory _nestedSelectorFactory;
        private string _attributeName;
        private string _attributeValue;
        private string _attributeOperator;

        internal SelectorFactory()
        {
            ResetFactory();
        }

        internal BaseSelector GetSelector()
        {
            if (_complexSelector != null)
            {
                _complexSelector.ConcludeSelector(_currentSelector);
                _currentSelector = _complexSelector;
            }

            if (_aggregateSelectorList == null || _aggregateSelectorList.Length == 0)
            {
                return _currentSelector ?? SimpleSelector.All;
            }

            if (_currentSelector == null && _aggregateSelectorList.Length == 1)
            {
                return _aggregateSelectorList[0];
            }

            if (_currentSelector == null)
            {
                return _aggregateSelectorList;
            }

            _aggregateSelectorList.AppendSelector(_currentSelector);
            _currentSelector = null;

            return _aggregateSelectorList;
        }

        internal void Apply(Block token)
        {
            switch (_selectorOperation)
            {
                case SelectorOperation.Data:
                    ParseSymbol(token);
                    break;

                case SelectorOperation.Class:
                    PraseClass(token);
                    break;

                case SelectorOperation.Attribute:
                    ParseAttribute(token);
                    break;

                case SelectorOperation.AttributeOperator:
                    ParseAttributeOperator(token);
                    break;

                case SelectorOperation.AttributeValue:
                    ParseAttributeValue(token);
                    break;

                case SelectorOperation.AttributeEnd:
                    ParseAttributeEnd(token);
                    break;

                case SelectorOperation.PseudoClass:
                    ParsePseudoClass(token);
                    break;

                case SelectorOperation.PseudoClassFunction:
                    ParsePseudoClassFunction(token);
                    break;

                case SelectorOperation.PseudoClassFunctionEnd:
                    PrasePseudoClassFunctionEnd(token);
                    break;

                case SelectorOperation.PseudoElement:
                    ParsePseudoElement(token);
                    break;
            }
        }

        internal SelectorFactory ResetFactory()
        {
            _attributeName = null;
            _attributeValue = null;
            _attributeOperator = string.Empty;
            _selectorOperation = SelectorOperation.Data;
            _combinator = Combinator.Descendent;
            _hasCombinator = false;
            _currentSelector = null;
            _aggregateSelectorList = null;
            _complexSelector = null;

            return this;
        }

        private void ParseSymbol(Block token)
        {
            switch (token.GrammarSegment)
            {
                // Attribute [A]
                case GrammarSegment.SquareBraceOpen:
                    _attributeName = null;
                    _attributeValue = null;
                    _attributeOperator = string.Empty;
                    _selectorOperation = SelectorOperation.Attribute;
                    return;

                // Pseudo :P
                case GrammarSegment.Colon:
                    _selectorOperation = SelectorOperation.PseudoClass;
                    return;

                // ID #I
                case GrammarSegment.Hash:
                    Insert(SimpleSelector.Id(((SymbolBlock)token).Value));
                    return;

                // Type E
                case GrammarSegment.Ident:
                    Insert(SimpleSelector.Type(((SymbolBlock)token).Value));
                    return;

                // Whitespace
                case GrammarSegment.Whitespace:
                    Insert(Combinator.Descendent);
                    return;

                case GrammarSegment.Delimiter:
                    ParseDelimiter(token);
                    return;

                case GrammarSegment.Comma:
                    InsertCommaDelimited();
                    return;
            }
        }

        private void ParseAttribute(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.Whitespace)
            {
                return;
            }

            _selectorOperation = SelectorOperation.AttributeOperator;

            switch (token.GrammarSegment)
            {
                case GrammarSegment.Ident:
                    _attributeName = ((SymbolBlock)token).Value;
                    break;

                case GrammarSegment.String:
                    _attributeName = ((StringBlock)token).Value;
                    break;

                default:
                    _selectorOperation = SelectorOperation.Data;
                    break;
            }
        }

        private void ParseAttributeOperator(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.Whitespace)
            {
                return;
            }

            _selectorOperation = SelectorOperation.AttributeValue;

            if (token.GrammarSegment == GrammarSegment.SquareBracketClose)
            {
                ParseAttributeEnd(token);
            }
            else if (token is MatchBlock || token.GrammarSegment == GrammarSegment.Delimiter)
            {
                _attributeOperator = token.ToString();
            }
            else
            {
                _selectorOperation = SelectorOperation.AttributeEnd;
            }
        }

        private void ParseAttributeValue(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.Whitespace)
            {
                return;
            }

            _selectorOperation = SelectorOperation.AttributeEnd;

            switch (token.GrammarSegment)
            {
                case GrammarSegment.Ident:
                    _attributeValue = ((SymbolBlock)token).Value;
                    break;

                case GrammarSegment.String:
                    _attributeValue = ((StringBlock)token).Value;
                    break;

                case GrammarSegment.Number:
                    _attributeValue = ((NumericBlock)token).Value.ToString(CultureInfo.InvariantCulture);
                    break;

                default:
                    _selectorOperation = SelectorOperation.Data;
                    break;
            }
        }

        private void ParseAttributeEnd(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.Whitespace)
            {
                return;
            }

            _selectorOperation = SelectorOperation.Data;

            if (token.GrammarSegment != GrammarSegment.SquareBracketClose)
            {
                return;
            }

            switch (_attributeOperator)
            {
                case "=":
                    Insert(SimpleSelector.AttributeMatch(_attributeName, _attributeValue));
                    break;

                case "~=":
                    Insert(SimpleSelector.AttributeSpaceSeparated(_attributeName, _attributeValue));
                    break;

                case "|=":
                    Insert(SimpleSelector.AttributeDashSeparated(_attributeName, _attributeValue));
                    break;

                case "^=":
                    Insert(SimpleSelector.AttributeStartsWith(_attributeName, _attributeValue));
                    break;

                case "$=":
                    Insert(SimpleSelector.AttributeEndsWith(_attributeName, _attributeValue));
                    break;

                case "*=":
                    Insert(SimpleSelector.AttributeContains(_attributeName, _attributeValue));
                    break;

                case "!=":
                    Insert(SimpleSelector.AttributeNegatedMatch(_attributeName, _attributeValue));
                    break;

                default:
                    Insert(SimpleSelector.AttributeUnmatched(_attributeName));
                    break;
            }
        }

        private void ParsePseudoClass(Block token)
        {
            _selectorOperation = SelectorOperation.Data;

            switch (token.GrammarSegment)
            {
                case GrammarSegment.Colon:
                    _selectorOperation = SelectorOperation.PseudoElement;
                    break;

                case GrammarSegment.Function:
                    _attributeName = ((SymbolBlock)token).Value;
                    _attributeValue = string.Empty;
                    _selectorOperation = SelectorOperation.PseudoClassFunction;

                    if (_nestedSelectorFactory != null)
                    {
                        _nestedSelectorFactory.ResetFactory();
                    }

                    break;

                case GrammarSegment.Ident:
                    var pseudoSelector = GetPseudoSelector(token);

                    if (pseudoSelector != null)
                    {
                        Insert(pseudoSelector);
                    }
                    break;
            }
        }

        private void ParsePseudoElement(Block token)
        {
            if (token.GrammarSegment != GrammarSegment.Ident)
            {
                return;
            }
            var data = ((SymbolBlock)token).Value;

            switch (data)
            {
                case PseudoSelectorPrefix.PseudoElementBefore:
                    Insert(SimpleSelector.PseudoElement(PseudoSelectorPrefix.PseudoElementBefore));
                    break;

                case PseudoSelectorPrefix.PseudoElementAfter:
                    Insert(SimpleSelector.PseudoElement(PseudoSelectorPrefix.PseudoElementAfter));
                    break;

                case PseudoSelectorPrefix.PseudoElementSelection:
                    Insert(SimpleSelector.PseudoElement(PseudoSelectorPrefix.PseudoElementSelection));
                    break;

                case PseudoSelectorPrefix.PseudoElementFirstline:
                    Insert(SimpleSelector.PseudoElement(PseudoSelectorPrefix.PseudoElementFirstline));
                    break;

                case PseudoSelectorPrefix.PseudoElementFirstletter:
                    Insert(SimpleSelector.PseudoElement(PseudoSelectorPrefix.PseudoElementFirstletter));
                    break;

                default:
                    Insert(SimpleSelector.PseudoElement(data));
                    break;
            }
        }

        private void PraseClass(Block token)
        {
            _selectorOperation = SelectorOperation.Data;

            if (token.GrammarSegment == GrammarSegment.Ident)
            {
                Insert(SimpleSelector.Class(((SymbolBlock)token).Value));
            }
        }

        private void ParsePseudoClassFunction(Block token)
        {
            if (token.GrammarSegment == GrammarSegment.Whitespace)
            {
                return;
            }

            switch (_attributeName)
            {
                case PseudoSelectorPrefix.PseudoFunctionNthchild:
                case PseudoSelectorPrefix.PseudoFunctionNthlastchild:
                case PseudoSelectorPrefix.PseudoFunctionNthOfType:
                case PseudoSelectorPrefix.PseudoFunctionNthLastOfType:
                    {
                        switch (token.GrammarSegment)
                        {
                            case GrammarSegment.Ident:
                            case GrammarSegment.Number:
                            case GrammarSegment.Dimension:
                                _attributeValue += token.ToString();
                                return;

                            case GrammarSegment.Delimiter:
                                var chr = ((DelimiterBlock)token).Value;

                                if (chr == Specification.PlusSign || chr == Specification.MinusSign)
                                {
                                    _attributeValue += chr;
                                    return;
                                }

                                break;
                        }

                        break;
                    }
                case PseudoSelectorPrefix.PseudoFunctionNot:
                    {
                        if (_nestedSelectorFactory == null)
                        {
                            _nestedSelectorFactory = new SelectorFactory();
                        }

                        if (token.GrammarSegment != GrammarSegment.ParenClose || _nestedSelectorFactory._selectorOperation != SelectorOperation.Data)
                        {
                            _nestedSelectorFactory.Apply(token);
                            return;
                        }

                        break;
                    }
                case PseudoSelectorPrefix.PseudoFunctionDir:
                    {
                        if (token.GrammarSegment == GrammarSegment.Ident)
                        {
                            _attributeValue = ((SymbolBlock)token).Value;
                        }

                        _selectorOperation = SelectorOperation.PseudoClassFunctionEnd;
                        return;
                    }
                case PseudoSelectorPrefix.PseudoFunctionLang:
                    {
                        if (token.GrammarSegment == GrammarSegment.Ident)
                        {
                            _attributeValue = ((SymbolBlock)token).Value;
                        }

                        _selectorOperation = SelectorOperation.PseudoClassFunctionEnd;
                        return;
                    }
                case PseudoSelectorPrefix.PseudoFunctionContains:
                    {
                        switch (token.GrammarSegment)
                        {
                            case GrammarSegment.String:
                                _attributeValue = ((StringBlock)token).Value;
                                break;

                            case GrammarSegment.Ident:
                                _attributeValue = ((SymbolBlock)token).Value;
                                break;
                        }

                        _selectorOperation = SelectorOperation.PseudoClassFunctionEnd;
                        return;
                    }
            }

            PrasePseudoClassFunctionEnd(token);
        }

        private void PrasePseudoClassFunctionEnd(Block token)
        {
            _selectorOperation = SelectorOperation.Data;

            if (token.GrammarSegment != GrammarSegment.ParenClose)
            {
                return;
            }

            switch (_attributeName)
            {
                case PseudoSelectorPrefix.PseudoFunctionNthchild:
                    Insert(GetChildSelector<NthFirstChildSelector>());
                    break;

                case PseudoSelectorPrefix.PseudoFunctionNthlastchild:
                    Insert(GetChildSelector<NthLastChildSelector>());
                    break;

                case PseudoSelectorPrefix.PseudoFunctionNthOfType:
                    Insert(GetChildSelector<NthOfTypeSelector>());
                    break;

                case PseudoSelectorPrefix.PseudoFunctionNthLastOfType:
                    Insert(GetChildSelector<NthLastOfTypeSelector>());
                    break;

                case PseudoSelectorPrefix.PseudoFunctionNot:
                    {
                        var selector = _nestedSelectorFactory.GetSelector();
                        var code = string.Format("{0}({1})", PseudoSelectorPrefix.PseudoFunctionNot, selector);
                        Insert(SimpleSelector.PseudoClass(code));
                        break;
                    }
                case PseudoSelectorPrefix.PseudoFunctionDir:
                    {
                        var code = string.Format("{0}({1})", PseudoSelectorPrefix.PseudoFunctionDir, _attributeValue);
                        Insert(SimpleSelector.PseudoClass(code));
                        break;
                    }
                case PseudoSelectorPrefix.PseudoFunctionLang:
                    {
                        var code = string.Format("{0}({1})", PseudoSelectorPrefix.PseudoFunctionLang, _attributeValue);
                        Insert(SimpleSelector.PseudoClass(code));
                        break;
                    }
                case PseudoSelectorPrefix.PseudoFunctionContains:
                    {
                        var code = string.Format("{0}({1})", PseudoSelectorPrefix.PseudoFunctionContains, _attributeValue);
                        Insert(SimpleSelector.PseudoClass(code));
                        break;
                    }
            }
        }

        private void InsertCommaDelimited()
        {
            if (_currentSelector == null)
            {
                return;
            }

            if (_aggregateSelectorList == null)
            {
                _aggregateSelectorList = new AggregateSelectorList(",");
            }

            if (_complexSelector != null)
            {
                _complexSelector.ConcludeSelector(_currentSelector);
                _aggregateSelectorList.AppendSelector(_complexSelector);
                _complexSelector = null;
            }
            else
            {
                _aggregateSelectorList.AppendSelector(_currentSelector);
            }

            _currentSelector = null;
        }

        private void Insert(BaseSelector selector)
        {
            if (_currentSelector != null)
            {
                if (!_hasCombinator)
                {
                    var compound = _currentSelector as AggregateSelectorList;

                    if (compound == null)
                    {
                        compound = new AggregateSelectorList("");
                        compound.AppendSelector(_currentSelector);
                    }

                    compound.AppendSelector(selector);
                    _currentSelector = compound;
                }
                else
                {
                    if (_complexSelector == null)
                    {
                        _complexSelector = new ComplexSelector();
                    }

                    _complexSelector.AppendSelector(_currentSelector, _combinator);
                    _combinator = Combinator.Descendent;
                    _hasCombinator = false;
                    _currentSelector = selector;
                }
            }
            else
            {
                if (_currentSelector == null && _complexSelector == null && _combinator == Combinator.Namespace)
                {
                    _complexSelector = new ComplexSelector();
                    _complexSelector.AppendSelector(SimpleSelector.Type(""), _combinator);
                    _currentSelector = selector;
                }
                else
                {
                    _combinator = Combinator.Descendent;
                    _hasCombinator = false;
                    _currentSelector = selector;
                }
            }
        }

        private void Insert(Combinator combinator)
        {
            _hasCombinator = true;

            if (combinator != Combinator.Descendent)
            {
                _combinator = combinator;
            }
        }

        private void ParseDelimiter(Block token)
        {
            switch (((DelimiterBlock)token).Value)
            {
                case Specification.Comma:
                    InsertCommaDelimited();
                    return;

                case Specification.GreaterThan:
                    Insert(Combinator.Child);
                    return;

                case Specification.PlusSign:
                    Insert(Combinator.AdjacentSibling);
                    return;

                case Specification.Tilde:
                    Insert(Combinator.Sibling);
                    return;

                case Specification.Asterisk:
                    Insert(SimpleSelector.All);
                    return;

                case Specification.Period:
                    _selectorOperation = SelectorOperation.Class;
                    return;

                case Specification.Pipe:
                    Insert(Combinator.Namespace);
                    return;
            }
        }

        private BaseSelector GetChildSelector<T>() where T : NthChildSelector, new()
        {
            var selector = new T();

            if (_attributeValue.Equals(PseudoSelectorPrefix.NthChildOdd, StringComparison.OrdinalIgnoreCase))
            {
                selector.Step = 2;
                selector.Offset = 1;
                selector.FunctionText = PseudoSelectorPrefix.NthChildOdd;
            }
            else if (_attributeValue.Equals(PseudoSelectorPrefix.NthChildEven, StringComparison.OrdinalIgnoreCase))
            {
                selector.Step = 2;
                selector.Offset = 0;
                selector.FunctionText = PseudoSelectorPrefix.NthChildEven;
            }
            else if (!int.TryParse(_attributeValue, out selector.Offset))
            {
                var index = _attributeValue.IndexOf(PseudoSelectorPrefix.NthChildN, StringComparison.OrdinalIgnoreCase);

                if (_attributeValue.Length <= 0 || index == -1)
                {
                    return selector;
                }

                var first = _attributeValue.Substring(0, index).Replace(" ", "");

                var second = "";

                if (_attributeValue.Length > index + 1)
                {
                    second = _attributeValue.Substring(index + 1).Replace(" ", "");
                }

                if (first == string.Empty || (first.Length == 1 && first[0] == Specification.PlusSign))
                {
                    selector.Step = 1;
                }
                else if (first.Length == 1 && first[0] == Specification.MinusSign)
                {
                    selector.Step = -1;
                }
                else
                {
                    int step;
                    if (int.TryParse(first, out step))
                    {
                        selector.Step = step;
                    }
                }

                if (second == string.Empty)
                {
                    selector.Offset = 0;
                }
                else
                {
                    int offset;
                    if (int.TryParse(second, out offset))
                    {
                        selector.Offset = offset;
                    }
                }
            }

            return selector;
        }

        private static BaseSelector GetPseudoSelector(Block token)
        {
            switch (((SymbolBlock)token).Value)
            {
                case PseudoSelectorPrefix.PseudoRoot:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoRoot);

                case PseudoSelectorPrefix.PseudoFirstOfType:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoFirstOfType);

                case PseudoSelectorPrefix.PseudoLastoftype:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoLastoftype);

                case PseudoSelectorPrefix.PseudoOnlychild:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoOnlychild);

                case PseudoSelectorPrefix.PseudoOnlyOfType:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoOnlyOfType);

                case PseudoSelectorPrefix.PseudoFirstchild:
                    return FirstChildSelector.Instance;

                case PseudoSelectorPrefix.PseudoLastchild:
                    return LastChildSelector.Instance;

                case PseudoSelectorPrefix.PseudoEmpty:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoEmpty);

                case PseudoSelectorPrefix.PseudoLink:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoLink);

                case PseudoSelectorPrefix.PseudoVisited:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoVisited);

                case PseudoSelectorPrefix.PseudoActive:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoActive);

                case PseudoSelectorPrefix.PseudoHover:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoHover);

                case PseudoSelectorPrefix.PseudoFocus:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoFocus);

                case PseudoSelectorPrefix.PseudoTarget:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoTarget);

                case PseudoSelectorPrefix.PseudoEnabled:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoEnabled);

                case PseudoSelectorPrefix.PseudoDisabled:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoDisabled);

                case PseudoSelectorPrefix.PseudoDefault:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoDefault);

                case PseudoSelectorPrefix.PseudoChecked:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoChecked);

                case PseudoSelectorPrefix.PseudoIndeterminate:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoIndeterminate);

                case PseudoSelectorPrefix.PseudoUnchecked:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoUnchecked);

                case PseudoSelectorPrefix.PseudoValid:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoValid);

                case PseudoSelectorPrefix.PseudoInvalid:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoInvalid);

                case PseudoSelectorPrefix.PseudoRequired:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoRequired);

                case PseudoSelectorPrefix.PseudoReadonly:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoReadonly);

                case PseudoSelectorPrefix.PseudoReadwrite:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoReadwrite);

                case PseudoSelectorPrefix.PseudoInrange:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoInrange);

                case PseudoSelectorPrefix.PseudoOutofrange:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoOutofrange);

                case PseudoSelectorPrefix.PseudoOptional:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoOptional);

                case PseudoSelectorPrefix.PseudoElementBefore:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoElementBefore);

                case PseudoSelectorPrefix.PseudoElementAfter:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoElementAfter);

                case PseudoSelectorPrefix.PseudoElementFirstline:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoElementFirstline);

                case PseudoSelectorPrefix.PseudoElementFirstletter:
                    return SimpleSelector.PseudoClass(PseudoSelectorPrefix.PseudoElementFirstletter);

                default:
                    return SimpleSelector.PseudoClass(token.ToString());
            }
        }
    }
}
