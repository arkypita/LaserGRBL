
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;


namespace LaserGRBL.UserControls
{

	public class ColorProgressBarDesigner : System.Windows.Forms.Design.ControlDesigner
	{

		public ColorProgressBarDesigner()
		{
		}
		//New


		// clean up some unnecessary properties
		protected override void PostFilterProperties(IDictionary Properties)
		{
			Properties.Remove("AllowDrop");
			Properties.Remove("BackgroundImage");
			Properties.Remove("ContextMenu");
			Properties.Remove("FlatStyle");
			Properties.Remove("Image");
			Properties.Remove("ImageAlign");
			Properties.Remove("ImageIndex");
			Properties.Remove("ImageList");
			Properties.Remove("Text");
			Properties.Remove("TextAlign");
		}
		//PostFilterProperties
	}
	//ColorProgressBarDesigner
}
