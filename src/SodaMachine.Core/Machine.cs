using System;
using System.Collections.Generic;
using SodaMachine.Core.Resources;
using SodaMachine.Core.UserCommands;
using SodaMachine.Core.UserInterface;

namespace SodaMachine.Core
{
    public class Machine 
    {
        private readonly IUserInterface _ui;
        private readonly object _purchaseLocker = new object();
        private decimal _balance;
        private readonly Inventory _inventory;

        public Machine(Dictionary<string, int> inventory, Dictionary<string, decimal> prices, IUserInterface ui)
        {
            _ui = ui;
            _inventory = new Inventory(inventory, prices);
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
                var message = string.Format(Messages.ReturnBalanceToCustomer, _balance);

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

                var parseResult = CommandParser.CommandParser.Parse(input);
                if (!parseResult.IsSuccess)
                {
                    _ui.Error(parseResult.Message);
                    continue;
                }

                CommandResult result;

                if (parseResult.Command is Insert insert)
                {
                    result = InsertMoney(insert.Amount);
                    _ui.ShowResult(result);
                }
                else if (parseResult.Command is OrderByCash orderByCash)
                {
                    result = PurchaseByCash(orderByCash.Order);
                }
                else if (parseResult.Command is OrderBySms orderBySms)
                {
                    result = PurchaseBySms(orderBySms.Order);
                }
                else if (parseResult.Command is Recall)
                {
                    result = Recall();
                }
                else if (parseResult.Command is Stop)
                {
                    _ui.Info(Messages.ShuttingDown);
                    break;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(string.Format(Messages.CommandIsNotSupported, parseResult.Command.GetType()));
                }

                _ui.ShowResult(result);
            }
        }
    }
}