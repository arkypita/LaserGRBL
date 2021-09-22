//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class JogForm : System.Windows.Forms.UserControl
	{
		GrblCore Core;

		public JogForm()
		{
			InitializeComponent();
            SettingsForm.SettingsChanged += SettingsForm_SettingsChanged;
		}

        public void SetCore(GrblCore core)
		{
			Core = core;

			UpdateFMax.Enabled = true;
			UpdateFMax_Tick(null, null);

			TbSpeed.Value = Math.Max(Math.Min(Settings.GetObject("Jog Speed", 1000), TbSpeed.Maximum), TbSpeed.Minimum);
            
			TbStep.Value = Convert.ToDecimal(Settings.GetObject("Jog Step", 10M));

			TbSpeed_ValueChanged(null, null); //set tooltip
			TbStep_ValueChanged(null, null); //set tooltip

            Core.JogStateChange += Core_JogStateChange;
            SettingsForm_SettingsChanged(this, null);
        }

        private void SettingsForm_SettingsChanged(object sender, EventArgs e)
        {
            TlpStepControl.Visible = !Settings.GetObject("Enable Continuous Jog", false);
            TlpZControl.Visible = Settings.GetObject("Enale Z Jog Control", false);
        }

        private void Core_JogStateChange(bool jog)
        {
            BtnHome.Visible = !jog;
        }

        private void OnJogButtonMouseDown(object sender, MouseEventArgs e)
		{
			Core.BeginJog((sender as DirectionButton).JogDirection, e.Button == MouseButtons.Right);
		}

        private void OnJogButtonMouseUp(object sender, MouseEventArgs e)
        {
            Core.EndJogV11();
        }

        private void OnZJogButtonMouseDown(object sender, MouseEventArgs e)
        {
            Core.EnqueueZJog((sender as DirectionStepButton).JogDirection, (sender as DirectionStepButton).JogStep, e.Button == MouseButtons.Right);
        }

        private void TbSpeed_ValueChanged(object sender, EventArgs e)
		{
			TT.SetToolTip(TbSpeed, $"{Strings.SpeedSliderToolTip} {TbSpeed.Value}");
			LblSpeed.Text = String.Format("F{0}", TbSpeed.Value);
			Settings.SetObject("Jog Speed", TbSpeed.Value);
			Core.JogSpeed = TbSpeed.Value;
		}

		private void TbStep_ValueChanged(object sender, EventArgs e)
		{
			TT.SetToolTip(TbStep, $"{Strings.StepSliderToolTip} {TbStep.Value}");
			LblStep.Text = TbStep.Value.ToString();
			Settings.SetObject("Jog Step", TbStep.Value);
			Core.JogStep = TbStep.Value;
		}

        public void ChangeJogStepIndexBy(int value)
        {
            TbStep.ChangeIndexBy(value);
        }

		internal void ChangeJogSpeedIndexBy(int v)
		{
			TbSpeed.Value = Math.Max(Math.Min(TbSpeed.Value + (TbSpeed.LargeChange * v), TbSpeed.Maximum), TbSpeed.Minimum);
		}

		int oldMax;
		private void UpdateFMax_Tick(object sender, EventArgs e)
		{
			int curMax = (int)Math.Max(TbSpeed.Minimum, Math.Max(Core.Configuration.MaxRateX, Core.Configuration.MaxRateY));

			if (oldMax != curMax)
			{
				oldMax = curMax;
				TbSpeed.Maximum = curMax;
				
				TbSpeed.LargeChange = Math.Max(1, curMax / 10);
				TbSpeed.SmallChange = Math.Max(1, curMax / 20);
			}
		}

	}

    public class StepBar : System.Windows.Forms.TrackBar
    {
        decimal[] values = { 0.1M, 0.2M, 0.5M, 1, 2, 5, 10, 20, 50, 100, 200 };

        public StepBar()
        {
            Minimum = 0;
            Maximum = values.Length -1;
            SmallChange = LargeChange = 1;
        }

        private int CurIndex { get { return base.Value; } set { base.Value = value; } }

        public new decimal Value
        {
            get
            {
                return values[CurIndex];
            }
            set
            {
                int found = 0;
                for (int index = 0; index < values.Length; index++)
                {
                    if (Math.Abs(value - values[index]) < Math.Abs(value - values[found]))
                        found = index;
                }
                CurIndex = found;
            }
        }

        public void ChangeIndexBy(int value)
        {
            CurIndex = Math.Max(Math.Min(CurIndex + value, Maximum), Minimum);
        }
    }


    public class DirectionButton : UserControls.ImageButton
	{
		private GrblCore.JogDirection mDir = GrblCore.JogDirection.N;

		public GrblCore.JogDirection JogDirection
		{
			get { return mDir; }
			set { mDir = value; }
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			if (Width != Height)
				Width = Height;

			base.OnSizeChanged(e);
		}
	}

    public class DirectionStepButton : DirectionButton
    {
        private decimal mStep = 1.0M;

        public decimal JogStep
        {
            get { return mStep; }
            set { mStep = value; }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (Width != Height)
                Width = Height;

            base.OnSizeChanged(e);
        }
    }
}
