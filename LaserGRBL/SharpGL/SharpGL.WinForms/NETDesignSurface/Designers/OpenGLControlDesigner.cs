using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGL.WinForms.NETDesignSurface.Designers
{
	/// <summary>
	/// This aids the design of the OpenGLCtrl
	/// </summary>
	public class OpenGLCtrlDesigner : System.Windows.Forms.Design.ControlDesigner
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OpenGLCtrlDesigner"/> class.
		/// </summary>
		public OpenGLCtrlDesigner() { }

        /// <summary>
        /// Remove Control properties that are not supported by the control.
        /// </summary>
        /// <param name="Properties"></param>
        protected override void PostFilterProperties(IDictionary Properties)
		{
			//	Appearance
			Properties.Remove("BackColor");
			Properties.Remove("BackgroundImage");
			Properties.Remove("Font");
			Properties.Remove("ForeColor");
			Properties.Remove("RightToLeft");

			//	Behaviour
			Properties.Remove("AllowDrop");
			Properties.Remove("ContextMenu");

			//	Layout
			Properties.Remove("AutoScroll");
			Properties.Remove("AutoScrollMargin");
			Properties.Remove("AutoScrollMinSize");
		}
	}
}
