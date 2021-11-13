using System;
using System.Collections.Generic;
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

        public CommandResult InsertMoney(decimal amount)
        {
            lock (_purchaseLocker)
            {
                _balance += amount;
            }

            return CommandResult.Success($"Added {amount} to credit.");
        }

        public CommandResult PurchaseByCash(string order)
        {
            lock (_purchaseLocker)
            {
                string message;

                if (!_inventory.TryGetPriceFor(order, out var price))
                {
                    message = _inventory.CheckAvailability(order) ? $"Only SMS order is available for {order}." : $"No {order} available in the inventory.";

                    return CommandResult.Fail(message);
                }

                if (_balance < price)
                {
                    return CommandResult.Fail($"Need {price - _balance} more.");
                }

                if (!_inventory.TryTake(order, out  message))
                    return CommandResult.Fail(message);

                _balance -= price;

                message = $"Giving {order} out.";

                if (_balance > 0)
                    message = $"{message} Giving  {_balance}  out in change.";

                _balance = 0;

                return CommandResult.Success(message);
            }
        }

        public CommandResult PurchaseBySms(string order)
        {
            lock (_purchaseLocker)
            {
                if (!_inventory.TryTake(order, out var message))
                    return  CommandResult.Fail(message);

                message = $"Giving {order} out.";

                return CommandResult.Success(message);
            }
        }

        public CommandResult Recall()
        {
            lock (_purchaseLocker)
            {
                var message = $"Returning {_balance} to customer";

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
                var welcomeMessage = "\n\nAvailable commands:" +
                                     "\r\ninsert (money) - Money put into money slot" +
                                     "\r\norder (coke, sprite, fanta) - Order from machines buttons" +
                                     "\r\nsms order (coke, sprite, fanta) - Order sent by sms" +
                                     "\r\nrecall - gives money back" +
                                     "\r\nstop - stops the machine" +
                                     "\r\n-------" +
                                     $"\r\nInserted money: {_balance}" +
                                     "\r\n-------\n\n";

                _ui.Info(welcomeMessage);

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
                    _ui.Info("Shutting down.");
                    break;
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Command is not supported, was {parseResult.Command.GetType()}");
                }

                _ui.ShowResult(result);
            }
        }
    }
}