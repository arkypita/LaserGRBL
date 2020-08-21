
namespace ExCSS.Model.TextBlocks
{
    internal class PipeBlock : Block
    {
        private readonly static PipeBlock TokenBlock;

        static PipeBlock()
        {
            TokenBlock = new PipeBlock();
        }

        PipeBlock()
        {
            GrammarSegment = GrammarSegment.Column;
        }

        internal static PipeBlock Token
        {
            get { return TokenBlock; }
        }

        public override string ToString()
        {
            return "||";
        }
    }
}
