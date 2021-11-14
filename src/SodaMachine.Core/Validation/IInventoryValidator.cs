using System.Collections.Generic;

namespace SodaMachine.Core.Validation
{
    public interface IInventoryValidator
    {
        ValidationResult ValidatePrices(Dictionary<string, decimal> prices);
        ValidationResult ValidateInventory(Dictionary<string, int> inventory);
    }
}