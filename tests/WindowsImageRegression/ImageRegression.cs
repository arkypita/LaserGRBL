using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using LaserGRBL;
using LaserGRBL.RasterConverter;
using LaserGRBL.SvgConverter;

public static class ImageRegression
{
    private const BindingFlags InternalInstance = BindingFlags.Instance | BindingFlags.NonPublic;
    private const BindingFlags InternalStatic = BindingFlags.Static | BindingFlags.NonPublic;
    private const int SourceMax = 1000;

    public static void Run(string directory)
    {
        typeof(GrblCore).GetField("mDataPath", InternalStatic).SetValue(null, directory);
        string png = Path.Combine(directory, "gradient.png");
        string jpg = Path.Combine(directory, "gradient.jpg");
        string svg = Path.Combine(directory, "contour.svg");
        CreateRasterFixtures(png, jpg);
        File.WriteAllText(svg, "<svg xmlns='http://www.w3.org/2000/svg' width='20mm' height='20mm' viewBox='0 0 20 20'>" +
            "<path d='M 3 3 L 16 3 Q 19 10 16 17 L 3 17 Z' fill='none' stroke='black'/></svg>", Encoding.ASCII);

        TestRasterImport(png, "PNG grayscale", bitmap => (Bitmap)bitmap.Clone(), true);
        TestRasterImport(jpg, "JPG black and white", bitmap => ImageTransform.Threshold(bitmap, 0.5f, true), false);
        TestRasterImport(png, "Floyd-Steinberg", bitmap => ImageTransform.DitherImage(bitmap, ImageTransform.DitheringMode.FloydSteinberg), false);
        TestRasterImport(png, "Atkinson", bitmap => ImageTransform.DitherImage(bitmap, ImageTransform.DitheringMode.Atkinson), false);
        TestRasterImport(png, "Jarvis", bitmap => ImageTransform.DitherImage(bitmap, ImageTransform.DitheringMode.Jarvis), false);
        TestRasterImport(png, "Blue noise", bitmap => ImageTransform.DitherImage(bitmap, ImageTransform.DitheringMode.BlueNoise), false);
        TestRasterVectorContour(png);
        TestSvgContour(svg);
    }

    private static void CreateRasterFixtures(string png, string jpg)
    {
        using (var bitmap = new Bitmap(32, 16, PixelFormat.Format32bppArgb))
        {
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int shade = y < 2 || y >= bitmap.Height - 2 || x < 2 || x >= bitmap.Width - 2
                        ? 255 : x < 9 ? 0 : x < 16 ? 80 : x < 23 ? 160 : 225;
                    bitmap.SetPixel(x, y, Color.FromArgb(shade, shade, shade));
                }
            }
            bitmap.Save(png, ImageFormat.Png);
            bitmap.Save(jpg, ImageFormat.Jpeg);
        }
    }

    private static void TestRasterImport(string path, string description, Func<Bitmap, Bitmap> transform, bool variablePower)
    {
        var processor = new ImageProcessor(null, path, new Size(32, 16), false);
        try
        {
            using (var input = processor.Original.Clone(new Rectangle(0, 0, processor.Original.Width, processor.Original.Height), PixelFormat.Format32bppArgb))
            using (Bitmap processed = transform(input))
            {
                var source = new GrblFile();
                source.LoadImageL2L(processed, path, RasterConfig(), false, null);
                VerifyCv01Export(source, description, variablePower);
            }
        }
        finally
        {
            processor.Dispose();
        }
    }

    private static GrblFile.L2LConf RasterConfig()
    {
        return new GrblFile.L2LConf
        {
            res = 2, fres = 2, oX = 10, oY = 10, markSpeed = 800, borderSpeed = 600,
            minPower = 0, maxPower = SourceMax, lOn = "M4", lOff = "M5", pwm = true,
            dir = ImageProcessor.Direction.Horizontal, firmwareType = Firmware.Grbl,
        };
    }

    private static void TestRasterVectorContour(string png)
    {
        using (var bitmap = new Bitmap(png))
        using (Bitmap blackWhite = ImageTransform.Threshold(bitmap, 0.5f, true))
        {
            var source = new GrblFile();
            GrblFile.L2LConf config = RasterConfig();
            config.dir = ImageProcessor.Direction.None;
            source.LoadImagePotrace(blackWhite, png, false, 2, false, 0m, false, 0.2m, false, config, false, null);
            VerifyCv01Export(source, "PNG vector contour", false);
        }
    }

    private static void TestSvgContour(string svg)
    {
        var source = new GrblFile();
        using (var complete = new ManualResetEvent(false))
        {
            source.OnFileLoaded += delegate { complete.Set(); };
            source.LoadImportedSVG(svg, false, null, ColorFilter.All);
            if (!complete.WaitOne(TimeSpan.FromSeconds(10)))
                throw new InvalidDataException("SVG conversion did not finish.");
            VerifyCv01Export(source, "SVG vector contour", false);
        }
    }

    private static void VerifyCv01Export(GrblFile source, string description, bool variablePower)
    {
        Assembly assembly = typeof(GrblFile).Assembly;
        Type profileType = assembly.GetType("LaserGRBL.DeviceProfiles.DeviceProfile", true);
        Type layerType = assembly.GetType("LaserGRBL.DeviceProfiles.ExportLayer", true);
        Type exporterType = assembly.GetType("LaserGRBL.DeviceProfiles.ProfileExporter", true);
        object profile = profileType.GetMethod("CreateCv01", InternalStatic).Invoke(null, null);
        object layer = Activator.CreateInstance(layerType, true);
        layerType.GetField("Lines", InternalInstance).SetValue(layer, source.Commands.Select(command => command.Command).ToArray());
        Array layers = Array.CreateInstance(layerType, 1);
        layers.SetValue(layer, 0);
        object job = exporterType.GetMethod("Create", InternalStatic).Invoke(null, new object[] { profile, layers, SourceMax, false, false });
        byte[] bytes = (byte[])job.GetType().GetField("Bytes", InternalInstance).GetValue(job);
        string[] lines = Encoding.ASCII.GetString(bytes).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[] powers = lines.Where(line => Regex.IsMatch(line, @"^M106 S[1-9]\d*$")).Distinct().ToArray();
        bool expectedPower = variablePower ? powers.Length >= 2 : powers.Length == 1;
        if (!expectedPower || !lines.Any(line => line.StartsWith("G1 X", StringComparison.Ordinal)) ||
            lines.First() != "M107" || lines.Last() != "G28 X0 Y0")
            throw new InvalidDataException(description + " failed: missing movement, power levels or laser-off sequence.");

        Console.WriteLine("PASS " + description + " (" + powers.Length + " positive power levels)");
    }
}
