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
		GrblCore mCore;
		bool supportPWM = (bool)Settings.GetObject("Support Hardware PWM", true);

		public ConvertSizeAndOptionForm(GrblCore core)
		{
			InitializeComponent();
			mCore = core;

			BackColor = ColorScheme.FormBackColor;
			GbLaser.ForeColor = GbSize.ForeColor = GbSpeed.ForeColor = ForeColor = ColorScheme.FormForeColor;
			BtnCancel.BackColor = BtnCreate.BackColor = ColorScheme.FormButtonsColor;

			LblSmin.Visible = LblSmax.Visible = IIMaxPower.Visible = IIMinPower.Visible = BtnModulationInfo.Visible = supportPWM;
			AssignMinMaxLimit();

			CBLaserON.Items.Add("M3");
			if (core.Configuration.LaserMode)
				CBLaserON.Items.Add("M4");
		}

		private void AssignMinMaxLimit()
		{
			IISizeW.MaxValue = (int)mCore.Configuration.TableWidth;
			IISizeH.MaxValue = (int)mCore.Configuration.TableHeight;

			IIOffsetX.MaxValue = (int)mCore.Configuration.TableWidth;
			IIOffsetY.MaxValue = (int)mCore.Configuration.TableHeight;
			IIOffsetX.MinValue = -(int)mCore.Configuration.TableWidth;
			IIOffsetY.MinValue = -(int)mCore.Configuration.TableHeight;

			IIBorderTracing.MaxValue = IILinearFilling.MaxValue = (int)mCore.Configuration.MaxRateX;
			IIMaxPower.MaxValue = (int)mCore.Configuration.MaxPWM;
		}

		ImageProcessor IP;

		public void ShowDialog(ImageProcessor processor)
		{
			IP = processor;


			if (IP.Original.Height < IP.Original.Width)
			{
				IISizeW.CurrentValue = (int)Settings.GetObject("GrayScaleConversion.Gcode.BiggestDimension", 100);
				IISizeH.CurrentValue = IP.WidthToHeight(IISizeW.CurrentValue);
			}
			else
			{
				IISizeH.CurrentValue = (int)Settings.GetObject("GrayScaleConversion.Gcode.BiggestDimension", 100);
				IISizeW.CurrentValue = IP.HeightToWidht(IISizeH.CurrentValue);
			}


			IIBorderTracing.CurrentValue = IP.BorderSpeed = (int)Settings.GetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", 1000);
			IILinearFilling.CurrentValue = IP.MarkSpeed = (int)Settings.GetObject("GrayScaleConversion.Gcode.Speed.Mark", 1000);

			IP.LaserOn = (string)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", "M3");

			if (CBLaserON.Items.Contains(IP.LaserOn))
				CBLaserON.SelectedItem = IP.LaserOn;
			else
				CBLaserON.SelectedIndex = 0;

			IP.LaserOff = (string)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", "M5");

			if (CBLaserOFF.Items.Contains(IP.LaserOff))
				CBLaserOFF.SelectedItem = IP.LaserOff;
			else
				CBLaserOFF.SelectedIndex = 0;

			IIMinPower.CurrentValue = IP.MinPower = (int)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", 0);
			IIMaxPower.CurrentValue = IP.MaxPower = (int)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", 255);

			IILinearFilling.Visible = LblLinearFilling.Visible = LblLinearFillingmm.Visible = (IP.SelectedTool == ImageProcessor.Tool.Line2Line || IP.SelectedTool == ImageProcessor.Tool.Dithering || (IP.SelectedTool == ImageProcessor.Tool.Vectorize && (IP.FillingDirection != ImageProcessor.Direction.None)));
			IIBorderTracing.Visible = LblBorderTracing.Visible = LblBorderTracingmm.Visible = (IP.SelectedTool == ImageProcessor.Tool.Vectorize);
			LblLinearFilling.Text = IP.SelectedTool == ImageProcessor.Tool.Vectorize ? "Filling Speed" : "Engraving Speed";

			IIOffsetX.CurrentValue = IP.TargetOffset.X = (int)Settings.GetObject("GrayScaleConversion.Gcode.Offset.X", 0);
			IIOffsetY.CurrentValue = IP.TargetOffset.Y = (int)Settings.GetObject("GrayScaleConversion.Gcode.Offset.Y", 0);

			ShowDialog();
		}


		private void IISizeW_CurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			IP.TargetSize = new Size(IISizeW.CurrentValue, IISizeH.CurrentValue);
		}

		private void IISizeH_CurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			IP.TargetSize = new Size(IISizeW.CurrentValue, IISizeH.CurrentValue);
		}

		void IIOffsetXYCurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			IP.TargetOffset = new Point(IIOffsetX.CurrentValue, IIOffsetY.CurrentValue);
		}

		void IIBorderTracingCurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			IP.BorderSpeed = NewValue;
		}

		void IIMarkSpeedCurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			IP.MarkSpeed = NewValue;
		}
		void IIMinPowerCurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			if (ByUser && IIMaxPower.CurrentValue <= NewValue)
				IIMaxPower.CurrentValue = NewValue + 1;

			IP.MinPower = NewValue;
		}
		void IIMaxPowerCurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			if (ByUser && IIMinPower.CurrentValue >= NewValue)
				IIMinPower.CurrentValue = NewValue - 1;

			IP.MaxPower = NewValue;
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
			IP.LaserOn = (string)CBLaserON.SelectedItem;
		}

		private void CBLaserOFF_SelectedIndexChanged(object sender, EventArgs e)
		{
			IP.LaserOff = (string)CBLaserOFF.SelectedItem;
		}

		private void IISizeW_OnTheFlyValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			if (ByUser)
				IISizeH.CurrentValue = IP.WidthToHeight(NewValue);
		}

		private void IISizeH_OnTheFlyValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			if (ByUser) IISizeW.CurrentValue = IP.HeightToWidht(NewValue);
		}

	}
}
