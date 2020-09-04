
// ReSharper disable once CheckNamespace
namespace ExCSS
{
    internal sealed class NthLastOfTypeSelector : NthChildSelector, IToString
    {
        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return FormatSelector(PseudoSelectorPrefix.PseudoFunctionNthLastOfType);
        }
    }
}