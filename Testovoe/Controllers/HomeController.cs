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
            //_db.Users.Add(new User
            //{
            //    Login = "sdgd",
            //    Password = "sad",
            //    FirstName = "kilu",
            //    SecondName = "dfdhrh"
            //});
            //_db.SaveChanges();
            //var users = _db.Users.ToList();
            //return View(users);

            // var user =  new User
            // {
            //     FirstName = "f",
            //     SecondName = "t",
            //     Login = "ttt",
            //     Password = "123"
            // };

            //var client = new Client
            //{
            //    BonusBalance = 8,
            //    Discount = 6,
            //    FirstName = "ccc",
            //    PhoneNumber = 99999,
            //    SecondName = "cc"
            //};
            //_db.Deals.Add(new Deal
            //{
            //    User = user,
            //    Client = client,
            //    IsDeleted = false,
            //    Date = DateTime.Now,
            //    DownBonus = 5,
            //    AmountOfPurchase = 2,
            //    TransactionAmout = 1,
            //    UpBonus = 2,
            //});
            //_db.SaveChanges();

            var deals = _db.Deals
                .Include(x => x.User)
                .Include(x => x.Client)
                .Where(x => !x.IsDeleted);
            return View(deals);

            
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
