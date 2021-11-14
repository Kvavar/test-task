using NUnit.Framework;
using SodaMachine.Core.CommandParser;
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
            Assert.IsInstanceOf<Insert>(result.Command);
            var command = result.Command as Insert;
            Assert.NotNull(command);
            Assert.AreEqual(money, command.Amount);
        }

        [Test]
        public void TestValidOrderByCashCommand()
        {
            var item = "coke";
            var input = $"order {item}";
            var result = CommandParser.Parse(input);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsInstanceOf<OrderByCash>(result.Command);
            var command = result.Command as OrderByCash;
            Assert.NotNull(command);
            Assert.AreEqual(item, command.Order);
        }

        [Test]
        public void TestValidOrderBySmsCommand()
        {
            var item = "fanta";
            var input = $"sms order {item}";
            var result = CommandParser.Parse(input);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsInstanceOf<OrderBySms>(result.Command);
            var command = result.Command as OrderBySms;
            Assert.NotNull(command);
            Assert.AreEqual(item, command.Order);
        }

        [Test]
        public void TestValidRecallCommand()
        {
            var input = "recall";
            var result = CommandParser.Parse(input);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsInstanceOf<Recall>(result.Command);
        }

        [Test]
        public void TestValidStopCommand()
        {
            var input = "stop";
            var result = CommandParser.Parse(input);

            Assert.IsTrue(result.IsSuccess);
            Assert.IsInstanceOf<Stop>(result.Command);
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