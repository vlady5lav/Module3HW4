using System;
using System.Collections.Generic;

namespace ModuleHW
{
    public class UserProvider
    {
        private readonly List<User> _userList;

        public UserProvider()
        {
            _userList = GetUserList();
        }

        public List<User> UserList => _userList;

        private List<User> GetUserList()
        {
            return new List<User>
            {
                new User { ID = 1, FirstName = "Ihor", LastName = "Serdiuk", Age = 27, BirthDate = DateTime.UtcNow.AddYears(-10), },
                new User { ID = 2, FirstName = "Taras", LastName = "Serdiuk", Age = 27, BirthDate = DateTime.UtcNow.AddYears(-11), },
                new User { ID = 3, FirstName = "Dmitry", LastName = "Serdiuk", Age = 27, BirthDate = DateTime.UtcNow.AddYears(-12), },
                new User { ID = 4, FirstName = "Georgiy", LastName = "Serdiuk", Age = 27, BirthDate = DateTime.UtcNow.AddYears(-13), },
                new User { ID = 5, FirstName = "Valentin", LastName = "Serdiuk", Age = 27, BirthDate = DateTime.UtcNow.AddYears(-14), },
                new User { ID = 6, FirstName = "Piotr", LastName = "Vasilenko", Age = 27, BirthDate = DateTime.UtcNow.AddYears(-15), },
                new User { ID = 7, FirstName = "Gennadiy", LastName = "Lorens", Age = 27, BirthDate = DateTime.UtcNow.AddYears(-16), },
                new User { ID = 8, FirstName = "Alexey", LastName = "Serdiuk", Age = 27, BirthDate = DateTime.UtcNow.AddYears(-17), },
            };
        }
    }
}
