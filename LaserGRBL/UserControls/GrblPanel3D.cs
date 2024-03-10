using LaserGRBL.Obj3D;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph.Core;
using SharpGL.Version;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace LaserGRBL.UserControls
{
    [ToolboxBitmap(typeof(SceneControl), "GrblScene")]
    public partial class GrblPanel3D : UserControl, IGrblPanel
    {
        // on zoom event
        public event Action<GrblPanel3D> OnZoom;
        // constants
        private const float MAX_Z = 1000;
        private const float MIN_Z = 1.2f;
        // last mouse position
        private Point? mLastMousePos = null;
        private Point? mCurrentMousePos
        {
            set
            {
                if (value != null)
                {
                    Point pos = (Point)value;
                    mMouseWorldPosition = new PointF((float)(pos.X / mRatio + Camera.Left),
                                                     -(float)(pos.Y / mRatio - Camera.Bottom - (-Camera.Bottom + Camera.Top)));

                }
                else
                {
                    mMouseWorldPosition = null;
                }
            }
        }
        // current mouse world position
        private PointF? mMouseWorldPosition {  get; set; }
        // compute width ratio
        private double mRatio => Width / (Camera.Right - Camera.Left);
        // origins shift
        private Vertex mShift = new Vertex(0, 0, 10f);
        // main camera
        public OrthographicCamera Camera { get; } = new OrthographicCamera();
        public GLColor BackgroundColor { get; set; } = Color.White;
        public GLColor TicksColor { get; set; } = Color.FromArgb(220, 220, 220);
        public GLColor MinorsColor { get; set; } = Color.FromArgb(242, 242, 242);
        public GLColor OriginsColor { get; set; } = Color.FromArgb(50, 50, 50);
        public GLColor PointerColor { get; set; } = Color.FromArgb(200, 40, 40);
        public GLColor TextColor { get; set; } = Color.FromArgb(0, 0, 0);
        public Font TextFont { get; set; } = new Font("Arial", 12);
        // background 
        private Grid3D mGrid = null;
        // grbl object 
        private Grbl3D mGrbl3D = null;
        // loaded grbl object
        private Grbl3D mGrbl3DLoaded = null;
        // viewport padding
        private Padding mPadding = new Padding(50, 0, 0, 30);
        // grbl core
        private GrblCore Core;
        public float PointerSize { get; set; } = 3;
        // drawing thread
        private Tools.ThreadObject mThreadDraw;
        // generated 3d bitmap
        private Bitmap mBmp = null;
        // critical section
        private object mBitmapLock = new object();
        // open gl object
        private OpenGL OpenGL;

        public GrblPanel3D()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            MouseWheel += GrblSceneControl_MouseWheel;
            MouseDown += GrblSceneControl_MouseDown;
            MouseMove += GrblSceneControl_MouseMove;
            MouseUp += GrblSceneControl_MouseUp;
            MouseLeave += GrblSceneControl_MouseLeave;
            Disposed += GrblPanel3D_Disposed;
            Camera.Position = new Vertex(0, 0, 0);
            Camera.Near = 0;
            Camera.Far = 100000000;
            mGrid = new Grid3D();
            mThreadDraw = new Tools.ThreadObject(DrawScene, 40, true, "Drawing Thread", InitializeOpenGL, ThreadPriority.BelowNormal);
            mThreadDraw.Start();
        }

        protected void InitializeOpenGL()
        {
            object parameter = null;
            OpenGL = new OpenGL();
            OpenGL.Create(OpenGLVersion.OpenGL2_1, RenderContextType.DIBSection, Width, Height, 32, parameter);
            OpenGL.ShadeModel(OpenGL.GL_SMOOTH);
            OpenGL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            OpenGL.ClearDepth(1.0f);
            OpenGL.Enable(OpenGL.GL_DEPTH_TEST);
            OpenGL.DepthFunc(OpenGL.GL_LEQUAL);
            OpenGL.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);
        }

        private void GrblPanel3D_Disposed(object sender, EventArgs e)
        {
            DisposeGrbl3D();
            mThreadDraw?.Dispose();
        }

        private void SetViewport()
        {
            double ratio = mRatio;
            // set viewport
            Camera.Left = Width / -mShift.Z + mShift.X - ((mPadding.Left - mPadding.Right) / 2 / ratio);
            Camera.Right = Width / mShift.Z + mShift.X - ((mPadding.Left - mPadding.Right) / 2 / ratio);
            Camera.Top = Height / mShift.Z + mShift.Y - ((mPadding.Bottom - mPadding.Top) / 2 / ratio);
            Camera.Bottom = Height / -mShift.Z + mShift.Y - ((mPadding.Bottom - mPadding.Top) / 2 / ratio);
        }

        private void DrawPointer()
        {
            if (Core == null) return;
            // draw laser cross
            GLColor color = new GLColor();
            color.FromColor(PointerColor);
            OpenGL.Color(color);
            OpenGL.LineWidth(PointerSize);
            OpenGL.Begin(OpenGL.GL_LINES);
            Vertex pointerPos = new Vertex(Core.MachinePosition.X, Core.MachinePosition.Y, 0.2f);
            OpenGL.Vertex(pointerPos.X - 2, pointerPos.Y, pointerPos.Z);
            OpenGL.Vertex(pointerPos.X + 2, pointerPos.Y, pointerPos.Z);
            OpenGL.End();
            OpenGL.Begin(OpenGL.GL_LINES);
            OpenGL.Vertex(pointerPos.X, pointerPos.Y - 2, pointerPos.Z);
            OpenGL.Vertex(pointerPos.X, pointerPos.Y + 2, pointerPos.Z);
            OpenGL.End();
            OpenGL.Flush();
        }

        public void DrawRulers()
        {
            // get ratio
            double ratio = mRatio; 
            // clear left ruler background
            OpenGL.Enable(OpenGL.GL_SCISSOR_TEST);
            OpenGL.Scissor(0, 0, mPadding.Left, Height);
            OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
            OpenGL.Disable(OpenGL.GL_SCISSOR_TEST);
            // clear bottom ruler background
            OpenGL.Enable(OpenGL.GL_SCISSOR_TEST);
            OpenGL.Scissor(0, 0, Width, mPadding.Bottom);
            OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
            OpenGL.Disable(OpenGL.GL_SCISSOR_TEST);
            // clear right ruler background
            OpenGL.Enable(OpenGL.GL_SCISSOR_TEST);
            OpenGL.Scissor(Width - mPadding.Right, 0, mPadding.Right, Height);
            OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
            OpenGL.Disable(OpenGL.GL_SCISSOR_TEST);
            // clear top ruler background
            OpenGL.Enable(OpenGL.GL_SCISSOR_TEST);
            OpenGL.Scissor(0, Height - mPadding.Top, Width, mPadding.Top);
            OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
            OpenGL.Disable(OpenGL.GL_SCISSOR_TEST);
            // define rulers step
            int step;
            if (mShift.Z > 10)
            {
                step = 5;
            }
            else if (mShift.Z > 4.5)
            {
                step = 10;
            }
            else if (mShift.Z > 3)
            {
                step = 20;
            }
            else if (mShift.Z > 1.5)
            {
                step = 50;
            }
            else
            {
                step = 100;
            }
            // draw horizontal
            for (int i = (int)Camera.Left + (int)(mPadding.Left / ratio); i < (int)Camera.Right - (int)(mPadding.Right / ratio); i += 1)
            {
                if (i % step == 0)
                {
                    string text = $"{i}";
                    Size size = TextRenderer.MeasureText(text, TextFont);
                    SizeF sizeProp = new SizeF(size.Width * 0.8f, size.Height * 0.8f);
                    double x = i * ratio - Camera.Left * ratio - sizeProp.Width / 4f;
                    OpenGL.DrawText((int)x, mPadding.Bottom - size.Height, TextColor.R, TextColor.G, TextColor.B, TextFont.FontFamily.Name, TextFont.Size, text);
                }
            }
            // draw vertical
            for (int i = (int)Camera.Bottom + (int)(mPadding.Bottom / ratio); i < (int)Camera.Top - (int)(mPadding.Top / ratio); i += 1)
            {
                if (i % step == 0)
                {
                    string text = $"{i}";
                    Size size = TextRenderer.MeasureText(text, TextFont);
                    SizeF sizeProp = new SizeF(size.Width * 0.8f, size.Height * 0.8f);
                    double x = mPadding.Left - sizeProp.Width;
                    double y = i * ratio - Camera.Bottom * ratio - sizeProp.Height / 4f;
                    OpenGL.DrawText((int)x, (int)y, TextColor.R, TextColor.G, TextColor.B, TextFont.FontFamily.Name, TextFont.Size, text);
                }
            }
            OpenGL.Flush();
        }

        private void DrawMouseCoord()
        {
            // draw current mouse position
            if (mMouseWorldPosition != null)
            {
                PointF pos = (PointF)mMouseWorldPosition;
                string mousePos = $"{pos.X:0.0} x {pos.Y:0.0}";
                Size size = TextRenderer.MeasureText(mousePos, TextFont);
                SizeF sizeProp = new SizeF(size.Width * 0.8f, size.Height * 0.8f);
                int xText = (int)(Width - sizeProp.Width);
                int yText = Height - 20;
                // clear background
                OpenGL.Enable(OpenGL.GL_SCISSOR_TEST);
                OpenGL.Scissor(xText - 10, yText, (int)sizeProp.Width + 10, 20);
                OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
                OpenGL.Disable(OpenGL.GL_SCISSOR_TEST);
                OpenGL.DrawText(xText, yText + 5, TextColor.R, TextColor.G, TextColor.B, TextFont.FontFamily.Name, TextFont.Size, mousePos);
                OpenGL.Flush();
            }
        }

        private void DrawScene()
        {
            Stopwatch sw = Stopwatch.StartNew();
            OpenGL.MakeCurrent();
            OpenGL.SetDimensions(Width, Height);
            OpenGL.Viewport(0, 0, Width, Height);
            Camera.Project(OpenGL);
            OpenGL.ClearColor(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, BackgroundColor.A);
            OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
            // render grid
            mGrid.ShowMinor = mShift.Z > 10;
            mGrid.TicksColor = TicksColor;
            mGrid.MinorsColor = MinorsColor;
            mGrid.OriginsColor = OriginsColor;
            mGrid.Render(OpenGL, RenderMode.Design);
            // enable anti alias
            OpenGL.Enable(OpenGL.GL_BLEND);
            OpenGL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            OpenGL.Enable(OpenGL.GL_LINE_SMOOTH);
            OpenGL.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);
            // manage grbl object
            if (mGrbl3DLoaded != null)
            {
                if (mGrbl3DLoaded != mGrbl3D)
                {
                    mGrbl3D?.Dispose();
                    mGrbl3D = mGrbl3DLoaded;
                }
                mGrbl3D.Invalidate();
                mGrbl3D.Render(OpenGL, RenderMode.Design);
            }
            // disable anti alias
            OpenGL.Disable(OpenGL.GL_BLEND);
            OpenGL.Disable(OpenGL.GL_LINE_SMOOTH);
            // main hud
            SetViewport();
            DrawPointer();
            DrawRulers();
            DrawMouseCoord();
            OpenGL.Flush();
            // enter lock and copy bitmap
            lock (mBitmapLock)
            {
                // new bitmap if different size
                if (mBmp == null ||
                    mBmp.Width != Width ||
                    mBmp.Height != Height)
                {
                    mBmp?.Dispose();
                    mBmp = new Bitmap(Width, Height);
                }
                // clone opengl graphics
                using (Graphics g = Graphics.FromImage(mBmp))
                {
                    IntPtr handleDeviceContext = g.GetHdc();
                    OpenGL.Blit(handleDeviceContext);
                    g.ReleaseHdc(handleDeviceContext);
                }
            }
            // call control invalidate
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // exit if not defined bitmap
            if (mBmp == null) return;
            // enter lock and copy bitmap to control
            lock (mBitmapLock)
            {
                e.Graphics.DrawImage(mBmp, new Point(0, 0));
            }
        }

        private void GrblSceneControl_MouseLeave(object sender, EventArgs e)
        {
            mLastMousePos = null;
            mCurrentMousePos = null;
        }

        private void GrblSceneControl_MouseUp(object sender, MouseEventArgs e)
        {
            mLastMousePos = null;
            mCurrentMousePos = null;
        }

        private void GrblSceneControl_MouseDown(object sender, MouseEventArgs e) => mLastMousePos = e.Location;

        private void GrblSceneControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (mLastMousePos != null)
            {
                mShift.X -= (e.Location.X - (int)mLastMousePos?.X) * 2f / mShift.Z;
                mShift.Y -= (e.Location.Y - (int)mLastMousePos?.Y) * 2f / -mShift.Z;
                mLastMousePos = e.Location;
            }
            mCurrentMousePos = e.Location;
        }

        private void GrblSceneControl_MouseWheel(object sender, MouseEventArgs e)
        {
            mShift.Z += e.Delta / 1000f * mShift.Z;
            if (mShift.Z > MAX_Z) mShift.Z = MAX_Z;
            if (mShift.Z < MIN_Z) mShift.Z = MIN_Z;
        }

        public void SetComProgram(GrblCore core)
        {
            Core = core;
            Core.OnFileLoading += OnFileLoading;
            Core.OnFileLoaded += OnFileLoaded;
        }

        private void DisposeGrbl3D()
        {
            mGrbl3DLoaded = null;
        }

        private void OnFileLoaded(long elapsed, string filename)
        {
            DisposeGrbl3D();
            mGrbl3DLoaded = new Grbl3D(Core.LoadedFile, "Grbl file", false);
            AutoSizeDrawing();
        }

        public void AutoSizeDrawing()
        {
            if (Core.LoadedFile.Range.DrawingRange.ValidRange)
            {
                mShift.X = (float)Core.LoadedFile.Range.DrawingRange.Center.X;
                mShift.Y = (float)Core.LoadedFile.Range.DrawingRange.Center.Y;
                double ratio = mRatio;
                double currentWidth = Camera.Right - Camera.Left - (mPadding.Left / ratio) - (mPadding.Right / ratio);
                double currentHeight = Camera.Top - Camera.Bottom - (mPadding.Bottom / ratio) - (mPadding.Top / ratio);
                double xFactor = (double)Core.LoadedFile.Range.DrawingRange.Width / currentWidth;
                double yFactor = (double)Core.LoadedFile.Range.DrawingRange.Height / currentHeight;
                mShift.Z = (float)(mShift.Z / Math.Max(xFactor, yFactor));
            }
        }

        private void OnFileLoading(long elapsed, string filename)
        {
        }

        public void TimerUpdate()
        {
        }

        public void OnColorChange()
        {
        }

    }

}
