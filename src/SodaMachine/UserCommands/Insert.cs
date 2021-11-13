namespace SodaMachine.Core.UserCommands
{
    public sealed class Insert : UserCommand
    {
        public Insert(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; }
    }
}