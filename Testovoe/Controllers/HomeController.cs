using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Testovoe.Models;

namespace Testovoe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger , ApplicationContext context)
        {
            _db = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //var user = new User
            //{
            //    FirstName = "Bill",
            //    SecondName = "Bi",
            //    Login = "111",
            //    IsDeleted = false,
            //    Password = "222"
            //};

            //var client = new Client
            //{
            //    BonusBalance = 8,
            //    Discount = 6,
            //    FirstName = "Alex",
            //    IsDeleted = false,
            //    PhoneNumber = 99999,
            //    SecondName = "Al"
            //};
            //_db.Deals.Add(new Deal
            //{
            //    User = user,
            //    Client = client,
            //    IsDeleted = false,
            //    Date = DateTime.Now,
            //    DownBonus = 5,
            //    AmountOfPurchase = 995,
            //    TransactionAmout = 1000,
            //    UpBonus = 2,
            //});
            //_db.Products.Add(new Product
            //{
            //    DealId = 1,
            //    ProductName = "игра",
            //    Cost = 1000
            //});
            //_db.SaveChanges();

            var deals = _db.Deals
                .Include(x => x.User)
                .Include(x => x.Client)
                .Where(x => !x.IsDeleted);
            return View(deals);


        }
        public IActionResult CreateClient() // добавление клиента
        {
            //return View(_db.Clients.ToList()); 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateClient(Client client) //сохранение добавления
        {
            _db.Clients.Add(client);
            await _db.SaveChangesAsync();
            return RedirectToAction("ClientList");
        }


        public async Task<IActionResult> EditClient(int? id)  // редактирование клиента
        {
            if (id != null)
            {
                Client client = await _db.Clients.FirstOrDefaultAsync(p => p.Id == id);
                if (client != null)
                    return View(client);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditClient(Client client)
        {
            Client client1 = await _db.Clients.FirstOrDefaultAsync(p => p.Id == client.Id);
            client1.FirstName = client.FirstName;
            client1.PhoneNumber = client.PhoneNumber;
            client1.SecondName = client.SecondName;
            client1.BonusBalance = client.BonusBalance;
            client1.Discount = client.Discount;
            _db.Clients.Update(client1);
            await _db.SaveChangesAsync();
            return RedirectToAction("ClientList");
        }


        public IActionResult ClientList()
        {
            var clients =_db.Clients.ToList().Where(x => !x.IsDeleted);
            return View(clients);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NewDeal()
        {
            return View();
        }
    }
}
