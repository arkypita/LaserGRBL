
namespace ExCSS.Model.TextBlocks
{
    internal class MatchBlock : Block
    {
        internal readonly static MatchBlock Include = new MatchBlock { GrammarSegment = GrammarSegment.IncludeMatch };
        internal readonly static MatchBlock Dash = new MatchBlock { GrammarSegment = GrammarSegment.DashMatch };
        internal readonly static Block Prefix = new MatchBlock { GrammarSegment = GrammarSegment.PrefixMatch };
        internal readonly static Block Substring = new MatchBlock { GrammarSegment = GrammarSegment.SubstringMatch };
        internal readonly static Block Suffix = new MatchBlock { GrammarSegment = GrammarSegment.SuffixMatch };
        internal readonly static Block Not = new MatchBlock { GrammarSegment = GrammarSegment.NegationMatch };

        public override string ToString()
        {
            switch (GrammarSegment)
            {
                case GrammarSegment.SubstringMatch:
                    return "*=";

                case GrammarSegment.SuffixMatch:
                    return "$=";

                case GrammarSegment.PrefixMatch:
                    return "^=";

                case GrammarSegment.IncludeMatch:
                    return "~=";

                case GrammarSegment.DashMatch:
                    return "|=";

                case GrammarSegment.NegationMatch:
                    return "!=";
            }

            return string.Empty;
        }
    }
}
