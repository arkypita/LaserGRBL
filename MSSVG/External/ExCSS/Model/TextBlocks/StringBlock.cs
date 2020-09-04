
namespace ExCSS.Model.TextBlocks
{
    internal class StringBlock : Block
    {
        StringBlock(GrammarSegment type)
        {
            GrammarSegment = type;
        }

        internal static StringBlock Plain(string data, bool bad = false)
        {
            return new StringBlock(GrammarSegment.String) { Value = data, IsBad = bad };
        }

        internal static StringBlock Url(string data, bool bad = false)
        {
            return new StringBlock(GrammarSegment.Url) { Value = data, IsBad = bad };
        }

        internal string Value { get; private set; }

        internal bool IsBad { get; private set; }

        public override string ToString()
        {
            if (GrammarSegment == GrammarSegment.Url)
            {
                return "url(" + Value + ")";
            }

            return "'" + Value + "'";
        }
    }
}
