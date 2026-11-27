// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.PrintAndCut
{
	/// <summary>
	/// Print and Cut wizard (Alt+0), modeled on LightBurn's: choose a mark, jog on its printed center, Set;
	/// same for the second mark; then Scaled / No Scaling. The wizard closes after choosing the alignment.
	/// Hotkeys and jog keys keep working while the wizard has the focus.
	/// </summary>
	public partial class PrintAndCutForm : Form
	{
		private readonly GrblCore mCore;
		private bool mFilling;
		private bool mAiming;
		private Keys mLastKeyData = Keys.None;

		private sealed class MarkItem
		{
			public readonly RegistrationMark Mark;
			public MarkItem(RegistrationMark mark) { Mark = mark; }
			public override string ToString()
			{ return string.Format(Strings.PnCMarkItem, Mark.Number, Mark.Center.X, Mark.Center.Y); }
		}

		/// <summary>Creates the wizard for the given core.</summary>
		public PrintAndCutForm(GrblCore core)
		{
			if (core == null) throw new ArgumentNullException("core");
			mCore = core;

			InitializeComponent();

			Text = Strings.PnCWizardTitle;
			GbFirst.Text = Strings.PnCFirstTarget;
			GbSecond.Text = Strings.PnCSecondTarget;
			BtnSetFirst.Text = BtnSetSecond.Text = Strings.PnCSet;
			BtnJogFirst.Text = BtnJogSecond.Text = Strings.PnCJog;
			BtnAim.Text = Strings.PnCAim;
			BtnScaled.Text = Strings.PnCScaled;
			BtnNoScaling.Text = Strings.PnCNoScaling;
			BtnReset.Text = Strings.PnCMenuReset;

			TT.SetToolTip(BtnSetFirst, Strings.PnCSetTip);
			TT.SetToolTip(BtnSetSecond, Strings.PnCSetTip);
			TT.SetToolTip(BtnJogFirst, Strings.PnCJogTip);
			TT.SetToolTip(BtnJogSecond, Strings.PnCJogTip);
			TT.SetToolTip(BtnAim, Strings.PnCAimTip);
			TT.SetToolTip(BtnScaled, Strings.PnCScaledTip);
			TT.SetToolTip(BtnNoScaling, Strings.PnCNoScalingTip);

			LblFirst.ForeColor = Color.Red;   // LightBurn: first target red
			LblSecond.ForeColor = Color.Blue; // second target blue

			mCore.PrintAndCutManager.StateChanged += OnStateChanged;
			mCore.OnFileLoaded += OnFileLoaded;
		}

		private void PrintAndCutForm_Load(object sender, EventArgs e)
		{
			if (Owner != null) // top right corner of the main window
				Location = new Point(Math.Max(Owner.Left, Owner.Right - Width - 40), Owner.Top + 80);

			FillMarks();
			RefreshState();
		}

		private void PrintAndCutForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			StopAim();
			RefreshTimer.Stop();
			mCore.PrintAndCutManager.StateChanged -= OnStateChanged;
			mCore.OnFileLoaded -= OnFileLoaded;
		}

		private void OnStateChanged(object sender, EventArgs e)
		{
			if (IsDisposed || !IsHandleCreated) return;
			if (InvokeRequired) BeginInvoke(new Action(RefreshState));
			else RefreshState();
		}

		private void OnFileLoaded(long elapsed, string filename)
		{
			if (IsDisposed || !IsHandleCreated) return;
			BeginInvoke(new Action(delegate { FillMarks(); RefreshState(); }));
		}

		#region marks

		private void FillMarks()
		{
			mFilling = true;
			try
			{
				PrintAndCutManager pnc = mCore.PrintAndCutManager;
				List<RegistrationMark> marks = mCore.LoadedFile.RegistrationMarks;
				Fill(CbFirst, marks, pnc.FirstMarkSelection);
				Fill(CbSecond, marks, pnc.SecondMarkSelection);
			}
			finally { mFilling = false; }

			RefreshInfo();
		}

		private static void Fill(ComboBox cb, List<RegistrationMark> marks, int selected)
		{
			cb.BeginUpdate();
			cb.Items.Clear();
			foreach (RegistrationMark m in marks)
			{
				int index = cb.Items.Add(new MarkItem(m));
				if (m.Number == selected) cb.SelectedIndex = index;
			}
			cb.EndUpdate();
		}

		private void CbFirst_SelectedIndexChanged(object sender, EventArgs e)
		{
			MarkItem item = CbFirst.SelectedItem as MarkItem;
			if (!mFilling && item != null) mCore.PrintAndCutManager.FirstMarkSelection = item.Mark.Number;
		}

		private void CbSecond_SelectedIndexChanged(object sender, EventArgs e)
		{
			MarkItem item = CbSecond.SelectedItem as MarkItem;
			if (!mFilling && item != null) mCore.PrintAndCutManager.SecondMarkSelection = item.Mark.Number;
		}

		private void RefreshInfo()
		{
			List<RegistrationMark> marks = mCore.LoadedFile.RegistrationMarks;
			string color = PrintAndCutSettings.DefaultReservedColor;
			try { color = ColorTranslator.ToHtml(PrintAndCutSettings.ReservedColor); }
			catch { }

			StringBuilder sb = new StringBuilder();
			if (marks.Count == 0) sb.Append(string.Format(Strings.PnCNoMarks, color));
			else if (marks.Count == 1) sb.Append(string.Format(Strings.PnCOneMark, color));
			else sb.Append(string.Format(Strings.PnCMarksFound, marks.Count, color));

			foreach (RegistrationMark m in marks)
				if (m.IsOversized) sb.Append("\n").Append(PrintAndCutActions.OversizedText(m));

			LblInfo.Text = sb.ToString();
			LblInfo.ForeColor = marks.Count < 2 ? Color.DarkRed : SystemColors.ControlText;
		}

		#endregion

		#region state

		private void RefreshTimer_Tick(object sender, EventArgs e)
		{
			if (mAiming && !PrintAndCutActions.CanAim(mCore))
				StopAim(); // disconnected, alarm, job started...
			RefreshState();
		}

		private void RefreshState()
		{
			if (IsDisposed) return;

			PrintAndCutManager pnc = mCore.PrintAndCutManager;
			bool ready = PrintAndCutActions.MachineReady(mCore);
			bool firstMark = PrintAndCutActions.FindMark(mCore, pnc.FirstMarkSelection) != null;
			bool secondMark = PrintAndCutActions.FindMark(mCore, pnc.SecondMarkSelection) != null;

			BtnSetFirst.Enabled = BtnJogFirst.Enabled = ready && firstMark;
			BtnSetSecond.Enabled = BtnJogSecond.Enabled = ready && secondMark;
			BtnAim.Enabled = mAiming || ready; // when the machine cannot be aimed safely, the click explains why
			BtnScaled.Enabled = BtnNoScaling.Enabled = pnc.CanAlign;
			BtnReset.Enabled = pnc.CanAlign || pnc.HasFirstTarget || pnc.HasSecondTarget || pnc.IsEnabled;

			SetText(LblFirst, TargetText(pnc.FirstMark, pnc.FirstMachinePosition));
			SetText(LblSecond, TargetText(pnc.SecondMark, pnc.SecondMachinePosition));
			SetText(LblStatus, StatusText(pnc));
			LblStatus.ForeColor = pnc.IsEnabled ? Color.DarkGreen : SystemColors.ControlText;
		}

		private static void SetText(Label l, string text)
		{
			if (l.Text != text) l.Text = text; // avoid flickering on timer refresh
		}

		private static string TargetText(RegistrationMark mark, Vec2? machine)
		{
			if (mark == null || !machine.HasValue) return Strings.PnCTargetNotSet;
			return string.Format(Strings.PnCTargetSet, mark.Number, machine.Value.X, machine.Value.Y);
		}

		private string StatusText(PrintAndCutManager pnc)
		{
			if (!pnc.IsEnabled) return Strings.PnCStatusDisabled;

			AlignmentTransform t = pnc.GetTransform(mCore.PrintAndCutWorkOffset);
			if (t == null) return Strings.PnCStatusDisabled;
			return string.Format(Strings.PnCStatusAligned, pnc.AllowScaling ? Strings.PnCModeScaled : Strings.PnCModeNoScaling, t.AngleDegrees, t.Scale, t.SecondTargetError);
		}

		#endregion

		#region buttons

		private void BtnSetFirst_Click(object sender, EventArgs e)
		{ PrintAndCutActions.SetTarget(this, mCore, true); RefreshState(); }

		private void BtnSetSecond_Click(object sender, EventArgs e)
		{ PrintAndCutActions.SetTarget(this, mCore, false); RefreshState(); }

		private void BtnJogFirst_Click(object sender, EventArgs e)
		{ PrintAndCutActions.JogToSelection(this, mCore, true); }

		private void BtnJogSecond_Click(object sender, EventArgs e)
		{ PrintAndCutActions.JogToSelection(this, mCore, false); }

		private void BtnScaled_Click(object sender, EventArgs e)
		{
			if (PrintAndCutActions.Align(this, mCore, true))
				Close(); // as in LightBurn, the wizard closes after choosing the alignment
		}

		private void BtnNoScaling_Click(object sender, EventArgs e)
		{
			if (PrintAndCutActions.Align(this, mCore, false))
				Close();
		}

		private void BtnReset_Click(object sender, EventArgs e)
		{ PrintAndCutActions.Reset(mCore); RefreshState(); }

		#endregion

		#region aiming laser: on only while the button is held down

		private void BtnAim_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left || mAiming) return;

			string blocked = PrintAndCutActions.MachineReady(mCore) ? PrintAndCutActions.AimBlockReason(mCore) : null;
			if (blocked != null)
			{
				MessageBox.Show(this, blocked, Strings.PnCTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (PrintAndCutActions.AimOn(mCore))
			{
				mAiming = true;
				AimTimeout.Stop();
				AimTimeout.Start(); // hard limit, even if the button is still held
			}
		}

		private void BtnAim_MouseUp(object sender, MouseEventArgs e)
		{ StopAim(); }

		private void BtnAim_MouseCaptureChanged(object sender, EventArgs e)
		{
			if (!BtnAim.Capture) StopAim(); // mouse released outside, alt+tab, popup...
		}

		private void BtnAim_Leave(object sender, EventArgs e)
		{ StopAim(); }

		private void PrintAndCutForm_Deactivate(object sender, EventArgs e)
		{ StopAim(); }

		private void AimTimeout_Tick(object sender, EventArgs e)
		{ StopAim(); }

		private void StopAim()
		{
			AimTimeout.Stop();
			if (!mAiming) return;

			mAiming = false;
			PrintAndCutActions.AimOff(mCore);
		}

		#endregion

		#region hotkeys and jog keys, as in the main window

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData != mLastKeyData)
			{
				mLastKeyData = keyData;
				if (mCore.ManageHotKeys(this, keyData))
					return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			mLastKeyData = Keys.None;
			mCore.ManageHotKeys(this, Keys.None);
			base.OnKeyUp(e);
		}

		#endregion
	}
}
