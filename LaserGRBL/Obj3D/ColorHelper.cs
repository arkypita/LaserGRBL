using SharpGL.SceneGraph;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserGRBL.Obj3D
{

    public static class ColorHelper
    {

        public static float GlRed(this Color color) => color.R / 255f;
        public static float GlGreen(this Color color) => color.G / 255f;
        public static float GlBlue(this Color color) => color.B / 255f;
        public static float GlAlpha(this Color color) => color.A / 255f;

        public static Color Blend(this Color color, Color blendColor)
        {
            return Color.FromArgb(
                (int)((color.A + blendColor.A) / 2f),
                (int)((color.R + blendColor.R) / 2f),
                (int)((color.G + blendColor.G) / 2f),
                (int)((color.B + blendColor.B) / 2f)
            );
        }

    }

}
