using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class ComplexSelector : BaseSelector, IEnumerable<CombinatorSelector>
    {
        private readonly List<CombinatorSelector> _selectors;

        public ComplexSelector()
        {
            _selectors = new List<CombinatorSelector>();
        }

        public ComplexSelector AppendSelector(BaseSelector selector, Combinator combinator)
        {
            _selectors.Add(new CombinatorSelector(selector, combinator));
            return this;
        }

        public IEnumerator<CombinatorSelector> GetEnumerator()
        {
            return _selectors.GetEnumerator();
        }
       
        internal void ConcludeSelector(BaseSelector selector)
        {
            _selectors.Add(new CombinatorSelector { Selector = selector });
        }

        public int Length
        {
            get { return _selectors.Count; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_selectors).GetEnumerator();
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            var builder = new StringBuilder();

            if (_selectors.Count <= 0)
            {
                return builder.ToString();
            }

            var n = _selectors.Count - 1;

            for (var i = 0; i < n; i++)
            {
                builder.Append(_selectors[i].Selector);
                builder.Append(_selectors[i].Character);
            }

            builder.Append(_selectors[n].Selector);

            return builder.ToString();
        }
    }
}
