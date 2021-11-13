using System.Collections.Generic;

namespace SodaMachine.Validation
{
    public static class InventoryValidator
    {
        public static  ValidationResult ValidatePrices(Dictionary<string, decimal> prices)
        {
            if (prices == null || prices.Count < 1)
                return ValidationResult.Invalid("Prices cannot be empty");

            foreach (var price in prices)
            {
                if (price.Value < 0)
                    return ValidationResult.Invalid($"Inventory item price must be greater than zero, was {price.Value}.");
            }

            return ValidationResult.Valid();
        }

        public static ValidationResult ValidateInventory(Dictionary<string, int> inventory)
        {
            if (inventory == null || inventory.Count < 1)
                return ValidationResult.Invalid("Inventory cannot be empty");

            foreach (var item in inventory)
            {
                if (item.Value < 1)
                    return ValidationResult.Invalid($"Inventory item amount must be greater than zero, was {item.Value}.");
            }

            return ValidationResult.Valid();
        }
    }
}