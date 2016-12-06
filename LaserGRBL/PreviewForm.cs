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
	public partial class PreviewForm : UserControls.DockingManager.DockContent
	{
		GrblCom ComPort;
		GrblFile LoadedFile;
		
		public PreviewForm(GrblCom com, GrblFile file)
		{
			InitializeComponent();

			ComPort = com;
			LoadedFile = file;
			Preview.SetComProgram(com, file);
			TimerUpdate();
		}
		
		public void TimerUpdate()
		{
			Preview.TimerUpdate();
			SuspendLayout();
			BtnReset.Enabled = ComPort.IsOpen && ComPort.MachineStatus != GrblCom.MacStatus.Disconnected;
			BtnGoHome.Enabled = ComPort.IsOpen && (ComPort.MachineStatus == GrblCom.MacStatus.Idle || ComPort.MachineStatus == GrblCom.MacStatus.Alarm);
			BtnStop.Enabled = ComPort.IsOpen && ComPort.MachineStatus == GrblCom.MacStatus.Run;
			BtnResume.Enabled = ComPort.IsOpen && (ComPort.MachineStatus == GrblCom.MacStatus.Door || ComPort.MachineStatus == GrblCom.MacStatus.Hold);
			ResumeLayout();
		}

		void BtnGoHomeClick(object sender, EventArgs e)
		{
			ComPort.EnqueueCommand(new GrblCommand("$H"));
		}
		void BtnResetClick(object sender, EventArgs e)
		{
			ComPort.GrblReset();
		}
		void BtnStopClick(object sender, EventArgs e)
		{
			ComPort.FeedHold();
		}
		void BtnResumeClick(object sender, EventArgs e)
		{
			ComPort.CycleStartResume();
		}
	}

}
