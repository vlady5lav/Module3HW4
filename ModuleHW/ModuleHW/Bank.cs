using System.Collections.Generic;

namespace ModuleHW
{
    public partial class App
    {
        public class Bank
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public List<Transaction> Transactions { get; set; }
        }
    }
}
