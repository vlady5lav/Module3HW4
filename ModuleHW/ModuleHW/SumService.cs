using System;
using System.Collections.Generic;

namespace ModuleHW
{
    public class SumService
    {
        private readonly List<int> _resultsList;
        private int _sumCallsCount;

        public SumService()
        {
            _resultsList = new List<int>();
            _sumCallsCount = default;
        }

        public delegate int SumDelegate(int a, int b);

        public List<int> ResultsList => _resultsList;

        public int SumCallsCount => _sumCallsCount;

        public int GetSum(int a, int b)
        {
            return SafeSum(InternalSum, a, b);
        }

        private int SafeSum(SumDelegate sumDelegate, int a, int b)
        {
            try
            {
                var result = sumDelegate.Invoke(a, b);
                _resultsList.Add(result);
                _sumCallsCount++;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Method: {ex.TargetSite}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw new AggregateException(ex);
            }
        }

        private int InternalSum(int a, int b)
        {
            return a + b;
        }
    }
}
