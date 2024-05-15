using System;
using System.Drawing;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace SharpGL.SceneGraph.Assets
{
	/// <summary>
	/// A Texture object is simply an array of bytes. It has OpenGL functions, but is
	/// not limited to OpenGL, so DirectX or custom library functions could be later added.
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[Serializable()]
	public class Texture : Asset, IBindable
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class.
        /// </summary>
        public Texture()
        {
        }

        /// <summary>
        /// Pushes this object into the provided OpenGL instance.
        /// This will generally push elements of the attribute stack
        /// and then set current values.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public virtual void Push(OpenGL gl)
        {
            //  Push texture attributes.
            gl.PushAttrib(Enumerations.AttributeMask.Texture);

            //  Bind the texture.
            Bind(gl);
        }

        /// <summary>
        /// Pops this object from the provided OpenGL instance.
        /// This will generally pop elements of the attribute stack,
        /// restoring previous attribute values.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public virtual void Pop(OpenGL gl)
        {
            //  Pop attributes.
            gl.PopAttrib();
        }

        /// <summary>
        /// Bind to the specified OpenGL instance.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void Bind(OpenGL gl)
        {
            //	Bind our texture object (make it the current texture).
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, TextureName);
        }
        
        /// <summary>
        /// This function creates the underlying OpenGL object.
        /// </summary>
        /// <param name="gl"></param>
        /// <returns></returns>
        public virtual bool Create(OpenGL gl)
        {
            //  If the texture currently exists in OpenGL, destroy it.
            Destroy(gl);

            //  Generate and store a texture identifier.
            gl.GenTextures(1, glTextureArray);

            return true;
        }

        /// <summary>
        /// This function creates the texture from an image.
        /// </summary>
        /// <param name="gl">The OpenGL object.</param>
        /// <param name="image">The image.</param>
        /// <returns>True if the texture was successfully loaded.</returns>
        public virtual bool Create(OpenGL gl, Bitmap image)
        {
            //  Create the underlying OpenGL object.
            Create(gl);

            //	Get the maximum texture size supported by OpenGL.
            int[] textureMaxSize = { 0 };
            gl.GetInteger(OpenGL.GL_MAX_TEXTURE_SIZE, textureMaxSize);

            //	Find the target width and height sizes, which is just the highest
            //	posible power of two that'll fit into the image.
            int targetWidth = textureMaxSize[0];
            int targetHeight = textureMaxSize[0];

            for (int size = 1; size <= textureMaxSize[0]; size *= 2)
            {
                if (image.Width < size)
                {
                    targetWidth = size / 2;
                    break;
                }
                if (image.Width == size)
                    targetWidth = size;

            }

            for (int size = 1; size <= textureMaxSize[0]; size *= 2)
            {
                if (image.Height < size)
                {
                    targetHeight = size / 2;
                    break;
                }
                if (image.Height == size)
                    targetHeight = size;
            }

            //  If need to scale, do so now.
            if (image.Width != targetWidth || image.Height != targetHeight)
            {
                //  Resize the image.
                Image newImage = image.GetThumbnailImage(targetWidth, targetHeight, null, IntPtr.Zero);

                //  Destory the old image, and reset.
                image.Dispose();
                image = (Bitmap)newImage;
            }

            //  Lock the image bits (so that we can pass them to OGL).
            BitmapData bitmapData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            //	Set the width and height.
            width = image.Width;
            height = image.Height;

            //	Bind our texture object (make it the current texture).
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, TextureName);

            //  Set the image data.
            gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, (int)OpenGL.GL_RGBA,
                width, height, 0, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE,
                bitmapData.Scan0);

            //  Unlock the image.
            image.UnlockBits(bitmapData);

            //  Dispose of the image file.
            image.Dispose();

            //  Set linear filtering mode.
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);

            //  We're done!
            return true;
        }

        /// <summary>
        /// This function creates the texture from an image file.
        /// </summary>
        /// <param name="gl">The OpenGL object.</param>
        /// <param name="path">The path to the image file.</param>
        /// <returns>True if the texture was successfully loaded.</returns>
        public virtual bool Create(OpenGL gl, string path)
        {
            //  Try and load the bitmap. Return false on failure.
            try
            {
                using (Bitmap image = new Bitmap(path))
                {
                    if (image == null)
                        return false;

                    //  Call the main create function.
                    return Create(gl, image);
                }
            }
            catch
            {
                return false; // Couldn't load the image; probably because of an invalid path
            }
        }

	
		/// <summary>
		/// This function destroys the OpenGL object that is a representation of this texture.
		/// </summary>
		public virtual void Destroy(OpenGL gl)
		{
            //  Only destroy if we have a valid id.
            if(glTextureArray[0] != 0)
            {
                //	Delete the texture object.
                gl.DeleteTextures(1, glTextureArray);
                glTextureArray[0] = 0;

                //  Destroy the pixel data.
                pixelData = null;

                //  Reset width and height.
                width = height = 0;
            }
		}

        /// <summary>
        /// This function (attempts) to make a bitmap from the raw data. The fact that
        /// the byte array is a managed type makes it slightly more complicated.
        /// </summary>
        /// <returns>The texture object as a Bitmap.</returns>
        public virtual Bitmap ToBitmap()
        {
            //  Check for the trivial case.
            if (pixelData == null)
                return null;

            //  Pin the pixel data.
            GCHandle handle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);

            //  Create the bitmap.
            Bitmap bitmap = new Bitmap(width, height, width * 4,
                    PixelFormat.Format32bppRgb, handle.AddrOfPinnedObject());

            //  Free the data.
            handle.Free();

            //  Return the bitmap.
            return bitmap;
        }

		/// <summary>
		/// This is an array of bytes (r, g, b, a) that represent the pixels in this
		/// texture object.
		/// </summary>
        private byte[] pixelData = null;

		/// <summary>
		/// The width of the texture image.
		/// </summary>
        private int width = 0;

		/// <summary>
		/// The height of the texture image.
		/// </summary>
        private int height = 0;

		/// <summary>
		/// This is for OpenGL textures, it is the unique ID for the OpenGL texture.
		/// </summary>
        private uint[] glTextureArray = new uint[1] { 0 };

        /// <summary>
        /// Gets the name of the texture.
        /// </summary>
        /// <value>
        /// The name of the texture.
        /// </value>
		[Description("The internal texture code.."), Category("Texture")]
		public uint TextureName
		{
            get { return glTextureArray[0]; }
		}
    }
}
