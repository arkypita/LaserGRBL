using System.Collections.Generic;

namespace ExCSS.Model
{
    interface ISupportsRuleSets
    {
        List<RuleSet> RuleSets { get; }
    }
}