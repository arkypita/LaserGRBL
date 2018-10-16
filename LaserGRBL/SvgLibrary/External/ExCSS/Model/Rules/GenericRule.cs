// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class GenericRule : AggregateRule
    {
        private string _text;
        private bool _stopped;

        internal void SetInstruction(string text)
        {
            _text = text;
            _stopped = true;
        }

        internal void SetCondition(string text)
        {
            _text = text;
            _stopped = false;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            if (_stopped)
            {
                return _text + ";";
            }

            return _text + "{" + RuleSets + "}";
        }
    }
}
