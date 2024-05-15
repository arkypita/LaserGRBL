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

        public static GLColor Blend(this GLColor color, GLColor blendColor)
        {
            return new GLColor(
                (float)((color.R + blendColor.R * 2) / 3f),
                (float)((color.G + blendColor.G * 2) / 3f),
                (float)((color.B + blendColor.B * 2) / 3f),
                (float)((color.A + blendColor.A * 2) / 3f)
            );
        }

    }

}
