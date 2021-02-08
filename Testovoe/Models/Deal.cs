using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Testovoe.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } // дата сделки
        public int AmountOfPurchase { get; set; } //сумма сделки
        public bool IsDeleted { get; set; }
        public int UpBonus { get; set; } // сумма начисленных бонусов
        public int DownBonus { get; set; } // сумма списаных бонусов
        public int TransactionAmout { get; set; } // сумма сделки со скидкой
        public int UserId { get; set; }
        public User User { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }

    }
}
