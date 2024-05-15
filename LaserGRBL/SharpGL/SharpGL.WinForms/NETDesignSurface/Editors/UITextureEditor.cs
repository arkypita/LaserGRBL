using System.Drawing.Design;
using SharpGL.SceneGraph.Assets;

namespace SharpGL.WinForms.NETDesignSurface.Editors
{
	/// <summary>
	/// The texture editor makes Textures in the properties window much better,
	/// giving them a little thumbnail.
	/// </summary>
	internal class UITextureEditor : UITypeEditor
	{
		public override System.Boolean GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return true;
		}

		public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
		{
			//	Make sure we are dealing with an actual texture.
			if (e.Value is Texture)
			{
				//	Get the texture.
				Texture tex = e.Value as Texture;

				if (tex.TextureName != 0)
					e.Graphics.DrawImage(tex.ToBitmap(), e.Bounds);
			}
		}
	}
}