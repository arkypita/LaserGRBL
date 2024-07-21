using LaserGRBL.Obj3D;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Cameras;
using SharpGL.Version;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using Tools;
using static LaserGRBL.ProgramRange;

namespace LaserGRBL.UserControls
{

    [ToolboxBitmap(typeof(SceneControl), "GrblScene")]
	public partial class GrblPanel3D : UserControl, IGrblPanel
	{
		// last mouse position
		private Point? mLastMousePos = null;
		// current mouse world position
        private Point? mMousePos = null;
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
        private GLColor mTextBoundingColor { get; set; }
        // default font
        private const string mFontName = "Courier New";
        private Font mTextFont { get; set; } = new Font(mFontName, 12);
		// background
		private Grid3D mGrid = null;
		// grbl object
		private Grbl3D mGrbl3D = null;
		private Grbl3D mGrbl3DOff = null;
		private object mGrbl3DLock = new object();
		private string mMessage = null;
        // reload request
        private bool mReload = false;
		// invalidate all request
		private bool mInvalidateAll = false;
		// viewport padding
		private Padding mPadding = new Padding(70, 0, 0, 30);
		// grbl core
		private GrblCore Core;
		public float PointerSize { get; set; } = 3;
		// last control size
		private PointF mLastControlSize;
		// drawing thread
		private Tools.ThreadObject mThreadDraw;
		// generated 3d bitmap
		private DoubleBufferBmp mBmp = new DoubleBufferBmp();
		// open gl object
		private OpenGL OpenGL;
		private GPoint mLastWPos;
		private GPoint mLastMPos;
		private float mCurF;
		private float mCurS;
		bool forcez = false;
		private bool mFSTrig;

		private Base.Mathematics.MobileDAverageCalculator FrameTime;
		private Base.Mathematics.MobileDAverageCalculator SleepTime;
		private Base.Mathematics.MobileDAverageCalculator RefreshRate;

		private static Exception FatalException;

		private bool mShowCursor = true;
		public bool ShowCursor
		{
            get => mShowCursor;
            set
			{
				if (mShowCursor != value)
                {
                    mShowCursor = value;
					if (value)
					{
						Cursor.Show();
					}
					else
					{
						Cursor.Hide();
					}
                }
            }
        }

		// 0 = never run, 1 = init begin, 2 = create complete, 3 = init complete, 4 = draw begin, 5 = draw end, > 5 = running (can be tested with a timer to check if it stop incrementing)
		private static ulong OpCounter;

		public static string CurrentVendor = "";
		public static string CurrentRenderer = "";
		public static string CurrentGLVersion = "";

		private static string FirstGlError = null;

		public static string GlDiagnosticMessage
		{
			get
			{
				if (Settings.CurrentGraphicMode == Settings.GraphicMode.GDI) //we not use GrblPanel3D
					return null;

				if (FatalException != null)
					return FatalException.Message;
				else if (FirstGlError != null)
					return FirstGlError;
				else if (OpCounter < 5)
					return $"OpCounter {OpCounter}";

				return null;
			}
		}

		public AutoResetEvent RR = new AutoResetEvent(true);		//redraw required
		Tools.ThreadObject TH = null;	//drawing thread
		public GrblPanel3D()
		{
			InitializeComponent();
			OpCounter = 0;
			FrameTime = new Base.Mathematics.MobileDAverageCalculator(10);
			SleepTime = new Base.Mathematics.MobileDAverageCalculator(10);
			RefreshRate = new Base.Mathematics.MobileDAverageCalculator(3);
			mLastWPos = GPoint.Zero;
			mLastMPos = GPoint.Zero;
			forcez = Settings.GetObject("Enale Z Jog Control", false);
			MouseDoubleClick += GrblPanel3D_DoubleClick;
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			MouseWheel += GrblSceneControl_MouseWheel;
			MouseDown += GrblSceneControl_MouseDown;
			MouseMove += GrblSceneControl_MouseMove;
			MouseUp += GrblSceneControl_MouseUp;
			MouseLeave += GrblSceneControl_MouseLeave;
			Grbl3D.OnLoadingPercentageChange += Grbl3D_OnLoadingPercentageChange;
			Resize += GrblPanel3D_Resize;
			Disposed += GrblPanel3D_Disposed;
			mCamera.Position = new Vertex(0, 0, 0);
			mCamera.Near = 0;
			mCamera.Far = 100000000;
			AutoSizeDrawing();
			mLastControlSize = new PointF(Width, Height);
			mGrid = new Grid3D(mCamera);
			OnColorChange();
			TH = new Tools.ThreadObject(DrawScene, 10000, true, "OpenGL", InitializeOpenGL, ThreadPriority.Lowest, ApartmentState.STA, RR);
			TH.Start();

			/*
			// TEST JOG 
			Task.Factory.StartNew(() => {
				Random RNG = new Random();
				while (true)
				{
					Core?.JogToPosition(new PointF(50 - RNG.Next(100), 50 - RNG.Next(100)), false);
					if (RNG.Next(20) == 10)
						Core?.JogAbort();
					if (RNG.Next(20) == 10)
						Thread.Sleep(10);
				}
			});
			*/
		}

		private static double GetRulerStep(double n)
        {
            int digitCount = Convert.ToInt32(Math.Max(3, Math.Floor(Math.Log10(n) + 1)));
            int power = Convert.ToInt32(Math.Pow(10, digitCount - 1));
            double step = n / power;
            double result;
            if (step < 1)
                result = 5;
            else if (step < 2)
                result = 10;
            else if (step < 6)
                result = 30;
            else
                result = 50;
            return result * Math.Pow(10, digitCount - 3);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				try
				{
					TH?.Stop();
					TH?.Dispose();
					TH = null;
				} catch { }
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		[System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
		protected void InitializeOpenGL()
		{
			try
			{
				OpCounter++;

				object parameter = null;
				OpenGL = new OpenGL();


				if (Settings.RequestedGraphicMode == Settings.GraphicMode.DIB)
				{
					Settings.CurrentGraphicMode = Settings.GraphicMode.DIB;
					OpenGL.Create(OpenGLVersion.OpenGL2_1, RenderContextType.DIBSection, Width, Height, 32, parameter);
					CheckError(OpenGL, "Create");
				}
				else // if we are here the requested is AUTO or FBO so we can proceed with FBO and fallback to DIB
				{
					try
					{
						Settings.CurrentGraphicMode = Settings.GraphicMode.FBO;
						OpenGL.Create(OpenGLVersion.OpenGL2_1, RenderContextType.FBO, Width, Height, 32, parameter);
						if (OpenGL.GetError() != OpenGL.GL_NO_ERROR) throw new Exception("Cannot Create FBO");
					}
					catch
					{
						Settings.CurrentGraphicMode = Settings.GraphicMode.DIB;
						OpenGL.Create(OpenGLVersion.OpenGL2_1, RenderContextType.DIBSection, Width, Height, 32, parameter);
						CheckError(OpenGL, "Create");
					}
				}

				OpCounter++;

				try { CurrentVendor = OpenGL.Vendor; } catch { CurrentVendor = "Unknown"; }
				try { CurrentRenderer = OpenGL.Renderer; } catch { CurrentRenderer = "Unknown"; }
				try { CurrentGLVersion = OpenGL.Version; } catch { CurrentGLVersion = "0.0"; }

				Logger.LogMessage("OpenGL", "{0}->{1} OpenGL {2}, {3}, {4}", Settings.RequestedGraphicMode, Settings.CurrentGraphicMode, CurrentGLVersion, CurrentVendor, CurrentRenderer);

				OpenGL.ShadeModel(OpenGL.GL_SMOOTH);
				CheckError(OpenGL, "ShadeModel");
				OpenGL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
				OpenGL.ClearDepth(1.0f);
				OpenGL.Enable(OpenGL.GL_DEPTH_TEST);
				CheckError(OpenGL, "DepthTest");
				OpenGL.DepthFunc(OpenGL.GL_LEQUAL);
				CheckError(OpenGL, "DepthFunc");
				OpenGL.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);
				CheckError(OpenGL, "Hint");

				OpCounter++;
			}
			catch (Exception ex)
			{
				Logger.LogException("OpenGL", ex);
				FatalException = ex;
				Invalidate();
				ExceptionManager.OnHandledException(ex, true);
			}
		}

		[System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
		private void DrawScene()
		{
			try
			{
				if (FatalException != null) return;

				OpCounter++;
				Tools.SimpleCrono crono = new Tools.SimpleCrono(true);
				OpenGL.MakeCurrent();
				CheckError(OpenGL, "MakeCurrent");
				OpenGL.SetDimensions(Width, Height);
				OpenGL.Viewport(0, 0, Width, Height);
				mCamera.Project(OpenGL);
				CheckError(OpenGL, "Viewport");
				OpenGL.ClearColor(mBackgroundColor.R, mBackgroundColor.G, mBackgroundColor.B, mBackgroundColor.A);
				OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
				CheckError(OpenGL, "Clear");
				// render grid
				mGrid.ControlWidth = Width;
				mGrid.TicksColor = mTicksColor;
				mGrid.MinorsColor = mMinorsColor;
				mGrid.OriginsColor = mOriginsColor;
				mGrid.Render(OpenGL);
				CheckError(OpenGL, "RenderGrid");
				// enable anti alias
				OpenGL.Enable(OpenGL.GL_BLEND);
				OpenGL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
				CheckError(OpenGL, "Blend");
				OpenGL.Enable(OpenGL.GL_LINE_SMOOTH);
				OpenGL.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);
				CheckError(OpenGL, "Smooth");
				// manage grbl object
				if (mReload)
				{
					mReload = false;
					Grbl3D oldGrbl3D = mGrbl3D;
                    Grbl3D oldGrbl3DOff = mGrbl3DOff;
                    lock (mGrbl3DLock)
                    {
                        mGrbl3D = null;
                        mGrbl3DOff = null;
					}
					mMessage = Strings.ClearDrawing;
					oldGrbl3D?.Dispose();
                    oldGrbl3DOff?.Dispose();
					mMessage = Strings.PrepareDrawing;
                    Grbl3D newGrbl3D = new Grbl3D(Core, "LaserOn", false, ColorScheme.PreviewLaserPower, ColorScheme.PreviewBackColor, ColorScheme.PreviewJobRange);
                    Grbl3D newGrbl3DOff = new Grbl3D(Core, "LaserOff", true, ColorScheme.PreviewOtherMovement, ColorScheme.PreviewBackColor, ColorScheme.PreviewJobRange);
                    mMessage = null;
                    lock (mGrbl3DLock)
					{
						mGrbl3D = newGrbl3D;
						mGrbl3DOff = newGrbl3DOff;
                    }
                    mGrbl3DOff.LineWidth = 1;
				}
				if (mGrbl3D != null)
				{
					if (mInvalidateAll)
					{
						mInvalidateAll = false;
						mGrbl3D.Color = ColorScheme.PreviewLaserPower;
						mGrbl3D.BackgroundColor = ColorScheme.PreviewBackColor;
                        mGrbl3D.BoundingBoxColor = ColorScheme.PreviewJobRange;
                        mGrbl3D.InvalidateAll();
						mGrbl3DOff.Color = ColorScheme.PreviewOtherMovement;
                        mGrbl3DOff.BackgroundColor = ColorScheme.PreviewBackColor;
                        mGrbl3DOff.InvalidateAll();
					}
					if (Core.ShowLaserOffMovements.Value)
					{
						mGrbl3DOff.Invalidate();
						mGrbl3DOff.Render(OpenGL);
					}
					mGrbl3D.ShowBoundingBox = Core.ShowBoundingBox.Value;
                    mGrbl3D.Invalidate();
					mGrbl3D.Render(OpenGL);
				}
				CheckError(OpenGL, "RenderObject");
				// disable anti alias
				OpenGL.Disable(OpenGL.GL_BLEND);
				OpenGL.Disable(OpenGL.GL_LINE_SMOOTH);
				CheckError(OpenGL, "BlendDisable");
				// main hud
				DrawPointer();
				CheckError(OpenGL, "Pointer");
				DrawRulers();
				CheckError(OpenGL, "Rulers");
				OpenGL.Flush();
				CheckError(OpenGL, "Flush");
				Bitmap newBmp = new Bitmap(Width, Height);
				// clone opengl graphics
				using (Graphics g = Graphics.FromImage(newBmp))
				{
					IntPtr handleDeviceContext = g.GetHdc();
					OpenGL.Blit(handleDeviceContext);
					CheckError(OpenGL, "Blit");
					g.ReleaseHdc(handleDeviceContext);
				}
				mBmp.Bitmap = newBmp;

				FrameTime.EnqueueNewSample(crono.ElapsedTime.TotalMilliseconds);

				crono.Start();
				Thread.Sleep(BestSleep(FrameTime.CurrentValue, 10, 100, 15, 50));
				SleepTime.EnqueueNewSample(crono.ElapsedTime.TotalMilliseconds);

				// call control invalidate
				Invalidate();

				OpCounter++;
			}
			catch (Exception ex)
			{
				Logger.LogException("OpenGL", ex);
				FatalException = ex;
				Invalidate();
				ExceptionManager.OnHandledException(ex, true);
			}
		}

		private static int BestSleep(double renderTime, double minRender, double maxRender, double minSleep, double maxSleep)
		{
			return (int) Math.Max(minSleep, Math.Min(maxSleep, (renderTime - minRender) * (maxSleep - minSleep) / (maxRender - minRender) + minSleep));
		}

		private void SetWorldPosition(double left, double right, double bottom, double top)
        {
			double maxViewport = Grid3D.ViewportSize * 2;
            if (right - left > maxViewport) return;
            if (bottom - top > maxViewport) return;
            if (left < - maxViewport / 2) {
				right += (-maxViewport / 2 - left);
				left = -maxViewport / 2;
            }
            if (right > maxViewport / 2)
            {
                left += (maxViewport / 2 - right);
                right = maxViewport / 2;
            }
            if (bottom < -maxViewport / 2)
            {
                top += (-maxViewport / 2 - bottom);
                bottom = -maxViewport / 2;
            }
            if (top > maxViewport / 2)
            {
                bottom += (maxViewport / 2 - top);
                top = maxViewport / 2;
            }
            // set new camera coords
            mCamera.Left = left;
			mCamera.Right = right;
			mCamera.Bottom = bottom;
			mCamera.Top = top;
			mGrid?.Invalidate();
			// call control invalidate
			RR.Set();
		}

		private void GrblPanel3D_Resize(object sender, EventArgs e)
		{
			// compute ratiobesed on last size
			double wRatio = Width / mLastControlSize.X;
			double hRatio = Height / mLastControlSize.Y;
			// compute increment
			double wIncrement = (mCamera.Right - mCamera.Left) - (mCamera.Right - mCamera.Left) * wRatio;
            double hIncrement = (mCamera.Top - mCamera.Bottom) - (mCamera.Top - mCamera.Bottom) * hRatio;
			double newLeft = mCamera.Left + wIncrement / 2;
			double newRight = mCamera.Right - wIncrement / 2;
			double newTop = mCamera.Top - hIncrement / 2;
			double newBottom = mCamera.Bottom + hIncrement / 2;
            double rw = 1;
            double rh = 1;
			double maxViewport = Grid3D.ViewportSize * 2;
            if (newRight - newLeft > maxViewport)
            {
                rw = maxViewport / (newRight - newLeft);
            }
            if (newBottom - newTop > maxViewport)
            {
                rh = maxViewport / (newBottom - newTop);
            }
			double r = Math.Min(rw, rh);
            newLeft = newLeft * r;
            newRight = newRight * r;
            newTop = newTop * r;
            newBottom = newBottom * r;
            // set world positions
            SetWorldPosition(newLeft, newRight, newBottom, newTop);
			// save last size
			mLastControlSize = new PointF(Width, Height);
		}

		private void GrblPanel3D_DoubleClick(object sender, MouseEventArgs e)
		{
			if (Settings.GetObject("Click N Jog", true) && mMouseWorldPosition != null)
			{
				Core.JogToPosition((PointF)mMouseWorldPosition, e.Button == MouseButtons.Right);
			}
		}

		static uint errcounter = 0;
		public static bool CheckError(OpenGL gl, string action)
		{
			bool rv = false;
			uint err = gl.GetError();
			uint loopCount = 0; //loop with count checks to prevent infinite loop in case of code errors
			while (err != OpenGL.GL_NO_ERROR && loopCount++ < 20) //glGetError should always be called in a loop, until it returns GL_NO_ERROR https://registry.khronos.org/OpenGL-Refpages/gl4/html/glGetError.xhtml
			{
				rv = true;
				if (errcounter < 3) //we store only first 3 error
				{
					errcounter++;
					string message = string.Format("[{0}] {1}: {2}", err, action, gl.GetErrorDescription(err));
					if (FirstGlError == null) FirstGlError = message; //we send only the first one
					Logger.LogMessage("OpenGL", message);
				}
				err = gl.GetError();
			}
			return rv;
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
			double size = mGrid.GridSize * 1.5;
			OpenGL.Vertex(pointerPos.X - size, pointerPos.Y, pointerPos.Z);
			OpenGL.Vertex(pointerPos.X + size, pointerPos.Y, pointerPos.Z);
			OpenGL.End();
			OpenGL.Begin(OpenGL.GL_LINES);
			OpenGL.Vertex(pointerPos.X, pointerPos.Y - size, pointerPos.Z);
			OpenGL.Vertex(pointerPos.X, pointerPos.Y + size, pointerPos.Z);
			OpenGL.End();
			OpenGL.Flush();
		}

		private string HumanReadableLength(int mm, double worldWidth, out string uom)
        {
            if (worldWidth > 10000)
            {
                uom = "m";
                return $"{Math.Round(mm / 1000.0, 0)}";
            }
            else if (worldWidth > 1000)
            {
                uom = "cm";
                return $"{Math.Round(mm / 10.0, 0)}";
			}
			else
			{
				uom = "mm";
				return $"{mm}";
			}
		}

		private SizeF MeasureOpenGlText(string text)
		{
			Size size = TextRenderer.MeasureText(text, mTextFont);
            return new SizeF(size.Width * 0.8f, size.Height * 0.8f);
        }

		private void DrawText(string text, double x, double y, GLColor color)
		{
            OpenGL.DrawText((int)x, (int)y, color.R, color.G, color.B, mTextFont.FontFamily.Name, mTextFont.Size, text);
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
			double step = GetRulerStep(worldWidth);
			// half step
			int halfStep = (int)(step / 2);
			// get ratio
            double wRatio = Width / (mCamera.Right - mCamera.Left);
			// unit of measure
			string uom = string.Empty;
            HumanReadableLength(0, worldWidth, out uom); //call it just to be sure to have uom loaded by worldWidth
			int? minDrawingX = null;
            int? maxDrawingX = null;
            int? minDrawingY = null;
            int? maxDrawingY = null;
            if (Core?.LoadedFile?.Range.DrawingRange.ValidRange == true)
            {
                minDrawingX = (int)Core.LoadedFile.Range.DrawingRange.X.Min;
                maxDrawingX = (int)Core.LoadedFile.Range.DrawingRange.X.Max;
                minDrawingY = (int)Core.LoadedFile.Range.DrawingRange.Y.Min;
                maxDrawingY = (int)Core.LoadedFile.Range.DrawingRange.Y.Max;
            }
			bool showBoundingBox = Core?.ShowBoundingBox.Value ?? false;
            // draw horizontal
            for (int i = (int)mCamera.Left + (int)(mPadding.Left / wRatio); i <= (int)mCamera.Right - (int)(mPadding.Right / wRatio); i += 1)
			{
				bool canDraw = i % step == 0;
                if (showBoundingBox && (minDrawingX != null || maxDrawingX != null))
                {
					canDraw &=
						i < (minDrawingX - halfStep) ||
						(i > (minDrawingX + halfStep) && i < (maxDrawingX - halfStep)) ||
						i > (maxDrawingX + halfStep);
                    canDraw |= i == maxDrawingX || i == minDrawingX;
                }
                if (canDraw)
				{
					string text = HumanReadableLength(i, worldWidth, out uom);
					SizeF sizeProp = MeasureOpenGlText(text);
					double x = i * wRatio - mCamera.Left * wRatio - sizeProp.Width / 4f;
					double y = mPadding.Bottom - sizeProp.Height;
					DrawText(text, x, y, i == minDrawingX || i == maxDrawingX ? mTextBoundingColor : mTextColor);
                }
			}
			// get ratio
			double hRatio = Height / (mCamera.Top - mCamera.Bottom);
            // draw vertical
            for (int i = (int)mCamera.Bottom + (int)(mPadding.Bottom / hRatio); i <= (int)mCamera.Top - (int)(mPadding.Top / hRatio); i += 1)
            {
                bool canDraw = i % step == 0;
                if (showBoundingBox && (minDrawingY != null || maxDrawingY != null))
                {
                    canDraw &=
                        i < (minDrawingY - halfStep) ||
                        (i > (minDrawingY + halfStep) && i < (maxDrawingY - halfStep)) ||
                        i > (maxDrawingY + halfStep);
                    canDraw |= i == maxDrawingY || i == minDrawingY;
                }
                if (canDraw)
				{
					string text = HumanReadableLength(i, worldWidth, out uom);
					SizeF sizeProp = MeasureOpenGlText(text);
                    double x = mPadding.Left - sizeProp.Width;
					double y = i * hRatio - mCamera.Bottom * hRatio - sizeProp.Height / 4f;
                    DrawText(text, x, y, i == minDrawingY || i == maxDrawingY ? mTextBoundingColor : mTextColor);
                }
            }
			// clear uom  background
			OpenGL.Enable(OpenGL.GL_SCISSOR_TEST);
			OpenGL.Scissor(0, 0, mPadding.Left, mPadding.Bottom);
			OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
			OpenGL.Disable(OpenGL.GL_SCISSOR_TEST);

			// draw unit of measure
			SizeF uomSizeProp = MeasureOpenGlText(uom);
			DrawText(uom, mPadding.Left - uomSizeProp.Width -20, mPadding.Bottom - uomSizeProp.Height, mTextBoundingColor);

            OpenGL.Flush();
		}

		Tools.SimpleCrono FpsCrono;
		protected override void OnPaint(PaintEventArgs e)
		{
			Exception OnPaintException = null;

			if (FpsCrono != null) RefreshRate.EnqueueNewSample(FpsCrono.ElapsedTime.TotalMilliseconds); //compute time distance between two OnPaint
			FpsCrono = new Tools.SimpleCrono(true);

            Bitmap bmp = mBmp.Bitmap;
            if (bmp != null)
			{
				try
				{
					e.Graphics.DrawImage(bmp, new Point(0, 0));
					DoGDIDraw(e);
				}
				catch (Exception ex) { OnPaintException = ex;  }
			}
			else
			{
				// nothing to draw from GL Thread
			}

			if (FatalException != null)
				DrawException(e, FatalException.ToString());
			else if (OnPaintException != null)
				DrawException(e, OnPaintException.ToString());
			else if (OpCounter == 0)        // 0 = never run, 1 = init begin, 2 = create complete, 3 = init complete, 4 = draw begin, 5 = draw end, > 5 = running (can be tested with a timer to check if it stop incrementing)
				DrawException(e, "0.Thread Starting");
			else if (OpCounter == 1)
				DrawException(e, "1.Initializing OpenGL");
			else if (OpCounter == 2)
				DrawException(e, "2.Creation Complete");
			else if (OpCounter == 3)
				DrawException(e, "3.Init Complete");
			else if (OpCounter == 4)
				DrawException(e, "4.Draw Begin");
			else if (mBmp == null)
				;
		}

		private void DrawException(PaintEventArgs e, string text)
		{
			try
			{
				using (Font font = new Font(mFontName, 12))
				{
					Size size = MeasureText(text, mTextFont);
					Size size2 = new Size(size.Width + 20, size.Height);
					Point point = new Point((Width - size2.Width) / 2, (Height - size2.Height) / 2);
					DrawOverlay(e, point, size2, Color.Red, 100);
					e.Graphics.DrawString(text, font, Brushes.White, point.X, point.Y);
				}
			}
			catch { }
		}

		private string FormatCoord(float coord)
		{
			return string.Format("{0,10:######0.000}", coord);
		}

		private void DoGDIDraw(PaintEventArgs e)
		{
			if (Core == null) return;
			const int top = 12;
			using (Brush b = new SolidBrush(ColorScheme.PreviewText))
			using (Font font = new Font(mFontName, 10))
			{
				// set aliasing
				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				// use z
				bool useZ = mLastWPos.Z != 0 || mLastMPos.Z != 0 || forcez;
				string text = "         X          Y";
				if (useZ) text += "          Z    ";
				// last position
				text += $"\nMCO {FormatCoord(mLastMPos.X)} {FormatCoord(mLastMPos.Y)}";
				if (useZ) text += $" {FormatCoord(mLastMPos.Z)}";
				// working offset
				if (Core.WorkingOffset != GPoint.Zero)
				{
					text += $"\nWCO {FormatCoord(mLastWPos.X)} {FormatCoord(mLastWPos.Y)}";
					if (useZ) text += $" {FormatCoord(mLastWPos.Z)}";
				}
				// mouse info
				if (mMouseWorldPosition != null)
				{
					PointF pos = (PointF)mMouseWorldPosition;
					text += $"\nPTR {FormatCoord(pos.X)} {FormatCoord(pos.Y)}";
					if (useZ) text += "          ";
				}
				Size size = MeasureText(text, font);
				Point point = new Point(Width - size.Width - mPadding.Right, top);
				DrawOverlay(e, point, size, ColorScheme.PreviewRuler, 100);
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
				e.Graphics.DrawString(text, font, b, point.X, point.Y);

				// laser info
				if (mCurF != 0 || mCurS != 0 || mFSTrig)
				{
					mFSTrig = true;
					int pos = point.Y + size.Height + 5;
					text = $"F {string.Format("{0,6:####0.##}", mCurF)}\nS {string.Format("{0,6:##0.##}", mCurS)}";
					size = MeasureText(text, font);
					point = new Point(Width - size.Width - mPadding.Right, pos);
					DrawOverlay(e, point, size, ColorScheme.PreviewRuler, 100);
					e.Graphics.DrawString(text, font, b, point.X, point.Y);
				}

				// performance
				if (Core.ShowPerformanceDiagnostic.Value)
				{
					int pos = point.Y + size.Height + 5;

					string VertexString = null;
					ulong VertexCounter = 0;
					lock (mGrbl3DLock)
					{VertexCounter = mGrbl3D != null ? mGrbl3D.VertexCounter : 0;}

					if (VertexCounter < 1000)
						VertexString = string.Format("{0,6:##0}", VertexCounter);
					else if (VertexCounter < 1000000)
						VertexString = string.Format("{0,6:###0.0} K", VertexCounter / 1000.0);
					else if (VertexCounter < 1000000000)
						VertexString = string.Format("{0,6:###0.0} M", VertexCounter / 1000000.0);
					else
						VertexString = string.Format("{0,6:###0.0} B", VertexCounter / 1000000000.0);

					text = $"VER   {VertexString}\nAFT   {string.Format("{0,6:##0} ms", FrameTime.Avg)}\nAST   {string.Format("{0,6:##0} ms", SleepTime.Avg)}\nRPS   {string.Format("{0,6:##0}", Math.Max(0, Math.Floor(1000.0 / RefreshRate.Avg - 1)))}";
					size = MeasureText(text, font);
					point = new Point(Width - size.Width - mPadding.Right, pos);
					DrawOverlay(e, point, size, ColorScheme.PreviewRuler, 100);
					e.Graphics.DrawString(text, font, b, point.X, point.Y);
                }

                // loading
                lock (mGrbl3DLock)
                {
                    if (mMessage != null || mGrbl3D != null && mGrbl3D.LoadingPercentage < 100 || Core.ShowLaserOffMovements.Value && mGrbl3DOff != null && mGrbl3DOff.LoadingPercentage < 100)
                    {
                        int pos = point.Y + size.Height + 5;
                        double? perc;
                        perc = mGrbl3D?.LoadingPercentage ?? 0;
                        perc += mGrbl3DOff?.LoadingPercentage ?? 0;
                        if (Core.ShowLaserOffMovements.Value) perc /= 2;
                        text = mMessage == null ? $"{Strings.Loading} {perc:0.0}%" : mMessage;
                        size = MeasureText(text, font);
                        point = new Point(Width - size.Width - mPadding.Right, pos);
                        DrawOverlay(e, point, size, ColorScheme.PreviewRuler, 100);
                        e.Graphics.DrawString(text, font, b, point.X, point.Y);
                    }
                }

                if (mGrbl3D == null)
				{
					text = $"{Strings.TipsBasicUsage}\r\n{Strings.TipsZoom}\r\n{Strings.TipsPan}\r\n{Strings.TipsJog}\r\n\r\n";
                    string shortcut = "";
					string a_asd = GetShortcut(HotKeysManager.HotKey.Actions.AutoSizeDrawing);
					string a_zin = GetShortcut(HotKeysManager.HotKey.Actions.ZoomInDrawing);
					string a_zou = GetShortcut(HotKeysManager.HotKey.Actions.ZoomInDrawing);
					if (!string.IsNullOrEmpty(a_asd)) shortcut += $"{GetShortcut(HotKeysManager.HotKey.Actions.AutoSizeDrawing)}: {Strings.TipsZoomAuto}\r\n";
					if (!string.IsNullOrEmpty(a_zin)) shortcut += $"{GetShortcut(HotKeysManager.HotKey.Actions.ZoomInDrawing)}: {Strings.TipsZoomIn}\r\n";
					if (!string.IsNullOrEmpty(a_zou)) shortcut += $"{GetShortcut(HotKeysManager.HotKey.Actions.ZoomOutDrawing)}: {Strings.TipsZoomOut}\r\n";
					if (!string.IsNullOrEmpty(shortcut))
						text = text + $"{Strings.TipsKeyboardShortcuts}\r\n" + shortcut;
                    text = text.Trim("\r\n".ToCharArray());
                    size = MeasureText(text, font);
					point = new Point(Width - size.Width - mPadding.Right, Height - size.Height - 35);
					DrawOverlay(e, point, size, ColorScheme.PreviewRuler, 60);
					e.Graphics.DrawString(text, font, b, point.X, point.Y);
				}

				if (Core.CrossCursor.Value &&
					mMousePos != null &&
					mMousePos.Value.X >= mPadding.Left &&
					mMousePos.Value.X <= Width - mPadding.Right &&
					mMousePos.Value.Y >= mPadding.Top &&
					mMousePos.Value.Y <= Height - mPadding.Bottom)
                {
					Color loShadowColor = Color.FromArgb(
						128,
						ColorScheme.PreviewBackColor.R,
                        ColorScheme.PreviewBackColor.G,
                        ColorScheme.PreviewBackColor.B
                    );
                    using (Pen pCross = new Pen(loShadowColor))
                    {
                        DrawCross(e.Graphics, pCross, new Point(mMousePos.Value.X + 1, mMousePos.Value.Y + 1));
                    }
                    using (Pen pCross = new Pen(ColorScheme.PreviewCrossCursor))
                    {
						DrawCross(e.Graphics, pCross, mMousePos.Value);
                        ShowCursor = false;
                    }
                }
				else
				{
					ShowCursor = true;
				}

			}
		}

		private void DrawCross(Graphics g, Pen pCross, Point point)
        {
			const int halfCrossSize = 4;
            g.DrawLine(pCross, new Point(mPadding.Left, point.Y), new Point(point.X - 5, point.Y));
            g.DrawLine(pCross, new Point(point.X + halfCrossSize, point.Y), new Point(Width - mPadding.Right, point.Y));
            g.DrawLine(pCross, new Point(point.X, mPadding.Top), new Point(point.X, point.Y - halfCrossSize));
            g.DrawLine(pCross, new Point(point.X, point.Y + halfCrossSize), new Point(point.X, Height - mPadding.Bottom));
            g.DrawRectangle(pCross, point.X - halfCrossSize, point.Y - halfCrossSize, halfCrossSize * 2, halfCrossSize * 2);
        }

		private string GetShortcut(HotKeysManager.HotKey.Actions action)
		{
			string shortcut = Core.GetHotKeyString(action);
			if (shortcut != null)
			{
				shortcut = shortcut.Replace("+", " ");
				shortcut = shortcut.Replace("Add", "+");
				shortcut = shortcut.Replace("Subtract", "-");
			}
			return shortcut;
		}

		private void DrawOverlay(PaintEventArgs e, Point point, Size size, Color color, int alfa)
		{
			Color c = Color.FromArgb(alfa, color);
			using (Brush brush = new SolidBrush(c))
			using (GraphicsPath path = Tools.Graph.RoundedRect(new Rectangle(point.X - 5, point.Y - 5, size.Width, size.Height), 7))
			{
				e.Graphics.FillPath(brush, path);
			}
		}

		private Size MeasureText(string text, Font font)
		{
			Size size = TextRenderer.MeasureText(text, font);
			size.Width += 10;
			size.Height += 5;
			return size;
		}

		private void GrblSceneControl_MouseLeave(object sender, EventArgs e)
		{
			mLastMousePos = null;
			mMouseWorldPosition = null;
			mMousePos = null;
			if (Core?.CrossCursor.Value == true) ShowCursor = true;
			Cursor.Current = Cursors.Default;
		}

		private void GrblSceneControl_MouseUp(object sender, MouseEventArgs e)
		{
            if (Core?.CrossCursor.Value == true) ShowCursor = true;
            Cursor.Current = Cursors.Default;
			mLastMousePos = null;
			mMouseWorldPosition = null;
		}

		private void GrblSceneControl_MouseDown(object sender, MouseEventArgs e)
        {
            mMousePos = null;
            Cursor.Current = Cursors.Cross;
			mLastMousePos = e.Location;
			Invalidate();
        }

		private void GrblSceneControl_MouseMove(object sender, MouseEventArgs e)
		{
            // compute mouse coord
            mMousePos = e.Location;
            double xp = e.X / (double)Width * (mCamera.Right - mCamera.Left) + mCamera.Left;
			double yp = (Height - e.Y) / (double)Height * (mCamera.Top - mCamera.Bottom) + mCamera.Bottom;
			mMouseWorldPosition = new PointF((float)xp, (float)yp);
            // if in drag
            if (mLastMousePos != null && e.Button == MouseButtons.Left)
			{
				Cursor.Current = Cursors.SizeAll;
				Point lastPos = (Point)mLastMousePos;
				double k = (mCamera.Right - mCamera.Left) / Width;
				double dx = e.X - lastPos.X;
				double dy = e.Y - lastPos.Y;
				SetWorldPosition(mCamera.Left - dx * k, mCamera.Right - dx * k, mCamera.Bottom + dy * k, mCamera.Top + dy * k);
				mLastMousePos = e.Location;
				mMousePos = null;
			}
			Invalidate();
		}

		private void GrblSceneControl_MouseWheel(object sender, MouseEventArgs e)
		{
			if (mMouseWorldPosition != null)
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
		}

		public void SetCore(GrblCore core)
		{
			Core = core;
            Core.OnFileLoading += OnFileLoading;
			Core.OnFileLoaded += OnFileLoaded;
			Core.ShowExecutedCommands.OnChange += ShowExecutedCommands_OnChange;
			Core.PreviewLineSize.OnChange += PrerviewLineSize_OnChange;
			Core.ShowLaserOffMovements.OnChange += ShowLaserOffMovements_OnChange;
            Core.ShowBoundingBox.OnChange += ShowBoundingBox_OnChange;
			Core.OnAutoSizeDrawing += Core_OnAutoSizeDrawing;
			Core.OnZoomInDrawing += Core_OnZoomInDrawing;
			Core.OnZoomOutDrawing += Core_OnZoomOutDrawing;
			Core.OnProgramEnded += OnProgramEnded;
		}

        private void OnFileLoading(long elapsed, string filename)
        {
			mMessage = Strings.Loading;
        }

        private void OnProgramEnded()
		{
			foreach (var command in Core.LoadedFile.Commands)
			{
				command.ClearResult();
			}
		}

		private void Core_OnAutoSizeDrawing(GrblCore obj)
		{
			AutoSizeDrawing();
		}
		private void Core_OnZoomInDrawing(GrblCore obj)
		{
			ZoomIn();
		}

		private void Core_OnZoomOutDrawing(GrblCore obj)
		{
			ZoomOut();
		}

		private void PrerviewLineSize_OnChange(Tools.RetainedSetting<float> obj)
		{
			if (mGrbl3D != null) mGrbl3D.LineWidth = obj.Value;
			mInvalidateAll = true;
			RR.Set();
		}

		private void ShowLaserOffMovements_OnChange(RetainedSetting<bool> obj)
		{
			mInvalidateAll = true;
			RR.Set();
		}

        private void ShowBoundingBox_OnChange(RetainedSetting<bool> obj)
        {
            mInvalidateAll = true;
            RR.Set();
        }

        private void ShowExecutedCommands_OnChange(Tools.RetainedSetting<bool> obj)
		{
			mInvalidateAll = true;
			RR.Set();
		}

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
			double ratio = Width / (double)Height;
			double availableWidth = Width - mPadding.Left - mPadding.Right;
			double availableHeight = Height - mPadding.Bottom - mPadding.Top;
			const double growFactor = 1.1;
			const decimal defaultViewport = 100;

			XYRange autosizeRange;

			if (Core?.LoadedFile.Range.DrawingRange.ValidRange == true)
			{
				autosizeRange = Core.AutoSizeOnDrawing.Value ? Core.LoadedFile.Range.DrawingRange : Core.LoadedFile.Range.MovingRange;
			}
			else
			{
				autosizeRange = new XYRange();
				autosizeRange.X.Min = -defaultViewport;
				autosizeRange.X.Max = defaultViewport;
				autosizeRange.Y.Min = -defaultViewport;
				autosizeRange.Y.Max = defaultViewport;
			}

			double drawingWidth = (double)autosizeRange.Width * growFactor;
			double drawingHeight = (double)autosizeRange.Height * growFactor;

			double widthRatio = drawingWidth / availableWidth;
			double paddingLeftWorld = mPadding.Left * widthRatio;
			double paddingRightWorld = mPadding.Right * widthRatio;

			double heightRatio = drawingHeight / availableHeight;
			double paddingBottomWorld = mPadding.Bottom * heightRatio;
			double paddingTopWorld = mPadding.Top * heightRatio;

			double left;
			double right;
			double bottom;
			double top;

			if (drawingWidth / drawingHeight > availableWidth / availableHeight)
			{
				left = (double)autosizeRange.Center.X - drawingWidth / 2;
				right = (double)autosizeRange.Center.X + drawingWidth / 2;

				left -= paddingLeftWorld;
				right += paddingRightWorld;

				double resizedHeight = (right - left) / ratio;
				double centerY = autosizeRange.Center.Y;

				bottom = centerY - resizedHeight / 2;
				top = centerY + resizedHeight / 2;

				double bottomShift = (top - bottom) / Height * mPadding.Bottom / 2;
				bottom -= bottomShift;
				top -= bottomShift;
			}
			else
			{
				bottom = (double)autosizeRange.Center.Y - drawingHeight / 2;
				top = (double)autosizeRange.Center.Y + drawingHeight / 2;

				bottom -= paddingBottomWorld;
				top += paddingTopWorld;

				double resizedWidth = (top - bottom) * ratio;
				double centerX = autosizeRange.Center.X;

				left = centerX - resizedWidth / 2;
				right = centerX + resizedWidth / 2;

				double leftShift = (right - left) / Width * mPadding.Left / 2;
				left -= leftShift;
				right -= leftShift;
			}

			SetWorldPosition(
				left,
				right,
				bottom,
				top
			);
		}

		PeriodicEventTimer InvalidateTimer = new Tools.PeriodicEventTimer(TimeSpan.FromSeconds(1), true);
		public void TimerUpdate()
		{
			if (Core != null && (mLastWPos != Core.WorkPosition || mLastMPos != Core.MachinePosition || mCurF != Core.CurrentF || mCurS != Core.CurrentS))
			{
				mLastWPos = Core.WorkPosition;
				mLastMPos = Core.MachinePosition;
				mCurF = Core.CurrentF;
				mCurS = Core.CurrentS;
				RR.Set();
			}

			if (InvalidateTimer.Expired)
				Invalidate();
		}

		private void Grbl3D_OnLoadingPercentageChange()
		{
			Invalidate();
		}

		public void OnColorChange()
		{
			mBackgroundColor = ColorScheme.PreviewBackColor;
			mTextColor = ColorScheme.PreviewText;
			mTextBoundingColor = ColorScheme.PreviewJobRange;
			mOriginsColor = ColorScheme.PreviewRuler;
			mPointerColor = ColorScheme.PreviewCross;
			mTicksColor = ColorScheme.PreviewGrid;
			mMinorsColor = ColorScheme.PreviewGridMinor;
			BackColor = mBackgroundColor;
			mInvalidateAll = true;
			RR.Set();
		}

		private void GrblPanel3D_Load(object sender, EventArgs e)
		{
			Load -= GrblPanel3D_Load;
			TimDetectIssue.Start();
		}

		private void TimDetectIssue_Tick(object sender, EventArgs e)
		{
			TimDetectIssue.Stop();
			if (OpCounter < 5 || FatalException != null) //non è riuscito a completare nemmeno un disegno nei 5 secondi del timer! (o se c'è stata una eccezione nei primi 5 secondi)
			{
				if (FatalException == null) //fatal exception are logged where they are generated
					Logger.LogMessage("OpenGL", "Rendering issue detected! OpCounte: {0}", OpCounter);

				if (Settings.CurrentGraphicMode == Settings.GraphicMode.FBO && MessageBox.Show(Strings.FBOIssueSuggestDBO, Strings.FirmwareRequireRestart, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					Settings.ConfiguredGraphicMode = Settings.GraphicMode.DIB;
					UsageStats.DoNotSendNow = true;
					Application2.RestartNoCommandLine();
				}
				else if (Settings.CurrentGraphicMode == Settings.GraphicMode.DIB && MessageBox.Show(Strings.DBOIssueSuggestGDI, Strings.FirmwareRequireRestart, MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					Settings.ConfiguredGraphicMode = Settings.GraphicMode.GDI;
					UsageStats.DoNotSendNow = true;
					Application2.RestartNoCommandLine();
				}
			}
		}

		internal void ZoomIn()
		{
			CenterZoom(1.0 / 1.1);
		}

		internal void ZoomOut()
		{
			CenterZoom(1.1);
		}

		private void CenterZoom(double k)
		{
			// Calculate the center of the current view
			double centerX = (mCamera.Left + mCamera.Right) / 2;
			double centerY = (mCamera.Bottom + mCamera.Top) / 2;

			// Calculate the new half-width and half-height based on the zoom factor
			double halfWidth = (mCamera.Right - mCamera.Left) / 2 ;
			double halfHeight = (mCamera.Top - mCamera.Bottom) / 2 ;

			halfWidth = halfWidth * k;
			halfHeight = halfHeight * k;

			// Update the boundaries
			double left = centerX - halfWidth;
			double right = centerX + halfWidth;
			double bottom = centerY - halfHeight;
			double top = centerY + halfHeight;

			SetWorldPosition(left, right, bottom, top);
		}
	}
}