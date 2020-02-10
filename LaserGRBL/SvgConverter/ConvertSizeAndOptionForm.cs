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

namespace LaserGRBL.SvgConverter
{
	/// <summary>
	/// Description of ConvertSizeAndOptionForm.
	/// </summary>
	public partial class SvgToGCodeForm : Form
	{
		GrblCore mCore;
		bool supportPWM = (bool)Settings.GetObject("Support Hardware PWM", true);

        internal static void CreateAndShowDialog(GrblCore core, string filename, Form parent, bool append)
        {
            using (SvgToGCodeForm f = new SvgToGCodeForm(core, filename, append))
            {
                f.ShowDialogForm(parent);
                if (f.DialogResult == DialogResult.OK)
                {
                    Settings.SetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", f.IIBorderTracing.CurrentValue);
                    Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", f.IIMaxPower.CurrentValue);
					Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", f.IIMinPower.CurrentValue);
					Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", f.CBLaserON.SelectedItem);
					Settings.SetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", f.CBLaserOFF.SelectedItem);

					core.LoadedFile.LoadImportedGcode(filename, append);
                }
            }
        }

        private SvgToGCodeForm(GrblCore core, string filename, bool append)
		{
			InitializeComponent();
			mCore = core;

			BackColor = ColorScheme.FormBackColor;
			GbLaser.ForeColor = GbSpeed.ForeColor = ForeColor = ColorScheme.FormForeColor;
			BtnCancel.BackColor = BtnCreate.BackColor = ColorScheme.FormButtonsColor;

			LblSmin.Visible = LblSmax.Visible = IIMaxPower.Visible = IIMinPower.Visible = BtnModulationInfo.Visible = supportPWM;
			AssignMinMaxLimit();

			if (core.Type != Firmware.Marlin)
			{
				CBLaserON.Items.Add("M3");
				if (core.Configuration.LaserMode)
					CBLaserON.Items.Add("M4");
			}
			else
			{
				CBLaserON.Items.Add("M106 P1");
				CBLaserOFF.Items.Add("M107 P1");
			}
		}

		private void AssignMinMaxLimit()
        { 
			IIBorderTracing.MaxValue = (int)mCore.Configuration.MaxRateX;
			IIMaxPower.MaxValue = (int)mCore.Configuration.MaxPWM;
		}

		public void ShowDialogForm(Form parent)
        {
			IIBorderTracing.CurrentValue = (int)Settings.GetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", 1000);

			string LaserOn = (string)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", "M3");

			if (CBLaserON.Items.Contains(LaserOn))
				CBLaserON.SelectedItem = LaserOn;
			else
				CBLaserON.SelectedIndex = 0;

			string LaserOff = (string)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", "M5");

			if (CBLaserOFF.Items.Contains(LaserOff))
				CBLaserOFF.SelectedItem = LaserOff;
			else
				CBLaserOFF.SelectedIndex = 0;

			IIMinPower.CurrentValue = (int)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", 0);
			IIMaxPower.CurrentValue = (int)Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", 255);

			IIBorderTracing.Visible = LblBorderTracing.Visible = LblBorderTracingmm.Visible = true;

			base.ShowDialog(parent);
		}


		void IIBorderTracingCurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			//IP.BorderSpeed = NewValue;
		}

	
		void IIMinPowerCurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			if (ByUser && IIMaxPower.CurrentValue <= NewValue)
				IIMaxPower.CurrentValue = NewValue + 1;

			//IP.MinPower = NewValue;
		}
		void IIMaxPowerCurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			if (ByUser && IIMinPower.CurrentValue >= NewValue)
				IIMinPower.CurrentValue = NewValue - 1;

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

		//private void IISizeW_OnTheFlyValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		//{
		//	if (ByUser)
		//		IISizeH.CurrentValue = IP.WidthToHeight(NewValue);
		//}

		//private void IISizeH_OnTheFlyValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		//{
		//	if (ByUser) IISizeW.CurrentValue = IP.HeightToWidht(NewValue);
		//}

	}
}
