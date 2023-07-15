using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class SafetyCountdown : Form
	{
		int down;
		string lbl;
		static SoundPlayer player;

		public SafetyCountdown()
		{
			InitializeComponent();
			down = 5;
			lbl = BtnStart.Text;
			BtnStart.Text = $"{lbl} ({down})";
		}

		static SafetyCountdown()
		{
			try
			{
				if (player == null)
				{ 
					player = new SoundPlayer($"Sound\\beep.wav");
					player.Load();
				}
			}
			catch { }
		}

		private void EngravingStarting_Load(object sender, EventArgs e)
		{
		
		}

		private void TimCountDown_Tick(object sender, EventArgs e)
		{
			down = down - 1;

			BtnStart.Text = $"{lbl} ({down})";
			Application.DoEvents();

			if (down == 0)
			{
				DialogResult = DialogResult.OK;
				TimCountDown.Enabled = false;
			}
			else
			{
				DoBeep();
			}
		}

		private static void DoBeep()
		{
			if (!Settings.GetObject("QuietSafetyCountdown", false))
			{
				try { player.PlaySync(); }
				catch { }
			}
		}

		private void EngravingStarting_FormClosing(object sender, FormClosingEventArgs e)
		{
			TimCountDown.Enabled = false;
		}

		private void EngravingStarting_Shown(object sender, EventArgs e)
		{
			TimCountDown.Start();
			Application.DoEvents();
			DoBeep();
		}

		internal static bool CanGo()
		{
			if (Settings.GetObject("DisableSafetyCountdown", false))
				return true;

			using (SafetyCountdown es = new SafetyCountdown())
			{
				if (es.ShowDialog() != DialogResult.OK)
					return false;
			}

			return true;
		}
	}
}
