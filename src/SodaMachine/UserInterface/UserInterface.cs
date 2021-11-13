using System;
using SodaMachine.UserCommands;

namespace SodaMachine.UserInterface
{
    public class UserInterface : IUserInterface
    {
        public void ShowResult(CommandResult result)
        {
            if (result.IsSuccess)
            {
                Info(result.Message);
            }
            else
            {
                Error(result.Message);
            }
        }

        public void Info(string message)
        {
            message = $"{DateTime.UtcNow:G}. Info: {message}";

            LogMessageWithColor(message, ConsoleColor.DarkGreen);
        }

        public void Warn(string message)
        {
            message = $"{DateTime.UtcNow:G}. Warning: {message}";

            LogMessageWithColor(message, ConsoleColor.DarkYellow);
        }

        public void Error(string message)
        {
            message = $"{DateTime.UtcNow:G}. Error: {message}";

            LogMessageWithColor(message, ConsoleColor.DarkRed);
        }

        private void LogMessageWithColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            Console.WriteLine(message);

            Console.ResetColor();
        }
    }
}