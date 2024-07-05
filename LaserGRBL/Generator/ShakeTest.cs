using LaserGRBL.Icons;
using LaserGRBL.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.Generator
{
	public partial class ShakeTest : Form
	{
		GrblCore mCore;

		public ShakeTest(GrblCore core)
		{
			InitializeComponent();
            ThemeMgr.SetTheme(this, true);
            BackColor = ColorScheme.FormBackColor;
            ForeColor = ColorScheme.FormForeColor;
            IconsMgr.PrepareButton(BtnCreate, "mdi-checkbox-marked");
            IconsMgr.PrepareButton(BtnCancel, "mdi-close-box");
            mCore = core;
		}

		public static void CreateAndShowDialog(GrblCore core)
		{
			using (ShakeTest f = new ShakeTest(core))
				f.ShowDialog(FormsHelper.MainForm);
		}

		private void BtnCreate_Click(object sender, EventArgs e)
		{
			mCore.LoadedFile.GenerateShakeTest(CbAxis.SelectedItem as string, (int)(CbLimit.SelectedItem), IiAxisLen.CurrentValue, IiCrossPower.CurrentValue, IiCrossSpeed.CurrentValue);
			Close();
		}

		private void ShakeTest_Load(object sender, EventArgs e)
		{
			AssignMinMaxLimit();
			RestoreDefault();
		}

		private void RestoreDefault()
		{
			IiCrossSpeed.CurrentValue = 1000;
			IiCrossPower.CurrentValue = 100;
			CbAxis.SelectedIndex = 0;
		}

		private void AssignMinMaxLimit()
		{
			int maxrateX = 20000;
			int maxrateY = 20000;

			int maxPwm = 1000;

			try {maxrateX = (int)GrblCore.Configuration.MaxRateX;}
			catch (Exception ex) { }
			try { maxrateY = (int)GrblCore.Configuration.MaxRateY; }
			catch (Exception ex) { }
			try { maxPwm = (int)GrblCore.Configuration.MaxPWM; }
			catch (Exception ex) { }

			

			IiCrossSpeed.MaxValue = Math.Min( maxrateX, maxrateY);
			IiCrossPower.MaxValue = maxPwm;
		}

		private void CbAxis_SelectedIndexChanged(object sender, EventArgs e)
		{
			int maxrateA = 20000;
			int maxlenA = 400;

			try { maxrateA = ((string)CbAxis.SelectedItem == "X") ? (int)GrblCore.Configuration.MaxRateX : (int)GrblCore.Configuration.MaxRateY; }
			catch (Exception ex) { }

			try { maxlenA = ((string)CbAxis.SelectedItem == "X") ? (int)GrblCore.Configuration.TableWidth : (int)GrblCore.Configuration.TableHeight; }
			catch (Exception ex) { }

			IiAxisLen.MaxValue = maxlenA;
			IiAxisLen.CurrentValue = maxlenA;

			string mcurlimit = CbLimit.SelectedItem as string;
			CbLimit.BeginUpdate();
			CbLimit.Items.Clear();

			CbLimit.Items.Add(maxrateA);

			for (int i = maxrateA / 1000 * 1000; i > 0; i-= 1000)
				if (!CbLimit.Items.Contains(i))
					CbLimit.Items.Add(i);

			CbLimit.EndUpdate();

			if (mcurlimit != null && CbLimit.Items.Contains(mcurlimit))
				CbLimit.SelectedItem = mcurlimit;
			else
				CbLimit.SelectedIndex = 0;
		}
	}
}
