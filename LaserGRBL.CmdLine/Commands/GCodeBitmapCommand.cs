using LaserGRBL.RasterConverter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace LaserGRBL.CmdLine.Commands
{
    public class GCodeBitmapCommand : ICommand
    {
        public string[] AttendTo => new string[] {"-bmp2g"};

        public string HelpText()
        {
            return "Convert SVG to GCode\nUsage: -bmp2g InputFile OutputFile";
        }

        public void Initialize(CommandProcessor processor) { }

        public bool Process(string command, string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid arguments, see --help");
                return false;
            }

            var fI = new FileInfo(args[1]);
            var fO = new FileInfo(args[2]);

            if (!fI.Exists)
            {
                Console.WriteLine("Input file not found");
                return false;
            }

            var file = new GrblFile(0, 0, 200, 300);

            ImageProcessor IP = new ImageProcessor(file, fI.FullName, Size.Empty, false);
            IP.WhiteClip = 5;
            IP.Brightness = 100;
            IP.Contrast = 100;
            IP.Quality = 3;
            IP.FillingQuality = 3;
            IP.BorderSpeed = 1000;
            IP.LaserOff = "M5";
            IP.LaserOn = "M3";
            IP.MarkSpeed = 1000;
            IP.MaxPower = 1000;
            IP.GenerateGCodeSync();
            file.SaveProgram(fO.FullName, true, true, false, 1);
            
            Console.WriteLine("Conversion finished");

            return true;
        }
    }
}
