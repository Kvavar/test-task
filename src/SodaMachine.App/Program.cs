using System.Collections.Generic;
using SodaMachine.Core;
using SodaMachine.Core.UserInterface;
using SodaMachine.Core.Validation;

namespace SodaMachine.App
{
    class Program
    {
        static void Main(string[] args)
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
            var validator = new InventoryValidator();
            var sodaMachine = new Machine(inventory, prices, ui, validator);

            sodaMachine.Start();
        }
    }
}
