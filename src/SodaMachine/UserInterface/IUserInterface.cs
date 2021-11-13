using SodaMachine.Core.UserCommands;

namespace SodaMachine.Core.UserInterface
{
    public interface IUserInterface
    {
        void ShowResult(CommandResult result);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
    }
}