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
	public partial class SetupVectorization : ToolConfig
	{
		private Core.RasterToGcode.Vectorization mTool;

		public SetupVectorization(Core.RasterToGcode.Vectorization tool)
		{
			InitializeComponent();
			mTool = tool;

			CbFillingDirection.SuspendLayout();
			foreach (Core.RasterToGcode.ConversionTool.EngravingDirection direction in Enum.GetValues(typeof(Core.RasterToGcode.ConversionTool.EngravingDirection)))
				CbFillingDirection.AddItem(direction);
			CbFillingDirection.SelectedIndex = 0;
			CbFillingDirection.ResumeLayout();
		}

		private void UDSmoothing_ValueChanged(object sender, EventArgs e)
		{
			mTool.Smoothing.Value = UDSmoothing.Value;
			Change();
		}

		private void CbSmoothing_CheckedChanged(object sender, EventArgs e)
		{
			mTool.Smoothing.Enabled = CbSmoothing.Checked;
			UDSmoothing.Enabled = CbSmoothing.Checked;
			Change();
		}

		private void UDOptimize_ValueChanged(object sender, EventArgs e)
		{
			mTool.Optimize.Value = UDOptimize.Value;
			Change();
		}

		private void CbOptimize_CheckedChanged(object sender, EventArgs e)
		{
			mTool.Optimize.Enabled = CbOptimize.Checked;
			UDOptimize.Enabled = CbOptimize.Checked;
			Change();
		}

		private void CbDownSample_CheckedChanged(object sender, EventArgs e)
		{
			mTool.DownSampling.Enabled = CbDownSample.Checked;
			UDDownSample.Enabled = CbDownSample.Checked;
			Change();
		}

		private void UDDownSample_ValueChanged(object sender, EventArgs e)
		{
			mTool.DownSampling.Value = UDDownSample.Value;
			Change();
		}

		private void UDSpotRemoval_ValueChanged(object sender, EventArgs e)
		{
			mTool.SpotRemoval.Value = UDSpotRemoval.Value;
			Change();
		}

		private void CbSpotRemoval_CheckedChanged(object sender, EventArgs e)
		{
			mTool.SpotRemoval.Enabled = CbSpotRemoval.Checked;
			UDSpotRemoval.Enabled = CbSpotRemoval.Checked;
			Change();
		}

		private void CbFillingDirection_SelectedIndexChanged(object sender, EventArgs e)
		{
			mTool.Direction = (Core.RasterToGcode.ConversionTool.EngravingDirection)CbFillingDirection.SelectedItem;
			BtnFillingQualityInfo.Visible = LblFillingLineLbl.Visible = LblFillingQuality.Visible = UDFillingQuality.Visible = ((Core.RasterToGcode.ConversionTool.EngravingDirection)CbFillingDirection.SelectedItem != Core.RasterToGcode.ConversionTool.EngravingDirection.None);
			Change();
		}

		private void UDFillingQuality_ValueChanged(object sender, EventArgs e)
		{
			mTool.Quality = (double)UDFillingQuality.Value;
		}

		private void BtnFillingQualityInfo_Click(object sender, EventArgs e)
		{
			UDFillingQuality.Value = (decimal)ResolutionHelperForm.CreateAndShowDialog((double)UDFillingQuality.Value);
			//System.Diagnostics.Process.Start(@"http://lasergrbl.com/usage/raster-image-import/setting-reliable-resolution/");
		}
	}
}
