// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA.

using System;

namespace LaserGRBL.PrintAndCut
{
	/// <summary>
	/// Print and Cut session state, as in LightBurn: two targets (a registration mark of the job plus
	/// the machine position where the user jogged the head on the printed mark), then
	/// "Align Output to Targets" enables the alignment for every following job until "Reset Print and Cut".
	/// <para>
	/// Machine positions are stored in machine coordinates (MPos), so they stay valid if the user changes
	/// the work zero; they are converted to work coordinates with the current WCO every time the
	/// transform is requested.
	/// </para>
	/// </summary>
	public sealed class PrintAndCutManager
	{
		private readonly object mLock = new object();
		private RegistrationMark mFirstMark;
		private RegistrationMark mSecondMark;
		private Vec2? mFirstMachine;
		private Vec2? mSecondMachine;
		private bool mEnabled;
		private bool mAllowScaling;
		private int mVersion;
		private int mFirstSelection = 1;
		private int mSecondSelection = 2;

		/// <summary>Raised after any change of state (targets, alignment, reset). May be raised from any thread.</summary>
		public event EventHandler StateChanged;

		/// <summary>Registration mark used as first target, or null.</summary>
		public RegistrationMark FirstMark { get { lock (mLock) return mFirstMark; } }
		/// <summary>Registration mark used as second target, or null.</summary>
		public RegistrationMark SecondMark { get { lock (mLock) return mSecondMark; } }
		/// <summary>Machine position (MPos) of the first target, or null.</summary>
		public Vec2? FirstMachinePosition { get { lock (mLock) return mFirstMachine; } }
		/// <summary>Machine position (MPos) of the second target, or null.</summary>
		public Vec2? SecondMachinePosition { get { lock (mLock) return mSecondMachine; } }

		/// <summary>True if the first target is set.</summary>
		public bool HasFirstTarget { get { lock (mLock) return mFirstMark != null && mFirstMachine.HasValue; } }
		/// <summary>True if the second target is set.</summary>
		public bool HasSecondTarget { get { lock (mLock) return mSecondMark != null && mSecondMachine.HasValue; } }
		/// <summary>True if both targets are set and the output can be aligned.</summary>
		public bool CanAlign { get { return HasFirstTarget && HasSecondTarget; } }

		/// <summary>True while "Print and Cut enabled": every job is aligned to the targets.</summary>
		public bool IsEnabled { get { lock (mLock) return mEnabled; } }
		/// <summary>True for "Align Output to Targets", false for "(no scaling)".</summary>
		public bool AllowScaling { get { lock (mLock) return mAllowScaling; } }

		/// <summary>Incremented on every change that affects the transform, so that cached aligned jobs can be invalidated.</summary>
		public int Version { get { lock (mLock) return mVersion; } }

		/// <summary>
		/// Number of the mark the user chose for the first target (the equivalent of selecting the crosshair
		/// in LightBurn before "Set First Target Position"). Defaults to 1.
		/// </summary>
		public int FirstMarkSelection
		{
			get { lock (mLock) return mFirstSelection; }
			set { lock (mLock) mFirstSelection = value; OnStateChanged(); }
		}

		/// <summary>Number of the mark the user chose for the second target. Defaults to 2.</summary>
		public int SecondMarkSelection
		{
			get { lock (mLock) return mSecondSelection; }
			set { lock (mLock) mSecondSelection = value; OnStateChanged(); }
		}

		/// <summary>
		/// The transform that <see cref="Align"/> would enable, without changing the state
		/// (to warn the user about a suspicious scale before enabling it).
		/// </summary>
		/// <exception cref="InvalidOperationException">If a target is missing or both targets use the same mark.</exception>
		/// <exception cref="ArgumentException">If the targets are too close to each other.</exception>
		public AlignmentTransform PreviewAlignment(bool allowScaling, Vec2 workOffset)
		{
			lock (mLock)
			{
				CheckTargets();
				return Build(allowScaling, workOffset);
			}
		}

		/// <summary>
		/// "Set First Target Position": associates the chosen mark of the job with the current machine position.
		/// Changing a target disables an active alignment (it must be aligned again).
		/// </summary>
		public void SetFirstTarget(RegistrationMark mark, Vec2 machinePosition)
		{
			if (mark == null) throw new ArgumentNullException("mark");
			lock (mLock)
			{
				mFirstMark = mark;
				mFirstMachine = machinePosition;
				mEnabled = false;
				mVersion++;
			}
			OnStateChanged();
		}

		/// <summary>"Set Second Target Position". See <see cref="SetFirstTarget"/>.</summary>
		public void SetSecondTarget(RegistrationMark mark, Vec2 machinePosition)
		{
			if (mark == null) throw new ArgumentNullException("mark");
			lock (mLock)
			{
				mSecondMark = mark;
				mSecondMachine = machinePosition;
				mEnabled = false;
				mVersion++;
			}
			OnStateChanged();
		}

		/// <summary>
		/// "Align Output to Targets" (<paramref name="allowScaling"/> true) or "(no scaling)" (false).
		/// Returns the transform computed with the given WCO, so that the caller can show diagnostics.
		/// </summary>
		/// <param name="allowScaling">Apply the measured scale.</param>
		/// <param name="workOffset">Current work coordinate offset (WCO = MPos - WPos).</param>
		/// <exception cref="InvalidOperationException">If a target is missing or both targets use the same mark.</exception>
		/// <exception cref="ArgumentException">If the targets are too close to each other.</exception>
		public AlignmentTransform Align(bool allowScaling, Vec2 workOffset)
		{
			AlignmentTransform transform;
			lock (mLock)
			{
				CheckTargets();
				transform = Build(allowScaling, workOffset); // throws if too close: state unchanged
				mAllowScaling = allowScaling;
				mEnabled = true;
				mVersion++;
			}
			OnStateChanged();
			return transform;
		}

		/// <summary>"Reset Print and Cut": forgets targets and disables the alignment.</summary>
		public void Reset()
		{
			lock (mLock)
			{
				mFirstMark = mSecondMark = null;
				mFirstMachine = mSecondMachine = null;
				mEnabled = false;
				mAllowScaling = false;
				mVersion++;
			}
			OnStateChanged();
		}

		/// <summary>
		/// Same as <see cref="GetTransform(Vec2)"/>, with the <see cref="Version"/> the transform belongs to
		/// (both read under the same lock, so a concurrent change cannot mix them).
		/// </summary>
		public AlignmentTransform GetTransform(Vec2 workOffset, out int version)
		{
			lock (mLock)
			{
				version = mVersion;
				return mEnabled ? Build(mAllowScaling, workOffset) : null;
			}
		}

		/// <summary>The transform to apply to the output with the given WCO, or null if Print and Cut is not enabled.</summary>
		public AlignmentTransform GetTransform(Vec2 workOffset)
		{
			lock (mLock)
				return mEnabled ? Build(mAllowScaling, workOffset) : null;
		}

		/// <summary>
		/// "Jog To Selection": best guess of the machine position (MPos) of a design point.
		/// Uses the alignment when enabled; otherwise assumes no rotation from a target already set;
		/// with no targets the design coordinates are taken as work coordinates.
		/// </summary>
		public Vec2 PredictMachinePosition(Vec2 designPoint, Vec2 workOffset)
		{
			lock (mLock)
			{
				if (mEnabled)
					return Build(mAllowScaling, workOffset).TransformPoint(designPoint) + workOffset;
				if (mFirstMark != null && mFirstMachine.HasValue)
					return mFirstMachine.Value + (designPoint - mFirstMark.Center);
				if (mSecondMark != null && mSecondMachine.HasValue)
					return mSecondMachine.Value + (designPoint - mSecondMark.Center);
				return designPoint + workOffset;
			}
		}

		private void CheckTargets()
		{
			if (mFirstMark == null || mSecondMark == null || !mFirstMachine.HasValue || !mSecondMachine.HasValue)
				throw new InvalidOperationException("Both targets must be set before aligning.");
			if (mFirstMark == mSecondMark)
				throw new InvalidOperationException("First and second target use the same mark.");
		}

		private AlignmentTransform Build(bool allowScaling, Vec2 workOffset)
		{
			return AlignmentTransform.FromTargets(mFirstMark.Center, mSecondMark.Center,
				mFirstMachine.Value - workOffset, mSecondMachine.Value - workOffset, allowScaling);
		}

		private void OnStateChanged()
		{
			EventHandler handler = StateChanged;
			if (handler != null) handler(this, EventArgs.Empty);
		}
	}
}
