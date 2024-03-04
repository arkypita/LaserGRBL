using LaserGRBL.Obj3D;
using LaserGRBL.UserControls;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Cameras;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL.UserControls
{
    [ToolboxBitmap(typeof(SceneControl), "GrblScene")]
    public partial class GrblPanel3D : SceneControl, IGrblPanel
    {
        // on zoom event
        public event Action<GrblPanel3D> OnZoom;
        // constants
        private const float MAX_Z = 1000;
        private const float MIN_Z = 1.2f;
        // last mouse position
        private Point? mMousePos = null;
        // current mouse position
        private Point? mMousePosCurrent = null;
        // origins shift
        private Vertex mShift = new Vertex(0, 0, 10f);
        // main camera
        public OrthographicCamera Camera { get; } = new OrthographicCamera();
        public Color BackgroundColor { get; set; } = Color.White;
        public Color TicksColor { get; set; } = Color.FromArgb(200, 200, 200);
        public Color MinorsColor { get; set; } = Color.FromArgb(220, 220, 220);
        public Color OriginsColor { get; set; } = Color.FromArgb(50, 50, 50);
        public Color PointerColor { get; set; } = Color.FromArgb(200, 40, 40);
        public Color TextColor { get; set; } = Color.FromArgb(0, 0, 0);
        public Font TextFont { get; set; } = new Font("Arial", 12);
        // background 
        private Grid3D mGrid = null;
        // grbl object 
        private Grbl3D mGrbl3D = null;
        // grbl core
        private GrblCore Core;

        // pointer info
        public Vertex PointerPosition { get; set; } = new Vertex(0, 0, 0);
        public float PointerSize { get; set; } = 3;

        public GrblPanel3D()
        {
            InitializeComponent();
            MouseWheel += GrblSceneControl_MouseWheel;
            MouseDown += GrblSceneControl_MouseDown;
            MouseMove += GrblSceneControl_MouseMove;
            MouseUp += GrblSceneControl_MouseUp;
            MouseLeave += GrblSceneControl_MouseLeave;
            OpenGLDraw += GrblSceneControl_OpenGLDraw;
            Scene.SceneContainer.Children.Clear();
            Scene.CurrentCamera = Camera;
            Camera.Position = new Vertex(0, 0, 0);
            Camera.Near = 0;
            Camera.Far = 100000000;
            mGrid = new Grid3D();
            Scene.SceneContainer.AddChild(mGrid);
        }

        private void SetViewPort()
        {
            // set viewport
            Camera.Left = Width / -mShift.Z + mShift.X;
            Camera.Right = Width / mShift.Z + mShift.X;
            Camera.Top = Height / mShift.Z - mShift.Y;
            Camera.Bottom = Height / -mShift.Z - mShift.Y;
        }

        private void DrawPointer()
        {
            // draw laser cross
            GLColor color = new GLColor();
            color.FromColor(PointerColor);
            OpenGL.Color(color);
            OpenGL.LineWidth(PointerSize);
            OpenGL.Begin(OpenGL.GL_LINES);
            OpenGL.Vertex(PointerPosition.X - 2, PointerPosition.Y, PointerPosition.Z);
            OpenGL.Vertex(PointerPosition.X + 2, PointerPosition.Y, PointerPosition.Z);
            OpenGL.End();
            OpenGL.Begin(OpenGL.GL_LINES);
            OpenGL.Vertex(PointerPosition.X, PointerPosition.Y - 2, PointerPosition.Z);
            OpenGL.Vertex(PointerPosition.X, PointerPosition.Y + 2, PointerPosition.Z);
            OpenGL.End();
        }

        public void DrawRulers(double ratio)
        {
            // clear left ruler background
            OpenGL.Enable(OpenGL.GL_SCISSOR_TEST);
            OpenGL.Scissor(0, 0, 50, Height);
            OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
            OpenGL.Disable(OpenGL.GL_SCISSOR_TEST);
            // clear bottom ruler background
            OpenGL.Enable(OpenGL.GL_SCISSOR_TEST);
            OpenGL.Scissor(0, 0, Width, 20);
            OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
            OpenGL.Disable(OpenGL.GL_SCISSOR_TEST);
            // define if minors are visible
            mGrid.ShowMinor = mShift.Z > 10;
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
            for (int i = -mGrid.MaxWidth; i < mGrid.MaxHeight; i += 1)
            {
                if (i % step == 0)
                {
                    string text = $"{i}";
                    Size size = TextRenderer.MeasureText(text, TextFont);
                    SizeF sizeProp = new SizeF(size.Width * 0.8f, size.Height * 0.8f);
                    double x = i * ratio - Camera.Left * ratio - sizeProp.Width / 4f;
                    if (x > 50) OpenGL.DrawText((int)x, 5, TextColor.GlRed(), TextColor.GlGreen(), TextColor.GlBlue(), TextFont.FontFamily.Name, TextFont.Size, text);
                }
            }
            // draw vertical
            for (int i = -mGrid.MaxHeight; i < mGrid.MaxHeight; i += 1)
            {
                if (i % step == 0)
                {
                    string text = $"{i}";
                    Size size = TextRenderer.MeasureText(text, TextFont);
                    SizeF sizeProp = new SizeF(size.Width * 0.8f, size.Height * 0.8f);
                    double x = 50 - sizeProp.Width;
                    double y = i * ratio - Camera.Bottom * ratio - sizeProp.Height / 4f;
                    OpenGL.DrawText((int)x, (int)y, TextColor.GlRed(), TextColor.GlGreen(), TextColor.GlBlue(), TextFont.FontFamily.Name, TextFont.Size, text);
                }
            }
        }

        private void DrawMouseCoord(double ratio)
        {
            // draw current mouse position
            if (mMousePosCurrent != null)
            {
                // define text position
                double xMouse = ((Point)mMousePosCurrent).X / ratio + Camera.Left;
                double yMouse = -(((Point)mMousePosCurrent).Y / ratio + Camera.Bottom);
                string mousePos = $"{xMouse:0.0}x{yMouse:0.0}";
                Size size = TextRenderer.MeasureText(mousePos, TextFont);
                SizeF sizeProp = new SizeF(size.Width * 0.8f, size.Height * 0.8f);
                int xText = (int)(Width - sizeProp.Width);
                int yText = Height - 20;
                // clear background
                OpenGL.Enable(OpenGL.GL_SCISSOR_TEST);
                OpenGL.Scissor(xText, yText, (int)sizeProp.Width, 20);
                OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
                OpenGL.Disable(OpenGL.GL_SCISSOR_TEST);
                OpenGL.DrawText(xText, yText + 5, TextColor.GlRed(), TextColor.GlGreen(), TextColor.GlBlue(), TextFont.FontFamily.Name, TextFont.Size, mousePos);
            }
        }

        private void GrblSceneControl_OpenGLDraw(object sender, RenderEventArgs args)
        {
            // compute width ratio
            double ratio = Width / (Camera.Right - Camera.Left);
            // set colors
            Scene.ClearColour.FromColor(BackgroundColor);
            mGrid.TicksColor = TicksColor;
            mGrid.MinorsColor = MinorsColor;
            mGrid.OriginsColor = OriginsColor;
            SetViewPort();
            DrawPointer();
            DrawRulers(ratio);
            DrawMouseCoord(ratio);
        }

        private void GrblSceneControl_MouseLeave(object sender, EventArgs e)
        {
            mMousePos = null;
            mMousePosCurrent = null;
        }

        private void GrblSceneControl_MouseUp(object sender, MouseEventArgs e)
        {
            mMousePos = null;
            mMousePosCurrent = null;
        }

        private void GrblSceneControl_MouseDown(object sender, MouseEventArgs e) => mMousePos = e.Location;

        private void GrblSceneControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (mMousePos != null)
            {
                mShift.X -= (e.Location.X - (int)mMousePos?.X) * 2f / mShift.Z;
                mShift.Y -= (e.Location.Y - (int)mMousePos?.Y) * 2f / mShift.Z;
                mMousePos = e.Location;
            }
            mMousePosCurrent = e.Location;
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

        private void OnFileLoaded(long elapsed, string filename)
        {
            if (mGrbl3D != null)
            {
                Scene.SceneContainer.RemoveChild(mGrbl3D);
                mGrbl3D = null;
            }
            mGrbl3D = new Grbl3D(Core.LoadedFile, "Grbl file", false);
            Scene.SceneContainer.AddChild(mGrbl3D);
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
