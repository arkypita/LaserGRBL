using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
    public class Constants
    {
        public static string GCODE_STD_HEADER = "G90 (use absolute coordinates)";
        public static string GCODE_STD_PASSES = ";(Uncomment if you want to sink Z axis)\r\n;G91 (use relative coordinates)\r\n;G0 Z-1 (sinks the Z axis, 1mm)\r\n;G90 (use absolute coordinates)";
        public static string GCODE_STD_FOOTER = "G0 X0 Y0 Z0 (move back to origin)";
    }
}
