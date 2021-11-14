namespace SodaMachine.Core.UserCommands
{
    public class ParseResult
    {
        private ParseResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        private ParseResult(bool isSuccess, UserCommand command) : this(isSuccess)
        {
            Command = command;
        }

        private ParseResult(bool isSuccess, string message) : this(isSuccess)
        {
            Message = message;
        }

        public bool IsSuccess { get; }
        public UserCommand Command { get; }
        public string Message { get; }

        public static ParseResult Success(UserCommand command)
        {
            return new ParseResult(true, command);
        }

        public static ParseResult Fail(string message)
        {
            return new ParseResult(false, message);
        }
    }
}