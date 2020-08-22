using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    public class SvgException : FormatException
    {
        public SvgException(string message) : base(message)
        {
        }
    }

    public class SvgIDException : FormatException
    {
        public SvgIDException(string message)
            : base(message)
        {
        }
    }

    public class SvgIDExistsException : SvgIDException
    {
        public SvgIDExistsException(string message)
            : base(message)
        {
        }
    }

    public class SvgIDWrongFormatException : SvgIDException
    {
        public SvgIDWrongFormatException(string message)
            : base(message)
        {
        }
    }
}