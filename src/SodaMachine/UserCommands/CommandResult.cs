namespace SodaMachine.UserCommands
{
    public class CommandResult
    {
        private CommandResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public bool IsSuccess { get; }
        public string Message { get; }

        public static CommandResult Success(string message)
        {
            return new CommandResult(true, message);
        }

        public static CommandResult Fail(string message)
        {
            return new CommandResult(false, message);
        }
    }
}