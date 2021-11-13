namespace SodaMachine.UserCommands
{
    public class Insert : UserCommand
    {
        public Insert(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; }
    }
}