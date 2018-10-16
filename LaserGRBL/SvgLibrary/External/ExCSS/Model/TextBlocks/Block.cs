
namespace ExCSS.Model.TextBlocks
{
    internal abstract class Block
    {
        internal GrammarSegment GrammarSegment { get;set; }

        internal static PipeBlock Column
        {
            get { return PipeBlock.Token; }
        }

        internal static DelimiterBlock Delim(char value)
        {
            return new DelimiterBlock(value);
        }

        internal static NumericBlock Number(string value)
        {
            return new NumericBlock(value);
        }

        internal static RangeBlock Range(string start, string end)
        {
            return new RangeBlock().SetRange(start, end);
        }
    }
}
