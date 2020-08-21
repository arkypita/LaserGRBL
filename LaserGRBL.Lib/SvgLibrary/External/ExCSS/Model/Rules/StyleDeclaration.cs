using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class StyleDeclaration : IList<Property>
    {
        private readonly List<Property> _properties;
        private readonly Func<string> _getter;
        private readonly Action<string> _setter;
        private bool _blocking;

        public StyleDeclaration()
        {
            var text = string.Empty;
            _getter = () => text;
            _setter = value => text = value;
            _properties = new List<Property>();
        }

        public string Value
        {
            get { return _getter(); }
            set
            {
                Update(value);
                _setter(value);
            }
        }

        public RuleSet ParentRule { get; set; }

        public void Add(Property item)
        {
            _properties.Add(item);
        }

        public void Clear()
        {
            _properties.Clear();
        }

        public bool Contains(Property item)
        {
            return _properties.Contains(item);
        }

        public void CopyTo(Property[] array, int arrayIndex)
        {
            _properties.CopyTo(array, arrayIndex);
        }

        public bool Remove(Property item)
        {
            return _properties.Remove(item);
        }

        public int IndexOf(Property item)
        {
            return _properties.IndexOf(item);
        }

        public void Insert(int index, Property item)
        {
            _properties.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _properties.RemoveAt(index);
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool friendlyFormat, int indentation = 0)
        { 
            var builder = new StringBuilder();

            foreach (var property in _properties)
            {
                if (friendlyFormat)
                {
                    builder.Append(Environment.NewLine);
                }

                builder.Append(property.ToString(friendlyFormat, indentation+1)).Append(';');
            }

            return builder.ToString();
        }

        internal string RemoveProperty(string propertyName)
        {
            for (var i = 0; i < _properties.Count; i++)
            {
                if (!_properties[i].Name.Equals(propertyName))
                {
                    continue;
                }

                var value = _properties[i].Term;

                _properties.RemoveAt(i);
                Propagate();

                return value.ToString();
            }

            return null;
        }

        internal string GetPropertyValue(string propertyName)
        {
            for (var i = 0; i < _properties.Count; i++)
            {
                if (_properties[i].Name.Equals(propertyName))
                {
                    return _properties[i].Term.ToString();
                }
            }

            return null;
        }

        public IEnumerator<Property> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        public Property this[int index]
        {
            get { return _properties[index]; }
            set { _properties[index] = value; }
        }

        public List<Property> Properties
        {
            get { return _properties; }
        }

        public int Count { get { return _properties.Count; } }

        public bool IsReadOnly { get { return false; } }

        internal StyleDeclaration SetProperty(string propertyName, string propertyValue)
        {
            //_properties.Add(Parser.ParseDeclaration(propertyName + ":" + propertyValue));
            //TODO
            Propagate();
            return this;
        }

        internal void Update(string value)
        {
            if (_blocking)
            {
                return;
            }

            var rules = Parser.ParseDeclarations(value ?? string.Empty).Properties;

            _properties.Clear();
            _properties.AddRange(rules);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_properties).GetEnumerator();
        }

        private void Propagate()
        {
            _blocking = true;
            _setter(ToString());
            _blocking = false;
        }
    }
}