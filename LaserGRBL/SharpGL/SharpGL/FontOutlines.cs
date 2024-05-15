using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL
{
    /// <summary>
    /// A FontBitmap entry contains the details of a font face.
    /// </summary>
    internal class FontBitmapEntry
    {
        public IntPtr HDC
        {
            get;
            set;
        }

        public IntPtr HRC
        {
            get;
            set;
        }

        public string FaceName
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public uint ListBase
        {
            get;
            set;
        }

        public uint ListCount
        {
            get;
            set;
        }


        /// <summary>
        /// ftlPhysicsGuy - added utility
        /// Returns the width (in pixels) of a text string using this FontBitmapEntry's Char info.
        /// </summary>
        /// <param name="gl"></param>
        /// <param name="faceName"></param>
        /// <param name="fontSize"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public float GetTextWidth(string text)
        {
            if (m_abcFloatArray == null)
            {
                // Create a new font, set it as the font for the current DeviceContextHandle, use
                // GetCharABCWidthsFloat to get character width info from the DeviceContextHandle,
                // then set the current DeviceContextHandle's font back to the original font and
                // delete the one we created:

                // Create the temporary font based on the face name.
                var hFont = Win32.CreateFont(this.Height, 0, 0, 0, Win32.FW_DONTCARE, 0, 0, 0, Win32.DEFAULT_CHARSET,
                    Win32.OUT_OUTLINE_PRECIS, Win32.CLIP_DEFAULT_PRECIS, Win32.CLEARTYPE_QUALITY, Win32.VARIABLE_PITCH, this.FaceName);

                // Select the font handle (storing the old font handle in hOldObject)
                var hOldObject = Win32.SelectObject(this.HDC, hFont);

                // Create space for the ABCFLOAT array.
                m_abcFloatArray = new ABCFLOAT[256];

                // Get the list for this text
                Win32.GetCharABCWidthsFloat(this.HDC, 0, 255, m_abcFloatArray);

                // Reselect the old font.
                Win32.SelectObject(this.HDC, hOldObject);

                // Free the temporary font.
                Win32.DeleteObject(hFont);
            }

            float width = 0;
            foreach (char c in text)
            {
                byte b = (byte)c;
                ABCFLOAT abcFloat = m_abcFloatArray[b];
                width = width + abcFloat.abcfA + abcFloat.abcfB + abcFloat.abcfC;
            }

            return width;
        }

        /// <summary>
        /// Can hold the array of ABC widths for characters in this font (see the
        /// <see cref="ABCFLOAT"/> structure)
        /// </summary>
        ABCFLOAT[] m_abcFloatArray;

    }


    /// <summary>
    /// ftlPhysicsGuy - Used to find overall text width of a given string
    /// The ABCFLOAT structure contains the A, B, and C widths of a font character.
    /// </summary>
    public struct ABCFLOAT
    {
        /// <summary>
        /// The A spacing of the character.  The A spacing is the distance to add to the current position before drawing the character glyph.
        /// </summary>
        public float abcfA;
        /// <summary>
        /// The B spacing of the character.  The B spacing is the width of the drawn portion of the character glyph.
        /// </summary>
        public float abcfB;
        /// <summary>
        /// The C spacing of the character.  The C spacing is the distance to add to the current position to provide white space to the right of the character glyph.
        /// </summary>
        public float abcfC;
    }


    /// <summary>
    /// This class wraps the functionality of the wglUseFontBitmaps function to
    /// allow straightforward rendering of text.
    /// </summary>
    public class FontBitmaps
    {
        private FontBitmapEntry CreateFontBitmapEntry(OpenGL gl, string faceName, int height)
        {
            //  Make the OpenGL instance current.
            gl.MakeCurrent();

            //  Create the font based on the face name.
            var hFont = Win32.CreateFont(height, 0, 0, 0, Win32.FW_DONTCARE, 0, 0, 0, Win32.DEFAULT_CHARSET, 
                Win32.OUT_OUTLINE_PRECIS, Win32.CLIP_DEFAULT_PRECIS, Win32.CLEARTYPE_QUALITY, Win32.VARIABLE_PITCH, faceName);
            
            //  Select the font handle.
            var hOldObject = Win32.SelectObject(gl.RenderContextProvider.DeviceContextHandle, hFont);

            //  Create the list base.
            var listBase = gl.GenLists(1);

            //  JWH - requires dll import addition for wglUseFontBitmaps in Win32.cs
            //  Create the font bitmaps.
            bool result = Win32.wglUseFontBitmaps(gl.RenderContextProvider.DeviceContextHandle, 0, 255, listBase);

            //  Reselect the old font.
            Win32.SelectObject(gl.RenderContextProvider.DeviceContextHandle, hOldObject);

            //  Free the font.
            Win32.DeleteObject(hFont);

            //  Create the font bitmap entry.
            var fbe = new FontBitmapEntry()
            {
                HDC = gl.RenderContextProvider.DeviceContextHandle,
                HRC = gl.RenderContextProvider.RenderContextHandle,
                FaceName = faceName,
                Height = height,
                ListBase = listBase,
                ListCount = 255
            };

            //  Add the font bitmap entry to the internal list.
            fontBitmapEntries.Add(fbe);

            return fbe;
        }


        long totalDrawTextTicks = 0;
        int totalDrawTestCalls = 0;
        double baselineTicksPerCall = 0;

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="r">The r.</param>
        /// <param name="g">The g.</param>
        /// <param name="b">The b.</param>
        /// <param name="faceName">Name of the face.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="text">The text.</param>
        /// <param name="resWidth">Horizontal resolution.</param>
        /// <param name="resHeight">Vertical resolution.</param>
        public void DrawText(OpenGL gl, int x, int y, float r, float g, float b, string faceName, float fontSize, string text,
            double resWidth, double resHeight)
        {
            DrawText(gl, x, y, r, g, b, faceName, fontSize, text, resWidth, resHeight, float.NaN);
        }

        /// <summary>
        /// Draws the text.  This call includes a <paramref name="depth"/> parameter for relative
        /// z-level text stacking between -1 and +1
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="r">The r.</param>
        /// <param name="g">The g.</param>
        /// <param name="b">The b.</param>
        /// <param name="faceName">Name of the face.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="text">The text.</param>
        /// <param name="resWidth">Horizontal resolution.</param>
        /// <param name="resHeight">Vertical resolution.</param>
        /// <param name="depth">Relative z depth (-1 to +1) for depth testing.</param>
        public void DrawText(OpenGL gl, int x, int y, float r, float g, float b, string faceName, float fontSize, string text,
            double resWidth, double resHeight, float depth)
        {
            // ftlPhysicsGuy:
            // Check for resolutions that are NaN and default to RenderContextProvider resolutions
            if (double.IsNaN(resWidth))
                resWidth = gl.RenderContextProvider.Width;
            if (double.IsNaN(resHeight))
                resHeight = gl.RenderContextProvider.Height;

            //  Get the font size in pixels.
            var fontHeight = (int)(fontSize * (16.0f / 12.0f));

            //  Do we have a font bitmap entry for this OpenGL instance and face name?
            //  ftlPhysicsGuy - This avoids using Linq and was about 7 times faster in testing
            FontBitmapEntry fontBitmapEntry = fontBitmapEntries.Find((font) =>
            {
                return font.Height == fontHeight &&
                font.HDC == gl.RenderContextProvider.DeviceContextHandle &&
                font.HRC == gl.RenderContextProvider.RenderContextHandle &&
                string.Equals(font.FaceName, faceName, StringComparison.OrdinalIgnoreCase);
            });
            //var result = (from fbe in fontBitmapEntries
            //                where fbe.HDC == gl.RenderContextProvider.DeviceContextHandle
            //                && fbe.HRC == gl.RenderContextProvider.RenderContextHandle
            //                && String.Compare(fbe.FaceName, faceName, StringComparison.OrdinalIgnoreCase) == 0
            //                && fbe.Height == fontHeight
            //                select fbe).ToList();
            ////  Get the FBE or null.
            //FontBitmapEntry fontBitmapEntry = result.FirstOrDefault();

            //  If we don't have the FBE, we must create it.
            if (fontBitmapEntry == null)
                fontBitmapEntry = CreateFontBitmapEntry(gl, faceName, fontHeight);

            //  Create the appropriate projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PushMatrix();
            gl.LoadIdentity();
            
            gl.Ortho(0, resWidth, 0, resHeight, -1, 1);

            //  Create the appropriate modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.PushMatrix();
            gl.LoadIdentity();
            gl.Color(r, g, b);
            gl.RasterPos(x, y);

            gl.PushAttrib(OpenGL.GL_LIST_BIT | OpenGL.GL_CURRENT_BIT |
                OpenGL.GL_ENABLE_BIT | OpenGL.GL_TRANSFORM_BIT);
            gl.Color(r, g, b);
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_TEXTURE_2D);

            //  ftlPhysicsGuy
            //  If using depth, do not disable GL_DEPTH_TEST below -- will stack text via depth
            if (float.IsNaN(depth) == false)
                gl.Translate(0, 0, depth);
            else
                gl.Disable(OpenGL.GL_DEPTH_TEST);
            gl.RasterPos(x, y);

            //  Set the list base.
            gl.ListBase(fontBitmapEntry.ListBase);

            //  Create an array of lists for the glyphs.
            //  ftlPhysicsGuy - This avoids using Linq and was about 5 times faster in testing
            var lists = new byte[text.Length];
            int iByte = 0;
            foreach (char c in text)
                lists[iByte++] = (byte)c;
            //var lists = text.Select(c => (byte)c).ToArray();

            // Text starting out of frame (in the negative direction) could still come into frame
            if (x <= 0 || y <= 0)
            {
                // If the text comes into frame, draw it from that point
                if (y + fontHeight > 0 && x + fontBitmapEntry.GetTextWidth(text) > 0)
                {
                    gl.RasterPos(0, 0);
                    gl.Bitmap(0, 0, 0, 0, x, y, lists);
                }
            }

            //  Call the lists for the string.
            gl.CallLists(lists.Length, lists);
            // ftlPhysicsGuy -- No real need to flush and it significantly slows down this method!
            //gl.Flush();
            
            //  Reset the list bit.
            gl.PopAttrib();

            //  Pop the modelview.
            gl.PopMatrix();

            //  back to the projection and pop it, then back to the model view.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.PopMatrix();
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

        }

        /// <summary>
        /// Cache of font bitmap enties.
        /// </summary>
        private readonly List<FontBitmapEntry> fontBitmapEntries = new List<FontBitmapEntry>();
    }
}
