﻿using System;
using System.Collections.Generic;

namespace SodaMachine
{
    public class SodaMachine
    {
        private readonly IUserInterface _ui;
        private readonly object _purchaseLocker = new object();
        private decimal _balance;
        private Dictionary<string, int> _inventory;
        private Dictionary<string, decimal> _prices;

        public SodaMachine(Dictionary<string, int> inventory, Dictionary<string, decimal> prices, IUserInterface ui)
        {
            _ui = ui;
            LoadPrices(prices);
            LoadInventory(inventory);
        }

        /// <summary>
        /// Allows to update prices for existing inventory as well as pre-load prices
        /// </summary>
        /// <param name="prices"></param>
        public void LoadPrices(Dictionary<string, decimal> prices)
        {
            if (prices == null || prices.Count < 1)
                throw new ArgumentException("Prices cannot be empty", nameof(prices));

            foreach (var price in prices)
            {
                if (price.Value < 0)
                    throw new ArgumentException($"Inventory item price must be greater than zero, was {price.Value}.");
            }

            if (_inventory != null)
            {
                foreach (var item in _inventory)
                {
                    if (!_prices.ContainsKey(item.Key))
                        _ui.Warn($"Price for {item.Key} not found in the price list. Item will be available only for SMS purchase.");
                }
            }

            _prices = prices;
        }

        public void LoadInventory(Dictionary<string, int> inventory)
        {
            if (inventory == null || inventory.Count < 1)
                throw new ArgumentException("Inventory cannot be empty", nameof(inventory));

            foreach (var item in inventory)
            {
                if(!_prices.ContainsKey(item.Key))
                    _ui.Warn($"Price for {item.Key} not found in the price list. Item will be available only for SMS purchase.");

                if (item.Value < 1)
                    throw new ArgumentException($"Inventory item amount must be greater than zero, was {item.Value}.");
            }

            _inventory = inventory;
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
                if (!CheckAvailability(order)) 
                    return CommandResult.Fail($"No {order} available in the inventory.");

                if(!_prices.TryGetValue(order, out var price))
                    return CommandResult.Fail($"Only SMS order is available for {order}.");

                if (_balance < price)
                {
                    return CommandResult.Fail($"Need {price - _balance} more.");
                }

                _inventory[order] -= 1;
                _balance -= price;

                var message = $"Giving {order} out.";

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
                if (!CheckAvailability(order))
                    return  CommandResult.Fail($"No {order} available in the inventory.");

                _inventory[order] -= 1;

                var message = $"Giving {order} out.";

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

        private bool CheckAvailability(string order)
        {
            return _inventory.TryGetValue(order, out var available) && available >= 1;
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

                var parseResult = CommandParser.Parse(input);

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