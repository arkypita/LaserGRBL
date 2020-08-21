// ReSharper disable once CheckNamespace
namespace ExCSS
{
    internal sealed class FirstChildSelector : BaseSelector, IToString
    {
        FirstChildSelector()
        { }

        static FirstChildSelector _instance;

        public static FirstChildSelector Instance
        {
            get { return _instance ?? (_instance = new FirstChildSelector()); }
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return ":" + PseudoSelectorPrefix.PseudoFirstchild;
        }
    }
}