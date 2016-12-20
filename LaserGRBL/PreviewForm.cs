/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 05/12/2016
 * Time: 23:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL
{
	/// <summary>
	/// Description of PreviewForm.
	/// </summary>
	public partial class PreviewForm : LaserGRBL.UserControls.DockingManager.DockContent
	{
		GrblCore Core;
		
		public PreviewForm(GrblCore core)
		{
			InitializeComponent();

			Core = core;
			Preview.SetComProgram(core);
			TimerUpdate();
		}
		
		public void TimerUpdate()
		{
			Preview.TimerUpdate();
			SuspendLayout();
			BtnReset.Enabled = Core.CanResetGrbl;
			BtnGoHome.Enabled = Core.CanGoHome;
			BtnUnlock.Enabled = Core.CanGoHome;
			BtnStop.Enabled = Core.CanFeedHold;
			BtnResume.Enabled = Core.CanResumeHold;
			ResumeLayout();
		}

		void BtnGoHomeClick(object sender, EventArgs e)
		{
			Core.EnqueueCommand(new GrblCommand("$H"));
		}
		void BtnResetClick(object sender, EventArgs e)
		{
			Core.GrblReset();
		}
		void BtnStopClick(object sender, EventArgs e)
		{
			Core.FeedHold();
		}
		void BtnResumeClick(object sender, EventArgs e)
		{
			Core.CycleStartResume();
		}

		private void BtnUnlockClick(object sender, EventArgs e)
		{
			Core.EnqueueCommand(new GrblCommand("$X"));
		}
	}

}
