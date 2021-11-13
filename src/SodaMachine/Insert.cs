namespace SodaMachine
{
    public class Insert : Command
    {
        public Insert(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; }
    }
}