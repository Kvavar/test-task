namespace SodaMachine.Core.UserCommands
{
    public sealed class OrderByCash : UserCommand
    {
        public OrderByCash(string order)
        {
            Order = order.ToLowerInvariant();
        }

        public string Order { get; }
    }
}