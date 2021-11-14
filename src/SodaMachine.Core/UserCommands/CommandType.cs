namespace SodaMachine.Core.UserCommands
{
    public enum CommandType
    {
        None = 0,
        Insert = 1,
        OrderByCash,
        OrderBySms,
        Recall,
        Stop
    }
}