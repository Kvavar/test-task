using System.Collections.Generic;

namespace SodaMachine
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inventory = new Dictionary<string, int>
            {
                { "coke", 10 },
                { "sprite", 10 },
                { "fanta", 10 }
            };

            var prices = new Dictionary<string, decimal>
            {
                { "coke", 20 },
                { "sprite", 15 },
                { "fanta", 15 }
            };

            var ui = new UserInterface();

            var sodaMachine = new SodaMachine(inventory, prices, ui);

            sodaMachine.Start();
        }
    }
}
