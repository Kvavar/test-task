using System;
using System.Collections.Generic;
using SodaMachine.Core.Resources;

namespace SodaMachine.Core.Validation
{
    public class InventoryValidator : IInventoryValidator
    {
        public ValidationResult ValidatePrices(Dictionary<string, decimal> prices)
        {
            if (prices == null || prices.Count < 1)
            {
                return ValidationResult.Invalid(Messages.PricesCannotBeEmpty);
            }

            foreach (var price in prices)
            {
                if (price.Value < 0)
                {
                    return ValidationResult.Invalid(string.Format(Messages.PriceMustBeGreaterThanZero, price.Key, price.Value));
                }
            }

            return ValidationResult.Valid();
        }

        public ValidationResult ValidateInventory(Dictionary<string, int> inventory)
        {
            if (inventory == null || inventory.Count < 1)
            {
                return ValidationResult.Invalid(Messages.InventoryCannotBeEmpty);
            }

            foreach (var item in inventory)
            {
                if (item.Value < 1)
                {
                    return ValidationResult.Invalid(string.Format(Messages.ItemAmountMustBeGreaterThanZero, item.Key, item.Value));
                }
            }

            return ValidationResult.Valid();
        }
    }
}