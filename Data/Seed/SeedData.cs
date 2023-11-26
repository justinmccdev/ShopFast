using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopFast.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using ShopFast.Models;

namespace ShopFast.Data.Seed
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                /* Guest Cart Now Seeded in Program.cs  
                 * Leaving this code for review and as example to alternative seeding
                // Logic to seed a guest shopping cart
                if (!context.ShoppingCarts.Any(c => c.UserId == "e392ab50-2933-4cb8-b96f-2a8441b59e1a"))
                {
                    var guestCart = new ShoppingCart
                    {
                        UserId = "e392ab50-2933-4cb8-b96f-2a8441b59e1a",
                        Status = "Active"
                    };

                    context.ShoppingCarts.Add(guestCart);
                    await context.SaveChangesAsync();
                }*/

                // Check if any categories already exist.
                if (context.Categories.Any())
                {
                    return;   // Database has been seeded.
                }

                // Add categories to the context
                var categories = new Category[]
                {
                    new Category { Name = "Fruits" },
                    new Category { Name = "Vegetables" },
                    new Category { Name = "Bakery" },
                    new Category { Name = "Dairy" },
                    new Category { Name = "Meat" },
                    new Category { Name = "Frozen" },
                    new Category { Name = "Beverages" },
                };

                foreach (Category c in categories)
                {
                    context.Categories.Add(c);
                }

                await context.SaveChangesAsync();

                // Check if any products already exist.
                if (context.Products.Any())
                {
                    return;   // Database has been seeded.
                }

                // Add products to the context
                var products = new Product[]
                {
                    // Fruits
                    new Product { Name = "Apple", Description = "A delicious red apple", Price = 0.5m, ImageUrl = "images/apple.jpg", CategoryId = categories[0].Id },
                    new Product { Name = "Banana", Description = "A ripe yellow banana", Price = 0.3m, ImageUrl = "images/banana.jpg", CategoryId = categories[0].Id },

                    // Vegetables
                    new Product { Name = "Carrot", Description = "A healthy orange carrot", Price = 0.3m, ImageUrl = "images/carrot.jpg", CategoryId = categories[1].Id },
                    new Product { Name = "Broccoli", Description = "A fresh green broccoli", Price = 0.6m, ImageUrl = "images/broccoli.jpg", CategoryId = categories[1].Id },

                    // Bakery
                    new Product { Name = "Bread", Description = "A loaf of fresh bread", Price = 1.5m, ImageUrl = "images/bread.jpg", CategoryId = categories[2].Id },
                    new Product { Name = "Croissant", Description = "A delicious buttery croissant", Price = 1.0m, ImageUrl = "images/croissant.jpg", CategoryId = categories[2].Id },

                    // Dairy
                    new Product { Name = "Milk", Description = "A carton of fresh milk", Price = 1.2m, ImageUrl = "images/milk.jpg", CategoryId = categories[3].Id },
                    new Product { Name = "Cheese", Description = "A block of tasty cheese", Price = 2.5m, ImageUrl = "images/cheese.jpg", CategoryId = categories[3].Id },

                    // Meat
                    new Product { Name = "Chicken", Description = "A pack of fresh chicken", Price = 5.0m, ImageUrl = "images/chicken.jpg", CategoryId = categories[4].Id },
                    new Product { Name = "Beef", Description = "A pack of tender beef", Price = 7.0m, ImageUrl = "images/beef.jpg", CategoryId = categories[4].Id },

                    // Frozen
                    new Product { Name = "Ice Cream", Description = "A tub of delicious ice cream", Price = 4.0m, ImageUrl = "images/ice_cream.jpg", CategoryId = categories[5].Id },
                    new Product { Name = "Frozen Pizza", Description = "A frozen pizza ready to bake", Price = 3.5m, ImageUrl = "images/frozen_pizza.jpg", CategoryId = categories[5].Id },

                    // Beverages
                    new Product { Name = "Coca Cola", Description = "A refreshing bottle of Coca Cola", Price = 1.5m, ImageUrl = "images/coca_cola.jpg", CategoryId = categories[6].Id },
                    new Product { Name = "Orange Juice", Description = "A carton of fresh orange juice", Price = 2.0m, ImageUrl = "images/orange_juice.jpg", CategoryId = categories[6].Id },
                };

                foreach (Product p in products)
                {
                    context.Products.Add(p);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
