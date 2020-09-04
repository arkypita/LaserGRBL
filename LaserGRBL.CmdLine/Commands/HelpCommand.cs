using System;
using System.Linq;

namespace LaserGRBL.CmdLine.Commands
{
    public class HelpCommand : ICommand
    {
        private CommandProcessor processor;

        public string[] AttendTo => new string[] { "--help" };
        public void Initialize(CommandProcessor processor)
        {
            this.processor = processor;
        }
        public string HelpText() => null;
        public bool Process(string command, string[] args)
        {
            Console.WriteLine(@" Usage:
> [Command] [Args]
List of Commands:");
            foreach (var cmd in processor.GetCommands())
            {
                string cmdText = "";
                if (cmd.AttendTo != null && cmd.AttendTo.Length > 0) cmdText = cmd.AttendTo[0];

                if (string.IsNullOrEmpty(cmdText)) continue;

                string alises = "";
                if (cmd.AttendTo != null)
                {
                    alises = string.Join(", ", cmd.AttendTo.Skip(1).ToArray());
                }

                Console.WriteLine($"Command: {cmdText}");
                if(!string.IsNullOrEmpty(alises)) Console.WriteLine($" Alisases: {alises}");
                string help = cmd.HelpText();
                if (!string.IsNullOrEmpty(help))
                {
                    Console.Write($" Command Text: ");
                    Console.WriteLine(cmd.HelpText());
                }
                Console.WriteLine();

            }
            return true;
        }
    }
}
