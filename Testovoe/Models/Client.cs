using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Testovoe.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; } // имя клиента
        public string SecondName { get; set; } // фамилия клиента
        public int PhoneNumber { get; set; } // телефон клиента
        public int BonusBalance { get; set; } // бонусы клиента
        public int Discount { get; set; } // скидка клиента
    }
}
