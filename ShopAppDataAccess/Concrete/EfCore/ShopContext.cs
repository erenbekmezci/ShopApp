using Microsoft.EntityFrameworkCore;
using ShopApp.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopAppDataAccess.Concrete.EfCore
{
    public class ShopContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders  { get; set; }
        public DbSet<OrderItem> OrderItems  { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=shopapp;Username=postgres;Password=kardelen67");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>().HasKey(c => new { c.CategoryId, c.ProductId });


        }
    }
}
