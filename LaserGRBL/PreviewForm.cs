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
using System.Collections.Generic;

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

			RefreshCustomButtons();
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

			//CustomButtonArea.Enabled = Core.CanSendManualCommand;

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

		private void addCustomButtonToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CustomButtonForm.CreateAndShowDialog();
			RefreshCustomButtons();
		}

		private void RefreshCustomButtons()
		{
			List<CustomButton> buttons = (List<CustomButton>)Settings.GetObject("Custom Buttons", new List<CustomButton>());

			foreach (UserControls.ImageButton ib in CustomButtonArea.Controls)
			{
				ib.Click -= OnCustomButtonClick;
				TT.SetToolTip(ib, null);
			}

			CustomButtonArea.Controls.Clear();

			foreach (CustomButton cb in buttons)
			{
				UserControls.ImageButton ib = new UserControls.ImageButton();
				ib.Image = cb.Image;
				ib.Tag = cb;
				ib.Click += OnCustomButtonClick;
				ib.ContextMenuStrip = MNRemEditCB;
				ib.SizingMode = UserControls.ImageButton.SizingModes.FixedSize;
				ib.BorderStyle = BorderStyle.FixedSingle;
				ib.Size = new Size(49, 49);
				TT.SetToolTip(ib, cb.ToolTip);
				CustomButtonArea.Controls.Add(ib);
			}

		}

		private void OnCustomButtonClick(object sender, EventArgs e)
		{
			UserControls.ImageButton ib = sender as UserControls.ImageButton;
			if (ib != null)
			{
				CustomButton cb = ib.Tag as CustomButton;

				string[] arr = cb.GCode.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

				foreach (string str in arr)
				{
					string command = str;
					if (command.Trim().Length > 0)
					{
						decimal left = Core.LoadedFile != null ? Core.LoadedFile.Range.DrawingRange.X.Min : 0;
						decimal right = Core.LoadedFile != null ? Core.LoadedFile.Range.DrawingRange.X.Max : 0;
						decimal top = Core.LoadedFile != null ? Core.LoadedFile.Range.DrawingRange.Y.Max : 0;
						decimal bottom = Core.LoadedFile != null ? Core.LoadedFile.Range.DrawingRange.Y.Min : 0;

						decimal width = right - left;
						decimal height = top - bottom;

						command = command.Replace("[left]", FormatNumber(left));
						command = command.Replace("[right]", FormatNumber(right));
						command = command.Replace("[top]", FormatNumber(top));
						command = command.Replace("[bottom]", FormatNumber(bottom));

						command = command.Replace("[width]", FormatNumber(width));
						command = command.Replace("[height]", FormatNumber(height));

						Core.EnqueueCommand(new GrblCommand(command));
					}
				}
			}
		}

		static string FormatNumber(decimal value)
		{
			return string.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:0.000}", value);
		}

		private void RemoveButton_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem mi = sender as ToolStripMenuItem;
			ContextMenuStrip cms = mi.Owner as ContextMenuStrip;
			UserControls.ImageButton ib = cms.SourceControl as UserControls.ImageButton;
			if (ib != null)
			{
				CustomButton cb = ib.Tag as CustomButton;

				List<CustomButton> buttons = (List<CustomButton>)Settings.GetObject("Custom Buttons", new List<CustomButton>());
				for (int i = 0; i < buttons.Count; i++)
				{
					if (buttons[i].guid == cb.guid)
						buttons.Remove(buttons[i]);
				}
				Settings.Save();

				RefreshCustomButtons();
			}

		}

		private void editButtonToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem mi = sender as ToolStripMenuItem;
			ContextMenuStrip cms = mi.Owner as ContextMenuStrip;
			UserControls.ImageButton ib = cms.SourceControl as UserControls.ImageButton;
			if (ib != null)
			{
				CustomButton cb = ib.Tag as CustomButton;
				CustomButtonForm.CreateAndShowDialog(cb);
				RefreshCustomButtons();
			}
		}
	}

}
