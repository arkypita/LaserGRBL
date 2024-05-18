using System;
using System.Drawing;
using System.Windows.Forms;

using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;

namespace SharpGL.Controls
{
	/// <summary>
	/// This allows a list view item to be created from a material, which is really
	/// useful, especially for 3D apps. For more advanced functionality, just derive
	/// from this class, as there's a lot of good code here. Writing this sort of 
	/// class from scratch is pointless.
	/// 
	/// At the moment this class is not working properly, because of changes to the
	/// material class, use with caution!
	/// </summary>
	public class MaterialListViewItem : System.Windows.Forms.ListViewItem
	{
		/// <summary>
		/// This function creates the list view item from a material, with an image
		/// of the specified size.
		/// </summary>
		/// <param name="material">The material to create it from.</param>
		/// <param name="images">The set of images.</param>
		public MaterialListViewItem(Material material, ImageList images)
		{
			//	Create a preview of the material.
		//	Bitmap preview = material.CreatePreview(images.ImageSize.Width, images.ImageSize.Height);

			//	Draw the texture in the top left cornder.
		//	Graphics graphics = Graphics.FromImage(preview);
		//	graphics.DrawImage(material.Texture.ToBitmap(), new Rectangle(5, 5, 25, 25));
		//	graphics.Dispose();

			//	Add this preview image to the imagelist.
		//	ImageIndex = images.Images.Add(preview, Color.Aquamarine);
	
			//	set the text, and the tag.
			Text = material.Name;
			Tag = material;
		}
	}
}
