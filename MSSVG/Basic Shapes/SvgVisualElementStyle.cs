using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Svg.DataTypes;
using System.Text.RegularExpressions;
using System.Linq;

namespace Svg
{
    public abstract partial class SvgVisualElement
    {
        /// <summary>
        /// Gets or sets a value to determine whether the element will be rendered.
        /// </summary>
        [TypeConverter(typeof(SvgBoolConverter))]
        [SvgAttribute("visibility")]
        public virtual bool Visible
        {
            get { return (this.Attributes["visibility"] == null) ? true : (bool)this.Attributes["visibility"]; }
            set { this.Attributes["visibility"] = value; }
        }

        /// <summary>
        /// Gets or sets a value to determine whether the element will be rendered.
        /// Needed to support SVG attribute display="none"
        /// </summary>
        [SvgAttribute("display")]
        public virtual string Display
        {
            get { return this.Attributes["display"] as string; }
            set { this.Attributes["display"] = value; }
        }

        // Displayable - false if attribute display="none", true otherwise
        protected virtual bool Displayable
        {
            get
            {
                string checkForDisplayNone = this.Attributes["display"] as string;
                if ((!string.IsNullOrEmpty(checkForDisplayNone)) && (checkForDisplayNone == "none"))
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Gets or sets the fill <see cref="SvgPaintServer"/> of this element.
        /// </summary>
        [SvgAttribute("enable-background")]
        public virtual string EnableBackground
        {
            get { return this.Attributes["enable-background"] as string; }
            set { this.Attributes["enable-background"] = value; }
        }

    }
}