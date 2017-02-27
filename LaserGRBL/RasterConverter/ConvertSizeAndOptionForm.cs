/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 15/01/2017
 * Time: 12:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL.RasterConverter
{
	/// <summary>
	/// Description of ConvertSizeAndOptionForm.
	/// </summary>
	public partial class ConvertSizeAndOptionForm : Form
	{
		public ConvertSizeAndOptionForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		ImageProcessor IP;
		
		public void ShowDialog(ImageProcessor processor)
		{
			IP = processor;
			
			
			if (IP.Original.Height < IP.Original.Width)
			{
				IISizeW.CurrentValue = 100;
				IISizeH.CurrentValue = IP.WidthToHeight(100);
			}
			else
			{
				IISizeH.CurrentValue = 100;
				IISizeW.CurrentValue = IP.HeightToWidht(100);
			}
		

			IIBorderTracing.CurrentValue = IP.BorderSpeed = (int)Settings.GetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", 1000);
			IILinearFilling.CurrentValue = IP.MarkSpeed = (int)Settings.GetObject("GrayScaleConversion.Gcode.Speed.Mark", 1000);
			IITravelSpeed.CurrentValue = IP.TravelSpeed = (int)Settings.GetObject("GrayScaleConversion.Gcode.Speed.Travel", 4000);

			TxtLaserOn.Text = IP.LaserOn = (string)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", "M3");
			TxtLaserOff.Text = IP.LaserOff = (string)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", "M5");
			IIMinPower.CurrentValue = IP.MinPower = (int)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", 0);
			IIMaxPower.CurrentValue = IP.MaxPower = (int)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", 255);

			IILinearFilling.Visible = LblLinearFilling.Visible = LblLinearFillingmm.Visible = (IP.SelectedTool == ImageProcessor.Tool.Line2Line || IP.SelectedTool == ImageProcessor.Tool.Dithering || (IP.SelectedTool == ImageProcessor.Tool.Vectorize && (IP.FillingDirection != ImageProcessor.Direction.None)));
			IIBorderTracing.Visible = LblBorderTracing.Visible = LblBorderTracingmm.Visible = (IP.SelectedTool == ImageProcessor.Tool.Vectorize);
			LblLinearFilling.Text = IP.SelectedTool == ImageProcessor.Tool.Vectorize ? "Filling Speed" : "Engraving Speed";
			
			ShowDialog();
		}
		
		
		private void IISizeW_CurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			if (ByUser)
				IISizeH.CurrentValue = IP.WidthToHeight(NewValue);
			
			IP.TargetSize = new Size(IISizeW.CurrentValue, IISizeH.CurrentValue);
		}

		private void IISizeH_CurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			if (ByUser)
				IISizeW.CurrentValue = IP.HeightToWidht(NewValue);
			
			IP.TargetSize = new Size(IISizeW.CurrentValue, IISizeH.CurrentValue);
		}
		
		void TxtLaserOnTextChanged(object sender, EventArgs e)
		{
			IP.LaserOn = TxtLaserOn.Text;
		}
		void TxtLaserOffTextChanged(object sender, EventArgs e)
		{
			IP.LaserOff = TxtLaserOff.Text;
		}
		void IIOffsetXYCurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			IP.TargetOffset = new Point(IIOffsetX.CurrentValue, IIOffsetY.CurrentValue);
		}
		
		void IIBorderTracingCurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			IP.BorderSpeed = NewValue;
		}
		
		void IIMarkSpeedCurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			IP.MarkSpeed = NewValue;
		}
		void IITravelSpeedCurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			IP.TravelSpeed = NewValue;
		}
		void IIMinPowerCurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			IP.MinPower = NewValue;
		}
		void IIMaxPowerCurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			IP.MaxPower = NewValue;
		}
		
	}
}
