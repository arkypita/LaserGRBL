using System;
using System.Collections.Generic;
using LaserGRBL.PrintAndCut;
using Xunit;

namespace LaserGRBL.Tests.PrintAndCut
{
	public class PrintAndCutManagerTests
	{
		// two marks: #1 top-left centered at (10, 190), #2 bottom-right centered at (190, 10)
		private static readonly List<RegistrationMark> Marks = MarkDetector.Detect(new List<Bounds>
		{
			new Bounds(185, 5, 195, 15),
			new Bounds(5, 185, 15, 195),
		});

		private static readonly Vec2 NoOffset = new Vec2(0, 0);

		private static PrintAndCutManager WithTargets(Vec2 m1, Vec2 m2)
		{
			var pnc = new PrintAndCutManager();
			pnc.SetFirstTarget(Marks[0], m1);
			pnc.SetSecondTarget(Marks[1], m2);
			return pnc;
		}

		[Fact]
		public void NewManager_IsDisabledWithoutTargets()
		{
			var pnc = new PrintAndCutManager();

			Assert.False(pnc.IsEnabled);
			Assert.False(pnc.CanAlign);
			Assert.Null(pnc.GetTransform(NoOffset));
		}

		[Fact]
		public void Align_WithoutBothTargets_Throws()
		{
			var pnc = new PrintAndCutManager();
			pnc.SetFirstTarget(Marks[0], new Vec2(10, 190));

			Assert.Throws<InvalidOperationException>(() => pnc.Align(true, NoOffset));
			Assert.False(pnc.IsEnabled);
		}

		[Fact]
		public void Align_SameMarkTwice_Throws()
		{
			var pnc = new PrintAndCutManager();
			pnc.SetFirstTarget(Marks[0], new Vec2(10, 190));
			pnc.SetSecondTarget(Marks[0], new Vec2(190, 10));

			Assert.Throws<InvalidOperationException>(() => pnc.Align(true, NoOffset));
		}

		[Fact]
		public void Align_MachineTargetsTooClose_ThrowsAndStaysDisabled()
		{
			var pnc = WithTargets(new Vec2(50, 50), new Vec2(50.2, 50));

			Assert.Throws<ArgumentException>(() => pnc.Align(true, NoOffset));
			Assert.False(pnc.IsEnabled);
		}

		[Fact]
		public void Align_EnablesAndMapsMarksOnMachinePositions()
		{
			var pnc = WithTargets(new Vec2(20, 200), new Vec2(200, 20));

			var t = pnc.Align(false, NoOffset);

			Assert.True(pnc.IsEnabled);
			Assert.False(pnc.AllowScaling);
			Assert.Equal(20, t.TransformPoint(Marks[0].Center).X, 9);
			Assert.Equal(200, t.TransformPoint(Marks[0].Center).Y, 9);
		}

		[Fact]
		public void GetTransform_ConvertsMachinePositionsToWorkCoordinates()
		{
			// positions are MPos; with WCO (100, 50) the first mark must land on WPos = MPos - WCO
			var pnc = WithTargets(new Vec2(110, 240), new Vec2(290, 60));
			pnc.Align(true, NoOffset);

			var t = pnc.GetTransform(new Vec2(100, 50));
			var p = t.TransformPoint(Marks[0].Center);

			Assert.Equal(10, p.X, 9);
			Assert.Equal(190, p.Y, 9);
		}

		[Fact]
		public void SetTarget_WhileEnabled_DisablesAlignment()
		{
			var pnc = WithTargets(new Vec2(10, 190), new Vec2(190, 10));
			pnc.Align(true, NoOffset);

			pnc.SetSecondTarget(Marks[1], new Vec2(191, 10));

			Assert.False(pnc.IsEnabled);
			Assert.True(pnc.CanAlign);
		}

		[Fact]
		public void Reset_ForgetsEverything()
		{
			var pnc = WithTargets(new Vec2(10, 190), new Vec2(190, 10));
			pnc.Align(true, NoOffset);

			pnc.Reset();

			Assert.False(pnc.IsEnabled);
			Assert.False(pnc.HasFirstTarget);
			Assert.False(pnc.HasSecondTarget);
			Assert.Null(pnc.FirstMark);
			Assert.Null(pnc.GetTransform(NoOffset));
		}

		[Fact]
		public void PreviewAlignment_DoesNotEnable()
		{
			var pnc = WithTargets(new Vec2(10, 190), new Vec2(195, 10));

			var t = pnc.PreviewAlignment(false, NoOffset);

			Assert.False(pnc.IsEnabled);
			Assert.True(t.MeasuredScale > 1);
		}

		[Fact]
		public void MarkSelection_DefaultsToFirstTwoMarks()
		{
			var pnc = new PrintAndCutManager();

			Assert.Equal(1, pnc.FirstMarkSelection);
			Assert.Equal(2, pnc.SecondMarkSelection);
		}

		[Fact]
		public void MarkSelection_RaisesStateChangedWithoutChangingVersion()
		{
			var pnc = new PrintAndCutManager();
			int raised = 0;
			pnc.StateChanged += (s, e) => raised++;
			int v0 = pnc.Version;

			pnc.SecondMarkSelection = 3;

			Assert.Equal(3, pnc.SecondMarkSelection);
			Assert.Equal(1, raised);
			Assert.Equal(v0, pnc.Version);
		}

		[Fact]
		public void PredictMachinePosition_WithoutTargets_UsesWorkCoordinates()
		{
			var pnc = new PrintAndCutManager();

			var p = pnc.PredictMachinePosition(new Vec2(190, 10), new Vec2(100, 50));

			Assert.Equal(290, p.X, 9);
			Assert.Equal(60, p.Y, 9);
		}

		[Fact]
		public void PredictMachinePosition_FromFirstTarget_AssumesNoRotation()
		{
			var pnc = new PrintAndCutManager();
			pnc.SetFirstTarget(Marks[0], new Vec2(110, 240)); // mark #1 design (10,190)

			var p = pnc.PredictMachinePosition(Marks[1].Center, new Vec2(999, 999)); // offset ignored here

			Assert.Equal(290, p.X, 9); // 110 + (190 - 10)
			Assert.Equal(60, p.Y, 9);  // 240 + (10 - 190)
		}

		[Fact]
		public void PredictMachinePosition_WhenEnabled_UsesAlignment()
		{
			var pnc = WithTargets(new Vec2(110, 240), new Vec2(290, 60));
			pnc.Align(true, NoOffset);

			var p = pnc.PredictMachinePosition(Marks[1].Center, new Vec2(100, 50));

			Assert.Equal(290, p.X, 6);
			Assert.Equal(60, p.Y, 6);
		}

		[Fact]
		public void EveryChange_RaisesStateChangedAndBumpsVersion()
		{
			var pnc = new PrintAndCutManager();
			int raised = 0;
			pnc.StateChanged += (s, e) => raised++;
			int v0 = pnc.Version;

			pnc.SetFirstTarget(Marks[0], new Vec2(10, 190));
			pnc.SetSecondTarget(Marks[1], new Vec2(190, 10));
			pnc.Align(true, NoOffset);
			pnc.Reset();

			Assert.Equal(4, raised);
			Assert.Equal(v0 + 4, pnc.Version);
		}
	}
}
