using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.UI.Forms.RasterConverter
{
	public partial class SetupDithering : ToolConfig
	{
		private Core.RasterToGcode.Dithering mTool;

		public SetupDithering(Core.RasterToGcode.Dithering tool)
		{
			InitializeComponent();
			mTool = tool;

			CbDither.SuspendLayout();
			foreach (Core.RasterToGcode.ImageTransform.DitheringMode formula in Enum.GetValues(typeof(Core.RasterToGcode.ImageTransform.DitheringMode)))
				CbDither.Items.Add(formula);
			CbDither.SelectedIndex = 0;
			CbDither.ResumeLayout();
			CbDither.SuspendLayout();

			CbDirections.SuspendLayout();
			foreach (Core.RasterToGcode.ConversionTool.EngravingDirection direction in Enum.GetValues(typeof(Core.RasterToGcode.ConversionTool.EngravingDirection)))
				if (direction != Core.RasterToGcode.ConversionTool.EngravingDirection.None)
					CbDirections.AddItem(direction);
			CbDirections.SelectedIndex = 0;
			CbDirections.ResumeLayout();
		}

		private void CbDither_SelectedIndexChanged(object sender, EventArgs e)
		{
			mTool.Mode = (Core.RasterToGcode.ImageTransform.DitheringMode)CbDither.SelectedItem;
			Change();
		}

		private void UDQuality_ValueChanged(object sender, EventArgs e)
		{
			mTool.Quality = (double)UDQuality.Value;
		}

		void CbDirectionsSelectedIndexChanged(object sender, EventArgs e)
		{
			mTool.Direction = (Core.RasterToGcode.ConversionTool.EngravingDirection)CbDirections.SelectedItem;
			Change();
		}

		private void BtnQualityInfo_Click(object sender, EventArgs e)
		{
			UDQuality.Value = (decimal)ResolutionHelperForm.CreateAndShowDialog((double)UDQuality.Value);
			//System.Diagnostics.Process.Start(@"http://lasergrbl.com/usage/raster-image-import/setting-reliable-resolution/");
		}

	}
}
