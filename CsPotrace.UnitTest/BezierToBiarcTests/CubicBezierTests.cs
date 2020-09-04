﻿using CsPotrace.BezierToBiarc;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Xunit;

namespace CsPotrace.UnitTest.BezierToBiarcTests
{
    public class CubicBezierTests
    {
        [Fact]
        public void CsPotrace_CubicBezier_EmptyCtorIsZero()
        {
            var cb = new CubicBezier();
            Assert.Equal(Vector2.Zero, cb.P1);
            Assert.Equal(Vector2.Zero, cb.P2);
            Assert.Equal(Vector2.Zero, cb.C1);
            Assert.Equal(Vector2.Zero, cb.C2);
        }

        [Fact]
        public void CsPotrace_CubicBezier_CtorNonZero()
        {
            // P1, C1, C2, P2
            var cb = new CubicBezier(new Vector2(1f, 2f),
                                     new Vector2(3f, 4f),
                                     new Vector2(5f, 6f),
                                     new Vector2(7f, 8f));

            Assert.Equal(1f, cb.P1.X);
            Assert.Equal(2f, cb.P1.Y);
            Assert.Equal(3f, cb.C1.X);
            Assert.Equal(4f, cb.C1.Y);
            Assert.Equal(5f, cb.C2.X);
            Assert.Equal(6f, cb.C2.Y);
            Assert.Equal(7f, cb.P2.X);
            Assert.Equal(8f, cb.P2.Y);
        }
        [Fact]
        public void CsPotrace_CubicBezier_IsClockWise()
        {
            var cb = simpleSemiArc();
            Assert.True(cb.IsClockwise);
        }
        [Fact]
        public void CsPotrace_CubicBezier_IsClockWiseReversed()
        {
            var cb = simpleSemiArcReversed();
            Assert.False(cb.IsClockwise);
        }
        [Fact]
        public void CsPotrace_CubicBezier_Example()
        {
            // https://stackoverflow.com/a/1165943/734639
            // This posts states that 
            // point[0] = (5, 0)   edge[0]: (6 - 5)(4 + 0) = 4
            // point[1] = (6, 4)   edge[1]: (4 - 6)(5 + 4) = -18
            // point[2] = (4, 5)   edge[2]: (1 - 4)(5 + 5) = -30
            // point[3] = (1, 5)   edge[3]: (1 - 1)(0 + 5) = 0
            // point[4] = (1, 0)   edge[4]: (5 - 1)(0 + 0) = 0
            // Is Counter Clock Wise

            var cb = new CubicBezier(new Vector2(5f, 0f),
                                     new Vector2(6f, 5f),
                                     new Vector2(4f, 5f),
                                     new Vector2(1f, 5f));
            Assert.False(cb.IsClockwise);
        }

        [Fact]
        public void CsPotrace_CubicBezier_InflexionPoints()
        {
            var cb = simpleSemiArc();
            var inflexionPoints = cb.InflexionPoints;

            Assert.Equal(0.5, inflexionPoints.Item1.Real);
            Assert.Equal(-0.5, inflexionPoints.Item1.Imaginary);

            Assert.Equal(0.5, inflexionPoints.Item2.Real);
            Assert.Equal(0.5, inflexionPoints.Item2.Imaginary);
        }

        [Fact]
        public void CsPotrace_CubicBezier_Split()
        {
            var cb = simpleSemiArc();
            var split = cb.Split(0.5f);

            // Check ends points
            Assert.Equal(cb.P1, split.Item1.P1);
            Assert.Equal(cb.P2, split.Item2.P2);

            // Check if the ends match
            Assert.Equal(split.Item1.P2, split.Item2.P1);
        }

        [Fact]
        public void CsPotrace_CubicBezier_PointAt()
        {
            var cb = simpleSemiArc();
            var point0 = cb.PointAt(0);
            var point25 = cb.PointAt(0.25f);
            var point50 = cb.PointAt(0.50f);
            var point75 = cb.PointAt(0.75f);
            var point1 = cb.PointAt(1);

            Assert.Equal(cb.P1, point0);
            Assert.Equal(new Vector2(0.15625f, 0.5625f), point25);
            Assert.Equal(new Vector2(0.5f, 0.75f), point50);
            Assert.Equal(new Vector2(0.84375f, 0.5625f), point75);
            Assert.Equal(cb.P2, point1);
        }

        private static CubicBezier simpleSemiArc()
        {
            return new CubicBezier(new Vector2(0f, 0f),
                                   new Vector2(0f, 1f),
                                   new Vector2(1f, 1f),
                                   new Vector2(1f, 0f));
        }
        private static CubicBezier simpleSemiArcReversed()
        {
            return new CubicBezier(new Vector2(1f, 0f),
                                   new Vector2(1f, 1f),
                                   new Vector2(0f, 1f),
                                   new Vector2(0f, 0f));
        }
    }
}
