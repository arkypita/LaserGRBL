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

			move2DControl1.SpeedValue = Math.Min((int)Settings.GetObject("Jog Speed", 1000), move2DControl1.SpeedMaximum);
			SpeedValueChanged(null, null); //set tooltip
		}

		private void SpeedValueChanged(object sender, UserControls.Move2DControl.SpeedEventArgs e)
		{
			int speed = e == null ? Core.JogSpeed : (int)e.F;
			//TT.SetToolTip(move2DControl1, string.Format("Speed: {0}", speed));
			Settings.SetObject("Jog Speed", speed);
			Core.JogSpeed = speed;
			needsave = true;
		}


		private void Home_Click(object sender, UserControls.Move2DControl.HomeEventArgs e)
		{
			Core.JogSpeed = (int)e.F;
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
				var currentSpeed = move2DControl1.SpeedValue;
				move2DControl1.SpeedMaximum = curVal;
				move2DControl1.SpeedValue= Math.Min(currentSpeed, curVal);
				oldVal = curVal;
			}
		}

		private void Move_Click(object sender, UserControls.Move2DControl.MoveEventArgs e)
		{
			var move = e.Move;
			Core.JogSpeed = (int)e.F;
			Core.Move(move);
		}
	}
}
