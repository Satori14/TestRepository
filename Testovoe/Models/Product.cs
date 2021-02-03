using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Testovoe.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string DealId { get; set; } // ИД сделки
        //
        public string ProductName { get; set; } // Наименование товара
        public string Cost { get; set; } // Цена товара
    }
}
