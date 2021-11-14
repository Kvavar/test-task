using NUnit.Framework;
using SodaMachine.Core.UserCommands;

namespace SodaMachine.UnitTests
{
    [TestFixture]
    public class CommandParserTests
    {
        [Test]
        public void TestValidInsertCommand()
        {
            var money = 100;
            var input = $"insert {money}";
            var result = CommandParser.Parse(input);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(CommandType.Insert, result.Command.Type);
            var amount = CommandParser.ExtractArgumentFrom<decimal>(result.Command);
            Assert.AreEqual(money, amount);
        }

        [Test]
        public void TestValidOrderByCashCommand()
        {
            var item = "coke";
            var input = $"order {item}";
            var result = CommandParser.Parse(input);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(CommandType.OrderByCash, result.Command.Type);
            var order = CommandParser.ExtractArgumentFrom<string>(result.Command);
            Assert.AreEqual(item, order);
        }

        [Test]
        public void TestValidOrderBySmsCommand()
        {
            var item = "fanta";
            var input = $"sms order {item}";
            var result = CommandParser.Parse(input);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(CommandType.OrderBySms, result.Command.Type);
            var order = CommandParser.ExtractArgumentFrom<string>(result.Command);
            Assert.AreEqual(item, order);
        }

        [Test]
        public void TestValidRecallCommand()
        {
            var input = "recall";
            var result = CommandParser.Parse(input);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(CommandType.Recall, result.Command.Type);
        }

        [Test]
        public void TestValidStopCommand()
        {
            var input = "stop";
            var result = CommandParser.Parse(input);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(CommandType.Stop, result.Command.Type);
        }

        [Test]
        public void TestInvalidInsertCommand()
        {
            var input = "insert money";
            var result = CommandParser.Parse(input);

            Assert.IsFalse(result.IsSuccess);
        }

        [Test]
        public void TestInvalidOrderByCashCommand()
        {
            var input = "order";
            var result = CommandParser.Parse(input);

            Assert.IsFalse(result.IsSuccess);
        }
    }
}