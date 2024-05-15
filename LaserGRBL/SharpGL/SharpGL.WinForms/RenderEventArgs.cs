using System.Drawing;

namespace SharpGL
{
    /// <summary>
    /// The RenderEventArgs - arguments used for render envets.
    /// </summary>
    public class RenderEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderEventArgs"/> class.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        public RenderEventArgs(Graphics graphics)
        {
            Graphics = graphics;
        }

        /// <summary>
        /// Gets the graphics.
        /// </summary>
        public Graphics Graphics { get; private set; }
    }
}
