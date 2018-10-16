using ExCSS.Model;
using ExCSS.Model.Extensions;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class PageRule : RuleSet, ISupportsSelector, ISupportsDeclarations
    {
        private readonly StyleDeclaration _declarations;
        private BaseSelector _selector;
        private string _selectorText;

        public PageRule() 
        {
            _declarations = new StyleDeclaration();
            RuleType = RuleType.Page;
        }

        internal PageRule AppendRule(Property rule)
        {
            _declarations.Properties.Add(rule);
            return this;
        }

        public BaseSelector Selector
        {
            get { return _selector; }
            set
            {
                _selector = value;
                _selectorText = value.ToString();
            }
        }

        public StyleDeclaration Declarations
        {
            get { return _declarations; }
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            var pseudo = string.IsNullOrEmpty(_selectorText)
                             ? ""
                             : ":" + _selectorText;

            var declarations = _declarations.ToString(friendlyFormat, indentation);//.TrimFirstLine();

            return ("@page " + pseudo + "{").NewLineIndent(friendlyFormat, indentation) +
                declarations +
                "}".NewLineIndent(friendlyFormat, indentation);
        }
    }
}
