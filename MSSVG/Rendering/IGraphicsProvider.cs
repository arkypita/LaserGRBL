using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Svg
{
    public interface IGraphicsProvider
    {
        Graphics GetGraphics();
    }
}
