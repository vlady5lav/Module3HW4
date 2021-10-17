using System;
using System.Collections.Generic;
using System.Linq;

namespace ModuleHW
{
    public class Starter
    {
        private readonly SumService _sumService;
        private readonly SumProvider _sumProvider;
        private readonly UserProvider _userProvider;
        private readonly List<User> _userList;

        public Starter()
        {
            _sumService = new SumService();
            _sumProvider = new SumProvider();
            _userProvider = new UserProvider();
            _userList = _userProvider.UserList;
        }

        public void Run()
        {
            var first = _userList.FirstOrDefault();

            var notSerdiuk = _userList.FirstOrDefault(x => x.LastName != "Serdiuk");

            var isNull = _userList?.Any() == true; // можно использовать внутри if

            var isNullable = _userList?.Any();

            _sumProvider.CounterIsTooBig += _sumService.GetSum;
            _sumProvider.CounterIsTooBig += _sumService.GetSum;
            _sumProvider.CounterIsTooBig -= _sumService.GetSum;

            Console.WriteLine(string.Empty);
            Console.WriteLine("Counters:");

            Multiply(8);
            Multiply(3);
            Multiply(5);
            Multiply(3);
            Multiply(5);
            Multiply(8);

            Multiply(8);
            Multiply(3);
            Multiply(5);
            Multiply(3);
            Multiply(5);
            Multiply(8);

            /*
            Console.WriteLine(string.Empty);
            Console.WriteLine("Results List:");
            _sumService.ResultsList.ForEach(Print);
            */

            Console.WriteLine(string.Empty);
            Console.WriteLine("Results List");
            _sumService.ResultsList.ForEach(delegate(int num) // анонимный метод
            {
                Console.WriteLine(num.ToString());
            });

            var sumCallsCount = _sumService.SumCallsCount;

            Console.WriteLine(string.Empty);
            Console.Write("Sum Calls Count: ");
            Console.WriteLine(sumCallsCount.ToString());

            Console.ReadLine();
        }

        public void Multiply(int multiplier)
        {
            _sumProvider.Multiply(multiplier);
        }

        /*
        public void Print(int num)
        {
            Console.WriteLine(num.ToString());
        }
        */
    }
}
