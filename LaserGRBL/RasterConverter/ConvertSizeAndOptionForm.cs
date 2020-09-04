﻿//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using LaserGRBL.PSHelper;
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
        bool supportPWM = Settings.GetObject("Support Hardware PWM", true);

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

			// For Marlin, we must change LaserOn & Laser Off command :
            //if (core.Type != Firmware.Marlin)
            //{
            //    CBLaserON.Items.Add("M3");
            //    if (core.Configuration.LaserMode)
            //        CBLaserON.Items.Add("M4");
            //}
            //else
            //{
            //    CBLaserON.Items.Add("M106 P1");
            //    CBLaserOFF.Items.Add("M107 P1");
            //}
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
                IISizeW.CurrentValue = Settings.GetObject("GrayScaleConversion.Gcode.BiggestDimension", 100F);
                IISizeH.CurrentValue = IP.WidthToHeight(IISizeW.CurrentValue);
            }
            else
            {
                IISizeH.CurrentValue = Settings.GetObject("GrayScaleConversion.Gcode.BiggestDimension", 100F);
                IISizeW.CurrentValue = IP.HeightToWidht(IISizeH.CurrentValue);
            }


            IIBorderTracing.CurrentValue = IP.BorderSpeed = Settings.GetObject("GrayScaleConversion.VectorizeOptions.BorderSpeed", 1000);
            IILinearFilling.CurrentValue = IP.MarkSpeed = Settings.GetObject("GrayScaleConversion.Gcode.Speed.Mark", 1000);

            IP.LaserOn = Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOn", "M3");

            if (CBLaserON.Items.Contains(IP.LaserOn))
                CBLaserON.SelectedItem = IP.LaserOn;
            else
                CBLaserON.SelectedIndex = 0;

            IP.LaserOff = Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.LaserOff", "M5");

            if (CBLaserOFF.Items.Contains(IP.LaserOff))
                CBLaserOFF.SelectedItem = IP.LaserOff;
            else
                CBLaserOFF.SelectedIndex = 0;

            IIMinPower.CurrentValue = IP.MinPower = Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMin", 0);
            IIMaxPower.CurrentValue = IP.MaxPower = Settings.GetObject("GrayScaleConversion.Gcode.LaserOptions.PowerMax", (int)mCore.Configuration.MaxPWM);

            IILinearFilling.Visible = LblLinearFilling.Visible = LblLinearFillingmm.Visible = (IP.SelectedTool == Tool.Line2Line || IP.SelectedTool == Tool.Dithering || (IP.SelectedTool == Tool.Vectorize && (IP.FillingDirection != Direction.None)));
            IIBorderTracing.Visible = LblBorderTracing.Visible = LblBorderTracingmm.Visible = (IP.SelectedTool == Tool.Vectorize || IP.SelectedTool == Tool.Centerline);
            LblLinearFilling.Text = IP.SelectedTool == Tool.Vectorize ? "Filling Speed" : "Engraving Speed";

            IIOffsetX.CurrentValue = IP.TargetOffset.X = Settings.GetObject("GrayScaleConversion.Gcode.Offset.X", 0F);
            IIOffsetY.CurrentValue = IP.TargetOffset.Y = Settings.GetObject("GrayScaleConversion.Gcode.Offset.Y", 0F);

            ShowDialog();
        }


        private void IISizeW_CurrentValueChanged(object sender, float OldValue, float NewValue, bool ByUser)
        {
            IP.TargetSize = new SizeF(IISizeW.CurrentValue, IISizeH.CurrentValue);
        }

        private void IISizeH_CurrentValueChanged(object sender, float OldValue, float NewValue, bool ByUser)
        {
            IP.TargetSize = new SizeF(IISizeW.CurrentValue, IISizeH.CurrentValue);
        }

        void IIOffsetXYCurrentValueChanged(object sender, float OldValue, float NewValue, bool ByUser)
        {
            IP.TargetOffset = new PointF(IIOffsetX.CurrentValue, IIOffsetY.CurrentValue);
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
            System.Diagnostics.Process.Start(@"https://lasergrbl.com/usage/raster-image-import/target-image-size-and-laser-options/#laser-modes");
        }

        private void BtnModulationInfo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://lasergrbl.com/usage/raster-image-import/target-image-size-and-laser-options/#power-modulation");
        }

        private void CBLaserON_SelectedIndexChanged(object sender, EventArgs e)
        {
            IP.LaserOn = (string)CBLaserON.SelectedItem;
        }

        private void CBLaserOFF_SelectedIndexChanged(object sender, EventArgs e)
        {
            IP.LaserOff = (string)CBLaserOFF.SelectedItem;
        }

        private void IISizeW_OnTheFlyValueChanged(object sender, float OldValue, float NewValue, bool ByUser)
        {
            if (ByUser)
                IISizeH.CurrentValue = IP.WidthToHeight(NewValue);
        }

        private void IISizeH_OnTheFlyValueChanged(object sender, float OldValue, float NewValue, bool ByUser)
        {
            if (ByUser) IISizeW.CurrentValue = IP.HeightToWidht(NewValue);
        }

        private void CbAutosize_CheckedChanged(object sender, EventArgs e)
        {
            IISizeH.Enabled = IISizeW.Enabled = !CbAutosize.Checked;
            IIDpi.Enabled = CbAutosize.Checked;

            ComputeDpiSize();
        }

        private void IIDpi_CurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
        {
            ComputeDpiSize();
        }

        private void ComputeDpiSize()
        {
            if (CbAutosize.Checked)
            {
                IISizeW.CurrentValue = Convert.ToSingle(25.4 * IP.TrueOriginal.Width / IIDpi.CurrentValue);
                IISizeH.CurrentValue = IP.WidthToHeight(IISizeW.CurrentValue);
            }

			BtnDPI.Enabled = CbAutosize.Checked && (IIDpi.CurrentValue != IP.FileDPI);
        }

        private void BtnDPI_Click(object sender, EventArgs e)
		{
			if (CbAutosize.Checked)
				IIDpi.CurrentValue = IP.FileDPI;
		}

		private void BtnPSHelper_Click(object sender, EventArgs e)
		{
			MaterialDB.MaterialsRow row = PSHelperForm.CreateAndShowDialog();
			if (row != null)
			{
				if (IIBorderTracing.Visible)
					IIBorderTracing.CurrentValue = row.Speed;
				if (IILinearFilling.Visible)
					IILinearFilling.CurrentValue = row.Speed;

				IIMaxPower.CurrentValue = IIMaxPower.MaxValue * row.Power / 100;
			}
		}
	}
}
