using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Testovoe.Models
{
    public class User
    {
        public List<Deal> Deals { get; set; } = new List<Deal>();
        public int Id { get; set; }
        public string Login { get; set; } // Логин пользователя
        public string Password { get; set; } // Пароль пользователя
        public string FirstName { get; set; } // Имя пользователя
        public string SecondName { get; set; } // Фамилия пользователя
    }
}
