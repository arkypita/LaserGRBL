using System;

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
                if (cmd.AttendTo != null && cmd.AttendTo.Length > 0) cmdText = args[0];

                if (string.IsNullOrEmpty(cmdText)) continue;

                string alises = "";
                if (cmd.AttendTo != null)
                {
                    alises = string.Join(", ", cmd.AttendTo);
                }

                Console.WriteLine($"Command: {cmdText}");
                Console.WriteLine($" Alisases: {alises}");
                string help = cmd.HelpText();
                if (!string.IsNullOrEmpty(help))
                {
                    Console.WriteLine($" Command Text:");
                    Console.WriteLine(cmd.HelpText());
                }
                Console.WriteLine();

            }
            return true;
        }
    }
}
