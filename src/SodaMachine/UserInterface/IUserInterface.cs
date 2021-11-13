using SodaMachine.UserCommands;

namespace SodaMachine.UserInterface
{
    public interface IUserInterface
    {
        void ShowResult(CommandResult result);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
    }
}