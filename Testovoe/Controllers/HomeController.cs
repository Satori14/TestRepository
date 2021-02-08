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
            //    FirstName = "Hoakin",
            //    SecondName = "Ho",
            //    Login = "11111",
            //    IsDeleted = false,
            //    Password = "22222"
            //};

            //var client = new Client
            //{
            //    BonusBalance = 8,
            //    FirstName = "Uinston",
            //    IsDeleted = false,
            //    PhoneNumber = 99999,
            //    SecondName = "Ui"
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
            public DateTime date;
            public int userId;
            public bool isDeleted;
            public int amountOfPurchase;
            public int upBonus; 
            public int downBonus;
            public int transactionAmout { get; set; }
            public int bonusBalance;
            public string clientFirstName;
            public string clientSecondName;
            public string clientFullName;
            public string userFirstName;
            public string userSecondName;
            public string userFullName;
            public int phoneNumber;


        }

        public async Task<IActionResult> NewDeal(int id)
        {
            string username = User.Identity.Name;
            User user = await _db.Users.FirstOrDefaultAsync(p => p.Login == username);
            Client client = await _db.Clients.FirstOrDefaultAsync(x => x.Id == id);
            Transaction t = new Transaction
            {
                userId = user.Id,
                userFirstName = user.FirstName,
                userSecondName = user.SecondName,
                clientId = id,
                clientFirstName = client.FirstName,
                clientSecondName = client.SecondName,
                bonusBalance = client.BonusBalance,
                date = DateTime.Now,
                clientFullName = client.FirstName + " " + client.SecondName,
                userFullName = user.FirstName + " " + user.SecondName,
                phoneNumber = client.PhoneNumber
            };
            return View(t);
        }

        public async Task<IActionResult> NewDeal1(Transaction transaction)
        {
            string username = User.Identity.Name;
            User user = await _db.Users.FirstOrDefaultAsync(p => p.Login == username);
            Client client = _db.Clients.FirstOrDefault(p => p.Id == 2); //transaction.clientId);
            client.BonusBalance +=transaction.upBonus-transaction.downBonus;
            // client.FirstName = transaction.clientFirstName;
            // client.IsDeleted = false;
            //client.PhoneNumber = transaction.phoneNumber;
            //client.SecondName = transaction.clientSecondName;

            //_db.Clients.Update(client);
            //var client = new Client
            //{
            //    Id = transaction.clientId,
            //    BonusBalance = transaction.bonusBalance,
            //    FirstName = transaction.clientFirstName,
            //    IsDeleted = false,
            //    PhoneNumber = transaction.phoneNumber,
            //    SecondName = transaction.clientSecondName
            //};
            _db.Deals.Add(new Deal
            {
                Client = client,
                User = user,
                IsDeleted = false,
                Date = transaction.date,
                DownBonus = transaction.downBonus,
                AmountOfPurchase = transaction.amountOfPurchase,
                TransactionAmout = transaction.transactionAmout,
                UpBonus = transaction.upBonus,
            });
            _db.Clients.Update(client);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
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
        //public async Task<IActionResult> NewDeal1(Deal deal)
        //{
        //    string username = User.Identity.Name;
        //    User user = await _db.Users.FirstOrDefaultAsync(p => p.Login == username);
        //    deal.UserId = user.Id;
        //    _db.Deals.Add(deal);
        //    await _db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
    }
}
