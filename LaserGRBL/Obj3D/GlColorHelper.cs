using SharpGL.SceneGraph;
using System.Drawing;

namespace LaserGRBL.Obj3D
{
    public static class GlColorHelper
    {

        public static void FromColor(this GLColor destColor, Color color)
        {
            destColor.Set(color.R / 255f, color.G / 255f, color.B / 255f);
            destColor.A = color.A / 255f;
        }

    }

}
