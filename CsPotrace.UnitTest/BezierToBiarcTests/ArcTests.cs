using CsPotrace.BezierToBiarc;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace CsPotrace.UnitTest.BezierToBiarcTests
{
    // A lot of this tests were implemented long after the library was in-use,
    // My first goal is to asssure that what was woking will continue working the SAME way
    // If any bug appears, it will be frozen broken in this tests and will be fixed later

    public class ArcTests
    {
        // Is already stated at Arc class that are redundant information

        const int floatPrecision = 6;

        [Fact]
        public void CsPotrace_Arc_EmptyCtorIsZero()
        {
            var arc = new Arc();
            Assert.Equal(0f, arc.C.X);
            Assert.Equal(0f, arc.C.Y);

            Assert.Equal(0f, arc.r);
            Assert.Equal(0f, arc.startAngle);
            Assert.Equal(0f, arc.sweepAngle);

            Assert.Equal(0f, arc.P1.X);
            Assert.Equal(0f, arc.P1.Y);
            Assert.Equal(0f, arc.P2.X);
            Assert.Equal(0f, arc.P2.Y);
        }
        [Fact]
        public void CsPotrace_Arc_EmptyCtorIsNonZero()
        {
            var arc = new Arc(new Vector2(0.1f, 0.2f),
                              0.3f, 0.4f, 0.5f,
                              new Vector2(0.6f, 0.7f),
                              new Vector2(0.8f, 0.9f));

            Assert.Equal(0.1f, arc.C.X);
            Assert.Equal(0.2f, arc.C.Y);

            Assert.Equal(0.3f, arc.r);
            Assert.Equal(0.4f, arc.startAngle);
            Assert.Equal(0.5f, arc.sweepAngle);

            Assert.Equal(0.6f, arc.P1.X);
            Assert.Equal(0.7f, arc.P1.Y);
            Assert.Equal(0.8f, arc.P2.X);
            Assert.Equal(0.9f, arc.P2.Y);
        }

        [Fact]
        public void CsPotrace_Arc_IsClockwise()
        {
            Arc arc = new Arc();
            // as today, zero is NOT clockwise
            Assert.False(arc.IsClockwise);

            arc = new Arc(Vector2.Zero, 0, 0, 1, Vector2.Zero, Vector2.Zero);
            Assert.True(arc.IsClockwise);

            arc = new Arc(Vector2.Zero, 0, 0, -1, Vector2.Zero, Vector2.Zero);
            Assert.False(arc.IsClockwise);
        }

        [Fact]
        public void CsPotrace_Arc_PoitAt()
        {
            var unity = unityCircle();
            // 0
            Vector2Equal(unity.P1, unity.PointAt(0f));
            // 2Pi
            Vector2Equal(unity.P2, unity.PointAt(1f));
            // 1Pi
            Vector2Equal(new Vector2(0, 1), unity.PointAt(0.25f));
            // 3Pi/2
            Vector2Equal(new Vector2(0, -1), unity.PointAt(0.75f));
        }
        [Fact]
        public void CsPotrace_Arc_Length()
        {
            var unity = unityCircle();
            Assert.Equal(2 * Math.PI, unity.Length, floatPrecision);
        }
        [Fact]
        public void CsPotrace_Arc_BiggerLength()
        {
            // 10x bigger, use less precision !!
            var unity = biggerCircle();
            Assert.Equal(20 * Math.PI, unity.Length, 4);
        }
        [Fact]
        public void CsPotrace_Arc_HalfLength()
        {
            var unity = new Arc(C: Vector2.Zero,
                                r: 1,
                                startAngle: 0,
                                sweepAngle: (float)(Math.PI), // 180deg
                                P1: new Vector2(1, 0),
                                P2: new Vector2(-1, 0));

            Assert.Equal(Math.PI, unity.Length, floatPrecision);
        }
        [Fact]
        public void CsPotrace_Arc_LinearLength()
        {
            var unity = unityCircle(); // Unity Circle has a ZERO LinearLength
            Assert.Equal(0, unity.LinearLength);
        }
        [Fact]
        public void CsPotrace_Arc_HalfArcLinearLength()
        {
            var unity = new Arc(C: Vector2.Zero,
                                r: 1,
                                startAngle: 0,
                                sweepAngle: (float)(Math.PI), // 180deg
                                P1: new Vector2(1, 0),
                                P2: new Vector2(-1, 0));
            Assert.Equal(2, unity.LinearLength); // From -1 to 1
        }
        [Fact]
        public void CsPotrace_Arc_TranslatedHalfArcLinearLength()
        {
            var unity = new Arc(C: new Vector2(1, 1), // translated
                                r: 1,
                                startAngle: 0,
                                sweepAngle: (float)(Math.PI), // 180deg
                                P1: new Vector2(1, 0),
                                P2: new Vector2(-1, 0));
            Assert.Equal(2, unity.LinearLength); // From -1 to 1
        }

        private static void Vector2Equal(Vector2 v1, Vector2 v2)
        {
            Assert.Equal(v1.X, v2.X, floatPrecision);
            Assert.Equal(v1.Y, v2.Y, floatPrecision);
        }

        private static Arc unityCircle()
        {
            return new Arc(C: Vector2.Zero,
                           r: 1,
                           startAngle: 0,
                           sweepAngle: (float)(2 * Math.PI), // 360deg
                           P1: new Vector2(1, 0),
                           P2: new Vector2(1, 0));
        }
        private static Arc biggerCircle()
        {
            return new Arc(C: Vector2.Zero,
                           r: 10,
                           startAngle: 0,
                           sweepAngle: (float)(2 * Math.PI), // 360deg
                           P1: new Vector2(10, 0),
                           P2: new Vector2(10, 0));
        }
    }
}
