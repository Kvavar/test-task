using System;
using SodaMachine.Core.UserCommands;

namespace SodaMachine.Core.CommandParser
{
    public class CommandParser
    {
        public static ParseResult Parse(string input)
        {
            if(string.IsNullOrWhiteSpace(input))
                return ParseResult.Fail("Input cannot be empty.");

            var args = input.Split(' ');

            if (input.StartsWith("stop", StringComparison.InvariantCultureIgnoreCase))
            {
                return ParseResult.Success(new Stop());
            }

            if (input.StartsWith("recall", StringComparison.InvariantCultureIgnoreCase))
            {
                return ParseResult.Success(new Recall());
            }

            var minArgs = 2;
            if (args.Length < minArgs)
            {
                return ParseResult.Fail($"Unable to parse command, was {input}.");
            }

            var commandArgument = args[minArgs - 1];

            if (input.StartsWith("insert", StringComparison.InvariantCultureIgnoreCase))
            {
                if (decimal.TryParse(commandArgument, out var amount))
                {
                    if (amount <= 0)
                        ParseResult.Fail($"Amount must be greater then zero, was {amount}.");

                    return ParseResult.Success(new Insert(amount));
                }

                ParseResult.Fail($"Unable to parse insert command, was {input}.");
            }

            if (input.StartsWith("order", StringComparison.InvariantCultureIgnoreCase))
            {
                return ParseResult.Success(new OrderByCash(commandArgument));
            }

            minArgs = 3;
            if (args.Length < minArgs)
            {
                return ParseResult.Fail($"Unable to parse command, was {input}.");
            }

            commandArgument = args[minArgs - 1];

            if (input.StartsWith("sms order", StringComparison.InvariantCultureIgnoreCase))
            {
                return ParseResult.Success(new OrderBySms(commandArgument));
            }

            return ParseResult.Fail($"Unable to parse command, was {input}");
        }
    }
}