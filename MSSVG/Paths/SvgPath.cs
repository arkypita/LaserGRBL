using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics;
using Svg.ExtensionMethods;
using Svg.Pathing;
using Svg.Transforms;

namespace Svg
{
    /// <summary>
    /// Represents an SVG path element.
    /// </summary>
    [SvgElement("path")]
    public class SvgPath : SvgVisualElement
    {
        private GraphicsPath _path;

        /// <summary>
        /// Gets or sets a <see cref="SvgPathSegmentList"/> of path data.
        /// </summary>
        [SvgAttribute("d", true)]
        public SvgPathSegmentList PathData
        {
        	get { return this.Attributes.GetAttribute<SvgPathSegmentList>("d"); }
            set
            {
            	this.Attributes["d"] = value;
            	value._owner = this;
                this.IsPathDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the length of the path.
        /// </summary>
        [SvgAttribute("pathLength", true)]
        public float PathLength
        {
            get { return this.Attributes.GetAttribute<float>("pathLength"); }
            set { this.Attributes["pathLength"] = value; }
        }

		
        /// <summary>
        /// Gets or sets the marker (end cap) of the path.
        /// </summary>
        [SvgAttribute("marker-end", true)]
		public Uri MarkerEnd
        {
			get { return this.Attributes.GetAttribute<Uri>("marker-end").ReplaceWithNullIfNone(); }
			set { this.Attributes["marker-end"] = value; }
		}


		/// <summary>
		/// Gets or sets the marker (start cap) of the path.
		/// </summary>
        [SvgAttribute("marker-mid", true)]
		public Uri MarkerMid
		{
			get { return this.Attributes.GetAttribute<Uri>("marker-mid").ReplaceWithNullIfNone(); }
			set { this.Attributes["marker-mid"] = value; }
		}


		/// <summary>
		/// Gets or sets the marker (start cap) of the path.
		/// </summary>
        [SvgAttribute("marker-start", true)]
		public Uri MarkerStart
		{
			get { return this.Attributes.GetAttribute<Uri>("marker-start").ReplaceWithNullIfNone(); }
			set { this.Attributes["marker-start"] = value; }
		}


        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path(ISvgRenderer renderer)
        {
            if (this._path == null || this.IsPathDirty)
            {
                _path = new GraphicsPath();

                foreach (SvgPathSegment segment in this.PathData)
                {
                    segment.AddToPath(_path);
                }

                this.IsPathDirty = false;
            }
            return _path;
        }

        internal void OnPathUpdated()
        {
            this.IsPathDirty = true;
            OnAttributeChanged(new AttributeEventArgs{ Attribute = "d", Value = this.Attributes.GetAttribute<SvgPathSegmentList>("d") });
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override System.Drawing.RectangleF Bounds
        {
            get { return this.Path(null).GetBounds(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgPath"/> class.
        /// </summary>
        public SvgPath()
        {
            var pathData = new SvgPathSegmentList();
            this.Attributes["d"] = pathData;
            pathData._owner = this;
        }

		/// <summary>
		/// Renders the stroke of the <see cref="SvgVisualElement"/> to the specified <see cref="ISvgRenderer"/>
		/// </summary>
		/// <param name="renderer">The <see cref="ISvgRenderer"/> object to render to.</param>
		protected internal override bool RenderStroke(ISvgRenderer renderer)
		{
            var result = base.RenderStroke(renderer);
            var path = this.Path(renderer);

            if (this.MarkerStart != null)
            {
                SvgMarker marker = this.OwnerDocument.GetElementById<SvgMarker>(this.MarkerStart.ToString());
                marker.RenderMarker(renderer, this, path.PathPoints[0], path.PathPoints[0], path.PathPoints[1]);
            }

            if (this.MarkerMid != null)
            {
                SvgMarker marker = this.OwnerDocument.GetElementById<SvgMarker>(this.MarkerMid.ToString());
                for (int i = 1; i <= path.PathPoints.Length - 2; i++)
                    marker.RenderMarker(renderer, this, path.PathPoints[i], path.PathPoints[i - 1], path.PathPoints[i], path.PathPoints[i + 1]);
            }

            if (this.MarkerEnd != null)
            {
                SvgMarker marker = this.OwnerDocument.GetElementById<SvgMarker>(this.MarkerEnd.ToString());
                marker.RenderMarker(renderer, this, path.PathPoints[path.PathPoints.Length - 1], path.PathPoints[path.PathPoints.Length - 2], path.PathPoints[path.PathPoints.Length - 1]);
            }
                
            return result;
		}

		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgPath>();
		}

		public override SvgElement DeepCopy<T>()
		{
			var newObj = base.DeepCopy<T>() as SvgPath;
			foreach (var pathData in this.PathData)
				newObj.PathData.Add(pathData.Clone());
			newObj.PathLength = this.PathLength;
			newObj.MarkerStart = this.MarkerStart;
			newObj.MarkerEnd = this.MarkerEnd;
			return newObj;

		}
    }
}