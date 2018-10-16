using System;
using System.Collections.Generic;

namespace ExCSS.Model
{
    internal class FunctionBuffer
    {
        private readonly string _function;
        private readonly List<Term> _termList;
        private Term _term;

        internal FunctionBuffer(string function)
        {
            _termList = new List<Term>();
            _function = function;
        }

        public List<Term> TermList
        {
            get { return _termList; }
        }

        public Term Term
        {
            get { return _term; }
            set { _term = value; }
        }

        public void Include()
        {
            if (_term != null)
            {
                _termList.Add(_term);
            }

            _term = null;
        }

        public Term Done()
        {
            Include();
            return BuildFunctionTerm(_function, _termList); 
        }

        private Term BuildFunctionTerm(string name, List<Term> terms)
        {
            switch (name)
            {
                case "rgb":
                {
                    if (terms.Count == 3)
                    {
                        if (CheckNumber(terms[0]) &&
                            CheckNumber(terms[1]) && 
                            CheckNumber(terms[2]))
                        {
                            return HtmlColor.FromRgb(
                                ToByte(terms[0]), 
                                ToByte(terms[1]),
                                ToByte(terms[2]));
                        }
                    }

                    break;
                }
                case "rgba":
                {
                    if (terms.Count == 4)
                    {
                        if (CheckNumber(terms[0]) && 
                            CheckNumber(terms[1]) && 
                            CheckNumber(terms[2]) &&
                            CheckNumber(terms[3]))
                        {
                            return HtmlColor.FromRgba(
                                ToByte(terms[0]), 
                                ToByte(terms[1]),
                                ToByte(terms[2]), 
                                ToSingle(terms[3]));
                        }
                    }

                    break;
                }
                case "hsl":
                {
                    if (_termList.Count == 3)
                    {
                        if (CheckNumber(terms[0]) && 
                            CheckNumber(terms[1]) && 
                            CheckNumber(terms[2]))
                        {
                            return HtmlColor.FromHsl(
                                ToSingle(terms[0]), 
                                ToSingle(terms[1]), 
                                ToSingle(terms[2]));
                        }
                    }

                    break;
                }
            }

            return new GenericFunction(name, terms);
        }

        private static bool CheckNumber(Term cssValue)
        {
            return (cssValue is PrimitiveTerm && 
                    ((PrimitiveTerm)cssValue).PrimitiveType == UnitType.Number);
        }

        private static Single ToSingle(Term cssValue)
        {
            var value = ((PrimitiveTerm)cssValue).GetFloatValue(UnitType.Number);
                
            return value.HasValue 
                ? value.Value 
                : 0f;
        }

        private static byte ToByte(Single? value)
        {
            if (value.HasValue)
            {
                return (byte)Math.Min(Math.Max(value.Value, 0), 255);
            }

            return 0;
        }

        private static byte ToByte(Term cssValue)
        {
            return ToByte(((PrimitiveTerm)cssValue).GetFloatValue(UnitType.Number));
        }
    }
}