using System;
using System.ComponentModel;
using System.Drawing.Design;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;
using SharpGL.WinForms.NETDesignSurface.Converters;
using SharpGL.WinForms.NETDesignSurface.Editors;

namespace SharpGL.WinForms.NETDesignSurface
{
    internal static class DesignSurface
    {
        public static void InitialiseDesignSurface()
        {
            //  TODO: When can we call this?

            //  We cannot decorate the 'Texture', 'Vertex' and associated SceneGraph classes with the attributes which define
            //  the type converters and editors, becauses the SceneGraph library is cross-platform. So we have to associate
            //  the attributes at runtime.
            TypeDescriptor.AddAttributes(typeof(Texture), new EditorAttribute(typeof(UITextureEditor), typeof(UITypeEditor)));
            TypeDescriptor.AddAttributes(typeof(Vertex), new EditorAttribute(typeof(VertexConverter), typeof(TypeConverter)));
        }
    }
}
