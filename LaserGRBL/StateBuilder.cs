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
			public Element X = "X0";
			public Element Y = "Y0";
			public Element F = "F0";
			public Element S = "S0";

			public override void Update(GrblCommand cmd)
			{
				bool delete = !cmd.JustBuilt;
				if (!cmd.JustBuilt) cmd.BuildHelper();

				UpdateModals(cmd);

				if (cmd.TrueMovement(X.Number, Y.Number, Absolute))
				{
					if (cmd.X != null) X.SetNumber(Absolute ? cmd.X.Number : X.Number + cmd.X.Number);
					if (cmd.Y != null) Y.SetNumber(Absolute ? cmd.Y.Number : Y.Number + cmd.Y.Number);
				}

				if (cmd.F != null) F.SetNumber(cmd.F.Number);
				if (cmd.S != null) S.SetNumber(cmd.S.Number);

				if (delete) cmd.DeleteHelper();
			}

			private bool Absolute
			{ get { return PlaneSelect.Number == 90; } }

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
				Element mDefault = "G0";
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

				private void Update(Element e)
				{
					if (e != null && mOptions.Contains(e))
					{ 
						mCommand = e.Command;
						mNumber = e.Number;
						mSettled = true;
					}
				}

				public void Update(Element G, Element M)
				{
					Update(G);
					Update(M);
				}
			}

			protected ModalElement MotionMode = new ModalElement("G0", "G1", "G2", "G3", "G38.2", "G38.3", "G38.4", "G38.5", "G80");
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

			public virtual void Update(GrblCommand cmd)
			{
				bool delete = !cmd.JustBuilt;
				if (!cmd.JustBuilt) cmd.BuildHelper();

				UpdateModals(cmd);

				if (delete) cmd.DeleteHelper();
			}

			protected void UpdateModals(GrblCommand cmd)
			{
				Element G = cmd.GetElement('G');
				Element M = cmd.GetElement('M');

				MotionMode.Update(G, M);
				CoordinateSelect.Update(G, M);
				PlaneSelect.Update(G, M);
				DistanceMode.Update(G, M);
				ArcDistanceMode.Update(G, M);
				FeedRateMode.Update(G, M);
				CutterRadiusCompensation.Update(G, M);
				ToolLengthOffset.Update(G, M);
				ProgramMode.Update(G, M);
				SpindleState.Update(G, M);
				CoolantState.Update(G, M);
			}

			public IEnumerable<ModalElement> GetSettledModals()
			{
 				List<ModalElement> rv = new List<ModalElement>();

				AddSettled(rv, MotionMode);
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

			private void AddSettled(List<ModalElement> list, ModalElement element)
			{if (element.IsSettled) list.Add(element);}

		}
	}
}
