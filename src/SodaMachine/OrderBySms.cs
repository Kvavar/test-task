namespace SodaMachine
{
    public class OrderBySms : Command
    {
        public OrderBySms(string order)
        {
            Order = order.ToLowerInvariant();
        }

        public string Order { get; }
    }
}