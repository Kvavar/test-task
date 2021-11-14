namespace SodaMachine.Core.UserCommands
{
    public class UserCommand
    {
        public UserCommand(CommandType type)
        {
            Type = type;
        }

        public CommandType Type { get; }
    }
}