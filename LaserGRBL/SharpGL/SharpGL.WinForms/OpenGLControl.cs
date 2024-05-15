using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using SharpGL.Version;
using SharpGL.WinForms.NETDesignSurface.Designers;

namespace SharpGL
{
   	/// <summary>
	/// This is the basic OpenGL control object, it gives all of the basic OpenGL functionality.
	/// </summary>
    [ToolboxBitmap(typeof(OpenGLControl), "SharpGL.png")]
    [Designer(typeof(OpenGLCtrlDesigner))]
    public partial class OpenGLControl : UserControl, ISupportInitialize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLControl"/> class.
        /// </summary>
        public OpenGLControl()
        {
            InitializeComponent();

            //  Set the user draw styles.
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //  Set up the drawing timer.
            SetupDrawingTimer();
        }

        /// <summary>
        /// Setups the drawing timer, based on the framerate settings.
        /// </summary>
        private void SetupDrawingTimer()
        {
            //  First, if the framerate is less than zero, set it to zero.
            if (frameRate < 0)
                frameRate = 0;

            //  Now, if the framerate is zero, we're going to disable the timer.
            if (frameRate == 0 && timerDrawing.Enabled)
            {
                //  Disable the timer - at this stage we're done.
                timerDrawing.Enabled = false;
                return;
            }

            //  Now set the interval.
            timerDrawing.Interval = (int)(1000.0 / FrameRate);

            //  Finally, if the timer is not enabled, enable it now.
            if(timerDrawing.Enabled == false)
                timerDrawing.Enabled = true;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (RenderContextType == RenderContextType.NativeWindow)
                return;
            base.OnPaintBackground(e);
        }

        /// <summary>
        /// Initialises OpenGL.
        /// </summary>
        protected void InitialiseOpenGL()
        {
            object parameter = null;
           
            //  Native render context providers need a little bit more attention.
            if(RenderContextType == RenderContextType.NativeWindow)
            {
                SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
                parameter = Handle;
            }

            //  Create the render context.
            gl.Create(OpenGLVersion, RenderContextType, Width, Height, 32, parameter);

            //  Set the most basic OpenGL styles.
            gl.ShadeModel(OpenGL.GL_SMOOTH);
            gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            gl.ClearDepth(1.0f);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.DepthFunc(OpenGL.GL_LEQUAL);
            gl.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);

            //  Fire the OpenGL initialized event.
            DoOpenGLInitialized();

            //  Set the draw timer.
            timerDrawing.Tick += timerDrawing_Tick;
        }

        /// <summary>
        /// Manually perform rendering.
        /// </summary>
        public void DoRender()
        {
            Render(CreateGraphics());
        }

        /// <summary>
        /// Renders to the specified graphics.
        /// </summary>
        /// <param name="graphics">The graphics to render to.</param>
        protected void Render(Graphics graphics)
        {
            //  Start the stopwatch so that we can time the rendering.
            stopwatch.Restart();

            //	Make sure it's our instance of openSharpGL that's active.
            OpenGL.MakeCurrent();

            //	If there is a draw handler, then call it.
            DoOpenGLDraw(new RenderEventArgs(graphics));

            //  Draw the FPS.
            if (DrawFPS)
            {
                OpenGL.DrawText(5, 5, 1.0f, 0.0f, 0.0f, "Courier New", 12.0f,
                    string.Format("Draw Time: {0:0.0000} ms ~ {1:0.0} FPS", frameTime, 1000.0 / frameTime));
                OpenGL.Flush();
            }

            //	Blit our offscreen bitmap.
            var handleDeviceContext = graphics.GetHdc();
            OpenGL.Blit(handleDeviceContext);
            graphics.ReleaseHdc(handleDeviceContext);

            //  Perform GDI drawing.
            if (OpenGL.RenderContextProvider != null && OpenGL.RenderContextProvider.GDIDrawingEnabled)
                DoGDIDraw(new RenderEventArgs(graphics));

            //  Stop the stopwatch.
            stopwatch.Stop();

            //  Store the frame time.
            frameTime = stopwatch.Elapsed.TotalMilliseconds;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //  Call the main rendering function.
            Render(e.Graphics);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            //  If we do not have a render context there is nothing more
            //  we can do.
            if (OpenGL.RenderContextProvider == null)
                return;

            //	Resize the DIB Surface.
            OpenGL.SetDimensions(Width, Height);

            //	Set the viewport.
            gl.Viewport(0, 0, Width, Height);

            //  If we have a project handler, call it...
            if (Width != -1 && Height != -1)
            {
                var handler = Resized;
                if (handler != null)
                    handler(this, e);
                else
                {                    
                    //  Otherwise we do our own projection.
                    gl.MatrixMode(OpenGL.GL_PROJECTION);
                    gl.LoadIdentity();

                    // Calculate The Aspect Ratio Of The Window
                    gl.Perspective(45.0f, (float)Width / (float)Height, 0.1f, 100.0f);

                    gl.MatrixMode(OpenGL.GL_MODELVIEW);
                    gl.LoadIdentity();
                }
            }

            Invalidate();
        }

        /// <summary>
        /// Calls the OpenGL initialized function.
        /// </summary>
        protected virtual void DoOpenGLInitialized()
        {
            OpenGLInitialized?.Invoke(this, null);
        }

        /// <summary>
        /// Call this function in derived classes to do the OpenGL Draw event.
        /// </summary>
        protected virtual void DoOpenGLDraw(RenderEventArgs e)
        {
            OpenGLDraw?.Invoke(this, e);
        }

        /// <summary>
        /// Call this function in derived classes to do the GDI Draw event.
        /// </summary>
        protected virtual void DoGDIDraw(RenderEventArgs e)
        {
            GDIDraw?.Invoke(this, e);
        }

        /// <summary>
        /// Handles the Tick event of the timerDrawing control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void timerDrawing_Tick(object sender, EventArgs e)
        {
            //  If we're in manual mode, we do not care about the timer.
            if(RenderTrigger == RenderTrigger.Manual)
                return;

            //  The timer for drawing simply invalidates the control at a regular interval.
            Invalidate();
        }

        /// <summary>
        /// Signals the object that initialization is starting.
        /// </summary>
        void ISupportInitialize.BeginInit()
        {
        }

        /// <summary>
        /// Signals the object that initialization is complete.
        /// </summary>
        void ISupportInitialize.EndInit()
        {
            InitialiseOpenGL();
            OnSizeChanged(null);
        }

        /// <summary>
        /// Occurs when OpenGL has been initialized.
        /// </summary>
        [Description("Called when OpenGL has been initialized occur."), Category("SharpGL")]
        public event EventHandler OpenGLInitialized;

        /// <summary>
        /// Occurs when OpenGL drawing should be performed.
        /// </summary>
        [Description("Called whenever OpenGL drawing can should occur."), Category("SharpGL")]
        public event RenderEventHandler OpenGLDraw;

        /// <summary>
        /// Occurs when GDI drawing should be performed.
        /// </summary>
        [Description("Called at the point in the render cycle when GDI drawing can occur."), Category("SharpGL")]
        public event RenderEventHandler GDIDraw;

        /// <summary>
        /// Occurs when the control is resized. Can be used to perform custom viewport projections.
        /// </summary>
        [Description("Called when the control is resized - you can use this to do custom viewport projections."), Category("SharpGL")]
        public event EventHandler Resized;

   	    /// <summary>
        /// The timer used for drawing the control.
        /// </summary>
        private readonly Timer timerDrawing = new Timer();

        /// <summary>
        /// The OpenGL object for the control.
        /// </summary>
        private readonly OpenGL gl = new OpenGL();

        /// <summary>
        /// A stopwatch used for timing rendering.
        /// </summary>
        protected Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// The last frame time in milliseconds.
        /// </summary>
        protected double frameTime;

        /// <summary>
        /// Gets the OpenGL object.
        /// </summary>
        /// <value>The OpenGL.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OpenGL OpenGL => gl;

        /// <summary>
        /// Gets or sets a value indicating whether to draw FPS information.
        /// </summary>
        /// <value>
        ///   <c>true</c> if FPS info should be drawn; otherwise, <c>false</c>.
        /// </value>
   	    [Description("Should the draw time be shown?"), Category("SharpGL")]
   	    public bool DrawFPS { get; set; }

   	    /// <summary>
        /// The framerate, in hertz.
        /// </summary>
        private int frameRate = 20;

        /// <summary>
        /// Gets or sets the frame rate, in Hertz.
        /// </summary>
        /// <value>
        /// The frame rate, in Hertz.
        /// </value>
        [Description("The rate at which the control should be re-drawn, in Hertz."), Category("SharpGL"), DefaultValue(20)]
        public int FrameRate
        {
            get { return frameRate; }
            set
            {
                frameRate = value;

                //  Update the drawing timer.
                SetupDrawingTimer();
            }
        }

        /// <summary>
        /// Gets or sets the type of the render context.
        /// </summary>
        /// <value>
        /// The type of the render context.
        /// </value>
        [Description("The render context type."), Category("SharpGL")]
        public RenderContextType RenderContextType { get; set; } = RenderContextType.DIBSection;

        /// <summary>
        /// Gets or sets the desired OpenGL version.
        /// </summary>
        /// <value>
        /// The desired OpenGL version.
        /// </value>
        [Description("The desired OpenGL version for the control."), Category("SharpGL")]
   	    public OpenGLVersion OpenGLVersion { get; set; } = OpenGLVersion.OpenGL2_1;

        /// <summary>
        /// Gets or sets the render trigger.
        /// </summary>
        /// <value>
        /// The render trigger.
        /// </value>
        [Description("The render trigger - determines when rendering will occur."), Category("SharpGL")]
        public RenderTrigger RenderTrigger { get; set; }
    }

    /// <summary>
    /// Delegate for a Render Event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="RenderEventArgs"/> instance containing the event data.</param>
    public delegate void RenderEventHandler(object sender, RenderEventArgs args);
}
