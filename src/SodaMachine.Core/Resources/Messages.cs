namespace SodaMachine.Core.Resources
{
    public class Messages
    {
        public const string WelcomeMessage = "\n\nAvailable commands:" +
                                             "\r\ninsert (money) - Money put into money slot" +
                                             "\r\norder (coke, sprite, fanta) - Order from machines buttons" +
                                             "\r\nsms order (coke, sprite, fanta) - Order sent by sms" +
                                             "\r\nrecall - gives money back" +
                                             "\r\nstop - stops the machine" +
                                             "\r\n-------" +
                                             "\r\nInserted money: {0}" +
                                             "\r\n-------\n\n";

        public const string InputCannotBeEmpty = "Input cannot be empty.";
        public const string AmountMustBeGreaterThanZero = "Amount must be greater then zero, was {0}.";
        public const string UnableToParseCommand = "Unable to parse command, was {0}.";
        public const string PricesCannotBeEmpty = "Prices cannot be empty";
        public const string PriceMustBeGreaterThanZero = "Inventory item price must be greater than zero, was {0} : {1}.";
        public const string  InventoryCannotBeEmpty = "Inventory cannot be empty";
        public const string  ItemAmountMustBeGreaterThanZero = "Inventory item amount must be greater than zero, was {0} : {1}.";
        public const string  NoOrderAvailable = "No {0} available in the inventory.";
        public const string  AddedAmountToCredit = "Added {0} to credit.";
        public const string  OnlySmsOrderIsAvailable = "Only SMS order is available for {0}.";
        public const string  NeedMoreMoney = "Need {0} more.";
        public const string GivingOrderOut = "Giving {0} out.";
        public const string  GivingBalanceOutInChange = "{0} Giving  {1}  out in change.";
        public const string  ReturnBalanceToCustomer = "Returning {0} to customer";
        public const string  CommandIsNotSupported = "Command is not supported, was {0}";
        public const string  ShuttingDown = "Shutting down.";
        public const string UnableToExtractArgument = "Unable to extract argument from command with type {0}";
    }
}