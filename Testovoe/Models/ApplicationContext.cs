using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Testovoe.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Product> Products { get; set; }

        private readonly IConfiguration Configuration;
        public ApplicationContext(IConfiguration configuration)
        {
            Configuration = configuration;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
               
        }

    }
}
