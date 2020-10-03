using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LaserGRBL.RasterConverter
{
    
    public class FontViewModel
    {
        public FontFamily Family { get; private set; }

        public FontViewModel(FontFamily family)
        {
            this.Family = family;
        }

        public override string ToString()
        {
            return Family.Name;
        }
    }
}
