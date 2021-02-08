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
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
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
            //return Content(User.Identity.Name);

            if (User.Identity.IsAuthenticated)
            {
                var deals = _db.Deals
                .Include(x => x.User)
                .Include(x => x.Client)
                .Where(x => !x.IsDeleted);
                return View(deals);
            }
            return Content("не аутентифицирован");



            //var deals = _db.Deals
            //    .Include(x => x.User)
            //    .Include(x => x.Client)
            //    .Where(x => !x.IsDeleted);
            //return View(deals);


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
        public class Transaction 
        {
            public int clientId;
            public bool isDelited;
            public DateTime date;
            public int userId;
            public bool isDeleted;
            public int amountOfPurchase;
            public int upBonus; 
            public int downBonus;
            public int transactionAmout;
            public int discount;
            public int bonusBalance;
            public string clientFirstName;
            public string clientSecondName;
            public string clientFullName;
            public string userFirstName;
            public string userSecondName;
            public string userFullName;


        }

        public async Task<IActionResult> NewDeal(int id)
        {
            string username = User.Identity.Name;
            User user = await _db.Users.FirstOrDefaultAsync(p => p.Login == username);
            Client client = await _db.Clients.FirstOrDefaultAsync(x => x.Id == id);
            Transaction t = new Transaction();
            var deal = new Deal
            {
                ClientId = id,
                UserId = user.Id,
                IsDeleted = false,
                Date = DateTime.Now

            };
            t.userId = user.Id;
            t.userFirstName = user.FirstName;
            t.userSecondName = user.SecondName;
            t.clientId = id;
            t.clientFirstName = client.FirstName;
            t.clientSecondName = client.SecondName;
            t.discount = client.Discount;
            t.bonusBalance = client.BonusBalance;
            t.date = DateTime.Now;
            t.clientFullName= client.FirstName + " " + client.SecondName;
            t.userFullName = user.FirstName + " " + user.SecondName;

            ViewBag.UN = user.FirstName + " " + user.SecondName;
            return View(t);
        }


        //public async Task<IActionResult> NewDeal(int id)
        //{
        //    string username = User.Identity.Name;
        //    User user = await _db.Users.FirstOrDefaultAsync(p => p.Login == username);
        //    var deal = new Deal
        //    {
        //        ClientId = id,
        //        UserId = user.Id,
        //        IsDeleted = false,
        //        Date = DateTime.Now

        //    };
        //    ViewBag.UN = user.FirstName + " " + user.SecondName;
        //    return View(deal);
        //}
        public async Task<IActionResult> NewDeal1(Deal deal)
        {
            string username = User.Identity.Name;
            User user = await _db.Users.FirstOrDefaultAsync(p => p.Login == username);
            deal.UserId = user.Id;
            //deal.ClientId = 2;
            _db.Deals.Add(deal);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
