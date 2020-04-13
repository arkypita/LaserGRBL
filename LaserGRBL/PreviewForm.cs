//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

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
			CustomButtonArea.OrderChanged += CustomButtonArea_OrderChanged;
		}

		private void CustomButtonArea_OrderChanged(int oldindex, int newindex)
		{
			CustomButtons.Reorder(oldindex, newindex);
			RefreshCustomButtons();
		}

		public void SetCore(GrblCore core)
		{
			Core = core;
			Preview.SetComProgram(core);

            BtnUnlock.Visible = Core.Type == Firmware.Grbl;

            RefreshCustomButtons();
			TimerUpdate();
		}

		public void TimerUpdate()
		{
			Preview.TimerUpdate();
			SuspendLayout();
			BtnReset.Enabled = Core.CanResetGrbl;
			BtnHoming.Visible = Core.Configuration.HomingEnabled;
			BtnHoming.Enabled = Core.CanDoHoming;
			BtnUnlock.Enabled = Core.CanUnlock;
			BtnStop.Enabled = Core.CanFeedHold;
			BtnResume.Enabled = Core.CanResumeHold;
			BtnZeroing.Enabled = Core.CanDoZeroing;

			foreach (CustomButtonIB ib in CustomButtonArea.Controls)
				ib.RefreshEnabled();

			ResumeLayout();
		}

		void BtnGoHomeClick(object sender, EventArgs e)
		{
			Core.GrblHoming();
		}
		void BtnResetClick(object sender, EventArgs e)
		{
			Core.GrblReset();
		}
		void BtnStopClick(object sender, EventArgs e)
		{
			Core.FeedHold(false);
		}
		void BtnResumeClick(object sender, EventArgs e)
		{
			Core.CycleStartResume(false);
		}

		private void BtnUnlockClick(object sender, EventArgs e)
		{
			Core.GrblUnlock();
		}

		private void addCustomButtonToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CustomButtonForm.CreateAndShowDialog();
			RefreshCustomButtons();
		}

		private void RefreshCustomButtons()
		{
			CustomButtonArea.Controls.Clear();
			foreach (CustomButton cb in CustomButtons.Buttons)
			{
				CustomButtonIB ib = new CustomButtonIB(Core, cb, this);
				CustomButtonArea.Controls.Add(ib);
			}

		}

        public List<CustomButtonIB> CustomImageButtons
        {
            get
            {
                List<CustomButtonIB> rv = new List<CustomButtonIB>();
                foreach (Control c in CustomButtonArea.Controls)
                    if (c is CustomButtonIB)
                        rv.Add(c as CustomButtonIB);
                return rv;
            }
        }

		public class CustomButtonIB : UserControls.ImageButton
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
				Margin = new Padding(2);
				this.Caption = cb.Caption;
				tt.SetToolTip(this, cb.ToolTip);


				cms = new ContextMenuStrip();
				cms.Items.Add(Strings.CustomButtonRemove, null, RemoveButton_Click);
				cms.Items.Add(Strings.CustomButtonEdit, null, EditButton_Click);

				ContextMenuStrip = cms;
				AllowDrag = true;
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
            {PerformClick(e);}

            private bool mEmulateMouseInside;
            public bool EmulateMouseInside
            {
                get { return mEmulateMouseInside; }
                set { mEmulateMouseInside = value; Invalidate(); }
            }

            public override bool IsMouseInside()
            {
                return EmulateMouseInside || base.IsMouseInside();
            }

            private bool on;
            public void PerformClick(EventArgs e)
            {
                if (((MouseEventArgs)e).Button != MouseButtons.Left)
                    return;

                if (mDrawDisabled || !CustomButton.EnabledNow(Core))
                    return;

                if (cb.ButtonType == CustomButton.ButtonTypes.Button)
                    Core.ExecuteCustombutton(cb.GCode);

                if (cb.ButtonType == CustomButton.ButtonTypes.TwoStateButton)
                {
                    on = !on;
                    Core.ExecuteCustombutton(on ? cb.GCode : cb.GCode2);
                    BackColor = on ? Color.Orange : Parent.BackColor;
                }

                base.OnClick(e);
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
				_mX = e.X;
				_mY = e.Y;
				this._isDragging = false;

				PerformMouseDown(e);
			}

            public void PerformMouseDown(MouseEventArgs e)
            {
                if (e.Button != MouseButtons.Left)
                    return;

                if (mDrawDisabled || !CustomButton.EnabledNow(Core))
                    return;

                if (cb.ButtonType == CustomButton.ButtonTypes.PushButton)
                {
                    Core.ExecuteCustombutton(cb.GCode);
                    BackColor = Color.LightBlue;
                }

                base.OnMouseDown(e);
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
				_isDragging = false;
				PerformMouseUp(e);
			}

            public void PerformMouseUp(MouseEventArgs e)
            {
                if (e.Button != MouseButtons.Left)
                    return;

                if (mDrawDisabled || !CustomButton.EnabledNow(Core))
                    return;

                if (cb.ButtonType == CustomButton.ButtonTypes.PushButton)
                {
                    Core.ExecuteCustombutton(cb.GCode2);
                    BackColor = Parent.BackColor;
                }

                base.OnMouseUp(e);
            }

            private void RemoveButton_Click(object sender, EventArgs e)
			{
				if (MessageBox.Show(Strings.BoxDeleteCustomButtonText, Strings.BoxDeleteCustomButtonTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
				{
					CustomButtons.Remove(CustomButton);
					CustomButtons.SaveFile();
					form.RefreshCustomButtons();
				}
			}

			private void EditButton_Click(object sender, EventArgs e)
			{
				CustomButtonForm.CreateAndShowDialog(CustomButton);
				form.RefreshCustomButtons();
			}


			#region DragDrop


			//Check radius for begin drag n drop
			public bool AllowDrag { get; set; }
			private bool _isDragging = false;
			private int _DDradius = 40;
			private int _mX = 0;
			private int _mY = 0;

			protected override void OnMouseMove(MouseEventArgs e)
			{
				if (!_isDragging)
				{
					// This is a check to see if the mouse is moving while pressed.
					// Without this, the DragDrop is fired directly when the control is clicked, now you have to drag a few pixels first.
					if (e.Button == MouseButtons.Left && _DDradius > 0 && this.AllowDrag)
					{
						int num1 = _mX - e.X;
						int num2 = _mY - e.Y;
						if (((num1 * num1) + (num2 * num2)) > _DDradius)
						{
							DoDragDrop(this, DragDropEffects.Move);
							_isDragging = true;
							return;
						}
					}
					base.OnMouseMove(e);
				}
			}

			#endregion

		}

		private class MyFlowPanel : FlowLayoutPanel
		{
			public delegate void OrderChangedDlg(int oldindex, int newindex);
			public event OrderChangedDlg OrderChanged;

			public MyFlowPanel()
			{
				AllowDrop = true;
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
					string text = Strings.AddCustomButtonsHint;
					SizeF size = e.Graphics.MeasureString(text, Font);
					e.Graphics.DrawString(text, Font, Brushes.DarkGray, (Width - size.Width) / 2, (Height - size.Height) / 2);
				}
			}

			protected override void OnDragEnter(DragEventArgs drgevent)
			{
				CustomButtonIB btn = (CustomButtonIB)drgevent.Data.GetData(typeof(CustomButtonIB));
				if (btn != null)
					drgevent.Effect = DragDropEffects.Move;
				else
					drgevent.Effect = DragDropEffects.None;

				base.OnDragEnter(drgevent);
			}

			int dragInsertIndex = -1;
			protected override void OnDragOver(DragEventArgs drgevent)
			{
				MyFlowPanel dst = this;

				Point p = dst.PointToClient(new Point(drgevent.X, drgevent.Y));
				Control item = dst.GetChildAtPoint(p);
				if (item == null) // move the point a bit to the left, if the cursor is between two buttons
				{
					p.X -= 6;
					item = dst.GetChildAtPoint(p);
				}

				if (item != null)
				{
					int currentIndex = dst.Controls.GetChildIndex(item, false);

					Point pRelativeToControl = item.PointToClient(new Point(drgevent.X, drgevent.Y));
					double xInPercent = pRelativeToControl.X / (double)item.Width * 100.0;
					bool isLastItem = false;

					if (xInPercent > 50.0)
					{
						if (++currentIndex >= dst.Controls.Count)
						{
							item = dst.Controls[dst.Controls.Count - 1];
							isLastItem = true;
						}
						else
						{
							item = dst.Controls[currentIndex];
						}
					}

					if (item != null && (dragInsertIndex == -1 || currentIndex != dragInsertIndex))
					{

						Graphics g = this.CreateGraphics();
						g.Clear(this.BackColor);
						dragInsertIndex = currentIndex;
						int xPos = item.Location.X - 3;
						if (isLastItem)
						{
							xPos = item.Location.X + item.Width + 1;
						}
						g.FillRectangle(Brushes.CornflowerBlue, xPos, 2, 2, item.Height);
					}
				}

				base.OnDragOver(drgevent);
			}

			protected override void OnDragDrop(DragEventArgs drgevent)
			{
				CustomButtonIB btn = (CustomButtonIB)drgevent.Data.GetData(typeof(CustomButtonIB));
				MyFlowPanel dst = this;
				MyFlowPanel src = btn?.Parent as MyFlowPanel;

				if (btn != null && src != null && dst != null &&  src == dst)
				{
					drgevent.Effect = DragDropEffects.Move;

					int oldindex = dst.Controls.GetChildIndex(btn);					
					OrderChanged?.Invoke(oldindex, dragInsertIndex);
					dragInsertIndex = -1;
				}
				else
				{
					drgevent.Effect = DragDropEffects.None;
				}

				base.OnDragDrop(drgevent);
			}

		}


		internal void OnColorChange()
		{
			Preview.OnColorChange();
		}

		private void BtnZeroing_Click(object sender, EventArgs e)
		{
			Core.SetNewZero();
		}

		private void exportCustomButtonsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CustomButtons.Export();
		}

		private void importCustomButtonsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (CustomButtons.Import())
			{
				RefreshCustomButtons();
				CustomButtons.SaveFile();
			}
		}

		private void MNAddCB_Opening(object sender, CancelEventArgs e)
		{
			exportCustomButtonsToolStripMenuItem.Enabled = CustomButtons.Count > 0;
		}
	}

}
