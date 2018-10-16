using System.Collections.Generic;
using System.Text;
using ExCSS.Model;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class GenericFunction : Term
    {
        public string Name { get; set; }
        public TermList Arguments { get; set; }

        public GenericFunction(string name, List<Term> arguments)
        {
            this.Name = name;

            var list = new TermList();
            for (int n = 0; n < arguments.Count; n++)
            {
                list.AddTerm(arguments[n]);
                if (n == arguments.Count - 1)
                    break;
                list.AddSeparator(GrammarSegment.Comma);
            }
            this.Arguments = list;
        }

        public override string ToString()
        {
            return Name + "(" + Arguments + ")";
        }
    }
}