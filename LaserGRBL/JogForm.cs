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
			TbStep.Value = (int)Settings.GetObject("Jog Step", 10);

			TbSpeed_ValueChanged(null, null); //set tooltip
			TbStep_ValueChanged(null, null); //set tooltip
		}

		private void OnJogButtonMouseDown(object sender, MouseEventArgs e)
		{
			Core.Jog((sender as DirectionButton).JogDirection);
		}

		private void TbSpeed_ValueChanged(object sender, EventArgs e)
		{
			TT.SetToolTip(TbSpeed, string.Format("Speed: {0}", TbSpeed.Value));
			LblSpeed.Text = String.Format("F{0}", TbSpeed.Value);
			Settings.SetObject("Jog Speed", TbSpeed.Value);
			Core.JogSpeed = TbSpeed.Value;
			needsave = true;
		}

		private void TbStep_ValueChanged(object sender, EventArgs e)
		{
			TT.SetToolTip(TbStep, string.Format("Step: {0}", TbStep.Value));
			LblStep.Text = TbStep.Value.ToString();
			Settings.SetObject("Jog Step", TbStep.Value);
			Core.JogStep = TbStep.Value;
			needsave = true;
		}

		private void BtnHome_Click(object sender, EventArgs e)
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
}
