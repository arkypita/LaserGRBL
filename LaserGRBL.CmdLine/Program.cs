using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL.CmdLine
{
    class Program
    {
        static void Main(string[] args)
        {
            var processor = new CommandProcessor();
            if (args.Length == 0)
            {
                processor.ProcessCommand("", args);
            }
            else
            {
                processor.ProcessCommand(args[0], args);
            }

            Console.ReadKey();
        }
    }
}
