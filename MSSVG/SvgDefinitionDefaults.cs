using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg
{
    /// <summary>
    /// Holds a dictionary of the default values of the SVG specification 
    /// </summary>
    public static class SvgDefaults
    {
        //internal dictionary for the defaults
        private static readonly Dictionary<string, string> _defaults = new Dictionary<string, string>();

        static SvgDefaults()
        {
            _defaults["d"] = "";

            _defaults["viewBox"] = "0, 0, 0, 0";

            _defaults["visibility"] = "visible";
            _defaults["opacity"] = "1";
            _defaults["clip-rule"] = "nonzero";

            _defaults["transform"] = "";
            _defaults["rx"] = "0";
            _defaults["ry"] = "0";
            _defaults["cx"] = "0";
            _defaults["cy"] = "0";

            _defaults["fill"] = "";
            _defaults["fill-opacity"] = "1";
            _defaults["fill-rule"] = "nonzero";

            _defaults["stroke"] = "none";
            _defaults["stroke-opacity"] = "1";
            _defaults["stroke-width"] = "1";
            _defaults["stroke-miterlimit"] = "4";
            _defaults["stroke-linecap"] = "butt";
            _defaults["stroke-linejoin"] = "miter";
            _defaults["stroke-dasharray"] = "none";
            _defaults["stroke-dashoffset"] = "0";
        }

        /// <summary>
        /// Checks whether the property value is the default value of the svg definition.
        /// </summary>
        /// <param name="attributeName">Name of the svg attribute</param>
        /// <param name="propertyValue">.NET value of the attribute</param>
        public static bool IsDefault(string attributeName, string value)
        {
            if (_defaults.ContainsKey(attributeName))
            {
                if (_defaults[attributeName] == value) return true;
            }
            return false;
        }
    }
}
