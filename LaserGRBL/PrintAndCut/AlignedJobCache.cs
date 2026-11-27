// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA.

namespace LaserGRBL.PrintAndCut
{
	/// <summary>
	/// Keeps the aligned version of the loaded job, rebuilt only when the job, the Print and Cut state
	/// or the work offset change (rewriting and analyzing a big raster job takes time).
	/// <para>
	/// Only lines are cached: the commands actually sent must be created when the job is enqueued
	/// (<see cref="GrblCommand.CloneWithLine"/>), because the preview links and resets the result of
	/// the original commands after loading and after every run.
	/// </para>
	/// </summary>
	internal sealed class AlignedJobCache
	{
		/// <summary>Work offset change (mm) that is not worth aligning the job again.</summary>
		public const double WorkOffsetTolerance = 0.002;

		private readonly object mLock = new object();
		private GrblFile mSource;
		private int mStateVersion = -1;
		private Vec2 mWorkOffset;
		private GCodeTransformResult mResult;
		private GrblFile mAnalyzed;

		/// <summary>Forgets the cached data (call it when the job is loaded again).</summary>
		public void Invalidate()
		{
			lock (mLock)
			{
				mResult = null;
				mAnalyzed = null;
			}
		}

		/// <summary>
		/// The aligned lines of <paramref name="source"/> (one for each command), with the warnings found,
		/// or null if Print and Cut is not enabled.
		/// </summary>
		public GCodeTransformResult GetLines(GrblFile source, PrintAndCutManager manager, Vec2 workOffset)
		{
			lock (mLock)
			{
				int version;
				AlignmentTransform transform = manager.GetTransform(workOffset, out version);
				if (transform == null)
					return null;

				// firmwares that report no WCO derive it from rounded positions: it jitters by a few microns
				bool valid = mResult != null && ReferenceEquals(mSource, source) && mStateVersion == version
					&& System.Math.Abs(mWorkOffset.X - workOffset.X) < WorkOffsetTolerance
					&& System.Math.Abs(mWorkOffset.Y - workOffset.Y) < WorkOffsetTolerance;

				if (!valid)
				{
					mResult = GCodeTransformer.Transform(source.GetLines(), transform);
					mAnalyzed = null;
					mSource = source;
					mStateVersion = version;
					mWorkOffset = workOffset;
					Logger.LogMessage("PrintAndCut", "Job aligned: angle {0:0.###} deg, scale {1:0.#####}, {2} warnings", transform.AngleDegrees, transform.Scale, mResult.Warnings.Count);
				}

				return mResult;
			}
		}

		/// <summary>
		/// An analyzed copy of the aligned job (range for framing, saving to file), or null if Print and Cut
		/// is not enabled or the job cannot be aligned. Not meant to be sent: see <see cref="GetLines"/>.
		/// </summary>
		public GrblFile GetAnalyzedCopy(GrblFile source, PrintAndCutManager manager, Vec2 workOffset)
		{
			lock (mLock)
			{
				GCodeTransformResult result = GetLines(source, manager, workOffset);
				if (result == null || result.HasBlockingIssues)
					return null;

				if (mAnalyzed == null)
					mAnalyzed = source.CreateLinkedCopy(result.Lines);
				return mAnalyzed;
			}
		}
	}
}
