using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public interface IRuleContainer
    {
        List<RuleSet> Declarations { get; }
    }
}