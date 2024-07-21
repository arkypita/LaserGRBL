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
	public partial class CuttingTest : Form
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

		public CuttingTest(GrblCore core)
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
			using (CuttingTest f = new CuttingTest(core))
				f.ShowDialog(FormsHelper.MainForm);
		}


		private void GreyScaleForm_Load(object sender, EventArgs e)
		{
			CBLaserON.Items.Add(LaserOptions[0]);
			CBLaserON.Items.Add(LaserOptions[1]);
			CBLaserON.SelectedItem = LaserOptions[0];
			AssignMinMaxLimit();
			RestoreDefault();

		}

		private void RestoreDefault()
		{
			Ii_Pmin.CurrentValue = 1;
			Ii_Pmax.CurrentValue = 4;

			Ii_Sfixed.CurrentValue = 1000;

			Ii_Fmin.CurrentValue = 200;
			Ii_Fmax.CurrentValue = 1000;
			Ii_Fsteps.CurrentValue = 5;

			Ii_Ftext.CurrentValue = 1000;
			Ii_Stext.CurrentValue = 500;

			//recompute sizes
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
			Ii_Stext.MaxValue = maxPwm;
		}

		private void BtnCreate_Click(object sender, EventArgs e)
		{
			ComboboxItem mode = CBLaserON.SelectedItem as ComboboxItem;
			mCore.LoadedFile.GenerateCuttingTest(Ii_Fsteps.CurrentValue, Ii_Fmin.CurrentValue, Ii_Fmax.CurrentValue, Ii_Pmin.CurrentValue, Ii_Pmax.CurrentValue, Ii_Sfixed.CurrentValue, Ii_Ftext.CurrentValue, Ii_Stext.CurrentValue, PhTbLabel.Text, mode.Value as string);
			Close();
		}

		private void Ii_PCurrentValueChanged(object sender, int OldValue, int NewValue, bool ByUser)
		{
			if (ByUser)
			{
				if (sender == Ii_Pmin && Ii_Pmin.CurrentValue > Ii_Pmax.CurrentValue)
					Ii_Pmax.CurrentValue = Ii_Pmin.CurrentValue;

				if (sender == Ii_Pmax && Ii_Pmax.CurrentValue < Ii_Pmin.CurrentValue)
					Ii_Pmin.CurrentValue = Ii_Pmax.CurrentValue ;
				ComputeSize();
			}
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
				ComputeSize();
		}

		private void ComputeSize()
		{
			IISizeW.CurrentValue = Ii_Fsteps.CurrentValue * 14 - 4 + 3;
			IISizeH.CurrentValue = (Ii_Pmax.CurrentValue - Ii_Pmin.CurrentValue +1) * 14 - 4 + 3 + 3 + 3;
		}

		private void BtnOnOffInfo_Click(object sender, EventArgs e)
		{
			Tools.Utils.OpenLink(@"https://lasergrbl.com/usage/raster-image-import/target-image-size-and-laser-options/#laser-modes");
		}


	}
}
