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
	public partial class SetupGrayscale : UserControl
	{
		public event EventHandler ValueChanged; 

		public SetupGrayscale()
		{
			InitializeComponent();
		}

		private LaserGRBL.Core.RasterToGcode.ColorToGrayscale ColorToGrayscale;

		public void Start(LaserGRBL.Core.RasterToGcode.Configuration setup, bool grayscale)
		{
			ColorToGrayscale = setup.ColorToGrayscale;

			CbMode.SuspendLayout();
			foreach (Core.RasterToGcode.ImageTransform.Formula formula in Enum.GetValues(typeof(Core.RasterToGcode.ImageTransform.Formula)))
				CbMode.AddItem(formula);
			CbMode.SelectedIndex = 0;
			CbMode.ResumeLayout();

			LblGrayscale.Visible = CbMode.Visible = !grayscale;
		}

		private void CbMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			SuspendLayout();
			ColorToGrayscale.Formula = (Core.RasterToGcode.ImageTransform.Formula)CbMode.SelectedItem;
			TBRed.Visible = TBGreen.Visible = TBBlue.Visible = (ColorToGrayscale.Formula == Core.RasterToGcode.ImageTransform.Formula.Custom);
			LblRed.Visible = LblGreen.Visible = LblBlue.Visible = (ColorToGrayscale.Formula == Core.RasterToGcode.ImageTransform.Formula.Custom);
			ResumeLayout();
			Change();
		}

		private void TBRed_ValueChanged(object sender, EventArgs e)
		{
			ColorToGrayscale.Red = (byte)TBRed.Value;
			Change();
		}


		private void TBGreen_ValueChanged(object sender, EventArgs e)
		{
			ColorToGrayscale.Green = (byte)TBGreen.Value;
			Change();
		}

		private void TBBlue_ValueChanged(object sender, EventArgs e)
		{
			ColorToGrayscale.Blue = (byte)TBBlue.Value;
			Change();
		}


		private void TbBright_ValueChanged(object sender, EventArgs e)
		{
			ColorToGrayscale.Brightness = (byte)TbBright.Value;
			Change();
		}

		private void TbContrast_ValueChanged(object sender, EventArgs e)
		{
			ColorToGrayscale.Contrast = (byte)TbContrast.Value;
			Change();
		}

		private void CbThreshold_CheckedChanged(object sender, EventArgs e)
		{
			ColorToGrayscale.Threshold.Enabled = CbThreshold.Checked;
			TbThreshold.Enabled = ColorToGrayscale.Threshold.Enabled;
			Change();
		}

		private void TbThreshold_ValueChanged(object sender, EventArgs e)
		{
			ColorToGrayscale.Threshold.Value = (byte)TbThreshold.Value;
			Change();
		}

		private void TBWhiteClip_ValueChanged(object sender, EventArgs e)
		{
			ColorToGrayscale.WhitePoint = (byte)TBWhiteClip.Value;
			Change();
		}

		private void TBWhiteClip_MouseDown(object sender, MouseEventArgs e)
		{
			ClipPreview = true;
			Change();
		}

		private void TBWhiteClip_MouseUp(object sender, MouseEventArgs e)
		{
			ClipPreview = false;
			Change();
		}

		void OnRGBCBDoubleClick(object sender, EventArgs e)
		{ ((UserControls.ColorSlider)sender).Value = 100; }

		void OnThresholdDoubleClick(object sender, EventArgs e)
		{ ((UserControls.ColorSlider)sender).Value = 50; }

		private void Change(bool clipPreview = false)
		{
			if (ValueChanged != null)
				ValueChanged(this, new EventArgs());
		}

		public bool ClipPreview { get; set; }
	}
}
