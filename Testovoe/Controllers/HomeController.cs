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
            if (User.Identity.IsAuthenticated)
            {
                Client client1 = _db.Clients.FirstOrDefault();
                if (client1 != null)
                {
                    var deals = _db.Deals
                    .Include(x => x.User)
                    .Include(x => x.Client)
                    .Where(x => !x.IsDeleted);
                    return View(deals);
                }
                else
                {
                    string username = User.Identity.Name;
                    User user = _db.Users.FirstOrDefault(p => p.Login == username);
                    var client = new Client
                    {
                        BonusBalance = 0,
                        FirstName = "Default",
                        IsDeleted = false,
                        PhoneNumber = 0,
                        SecondName = "Client"
                    };
                    _db.Deals.Add(new Deal
                    {
                        User = user,
                        Client = client,
                        IsDeleted = false,
                        Date = DateTime.Now,
                        DownBonus = 5,
                        AmountOfPurchase = 995,
                        TransactionAmout = 1000,
                        UpBonus = 2,
                    });
                    _db.SaveChanges();
                    var deals = _db.Deals
                    .Include(x => x.User)
                    .Include(x => x.Client)
                    .Where(x => !x.IsDeleted);
                    return View(deals);
                }

            }
            else { return Content("не аутентифицирован"); }
            
        }
        public IActionResult CreateClient() // добавление клиента
        {
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
        public async Task<IActionResult> EditClient(Client client)// редактирование клиента
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
            public int ClientId { get; set; }
            public DateTime Date { get; set; }
            public int UserId { get; set; }
            public bool IsDeleted { get; set; }
            public int AmountOfPurchase { get; set; }
            public int UpBonus { get; set; }
            public int DownBonus { get; set; }
            public int TransactionAmout { get; set; }
            public int BonusBalance { get; set; }
            public string ClientFirstName { get; set; }
            public string ClientSecondName { get; set; }
            public string ClientFullName { get; set; }
            public string UserFirstName { get; set; }
            public string UserSecondName { get; set; }
            public string UserFullName { get; set; }
            public int PhoneNumber { get; set; }


        }

        public async Task<IActionResult> NewDeal() // Добавление нового чека
        {
            string username = User.Identity.Name;
            User user = await _db.Users.FirstOrDefaultAsync(p => p.Login == username);
            Client client = await _db.Clients.FirstOrDefaultAsync(x => x.FirstName == "Default");
            Transaction t = new Transaction
            {
                UserId = user.Id,
                UserFirstName = user.FirstName,
                UserSecondName = user.SecondName,
                ClientId = client.Id,
                ClientFirstName = client.FirstName,
                ClientSecondName = client.SecondName,
                BonusBalance = client.BonusBalance,
                Date = DateTime.Now,
                ClientFullName = client.FirstName + " " + client.SecondName,
                UserFullName = user.FirstName + " " + user.SecondName,
                PhoneNumber = client.PhoneNumber
            };
            return View(t);
        }

        public async Task<IActionResult> NewDeal1(Transaction transaction) // Добавление нового чека
        {
            string username = User.Identity.Name;
            User user = await _db.Users.FirstOrDefaultAsync(p => p.Login == username);
            Client client = _db.Clients.FirstOrDefault(p => p.Id == transaction.ClientId);
            client.BonusBalance +=transaction.UpBonus-transaction.DownBonus;
            _db.Deals.Add(new Deal
            {
                Client = client,
                User = user,
                IsDeleted = false,
                Date = DateTime.Now,
                DownBonus = transaction.DownBonus,
                AmountOfPurchase = transaction.AmountOfPurchase,
                TransactionAmout = transaction.TransactionAmout,
                UpBonus = transaction.UpBonus,
            });
            _db.Clients.Update(client);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
