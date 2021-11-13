namespace SodaMachine.UserCommands
{
    public class OrderBySms : UserCommand
    {
        public OrderBySms(string order)
        {
            Order = order.ToLowerInvariant();
        }

        public string Order { get; }
    }
}