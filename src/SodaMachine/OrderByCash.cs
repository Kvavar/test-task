namespace SodaMachine
{
    public class OrderByCash : Command
    {
        public OrderByCash(string order)
        {
            Order = order.ToLowerInvariant();
        }

        public string Order { get; }
    }
}