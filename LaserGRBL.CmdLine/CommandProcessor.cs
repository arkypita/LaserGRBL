using LaserGRBL.CmdLine.Commands;
using System.Linq;

namespace LaserGRBL.CmdLine
{
    public class CommandProcessor
    {
        UnknowCommand unknowCommand;

        ICommand[] commands;
        public CommandProcessor()
        {
            unknowCommand = new UnknowCommand();
            commands = new ICommand[]
            {
                unknowCommand,
                new HelpCommand(),
                new GCodeBitmapCommand(),
            };

            foreach (var cmd in commands) cmd.Initialize(this);
        }

        public ICommand[] GetCommands()
        {
            return commands.ToArray();
        }

        public void ProcessCommand(string command, string[] args)
        {
            var cmd = commands
                .Where(c => c.AttendTo != null)
                .FirstOrDefault(commands => commands
                                            .AttendTo
                                            .Contains(command));
            if (cmd == null)
            {
                unknowCommand.Process(command, args);
            }
            else
            {
                cmd.Process(command, args);
            }
        }
    }
}