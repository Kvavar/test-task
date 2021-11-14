using System.Collections.Generic;
using NUnit.Framework;
using SodaMachine.Core;
using SodaMachine.Core.UserInterface;

namespace SodaMachine.UnitTests
{
    [TestFixture]
    public class SodaMachineTests
    {
        private Machine _machine;

        private readonly Dictionary<string, decimal> _prices = new Dictionary<string, decimal>
        {
            { "coke", 20 },
            { "sprite", 15 },
            { "fanta", 15 }
        };

        private readonly Dictionary<string, int> _inventory = new Dictionary<string, int>
        {
            { "coke", 10 },
            { "sprite", 13 },
            { "fanta", 17 }
        };

        [SetUp]
        public void Setup()
        {
            var ui = new UserInterface();
            _machine = new Machine(_inventory, _prices, ui);
        }

        [TestCase("coke", 20)]
        [TestCase("sprite", 15)]
        [TestCase("fanta", 15)]
        public void TestPurchase_EnoughMoney(string order, decimal money)
        {
            _machine.InsertMoney(money);

            var result = _machine.PurchaseByCash(order);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestCase("coke", 17)]
        [TestCase("sprite", 10)]
        [TestCase("fanta", 5)]
        public void TestPurchase_NotEnoughMoney(string order, decimal money)
        {
            _machine.InsertMoney(money);
            var price = _prices[order];
            var result = _machine.PurchaseByCash(order);

            Assert.IsFalse(result.IsSuccess);

            Assert.IsTrue(result.Message.Contains($"Need {price - money} more."));
        }

        [TestCase("coke", 20)]
        [TestCase("sprite", 15)]
        [TestCase("fanta", 15)]
        public void TestPurchase_TheLastAvailable(string order, decimal money)
        {
            var prices = new Dictionary<string, decimal> { { order, money } };
            var inventory = new Dictionary<string, int> { { order, 1} };

            var ui = new UserInterface();
            _machine = new Machine(inventory, prices, ui);

            _machine.InsertMoney(money);
            var result = _machine.PurchaseByCash(order);

            //the last available item sold successfully
            Assert.IsTrue(result.IsSuccess);

            _machine.InsertMoney(money);
            result = _machine.PurchaseByCash(order);
            //No items to sell
            Assert.IsFalse(result.IsSuccess);

            Assert.IsTrue(result.Message.Contains($"No {order} available in the inventory."));
        }
    }
}
