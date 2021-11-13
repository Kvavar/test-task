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

        Dictionary<string, decimal> _prices = new Dictionary<string, decimal>
        {
            { "coke", 20 },
            { "sprite", 15 },
            { "fanta", 15 }
        };

        [SetUp]
        public void Setup()
        {
            var inventory = new Dictionary<string, int>
            {
                { "coke", 10 },
                { "sprite", 13 },
                { "fanta", 17 }
            };

            var ui = new UserInterface();

            _machine = new Machine(inventory, _prices, ui);
        }

        [Test]
        public void TestPurchase_EnoughMoney()
        {
            _machine.InsertMoney(55);

            var result = _machine.PurchaseByCash("coke");

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
    }
}
