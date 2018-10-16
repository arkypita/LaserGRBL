using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Svg
{
    internal class CoordinateParser
    {
        private enum NumState
        {
            invalid,
            separator,
            prefix,
            integer,
            decPlace,
            fraction,
            exponent,
            expPrefix,
            expValue
        }

        private string _coords;
        private int _pos = 0;
        private NumState _currState = NumState.separator;
        private NumState _newState = NumState.separator;
        private int i = 0;
        private bool _parseWorked = true;

        public int Position { get { return _pos; } }

        public CoordinateParser(string coords)
        {
            _coords = coords;
            if (string.IsNullOrEmpty(_coords)) _parseWorked = false;
            if (char.IsLetter(coords[0])) i++;
        }

        public bool HasMore { get { return _parseWorked; } }

        private bool MarkState(bool state)
        {
            _parseWorked = state;
            i++;
            return state;
        }

        public bool TryGetBool(out bool result)
        {
            while (i < _coords.Length && _parseWorked)
            {
                switch (_currState)
                {
                    case NumState.separator:
                        if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else if (_coords[i] == '0')
                        {
                            result = false;
                            _newState = NumState.separator;
                            _pos = i + 1;
                            return MarkState(true);
                        }
                        else if (_coords[i] == '1')
                        {
                            result = true;
                            _newState = NumState.separator;
                            _pos = i + 1;
                            return MarkState(true);
                        }
                        else
                        {
                            result = false;
                            return MarkState(false);
                        }
                        break;
                    default:
                        result = false;
                        return MarkState(false);
                }
                i++;
            }
            result = false;
            return MarkState(false);
        }

        public bool TryGetFloat(out float result)
        {
            while (i < _coords.Length && _parseWorked)
            {
                switch (_currState)
                {
                    case NumState.separator:
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.integer;
                        }
                        else if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (_coords[i])
                            {
                                case '.':
                                    _newState = NumState.decPlace;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.prefix;
                                    break;
                                default:
                                    _newState = NumState.invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.prefix:
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.integer;
                        }
                        else if (_coords[i] == '.')
                        {
                            _newState = NumState.decPlace;
                        }
                        else
                        {
                            _newState = NumState.invalid;
                        }
                        break;
                    case NumState.integer:
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.integer;
                        }
                        else if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (_coords[i])
                            {
                                case '.':
                                    _newState = NumState.decPlace;
                                    break;
                                case 'E':
                                case 'e':
                                    _newState = NumState.exponent;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.prefix;
                                    break;
                                default:
                                    _newState = NumState.invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.decPlace:
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.fraction;
                        }
                        else if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (_coords[i])
                            {
                                case 'E':
                                case 'e':
                                    _newState = NumState.exponent;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.prefix;
                                    break;
                                default:
                                    _newState = NumState.invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.fraction:
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.fraction;
                        }
                        else if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (_coords[i])
                            {
                                case '.':
                                    _newState = NumState.decPlace;
                                    break;
                                case 'E':
                                case 'e':
                                    _newState = NumState.exponent;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.prefix;
                                    break;
                                default:
                                    _newState = NumState.invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.exponent:
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.expValue;
                        }
                        else if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.invalid;
                        }
                        else
                        {
                            switch (_coords[i])
                            {
                                case '+':
                                case '-':
                                    _newState = NumState.expPrefix;
                                    break;
                                default:
                                    _newState = NumState.invalid;
                                    break;
                            }
                        }
                        break;
                    case NumState.expPrefix:
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.expValue;
                        }
                        else
                        {
                            _newState = NumState.invalid;
                        }
                        break;
                    case NumState.expValue:
                        if (char.IsNumber(_coords[i]))
                        {
                            _newState = NumState.expValue;
                        }
                        else if (IsCoordSeparator(_coords[i]))
                        {
                            _newState = NumState.separator;
                        }
                        else
                        {
                            switch (_coords[i])
                            {
                                case '.':
                                    _newState = NumState.decPlace;
                                    break;
                                case '+':
                                case '-':
                                    _newState = NumState.prefix;
                                    break;
                                default:
                                    _newState = NumState.invalid;
                                    break;
                            }
                        }
                        break;
                }

                if (_newState < _currState)
                {
                    result = float.Parse(_coords.Substring(_pos, i - _pos), NumberStyles.Float, CultureInfo.InvariantCulture);
                    _pos = i;
                    _currState = _newState;
                    return MarkState(true);
                }
                else if (_newState != _currState && _currState == NumState.separator)
                {
                    _pos = i;
                }

                if (_newState == NumState.invalid)
                {
                    result = float.MinValue;
                    return MarkState(false);
                }
                _currState = _newState;
                i++;
            }

            if (_currState == NumState.separator || !_parseWorked || _pos >= _coords.Length)
            {
                result = float.MinValue;
                return MarkState(false);
            }
            else
            {
                result = float.Parse(_coords.Substring(_pos, _coords.Length - _pos), NumberStyles.Float, CultureInfo.InvariantCulture);
                _pos = _coords.Length;
                return MarkState(true);
            }
        }

        private static bool IsCoordSeparator(char value)
        {
            switch (value)
            {
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                case ',':
                    return true;
            }
            return false;
        }
    }
}
