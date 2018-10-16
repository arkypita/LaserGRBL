using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    internal interface ISvgSupportsCoordinateUnits
    {
        SvgCoordinateUnits GetUnits();
    }
}
