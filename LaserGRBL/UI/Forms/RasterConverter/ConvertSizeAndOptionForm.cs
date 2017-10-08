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

namespace LaserGRBL.UI.Forms.RasterConverter
{
	/// <summary>
	/// Description of ConvertSizeAndOptionForm.
	/// </summary>
	public partial class ConvertSizeAndOptionForm : Form
	{
		bool supportPWM = (bool)Settings.GetObject("Support Hardware PWM", true);

		public ConvertSizeAndOptionForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			BackColor = ColorScheme.FormBackColor;
			//GbLaser.ForeColor = GbSize.ForeColor = GbSpeed.ForeColor = ForeColor = ColorScheme.FormForeColor;
			//BtnCancel.BackColor = BtnCreate.BackColor = ColorScheme.FormButtonsColor;

			//LblSmin.Visible = LblSmax.Visible = IIMaxPower.Visible = IIMinPower.Visible = BtnModulationInfo.Visible = supportPWM;
		}

		Core.RasterToGcode.PreviewGenerator IP;

		public void ShowDialog(Core.RasterToGcode.PreviewGenerator processor)
		{
			//IP = processor;
			
			
			//if (IP.Original.Height < IP.Original.Width)
			//{
			//	IISizeW.CurrentValue = (int)Settings.GetObject("GrayScaleConversion.Gcode.BiggestDimension", 100);
			//	IISizeH.CurrentValue = IP.WidthToHeight(IISizeW.CurrentValue);
			//}
			//else
			//{
			//	IISizeH.CurrentValue = (int)Settings.GetObject("GrayScaleConversion.Gcode.BiggestDimension", 100);
			//	IISizeW.CurrentValue = IP.HeightToWidht(IISizeH.CurrentValue);
			//}
		

			//IIBorderTracing.CurrentValue = IP.BorderSpeed = (int)Settings.GetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", 1000);
			//IILinearFilling.CurrentValue = IP.MarkSpeed = (int)Settings.GetObject("GrayScaleConversion.Gcode.Speed.Mark", 1000);
			//IITravelSpeed.CurrentValue = IP.TravelSpeed = (int)Settings.GetObject("GrayScaleConversion.Gcode.Speed.Travel", 4000);

			//IP.LaserOn = (string)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", "M3");

			//if (CBLaserON.Items.Contains(IP.LaserOn))
			//	CBLaserON.SelectedItem = IP.LaserOn;
			//else
			//	CBLaserON.SelectedIndex = 0;

			//IP.LaserOff = (string)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", "M5");

			//if (CBLaserOFF.Items.Contains(IP.LaserOff))
			//	CBLaserOFF.SelectedItem = IP.LaserOff;
			//else
			//	CBLaserOFF.SelectedIndex = 0;

			//IIMinPower.CurrentValue = IP.MinPower = (int)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", 0);
			//IIMaxPower.CurrentValue = IP.MaxPower = (int)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", 255);

			//IILinearFilling.Visible = LblLinearFilling.Visible = LblLinearFillingmm.Visible = (IP.SelectedTool == Core.RasterToGcode.PreviewGenerator.Tool.Line2Line || IP.SelectedTool == Core.RasterToGcode.PreviewGenerator.Tool.Dithering || (IP.SelectedTool == Core.RasterToGcode.PreviewGenerator.Tool.Vectorize && (IP.FillingDirection != Core.RasterToGcode.ConversionTool.EngravingDirection.None)));
			//IIBorderTracing.Visible = LblBorderTracing.Visible = LblBorderTracingmm.Visible = (IP.SelectedTool == Core.RasterToGcode.PreviewGenerator.Tool.Vectorize);
			//LblLinearFilling.Text = IP.SelectedTool == Core.RasterToGcode.PreviewGenerator.Tool.Vectorize ? "Filling Speed" : "Engraving Speed";

			//IIOffsetX.CurrentValue = IP.TargetOffset.X = (int)Settings.GetObject("GrayScaleConversion.Gcode.Offset.X", 0);
			//IIOffsetY.CurrentValue = IP.TargetOffset.Y = (int)Settings.GetObject("GrayScaleConversion.Gcode.Offset.Y", 0);

			//ShowDialog();
		}
		
		
		private void IISizeW_CurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			//if (ByUser)
			//	IISizeH.CurrentValue = IP.WidthToHeight(NewValue);
			
			//IP.TargetSize = new Size(IISizeW.CurrentValue, IISizeH.CurrentValue);
		}

		private void IISizeH_CurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			//if (ByUser)
			//	IISizeW.CurrentValue = IP.HeightToWidht(NewValue);
			
			//IP.TargetSize = new Size(IISizeW.CurrentValue, IISizeH.CurrentValue);
		}
		
		void IIOffsetXYCurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			//IP.TargetOffset = new Point(IIOffsetX.CurrentValue, IIOffsetY.CurrentValue);
		}
		
		void IIBorderTracingCurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			//IP.BorderSpeed = NewValue;
		}
		
		void IIMarkSpeedCurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			//IP.MarkSpeed = NewValue;
		}
		void IITravelSpeedCurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			//IP.TravelSpeed = NewValue;
		}
		void IIMinPowerCurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			//if (ByUser && IIMaxPower.CurrentValue <= NewValue)
			//	IIMaxPower.CurrentValue = NewValue + 1;

			//IP.MinPower = NewValue;
		}
		void IIMaxPowerCurrentValueChanged(object sender, int NewValue, bool ByUser)
		{
			//if (ByUser && IIMinPower.CurrentValue >= NewValue)
			//	IIMinPower.CurrentValue = NewValue - 1;

			//IP.MaxPower = NewValue;
		}

		private void BtnOnOffInfo_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/usage/raster-image-import/target-image-size-and-laser-options/#laser-modes");
		}

		private void BtnModulationInfo_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://lasergrbl.com/usage/raster-image-import/target-image-size-and-laser-options/#power-modulation");
		}

		private void CBLaserON_SelectedIndexChanged(object sender, EventArgs e)
		{
			//IP.LaserOn = (string)CBLaserON.SelectedItem;
		}

		private void CBLaserOFF_SelectedIndexChanged(object sender, EventArgs e)
		{
			//IP.LaserOff = (string)CBLaserOFF.SelectedItem;
		}
		
	}
}
