using System;
using System.Linq;
using System.Windows;
using LaserGRBL.SvgConverter;
using Xunit;

namespace LaserGRBL.Tests
{
    public class BezierToolsTests
    {
        [InlineData(new double[] { 0,0,  0,1,  0,2,  0,3 })] /* vertical line LR */
        [InlineData(new double[] { 0,3,  0,2,  0,1,  0,0 })] /* vertical line RL */
        [InlineData(new double[] { 0,0,  1,0,  2,0,  3,0 })] /* horizontal line, catches det == 0 bug. */
        [InlineData(new double[] { 0,0,  1,1,  2,2,  3,3 })] /* diagonal */
        [Theory]
        public void CalculateFlatnessError_ZeroErrorWhenLinear(double[] polygon)
        {
            var points = GetPoints(polygon);
            var result = BezierTools.CalculateFlatnessError(points, 3);
            Assert.Equal(0, result);
        }

        [InlineData(3.0, new double[] { 0,0,  1,2,  3,3,  4,2 })] /* from method comment */
        [Theory]
        public void CalculateFlatnessError_PassesSpecificRegressionChecks(double expectedError, double[] polygon)
        {
            var points = GetPoints(polygon);
            var result = BezierTools.CalculateFlatnessError(points, 3);
            Assert.Equal(expectedError, result);
        }

        public Point[] GetPoints(double[] p)
        {
            return new[] {new Point(p[0], p[1]), new Point(p[2], p[3]), new Point(p[4], p[5]), new Point(p[6], p[7])};
        }
    }
}
