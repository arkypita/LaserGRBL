namespace LaserGRBL.CmdLine
{
    public interface ICommand
    {
        string[] AttendTo { get; }
        void Initialize(CommandProcessor processor);
        bool Process(string command, string[] args);
        string HelpText();
    }
}