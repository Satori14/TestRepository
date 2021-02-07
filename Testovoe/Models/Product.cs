using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Testovoe.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int DealId { get; set; } // ИД сделки
        public Deal Deal { get; set; }
        public string ProductName { get; set; } // Наименование товара
        public int Cost { get; set; } // Цена товара
    }
}
