using LaserGRBL.Icons;
using LaserGRBL.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaserGRBL.Generator
{
	public partial class PowerVsSpeedForm : Form
	{
		GrblCore mCore;

		public ComboboxItem[] LaserOptions = new ComboboxItem[] { new ComboboxItem("M3 - Constant Power", "M3"), new ComboboxItem("M4 - Dynamic Power", "M4") };
		public class ComboboxItem
		{
			public string Text { get; set; }
			public object Value { get; set; }

			public ComboboxItem(string text, object value)
			{ Text = text; Value = value; }

			public override string ToString()
			{
				return Text;
			}
		}

		public PowerVsSpeedForm(GrblCore core)
		{
			InitializeComponent();
			ThemeMgr.SetTheme(this, true);
			BackColor = ColorScheme.FormBackColor;
			ForeColor = ColorScheme.FormForeColor;
			IconsMgr.PrepareButton(BtnOnOffInfo, "mdi-information-slab-box", new Size(16, 16));
            IconsMgr.PrepareButton(BtnCreate, "mdi-checkbox-marked");
            IconsMgr.PrepareButton(BtnCancel, "mdi-close-box");
            mCore = core;
		}

		public static void CreateAndShowDialog(GrblCore core)
		{
			using (PowerVsSpeedForm f = new PowerVsSpeedForm(core))
				f.ShowDialog(FormsHelper.MainForm);
		}


		private void GreyScaleForm_Load(object sender, EventArgs e)
		{
			CBLaserON.Items.Add(LaserOptions[0]);
			CBLaserON.Items.Add(LaserOptions[1]);
			CBLaserON.SelectedItem = LaserOptions[1];
			AssignMinMaxLimit();
			RestoreDefault();

		}

		private void RestoreDefault()
		{
			Ii_Smin.CurrentValue = 100;
			Ii_Smax.CurrentValue = 1000;

			Ii_Fmin.CurrentValue = 1000;
			Ii_Fmax.CurrentValue = 4000;

			UDQuality.Value = 8;

			Ii_Ssteps.CurrentValue = 10;
			Ii_Fsteps.CurrentValue = 7;

			Ii_Ftext.CurrentValue = 1000;
			Ii_Stext.CurrentValue = 500;

			//recompute sizes
			IiS_CurrentValueChanged(null, 0, 0, true);
			IiF_CurrentValueChanged(null, 0, 0, true);
		}

		private void AssignMinMaxLimit()
		{
			int maxrateX = 20000;
			int maxPwm = 1000;

			try
			{
				maxrateX = (int)GrblCore.Configuration.MaxRateX;
				maxPwm = (int)GrblCore.Configuration.MaxPWM;
			}
			catch (Exception ex) { }

			Ii_Ftext.MaxValue = Ii_Fmin.MaxValue = Ii_Fmax.MaxValue = maxrateX;
			Ii_Stext.MaxValue = Ii_Smin.MaxValue = Ii_Smax.MaxValue = maxPwm;
		}

		private void BtnCreate_Click(object sender, EventArgs e)
		{
			ComboboxItem mode = CBLaserON.SelectedItem as ComboboxItem;
			mCore.LoadedFile.GenerateGreyscaleTest(Ii_Fsteps.CurrentValue, Ii_Ssteps.CurrentValue, Ii_Fmin.CurrentValue, Ii_Fmax.CurrentValue, Ii_Smin.CurrentValue, Ii_Smax.CurrentValue, (int)IISizeW.CurrentValue, (int)IISizeH.CurrentValue, (double)UDQuality.Value, Ii_Ftext.CurrentValue, Ii_Stext.CurrentValue, PhTbLabel.Text, Ii_Ftext.CurrentValue, Ii_Stext.CurrentValue, mode.Value as string);
			Close();
		}

		private void IiS_CurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			if (ByUser)
			{
				//recompute step size
				if (Ii_Ssteps.CurrentValue == 1)
				{
					Ii_Smin.Enabled = false;
					Ii_Smin.CurrentValue = Ii_Smax.CurrentValue;
					Ii_SStepSize.CurrentValue = 0;
				}
				else
				{
					if (Ii_Smin.Enabled == false)
					{
						Ii_Smin.CurrentValue = 0;
						Ii_Smin.Enabled = true;
					}
					Ii_SStepSize.CurrentValue = (int)((Ii_Smax.CurrentValue - Ii_Smin.CurrentValue) / (Ii_Ssteps.CurrentValue - 1));
				}
			}

			if (Equals(sender, Ii_Ssteps) || sender == null)
				IISizeW.CurrentValue = Ii_Ssteps.CurrentValue * 10;
		}

		private void IiF_CurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			if (ByUser)
			{
				//recompute step size
				if (Ii_Fsteps.CurrentValue == 1)
				{
					Ii_Fmin.Enabled = false;
					Ii_Fmin.CurrentValue = Ii_Fmax.CurrentValue;
					Ii_FStepSize.CurrentValue = 0;
				}
				else
				{
					if (Ii_Fmin.Enabled == false)
					{
						Ii_Fmin.CurrentValue = 0;
						Ii_Fmin.Enabled = true;
					}
					Ii_FStepSize.CurrentValue = (int)((Ii_Fmax.CurrentValue - Ii_Fmin.CurrentValue) / (Ii_Fsteps.CurrentValue - 1));
				}
			}

			if (Equals(sender, Ii_Fsteps) || sender == null)
				IISizeH.CurrentValue = Ii_Fsteps.CurrentValue * 10;
		}

		private void BtnOnOffInfo_Click(object sender, EventArgs e)
		{
			Tools.Utils.OpenLink(@"https://lasergrbl.com/usage/raster-image-import/target-image-size-and-laser-options/#laser-modes");
		}
	}
}
