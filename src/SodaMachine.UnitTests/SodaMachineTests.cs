using System.Collections.Generic;
using NUnit.Framework;
using SodaMachine.Core;
using SodaMachine.Core.Resources;
using SodaMachine.Core.UserInterface;
using SodaMachine.Core.Validation;

namespace SodaMachine.UnitTests
{
    [TestFixture]
    public class SodaMachineTests
    {
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

        [TestCase("coke", 20)]
        [TestCase("sprite", 15)]
        [TestCase("fanta", 15)]
        public void TestPurchaseByCash_EnoughMoney(string order, decimal money)
        {
            var machine = CreateNewMachineWith(_prices, _inventory);
            machine.InsertMoney(money);

            var result = machine.PurchaseByCash(order);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(string.Format(Messages.GivingOrderOut, order), result.Message);
        }

        [TestCase("coke", 17)]
        [TestCase("sprite", 10)]
        [TestCase("fanta", 5)]
        public void TestPurchaseByCash_NotEnoughMoney(string order, decimal money)
        {
            var machine = CreateNewMachineWith(_prices, _inventory);
            machine.InsertMoney(money);
            var price = _prices[order];
            var result = machine.PurchaseByCash(order);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(string.Format(Messages.NeedMoreMoney, price - money), result.Message);
        }

        [TestCase("coke", 17)]
        [TestCase("sprite", 10)]
        [TestCase("fanta", 5)]
        public void TestPurchaseBySms_NotEnoughMoney(string order, decimal money)
        {
            var machine = CreateNewMachineWith(_prices, _inventory);
            machine.InsertMoney(money);
            var result = machine.PurchaseBySms(order);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(string.Format(Messages.GivingOrderOut, order), result.Message);
        }

        [TestCase("coke", 20)]
        [TestCase("sprite", 15)]
        [TestCase("fanta", 15)]
        public void TestPurchaseByCash_TheLastAvailable(string order, decimal money)
        {
            var prices = new Dictionary<string, decimal> { { order, money } };
            var inventory = new Dictionary<string, int> { { order, 1 } };

            var machine = CreateNewMachineWith(prices, inventory);

            machine.InsertMoney(money);
            var result = machine.PurchaseByCash(order);

            //the last available item sold successfully
            Assert.IsTrue(result.IsSuccess);

            machine.InsertMoney(money);
            result = machine.PurchaseByCash(order);
            //No items to sell
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(string.Format(Messages.NoOrderAvailable, order), result.Message);
        }

        [TestCase("coke", 20)]
        [TestCase("sprite", 15)]
        [TestCase("fanta", 15)]
        public void TestPurchaseBySms_TheLastAvailable(string order, decimal money)
        {
            var prices = new Dictionary<string, decimal> { { order, money } };
            var inventory = new Dictionary<string, int> { { order, 1 } };

            var machine = CreateNewMachineWith(prices, inventory);

            machine.InsertMoney(money);
            var result = machine.PurchaseBySms(order);

            //the last available item sold successfully
            Assert.IsTrue(result.IsSuccess);

            machine.InsertMoney(money);
            result = machine.PurchaseBySms(order);
            //No items to sell
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(string.Format(Messages.NoOrderAvailable, order), result.Message);
        }

        [Test]
        public void TestRecall_PositiveBalance()
        {
            var money = 10;
            var machine = CreateNewMachineWith(_prices, _inventory);
            machine.InsertMoney(money);
            var result = machine.Recall();

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(string.Format(Messages.ReturnBalanceToCustomer, money), result.Message);
        }

        [Test]
        public void TestRecall_ZeroBalance()
        {
            var machine = CreateNewMachineWith(_prices, _inventory);
            var result = machine.Recall();

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(Messages.NoMoneyToReturn, result.Message);
        }

        private static Machine CreateNewMachineWith(Dictionary<string, decimal> prices, Dictionary<string, int> inventory)
        {
            var ui = new UserInterface();
            var validator = new InventoryValidator();

            return new Machine(inventory, prices, ui, validator);
        }
    }
}
