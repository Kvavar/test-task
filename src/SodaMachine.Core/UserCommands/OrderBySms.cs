namespace SodaMachine.Core.UserCommands
{
    public sealed class OrderBySms : UserCommand
    {
        public OrderBySms(string order)
        {
            Order = order.ToLowerInvariant();
        }

        public string Order { get; }
    }
}