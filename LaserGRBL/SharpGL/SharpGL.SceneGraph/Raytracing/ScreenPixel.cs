using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Raytracing
{
    /// <summary>
    /// A ScreenPixel, password around when raytracing.
    /// </summary>
    public class ScreenPixel
    {
        public GLColor color = new GLColor();
        public Ray ray = new Ray();
        public int x = 0;
        public int y = 0;
        public Vertex worldpos = new Vertex();
    }
}
