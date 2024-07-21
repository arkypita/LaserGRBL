//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using LaserGRBL.Icons;
using LaserGRBL.UserControls;
using Sound;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL
{
    public partial class SettingsForm : Form
	{
        private GrblCore Core;
        public static event EventHandler SettingsChanged;
		private Settings.GraphicMode PrevGraphicMode;

		public SettingsForm(GrblCore core)
		{
			InitializeComponent();

            this.Core = core;

            BackColor = ColorScheme.FormBackColor;
            ForeColor = ColorScheme.FormForeColor;
            TpVectorImport.BackColor = TpRasterImport.BackColor = TpHardware.BackColor = TpJogControl.BackColor = TpAutoCooling.BackColor  = TpGCodeSettings.BackColor = BtnCancel.BackColor = BtnSave.BackColor = TpSoundSettings.BackColor = changeConBtn.BackColor = changeDconBtn.BackColor = changeFatBtn.BackColor = changeSucBtn.BackColor = changeWarBtn.BackColor = ColorScheme.FormBackColor;

			ThemeMgr.SetTheme(this, true);
			Size btnSize = new Size(16, 16);
			IconsMgr.PrepareButton(BtnStreamingMode, "mdi-information-slab-box", btnSize);
            IconsMgr.PrepareButton(BtnProtocol, "mdi-information-slab-box", btnSize);
            IconsMgr.PrepareButton(BtnThreadingModel, "mdi-information-slab-box", btnSize);
            IconsMgr.PrepareButton(BtnFType, "mdi-information-slab-box", btnSize);
            IconsMgr.PrepareButton(BtnModulationInfo, "mdi-information-slab-box", btnSize);
            IconsMgr.PrepareButton(BtnTelegNoteInfo, "mdi-information-slab-box", btnSize);
            IconsMgr.PrepareButton(imageButton1, "mdi-information-slab-box", btnSize);
            IconsMgr.PrepareButton(BtnSave, "mdi-checkbox-marked");
            IconsMgr.PrepareButton(BtnCancel, "mdi-close-box");

            InitCoreCB();
			InitProtocolCB();
			InitStreamingCB();
			InitThreadingCB();
			InitGraphicModeCB();

            CBCore.SelectedItem = Settings.GetObject("Firmware Type", Firmware.Grbl);
			CBSupportPWM.Checked = Settings.GetObject("Support Hardware PWM", true);
			CBProtocol.SelectedItem = Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial);
			CBStreamingMode.SelectedItem = Settings.GetObject("Streaming Mode", GrblCore.StreamingMode.Buffered);
			CbUnidirectional.Checked = Settings.GetObject("Unidirectional Engraving", false);
			CbDisableSkip.Checked = Settings.GetObject("Disable G0 fast skip", false);
			CbThreadingMode.SelectedItem = Settings.GetObject("Threading Mode", GrblCore.ThreadingMode.UltraFast);
			CbIssueDetector.Checked = !Settings.GetObject("Do not show Issue Detector", false);
			CbSoftReset.Checked = Settings.GetObject("Reset Grbl On Connect", true);
			CbHardReset.Checked = Settings.GetObject("HardReset Grbl On Connect", false);
			CbQueryDI.Checked = Settings.GetObject("Query MachineInfo ($I) at connect", true);
			CbDisableBoundWarn.Checked = Settings.GetObject("DisableBoundaryWarning", false);
			CbClickNJog.Checked = Settings.GetObject("Click N Jog", true);

			CbContinuosJog.Checked = Settings.GetObject("Enable Continuous Jog", false);
            CbEnableZJog.Checked = Settings.GetObject("Enale Z Jog Control", false);

			CbHiRes.Checked = Settings.GetObject("Raster Hi-Res", false );

			TBHeader.Text = Settings.GetObject("GCode.CustomHeader", GrblCore.GCODE_STD_HEADER);
            TBHeader.ForeColor = ColorScheme.FormForeColor;
            TBHeader.BackColor = ColorScheme.FormBackColor;
            TBPasses.Text = Settings.GetObject("GCode.CustomPasses", GrblCore.GCODE_STD_PASSES);
            TBPasses.ForeColor = ColorScheme.FormForeColor;
            TBPasses.BackColor = ColorScheme.FormBackColor;
            TBFooter.Text = Settings.GetObject("GCode.CustomFooter", GrblCore.GCODE_STD_FOOTER);
            TBFooter.ForeColor = ColorScheme.FormForeColor;
            TBFooter.BackColor = ColorScheme.FormBackColor;
            TpOptions.BackColor = ColorScheme.FormBackColor;

            CbPlaySuccess.Checked = Settings.GetObject($"Sound.{SoundEvent.EventId.Success}.Enabled", true);
            CbPlayWarning.Checked = Settings.GetObject($"Sound.{SoundEvent.EventId.Warning}.Enabled", true);
            CbPlayFatal.Checked = Settings.GetObject($"Sound.{SoundEvent.EventId.Fatal}.Enabled", true);
            CbPlayConnect.Checked = Settings.GetObject($"Sound.{SoundEvent.EventId.Connect}.Enabled", true);
            CbPlayDisconnect.Checked = Settings.GetObject($"Sound.{SoundEvent.EventId.Disconnect}.Enabled", true);

			CbTelegramNotification.Checked = Settings.GetObject("TelegramNotification.Enabled", false);
			TxtNotification.Text = Tools.Protector.Decrypt(Settings.GetObject("TelegramNotification.Code", ""), "");
			UdTelegramNotificationThreshold.Value = (decimal)Settings.GetObject("TelegramNotification.Threshold", 1);

			successSoundLabel.Text = System.IO.Path.GetFileName(Settings.GetObject($"Sound.{SoundEvent.EventId.Success}", $"Sound\\{SoundEvent.EventId.Success}.wav"));
            SuccesFullLabel.Text = Settings.GetObject($"Sound.{SoundEvent.EventId.Success}", $"Sound\\{SoundEvent.EventId.Success}.wav");
            warningSoundLabel.Text = System.IO.Path.GetFileName(Settings.GetObject($"Sound.{SoundEvent.EventId.Warning}", $"Sound\\{SoundEvent.EventId.Warning}.wav"));
            WarningFullLabel.Text = Settings.GetObject($"Sound.{SoundEvent.EventId.Warning}", $"Sound\\{SoundEvent.EventId.Warning}.wav");
            fatalSoundLabel.Text = System.IO.Path.GetFileName(Settings.GetObject($"Sound.{SoundEvent.EventId.Fatal}", $"Sound\\{SoundEvent.EventId.Fatal}.wav"));
            ErrorFullLabel.Text = Settings.GetObject($"Sound.{SoundEvent.EventId.Fatal}", $"Sound\\{SoundEvent.EventId.Fatal}.wav");
            connectSoundLabel.Text = System.IO.Path.GetFileName(Settings.GetObject($"Sound.{SoundEvent.EventId.Connect}", $"Sound\\{SoundEvent.EventId.Connect}.wav"));
            ConnectFullLabel.Text = Settings.GetObject($"Sound.{SoundEvent.EventId.Connect}", $"Sound\\{SoundEvent.EventId.Connect}.wav");
            disconnectSoundLabel.Text = System.IO.Path.GetFileName(Settings.GetObject($"Sound.{SoundEvent.EventId.Disconnect}", $"Sound\\{SoundEvent.EventId.Disconnect}.wav"));
            DisconnectFullLabel.Text = Settings.GetObject($"Sound.{SoundEvent.EventId.Disconnect}", $"Sound\\{SoundEvent.EventId.Disconnect}.wav");

			CbSmartBezier.Checked = Settings.GetObject($"Vector.UseSmartBezier", true);

			CbDisableSafetyCD.Checked = Settings.GetObject("DisableSafetyCountdown", false);
			CbQuietSafetyCB.Checked = Settings.GetObject("QuietSafetyCountdown", false);
            CbLegacyIcons.Checked = Settings.GetObject("LegacyIcons", false);
			CBGraphicMode.SelectedValue = PrevGraphicMode = Settings.ConfiguredGraphicMode;

			groupBox1.ForeColor = groupBox2.ForeColor = groupBox3.ForeColor = ColorScheme.FormForeColor;

            SuccesFullLabel.Visible = WarningFullLabel.Visible = ErrorFullLabel.Visible = ConnectFullLabel.Visible = DisconnectFullLabel.Visible = false;

			if (Core.GrblVersion != null && Core.GrblVersion.IsOrtur && Core.GrblVersion.OrturFWVersionNumber >= 140)
				LblWarnOrturAC.Visible = false;

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
			CBCore.Items.Add(Firmware.VigoWork);
			CBCore.EndUpdate();
        }

        private void InitThreadingCB()
		{
			CbThreadingMode.BeginUpdate();
			CbThreadingMode.Items.Add(GrblCore.ThreadingMode.Insane);
			CbThreadingMode.Items.Add(GrblCore.ThreadingMode.UltraFast);
			CbThreadingMode.Items.Add(GrblCore.ThreadingMode.Fast);
			CbThreadingMode.Items.Add(GrblCore.ThreadingMode.Quiet);
			CbThreadingMode.Items.Add(GrblCore.ThreadingMode.Slow);
			CbThreadingMode.EndUpdate();
		}

		private void InitProtocolCB()
		{
			CBProtocol.BeginUpdate();
			CBProtocol.Items.Add(ComWrapper.WrapperType.UsbSerial);
			CBProtocol.Items.Add(ComWrapper.WrapperType.UsbSerial2);
			CBProtocol.Items.Add(ComWrapper.WrapperType.Telnet);
			CBProtocol.Items.Add(ComWrapper.WrapperType.LaserWebESP8266);
			CBProtocol.Items.Add(ComWrapper.WrapperType.Emulator);
			CBProtocol.EndUpdate();
		}

		private void InitStreamingCB()
		{
			CBStreamingMode.BeginUpdate();
			CBStreamingMode.Items.Add(GrblCore.StreamingMode.Buffered);
			CBStreamingMode.Items.Add(GrblCore.StreamingMode.Synchronous);
			CBStreamingMode.Items.Add(GrblCore.StreamingMode.RepeatOnError);
			CBStreamingMode.EndUpdate();
		}

		private void InitGraphicModeCB()
		{
			CBGraphicMode.BeginUpdate();
			List<KeyValuePair<string, Settings.GraphicMode>> list = new List<KeyValuePair<string, Settings.GraphicMode>>();
			list.Add(new KeyValuePair<string, Settings.GraphicMode>("Auto", Settings.GraphicMode.AUTO));
			list.Add(new KeyValuePair<string, Settings.GraphicMode>("Hardware Acceleration", Settings.GraphicMode.FBO));
			list.Add(new KeyValuePair<string, Settings.GraphicMode>("Software Rendering", Settings.GraphicMode.DIB));
			list.Add(new KeyValuePair<string, Settings.GraphicMode>("Legacy", Settings.GraphicMode.GDI));
			CBGraphicMode.DisplayMember = "Key";
			CBGraphicMode.ValueMember = "Value";
			CBGraphicMode.DataSource = list;
			CBGraphicMode.EndUpdate();
		}

		internal static void CreateAndShowDialog(Form parent, GrblCore core)
		{
			using (SettingsForm sf = new SettingsForm(core))
				sf.ShowDialog(parent);
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
            Settings.SetObject("Firmware Type", CBCore.SelectedItem);
            Settings.SetObject("Support Hardware PWM", CBSupportPWM.Checked);
			Settings.SetObject("ComWrapper Protocol", CBProtocol.SelectedItem);
			Settings.SetObject("Streaming Mode", CBStreamingMode.SelectedItem);
			Settings.SetObject("Unidirectional Engraving", CbUnidirectional.Checked);
			Settings.SetObject("Disable G0 fast skip", CbDisableSkip.Checked);
			Settings.SetObject("Threading Mode", CbThreadingMode.SelectedItem);
			Settings.SetObject("Do not show Issue Detector", !CbIssueDetector.Checked);
			Settings.SetObject("Reset Grbl On Connect", CbSoftReset.Checked);
			Settings.SetObject("HardReset Grbl On Connect", CbHardReset.Checked);
			Settings.SetObject("Query MachineInfo ($I) at connect", CbQueryDI.Checked);
			Settings.SetObject("Enable Continuous Jog", CbContinuosJog.Checked);
            Settings.SetObject("Enale Z Jog Control", CbEnableZJog.Checked);
			Settings.SetObject("DisableBoundaryWarning", CbDisableBoundWarn.Checked);
			Settings.SetObject("Click N Jog", CbClickNJog.Checked);

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

			Settings.SetObject("TelegramNotification.Enabled", CbTelegramNotification.Checked);
			Settings.SetObject("TelegramNotification.Threshold", (int)UdTelegramNotificationThreshold.Value);
			Settings.SetObject("TelegramNotification.Code", Tools.Protector.Encrypt(TxtNotification.Text));

            Settings.SetObject("Raster Hi-Res", CbHiRes.Checked);

			Settings.SetObject("Vector.UseSmartBezier", CbSmartBezier.Checked);

			Settings.SetObject("DisableSafetyCountdown", CbDisableSafetyCD.Checked);
			Settings.SetObject("QuietSafetyCountdown", CbQuietSafetyCB.Checked);
            Settings.SetObject("LegacyIcons", CbLegacyIcons.Checked);
			Settings.ConfiguredGraphicMode = (Settings.GraphicMode)CBGraphicMode.SelectedValue;

			SettingsChanged?.Invoke(this, null);

            Close();

			if (PrevGraphicMode != Settings.ConfiguredGraphicMode && MessageBox.Show(Strings.PreviewChangesRequiresRestart, Strings.FirmwareRequireRestart, MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				UsageStats.DoNotSendNow = true;
				Application2.RestartNoCommandLine();
			}
			if (Core.Type != Settings.GetObject("Firmware Type", Firmware.Grbl) && MessageBox.Show(Strings.FirmwareRequireRestartNow, Strings.FirmwareRequireRestart, MessageBoxButtons.OKCancel) == DialogResult.OK)
                Application.Restart();

            if (IconsMgr.LegacyIcons != Settings.GetObject("LegacyIcons", false) && MessageBox.Show(Strings.IconsChangesRequiresRestart, Strings.FirmwareRequireRestart, MessageBoxButtons.OKCancel) == DialogResult.OK)
                Application.Restart();
        }

		private TimeSpan MaxTs(TimeSpan a, TimeSpan b)
		{ return TimeSpan.FromTicks(Math.Max(a.Ticks, b.Ticks)); }

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BtnModulationInfo_Click(object sender, EventArgs e)
		{Tools.Utils.OpenLink(@"https://lasergrbl.com/configuration/#pwm-support");}

		private void BtnLaserMode_Click(object sender, EventArgs e)
		{Tools.Utils.OpenLink(@"https://lasergrbl.com/configuration/#laser-mode");}

		private void BtnProtocol_Click(object sender, EventArgs e)
		{Tools.Utils.OpenLink(@"https://lasergrbl.com/configuration/#protocol");}

		private void BtnStreamingMode_Click(object sender, EventArgs e)
		{Tools.Utils.OpenLink(@"https://lasergrbl.com/configuration/#streaming-mode");}

		private void BtnThreadingModel_Click(object sender, EventArgs e)
		{Tools.Utils.OpenLink(@"https://lasergrbl.com/configuration/#threading-mode");}

        private void BtnFType_Click(object sender, EventArgs e)
        {Tools.Utils.OpenLink(@"https://lasergrbl.com/configuration/#firmware-type");}

        private void changeSucBtn_Click(object sender, EventArgs e)
        {
            if (SoundBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                successSoundLabel.Text = System.IO.Path.GetFileName(SoundBrowserDialog.FileName);
                SuccesFullLabel.Text = SoundBrowserDialog.FileName;
            }
        }

        private void changeWarBtn_Click(object sender, EventArgs e)
        {
            if (SoundBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                warningSoundLabel.Text = System.IO.Path.GetFileName(SoundBrowserDialog.FileName);
                WarningFullLabel.Text = SoundBrowserDialog.FileName;
            }
        }

        private void changeFatBtn_Click(object sender, EventArgs e)
        {
            if (SoundBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                fatalSoundLabel.Text = System.IO.Path.GetFileName(SoundBrowserDialog.FileName);
                ErrorFullLabel.Text = SoundBrowserDialog.FileName;
            }
        }

        private void changeConBtn_Click(object sender, EventArgs e)
        {
            if (SoundBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                connectSoundLabel.Text = System.IO.Path.GetFileName(SoundBrowserDialog.FileName);
                ConnectFullLabel.Text = SoundBrowserDialog.FileName;
            }
        }

        private void changeDconBtn_Click(object sender, EventArgs e)
        {
            if (SoundBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                disconnectSoundLabel.Text = System.IO.Path.GetFileName(SoundBrowserDialog.FileName);
                DisconnectFullLabel.Text = SoundBrowserDialog.FileName;
            }
        }

		private void TbNotification_TextChanged(object sender, EventArgs e)
		{
			EnableTest();
		}

		private void EnableTest()
		{
			BtnTestNotification.Enabled = TxtNotification.Text.Trim().Length == 10 && CbTelegramNotification.Checked;
		}

		private void BtnTestNotification_Click(object sender, EventArgs e)
		{
			Telegram.NotifyEvent(TxtNotification.Text, "If you receive this message, all is fine!");
			MessageBox.Show(Strings.BoxTelegramSettingText, Strings.BoxTelegramSettingTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void BtnTelegNoteInfo_Click(object sender, EventArgs e)
		{ Tools.Utils.OpenLink(@"https://lasergrbl.com/telegram/"); }

		private void CbTelegramNotification_CheckedChanged(object sender, EventArgs e)
		{
			EnableTest();
		}

		private void BtnRenderingMode_Click(object sender, EventArgs e)
		{ Tools.Utils.OpenLink(@"https://lasergrbl.com/configuration/#rendering-mode"); }
	}
}
