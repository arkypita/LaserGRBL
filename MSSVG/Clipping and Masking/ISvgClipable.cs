using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// Defines the methods and properties that an <see cref="SvgElement"/> must implement to support clipping.
    /// </summary>
    public interface ISvgClipable
    {
        /// <summary>
        /// Gets or sets the ID of the associated <see cref="SvgClipPath"/> if one has been specified.
        /// </summary>
        Uri ClipPath { get; set; }
        /// <summary>
        /// Specifies the rule used to define the clipping region when the element is within a <see cref="SvgClipPath"/>.
        /// </summary>
        SvgClipRule ClipRule { get; set; }
        /// <summary>
        /// Sets the clipping region of the specified <see cref="ISvgRenderer"/>.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to have its clipping region set.</param>
        void SetClip(ISvgRenderer renderer);
        /// <summary>
        /// Resets the clipping region of the specified <see cref="ISvgRenderer"/> back to where it was before the <see cref="SetClip"/> method was called.
        /// </summary>
        /// <param name="renderer">The <see cref="ISvgRenderer"/> to have its clipping region reset.</param>
        void ResetClip(ISvgRenderer renderer);
    }
}