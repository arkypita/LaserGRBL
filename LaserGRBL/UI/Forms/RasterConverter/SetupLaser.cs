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
	public partial class SetupLaser : UserControl
	{
		LaserGRBL.Core.RasterToGcode.Configuration Config;
		public LaserGRBL.Core.RasterToGcode.LaserSetting Laser { get { return Config.LaserSetting; } }

		public SetupLaser()
		{
			InitializeComponent();
		}

		public void Start(LaserGRBL.Core.RasterToGcode.Configuration setup)
		{
			Config = setup;

			CbModulate.SuspendLayout();
			if (SupportPWM) CbModulate.AddItem(Core.RasterToGcode.LaserSetting.ModulationType.Power);
			CbModulate.AddItem(Core.RasterToGcode.LaserSetting.ModulationType.Speed);
			CbModulate.SelectedIndex = 0;
			LblWhatModulate.Visible = CbModulate.Visible = SupportPWM;
			CbModulate.ResumeLayout();
			RefreshAll();
		}

		private void CbModulate_SelectedIndexChanged(object sender, EventArgs e)
		{
			RefreshAll();
		}

		
		public void RefreshOnToolChange()
		{
			RefreshAll();
		}

		private void RefreshAll()
		{
			if (Config == null || Config.SelectedTool == null)
				return;

			SuspendLayout();
			PbLink.Visible = Config.SelectedTool.RequireModulation;
			LblWhatModulate.Visible = CbModulate.Visible = Config.SelectedTool.RequireModulation;
			LblModWhite.Visible = LblModWhiteCode.Visible = IIModWhite.Visible = LblModWhiteUM.Visible = Config.SelectedTool.RequireModulation;
			LblModBlack.Visible = LblModBlackCode.Visible = IIModBlack.Visible = LblModBlackUM.Visible = Config.SelectedTool.RequireModulation;
			LblPower.Visible = LblPowerCode.Visible = LblPowerUM.Visible = IIPower.Visible = !Config.SelectedTool.RequireModulation && SupportPWM;
			LblBorders.Visible = LblBorderCode.Visible = LblBorderUM.Visible = IIBorder.Visible = Config.SelectedTool.UseBorders;
			LblFilling.Visible = LblFillingCode.Visible = LblFillingUM.Visible = IIFilling.Visible = Config.SelectedTool.UseFilling;

			Laser.WhatModulate = (Core.RasterToGcode.LaserSetting.ModulationType)CbModulate.SelectedItem;
			LblModBlackCode.Text = LblModWhiteCode.Text = Laser.WhatModulateCode;
			LblModBlackUM.Text = LblModWhiteUM.Text = Laser.WhatModulateUM;
			LblBorderCode.Text = LblFillingCode.Text = Laser.WhatFillingCode(Config.SelectedTool);
			LblBorderUM.Text = LblFillingUM.Text = Laser.WhatFillingUM(Config.SelectedTool);
			LblFilling.Text = Laser.WhatFillingLabel(Config.SelectedTool);
			ResumeLayout();
		}


		public bool SupportPWM { get { return true; } }
	}
}
