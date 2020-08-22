using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LaserGRBL.CmdLine.Commands
{
    public class GCodeSvgCommand : ICommand
    {
        public string[] AttendTo => new string[] {"-svg2g"};

        public string HelpText()
        {
            return "Convert SVG to GCode\nUsage: -svg2g InputFile OutputFile";
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
            file.LoadImportedSVG(fI.FullName, false);
            file.SaveProgram(fO.FullName, true, true, false, 1);

            Console.WriteLine("Conversion finished");

            return true;
        }
    }
}
