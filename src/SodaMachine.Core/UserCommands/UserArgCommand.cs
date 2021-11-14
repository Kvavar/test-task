namespace SodaMachine.Core.UserCommands
{
    public class UserArgCommand<TArg> : UserCommand
    {
        public UserArgCommand(CommandType type, TArg arg) : base(type)
        {
            Arg = arg;
        }

        public TArg Arg { get; }
    }
}