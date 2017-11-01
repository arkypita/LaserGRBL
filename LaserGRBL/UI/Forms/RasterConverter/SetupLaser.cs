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
			RefreshOnToolChange();
		}

		private void CbModulate_SelectedIndexChanged(object sender, EventArgs e)
		{
			SuspendLayout();
			Laser.WhatModulate = (Core.RasterToGcode.LaserSetting.ModulationType)CbModulate.SelectedItem;
			LblModBlackCode.Text = LblModWhiteCode.Text = Laser.WhatModulateCode;
			LblBorderCode.Text = LblFillingCode.Text = Laser.WhatFixedCode;
			LblModBlackUM.Text = LblModWhiteUM.Text = Laser.WhatModulateUM;
			LblBorderUM.Text = LblFillingUM.Text = Laser.WhatFixedUM;
			ResumeLayout();
		}

		
		public void RefreshOnToolChange()
		{
			if (Config == null || Config.SelectedTool == null)
				return;

			SuspendLayout();
			LblWhatModulate.Visible = CbModulate.Visible = Config.SelectedTool.RequireModulation;
			LblModWhite.Visible = LblModWhiteCode.Visible = IIModWhite.Visible = LblModWhiteUM.Visible = Config.SelectedTool.RequireModulation;
			LblModBlack.Visible = LblModBlackCode.Visible = IIModBlack.Visible = LblModBlackUM.Visible = Config.SelectedTool.RequireModulation;
			LblBorders.Visible = LblBorderCode.Visible = LblBorderUM.Visible = IIBorder.Visible = Config.SelectedTool.UseBorders;
			LblFilling.Visible = LblFillingCode.Visible = LblFillingUM.Visible = IIFilling.Visible = Config.SelectedTool.UseFilling;
			ResumeLayout();
		}


		public bool SupportPWM { get { return true; } }
	}
}
