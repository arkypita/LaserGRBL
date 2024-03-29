using LaserGRBL.Obj3D;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph.Core;
using SharpGL.Version;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaserGRBL.UserControls
{
    [ToolboxBitmap(typeof(SceneControl), "GrblScene")]
    public partial class GrblPanel3D : UserControl, IGrblPanel
    {
        // on zoom event
        public event Action<GrblPanel3D> OnZoom;
        // last mouse position
        private Point? mLastMousePos = null;
        // current mouse world position
        private PointF? mMouseWorldPosition { get; set; }
        // main camera
        private OrthographicCamera mCamera { get; } = new OrthographicCamera();
        private GLColor mBackgroundColor { get; set; }
        private GLColor mTicksColor { get; set; }
        private GLColor mMinorsColor { get; set; }
        private GLColor mOriginsColor { get; set; }
        private GLColor mPointerColor { get; set; }
        private GLColor mTextColor { get; set; }
        private Font mTextFont { get; set; } = new Font("Arial", 12);
        // background
        private Grid3D mGrid = null;
        // grbl object
        private Grbl3D mGrbl3D = null;
        private Grbl3D mGrbl3DOff = null;
        // reload request
        private bool mReload = false;
        // invalidate all request
        private bool mInvalidateAll = false;
        // viewport padding
        private Padding mPadding = new Padding(50, 0, 0, 30);
        // grbl core
        private GrblCore Core;
        public float PointerSize { get; set; } = 3;
        // last control size
        private PointF mLastControlSize;
        // drawing thread
        private Tools.ThreadObject mThreadDraw;
        // generated 3d bitmap
        private Bitmap mBmp = null;
        // critical section
        private object mBitmapLock = new object();
        // open gl object
        private OpenGL OpenGL;
        // rulers steps
        private List<KeyValuePair<double, int>> mRulerSteps = new List<KeyValuePair<double, int>> {
            new KeyValuePair<double, int>( 100,   5),
            new KeyValuePair<double, int>( 200,  10),
            new KeyValuePair<double, int>( 600,  30),
            new KeyValuePair<double, int>(1000,  50),
            new KeyValuePair<double, int>(2000, 100)
        };

        public GrblPanel3D()
        {
            InitializeComponent();
            MouseDoubleClick += GrblPanel3D_DoubleClick;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            MouseWheel += GrblSceneControl_MouseWheel;
            MouseDown += GrblSceneControl_MouseDown;
            MouseMove += GrblSceneControl_MouseMove;
            MouseUp += GrblSceneControl_MouseUp;
            MouseLeave += GrblSceneControl_MouseLeave;
            Resize += GrblPanel3D_Resize;
            Disposed += GrblPanel3D_Disposed;
            mCamera.Position = new Vertex(0, 0, 0);
            mCamera.Near = 0;
            mCamera.Far = 100000000;
            AutoSizeDrawing();
            mLastControlSize = new PointF(Width, Height);
            mGrid = new Grid3D();
            Task.Factory.StartNew(() => {
                InitializeOpenGL();
                while (true)
                {
                    DrawScene();
                    Thread.Sleep(30);
                }
            });

        }

        private void SetWorldPosition(double left, double right, double bottom, double top)
        {
            double max = 2000;
            double width = right - left;
            double height = top - bottom;
            if (width > max * 2 || height > max * 2) return;
            if (left < -max)
            {
                left = -max;
                right = left + width;
            } 
            if (right > max)
            {
                right = max;
                left = max - width;
            }
            if (bottom < -max)
            {
                bottom = -max;
                top = bottom + height;
            }
            if (top > max)
            {
                top = max;
                bottom = top - height;
            }
            mCamera.Left = left;
            mCamera.Right = right;
            mCamera.Bottom = bottom;
            mCamera.Top = top;
        }

        private void GrblPanel3D_Resize(object sender, EventArgs e)
        {
            double wRatio = Width / mLastControlSize.X;
            double hRatio = Height / mLastControlSize.Y;
            SetWorldPosition(mCamera.Left * wRatio, mCamera.Right * wRatio, mCamera.Bottom * hRatio, mCamera.Top * hRatio);
            mLastControlSize = new PointF(Width, Height);
        }

        private void GrblPanel3D_DoubleClick(object sender, MouseEventArgs e)
        {
            if (Settings.GetObject("Click N Jog", true) && mMouseWorldPosition != null)
            {
                Core.BeginJog((PointF)mMouseWorldPosition, e.Button == MouseButtons.Right);
            }
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

        private void DrawPointer()
        {
            if (Core == null) return;
            // draw laser cross
            GLColor color = new GLColor();
            color.FromColor(mPointerColor);
            OpenGL.Color(color);
            OpenGL.LineWidth(PointerSize);
            OpenGL.Begin(OpenGL.GL_LINES);
            Vertex pointerPos = new Vertex(Core.WorkPosition.X, Core.WorkPosition.Y, 0.2f);
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
            // get world width
            double worldWidth = mCamera.Right - mCamera.Left;
            // init rulers step
            int step = 200;
            // get step from list
            foreach (KeyValuePair<double, int> rulerStep in mRulerSteps)
            {
                if (worldWidth < rulerStep.Key)
                {
                    step = rulerStep.Value;
                    break;
                }
            }
            // get ratio
            double wRatio = Width / (mCamera.Right - mCamera.Left);
            // draw horizontal
            for (int i = (int)mCamera.Left + (int)(mPadding.Left / wRatio); i < (int)mCamera.Right - (int)(mPadding.Right / wRatio); i += 1)
            {
                if (i % step == 0)
                {
                    string text = $"{i}";
                    Size size = TextRenderer.MeasureText(text, mTextFont);
                    SizeF sizeProp = new SizeF(size.Width * 0.8f, size.Height * 0.8f);
                    double x = i * wRatio - mCamera.Left * wRatio - sizeProp.Width / 4f;
                    OpenGL.DrawText((int)x, mPadding.Bottom - size.Height, mTextColor.R, mTextColor.G, mTextColor.B, mTextFont.FontFamily.Name, mTextFont.Size, text);
                }
            }
            // get ratio
            double hRatio = Height / (mCamera.Top - mCamera.Bottom);
            // draw vertical
            for (int i = (int)mCamera.Bottom + (int)(mPadding.Bottom / hRatio); i < (int)mCamera.Top - (int)(mPadding.Top / hRatio); i += 1)
            {
                if (i % step == 0)
                {
                    string text = $"{i}";
                    Size size = TextRenderer.MeasureText(text, mTextFont);
                    SizeF sizeProp = new SizeF(size.Width * 0.8f, size.Height * 0.8f);
                    double x = mPadding.Left - sizeProp.Width;
                    double y = i * hRatio - mCamera.Bottom * hRatio - sizeProp.Height / 4f;
                    OpenGL.DrawText((int)x, (int)y, mTextColor.R, mTextColor.G, mTextColor.B, mTextFont.FontFamily.Name, mTextFont.Size, text);
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
                Size size = TextRenderer.MeasureText(mousePos, mTextFont);
                SizeF sizeProp = new SizeF(size.Width * 0.8f, size.Height * 0.8f);
                int xText = (int)(Width - sizeProp.Width);
                int yText = Height - 20;
                // clear background
                OpenGL.Enable(OpenGL.GL_SCISSOR_TEST);
                OpenGL.Scissor(xText - 10, yText, (int)sizeProp.Width + 10, 20);
                OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
                OpenGL.Disable(OpenGL.GL_SCISSOR_TEST);
                OpenGL.DrawText(xText, yText + 5, mTextColor.R, mTextColor.G, mTextColor.B, mTextFont.FontFamily.Name, mTextFont.Size, mousePos);
                OpenGL.Flush();
            }
        }

        private void DrawScene()
        {
            Stopwatch sw = Stopwatch.StartNew();
            if (mBackgroundColor == null) return;
            OpenGL.MakeCurrent();
            OpenGL.SetDimensions(Width, Height);
            OpenGL.Viewport(0, 0, Width, Height);
            mCamera.Project(OpenGL);
            OpenGL.ClearColor(mBackgroundColor.R, mBackgroundColor.G, mBackgroundColor.B, mBackgroundColor.A);
            OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
            // render grid
            mGrid.ShowMinor = mCamera.Right - mCamera.Left < 100;
            mGrid.TicksColor = mTicksColor;
            mGrid.MinorsColor = mMinorsColor;
            mGrid.OriginsColor = mOriginsColor;
            mGrid.Render(OpenGL, RenderMode.Design);
            // enable anti alias
            OpenGL.Enable(OpenGL.GL_BLEND);
            OpenGL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            OpenGL.Enable(OpenGL.GL_LINE_SMOOTH);
            OpenGL.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);
            // manage grbl object
            if (mReload)
            {
                mReload = false;
                mGrbl3D?.Dispose();
                mGrbl3DOff?.Dispose();
                mGrbl3D = new Grbl3D(Core, "LaserOn", false, ColorScheme.PreviewLaserPower);
                mGrbl3DOff = new Grbl3D(Core, "LaserOff", true, ColorScheme.PreviewOtherMovement);
                mGrbl3DOff.LineWidth = 1;
            }
            if (mGrbl3D != null)
            {
                if (mInvalidateAll)
                {
                    mInvalidateAll = false;
                    mGrbl3D.Color = ColorScheme.PreviewLaserPower;
                    mGrbl3D.InvalidateAll();
                    mGrbl3DOff.Color = ColorScheme.PreviewOtherMovement;
                    mGrbl3DOff.InvalidateAll();
                }
                if (Core.ShowLaserOffMovements.Value)
                {
                    mGrbl3DOff.Invalidate();
                    mGrbl3DOff.Render(OpenGL, RenderMode.Design);
                }
                mGrbl3D.Invalidate();
                mGrbl3D.Render(OpenGL, RenderMode.Design);
            }
            // disable anti alias
            OpenGL.Disable(OpenGL.GL_BLEND);
            OpenGL.Disable(OpenGL.GL_LINE_SMOOTH);
            // main hud
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
            mMouseWorldPosition = null;
        }

        private void GrblSceneControl_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            mLastMousePos = null;
            mMouseWorldPosition = null;
        }

        private void GrblSceneControl_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.SizeAll;
            mLastMousePos = e.Location;
        }

        private void GrblSceneControl_MouseMove(object sender, MouseEventArgs e)
        {
            double xp = e.X / (double)Width * (mCamera.Right - mCamera.Left) + mCamera.Left;
            double yp = (Height - e.Y) / (double)Height * (mCamera.Top - mCamera.Bottom) + mCamera.Bottom;
            mMouseWorldPosition = new PointF((float)xp, (float)yp);
            if (mLastMousePos != null && e.Button == MouseButtons.Left)
            {
                Point lastPos = (Point)mLastMousePos;
                double k = (mCamera.Right - mCamera.Left) / Width;
                double dx = e.X - lastPos.X;
                double dy = e.Y - lastPos.Y;
                SetWorldPosition(mCamera.Left - dx * k, mCamera.Right - dx * k, mCamera.Bottom + dy * k, mCamera.Top + dy * k);
                mLastMousePos = e.Location;
            }
        }

        private void GrblSceneControl_MouseWheel(object sender, MouseEventArgs e)
        {
            float k = e.Delta / 1000f;
            PointF mousePos = (PointF)mMouseWorldPosition;
            SetWorldPosition(
                mCamera.Left + (mousePos.X - mCamera.Left) * k,
                mCamera.Right - (mCamera.Right - mousePos.X) * k,
                mCamera.Bottom + (mousePos.Y - mCamera.Bottom) * k,
                mCamera.Top - (mCamera.Top - mousePos.Y) * k
            );
        }

        public void SetCore(GrblCore core)
        {
            Core = core;
            Core.OnFileLoaded += OnFileLoaded;
            Core.ShowExecutedCommands.OnChange += ShowExecutedCommands_OnChange;
            Core.PreviewLineSize.OnChange += PrerviewLineSize_OnChange;
        }

        private void PrerviewLineSize_OnChange(Tools.RetainedSetting<float> obj)
        {
            if (mGrbl3D != null) mGrbl3D.LineWidth = obj.Value;
            mInvalidateAll = true;
        }

        private void ShowExecutedCommands_OnChange(Tools.RetainedSetting<bool> obj) => mInvalidateAll = true;

        private void DisposeGrbl3D()
        {
            mReload = true;
        }

        private void OnFileLoaded(long elapsed, string filename)
        {
            DisposeGrbl3D();
            mReload = true;
            AutoSizeDrawing();
        }

        public void AutoSizeDrawing()
        {
            if (Core?.LoadedFile.Range.DrawingRange.ValidRange == true)
            {
                float ratio = (Width / (float)Height) / (float)(Core.LoadedFile.Range.DrawingRange.Width / Core.LoadedFile.Range.DrawingRange.Height);
                if (ratio > 1)
                {
                    float width = (float)Core.LoadedFile.Range.DrawingRange.Width * ratio;
                    float border = (float)Core.LoadedFile.Range.DrawingRange.Height * 0.1f;
                    SetWorldPosition(
                        Core.LoadedFile.Range.DrawingRange.Center.X - width / 2f - border * ratio,
                        Core.LoadedFile.Range.DrawingRange.Center.Y + width / 2f + border * ratio,
                        (float)Core.LoadedFile.Range.DrawingRange.Y.Min - border,
                        (float)Core.LoadedFile.Range.DrawingRange.Y.Max + border
                    );
                }
                else
                {
                    float height = ((float)Core.LoadedFile.Range.DrawingRange.Height) / ratio;
                    float border = (float)Core.LoadedFile.Range.DrawingRange.Width * 0.1f;
                    SetWorldPosition(
                        (float)Core.LoadedFile.Range.DrawingRange.X.Min - border,
                        (float)Core.LoadedFile.Range.DrawingRange.X.Max + border,
                        Core.LoadedFile.Range.DrawingRange.Center.Y - height / 2f - border * ratio,
                        Core.LoadedFile.Range.DrawingRange.Center.Y + height / 2f + border * ratio
                    );
                }
            }
            else
            {
                double ratio = (Height) / (float)Width;
                float limit = 200;
                SetWorldPosition(-limit, limit, -limit * ratio, limit * ratio);
            }
        }


        public void TimerUpdate()
        {
        }

        public void OnColorChange()
        {
            mBackgroundColor = ColorScheme.PreviewBackColor;
            mTextColor = ColorScheme.PreviewText;
            mOriginsColor = ColorScheme.PreviewRuler;
            mPointerColor = ColorScheme.PreviewCross;
            mTicksColor = ColorScheme.PreviewGrid;
            mMinorsColor = ColorScheme.PreviewGridMinor;
            mInvalidateAll = true;
        }

    }

}