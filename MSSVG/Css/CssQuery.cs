using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fizzler;
using ExCSS;

namespace Svg.Css
{
    internal static class CssQuery
    {
        public static IEnumerable<SvgElement> QuerySelectorAll(this SvgElement elem, string selector, SvgElementFactory elementFactory)
        {
            var generator = new SelectorGenerator<SvgElement>(new SvgElementOps(elementFactory));
            Fizzler.Parser.Parse(selector, generator);
            return generator.Selector(Enumerable.Repeat(elem, 1));
        }

        public static int GetSpecificity(this BaseSelector selector)
        {
            if (selector is SimpleSelector)
            {
                var simpleCode = selector.ToString().ToLowerInvariant();
                if (simpleCode.StartsWith(":not("))
                {
                    simpleCode = simpleCode.Substring(5, simpleCode.Length - 6);
                    return GetSpecificity(new SimpleSelector(simpleCode));
                }
                else if (simpleCode.StartsWith("#"))
                {
                    // ID selector
                    return 1 << 12;
                }
                else if (simpleCode.StartsWith("::") || simpleCode == ":after" || simpleCode == ":before" ||
                    simpleCode == ":first-letter" || simpleCode == ":first-line" || simpleCode == ":selection")
                {
                    // pseudo-element
                    return 1 << 4;
                }
                else if (simpleCode.StartsWith(".") || simpleCode.StartsWith(":") || simpleCode.StartsWith("["))
                {
                    // class, pseudo-class, attribute
                    return 1 << 8;
                }
                else if (selector == SimpleSelector.All)
                {
                    // all selector
                    return 0;
                }
                else
                {
                    // element selector
                    return 1 << 4;
                }
            }
            else
            {
                var list = selector as IEnumerable<BaseSelector>;
                if (list != null)
                {
                    return (from s in list select GetSpecificity(s)).Aggregate((p, c) => p + c);
                }
                else
                {
                    var complex = selector as IEnumerable<CombinatorSelector>;
                    if (complex != null)
                    {
                        return (from s in complex select GetSpecificity(s.Selector)).Aggregate((p, c) => p + c);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}
