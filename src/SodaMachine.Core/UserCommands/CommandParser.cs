using System;
using SodaMachine.Core.Resources;

namespace SodaMachine.Core.UserCommands
{
    public class CommandParser
    {
        public static ParseResult Parse(string input)
        {
            if(string.IsNullOrWhiteSpace(input))
            {
                return ParseResult.Fail(Messages.InputCannotBeEmpty);
            }

            var args = input.Split(' ');

            if (input.StartsWith("stop", StringComparison.InvariantCultureIgnoreCase))
            {
                return ParseResult.Success(new UserCommand(CommandType.Stop));
            }

            if (input.StartsWith("recall", StringComparison.InvariantCultureIgnoreCase))
            {
                return ParseResult.Success(new UserCommand(CommandType.Recall));
            }

            var minArgs = 2;
            if (args.Length < minArgs)
            {
                return ParseResult.Fail(string.Format(Messages.UnableToParseCommand, input));
            }

            var commandArgument = args[minArgs - 1];

            if (input.StartsWith("insert", StringComparison.InvariantCultureIgnoreCase))
            {
                if (decimal.TryParse(commandArgument, out var amount))
                {
                    if (amount <= 0)
                    {
                        return ParseResult.Fail(string.Format(Messages.AmountMustBeGreaterThanZero, amount));
                    }

                    return ParseResult.Success(new UserArgCommand<decimal>(CommandType.Insert, amount));
                }

                return ParseResult.Fail(string.Format(Messages.UnableToParseCommand, input));
            }

            if (input.StartsWith("order", StringComparison.InvariantCultureIgnoreCase))
            {
                return ParseResult.Success(new UserArgCommand<string>(CommandType.OrderByCash, commandArgument.ToLowerInvariant()));
            }

            minArgs = 3;
            if (args.Length < minArgs)
            {
                return ParseResult.Fail(string.Format(Messages.UnableToParseCommand, input));
            }

            commandArgument = args[minArgs - 1];

            if (input.StartsWith("sms order", StringComparison.InvariantCultureIgnoreCase))
            {
                return ParseResult.Success(new UserArgCommand<string>(CommandType.OrderBySms, commandArgument.ToLowerInvariant()));
            }

            return ParseResult.Fail(string.Format(Messages.UnableToParseCommand, input));
        }

        public static T ExtractArgumentFrom<T>(UserCommand command)
        {
            if (command is UserArgCommand<T> argCommand)
            {
                return argCommand.Arg;
            }

            throw new InvalidOperationException(string.Format(Messages.UnableToExtractArgument, command.Type));
        }
    }
}