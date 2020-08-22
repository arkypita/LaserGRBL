using ExCSS.Model.Extensions;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class Property
    {
        private Term _term;
        private bool _important;
        
        public Property(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public Term Term
        {
            get { return _term; }
            set { _term = value; }
        }

        public bool Important
        {
            get { return _important; }
            set { _important = value; }
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool friendlyFormat, int indentation = 0)
        { 
            var value = Name + ":" + _term;

            if (_important)
            {
                value += " !important";
            }

            return value.Indent(friendlyFormat, indentation);
        }
    }
}
