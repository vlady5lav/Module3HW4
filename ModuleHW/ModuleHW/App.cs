using System;
using System.Collections.Generic;
using System.Linq;

namespace ModuleHW
{
    public partial class App
    {
        private const int BanksMin = 2;
        private const int BanksMax = 10;

        private const int NameMaxLength = 20;
        private const int NameMinLength = 4;

        private static readonly Random _random = new Random();

        public void Start()
        {
            var banks = GenerateBanks();
            var users = GenerateUsers(banks);

            // 1) Сделать выборку всех Пользователей, Имя + Фамилия которых длиннее 12 символов.

            var longNamedUsers = users.Where(u => (u.FirstName.Length + u.LastName.Length) > 12).OrderBy(u => u.FirstName).ToList();

            Console.WriteLine(string.Empty);
            Console.WriteLine("Generated users:");
            foreach (var user in longNamedUsers)
            {
                Console.WriteLine($"{user.FirstName} {user.LastName}");
            }

            // 2) Сделать выборку всех транзакций (в результате должен получиться список из 1000 транзакций)

            var transactionsList = users.SelectMany(u => u.Transactions).ToList();

            Console.WriteLine(string.Empty);
            Console.WriteLine("Generated transactions:");

            foreach (var transaction in transactionsList)
            {
                Console.WriteLine($"{transaction.Value} {transaction.Currency}");
            }

            // 3) Вывести Банк и всех его пользователей (Имя + Фамилия + количество транзакций в гривне), отсортированных по Фамилии в порядке убывания, в таком виде :
            //   Имя банка
            //   ***************
            //   Игорь Сердюк
            //   Николай Басков

            var userBanks = banks
                .Join(
                users.OrderByDescending(u => u.LastName),
                b => b.Id,
                u => u.Bank.Id,
                (b, u) =>
                new
                {
                    BankName = b.Name,
                    u.FirstName,
                    u.LastName,
                    u.Transactions,
                    UserTransactions = u.Transactions.Count(x => x.Currency == Currency.UAH)
                })
                .GroupBy(
                b => b.BankName)
                .ToList();

            foreach (var group in userBanks)
            {
                Console.WriteLine(string.Empty);
                Console.WriteLine($"Group Key: {group.Key}");
                Console.WriteLine($"Users:");
                Console.WriteLine(string.Empty);

                foreach (var user in group)
                {
                    Console.WriteLine($"{user.FirstName} {user.LastName} {user.Transactions.Count} ({user.Transactions.Count(t => t.Currency == Currency.UAH)})");
                }
            }

            var banksUsers = banks.GroupJoin(users.OrderByDescending(u => u.LastName), b => b.Name, u => u.Bank.Name, (b, u) => new { Bank = b, Users = u.ToList() }).ToList();

            foreach (var items in banksUsers)
            {
                Console.WriteLine(string.Empty);
                Console.WriteLine($"Clients of \"{items.Bank.Name}\" Bank ({items.Bank.Transactions.Count}):");
                Console.WriteLine("------------------------------------------");
                foreach (var user in items.Users)
                {
                    Console.WriteLine($"{user.FirstName} {user.LastName} {user.Transactions.Count} ({user.Transactions.Count(t => t.Currency == Currency.UAH)})");
                }
            }

            // 4) Сделать выборку всех Пользователей типа Admin, у которых счет в банке, в котором больше всего транзакций

            // var bankManyTransactions = banks.Aggregate((m, n) => m.Transactions.Count > n.Transactions.Count ? m : n);
            // var bankManyTransactions = banks.OrderByDescending(b => b.Transactions.Count).First();
            // var bankManyTransactions = banks.MaxByKey(b => b.Transactions.Count);
            // var bankManyTransactions = banks.FirstOrDefault(b => b.Transactions.Count == banks.Max(b => b.Transactions.Count));

            var bankValuableTransactions = banks.Aggregate((b1, b2) => b1.Transactions.Count > b2.Transactions.Count ? b1 : b2);

            Console.WriteLine(string.Empty);
            Console.Write($"Bank with the highest amount of transactions: ");
            Console.WriteLine($"{bankValuableTransactions.Name} ({bankValuableTransactions.Transactions.Count})");

            var admins = users.Where(u => u.Type == UserType.Admin && u.Bank == bankValuableTransactions).ToList();

            Console.WriteLine(string.Empty);

            foreach (var user in admins)
            {
                Console.WriteLine($"Type: {user.Type} | Bank: {user.Bank.Name} | Name: {user.FirstName} {user.LastName}");
            }

            Console.WriteLine(string.Empty);

            // 5) Найти Пользователей(НЕ АДМИНОВ), которые произвели больше всего транзакций в определенной из валют (UAH,USD,EUR)
            // то есть найти трёх пользователей: 1й который произвел больше всего транзакций в гривне, второй пользователь, который произвел больше всего транзакций в USD
            // и третьего в EUR

            User nonAdminValuable;

            for (int i = 1; i < 4; i++)
            {
                nonAdminValuable = users.Where(u => u.Type != UserType.Admin).OrderByDescending(u => u.Transactions.Count(t => t.Currency == (Currency)i)).FirstOrDefault();

                var totalTransactions = 0;
                var totalAdmins = 0;
                var totalUsers = 0;

                foreach (var user in users)
                {
                    totalTransactions += user.Transactions.Count(t => t.Currency == (Currency)i);

                    if (user.Transactions.Any(t => t.Currency == (Currency)i))
                    {
                        _ = user.Type == UserType.Admin ? totalAdmins++ : 0;
                        totalUsers++;
                    }
                }

                Console.WriteLine($"{(Currency)i} Transactions Count: {totalTransactions}");
                Console.WriteLine($"{(Currency)i} Transactions Admins Count: {totalAdmins}");
                Console.WriteLine($"{(Currency)i} Transactions Users Count: {totalUsers}");

                if (nonAdminValuable != null)
                {
                    Console.WriteLine($"Type: {nonAdminValuable.Type} | Bank: {nonAdminValuable.Bank.Name} | Name: {nonAdminValuable.FirstName} {nonAdminValuable.LastName} | Transactions Count: {nonAdminValuable.Transactions.Count(t => t.Currency == (Currency)i)} | Currency : {(Currency)i}");
                }

                Console.WriteLine(string.Empty);
            }

            // 6) Сделать выборку транзакций банка, у которого больше всего Pemium пользователей

            var bankPremiumUsers = 0;

            foreach (var bank in banksUsers)
            {
                bankPremiumUsers = bank.Users.Count(u => u.Type == UserType.Premium);
                Console.WriteLine($"Bank: {bank.Bank.Name} | {bankPremiumUsers} Premium Users");
            }

            var bankName = banksUsers.OrderByDescending(b => b.Users.Count(u => u.Type == UserType.Premium)).Select(b => b.Bank.Name).FirstOrDefault();

            Console.WriteLine(string.Empty);
            Console.WriteLine($"Transactions of the Bank with the highest number of Premium Users ({bankName}):");
            Console.WriteLine(string.Empty);

            var bankMostPremiumUsersTransactions = banksUsers.OrderByDescending(b => b.Users.Count(u => u.Type == UserType.Premium)).SelectMany(b => b.Bank.Transactions).ToList();

            foreach (var item in bankMostPremiumUsersTransactions)
            {
                Console.WriteLine($"{item.Value} {item.Currency}");
            }

            Console.WriteLine(string.Empty);

            Console.ReadKey();
        }

        public List<Transaction> GetTransactions()
        {
            var result = new List<Transaction>();
            var sign = _random.Next(0, 2); // this is where the mistake which led to always negative values was
            var signValue = sign == 0 ? -1 : 1;

            for (var i = 0; i < 10; i++)
            {
                result.Add(new Transaction
                {
                    Value = (decimal)_random.NextDouble() * signValue * 100m,
                    Currency = GetRandomCurrency(),
                });
            }

            return result;
        }

        public UserType GetRandomUserType()
        {
            var userType = _random.Next(1, 4);

            return (UserType)userType;
        }

        public Currency GetRandomCurrency()
        {
            var currencyType = _random.Next(1, 4);

            return (Currency)currencyType;
        }

        public List<Bank> GenerateBanks()
        {
            var banksCount = _random.Next(BanksMin, BanksMax);
            var result = new List<Bank>();

            for (int i = 0; i < banksCount; i++)
            {
                result.Add(new Bank
                {
                    Id = i + 1,
                    Name = RandomString(_random.Next(NameMinLength, NameMaxLength)),
                    Transactions = new List<Transaction>()
                });
            }

            return result;
        }

        public List<User> GenerateUsers(List<Bank> banks)
        {
            var result = new List<User>();

            int bankId;
            Bank bank;
            List<Transaction> transactions;

            for (int i = 0; i < 100; i++)
            {
                bankId = _random.Next(0, banks.Count);
                bank = banks[bankId];
                transactions = GetTransactions();

                result.Add(new User
                {
                    Bank = bank,
                    FirstName = RandomString(_random.Next(NameMinLength, NameMaxLength)),
                    Id = i + 1,
                    LastName = RandomString(_random.Next(NameMinLength, NameMaxLength)),
                    Type = GetRandomUserType(),
                    Transactions = transactions
                });

                bank.Transactions.AddRange(transactions);
            }

            return result;
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
