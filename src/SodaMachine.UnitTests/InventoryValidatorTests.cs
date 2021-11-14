using System.Collections.Generic;
using NUnit.Framework;
using SodaMachine.Core.Resources;
using SodaMachine.Core.Validation;

namespace SodaMachine.UnitTests
{
    [TestFixture]
    public class InventoryValidatorTests
    {
        private readonly IInventoryValidator _validator = new InventoryValidator();

        [Test]
        public void TestValidatePrices_ValidPrices()
        {
            var prices = new Dictionary<string, decimal>
            {
                { "coke", 23 },
                { "fanta", 23 }
            };

            var result = _validator.ValidatePrices(prices);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void TestValidatePrices_EmptyPrices()
        {
            var prices = new Dictionary<string, decimal>();

            var result = _validator.ValidatePrices(prices);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(Messages.PricesCannotBeEmpty, result.Message);
        }

        [Test]
        public void TestValidatePrices_NegativePrices()
        {
            var item = "fanta";
            var negativePrice = -23;
            var prices = new Dictionary<string, decimal>
            {
                { "coke", 23 },
                { item, negativePrice }
            };

            var result = _validator.ValidatePrices(prices);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(Messages.PriceMustBeGreaterThanZero, item, negativePrice), result.Message);
        }

        [Test]
        public void TestValidateInventory_ValidInventory()
        {
            var inventory = new Dictionary<string, int>
            {
                { "coke", 10 },
                { "fanta", 10 }
            };

            var result = _validator.ValidateInventory(inventory);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void TestValidatePrices_EmptyInventory()
        {
            var inventory = new Dictionary<string, int>();

            var result = _validator.ValidateInventory(inventory);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(Messages.InventoryCannotBeEmpty, result.Message);
        }

        [Test]
        public void TestValidatePrices_NegativeAmount()
        {
            var item = "fanta";
            var negativeAmount = -1;
            var inventory = new Dictionary<string, int>
            {
                { "coke", 10 },
                { item, negativeAmount }
            };

            var result = _validator.ValidateInventory(inventory);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(Messages.ItemAmountMustBeGreaterThanZero, item, negativeAmount), result.Message);
        }
    }
}