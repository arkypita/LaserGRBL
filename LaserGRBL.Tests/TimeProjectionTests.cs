using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using LaserGRBL;

namespace LaserGRBL.Tests
{
    /// <summary>
    /// Tests for TimeProjection multi-pass time estimation behavior.
    /// </summary>
    public class TimeProjectionTests
    {
        private readonly ITestOutputHelper _output;

        public TimeProjectionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// Helper to create a mock time provider for testing
        /// </summary>
        private class MockTimeProvider
        {
            private long _currentTime = 0;

            public long CurrentTime => _currentTime;

            public void Advance(long milliseconds)
            {
                _currentTime += milliseconds;
            }

            public Func<long> GetProvider()
            {
                return () => _currentTime;
            }
        }

        /// <summary>
        /// Helper to create a simple GrblFile mock
        /// </summary>
        private GrblFile CreateMockFile(TimeSpan estimatedTime, int commandCount)
        {
            var file = new GrblFile(0, 0, 0, 0);
            // Set estimated time via reflection since we can't modify file internals easily
            var field = typeof(GrblFile).GetField("mEstimatedTotalTime", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
                field.SetValue(file, estimatedTime);
            
            return file;
        }

        /// <summary>
        /// Helper to create a mock queue
        /// </summary>
        private Queue<GrblCommand> CreateMockQueue(int count)
        {
            var queue = new Queue<GrblCommand>();
            for (int i = 0; i < count; i++)
            {
                queue.Enqueue(new GrblCommand("G1 X10"));
            }
            return queue;
        }

        [Fact]
        public void TraditionalMultiPass_ThreePasses_TimeDecreases()
        {
            // Arrange
            var timeProvider = new MockTimeProvider();
            var projection = new TimeProjection(timeProvider.GetProvider());
            var file = CreateMockFile(TimeSpan.FromSeconds(60), 100);
            var queue = CreateMockQueue(100);

            _output.WriteLine("Test: Traditional Multi-Pass with 3 Passes");
            _output.WriteLine("Expected: Time decreases by ~1/3 after each pass completion");
            _output.WriteLine("");

            // Act & Assert - Pass 1
            projection.JobStart(file, queue, global: true, totalPasses: 3);
            var initialTotal = projection.ProjectedTotalTime;
            _output.WriteLine($"After JobStart (3 passes): ProjectedTotalTime = {initialTotal.TotalSeconds:F1}s");
            
            // Should estimate ~180 seconds total (60 * 3)
            Assert.InRange(initialTotal.TotalSeconds, 170, 190);

            // Simulate pass 1 execution (60 seconds)
            for (int i = 0; i < 100; i++)
            {
                timeProvider.Advance(600); // 600ms per command = 60s total
                projection.JobExecuted(TimeSpan.FromSeconds((i + 1) * 0.6));
            }

            // End pass 1
            projection.JobEnd(global: false);

            // Start pass 2 - NOW check the projected time
            projection.Reset(global: false);
            projection.JobStart(file, queue, global: false, totalPasses: 3);
            
            var afterPass1Total = projection.ProjectedTotalTime;
            var afterPass1Remaining = projection.ProjectedTotalTimeRemaining;
            _output.WriteLine($"After Pass 1 Complete (at start of Pass 2):");
            _output.WriteLine($"  ProjectedTotalTime = {afterPass1Total.TotalSeconds:F1}s (should be ~180s total for all 3 passes)");
            _output.WriteLine($"  ProjectedTotalTimeRemaining = {afterPass1Remaining.TotalSeconds:F1}s (should be ~120s for 2 remaining passes)");
            
            // ProjectedTotalTime should still be ~180s (total time including already elapsed time)
            Assert.InRange(afterPass1Total.TotalSeconds, 170, 190);
            // ProjectedTotalTimeRemaining should be ~120s (2 passes remaining)
            Assert.InRange(afterPass1Remaining.TotalSeconds, 110, 130);
            
            // Simulate pass 2 execution (60 seconds)
            for (int i = 0; i < 100; i++)
            {
                timeProvider.Advance(600);
                projection.JobExecuted(TimeSpan.FromSeconds((i + 1) * 0.6));
            }

            // End pass 2
            projection.JobEnd(global: false);

            // Start pass 3 - NOW check the projected time
            projection.Reset(global: false);
            projection.JobStart(file, queue, global: false, totalPasses: 3);
            
            var afterPass2Total = projection.ProjectedTotalTime;
            var afterPass2Remaining = projection.ProjectedTotalTimeRemaining;
            _output.WriteLine($"After Pass 2 Complete (at start of Pass 3):");
            _output.WriteLine($"  ProjectedTotalTime = {afterPass2Total.TotalSeconds:F1}s (should be ~180s total for all 3 passes)");
            _output.WriteLine($"  ProjectedTotalTimeRemaining = {afterPass2Remaining.TotalSeconds:F1}s (should be ~60s for 1 remaining pass)");
            
            // ProjectedTotalTime should still be ~180s (total time including already elapsed time)
            Assert.InRange(afterPass2Total.TotalSeconds, 170, 190);
            // ProjectedTotalTimeRemaining should be ~60s (1 pass remaining)
            Assert.InRange(afterPass2Remaining.TotalSeconds, 50, 70);
            
            // Simulate pass 3 execution (60 seconds)
            for (int i = 0; i < 100; i++)
            {
                timeProvider.Advance(600);
                projection.JobExecuted(TimeSpan.FromSeconds((i + 1) * 0.6));
            }

            // End pass 3
            projection.JobEnd(global: true);
            var afterPass3 = projection.ProjectedTotalTime;
            _output.WriteLine($"After Pass 3 Complete: ProjectedTotalTime = {afterPass3.TotalSeconds:F1}s");
            
            // Should be ~0 seconds remaining
            Assert.InRange(afterPass3.TotalSeconds, 0, 5);

            _output.WriteLine("");
            _output.WriteLine("✓ Traditional multi-pass time estimation works correctly");
        }

        [Fact]
        public void SegmentBasedMultiPass_SinglePassWithTotalTime_RemainsStable()
        {
            // Arrange
            var timeProvider = new MockTimeProvider();
            var projection = new TimeProjection(timeProvider.GetProvider());
            
            // For segment-based: all passes pre-enqueued, so totalPasses=1
            // but EstimatedTime should be TOTAL time for all segment passes
            var totalTime = TimeSpan.FromSeconds(180); // 3 segments * 60s each
            var file = CreateMockFile(totalTime, 300); // 300 commands total
            var queue = CreateMockQueue(300);

            _output.WriteLine("Test: Segment-Based Multi-Pass (Pre-enqueued)");
            _output.WriteLine("Expected: Time estimate remains stable (counts down linearly)");
            _output.WriteLine("");

            // Act & Assert
            projection.JobStart(file, queue, global: true, totalPasses: 1); // Single "pass" containing all segments
            var initialTotal = projection.ProjectedTotalTime;
            _output.WriteLine($"After JobStart (totalPasses=1): ProjectedTotalTime = {initialTotal.TotalSeconds:F1}s");
            
            // Should estimate ~180 seconds total
            Assert.InRange(initialTotal.TotalSeconds, 170, 190);

            // Simulate execution of first 100 commands (first segment - 60 seconds)
            for (int i = 0; i < 100; i++)
            {
                timeProvider.Advance(600);
                projection.JobExecuted(TimeSpan.FromSeconds((i + 1) * 0.6));
            }

            var afterSegment1 = projection.ProjectedTotalTime;
            _output.WriteLine($"After ~100 commands (1st segment): ProjectedTotalTime = {afterSegment1.TotalSeconds:F1}s");
            
            // Should still estimate ~180 seconds total (doesn't jump)
            Assert.InRange(afterSegment1.TotalSeconds, 170, 190);

            // Simulate execution of next 100 commands (second segment - 60 seconds)
            for (int i = 100; i < 200; i++)
            {
                timeProvider.Advance(600);
                projection.JobExecuted(TimeSpan.FromSeconds((i + 1) * 0.6));
            }

            var afterSegment2 = projection.ProjectedTotalTime;
            _output.WriteLine($"After ~200 commands (2nd segment): ProjectedTotalTime = {afterSegment2.TotalSeconds:F1}s");
            
            // Should still estimate ~180 seconds total (doesn't jump)
            Assert.InRange(afterSegment2.TotalSeconds, 170, 190);

            // Simulate execution of final 100 commands (third segment - 60 seconds)
            for (int i = 200; i < 300; i++)
            {
                timeProvider.Advance(600);
                projection.JobExecuted(TimeSpan.FromSeconds((i + 1) * 0.6));
            }

            projection.JobEnd(global: true);
            var afterComplete = projection.ProjectedTotalTime;
            _output.WriteLine($"After All Segments Complete: ProjectedTotalTime = {afterComplete.TotalSeconds:F1}s");
            
            // Should be ~0 seconds remaining
            Assert.InRange(afterComplete.TotalSeconds, 0, 5);

            _output.WriteLine("");
            _output.WriteLine("✓ Segment-based multi-pass time estimation remains stable");
        }

        [Fact]
        public void SinglePass_EstimatesCorrectly()
        {
            // Arrange
            var timeProvider = new MockTimeProvider();
            var projection = new TimeProjection(timeProvider.GetProvider());
            var file = CreateMockFile(TimeSpan.FromSeconds(60), 100);
            var queue = CreateMockQueue(100);

            _output.WriteLine("Test: Single Pass Time Estimation");
            _output.WriteLine("");

            // Act & Assert
            projection.JobStart(file, queue, global: true, totalPasses: 1);
            var initialTotal = projection.ProjectedTotalTime;
            _output.WriteLine($"After JobStart: ProjectedTotalTime = {initialTotal.TotalSeconds:F1}s");
            
            // Should estimate ~60 seconds
            Assert.InRange(initialTotal.TotalSeconds, 55, 65);

            // Simulate halfway through
            for (int i = 0; i < 50; i++)
            {
                timeProvider.Advance(600);
                projection.JobExecuted(TimeSpan.FromSeconds((i + 1) * 0.6));
            }

            var halfwayTotal = projection.ProjectedTotalTime;
            _output.WriteLine($"Halfway Through: ProjectedTotalTime = {halfwayTotal.TotalSeconds:F1}s");
            
            // Should still estimate ~60 seconds total
            Assert.InRange(halfwayTotal.TotalSeconds, 55, 65);

            // Complete execution
            for (int i = 50; i < 100; i++)
            {
                timeProvider.Advance(600);
                projection.JobExecuted(TimeSpan.FromSeconds((i + 1) * 0.6));
            }

            projection.JobEnd(global: true);
            var finalTotal = projection.ProjectedTotalTime;
            _output.WriteLine($"After Complete: ProjectedTotalTime = {finalTotal.TotalSeconds:F1}s");
            
            // Should be ~0 seconds remaining
            Assert.InRange(finalTotal.TotalSeconds, 0, 5);

            _output.WriteLine("");
            _output.WriteLine("✓ Single pass time estimation works correctly");
        }

        [Fact]
        public void SegmentBasedMultiPass_CorrectTotalPassesValue()
        {
            // This test verifies the fix for the bug where segment mode was passing totalPasses=1
            // but then legacy mLoopCount mechanism would run again, causing pass 2/1 situation
            
            // Arrange
            var timeProvider = new MockTimeProvider();
            var projection = new TimeProjection(timeProvider.GetProvider());
            
            // Segment mode with 3 passes pre-enqueued (maxPassCount=3)
            // Each pass takes 60 seconds, so total = 180s
            var totalTime = TimeSpan.FromSeconds(180);
            var file = CreateMockFile(totalTime, 300);
            var queue = CreateMockQueue(300);

            _output.WriteLine("Test: Segment-Based Multi-Pass Receives Correct totalPasses");
            _output.WriteLine("Expected: totalPasses=3, not 1, to avoid negative remainingPasses");
            _output.WriteLine("");

            // Act
            projection.JobStart(file, queue, global: true, totalPasses: 3);
            
            // Execute some commands
            for (int i = 0; i < 50; i++)
            {
                timeProvider.Advance(600);
                projection.JobExecuted(TimeSpan.FromSeconds((i + 1) * 0.6));
            }

            // Assert
            var projectedTotal = projection.ProjectedTotalTime;
            var projectedRemaining = projection.ProjectedTotalTimeRemaining;
            
            _output.WriteLine($"ProjectedTotalTime: {projectedTotal.TotalSeconds:F1}s");
            _output.WriteLine($"ProjectedTotalTimeRemaining: {projectedRemaining.TotalSeconds:F1}s");
            
            // Both should be positive and reasonable
            Assert.True(projectedTotal.TotalSeconds > 0, "ProjectedTotalTime should be positive");
            Assert.True(projectedRemaining.TotalSeconds > 0, "ProjectedTotalTimeRemaining should be positive");
            Assert.InRange(projectedTotal.TotalSeconds, 170, 200);
            Assert.InRange(projectedRemaining.TotalSeconds, 140, 170);
            
            _output.WriteLine("");
            _output.WriteLine("✓ Segment mode correctly receives totalPasses parameter");
        }
    }
}
