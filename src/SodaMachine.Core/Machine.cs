using System;
using System.Collections.Generic;
using SodaMachine.Core.Resources;
using SodaMachine.Core.UserCommands;
using SodaMachine.Core.UserInterface;
using SodaMachine.Core.Validation;

namespace SodaMachine.Core
{
    public class Machine 
    {
        private readonly IUserInterface _ui;
        private readonly object _purchaseLocker = new object();
        private decimal _balance;
        private readonly Inventory _inventory;

        public Machine(Dictionary<string, int> inventory, Dictionary<string, decimal> prices, IUserInterface ui, IInventoryValidator validator)
        {
            _ui = ui;
            _inventory = new Inventory(inventory, prices, validator);
        }

        /// <summary>
        /// Allows to add money to balance, Input format - insert amount_decimal
        /// </summary>
        public CommandResult InsertMoney(decimal amount)
        {
            lock (_purchaseLocker)
            {
                _balance += amount;
            }

            return CommandResult.Success(string.Format(Messages.AddedAmountToCredit, amount));
        }

        /// <summary>
        /// Allows to buy items by cash, Input format - order item_name
        /// </summary>
        public CommandResult PurchaseByCash(string order)
        {
            lock (_purchaseLocker)
            {
                string message;

                if (!_inventory.TryGetPriceFor(order, out var price))
                {
                    message = _inventory.CheckAvailability(order) 
                        ? string.Format(Messages.OnlySmsOrderIsAvailable, order) 
                        : string.Format(Messages.NoOrderAvailable, order);

                    return CommandResult.Fail(message);
                }

                if (_balance < price)
                {
                    return CommandResult.Fail(string.Format(Messages.NeedMoreMoney, price - _balance));
                }

                if (!_inventory.TryTake(order, out  message))
                    return CommandResult.Fail(message);

                _balance -= price;

                message = string.Format(Messages.GivingOrderOut, order);

                if (_balance > 0)
                {
                    message = string.Format(Messages.GivingBalanceOutInChange, message, _balance);
                }

                _balance = 0;

                return CommandResult.Success(message);
            }
        }

        /// <summary>
        /// Allows to buy items by sms, no money on balance required, Input format - sms order item_name
        /// </summary>
        public CommandResult PurchaseBySms(string order)
        {
            lock (_purchaseLocker)
            {
                if (!_inventory.TryTake(order, out var message))
                    return  CommandResult.Fail(message);

                message = string.Format(Messages.GivingOrderOut, order);
                
                return CommandResult.Success(message);
            }
        }

        public CommandResult Recall()
        {
            lock (_purchaseLocker)
            {
                var message = _balance > 0 ? string.Format(Messages.ReturnBalanceToCustomer, _balance) : Messages.NoMoneyToReturn;

                _balance = 0;

                return CommandResult.Success(message);
            }
        }

        /// <summary>
        /// This is the starter method for the machine
        /// </summary>
        public void Start()
        {
            while (true)
            {
                _ui.Info(string.Format(Messages.WelcomeMessage, _balance));
                var input = Console.ReadLine();

                var parseResult = CommandParser.Parse(input);
                if (!parseResult.IsSuccess)
                {
                    _ui.Error(parseResult.Message);
                    continue;
                }

                CommandResult result;
                var command = parseResult.Command;

                if (command.Type == CommandType.Stop)
                {
                    _ui.Info(Messages.ShuttingDown);
                    break;
                }

                switch (command.Type)
                {
                    case CommandType.Insert:
                        var amount = CommandParser.ExtractArgumentFrom<decimal>(command);
                        result = InsertMoney(amount);
                        break;
                    case CommandType.OrderByCash:
                        var orderByCash = CommandParser.ExtractArgumentFrom<string>(command);
                        result = PurchaseByCash(orderByCash);
                        break;
                    case CommandType.OrderBySms:
                        var orderBySms = CommandParser.ExtractArgumentFrom<string>(command);
                        result = PurchaseBySms(orderBySms);
                        break;
                    case CommandType.Recall:
                        result = Recall();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(string.Format(Messages.CommandIsNotSupported, command.Type));
                }

                _ui.ShowResult(result);
            }
        }
    }
}