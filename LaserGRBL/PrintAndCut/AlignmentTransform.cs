// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA.

using System;

namespace LaserGRBL.PrintAndCut
{
	/// <summary>
	/// Immutable 2D point/vector with double precision, used by the Print and Cut math.
	/// </summary>
	public struct Vec2
	{
		/// <summary>X component.</summary>
		public readonly double X;
		/// <summary>Y component.</summary>
		public readonly double Y;

		/// <summary>Creates a new vector.</summary>
		public Vec2(double x, double y)
		{
			X = x;
			Y = y;
		}

		/// <summary>Euclidean length of the vector.</summary>
		public double Length
		{ get { return Math.Sqrt(X * X + Y * Y); } }

		/// <summary>Angle of the vector in radians, measured from the positive X axis.</summary>
		public double Angle
		{ get { return Math.Atan2(Y, X); } }

		public static Vec2 operator +(Vec2 a, Vec2 b) { return new Vec2(a.X + b.X, a.Y + b.Y); }
		public static Vec2 operator -(Vec2 a, Vec2 b) { return new Vec2(a.X - b.X, a.Y - b.Y); }
		public static Vec2 operator *(double k, Vec2 v) { return new Vec2(k * v.X, k * v.Y); }

		/// <summary>Distance between two points.</summary>
		public static double Distance(Vec2 a, Vec2 b) { return (a - b).Length; }

		public override string ToString()
		{ return string.Format(System.Globalization.CultureInfo.InvariantCulture, "({0:0.###}; {1:0.###})", X, Y); }
	}

	/// <summary>
	/// Similarity transform (rotation, optional uniform scale and translation) that maps
	/// design coordinates onto machine coordinates, computed from two registration targets.
	/// <para>P' = M1 + scale * R(angle) * (P - D1)</para>
	/// </summary>
	public sealed class AlignmentTransform
	{
		/// <summary>
		/// Minimum distance (mm) between the two targets, both in the design and on the machine.
		/// Closer targets make the angle meaningless.
		/// </summary>
		public const double MinTargetDistance = 1.0;

		/// <summary>
		/// Relative scale deviation above which the alignment should be reported to the user
		/// as suspicious (wrong target order, wrong mark, printer scaling, ...).
		/// </summary>
		public const double SuspiciousScaleTolerance = 0.03;

		private readonly double mCos;
		private readonly double mSin;

		private AlignmentTransform(Vec2 d1, Vec2 d2, Vec2 m1, Vec2 m2, bool allowScaling)
		{
			Vec2 vd = d2 - d1;
			Vec2 vm = m2 - m1;

			DesignFirst = d1;
			DesignSecond = d2;
			MachineFirst = m1;
			MachineSecond = m2;
			AllowScaling = allowScaling;
			Angle = NormalizeAngle(vm.Angle - vd.Angle);
			MeasuredScale = vm.Length / vd.Length;
			Scale = allowScaling ? MeasuredScale : 1.0;
			mCos = Math.Cos(Angle);
			mSin = Math.Sin(Angle);
		}

		/// <summary>
		/// Builds the transform from the two design targets (mark centers in the job) and the two
		/// positions measured on the machine.
		/// </summary>
		/// <param name="d1">Center of the first mark in design coordinates.</param>
		/// <param name="d2">Center of the second mark in design coordinates.</param>
		/// <param name="m1">Position measured on the machine for the first mark.</param>
		/// <param name="m2">Position measured on the machine for the second mark.</param>
		/// <param name="allowScaling">True for "Align Output to Targets", false for "(no scaling)".</param>
		/// <exception cref="ArgumentException">If the targets are too close to compute an angle.</exception>
		public static AlignmentTransform FromTargets(Vec2 d1, Vec2 d2, Vec2 m1, Vec2 m2, bool allowScaling)
		{
			if (Vec2.Distance(d1, d2) < MinTargetDistance)
				throw new ArgumentException("Design targets are too close to each other.", "d2");
			if (Vec2.Distance(m1, m2) < MinTargetDistance)
				throw new ArgumentException("Machine targets are too close to each other.", "m2");

			return new AlignmentTransform(d1, d2, m1, m2, allowScaling);
		}

		/// <summary>First target in design coordinates (D1).</summary>
		public Vec2 DesignFirst { get; private set; }
		/// <summary>Second target in design coordinates (D2).</summary>
		public Vec2 DesignSecond { get; private set; }
		/// <summary>First target measured on the machine (M1).</summary>
		public Vec2 MachineFirst { get; private set; }
		/// <summary>Second target measured on the machine (M2).</summary>
		public Vec2 MachineSecond { get; private set; }
		/// <summary>True if the transform applies the measured scale.</summary>
		public bool AllowScaling { get; private set; }
		/// <summary>Rotation in radians, normalized to (-PI, PI].</summary>
		public double Angle { get; private set; }
		/// <summary>Rotation in degrees, normalized to (-180, 180].</summary>
		public double AngleDegrees
		{ get { return Angle * 180.0 / Math.PI; } }
		/// <summary>Scale applied to the output (1.0 when scaling is not allowed).</summary>
		public double Scale { get; private set; }
		/// <summary>Ratio between machine and design target distance, regardless of <see cref="AllowScaling"/>.</summary>
		public double MeasuredScale { get; private set; }

		/// <summary>
		/// True when the measured scale deviates from 1.0 more than <see cref="SuspiciousScaleTolerance"/>.
		/// </summary>
		public bool IsScaleSuspicious
		{ get { return Math.Abs(MeasuredScale - 1.0) > SuspiciousScaleTolerance; } }

		/// <summary>
		/// Distance (mm) between the transformed second design target and the second machine target.
		/// Always 0 with scaling; in "no scaling" mode it is the residual error the user should know about.
		/// </summary>
		public double SecondTargetError
		{ get { return Vec2.Distance(TransformPoint(DesignSecond), MachineSecond); } }

		/// <summary>Maps a design point to machine coordinates (rotation, scale and translation).</summary>
		public Vec2 TransformPoint(Vec2 p)
		{
			return MachineFirst + TransformVector(p - DesignFirst);
		}

		/// <summary>Maps a relative vector (G91 deltas, arc I/J offsets): rotation and scale, no translation.</summary>
		public Vec2 TransformVector(Vec2 v)
		{
			return new Vec2(Scale * (mCos * v.X - mSin * v.Y), Scale * (mSin * v.X + mCos * v.Y));
		}

		private static double NormalizeAngle(double a)
		{
			while (a <= -Math.PI) a += 2 * Math.PI;
			while (a > Math.PI) a -= 2 * Math.PI;
			return a;
		}
	}
}
