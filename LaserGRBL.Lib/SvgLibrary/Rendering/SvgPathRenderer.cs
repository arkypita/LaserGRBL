using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Svg
{
	/// <summary>
	/// Convenience wrapper around a graphics object
	/// </summary>
	public sealed class SvgPathRenderer : IDisposable, IGraphicsProvider, ISvgRenderer
	{
		private Matrix _innerMatrix;
		private GraphicsPath _innerPath;
		private Stack<ISvgBoundable> _boundables = new Stack<ISvgBoundable>();

		public void SetBoundable(ISvgBoundable boundable)
		{
			_boundables.Push(boundable);
		}
		public ISvgBoundable GetBoundable()
		{
			return _boundables.Peek();
		}
		public ISvgBoundable PopBoundable()
		{
			return _boundables.Pop();
		}

		public float DpiY
		{
			get { return 90; /*_innerPath.DpiY;*/ }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ISvgRenderer"/> class.
		/// </summary>
		private SvgPathRenderer(GraphicsPath path)
		{
			_innerPath = path;
			_innerMatrix = new Matrix();
		}

		public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit graphicsUnit)
		{
			//_innerPath.DrawImage(image, destRect, srcRect, graphicsUnit);
		}
		public void DrawImageUnscaled(Image image, Point location)
		{
			//this._innerPath.DrawImageUnscaled(image, location);
		}
		public void DrawPath(Pen pen, GraphicsPath path)
		{
			this._innerPath.AddPath(path, false);
		}
		public void FillPath(Brush brush, GraphicsPath path)
		{
			this._innerPath.AddPath(path, false);
		}
		public Region GetClip()
		{
			return new Region(_innerPath) /*this._innerPath.Clip*/;
		}
		public void RotateTransform(float fAngle, MatrixOrder order = MatrixOrder.Append)
		{
			_innerMatrix.Rotate(fAngle, order);
		}
		public void ScaleTransform(float sx, float sy, MatrixOrder order = MatrixOrder.Append)
		{
			_innerMatrix.Scale(sx, sy, order);
		}
		public void SetClip(Region region, CombineMode combineMode = CombineMode.Replace)
		{
			//this._innerPath.SetClip(region, combineMode);
		}
		public void TranslateTransform(float dx, float dy, MatrixOrder order = MatrixOrder.Append)
		{
			this._innerMatrix.Translate(dx, dy, order);
		}



		public SmoothingMode SmoothingMode
		{
			get { return SmoothingMode.AntiAlias; }
			set { /*this._innerPath.SmoothingMode = value;*/ }
		}

		public Matrix Transform
		{
			get { return this._innerMatrix; }
			set { /*this._innerPath.Transform = value;*/ }
		}

		public bool Wireframe { get ; set ; }

		public void Dispose()
		{
			this._innerPath.Dispose();
		}

		Graphics IGraphicsProvider.GetGraphics()
		{
			return null;
		}

		/// <summary>
		/// Creates a new <see cref="ISvgRenderer"/> from the specified <see cref="GraphicsPath"/>.
		/// </summary>
		/// <param name="path">The <see cref="GraphicsPath"/> to create the renderer from.</param>
		public static ISvgRenderer FromPath(GraphicsPath path)
		{
			return new SvgPathRenderer(path);
		}

		public static ISvgRenderer FromNull()
		{
			GraphicsPath path = new GraphicsPath();
			return SvgPathRenderer.FromPath(path);
		}
	}
}