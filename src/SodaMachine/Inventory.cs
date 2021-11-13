using System;
using System.Collections.Generic;
using SodaMachine.Core.Validation;

namespace SodaMachine.Core
{
    public class Inventory
    {
        private Dictionary<string, int> _inventory;
        private Dictionary<string, decimal> _prices;

        public Inventory(Dictionary<string, int> inventory, Dictionary<string, decimal> prices)
        {
            LoadPrices(prices);
            LoadInventory(inventory);
        }

        /// <summary>
        /// Allows to update prices for existing inventory as well as pre-load prices
        /// </summary>
        /// <param name="prices"></param>
        public void LoadPrices(Dictionary<string, decimal> prices)
        {
            var validationResult = InventoryValidator.ValidatePrices(prices);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.Message);
            }

            _prices = prices;
        }

        public void LoadInventory(Dictionary<string, int> inventory)
        {
            var validationResult = InventoryValidator.ValidateInventory(inventory);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.Message);
            }

            _inventory = inventory;
        }

        public void ReplenishInventory(Dictionary<string, int> items)
        {
            var validationResult = InventoryValidator.ValidateInventory(items);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.Message);
            }

            foreach (var item in items)
            {
                if (_inventory.ContainsKey(item.Key))
                {
                    _inventory[item.Key] += item.Value;
                }
                else
                {
                    _inventory[item.Key] = item.Value;
                }
            }
        }

        public bool TryTake(string order, out string message)
        {
            message = string.Empty;

            if (!CheckAvailability(order))
            {
                message = $"No {order} available in the inventory.";

                return false;
            }

            _inventory[order] -= 1;

            return true;
        }

        public bool TryGetPriceFor(string order, out decimal price)
        {
            return _prices.TryGetValue(order, out price);
        }

        public bool CheckAvailability(string order)
        {
            return _inventory.TryGetValue(order, out var available) && available >= 1;
        }
    }
}