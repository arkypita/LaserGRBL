using System;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    internal sealed class NthLastChildSelector : NthChildSelector, IToString
    {
        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            return FormatSelector(PseudoSelectorPrefix.PseudoFunctionNthlastchild);
        }
    }
}