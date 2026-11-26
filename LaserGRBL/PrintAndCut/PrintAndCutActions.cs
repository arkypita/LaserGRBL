// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LaserGRBL.PrintAndCut
{
	/// <summary>
	/// Print and Cut user actions, shared by the menu, the hotkeys (Alt+0..Alt+4 as in LightBurn) and the wizard.
	/// Every action validates the state and explains to the user what is missing.
	/// </summary>
	internal static class PrintAndCutActions
	{
		/// <summary>Power of the aiming laser, same as the standard "Focus" custom button: 3% of $30.</summary>
		private const string AimOnCode = "M3 S[$30*3/100]\r\nG1 F1000";  // G1: grbl laser mode ($32=1) keeps the laser off in G0
		private const string AimOffCode = "M5 S0\r\nG0";

		// Texts of the safety checks added after the review. Translations are pending, as for the rest of the feature.
		private const string ErrorJobRunning = "Print and Cut cannot be changed while a job is running: the next passes would use another alignment. Stop the job first.";
		private const string ErrorAimFirmware = "The aiming laser is only available with GRBL firmware: its power is a percentage of $30, a scale that other firmwares do not use.";
		private const string ErrorAimMaxPower = "The aiming laser needs the maximum spindle speed ($30) of the machine, and it could not be read. Reconnect the machine so that its configuration is read.";
		private const string WarnOutsideWorkspace = "The aligned job goes outside the workspace configured in your machine ({0} mm x {1} mm, GRBL $130 and $131) and soft limits are enabled ($20=1).\n\nThis could generate a soft-limit alarm, or the head could hit the end of the axes.\n\nEnable the alignment anyway?";

		private static PrintAndCutForm mWizard;

		/// <summary>True if the machine can be jogged or measured: connected, idle and not running a job.</summary>
		public static bool MachineReady(GrblCore core)
		{
			return core.IsConnected && core.MachineStatus == GrblCore.MacStatus.Idle && !core.InProgram;
		}

		/// <summary>Registration mark of the loaded job with the given number, or null.</summary>
		public static RegistrationMark FindMark(GrblCore core, int number)
		{
			List<RegistrationMark> marks = core.LoadedFile.RegistrationMarks;
			foreach (RegistrationMark m in marks)
				if (m.Number == number) return m;
			return null;
		}

		/// <summary>"Start Wizard" (Alt+0): shows the wizard, or brings it to front.</summary>
		public static void ShowWizard(Form parent, GrblCore core)
		{
			if (mWizard == null || mWizard.IsDisposed)
			{
				Form owner = parent is PrintAndCutForm ? parent.Owner : parent;
				mWizard = new PrintAndCutForm(core);
				mWizard.Show(owner);
			}
			else
			{
				mWizard.Activate();
			}
		}

		/// <summary>
		/// "Set First / Second Target Position" (Alt+1 / Alt+2): associates the selected mark with
		/// the current machine position (MPos).
		/// </summary>
		public static bool SetTarget(Form parent, GrblCore core, bool first)
		{
			if (!MachineReady(core) || !core.MotionSettled)
				return Error(parent, Strings.PnCErrorNotIdle); // a jog still queued would leave the position of before the move

			PrintAndCutManager pnc = core.PrintAndCutManager;
			int number = first ? pnc.FirstMarkSelection : pnc.SecondMarkSelection;
			RegistrationMark mark = FindMark(core, number);
			if (mark == null)
				return Error(parent, string.Format(Strings.PnCErrorNoMark, number));

			if (mark.IsOversized && !Confirm(parent, string.Format(Strings.PnCWarnOversized, OversizedText(mark))))
				return false;

			GPoint mpos = core.MachinePosition;
			Vec2 position = new Vec2(mpos.X, mpos.Y);
			if (first) pnc.SetFirstTarget(mark, position);
			else pnc.SetSecondTarget(mark, position);

			Logger.LogMessage("PrintAndCut", "{0} target: mark #{1} {2} at MPos {3}", first ? "First" : "Second", mark.Number, mark.Center, position);
			return true;
		}

		/// <summary>
		/// "Align Output to Targets" (Alt+3, <paramref name="scaling"/> true) or
		/// "Align Output to Targets (no scaling)" (Alt+4). Warns before enabling a suspicious scale.
		/// </summary>
		public static bool Align(Form parent, GrblCore core, bool scaling)
		{
			if (core.InProgram)
				return Error(parent, ErrorJobRunning); // every pass of the job reads the alignment

			PrintAndCutManager pnc = core.PrintAndCutManager;
			if (!pnc.CanAlign)
				return Error(parent, Strings.PnCErrorTargetsMissing);

			AlignmentTransform preview;
			try
			{
				preview = pnc.PreviewAlignment(scaling, core.PrintAndCutWorkOffset);
			}
			catch (InvalidOperationException) { return Error(parent, Strings.PnCErrorSameMark); }
			catch (ArgumentException) { return Error(parent, Strings.PnCErrorTooClose); }

			if (OutsideWorkspace(core, preview) && !Confirm(parent, string.Format(WarnOutsideWorkspace, (int)GrblCore.Configuration.TableWidth, (int)GrblCore.Configuration.TableHeight)))
				return false;

			if (preview.IsScaleSuspicious)
			{
				double percent = (preview.MeasuredScale - 1.0) * 100.0;
				if (!Confirm(parent, string.Format(Strings.PnCWarnScale, percent)))
					return false;
			}

			AlignmentTransform t = pnc.Align(scaling, core.PrintAndCutWorkOffset);
			Logger.LogMessage("PrintAndCut", "Enabled ({0}): rotation {1:0.###} deg, scale {2:0.#####}, measured scale {3:0.#####}, second target error {4:0.###} mm",
				scaling ? "scaled" : "no scaling", t.AngleDegrees, t.Scale, t.MeasuredScale, t.SecondTargetError);
			return true;
		}

		/// <summary>"Reset Print and Cut": forgets targets and alignment.</summary>
		public static void Reset(GrblCore core)
		{
			if (core.InProgram)
			{
				MessageBox.Show(ErrorJobRunning, Strings.PnCTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			core.PrintAndCutManager.Reset();
			Logger.LogMessage("PrintAndCut", "Reset");
		}

		/// <summary>"Jog To Selection": moves the head, laser off, to the expected position of the selected mark.</summary>
		public static bool JogToSelection(Form parent, GrblCore core, bool first)
		{
			if (!MachineReady(core))
				return Error(parent, Strings.PnCErrorNotIdle);

			PrintAndCutManager pnc = core.PrintAndCutManager;
			int number = first ? pnc.FirstMarkSelection : pnc.SecondMarkSelection;
			RegistrationMark mark = FindMark(core, number);
			if (mark == null)
				return Error(parent, string.Format(Strings.PnCErrorNoMark, number));

			Vec2 wco = core.PrintAndCutWorkOffset;
			Vec2 work = pnc.PredictMachinePosition(mark.Center, wco) - wco; // jog targets are in work coordinates
			core.JogToPosition(new PointF((float)work.X, (float)work.Y), false);
			return true;
		}

		/// <summary>True if the aiming laser can be turned on now.</summary>
		public static bool CanAim(GrblCore core)
		{
			return MachineReady(core) && AimBlockReason(core) == null;
		}

		/// <summary>
		/// Why the aiming laser must not be used with this machine, or null. The power is a percentage of $30
		/// (GRBL), so firmwares with another power scale, or a $30 that was not read, could light the laser much
		/// stronger than 3% while it is standing still.
		/// </summary>
		public static string AimBlockReason(GrblCore core)
		{
			if (core.GetType() != typeof(GrblCore))
				return ErrorAimFirmware;

			GrblConfST conf = GrblCore.Configuration;
			if (conf != null)
			{
				foreach (KeyValuePair<int, string> p in conf)
				{
					double power;
					if (p.Key == 30 && double.TryParse(p.Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out power) && power > 0)
						return null;
				}
			}
			return ErrorAimMaxPower;
		}

		/// <summary>True if the job, once aligned, goes outside the workspace of the machine ($130/$131) with soft limits enabled.</summary>
		private static bool OutsideWorkspace(GrblCore core, AlignmentTransform alignment)
		{
			GrblConfST conf = GrblCore.Configuration;
			GrblFile file = core.LoadedFile;
			if (conf == null || !conf.SoftLimit || file == null || !file.Range.DrawingRange.ValidRange)
				return false;

			Vec2 wco = core.PrintAndCutWorkOffset;
			double width = (double)conf.TableWidth, height = (double)conf.TableHeight;
			ProgramRange.XYRange r = file.Range.DrawingRange;
			double[] xs = { (double)r.X.Min, (double)r.X.Max };
			double[] ys = { (double)r.Y.Min, (double)r.Y.Max };
			foreach (double x in xs)
				foreach (double y in ys)
				{
					Vec2 p = alignment.TransformPoint(new Vec2(x, y)); // work coordinates, positive space as in the jog limits
					if (p.X < -wco.X - 0.01 || p.X > width - wco.X + 0.01 || p.Y < -wco.Y - 0.01 || p.Y > height - wco.Y + 0.01)
						return true;
				}
			return false;
		}

		/// <summary>
		/// Turns on the aiming laser at low power, with the head stopped. The caller must guarantee
		/// <see cref="AimOff"/> (button released, focus lost, timeout, window closed).
		/// </summary>
		public static bool AimOn(GrblCore core)
		{
			if (!CanAim(core)) return false;
			core.ExecuteCustomCode(AimOnCode.Replace("\r\n", Environment.NewLine));
			return true;
		}

		/// <summary>Turns off the aiming laser.</summary>
		public static void AimOff(GrblCore core)
		{
			if (core.IsConnected)
				core.ExecuteCustomCode(AimOffCode.Replace("\r\n", Environment.NewLine));
		}

		/// <summary>
		/// Box of a mark where the preview shows it: with the alignment enabled it moves with the job
		/// (same center transformed, size scaled), so it stays on the printed mark.
		/// </summary>
		public static Bounds PreviewBounds(RegistrationMark mark, AlignmentTransform alignment)
		{
			if (alignment == null)
				return mark.Bounds;

			Vec2 center = alignment.TransformPoint(mark.Center);
			double halfWidth = mark.Bounds.Width / 2 * alignment.Scale;
			double halfHeight = mark.Bounds.Height / 2 * alignment.Scale;
			return new Bounds(center.X - halfWidth, center.Y - halfHeight, center.X + halfWidth, center.Y + halfHeight);
		}

		/// <summary>
		/// Preview color of a mark, as in LightBurn: first target red, second target blue,
		/// both green once Print and Cut is enabled; other marks magenta (the reserved color).
		/// </summary>
		public static Color MarkColor(RegistrationMark mark, PrintAndCutManager pnc)
		{
			bool first = ReferenceEquals(mark, pnc.FirstMark) && pnc.HasFirstTarget;
			bool second = ReferenceEquals(mark, pnc.SecondMark) && pnc.HasSecondTarget;
			if (pnc.IsEnabled && (first || second)) return Color.LimeGreen;
			if (first) return Color.Red;
			if (second) return Color.DodgerBlue; // plain blue is hard to see on the dark preview schemes
			return Color.Magenta;
		}

		/// <summary>Label of a mark in the preview: its number, and the target it is used for.</summary>
		public static string MarkLabel(RegistrationMark mark, PrintAndCutManager pnc)
		{
			if (ReferenceEquals(mark, pnc.FirstMark) && pnc.HasFirstTarget) return string.Format("#{0} (1)", mark.Number);
			if (ReferenceEquals(mark, pnc.SecondMark) && pnc.HasSecondTarget) return string.Format("#{0} (2)", mark.Number);
			return "#" + mark.Number;
		}

		/// <summary>User readable description of an oversized mark.</summary>
		public static string OversizedText(RegistrationMark mark)
		{
			return string.Format(Strings.PnCOversized, mark.Number, mark.Bounds.Width, mark.Bounds.Height);
		}

		private static bool Error(Form parent, string message)
		{
			MessageBox.Show(parent, message, Strings.PnCTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return false;
		}

		private static bool Confirm(Form parent, string message)
		{
			return MessageBox.Show(parent, message, Strings.PnCTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
		}
	}
}
