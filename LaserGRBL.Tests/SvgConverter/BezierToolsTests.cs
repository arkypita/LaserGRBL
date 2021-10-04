using System;
using System.Linq;
using System.Windows;
using LaserGRBL.SvgConverter;
using Xunit;

namespace LaserGRBL.Tests
{
    public class BezierToolsTests
    {
        [InlineData(new double[] { 0, 0, 0, 1, 0, 2, 0, 3 })] /* vertical line LR */
        [InlineData(new double[] { 0, 3, 0, 2, 0, 1, 0, 0 })] /* vertical line RL */
        [InlineData(new double[] { 0, 0, 1, 0, 2, 0, 3, 0 })] /* horizontal line (regression) */
        [InlineData(new double[] { 0, 0, 1, 1, 2, 2, 3, 3 })] /* diagonal */
        [Theory]
        public void FlattenTo_NoSubdivisionWhenLinear(double[] polygon)
        {
            var points = GetPoints(polygon);
            var result = BezierTools.FlattenTo(points, .001);
            Assert.Equal(4, result.Count());
        }
        
        [InlineData(new double[] { 0,0,  0,1,  1,1,  1,0 }, 0.5, 1 + 3)] /* should produce single line at error 0.5 */
        [InlineData(new double[] { 0, 0, 0, 1, 1, 1, 1, 0 }, 0.25, 1 + 3 * 2)] /* should produce triangle at error 0.5^2 */
        [InlineData(new double[] { 0, 0, 0, 1, 1, 1, 1, 0 }, 0.0625, 1 + 3 * 4)] /* should produce quad-arc at error 0.5^3 */
        [Theory]
        public void FlattenTo_SubdividesToCorrectError(double[] polygon, double acceptedError, int expectedPoints)
        {
            var points = GetPoints(polygon);
            var result = BezierTools.FlattenTo(points, acceptedError);
            Assert.Equal(expectedPoints, result.Count());
        }

        public Point[] GetPoints(double[] p)
        {
            return new[] {new Point(p[0], p[1]), new Point(p[2], p[3]), new Point(p[4], p[5]), new Point(p[6], p[7])};
        }
    }
}
