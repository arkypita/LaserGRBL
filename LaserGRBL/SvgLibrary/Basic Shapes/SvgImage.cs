using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using Svg.Transforms;

namespace Svg
{
    /// <summary>
    /// Represents and SVG image
    /// </summary>
    [SvgElement("image")]
    public class SvgImage : SvgVisualElement
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="SvgImage"/> class.
        /// </summary>
		public SvgImage()
        {
            Width = new SvgUnit(0.0f);
            Height = new SvgUnit(0.0f);
        }

        private GraphicsPath _path;

        /// <summary>
        /// Gets an <see cref="SvgPoint"/> representing the top left point of the rectangle.
        /// </summary>
        public SvgPoint Location
        {
            get { return new SvgPoint(X, Y); }
        }

        /// <summary>
        /// Gets or sets the aspect of the viewport.
        /// </summary>
        /// <value></value>
        [SvgAttribute("preserveAspectRatio")]
        public SvgAspectRatio AspectRatio
        {
            get { return this.Attributes.GetAttribute<SvgAspectRatio>("preserveAspectRatio"); }
            set { this.Attributes["preserveAspectRatio"] = value; }
        }

		[SvgAttribute("x")]
		public virtual SvgUnit X
		{
			get { return this.Attributes.GetAttribute<SvgUnit>("x"); }
			set { this.Attributes["x"] = value; }
		}

		[SvgAttribute("y")]
		public virtual SvgUnit Y
		{
			get { return this.Attributes.GetAttribute<SvgUnit>("y"); }
			set { this.Attributes["y"] = value; }
		}


		[SvgAttribute("width")]
		public virtual SvgUnit Width
		{
			get { return this.Attributes.GetAttribute<SvgUnit>("width"); }
			set { this.Attributes["width"] = value; }
		}

		[SvgAttribute("height")]
		public virtual SvgUnit Height
		{
			get { return this.Attributes.GetAttribute<SvgUnit>("height"); }
			set { this.Attributes["height"] = value; }
		}

		[SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
		public virtual string Href
		{
			get { return this.Attributes.GetAttribute<string>("href"); }
			set { this.Attributes["href"] = value; }
		}



        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override RectangleF Bounds
        {
			get { return new RectangleF(this.Location.ToDeviceValue(null, this),
                                        new SizeF(this.Width.ToDeviceValue(null, UnitRenderingType.Horizontal, this),
                                                  this.Height.ToDeviceValue(null, UnitRenderingType.Vertical, this))); }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
          if (_path == null)
          {
            // Same size of rectangle can suffice to provide bounds of the image
            var rectangle = new RectangleF(Location.ToDeviceValue(renderer, this),
                SvgUnit.GetDeviceSize(Width, Height, renderer, this));

            _path = new GraphicsPath();
            _path.StartFigure();
            _path.AddRectangle(rectangle);
            _path.CloseFigure();
          }

          return _path;
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        protected override void Render(ISvgRenderer renderer)
        {
            if (!Visible || !Displayable)
                return;

            if (Width.Value > 0.0f && Height.Value > 0.0f && this.Href != null)
            {
                var img = GetImage();
                if (img != null)
                {
                    RectangleF srcRect;
                    var bmp = img as Image;
                    var svg = img as SvgFragment;
                    if (bmp != null)
                    {
                        srcRect = new RectangleF(0, 0, bmp.Width, bmp.Height);
                    }
                    else if (svg != null)
                    {
                        srcRect = new RectangleF(new PointF(0, 0), svg.GetDimensions());
                    }
                    else
                    {
                        return;
                    }

                    var destClip = new RectangleF(this.Location.ToDeviceValue(renderer, this),
                                                  new SizeF(Width.ToDeviceValue(renderer, UnitRenderingType.Horizontal, this),
                                                            Height.ToDeviceValue(renderer, UnitRenderingType.Vertical, this)));
                    RectangleF destRect = destClip;

                    this.PushTransforms(renderer);
                    renderer.SetClip(new Region(destClip), CombineMode.Intersect);
                    this.SetClip(renderer);

                    if (AspectRatio != null && AspectRatio.Align != SvgPreserveAspectRatio.none)
                    {
                        var fScaleX = destClip.Width / srcRect.Width;
                        var fScaleY = destClip.Height / srcRect.Height;
                        var xOffset = 0.0f;
                        var yOffset = 0.0f;

                        if (AspectRatio.Slice)
                        {
                            fScaleX = Math.Max(fScaleX, fScaleY);
                            fScaleY = Math.Max(fScaleX, fScaleY);
                        }
                        else
                        {
                            fScaleX = Math.Min(fScaleX, fScaleY);
                            fScaleY = Math.Min(fScaleX, fScaleY);
                        }

                        switch (AspectRatio.Align)
                        {
                            case SvgPreserveAspectRatio.xMinYMin:
                                break;
                            case SvgPreserveAspectRatio.xMidYMin:
                                xOffset = (destClip.Width - srcRect.Width * fScaleX) / 2;
                                break;
                            case SvgPreserveAspectRatio.xMaxYMin:
                                xOffset = (destClip.Width - srcRect.Width * fScaleX);
                                break;
                            case SvgPreserveAspectRatio.xMinYMid:
                                yOffset = (destClip.Height - srcRect.Height * fScaleY) / 2;
                                break;
                            case SvgPreserveAspectRatio.xMidYMid:
                                xOffset = (destClip.Width - srcRect.Width * fScaleX) / 2;
                                yOffset = (destClip.Height - srcRect.Height * fScaleY) / 2;
                                break;
                            case SvgPreserveAspectRatio.xMaxYMid:
                                xOffset = (destClip.Width - srcRect.Width * fScaleX);
                                yOffset = (destClip.Height - srcRect.Height * fScaleY) / 2;
                                break;
                            case SvgPreserveAspectRatio.xMinYMax:
                                yOffset = (destClip.Height - srcRect.Height * fScaleY);
                                break;
                            case SvgPreserveAspectRatio.xMidYMax:
                                xOffset = (destClip.Width - srcRect.Width * fScaleX) / 2;
                                yOffset = (destClip.Height - srcRect.Height * fScaleY);
                                break;
                            case SvgPreserveAspectRatio.xMaxYMax:
                                xOffset = (destClip.Width - srcRect.Width * fScaleX);
                                yOffset = (destClip.Height - srcRect.Height * fScaleY);
                                break;
                        }

                        destRect = new RectangleF(destClip.X + xOffset, destClip.Y + yOffset,
                                                    srcRect.Width * fScaleX, srcRect.Height * fScaleY);
                    }

                    if (bmp != null)
                    {
                        renderer.DrawImage(bmp, destRect, srcRect, GraphicsUnit.Pixel);
                        bmp.Dispose();
                    }
                    else if (svg != null)
                    {
                        var currOffset = new PointF(renderer.Transform.OffsetX, renderer.Transform.OffsetY);
                        renderer.TranslateTransform(-currOffset.X, -currOffset.Y);
                        renderer.ScaleTransform(destRect.Width / srcRect.Width, destRect.Height / srcRect.Height);
                        renderer.TranslateTransform(currOffset.X + destRect.X, currOffset.Y + destRect.Y);
                        renderer.SetBoundable(new GenericBoundable(srcRect));
                        svg.RenderElement(renderer);
                        renderer.PopBoundable();
                    }


                    this.ResetClip(renderer);
                    this.PopTransforms(renderer);
                }
                // TODO: cache images... will need a shared context for this
                // TODO: support preserveAspectRatio, etc
            }
        }

        public object GetImage()
        {
            return this.GetImage(this.Href);
        }

        public object GetImage(string uriString)
        {
            string safeUriString;
            if (uriString.Length > 65519)
            {
                //Uri MaxLength is 65519 (https://msdn.microsoft.com/en-us/library/z6c2z492.aspx)
                safeUriString = uriString.Substring(0, 65519);
            }
            else
            {
                safeUriString = uriString;
            }

            try
            {
                var uri = new Uri(safeUriString, UriKind.RelativeOrAbsolute);

                // handle data/uri embedded images (http://en.wikipedia.org/wiki/Data_URI_scheme)
                if (uri.IsAbsoluteUri && uri.Scheme == "data")
                {
                    int dataIdx = uriString.IndexOf(",") + 1;
                    if (dataIdx <= 0 || dataIdx + 1 > uriString.Length)
                        throw new Exception("Invalid data URI");

                    // we're assuming base64, as ascii encoding would be *highly* unsusual for images
                    // also assuming it's png or jpeg mimetype
                    byte[] imageBytes = Convert.FromBase64String(uriString.Substring(dataIdx));
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        return Image.FromStream(stream);
                    }
                }

                if (!uri.IsAbsoluteUri)
                {
                    uri = new Uri(OwnerDocument.BaseUri, uri);
                }

                // should work with http: and file: protocol urls
                var httpRequest = WebRequest.Create(uri);

                using (WebResponse webResponse = httpRequest.GetResponse())
                {
                    using (var stream = webResponse.GetResponseStream())
                    {
                        if (stream.CanSeek)
                        {
                            stream.Position = 0;
                        }
                        if (uri.LocalPath.EndsWith(".svg", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var doc = SvgDocument.Open<SvgDocument>(stream);
                            doc.BaseUri = uri;
                            return doc;
                        }
                        else
                        {
                            return Bitmap.FromStream(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error loading image: '{0}', error: {1} ", uriString, ex.Message);
                return null;
            }
        }

        protected static MemoryStream BufferToMemoryStream(Stream input)
        {
            byte[] buffer = new byte[4 * 1024];
            int len;
            MemoryStream ms = new MemoryStream();
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, len);
            }
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgImage>();
		}

		public override SvgElement DeepCopy<T>()
		{
 			var newObj = base.DeepCopy<T>() as SvgImage;
			newObj.Height = this.Height;
			newObj.Width = this.Width;
			newObj.X = this.X;
			newObj.Y = this.Y;
			newObj.Href = this.Href;
			return newObj;
		}
    }
}