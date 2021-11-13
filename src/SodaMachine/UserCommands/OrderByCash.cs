namespace SodaMachine.UserCommands
{
    public class OrderByCash : UserCommand
    {
        public OrderByCash(string order)
        {
            Order = order.ToLowerInvariant();
        }

        public string Order { get; }
    }
}