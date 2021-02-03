using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Testovoe.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } // Логин пользователя
        public string Password { get; set; } // Пароль пользователя
        public string FirstName { get; set; } // Имя пользователя
        public string SecondName { get; set; } // Фамилия пользователя
    }
}
