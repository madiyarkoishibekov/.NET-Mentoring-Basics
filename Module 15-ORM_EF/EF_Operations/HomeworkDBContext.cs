using DataBaseAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess
{
    public class HomeworkDBContext : DbContext
    {
        public DbSet<ProductModel> Product { get; set; }
        public DbSet<OrderModel> Order { get; set; }

        public HomeworkDBContext(DbContextOptions options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductModel>()
                .HasMany(p => p.Orders)
                .WithOne(o => o.Product)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
