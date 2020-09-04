﻿//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using Sound;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class SettingsForm : Form
	{
        private GrblCore Core;
        public static event EventHandler SettingsChanged;

		public SettingsForm(GrblCore core)
		{
			InitializeComponent();

            this.Core = core;

            BackColor = ColorScheme.FormBackColor;
            ForeColor = ColorScheme.FormForeColor;
            TpRasterImport.BackColor = TpHardware.BackColor = TpJogControl.BackColor = TpAutoCooling.BackColor  = TpGCodeSettings.BackColor = BtnCancel.BackColor = BtnSave.BackColor = TpSoundSettings.BackColor = changeConBtn.BackColor = changeDconBtn.BackColor = changeFatBtn.BackColor = changeSucBtn.BackColor = changeWarBtn.BackColor = ColorScheme.FormBackColor;

            InitCoreCB();
			InitProtocolCB();
			InitStreamingCB();
			InitThreadingCB();

            CBCore.SelectedItem = Settings.GetObject("Firmware Type", Firmware.Grbl);
			CBSupportPWM.Checked = Settings.GetObject("Support Hardware PWM", true);
			CBProtocol.SelectedItem = Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial);
			CBStreamingMode.SelectedItem = Settings.GetObject("Streaming Mode", StreamingMode.Buffered);
			CbUnidirectional.Checked = Settings.GetObject("Unidirectional Engraving", false);
			CbThreadingMode.SelectedItem = Settings.GetObject("Threading Mode", ThreadingMode.UltraFast);
			CbIssueDetector.Checked = !Settings.GetObject("Do not show Issue Detector", false);
			CbSoftReset.Checked = Settings.GetObject("Reset Grbl On Connect", true);
			CbHardReset.Checked = Settings.GetObject("HardReset Grbl On Connect", false);

            CbContinuosJog.Checked = Settings.GetObject("Enable Continuous Jog", false);
            CbEnableZJog.Checked = Settings.GetObject("Enale Z Jog Control", false);

			CbHiRes.Checked = Settings.GetObject("Raster Hi-Res", false );

			TBHeader.Text = Settings.GetObject("GCode.CustomHeader", Constants.GCODE_STD_HEADER);
            TBHeader.ForeColor = ColorScheme.FormForeColor;
            TBHeader.BackColor = ColorScheme.FormBackColor;
            TBPasses.Text = Settings.GetObject("GCode.CustomPasses", Constants.GCODE_STD_PASSES);
            TBPasses.ForeColor = ColorScheme.FormForeColor;
            TBPasses.BackColor = ColorScheme.FormBackColor;
            TBFooter.Text = Settings.GetObject("GCode.CustomFooter", Constants.GCODE_STD_FOOTER);
            TBFooter.ForeColor = ColorScheme.FormForeColor;
            TBFooter.BackColor = ColorScheme.FormBackColor;

            CbPlaySuccess.Checked = Settings.GetObject($"Sound.{SoundEvent.EventId.Success}.Enabled", true);
            CbPlayWarning.Checked = Settings.GetObject($"Sound.{SoundEvent.EventId.Warning}.Enabled", true);
            CbPlayFatal.Checked = Settings.GetObject($"Sound.{SoundEvent.EventId.Fatal}.Enabled", true);
            CbPlayConnect.Checked = Settings.GetObject($"Sound.{SoundEvent.EventId.Connect}.Enabled", true);
            CbPlayDisconnect.Checked = Settings.GetObject($"Sound.{SoundEvent.EventId.Disconnect}.Enabled", true);

            successSoundLabel.Text = Path.GetFileName(Settings.GetObject($"Sound.{SoundEvent.EventId.Success}", $"Sound\\{SoundEvent.EventId.Success}.wav"));
            SuccesFullLabel.Text = Settings.GetObject($"Sound.{SoundEvent.EventId.Success}", $"Sound\\{SoundEvent.EventId.Success}.wav");
            warningSoundLabel.Text = Path.GetFileName(Settings.GetObject($"Sound.{SoundEvent.EventId.Warning}", $"Sound\\{SoundEvent.EventId.Warning}.wav"));
            WarningFullLabel.Text = Settings.GetObject($"Sound.{SoundEvent.EventId.Warning}", $"Sound\\{SoundEvent.EventId.Warning}.wav");
            fatalSoundLabel.Text = Path.GetFileName(Settings.GetObject($"Sound.{SoundEvent.EventId.Fatal}", $"Sound\\{SoundEvent.EventId.Fatal}.wav"));
            ErrorFullLabel.Text = Settings.GetObject($"Sound.{SoundEvent.EventId.Fatal}", $"Sound\\{SoundEvent.EventId.Fatal}.wav");
            connectSoundLabel.Text = Path.GetFileName(Settings.GetObject($"Sound.{SoundEvent.EventId.Connect}", $"Sound\\{SoundEvent.EventId.Connect}.wav"));
            ConnectFullLabel.Text = Settings.GetObject($"Sound.{SoundEvent.EventId.Connect}", $"Sound\\{SoundEvent.EventId.Connect}.wav");
            disconnectSoundLabel.Text = Path.GetFileName(Settings.GetObject($"Sound.{SoundEvent.EventId.Disconnect}", $"Sound\\{SoundEvent.EventId.Disconnect}.wav"));
            DisconnectFullLabel.Text = Settings.GetObject($"Sound.{SoundEvent.EventId.Disconnect}", $"Sound\\{SoundEvent.EventId.Disconnect}.wav");

            groupBox1.ForeColor = groupBox2.ForeColor = groupBox3.ForeColor = ColorScheme.FormForeColor;

            SuccesFullLabel.Visible = WarningFullLabel.Visible = ErrorFullLabel.Visible = ConnectFullLabel.Visible = DisconnectFullLabel.Visible = false;

            InitAutoCoolingTab();
        }

		private void InitAutoCoolingTab()
		{
			CbAutoCooling.Checked = Settings.GetObject("AutoCooling", false);
			CbOffMin.Items.Clear();
			CbOffSec.Items.Clear();
			CbOnMin.Items.Clear();
			CbOnSec.Items.Clear();

			for (int i = 0; i <= 60; i++)
				CbOnMin.Items.Add(i);
			for (int i = 0; i <= 10; i++)
				CbOffMin.Items.Add(i);

			for (int i = 0; i <= 59; i++)
				CbOnSec.Items.Add(i);
			for (int i = 0; i <= 59; i++)
				CbOffSec.Items.Add(i);

			TimeSpan CoolingOn = Settings.GetObject("AutoCooling TOn", TimeSpan.FromMinutes(10));
			TimeSpan CoolingOff = Settings.GetObject("AutoCooling TOff", TimeSpan.FromMinutes(1));

			CbOnMin.SelectedItem = CoolingOn.Minutes;
			CbOffMin.SelectedItem = CoolingOff.Minutes;
			CbOnSec.SelectedItem = CoolingOn.Seconds;
			CbOffSec.SelectedItem = CoolingOff.Seconds;
		}

		private void InitCoreCB()
        {
            CBCore.BeginUpdate();
            CBCore.Items.Add(Firmware.Grbl);
            CBCore.Items.Add(Firmware.Smoothie);
            CBCore.Items.Add(Firmware.Marlin);
            CBCore.EndUpdate();
        }

        private void InitThreadingCB()
		{
			CbThreadingMode.BeginUpdate();
			CbThreadingMode.Items.Add(ThreadingMode.Insane);
			CbThreadingMode.Items.Add(ThreadingMode.UltraFast);
			CbThreadingMode.Items.Add(ThreadingMode.Fast);
			CbThreadingMode.Items.Add(ThreadingMode.Quiet);
			CbThreadingMode.Items.Add(ThreadingMode.Slow);
			CbThreadingMode.EndUpdate();
		}

		private void InitProtocolCB()
		{
			CBProtocol.BeginUpdate();
			CBProtocol.Items.Add(ComWrapper.WrapperType.UsbSerial);
			CBProtocol.Items.Add(ComWrapper.WrapperType.Telnet);
			CBProtocol.Items.Add(ComWrapper.WrapperType.LaserWebESP8266);
			CBProtocol.Items.Add(ComWrapper.WrapperType.Emulator);
			CBProtocol.EndUpdate();
		}

		private void InitStreamingCB()
		{
			CBStreamingMode.BeginUpdate();
			CBStreamingMode.Items.Add(StreamingMode.Buffered);
			CBStreamingMode.Items.Add(StreamingMode.Synchronous);
			CBStreamingMode.Items.Add(StreamingMode.RepeatOnError);
			CBStreamingMode.EndUpdate();
		}

		internal static void CreateAndShowDialog(GrblCore core)
		{
			using (SettingsForm sf = new SettingsForm(core))
				sf.ShowDialog();
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
            Settings.SetObject("Firmware Type", CBCore.SelectedItem);
            Settings.SetObject("Support Hardware PWM", CBSupportPWM.Checked);
			Settings.SetObject("ComWrapper Protocol", CBProtocol.SelectedItem);
			Settings.SetObject("Streaming Mode", CBStreamingMode.SelectedItem);
			Settings.SetObject("Unidirectional Engraving", CbUnidirectional.Checked);
			Settings.SetObject("Threading Mode", CbThreadingMode.SelectedItem);
			Settings.SetObject("Do not show Issue Detector", !CbIssueDetector.Checked);
			Settings.SetObject("Reset Grbl On Connect", CbSoftReset.Checked);
			Settings.SetObject("HardReset Grbl On Connect", CbHardReset.Checked);
            Settings.SetObject("Enable Continuous Jog", CbContinuosJog.Checked);
            Settings.SetObject("Enale Z Jog Control", CbEnableZJog.Checked);

			Settings.SetObject("AutoCooling", CbAutoCooling.Checked);
			Settings.SetObject("AutoCooling TOn", MaxTs(TimeSpan.FromSeconds(10), new TimeSpan(0, (int)CbOnMin.SelectedItem, (int)CbOnSec.SelectedItem)));
			Settings.SetObject("AutoCooling TOff", MaxTs(TimeSpan.FromSeconds(10), new TimeSpan(0, (int)CbOffMin.SelectedItem, (int)CbOffSec.SelectedItem)));

			Settings.SetObject("GCode.CustomHeader", TBHeader.Text.Trim());
			Settings.SetObject("GCode.CustomPasses", TBPasses.Text.Trim());
			Settings.SetObject("GCode.CustomFooter", TBFooter.Text.Trim());

            Settings.SetObject($"Sound.{SoundEvent.EventId.Success}", SuccesFullLabel.Text.Trim());
            Settings.SetObject($"Sound.{SoundEvent.EventId.Warning}", WarningFullLabel.Text.Trim());
            Settings.SetObject($"Sound.{SoundEvent.EventId.Fatal}", ErrorFullLabel.Text.Trim());
            Settings.SetObject($"Sound.{SoundEvent.EventId.Connect}", ConnectFullLabel.Text.Trim());
            Settings.SetObject($"Sound.{SoundEvent.EventId.Disconnect}", DisconnectFullLabel.Text.Trim());

            Settings.SetObject($"Sound.{SoundEvent.EventId.Success}.Enabled", CbPlaySuccess.Checked);
            Settings.SetObject($"Sound.{SoundEvent.EventId.Warning}.Enabled", CbPlayWarning.Checked);
            Settings.SetObject($"Sound.{SoundEvent.EventId.Fatal}.Enabled", CbPlayFatal.Checked);
            Settings.SetObject($"Sound.{SoundEvent.EventId.Connect}.Enabled", CbPlayConnect.Checked);
            Settings.SetObject($"Sound.{SoundEvent.EventId.Disconnect}.Enabled", CbPlayDisconnect.Checked);

            Settings.SetObject("Raster Hi-Res", CbHiRes.Checked);

			Settings.Save();

            SettingsChanged?.Invoke(this, null);

            Close();

            if (Core.Type != Settings.GetObject("Firmware Type", Firmware.Grbl) && MessageBox.Show(Strings.FirmwareRequireRestartNow, Strings.FirmwareRequireRestart, MessageBoxButtons.OKCancel) == DialogResult.OK)
                Application.Restart();
		}

		private TimeSpan MaxTs(TimeSpan a, TimeSpan b)
		{ return TimeSpan.FromTicks(Math.Max(a.Ticks, b.Ticks)); }

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BtnModulationInfo_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://lasergrbl.com/configuration/#pwm-support");
		}

		private void BtnLaserMode_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://lasergrbl.com/configuration/#laser-mode");
		}

		private void BtnProtocol_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://lasergrbl.com/configuration/#protocol");
		}

		private void BtnStreamingMode_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://lasergrbl.com/configuration/#streaming-mode");
		}

		private void BtnThreadingModel_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(@"https://lasergrbl.com/configuration/#threading-mode");
		}

        private void BtnFType_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://lasergrbl.com/configuration/#firmware-type");
        }

        private void changeSucBtn_Click(object sender, EventArgs e)
        {
            if (SoundBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                successSoundLabel.Text = Path.GetFileName(SoundBrowserDialog.FileName);
                SuccesFullLabel.Text = SoundBrowserDialog.FileName;
            }
        }

        private void changeWarBtn_Click(object sender, EventArgs e)
        {
            if (SoundBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                warningSoundLabel.Text = Path.GetFileName(SoundBrowserDialog.FileName);
                WarningFullLabel.Text = SoundBrowserDialog.FileName;
            }
        }

        private void changeFatBtn_Click(object sender, EventArgs e)
        {
            if (SoundBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                fatalSoundLabel.Text = Path.GetFileName(SoundBrowserDialog.FileName);
                ErrorFullLabel.Text = SoundBrowserDialog.FileName;
            }
        }

        private void changeConBtn_Click(object sender, EventArgs e)
        {
            if (SoundBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                connectSoundLabel.Text = Path.GetFileName(SoundBrowserDialog.FileName);
                ConnectFullLabel.Text = SoundBrowserDialog.FileName;
            }
        }

        private void changeDconBtn_Click(object sender, EventArgs e)
        {
            if (SoundBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                disconnectSoundLabel.Text = Path.GetFileName(SoundBrowserDialog.FileName);
                DisconnectFullLabel.Text = SoundBrowserDialog.FileName;
            }
        }
    }
}
