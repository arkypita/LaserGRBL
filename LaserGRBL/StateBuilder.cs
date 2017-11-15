using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
	public partial class GrblCommand
	{
		public class StatePositionBuilder : StateBuilder
		{
			bool supportPWM = (bool)Settings.GetObject("Support Hardware PWM", true);

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

				public void Update(Element e, bool Absolute)
				{
					mPrevious = mNumber;

					if (e != null)
					{
						mNumber = Absolute ? e.Number : mNumber + e.Number;
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
			private LastValueElement mCurF = new LastValueElement("F0");
			private LastValueElement mCurS = new LastValueElement("S0");

			public class AnalyzeCommandRV
			{
				public decimal Distance;
				public TimeSpan Delay;
			}

			public TimeSpan AnalyzeCommand(GrblCommand cmd, bool compute, GrblConf conf = null)
			{
				bool delete = !cmd.JustBuilt;
				if (!cmd.JustBuilt) cmd.BuildHelper();

				UpdateModalsNB(cmd);

				mCurX.Update(cmd.X, ABS);
				mCurY.Update(cmd.Y, ABS);

				mCurF.Update(cmd.F);
				mCurS.Update(cmd.S);

				TimeSpan rv = compute ? ComputeExecutionTime(cmd, conf) : TimeSpan.Zero;
				if (delete) cmd.DeleteHelper();

				return rv;
			}

			public LastValueElement F
			{ get { return mCurF; } }

			public LastValueElement S
			{ get { return mCurS; } }

			public CumulativeElement X
			{ get { return mCurX; } }

			public CumulativeElement Y
			{ get { return mCurY; } }

			internal bool TrueMovement()
			{ return (mCurX.Number != mCurX.Previous || mCurY.Number != mCurY.Previous); }


			private TimeSpan ComputeExecutionTime(GrblCommand cmd, GrblConf conf)
			{
				if (G0 && cmd.IsLinearMovement)
					return TimeSpan.FromMinutes((double)GetSegmentLenght(cmd) / (double)conf.MaxRateX); //todo: use a better computation of xy if different speed
				else if (G1G2G3 && cmd.IsMovement && mCurF.Number != 0)
					return TimeSpan.FromMinutes((double)GetSegmentLenght(cmd) / (double)Math.Min(mCurF.Number, conf.MaxRateX));
				else if (cmd.IsPause)
					return cmd.P != null ? TimeSpan.FromSeconds((double)cmd.P.Number) : cmd.S != null ? TimeSpan.FromSeconds((double)cmd.S.Number) : TimeSpan.Zero;
				else
					return TimeSpan.Zero;
			}

			private decimal GetSegmentLenght(GrblCommand cmd)
			{
				if (cmd.IsLinearMovement)
					return Tools.MathHelper.LinearDistance(mCurX.Previous, mCurY.Previous, mCurX.Number, mCurY.Number);
				else if (cmd.IsArcMovement) //arc of given radius
					return Tools.MathHelper.ArcDistance(mCurX.Previous, mCurY.Previous, mCurX.Number, mCurY.Number, cmd.GetArcRadius());
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
	}
}
