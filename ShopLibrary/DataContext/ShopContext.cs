using Microsoft.EntityFrameworkCore;
using ShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopLibrary.DataContext
{
    public class ShopContext : DbContext
    {
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public ShopContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite("Data Source=ShopDB.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                 new Category { Id = 1, Name = "Phones" },
                 new Category { Id = 2, Name = "Monitors" },
                 new Category { Id = 3, Name = "Laptops" },
                 new Category { Id = 4, Name = "Desktop Computers" }
                );
            modelBuilder.Entity<Product>().HasData(
    // Phones
    new Product { Id = 1, Title = "iPhone 15", Price = 799, CategoryId = 1 },
    new Product { Id = 2, Title = "Samsung Galaxy S24", Price = 849, CategoryId = 1 },
    new Product { Id = 3, Title = "Google Pixel 8", Price = 699, CategoryId = 1 },

    // Monitors
    new Product { Id = 4, Title = "Dell UltraSharp 27", Price = 449, CategoryId = 2 },
    new Product { Id = 5, Title = "LG UltraGear 27", Price = 399, CategoryId = 2 },

    // Laptops
    new Product { Id = 6, Title = "MacBook Air M3", Price = 1099, CategoryId = 3 },
    new Product { Id = 7, Title = "Dell XPS 15", Price = 1499, CategoryId = 3 },
    new Product { Id = 8, Title = "Lenovo ThinkPad E14", Price = 899, CategoryId = 3 },

    // Desktop Computers
    new Product { Id = 9, Title = "Gaming Desktop PC", Price = 1599, CategoryId = 4 },
    new Product { Id = 10, Title = "Office Desktop PC", Price = 699, CategoryId = 4 }
                );
        }

    }
}
