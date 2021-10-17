using System;

namespace ModuleHW
{
    public partial class App
    {
        public class Comparer : IComparable<Comparer>
        {
            private readonly double _value = default;

            public int CompareTo(Comparer other)
            {
                return _value.CompareTo(other._value);
            }
        }
    }
}
