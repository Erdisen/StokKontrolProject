using Microsoft.EntityFrameworkCore;
using StokKontrolProject.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrolProject.Repositories.Context
{
    public class StokKontrolContext : DbContext
    {
        public StokKontrolContext(DbContextOptions<StokKontrolContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-JH0OB08; Database=StokKontrolDB; uid = sa; pwd=1;");
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<User> Users { get; set; }

    }
}
