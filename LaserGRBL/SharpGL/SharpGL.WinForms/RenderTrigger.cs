namespace SharpGL
{
    /// <summary>
    /// The RenderingMode - specifies how and when rendering 
    /// will occur in an OpenGL control.
    /// </summary>
    public enum RenderTrigger
    {
        /// <summary>
        /// The Default RenderingMode, TimerBased. This mode means that
        /// a timer will be set up based on the FPS property of the control.
        /// </summary>
        TimerBased,

        /// <summary>
        /// Rendering must be performed manually by using the DoRender function.
        /// </summary>
        Manual,
    }
}
