//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Windows;

namespace LaserGRBL
{
    public class JogCommand : GrblCommand
	{
		public JogCommand(string line) : base(line.Substring(3))
		{
		}
	}

	public partial class GrblCommand
	{
		public class StatePositionBuilder : StateBuilder
		{
			bool supportPWM = Settings.GetObject("Support Hardware PWM", true);

			public class CumulativeElement : Element
			{
				Element mDefault = null;
				bool mSettled = false;
				decimal mPrevious = 0;

				public CumulativeElement(Element defval) : base(defval.Command, defval.Number)
				{mDefault = defval;}

				public bool IsDefault
				{ get { return base.Equals(mDefault); } }

				public bool IsSettled
				{ get { return mSettled; } }

				public void Update(Element e, bool Absolute, decimal offset)
				{
					mPrevious = mNumber;

					if (e != null)
					{
						mNumber = Absolute ? offset + e.Number : mNumber + e.Number;
						mSettled = true;
					}
				}

				public decimal Previous
				{ get { return mPrevious; } }
			}

			public class LastValueElement : Element
			{
				Element mDefault = null;
				bool mSettled = false;

				public LastValueElement(Element defval) : base(defval.Command, defval.Number)
				{ mDefault = defval; }

				public bool IsDefault
				{ get { return base.Equals(mDefault); } }

				public bool IsSettled
				{ get { return mSettled; } }

				public void Update(Element e)
				{
					if (e != null)
					{
						mNumber = e.Number;
						mSettled = true;
					}
				}
			}

			private CumulativeElement mCurX = new CumulativeElement("X0");
			private CumulativeElement mCurY = new CumulativeElement("Y0");
			private CumulativeElement mCurZ = new CumulativeElement("Z0");

			private decimal mWcoX = 0;
			private decimal mWcoY = 0;
			private decimal mWcoZ = 0;

			private LastValueElement mCurF = new LastValueElement("F0");
			private LastValueElement mCurS = new LastValueElement("S0");

			public G2G3Helper LastArcHelperResult;

			public TimeSpan AnalyzeCommand(GrblCommand cmd, bool compute, GrblConfST conf = null)
			{
				bool delete = !cmd.JustBuilt;
				if (!cmd.JustBuilt) cmd.BuildHelper();

				UpdateModalsNB(cmd);
				UpdateWCO(cmd);
				UpdateXYZ(cmd);
				UpdateFS(cmd);

				TimeSpan rv = compute ? ComputeExecutionTime(cmd, conf) : TimeSpan.Zero;
				if (delete) cmd.DeleteHelper();

				return rv;
			}

			private void UpdateFS(GrblCommand cmd)
			{
				if (cmd is JogCommand)
					return;

				mCurF.Update(cmd.F);
				mCurS.Update(cmd.S);
			}

			private void UpdateXYZ(GrblCommand cmd)
			{
				if (cmd is JogCommand)
				{
					mCurX.Update(cmd.X, cmd.IsAbsoluteCoord, mWcoX);
					mCurY.Update(cmd.Y, cmd.IsAbsoluteCoord, mWcoY);
					mCurZ.Update(cmd.Z, cmd.IsAbsoluteCoord, mWcoZ);
				}
				else if (cmd.IsMovement)
				{
					mCurX.Update(cmd.X, ABS, mWcoX);
					mCurY.Update(cmd.Y, ABS, mWcoY);
					mCurZ.Update(cmd.Z, ABS, mWcoZ);
				}
			}

			private void UpdateWCO(GrblCommand cmd)
			{
				if (cmd.IsSetWCO)
				{
					if (cmd.X != null) mWcoX = mCurX.Number - cmd.X.Number;
					if (cmd.Y != null) mWcoY = mCurY.Number - cmd.Y.Number;
					if (cmd.Z != null) mWcoZ = mCurZ.Number - cmd.Z.Number;
				}

			}

			public bool HasWCO
			{ get { return mWcoX != 0 || mWcoY != 0 || mWcoZ != 0; } }

			public decimal WcoX
			{ get { return mWcoX; } }

			public decimal WcoY
			{ get { return mWcoY; } }

			public decimal WcoZ
			{ get { return mWcoZ; } }

			public LastValueElement F
			{ get { return mCurF; } }

			public LastValueElement S
			{ get { return mCurS; } }

			public CumulativeElement X
			{ get { return mCurX; } }

			public CumulativeElement Y
			{ get { return mCurY; } }

			public CumulativeElement Z
			{ get { return mCurZ; } }

			internal bool TrueMovement()
			{ return (mCurX.Number != mCurX.Previous || mCurY.Number != mCurY.Previous || G2G3); }


			private TimeSpan ComputeExecutionTime(GrblCommand cmd, GrblConfST conf)
			{
				decimal f = cmd is JogCommand && cmd.F != null ? cmd.F.Number : mCurF.Number;

				if (G0 && cmd.IsLinearMovement)
					return TimeSpan.FromMinutes((double)GetSegmentLenght(cmd) / (double)conf.MaxRateX); //todo: use a better computation of xy if different x/y max speed
				else if (G1G2G3 && cmd.IsMovement && f != 0)
					return TimeSpan.FromMinutes((double)GetSegmentLenght(cmd) / (double)Math.Min(f, conf.MaxRateX));
				else if (cmd.IsPause)
					return cmd.P != null ? TimeSpan.FromSeconds((double)cmd.P.Number) : cmd.S != null ? TimeSpan.FromSeconds((double)cmd.S.Number) : TimeSpan.Zero;
				else
					return TimeSpan.Zero;
			}

			private decimal GetSegmentLenght(GrblCommand cmd)
			{
				LastArcHelperResult = null;

				if (cmd.IsLinearMovement)
					return Tools.MathHelper.LinearDistance(mCurX.Previous, mCurY.Previous, mCurX.Number, mCurY.Number);
				else if (cmd.IsArcMovement) //arc of given radius
					return (decimal)GetArcHelper(cmd).AbsLenght;
				else
					return 0;
			}

			internal int GetCurrentAlpha(ProgramRange.SRange range)
			{
				if (!LaserBurning)
					return 150; //supportPWM ? 150 : 50
				else if (supportPWM && range.ValidRange && S.IsSettled)
					return (int)((S.Number - range.S.Min) * 255 / (range.S.Max - range.S.Min));
				else
					return 255;
            }

            public bool G2
			{ get { return MotionMode.Number == 2; } }

			public bool G2G3
			{ get { return MotionMode.Number == 2 || MotionMode.Number == 3; } }

			public bool G0G1
			{ get { return MotionMode.Number == 0 || MotionMode.Number == 1; } }

			public bool G0
			{ get { return MotionMode.Number == 0; } }

			public bool G1G2G3
			{ get { return MotionMode.Number == 1 || MotionMode.Number == 2 || MotionMode.Number == 3; } }

			public bool ABS
			{ get { return DistanceMode.Number == 90; } }

			public bool M3M4
			{ get { return SpindleState.Number == 3 || SpindleState.Number == 4; } }

			public bool LaserBurning
			{ get { return (!supportPWM || S.Number > 0) && (SpindleState.Number == 3 || (SpindleState.Number == 4 && MotionMode.Number != 0)); } }

			internal void Homing()
			{
				mCurX = new CumulativeElement("X0");
				mCurY = new CumulativeElement("Y0");
				mCurZ = new CumulativeElement("Z0");
			}

			internal G2G3Helper GetArcHelper(GrblCommand cmd)
			{
				LastArcHelperResult = new G2G3Helper(this, cmd);
				return LastArcHelperResult;
			}
		}

		public class StateBuilder
		{
			//This class is able to parse a series of gcode lines and build the parser state
			//https://github.com/gnea/grbl/wiki/Grbl-v1.1-Commands#g---view-gcode-parser-state

			/*
			Motion Mode					[G0], G1, G2, G3, G38.2, G38.3, G38.4, G38.5, G80
			Coordinate System Select	[G54], G55, G56, G57, G58, G59
			Plane Select				[G17], G18, G19
			Distance Mode				[G90], G91
			Arc IJK Distance Mode		[G91.1]
			Feed Rate Mode				G93, [G94]
			Units Mode					G20, [G21]
			Cutter Radius Compensation	[G40]
			Tool Length Offset			G43.1, [G49]
			Program Mode				[M0], M1, M2, M30
			Spindle State				M3, M4, [M5]
			Coolant State				M7, M8, [M9]
			*/

			public class ModalElement : Element
			{
				List<Element> mOptions = new List<Element>();
				Element mDefault = null;
				bool mSettled = false;
				

				public ModalElement(Element defval, params Element[] options) : base(defval.Command, defval.Number)
				{
					mDefault = defval;
					mOptions.Add(defval);
					foreach (Element e in options)
						mOptions.Add(e);
				}

				public bool IsDefault
				{ get { return base.Equals(mDefault); } }

				public bool IsSettled
				{ get { return mSettled; } }

				public void Update(Element e)
				{
					if (e != null && mOptions.Contains(e))
					{ 
						mCommand = e.Command;
						mNumber = e.Number;
						mSettled = true;
					}
				}
			}

			public ModalElement MotionMode = new ModalElement("G0", "G1", "G2", "G3", "G38.2", "G38.3", "G38.4", "G38.5", "G80");
			protected ModalElement CoordinateSelect = new ModalElement("G54", "G55", "G56", "G57", "G58", "G59");
			protected ModalElement PlaneSelect = new ModalElement("G17", "G18", "G19");
			protected ModalElement DistanceMode = new ModalElement("G90", "G91");
			protected ModalElement ArcDistanceMode = new ModalElement("G91.1");
			protected ModalElement FeedRateMode = new ModalElement("G94", "G93");
			protected ModalElement UnitsMode = new ModalElement("G21", "G20");
			protected ModalElement CutterRadiusCompensation = new ModalElement("G40");
			protected ModalElement ToolLengthOffset = new ModalElement("G49", "G43.1");
			protected ModalElement ProgramMode = new ModalElement("M0", "M1", "M2", "M30");
			protected ModalElement CoolantState = new ModalElement("M9", "M7", "M8");
			protected ModalElement SpindleState = new ModalElement("M5", "M3", "M4");

			//private void UpdateModals(GrblCommand cmd) //update modals - BUILD IF NEEDED
			//{
			//	bool delete = !cmd.JustBuilt;
			//	if (!cmd.JustBuilt) cmd.BuildHelper();

			//	UpdateModalsNB(cmd);

			//	if (delete) cmd.DeleteHelper();
			//}

			protected void UpdateModalsNB(GrblCommand cmd) //update modals - EXTERNAL BUILD
			{
				if (cmd is JogCommand)
					return;

				MotionMode.Update(cmd.G);
				CoordinateSelect.Update(cmd.G);
				PlaneSelect.Update(cmd.G);
				DistanceMode.Update(cmd.G);
				ArcDistanceMode.Update(cmd.G);
				FeedRateMode.Update(cmd.G);
				CutterRadiusCompensation.Update(cmd.G);
				ToolLengthOffset.Update(cmd.G);

				ProgramMode.Update(cmd.M);
				CoolantState.Update(cmd.M);
				SpindleState.Update(cmd.M);
			}

			public IEnumerable<Element> GetSettledModals()
			{
				List<Element> rv = new List<Element>();

				AddSettled(rv, CoordinateSelect);
				AddSettled(rv, PlaneSelect);
				AddSettled(rv, DistanceMode);
				AddSettled(rv, ArcDistanceMode);
				AddSettled(rv, FeedRateMode);
				AddSettled(rv, CutterRadiusCompensation);
				AddSettled(rv, ToolLengthOffset);
				AddSettled(rv, ProgramMode);
				AddSettled(rv, CoolantState);
				AddSettled(rv, SpindleState);

				return rv;
			}

			private void AddSettled(List<Element> list, ModalElement element)
			{if (element.IsSettled) list.Add(element);}

		}

		public bool IsSetWCO
		{ get { return G != null && G.Number == 92; } }

        public class G2G3Helper
		{
			public double CenterX;
			public double CenterY;
			public double Lenght => AngularWidth * Ray;
			public double AbsLenght => Math.Abs(Lenght);
			public bool CW;

			public double Ray;

			public double RectX;
			public double RectY;
			public double RectW;
			public double RectH;

			public Rect BBox;

			public double StartAngle;
			public double EndAngle;
			public double AngularWidth;

			public G2G3Helper(LaserGRBL.GrblCommand.StatePositionBuilder spb, LaserGRBL.GrblCommand cmd)
			{
				bool jb = cmd.JustBuilt;
				if (!jb) cmd.BuildHelper();

				CW = spb.G2;

				double aX = (double)spb.X.Previous; //startX
				double aY = (double)spb.Y.Previous; //startY
				double bX = (double)spb.X.Number;	//endX
				double bY = (double)spb.Y.Number;   //endY
				double oX = 0.0;
				double oY = 0.0;
				if (cmd.R != null) //G2G3 use R cmd
				{
					oX = ((aX + bX) / 2.0) - aX;
					oY = ((aY + bY) / 2.0) - aY;
				}
				else
				{
					oX = cmd.I != null ? (double)cmd.I.Number : 0.0; //offsetX
					oY = cmd.J != null ? (double)cmd.J.Number : 0.0; //offsetY
				}

				CenterX = aX + oX; //centerX
				CenterY = aY + oY; //centerY

				Ray = Math.Sqrt(oX * oX + oY * oY);  //raggio

				RectX = CenterX - Ray;
				RectY = CenterY - Ray;
				RectW = 2 * Ray;
				RectH = 2 * Ray;

				StartAngle = CalculateAngle(CenterX, CenterY, aX, aY); //angolo iniziale
				EndAngle = CalculateAngle(CenterX, CenterY, bX, bY); //angolo finale
				AngularWidth = AngularDistance(StartAngle, EndAngle, spb.G2);

				if (Circle(aX, aY, bX, bY))
					BBox = new Rect(RectX, RectY, RectW, RectH);
				else
					BBox = CW ? BBBox(EndAngle, StartAngle, Ray, CenterX, CenterY) : BBBox(StartAngle, EndAngle, Ray, CenterX, CenterY);

				if (!jb) cmd.DeleteHelper();
			}

			private bool Circle(double aX, double aY, double bX, double bY)
			{
				return aX == bX && aY == bY;
			}

			private static double CalculateAngle(double x1, double y1, double x2, double y2)
			{
				// returns Angle of line between 2 points and X axis (according to quadrants)
				double Angle = 0;

				if (x1 == x2 && y1 == y2) // same points
					return 0;
				else if (x1 == x2) // 90 or 270
				{
					Angle = Math.PI / 2;
					if (y1 > y2) Angle += Math.PI;
				}
				else if (y1 == y2) // 0 or 180
				{
					Angle = 0;
					if (x1 > x2) Angle += Math.PI;
				}
				else
				{
					Angle = Math.Atan(Math.Abs((y2 - y1) / (x2 - x1))); // 1. quadrant
					if (x1 > x2 && y1 < y2) // 2. quadrant
						Angle = Math.PI - Angle;
					else if (x1 > x2 && y1 > y2) // 3. quadrant
						Angle += Math.PI;
					else if (x1 < x2 && y1 > y2) // 4. quadrant
						Angle = 2 * Math.PI - Angle;
				}
				return Angle;
			}

			private static double AngularDistance(double aA, double bA, bool cw)
			{
				if (cw)
					return bA >= aA ? (bA - 2 * Math.PI - aA) : bA - aA;
				else
					return -(aA >= bA ? (aA - 2 * Math.PI - bA) : aA - bA);
			}


			private const double a0 = 0.0;
			private const double a90 = Math.PI / 2.0;
			private const double a180 = Math.PI;
			private const double a270 = Math.PI * 3.0 / 2.0;
			private const double a360 = Math.PI * 2;

			public static int GetQuadrant(Double angle)
			{
				var trueAngle = angle % (2 * Math.PI);

				if (trueAngle >= a0 && trueAngle < a90)
					return 1;
				else if (trueAngle >= a90 && trueAngle < a180)
					return 2;
				else if (trueAngle >= a180 && trueAngle < a270)
					return 3;
				else //if (trueAngle >= a270 && trueAngle < a360)
					return 4;
			}

			//Oleg Petrochenko alghorithm
			//From https://stackoverflow.com/questions/32365479/formula-to-calculate-bounding-coordinates-of-an-arc-in-space
			public static Rect BBBox(Double startAngle, Double endAngle, Double r, Double centerX, Double centerY)
			{
				int startQuad = GetQuadrant(startAngle) - 1;
				int endQuad = GetQuadrant(endAngle) - 1;

				// Convert to Cartesian coordinates.
				Point stPt = new Point(Math.Round(r * Math.Cos(startAngle), 14), Math.Round(r * Math.Sin(startAngle), 14));
				Point enPt = new Point(Math.Round(r * Math.Cos(endAngle), 14), Math.Round(r * Math.Sin(endAngle), 14));

				// Find bounding box excluding extremum.
				double minX = stPt.X;
				double minY = stPt.Y;
				double maxX = stPt.X;
				double maxY = stPt.Y;

				if (maxX < enPt.X) maxX = enPt.X;
				if (maxY < enPt.Y) maxY = enPt.Y;
				if (minX > enPt.X) minX = enPt.X;
				if (minY > enPt.Y) minY = enPt.Y;

				// Build extremum matrices.
				var xMax = new[,] { { maxX, r, r, r }, { maxX, maxX, r, r }, { maxX, maxX, maxX, r }, { maxX, maxX, maxX, maxX } };
				var yMax = new[,] { { maxY, maxY, maxY, maxY }, { r, maxY, r, r }, { r, maxY, maxY, r }, { r, maxY, maxY, maxY } };
				var xMin = new[,] { { minX, -r, minX, minX }, { minX, minX, minX, minX }, { -r, -r, minX, -r }, { -r, -r, minX, minX } };
				var yMin = new[,] { { minY, -r, -r, minY }, { minY, minY, -r, minY }, { minY, minY, minY, minY }, { -r, -r, -r, minY } };

				// Select desired values
				var startPt = new Point(xMin[endQuad, startQuad], yMin[endQuad, startQuad]);
				var endPt = new Point(xMax[endQuad, startQuad], yMax[endQuad, startQuad]);
				
				Rect rv = new Rect(startPt, endPt);
				rv.Offset(centerX, centerY);	//i conti sono fatti su un arco con centro in 0,0 quindi aggiungiamo il vero offset alla fine

				return rv;
			}
		}
	}
}
