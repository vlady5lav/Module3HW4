using System;

namespace ModuleHW
{
    public class SumProvider
    {
        private readonly int _limit;
        private int _counter;

        public SumProvider()
        {
            _counter = 1;
            _limit = 200;
        }

        public SumProvider(int limit)
        {
            _limit = limit;
        }

        public event SumService.SumDelegate CounterIsTooBig;

        public int Counter => _counter;
        public int Limit => _limit;

        public void Multiply(int multiplier)
        {
            _counter *= multiplier;

            Console.WriteLine(string.Empty);

            if (_counter > _limit)
            {
                Console.WriteLine($"{_counter} > {_limit}");
                Console.WriteLine($"{_counter} / 5 = {_counter / 5}");
                Console.WriteLine($"{_counter} % 7 = {_counter % 7}");
                CounterIsTooBig?.Invoke(_counter / 5, _counter % 7);
            }
            else
            {
                Console.WriteLine($"{_counter} <= {_limit}");
            }
        }
    }
}
