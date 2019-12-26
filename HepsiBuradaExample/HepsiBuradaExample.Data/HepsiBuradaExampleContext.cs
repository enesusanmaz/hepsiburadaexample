using HepsiBuradaExample.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HepsiBuradaExample.Data
{
    public class HepsiBuradaExampleContext : DbContext
    {
        public HepsiBuradaExampleContext(DbContextOptions connString) : base(connString)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }

    }
}
