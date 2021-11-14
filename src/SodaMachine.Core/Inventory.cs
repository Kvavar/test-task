using System;
using System.Collections.Generic;
using SodaMachine.Core.Resources;
using SodaMachine.Core.Validation;

namespace SodaMachine.Core
{
    public class Inventory
    {
        private readonly IInventoryValidator _validator;
        private Dictionary<string, int> _inventory;
        private Dictionary<string, decimal> _prices;

        public Inventory(Dictionary<string, int> inventory, Dictionary<string, decimal> prices, IInventoryValidator validator)
        {
            _validator = validator;
            LoadPrices(prices);
            LoadInventory(inventory);
        }

        public bool TryTake(string order, out string message)
        {
            message = string.Empty;

            if (!CheckAvailability(order))
            {
                message = string.Format(Messages.NoOrderAvailable, order);

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

        private void LoadPrices(Dictionary<string, decimal> prices)
        {
            var validationResult = _validator.ValidatePrices(prices);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.Message);
            }

            _prices = prices;
        }

        private void LoadInventory(Dictionary<string, int> inventory)
        {
            var validationResult = _validator.ValidateInventory(inventory);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.Message);
            }

            _inventory = inventory;
        }
    }
}