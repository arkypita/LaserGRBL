using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExCSS.Model.Extensions;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public sealed class StyleSheet
    {
        private readonly List<RuleSet> _rules;

        public StyleSheet()
        {
            _rules = new List<RuleSet>();
            Errors = new List<StylesheetParseError>();
        }

        public List<RuleSet> Rules
        {
            get { return _rules; }
        }

        public StyleSheet RemoveRule(int index)
        {
            if (index >= 0 && index < _rules.Count)
            {
                _rules.RemoveAt(index);
            }

            return this;
        }

        public StyleSheet InsertRule(string rule, int index)
        {
            if (index < 0 || index > _rules.Count)
            {
                return this;
            }

            var value = Parser.ParseRule(rule);
            _rules.Insert(index, value);

            return this;
        }

        public IList<StyleRule> StyleRules
        {
            get
            {
                return Rules.Where(r => r is StyleRule).Cast<StyleRule>().ToList();
            }
        } 

        public IList<CharacterSetRule> CharsetDirectives
        {
            get
            {
                return GetDirectives<CharacterSetRule>(RuleType.Charset);
            }
        }

        public IList<ImportRule> ImportDirectives
        {
            get
            {
                return GetDirectives<ImportRule>(RuleType.Import);
            }
        }

        public IList<FontFaceRule> FontFaceDirectives
        {
            get
            {
                return GetDirectives<FontFaceRule>(RuleType.FontFace);
            }
        }

        public IList<KeyframesRule> KeyframeDirectives
        {
            get
            {
                return GetDirectives<KeyframesRule>(RuleType.Keyframes);
            }
        }

        public IList<MediaRule> MediaDirectives
        {
            get
            {
                return GetDirectives<MediaRule>(RuleType.Media);

            }
        }

        public IList<PageRule> PageDirectives
        {
            get
            {
                return GetDirectives<PageRule>(RuleType.Page);

            }
        }

        public IList<SupportsRule> SupportsDirectives
        {
            get
            {
                return GetDirectives<SupportsRule>(RuleType.Supports);
            }
        }

        public IList<NamespaceRule> NamespaceDirectives
        {
            get
            {
                return GetDirectives<NamespaceRule>(RuleType.Namespace);
            }
        }

        private IList<T> GetDirectives<T>(RuleType ruleType)
        {
            return Rules.Where(r => r.RuleType == ruleType).Cast<T>().ToList();
        }

        public List<StylesheetParseError> Errors { get; private set; }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool friendlyFormat, int indentation = 0)
        {
            var builder = new StringBuilder();

            foreach (var rule in _rules)
            {
                builder.Append(rule.ToString(friendlyFormat, indentation).TrimStart() + (friendlyFormat ? Environment.NewLine : ""));
            }

            return builder.TrimFirstLine().TrimLastLine().ToString();
        }
    }
}
