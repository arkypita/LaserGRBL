using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class OverridesForm : UserControls.DockingManager.DockContent
	{
		GrblCom ComPort;
		
//		private int mDelayedSpeed = 0;
//		private int mDelayedPower = 0;
//		private bool mIgnoreEvent = false;

		public OverridesForm(GrblCom com)
		{
			InitializeComponent();
			ComPort = com;
			ComPort.OnOverrideChange += ComPort_OnOverrideChange;
			ComPort_OnOverrideChange();
			TimerUpdate();
		}

		public void TimerUpdate()
		{
			SuspendLayout();
			foreach (Control ctr in tlp1.Controls)
				ctr.Enabled = (ComPort.MachineStatus != GrblCom.MacStatus.Disconnected && ComPort.SupportOverride);
			ResumeLayout();
		}

		void ComPort_OnOverrideChange()
		{
			if (InvokeRequired)
			{
				BeginInvoke(new GrblCom.dlgOnOverrideChange(ComPort_OnOverrideChange));
			}
			else
			{
				SuspendLayout();
				LblSpeed.Text = string.Format("Speed [{0:0.00}x]", ComPort.OverrideFeed / 100.0);
				LblRapid.Text = string.Format("Rapid [{0:0.00}x]", ComPort.OverrideRapids / 100.0);
				LblPower.Text = string.Format("Power [{0:0.00}x]", ComPort.OverrideSpindle / 100.0);
				ResumeLayout();
			}
		}
		
		/*
		Rapid Overrides
		Immediately alters the rapid override value. An active rapid motion is altered within tens of milliseconds.
		Only effects rapid motions, which include G0, G28, and G30.
		If rapid override value does not change, the command is ignored.
		Rapid override set values may be changed in config.h.
		The commands are:
		0x95 : Set to 100% full rapid rate.
		0x96 : Set to 50% of rapid rate.
		0x97 : Set to 25% of rapid rate.

		Feed Overrides
		Immediately alters the feed override value. An active feed motion is altered within tens of milliseconds.
		Does not alter rapid rates, which include G0, G28, and G30, or jog motions.
		Feed override value can not be 10% or greater than 200%.
		If feed override value does not change, the command is ignored.
		Feed override range and increments may be changed in config.h.
		The commands are:
		0x90 : Set 100% of programmed rate.
		0x91 : Increase 10%
		0x92 : Decrease 10%
		0x93 : Increase 1%
		0x94 : Decrease 1%

		Spindle Speed Overrides
		Immediately alters the spindle speed override value. An active spindle speed is altered within tens of milliseconds.
		Override values may be changed at any time, regardless of if the spindle is enabled or disabled.
		Spindle override value can not be 10% or greater than 200%
		If spindle override value does not change, the command is ignored.
		Spindle override range and increments may be altered in config.h.
		The commands are:
		0x99 : Set 100% of programmed spindle speed
		0x9A : Increase 10%
		0x9B : Decrease 10%
		0x9C : Increase 1%
		0x9D : Decrease 1%
		*/
		
		void SpeedCommandButtonMouseDown(object sender, MouseEventArgs e)
		{
			ImmediateCommandButton btn = sender as ImmediateCommandButton;
			ComPort.SendImmediate(btn.ImmediateCommand);
		}
		void PowerCommandButtonMouseDown(object sender, MouseEventArgs e)
		{
			ImmediateCommandButton btn = sender as ImmediateCommandButton;
			ComPort.SendImmediate(btn.ImmediateCommand);	
		}
		void RapidCommandButtonMouseDown(object sender, MouseEventArgs e)
		{
			ImmediateCommandButton btn = sender as ImmediateCommandButton;
			ComPort.SendImmediate(btn.ImmediateCommand);		
		}
	}
	
	public class ImmediateCommandButton : LaserGRBL.UserControls.ImageButton
	{
		
		private byte mIc = 0;
		
		public byte ImmediateCommand
		{ 
			get {return mIc;}
			set {mIc = value;}
		}
	}
}


