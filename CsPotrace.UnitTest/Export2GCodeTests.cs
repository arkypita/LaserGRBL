using CsPotrace.UnitTest.CsPotraceTests;
using System.IO;
using Xunit;

namespace CsPotrace.UnitTest
{
    // A lot of this tests were implemented long after the library was in-use,
    // My first goal is to asssure that what was woking will continue working the SAME way
    // If any bug appears, it will be frozen broken in this tests and will be fixed later

    public class Export2GCodeTests
    {
        [Fact]
        public void CsPotrace_Export2GCode_Export2GCode_0010()
        {
            using var bmp = PotraceTraceTests.getSquare();
            var trace = new Potrace().PotraceTrace(bmp);
            // Image size is only used for DEBUG (static variable hardocded to FALSE)
            var gcodeLines = Potrace.Export2GCode(trace, 0, 0, 10, "ON", "OFF", bmp.Size);

            Assert.Equal(11, gcodeLines.Count);
            Assert.Equal("G0 X0.2 Y0.5", gcodeLines[0]);
            Assert.Equal("ON", gcodeLines[1]);
            Assert.Equal("G1 X0.288 Y0.712", gcodeLines[2]);
            Assert.Equal("G2 X0.5 Y0.8 I0.212 J-0.212", gcodeLines[3]);
            Assert.Equal("G2 X0.712 Y0.712 I0 J-0.3", gcodeLines[4]);
            Assert.Equal("G1 X0.8 Y0.5", gcodeLines[5]);
            Assert.Equal("G1 X0.712 Y0.288", gcodeLines[6]);
            Assert.Equal("G2 X0.5 Y0.2 I-0.212 J0.212", gcodeLines[7]);
            Assert.Equal("G2 X0.288 Y0.288 I0 J0.3", gcodeLines[8]);
            Assert.Equal("G1 X0.2 Y0.5", gcodeLines[9]);
            Assert.Equal("OFF", gcodeLines[10]);
        }

        [Fact]
        public void CsPotrace_Export2GCode_Export2GCode_5520()
        {
            using var bmp = PotraceTraceTests.getSquare();
            var trace = new Potrace().PotraceTrace(bmp);
            // Image size is only used for DEBUG (static variable hardocded to FALSE)
            var gcodeLines = Potrace.Export2GCode(trace, 5, 5, 20, "ON", "OFF", bmp.Size);

            Assert.Equal(11, gcodeLines.Count);
            Assert.Equal("G0 X5.1 Y5.25", gcodeLines[0]);
            Assert.Equal("ON", gcodeLines[1]);
            Assert.Equal("G1 X5.144 Y5.356", gcodeLines[2]);
            Assert.Equal("G2 X5.25 Y5.4 I0.106 J-0.106", gcodeLines[3]);
            Assert.Equal("G2 X5.356 Y5.356 I0 J-0.15", gcodeLines[4]);
            Assert.Equal("G1 X5.4 Y5.25", gcodeLines[5]);
            Assert.Equal("G1 X5.356 Y5.144", gcodeLines[6]);
            Assert.Equal("G2 X5.25 Y5.1 I-0.106 J0.106", gcodeLines[7]);
            Assert.Equal("G2 X5.144 Y5.144 I0 J0.15", gcodeLines[8]);
            Assert.Equal("G1 X5.1 Y5.25", gcodeLines[9]);
            Assert.Equal("OFF", gcodeLines[10]);
        }

        [Fact]
        public void CsPotrace_Export2GCode_Export2GCode_N5520()
        {
            using var bmp = PotraceTraceTests.getSquare();
            var trace = new Potrace().PotraceTrace(bmp);
            // Image size is only used for DEBUG (static variable hardocded to FALSE)
            var gcodeLines = Potrace.Export2GCode(trace, -5, -5, 20, "ON", "OFF", bmp.Size);

            Assert.Equal(11, gcodeLines.Count);
            Assert.Equal("G0 X-4.9 Y-4.75", gcodeLines[0]);
            Assert.Equal("ON", gcodeLines[1]);
            Assert.Equal("G1 X-4.856 Y-4.644", gcodeLines[2]);
            Assert.Equal("G2 X-4.75 Y-4.6 I0.106 J-0.106", gcodeLines[3]);
            Assert.Equal("G2 X-4.644 Y-4.644 I0 J-0.15", gcodeLines[4]);
            Assert.Equal("G1 X-4.6 Y-4.75", gcodeLines[5]);
            Assert.Equal("G1 X-4.644 Y-4.856", gcodeLines[6]);
            Assert.Equal("G2 X-4.75 Y-4.9 I-0.106 J0.106", gcodeLines[7]);
            Assert.Equal("G2 X-4.856 Y-4.856 I0 J0.15", gcodeLines[8]);
            Assert.Equal("G1 X-4.9 Y-4.75", gcodeLines[9]);
            Assert.Equal("OFF", gcodeLines[10]);
        }

    }
}
