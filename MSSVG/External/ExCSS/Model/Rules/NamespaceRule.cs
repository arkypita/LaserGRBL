using ExCSS.Model.Extensions;
// ReSharper disable once CheckNamespace


namespace ExCSS
{
    public class NamespaceRule : RuleSet
    {
        public NamespaceRule() 
        {
            RuleType = RuleType.Namespace;
        }

        public string Uri { get; set; }

        public string Prefix { get; set; }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return string.IsNullOrEmpty(Prefix)
                 ? string.Format("@namespace '{0}';", Uri).NewLineIndent(friendlyFormat, indentation)
                 : string.Format("@namespace {0} '{1}';", Prefix, Uri).NewLineIndent(friendlyFormat, indentation);
        }
    }
}
