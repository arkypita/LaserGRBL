using System.Text;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class MultipleSelectorList : SelectorList, IToString
    {
        internal static MultipleSelectorList Create(params SimpleSelector[] selectors)
        {
            var multiple = new MultipleSelectorList();

            foreach (var selector in selectors)
            {
                multiple.Selectors.Add(selector);
            }

            return multiple;
        }

        internal bool IsInvalid { get; set; }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            var builder = new StringBuilder();

            if (Selectors.Count <= 0)
            {
                return builder.ToString();
            }

            builder.Append(Selectors[0]);

            for (var i = 1; i < Selectors.Count; i++)
            {
                builder.Append(',').Append(Selectors[i]);
            }

            return builder.ToString();
        }
    }
}
