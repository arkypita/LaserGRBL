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
	public partial class PreviewForm : System.Windows.Forms.UserControl
	{
		GrblCore Core;

		public PreviewForm()
		{
			InitializeComponent();
		}

		public void SetCore(GrblCore core)
		{
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
			BtnGoHome.Visible = Core.Configuration.HomingEnabled;
			BtnGoHome.Enabled = Core.CanGoHome;
			BtnUnlock.Enabled = Core.CanUnlock;
			BtnStop.Enabled = Core.CanFeedHold;
			BtnResume.Enabled = Core.CanResumeHold;

			foreach (CustomButtonIB ib in CustomButtonArea.Controls)
				ib.RefreshEnabled();

			ResumeLayout();
		}

		void BtnGoHomeClick(object sender, EventArgs e)
		{
			Core.EnqueueCommand(new GrblCommand("$H"));
		}
		void BtnResetClick(object sender, EventArgs e)
		{
			Core.GrblReset(true);
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

			CustomButtonArea.Controls.Clear();
			foreach (CustomButton cb in buttons)
			{
				CustomButtonIB ib = new CustomButtonIB(Core, cb, this);
				CustomButtonArea.Controls.Add(ib);
			}

		}

		private class CustomButtonIB : UserControls.ImageButton
		{
			private GrblCore Core;
			private LaserGRBL.CustomButton cb;
			PreviewForm form;
			private ToolTip tt;
			private ContextMenuStrip cms;
			private bool mDrawDisabled = true;

			public CustomButtonIB(GrblCore Core, LaserGRBL.CustomButton cb, PreviewForm form)
			{
				// TODO: Complete member initialization
				this.Core = Core;
				this.cb = cb;
				this.form = form;
				this.tt = new ToolTip();

				Image = cb.Image;
				Tag = cb;

				//ContextMenuStrip = MNRemEditCB;
				SizingMode = UserControls.ImageButton.SizingModes.FixedSize;
				BorderStyle = BorderStyle.FixedSingle;
				Size = new Size(49, 49);
				tt.SetToolTip(this, cb.ToolTip);


				cms = new ContextMenuStrip();
				cms.Items.Add(Strings.CustomButtonRemove, null, RemoveButton_Click);
				cms.Items.Add(Strings.CustomButtonEdit, null, EditButton_Click);

				this.ContextMenuStrip = cms;
			}


			protected override void Dispose(bool disposing)
			{
				tt.SetToolTip(this, null);
				base.Dispose(disposing);
			}

			public CustomButton CustomButton
			{ get { return Tag as CustomButton; } }

			internal void RefreshEnabled()
			{
				bool disabled = !CustomButton.EnabledNow(Core);
				if (mDrawDisabled != disabled)
				{
					mDrawDisabled = disabled;
					Refresh();
				}
			}

			protected override bool DrawDisabled()
			{
				return mDrawDisabled;
			}

			protected override void OnClick(EventArgs e)
			{
				if (mDrawDisabled || !CustomButton.EnabledNow(Core))
					return;

				string[] arr = cb.GCode.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
				foreach (string str in arr)
				{
					string command = str;
					if (command.Trim().Length > 0)
					{
						decimal left = Core.LoadedFile != null && Core.LoadedFile.Range.DrawingRange.ValidRange ? Core.LoadedFile.Range.DrawingRange.X.Min : 0;
						decimal right = Core.LoadedFile != null && Core.LoadedFile.Range.DrawingRange.ValidRange ? Core.LoadedFile.Range.DrawingRange.X.Max : 0;
						decimal top = Core.LoadedFile != null && Core.LoadedFile.Range.DrawingRange.ValidRange ? Core.LoadedFile.Range.DrawingRange.Y.Max : 0;
						decimal bottom = Core.LoadedFile != null && Core.LoadedFile.Range.DrawingRange.ValidRange ? Core.LoadedFile.Range.DrawingRange.Y.Min : 0;

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
				base.OnClick(e);
			}

			static string FormatNumber(decimal value)
			{
				return string.Format(System.Globalization.CultureInfo.GetCultureInfo("en-US"), "{0:0.000}", value);
			}


			private void RemoveButton_Click(object sender, EventArgs e)
			{
				List<CustomButton> buttons = (List<CustomButton>)Settings.GetObject("Custom Buttons", new List<CustomButton>());
				for (int i = 0; i < buttons.Count; i++)
				{
					if (buttons[i].guid == CustomButton.guid)
						buttons.Remove(buttons[i]);
				}
				Settings.Save();

				form.RefreshCustomButtons();
			}

			private void EditButton_Click(object sender, EventArgs e)
			{
				CustomButtonForm.CreateAndShowDialog(CustomButton);
				form.RefreshCustomButtons();
			}

		}

		private class MyFlowPanel : FlowLayoutPanel
		{
			public MyFlowPanel()
			{
				ResizeRedraw = true;
			}

			protected override void OnControlAdded(ControlEventArgs e)
			{
				base.OnControlAdded(e);
				Invalidate();
			}
			protected override void OnControlRemoved(ControlEventArgs e)
			{
				base.OnControlRemoved(e);
				Invalidate();
			}

			protected override void OnPaintBackground(PaintEventArgs e)
			{
				e.Graphics.Clear(BackColor);
				if (Controls.Count == 0)
				{
					SizeF size = e.Graphics.MeasureString("Right click here to add custom buttons", Font);
					e.Graphics.DrawString("Right click here to add custom buttons", Font, Brushes.DarkGray, (Width - size.Width) / 2, (Height - size.Height) / 2);
				}
			}
		}


		internal void OnColorChange()
		{
			Preview.OnColorChange();
		}
	}

}
