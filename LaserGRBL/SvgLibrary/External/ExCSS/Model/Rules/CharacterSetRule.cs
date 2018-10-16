using ExCSS.Model.Extensions;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class CharacterSetRule : RuleSet
    {
        public CharacterSetRule() 
        {
            RuleType = RuleType.Charset;
        }

        public string Encoding { get; internal set; }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return string.Format("@charset '{0}';", Encoding).NewLineIndent(friendlyFormat, indentation);
        }
    }
}
