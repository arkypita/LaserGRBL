using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LaserGRBL.DeviceProfiles;

internal static class Program
{
    private static int Main(string[] args)
    {
        var tests = new List<Action>
        {
            Cv01DefaultsAndJsonRoundTrip,
            Cv01ExportIsInvariantAndTurnsPowerOffForTravel,
            PreviewRunsAtFiveThenTurnsOffBeforePause,
            WorkAreaAndOverscanAreChecked,
            OriginAndAxisDirectionsAreApplied,
            ModalInputAndRelativeCoordinatesAreResolved,
            InchInputIsConvertedToMillimeters,
            ArcIsConvertedAndItsInteriorIsChecked,
            UntrustedCommandsAndNumbersAreRejected,
            UnsafeProfilesAreRejected,
            GrblDialectUsesMatchingOffCommands,
            PassesAndLayerOverridesAreApplied,
            Cv01StrokeMatchesKnownGoodSequence,
            CalibrationPatternsExportWithinTheBed,
            RulerHasMillimeterTicksAndLabels,
            MaterialProfilesAreScopedAndValidated,
            CardJobsPreserveUnmanagedFiles,
        };
        if (args.Length == 2 && args[0] == "--cv01-reference")
            tests.Add(() => VerifyCv01ReferenceFile(args[1]));
        else if (args.Length != 0)
        {
            Console.Error.WriteLine("Usage: ProfileRegression [--cv01-reference <path to the CV-01 ruler test G-code>]");
            return 2;
        }

        int failures = 0;
        foreach (Action test in tests)
        {
            try
            {
                test();
                Console.WriteLine("PASS " + test.Method.Name);
            }
            catch (Exception error)
            {
                failures++;
                Console.Error.WriteLine("FAIL " + test.Method.Name + ": " + error);
            }
        }

        Console.WriteLine("{0}/{1} passed", tests.Count - failures, tests.Count);
        return failures == 0 ? 0 : 1;
    }

    private static void Cv01DefaultsAndJsonRoundTrip()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        Check(profile.Width == 170 && profile.Height == 200, "CV-01 bed size");
        Check(profile.MinPower == 0 && profile.MaxPower == 255, "CV-01 power range");
        Check(profile.RapidFeed == 7200 && profile.Overscan == 4, "CV-01 travel defaults");
        Check(profile.PreviewPower == 5 && profile.Extension.TrimStart('.') == "gcode", "CV-01 file defaults");

        string directory = Path.Combine(Path.GetTempPath(), "lasergrbl-profile-" + Guid.NewGuid());
        Directory.CreateDirectory(directory);
        try
        {
            string path = Path.Combine(directory, "profile.json");
            profile.OriginX = 12.125m;
            profile.OriginY = 17.25m;
            profile.ReverseX = true;
            profile.ReverseY = true;
            profile.Overscan = 1.75m;
            profile.WorkFeed = 333.5m;
            profile.PreviewPasses = 3;
            profile.FrameByDefault = true;
            profile.PauseByDefault = true;
            profile.LineEndings = "LF";
            profile.Write(path);

            DeviceProfile restored = DeviceProfile.Read(path);
            Check(File.ReadAllText(path).TrimStart().StartsWith("{", StringComparison.Ordinal), "JSON format");
            Check(restored.Width == profile.Width && restored.Height == profile.Height, "work area round trip");
            Check(restored.OriginX == profile.OriginX && restored.OriginY == profile.OriginY, "origin round trip");
            Check(restored.ReverseX && restored.ReverseY, "axis directions round trip");
            Check(restored.Overscan == profile.Overscan && restored.WorkFeed == profile.WorkFeed, "decimal round trip");
            Check(restored.LaserOn == profile.LaserOn && restored.LaserOff == profile.LaserOff, "laser commands round trip");
            Check(restored.PreviewPasses == 3 && restored.LineEndings == "LF", "output options round trip");
            Check(restored.FrameByDefault && restored.PauseByDefault, "frame and pause settings round trip");
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    private static void Cv01ExportIsInvariantAndTurnsPowerOffForTravel()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        profile.LineEndings = "CRLF";
        ExportLayer layer = Layer("G21", "G90", "M4 S0", "G0 X12.5 Y15", "F720", "G1 X25 S250",
            "X35 S750", "G0 X40", "G1 X55 S0", "M5");

        CultureInfo originalCulture = CultureInfo.CurrentCulture;
        ExportedJob job;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("et-EE");
            job = Export(profile, layer);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }

        string text = Encoding.ASCII.GetString(job.Bytes);
        Check(job.Bytes.All(value => value < 128), "ASCII without BOM");
        Check(text.Contains("\r\n") && !text.Replace("\r\n", "").Contains('\n'), "CRLF output");
        Check(job.Bytes.SequenceEqual(Export(profile, layer).Bytes), "output independent of locale");
        string[] lines = Lines(job);
        Check(lines.Contains("G21") && lines.Contains("G90") && lines.Contains("G28 X0 Y0"), "CV-01 setup commands");
        Check(lines.Any(line => IsCommand(line, "G0") && Word(line, 'X') == 12.5m && Word(line, 'F') == 7200), "rapid feed and coordinate");
        Check(PowerLevels(lines).SequenceEqual(new decimal[] { 64, 191 }), "1000-to-255 power scaling");
        Check(job.Moves.Count != 0, "export preview moves");
        VerifyCv01Safety(lines, profile, false);
    }

    private static void PreviewRunsAtFiveThenTurnsOffBeforePause()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        profile.PreviewPasses = 2;
        profile.SynchronizeCommand = "M400";
        profile.LineEndings = "LF";
        var layer = Layer("M4 S0", "G0 X10 Y10", "G1 X20 Y20 F800 S500", "M5");
        ExportedJob job = ProfileExporter.Create(profile, new[] { layer }, 1000, true, true);
        string[] lines = Lines(job);
        int pause = Array.IndexOf(lines, "M0");

        Check(!job.Bytes.Contains((byte)'\r'), "LF output");
        Check(pause > 0, "explicit pause after framing");
        Check(PowerLevels(lines.Take(pause)).SequenceEqual(new decimal[] { 5, 5 }), "each frame pass uses S5");
        Check(lines.Take(pause).Count(line => IsCommand(line, "G1")) == 8, "two frame laps");
        Check(PowerLevels(lines.Skip(pause)).SequenceEqual(new decimal[] { 128 }), "work after pause");
        Check(lines.Contains("M400"), "configured synchronization");
        VerifyCv01Safety(lines, profile, true);

        Check(!Lines(ProfileExporter.Create(profile, new[] { layer }, 1000, true, false)).Contains("M0"), "pause can be disabled");
        Check(!PowerLevels(Lines(Export(profile, layer))).Contains(5), "frame can be disabled");
    }

    private static void WorkAreaAndOverscanAreChecked()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        Reject(() => Export(profile, Layer("G0 X170.01 Y2", "M4 S1000", "G1 X168 Y2")));
        Reject(() => Export(profile, Layer("G0 X10 Y200.01", "M4 S1000", "G1 X15 Y190")));
        Reject(() => Export(profile, Layer("G0 X-0.1 Y2", "M4 S1000", "G1 X3 Y2")));
        VerifyCv01Safety(Lines(Export(profile, Layer("G0 X170 Y200", "M4 S1000", "G1 X168 Y199"))), profile, false);

        var layer = Layer("M4 S0", "G0 X10 Y25", "G1 X20 F900 S1000", "M5");
        layer.ApplyOverscan = true;
        string[] lines = Lines(Export(profile, layer));
        Check(lines.Any(line => Word(line, 'X') == 6 && Word(line, 'Y') == 25), "4 mm lead-in");
        Check(lines.Any(line => Word(line, 'X') == 24 && Word(line, 'Y') == 25), "4 mm lead-out");
        VerifyCv01Safety(lines, profile, false);

        var edge = Layer("M4 S0", "G0 X2 Y25", "G1 X20 F900 S1000", "M5");
        edge.ApplyOverscan = true;
        Reject(() => Export(profile, edge));
    }

    private static void OriginAndAxisDirectionsAreApplied()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        profile.OriginX = 100;
        profile.OriginY = 100;
        profile.ReverseX = true;
        profile.ReverseY = true;
        string[] lines = Lines(Export(profile, Layer("M4 S0", "G0 X10 Y20", "G1 X25 Y30 F800 S1000", "M5")));

        Check(lines.Any(line => IsCommand(line, "G0") && Word(line, 'X') == 90 && Word(line, 'Y') == 80), "reversed rapid coordinates");
        Check(lines.Any(line => IsCommand(line, "G1") && Word(line, 'X') == 75 && Word(line, 'Y') == 70), "reversed working coordinates");
        VerifyCv01Safety(lines, profile, false);
    }

    private static void ModalInputAndRelativeCoordinatesAreResolved()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        string[] lines = Lines(Export(profile, Layer("G90", "G0X20Y20", "M4S500", "G91", "G1X5F600", "Y-2.5",
            "S750", "X2.25", "M5", "G90", "G0 X3 Y7")));

        Check(lines.Any(line => IsCommand(line, "G1") && Word(line, 'X') == 25 && Word(line, 'Y') == 17.5m), "modal relative Y");
        Check(lines.Any(line => IsCommand(line, "G1") && Word(line, 'X') == 27.25m && Word(line, 'Y') == 17.5m), "modal relative X");
        Check(lines.Any(line => IsCommand(line, "G0") && Word(line, 'X') == 3 && Word(line, 'Y') == 7), "absolute reset");
        Check(PowerLevels(lines).SequenceEqual(new decimal[] { 128, 191 }), "modal power changes");
        VerifyCv01Safety(lines, profile, false);
    }

    private static void ArcIsConvertedAndItsInteriorIsChecked()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        var arc = Layer("G90", "G0 X15 Y20", "M4 S500", "G3 X25 Y20 I5 J0 F800", "M5");
        string[] lines = Lines(Export(profile, arc));
        Check(!lines.Any(line => IsCommand(line, "G2") || IsCommand(line, "G3")), "CV-01 uses G0 and G1 only");
        Check(lines.Count(line => IsCommand(line, "G1")) > 2, "arc was subdivided");
        Check(lines.Any(line => IsCommand(line, "G1") && Word(line, 'X') == 25 && Word(line, 'Y') == 20), "arc endpoint preserved");
        VerifyCv01Safety(lines, profile, false);

        var outside = Layer("G90", "G0 X5 Y1", "M4 S500", "G2 X1 Y5 I-4 J0 F800", "M5");
        Reject(() => Export(profile, outside));
    }

    private static void InchInputIsConvertedToMillimeters()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        string[] lines = Lines(Export(profile, Layer("G20 G90", "G0 X1 Y0.5", "M4 S500", "G1 X2 F10", "M5")));
        Check(!lines.Contains("G20"), "output is millimeters");
        Check(lines.Any(line => IsMove(line, "G0", 25.4m, 12.7m)), "inch coordinate conversion");
        Check(lines.Any(line => IsMove(line, "G1", 50.8m, 12.7m) && Word(line, 'F') == 254), "inch feed conversion");
        VerifyCv01Safety(lines, profile, false);
    }

    private static void UntrustedCommandsAndNumbersAreRejected()
    {
        foreach (string command in new[] { "M42 P0 S255", "M106 S255", "G92 X0 Y0", "G1 X10.2.3", "G1 Z-1", "S1001", "F-1" })
        {
            Reject(() => SourceProgram.Parse(new[] { "G0 X10 Y10", command, "M4 S500", "G1 X20" }, 1000, 800));
        }
    }

    private static void UnsafeProfilesAreRejected()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        profile.StartCommands = "G90|M106 S255";
        Reject(profile.Validate);

        profile = DeviceProfile.CreateCv01();
        profile.LaserOn = "M106 P1 S{power}";
        Reject(profile.Validate);

        profile = DeviceProfile.CreateCv01();
        profile.MaxPower = 256;
        Reject(profile.Validate);

        profile = DeviceProfile.CreateCv01();
        profile.Extension = "../something.gcode";
        Reject(profile.Validate);
    }

    private static void GrblDialectUsesMatchingOffCommands()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        profile.StartCommands = "G90|G21";
        profile.EndCommands = "";
        profile.LaserOn = "M4 S{power}";
        profile.LaserOff = "M5";
        profile.PauseCommand = "";
        profile.MaxPower = 1000;
        profile.Extension = ".nc";
        string[] lines = Lines(Export(profile, Layer("M4 S0", "G0 X5 Y5", "G1 X15 F800 S750", "G0 X20", "M5")));

        Check(lines.First() == "M5" && lines.Last() == "M5", "GRBL starts and ends off");
        Check(!lines.Any(line => IsCommand(line, "M106") || IsCommand(line, "M107")), "GRBL profile uses GRBL power commands");
        Check(lines.Contains("M4 S750"), "GRBL power preserved");
        int rapid = Array.FindIndex(lines, line => IsMove(line, "G0", 20, 5));
        Check(rapid > 0 && lines[rapid - 1] == "M5", "GRBL power disabled before travel");
    }

    private static void PassesAndLayerOverridesAreApplied()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        var first = Layer("M4 S0", "G0 X5 Y5", "G1 X15 F800 S500", "M5");
        first.Passes = 2;
        first.OperationPower = 25;
        first.WorkFeed = 650;
        var second = Layer("M4 S0", "G0 X20 Y20", "G1 X30 F850 S1000", "M5");
        second.OperationPower = 200;
        second.WorkFeed = 350;
        string[] lines = Lines(ProfileExporter.Create(profile, new[] { first, second }, 1000, false, false));

        string[] work = lines.Where(line => IsCommand(line, "G1")).ToArray();
        Check(work.Select(line => Word(line, 'X')).SequenceEqual(new decimal?[] { 15, 15, 30 }), "passes and layer order");
        Check(work.Select(line => Word(line, 'F')).SequenceEqual(new decimal?[] { 650, 650, 350 }), "layer feed override");
        Check(PowerLevels(lines).Last() == 200 && PowerLevels(lines).First() > 0 && PowerLevels(lines).First() <= 25, "layer power override");
        VerifyCv01Safety(lines, profile, false);
    }

    private static void CalibrationPatternsExportWithinTheBed()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        foreach (string pattern in new[] { JobPatterns.Matrix, JobPatterns.LineWidths, JobPatterns.Squares, JobPatterns.Ruler })
        {
            var layer = new ExportLayer
            {
                Lines = JobPatterns.Generate(pattern, profile.Width, profile.Height, 1000, 800),
                Passes = 1,
            };
            string[] lines = Lines(Export(profile, layer));
            Check(lines.Any(line => IsCommand(line, "G1")), pattern + " produced working moves");
            VerifyCv01Safety(lines, profile, false);
        }
    }

    private static void Cv01StrokeMatchesKnownGoodSequence()
    {
        DeviceProfile profile = DeviceProfile.CreateCv01();
        var stroke = Layer("M4 S0", "G0 X14 Y135", "G1 X22 F1800 S100", "M5");
        stroke.ApplyOverscan = true;
        string[] lines = Lines(Export(profile, stroke));
        Check(HasCv01ReferenceStroke(lines), "known-good overscan and power sequence");
        Check(!lines.Contains("M400"), "default CV-01 uses the known-good sequence without forcing a planner stop");
        Check(lines.First() == "M107" && lines.Last() == "G28 X0 Y0", "known-good shutdown and homing");
        VerifyCv01Safety(lines, profile, false);
    }

    private static void VerifyCv01ReferenceFile(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        Check(bytes.All(value => value < 128), "reference file uses ASCII");
        string text = Encoding.ASCII.GetString(bytes);
        Check(text.Contains("\r\n") && !text.Replace("\r\n", "").Contains('\n'), "reference file uses CRLF");
        string[] lines = text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim()).ToArray();
        Check(lines.Take(7).Contains("G28 X0 Y0") && lines.First() == "M107", "reference starts with laser off and homes");
        Check(lines.TakeWhile(line => line != "M0").Count(line => line == "M106 S5") == 2, "reference has two S5 frame passes before the pause");
        Check(lines.Count(line => line == "M0") == 1, "reference pause");
        Check(lines.Reverse().Skip(1).Take(2).Contains("M107") && IsCommand(lines.Last(), "G28"), "reference ends with laser off and homes");
        Check(!lines.Contains("M400") && HasCv01ReferenceStroke(lines), "reference stroke timing command order");
    }

    private static bool HasCv01ReferenceStroke(string[] lines)
    {
        for (int index = 1; index + 3 < lines.Length; index++)
        {
            if (IsMove(lines[index - 1], "G1", 14, 135) && lines[index] == "M106 S26" &&
                IsMove(lines[index + 1], "G1", 22, 135) && lines[index + 2] == "M107" &&
                IsMove(lines[index + 3], "G1", 26, 135))
            {
                string leadIn = lines.Take(index - 1).Reverse().FirstOrDefault(line => IsCommand(line, "G0"));
                if (leadIn != null && IsMove(leadIn, "G0", 10, 135))
                    return true;
            }
        }

        return false;
    }

    private static void RulerHasMillimeterTicksAndLabels()
    {
        string[] moves = JobPatterns.Generate(JobPatterns.Ruler, 170, 200, 1000, 800)
            .Where(line => IsCommand(line, "G0") || IsCommand(line, "G1")).ToArray();
        Check(moves.Zip(moves.Skip(1)).Any(pair => IsMove(pair.First, "G0", 2, 2) && IsMove(pair.Second, "G1", 102, 2)), "100 mm baseline");

        for (int millimeter = 0; millimeter <= 100; millimeter++)
        {
            int x = millimeter + 2;
            int tick = millimeter % 10 == 0 ? 3 : millimeter % 5 == 0 ? 2 : 1;
            Check(moves.Zip(moves.Skip(1)).Any(pair => IsMove(pair.First, "G0", x, 2) && IsMove(pair.Second, "G1", x, 2 + tick)),
                "missing tick at " + millimeter + " mm");
        }

        for (int millimeter = 0; millimeter <= 100; millimeter += 10)
        {
            int x = millimeter + 2;
            Check(moves.Any(line => IsCommand(line, "G1") && Math.Abs(Word(line, 'X').GetValueOrDefault() - x) <= 3 && Word(line, 'Y') > 5),
                "missing numeric strokes at " + millimeter + " mm");
        }
    }

    private static void MaterialProfilesAreScopedAndValidated()
    {
        MaterialRegression.Run();
    }

    private static void CardJobsPreserveUnmanagedFiles()
    {
        CardRegression.Run();
    }

    private static ExportLayer Layer(params string[] lines)
    {
        return new ExportLayer { Lines = lines, Passes = 1 };
    }

    private static ExportedJob Export(DeviceProfile profile, ExportLayer layer)
    {
        return ProfileExporter.Create(profile, new[] { layer }, 1000, false, false);
    }

    private static string[] Lines(ExportedJob job)
    {
        return Encoding.ASCII.GetString(job.Bytes).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
    }

    private static IEnumerable<decimal> PowerLevels(IEnumerable<string> lines)
    {
        return lines.Where(line => IsCommand(line, "M106")).Select(line => Word(line, 'S').Value).Where(value => value != 0);
    }

    private static bool IsCommand(string line, string command)
    {
        return line.Equals(command, StringComparison.Ordinal) || line.StartsWith(command + " ", StringComparison.Ordinal);
    }

    private static bool IsMove(string line, string command, decimal x, decimal y)
    {
        return IsCommand(line, command) && Word(line, 'X') == x && Word(line, 'Y') == y;
    }

    private static decimal? Word(string line, char letter)
    {
        Match word = Regex.Match(line, "(?:^|\\s)" + letter + @"(-?\d+(?:\.\d+)?)");
        return word.Success ? decimal.Parse(word.Groups[1].Value, CultureInfo.InvariantCulture) : null;
    }

    private static void VerifyCv01Safety(string[] lines, DeviceProfile profile, bool expectPause)
    {
        bool? beamOn = null;
        foreach (string line in lines)
        {
            if (IsCommand(line, "M106"))
            {
                decimal? power = Word(line, 'S');
                Check(power >= profile.MinPower && power <= profile.MaxPower, "unbounded laser power: " + line);
                beamOn = power > 0;
            }

            if (IsCommand(line, "M107"))
                beamOn = false;

            if (IsCommand(line, "G0") || IsCommand(line, "G28") || IsCommand(line, "M0"))
                Check(beamOn == false, "beam active or unknown before travel, homing or pause: " + line);

            if (IsCommand(line, "G0") || IsCommand(line, "G1"))
            {
                decimal? x = Word(line, 'X');
                decimal? y = Word(line, 'Y');
                Check(x >= 0 && x <= profile.Width && y >= 0 && y <= profile.Height, "out-of-bounds move: " + line);
            }
        }

        Check(beamOn == false, "beam off at end");
        Check(lines.Contains("M0") == expectPause, "pause state");
    }

    private static void Reject(Action action)
    {
        try
        {
            action();
        }
        catch (Exception error) when (error is InvalidDataException || error is ArgumentException || error is FormatException)
        {
            return;
        }

        throw new InvalidOperationException("Expected an input validation error.");
    }

    private static void Check(bool condition, string failure)
    {
        if (!condition)
            throw new InvalidOperationException(failure);
    }
}
