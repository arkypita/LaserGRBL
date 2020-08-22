using ExCSS.Model;
using ExCSS.Model.Extensions;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class KeyframeRule : RuleSet, ISupportsDeclarations
    {
        private string _value;

        public KeyframeRule()
        {
            Declarations = new StyleDeclaration();
            RuleType = RuleType.Keyframe;
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public StyleDeclaration Declarations { get; private set; }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return string.Empty.Indent(friendlyFormat, indentation) +
                _value + 
                "{" + 
                Declarations.ToString(friendlyFormat, indentation) +
                "}".NewLineIndent(friendlyFormat, indentation);
        }
    }
}
