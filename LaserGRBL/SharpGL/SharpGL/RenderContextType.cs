using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL
{
	/// <summary>
    /// The render context type.
    /// </summary>
    public enum RenderContextType
    {
        /// <summary>
        /// A DIB section - offscreen but NEVER hardware accelerated.
        /// </summary>
        DIBSection,

        /// <summary>
        /// A Native Window - directly render to a window, the window handle
        /// must be passed as the parameter to Create. Hardware acceleration
        /// is supported but one can never do GDI drawing on top of the 
        /// OpenGL drawing.
        /// </summary>
        NativeWindow,

        /// <summary>
        /// A Hidden Window - more initial overhead but acceleratable.
        /// </summary>
        HiddenWindow,

		/// <summary>
		///	A Framebuffer Object - accelerated but may not be supported on some cards.
		/// </summary>
		FBO
	};
}
