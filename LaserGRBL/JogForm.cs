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
		}

		public void SetCore(GrblCore core)
		{
			Core = core;

			UpdateFMax.Enabled = true;
			UpdateFMax_Tick(null, null);

			TbSpeed.Value = Math.Min((int)Settings.GetObject("Jog Speed", 1000), TbSpeed.Maximum);
			TbSpeed_ValueChanged(null, null); //set tooltip
		}

		private void TbSpeed_ValueChanged(object sender, EventArgs e)
		{
			TT.SetToolTip(TbSpeed, string.Format("Speed: {0}", TbSpeed.Value));
			LblSpeed.Text = String.Format("F{0}", TbSpeed.Value);
			Settings.SetObject("Jog Speed", TbSpeed.Value);
			Core.JogSpeed = TbSpeed.Value;
			needsave = true;
		}


		private void Home_Click(object sender, EventArgs e)
		{
			Core.JogHome();
		}

		bool needsave = false;
		private void OnSliderMouseUP(object sender, MouseEventArgs e)
		{
			if (needsave)
			{
				needsave = false;
				Settings.Save();
			}
		}

		int oldVal;
		private void UpdateFMax_Tick(object sender, EventArgs e)
		{
			int curVal = (int)Math.Max(Core.Configuration.MaxRateX, Core.Configuration.MaxRateY);
			if (oldVal != curVal)
			{
				TbSpeed.Value = Math.Min(TbSpeed.Value, curVal);
				TbSpeed.Maximum = curVal;
				TbSpeed.LargeChange = curVal / 10;
				TbSpeed.SmallChange = curVal / 20;
				oldVal = curVal;
			}
		}

		private void Move_Click(object sender, UserControls.Move2DControl.MoveEventArgs e)
		{
			var move = e.Move;
			Core.Move(move.Mouvement);
		}
	}
}
