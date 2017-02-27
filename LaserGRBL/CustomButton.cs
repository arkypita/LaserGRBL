using System;
using System.Collections.Generic;
using System.Text;

namespace LaserGRBL
{
	[Serializable]
	class CustomButton
	{
		public System.Guid guid = Guid.NewGuid();
		public System.Drawing.Image Image;
		public string GCode;
		public string ToolTip;
	}
}
