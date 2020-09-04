using System;

namespace LaserGRBL.CmdLine.Commands
{
    public class UnknowCommand : ICommand
    {
        public string[] AttendTo => new string[0];
        public void Initialize(CommandProcessor processor) { }
        public string HelpText() => null;
        public bool Process(string command, string[] args)
        {
            Console.WriteLine("Type --help for instructions");
            return true;
        }
    }
}
