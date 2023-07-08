using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL.SvgConverter
{
    public enum AdaptiveLaserOptions
    {
        disabled = 0,
        Luminescence,
        Alpha,
        Red,
        Green,
        Blue
    }

    public static class AdaptiveLaserOptionsExtentions
    {
        public static AdaptiveLaserOptions ToAdaptiveLaserOptions(this string str)
        {
            var ret = AdaptiveLaserOptions.disabled;
            Enum.TryParse<AdaptiveLaserOptions>(str, out ret);
            return ret;
        }
    }
}
