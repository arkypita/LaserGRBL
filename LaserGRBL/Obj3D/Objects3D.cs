using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace LaserGRBL.Obj3D
{
    public class Object3DVertex
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Intensity { get; set; }
        public GrblCommand Command { get; set; }
    }

    public class Object3DDisplayList
    {
        public bool IsValid { get; private set; } = true;
        public DisplayList DisplayList { get; private set; }
        public List<Object3DVertex> Vertices { get; } = new List<Object3DVertex>();

        public void Begin(OpenGL gl, float lineWidth, int stipple = 0)
        {
            DisplayList = new DisplayList();
            DisplayList.Generate(gl);
            DisplayList.New(gl, DisplayList.DisplayListMode.Compile);
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT | OpenGL.GL_ENABLE_BIT | OpenGL.GL_LINE_BIT);
            UserControls.GrblPanel3D.CheckError(gl, "Attrib");
			gl.Disable(OpenGL.GL_LIGHTING);
			UserControls.GrblPanel3D.CheckError(gl, "Light");
			gl.Disable(OpenGL.GL_TEXTURE_2D);
			UserControls.GrblPanel3D.CheckError(gl, "Texture");
			gl.LineWidth(lineWidth);
            UserControls.GrblPanel3D.CheckError(gl, "LineWidth");
            if (stipple > 0) {
                gl.LineStipple(stipple, 0xAAAA);
                UserControls.GrblPanel3D.CheckError(gl, "LineStipple");
                gl.Enable(OpenGL.GL_LINE_STIPPLE);
                UserControls.GrblPanel3D.CheckError(gl, "EnableLineStipple");
            }
			gl.Begin(OpenGL.GL_LINES);
        }

        public void End(OpenGL gl)
        {
            gl.End();
            gl.PopAttrib();
            DisplayList.End(gl);
        }

        internal void Invalidate() => IsValid = false;

        internal void Invalidated() => IsValid = true;

    }

    public abstract class Object3D : SceneElement, IDisposable
    {
        protected List<Object3DDisplayList> mDisplayLists = new List<Object3DDisplayList>();
        public float LineWidth { get; set; }
        protected OpenGL mGL;
        protected Object3DDisplayList mCurrentDisplayList = null;
        protected const int MAX_VECTOR_IN_DISPLAY_LIST = 5000;
        protected bool mInvalidate = true;

        public ulong VertexCounter { get; private set; } = 0;

        public Object3D(string name, float lineWidth)
        {
            Name = name;
            LineWidth = lineWidth;
        }

        protected virtual void ClearDisplayList()
        {
            foreach (Object3DDisplayList list in mDisplayLists)
            {
                list.DisplayList.Delete(mGL);
            }
            mDisplayLists.Clear();
        }

        public virtual void Render(OpenGL gl)
        {
            if (mDisplayLists.Count <= 0 || mInvalidate)
            {
                mGL = gl;
                ClearDisplayList();
                CreateDisplayList();
                mInvalidate = false;
            }
            CallDisplayList(gl);
        }

        protected abstract void CallDisplayList(OpenGL gl);

        private void CreateDisplayList()
        {
            Draw();
			mCurrentDisplayList.End(mGL);
            mCurrentDisplayList = null;
        }

        protected abstract void Draw();

        protected void NewDisplayList(bool first = false, int? lineWidth = null, int stipple = 0)
        {
            if (!first) mCurrentDisplayList.End(mGL);
            mCurrentDisplayList = new Object3DDisplayList();
            mCurrentDisplayList.Begin(mGL, lineWidth == null ? LineWidth : (int)lineWidth, stipple);
            mDisplayLists.Add(mCurrentDisplayList);
        }

        public void AddVertex(double x, double y, double z, GLColor color, GrblCommand command = null, float intensity = 1)
        {
			VertexCounter++;
            mGL.Color(color);
            mGL.Vertex(x, y, z);
            mCurrentDisplayList.Vertices.Add(new Object3DVertex  { X = x, Y = y, Z = z, Intensity = intensity, Command = command });
            if (command != null) command.LinkedDisplayList = mCurrentDisplayList;
        }

        public void CheckListSize()
        {
            if (mCurrentDisplayList.Vertices.Count > MAX_VECTOR_IN_DISPLAY_LIST) NewDisplayList();
        }

        public virtual void Dispose()
        {
            foreach (Object3DDisplayList object3DDisplayList in mDisplayLists)
            {
                object3DDisplayList.Vertices.Clear();
            }
            ClearDisplayList();
        }

        public virtual void Invalidate()
        {
            mInvalidate = true;
        }

    }

    public class Grid3D : Object3D
    {
        public static int ViewportSize { get; set; } = 1000000;
        public int ControlWidth { get; set; }
        private DisplayList mDisplayTick;
        private DisplayList mDisplayMinor;
        protected OrthographicCamera mCamera;
        private GLColor mTicksColor = new GLColor();
        [XmlIgnore]
        public GLColor TicksColor {
            get => mTicksColor;
            set
            {
                if (value != mTicksColor)
                {
                    mTicksColor = value;
                    ClearDisplayList();
                }
            }
        }
        private GLColor mMinorsColor = new GLColor();
        public GLColor MinorsColor
        {
            get => mMinorsColor;
            set
            {
                if (value != mMinorsColor)
                {
                    mMinorsColor = value;
                    ClearDisplayList();
                }
            }
        }
        private GLColor mOriginsColor = new GLColor();
        public GLColor OriginsColor
        {
            get => mOriginsColor;
            set
            {
                if (value != mOriginsColor)
                {
                    mOriginsColor = value;
                    ClearDisplayList();
                }
            }
        }

        public double GridSize => ControlWidth > 0 ? (mCamera.Right - mCamera.Left) * 10 / ControlWidth : 0;
        private bool mShowMinor => GridSize <= 2;

        public Grid3D(OrthographicCamera camera) : base("Grid", 0.01f) {
            mCamera = camera;
        }

        private void DrawVerticalLine(double x, float f, GLColor color)
        {
            AddVertex(x, mCamera.Bottom, f, color);
            AddVertex(x, mCamera.Top, f, color);
        }

        private void DrawHorizontalLine(double y, float f, GLColor color)
        {
            AddVertex(mCamera.Left, y, f, color);
            AddVertex(mCamera.Right, y, f, color);
        }

        protected override void Draw()
        {
            NewDisplayList(true);
            if (ControlWidth <= 0) return;
            int left = (int)mCamera.Left;
            int right = (int)mCamera.Right;
            int bottom = (int)mCamera.Bottom;
            int top = (int)mCamera.Top;
            float f = -20;
            if (mShowMinor)
            {
                // minor tick display list
                mDisplayMinor = mCurrentDisplayList.DisplayList;
                for (int i = 1; i <= right; i++) DrawVerticalLine(i, f, MinorsColor);
                for (int i = 1; i >= left; i--) DrawVerticalLine(i, f, MinorsColor);
                for (int i = 0; i <= top; i++) DrawHorizontalLine(i, f, MinorsColor);
                for (int i = 0; i >= bottom; i--) DrawHorizontalLine(i, f, MinorsColor);
                // tick display list
                NewDisplayList();
            }
            int gridSize = Math.Max(10, (int)GridSize * 2);
            f = -10;
            mDisplayTick = mCurrentDisplayList.DisplayList;
            for (int i = 0; i <= right; i += gridSize) DrawVerticalLine(i, f, TicksColor);
            for (int i = 0; i >= left; i -= gridSize) DrawVerticalLine(i, f, TicksColor);
            for (int i = 0; i <= top; i += gridSize) DrawHorizontalLine(i, f, TicksColor);
            for (int i = 0; i >= bottom; i -= gridSize) DrawHorizontalLine(i, f, TicksColor);
            f = -5;
            DrawVerticalLine(0, f, OriginsColor);
            DrawHorizontalLine(0, f, OriginsColor);
        }

        protected override void CallDisplayList(OpenGL gl)
        {
            mDisplayTick.Call(gl);
            if (mShowMinor)
            {
                mDisplayMinor.Call(gl);
            }
            mInvalidate = false;
        }

    }

	public class Grbl3D : Object3D
	{
		public readonly GrblCore Core;
		private readonly bool mJustLaserOffMovements;
		private Object3DDisplayList mBoundingBoxDisplayList;
        public GLColor BackgroundColor;
		public GLColor Color;
		public GLColor BoundingBoxColor;
		private double mLoadingPercentage = 0;
		public bool ShowBoundingBox { get; set; } = true;
		private const int BOUNDING_RECT_LINE_WIDTH = 1;
		private const int BOUNDING_RECT_STIPPLE = 10;

		public delegate void OnLoadingPercentageChangeDlg();
		public static event OnLoadingPercentageChangeDlg OnLoadingPercentageChange;

		public Grbl3D(GrblCore core, string name, bool justLaserOffMovements, Color color, Color backgroundColor, Color boundingBoxColor) : base(name, core.PreviewLineSize.Value)
		{
			Core = core;
			mJustLaserOffMovements = justLaserOffMovements;
			Color = color;
            BackgroundColor = backgroundColor;
			BoundingBoxColor = boundingBoxColor;
		}


		public double LoadingPercentage
		{
			get { return mLoadingPercentage; }
			private set
			{
				if (value != mLoadingPercentage)
				{
					mLoadingPercentage = value;
					OnLoadingPercentageChange?.Invoke();
				}
			}
		}

        private float Map(float value, float outMin, float outMax)
        {
            return value * (outMax - outMin) + outMin;
        }

        private GLColor Blend(GLColor color, float intensity)
        {
            GLColor result = new GLColor();
            result.R = Map(intensity, BackgroundColor.R, color.R);
            result.G = Map(intensity, BackgroundColor.G, color.G);
            result.B = Map(intensity, BackgroundColor.B, color.B);
            result.A = 1;
            return result;
        }

		protected override void Draw()
        {
            const int zPos = 0;
            NewDisplayList(true, BOUNDING_RECT_LINE_WIDTH, BOUNDING_RECT_STIPPLE);
            GrblCommand.StatePositionBuilder spb = new GrblCommand.StatePositionBuilder();
            Core.LoadedFile.InUse = true;
            
            mBoundingBoxDisplayList = mCurrentDisplayList;
            if (!mJustLaserOffMovements)
            {
                const int borderLimit = 1000000;

                AddVertex(-borderLimit, (double)Core.LoadedFile.Range.DrawingRange.Y.Min, zPos, BoundingBoxColor);
                AddVertex(borderLimit, (double)Core.LoadedFile.Range.DrawingRange.Y.Min, zPos, BoundingBoxColor);

                AddVertex((double)Core.LoadedFile.Range.DrawingRange.X.Max, -borderLimit, zPos, BoundingBoxColor);
                AddVertex((double)Core.LoadedFile.Range.DrawingRange.X.Max, borderLimit, zPos, BoundingBoxColor);

                AddVertex(-borderLimit, (double)Core.LoadedFile.Range.DrawingRange.Y.Max, zPos, BoundingBoxColor);
                AddVertex(borderLimit, (double)Core.LoadedFile.Range.DrawingRange.Y.Max, zPos, BoundingBoxColor);

                AddVertex((double)Core.LoadedFile.Range.DrawingRange.X.Min, -borderLimit, zPos, BoundingBoxColor);
                AddVertex((double)Core.LoadedFile.Range.DrawingRange.X.Min, borderLimit, zPos, BoundingBoxColor);
            }
            NewDisplayList();
            int commandsCount = Core.LoadedFile.Commands.Count;
            for (int i = 0; i < commandsCount; i++)
            {
                LoadingPercentage = (i + 1.0) / commandsCount * 100;
                GrblCommand cmd = Core.LoadedFile.Commands[i];
                try
                {
                    cmd.BuildHelper();
                    spb.AnalyzeCommand(cmd, false);

                    if (spb.TrueMovement())
                    {
                        if (spb.G0G1 && cmd.IsLinearMovement)
                        {
                            GLColor color = Color;
                            float intensity = 1;
                            if (spb.LaserBurning && !mJustLaserOffMovements)
                            {
                                intensity = spb.GetCurrentAlpha(Core.LoadedFile.Range.SpindleRange) / 255f;
                            }
                            else
                            {
                                intensity = mJustLaserOffMovements ? 1 : 0;
                            }
                            if (intensity > 0)
                            {
                                color = Blend(color, intensity);
                                AddVertex((float)spb.X.Previous, (float)spb.Y.Previous, zPos, color, cmd, intensity);
                                AddVertex((float)spb.X.Number, (float)spb.Y.Number, zPos, color, cmd, intensity);
                            }
                        }
                        else if (spb.G2G3 && cmd.IsArcMovement)
                        {
                            GrblCommand.G2G3Helper ah = spb.GetArcHelper(cmd);
                            if (ah.RectW > 0 && ah.RectH > 0)
                            {
                                double? lastX = null;
                                double? lastY = null;
                                GLColor color = Color;
                                float intensity = spb.GetCurrentAlpha(Core.LoadedFile.Range.SpindleRange) / 255f;
                                color = Blend(color, intensity);
                                double startAngle = ah.StartAngle;
                                double endAngle = ah.StartAngle + ah.AngularWidth;
                                int sign = Math.Sign(ah.AngularWidth);
                                double angleStep = Math.Abs(ah.AngularWidth / Math.PI / 36);
                                for (double angle = startAngle; sign * (angle - startAngle) <= sign * ah.AngularWidth; angle += sign * angleStep)
                                {
                                    double x = ah.CenterX + ah.RectW / 2 * Math.Cos(angle);
                                    double y = ah.CenterY + ah.RectH / 2 * Math.Sin(angle);
                                    if (lastX != null && lastY != null)
                                    {
                                        AddVertex((double)lastX, (double)lastY, zPos, color, cmd, intensity);
                                    }
                                    else
                                    {
                                        AddVertex(x, y, zPos, color, cmd, intensity);
                                    }
                                    AddVertex(x, y, zPos, color, cmd, intensity);
                                    lastX = x;
                                    lastY = y;
                                }
                            }
                        }
                        CheckListSize();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { cmd.DeleteHelper(); }
            }
            Core.LoadedFile.InUse = false;
        }

        public override void Invalidate()
        {
            if (!mJustLaserOffMovements && mBoundingBoxDisplayList != null)
            {
                mBoundingBoxDisplayList.DisplayList.Delete(mGL);
                mBoundingBoxDisplayList.Begin(mGL, BOUNDING_RECT_LINE_WIDTH, BOUNDING_RECT_STIPPLE);
                foreach (Object3DVertex vertex in mBoundingBoxDisplayList.Vertices)
                {
                    mGL.Color(BoundingBoxColor);
                    mGL.Vertex(vertex.X, vertex.Y, vertex.Z);
                }
                mBoundingBoxDisplayList.End(mGL);
                mBoundingBoxDisplayList.Invalidated();
            }
            for (int i = 1; i < mDisplayLists.Count; i++)
            {
                Object3DDisplayList list = mDisplayLists[i];
                if (!list.IsValid) {
                    list.DisplayList.Delete(mGL);
                    list.Begin(mGL, LineWidth);
                    foreach (Object3DVertex vertex in list.Vertices)
                    {
                        Color newColor;
                        if (mJustLaserOffMovements || !Core.ShowExecutedCommands.Value)
                        {
                            newColor = Color;
                        }
                        else
                        {
                            switch (vertex.Command.Status)
                            {
                                case GrblCommand.CommandStatus.ResponseGood:
                                    newColor = ColorScheme.PreviewCommandOK;
                                    break;
                                case GrblCommand.CommandStatus.ResponseBad:
                                case GrblCommand.CommandStatus.InvalidResponse:
                                    newColor = ColorScheme.PreviewCommandKO;
                                    break;
                                case GrblCommand.CommandStatus.WaitingResponse:
                                    newColor = ColorScheme.PreviewCommandWait;
                                    break;
                                default:
                                    newColor = Color;
                                    break;
                            }
                        }
                        newColor = Blend(newColor, vertex.Intensity);
                        mGL.Color(newColor);
                        mGL.Vertex(vertex.X, vertex.Y, vertex.Z);
                    }
                    list.End(mGL);
                    list.Invalidated();
                }
            }
        }

        internal void InvalidateAll()
        {
            for (int i = 1; i < mDisplayLists.Count; i++)
            {
                mDisplayLists[i].Invalidate();
            }
            Invalidate();
        }

        protected override void CallDisplayList(OpenGL gl)
        {
            if (ShowBoundingBox) mBoundingBoxDisplayList?.DisplayList?.Call(mGL);
            for (int i = 1; i < mDisplayLists.Count; i++)
            {
                mDisplayLists[i].DisplayList.Call(gl);
            }
        }

    }

}
