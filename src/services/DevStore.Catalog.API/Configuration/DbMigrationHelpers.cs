using DevStore.Catalog.API.Data;
using DevStore.Catalog.API.Models;
using DevStore.WebAPI.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace DevStore.Catalog.API.Configuration
{

    public static class DbMigrationHelpers
    {
        /// <summary>
        /// Generate migrations before running this method, you can use command bellow:
        /// Nuget package manager: Add-Migration DbInit -context CatalogContext
        /// Dotnet CLI: dotnet ef migrations add DbInit -c CatalogContext
        /// </summary>
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();

            await DbHealthChecker.TestConnection(context);

            if (env.IsDevelopment() || env.IsEnvironment("Docker"))
            {
                await context.Database.EnsureCreatedAsync();
                await EnsureSeedProducts(context);
            }
        }

        private static async Task EnsureSeedProducts(CatalogContext context)
        {
            if (context.Products.Any())
                return;

            await context.Products.AddAsync(new Product() { Name = "T-Shirt 4 Head", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "4head.webp", Stock = 5 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt 4 Head White", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Branca 4head.webp", Stock = 5 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Tilt", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "tiltado.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Tilt White", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Branco Tiltado.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Heisenberg", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Heisenberg.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Kappa", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Kappa.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt MacGyver", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "MacGyver.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Maestria", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Maestria.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Code Life Black", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "camiseta2.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt My Yoda", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "My Yoda.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Pato", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Pato.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Xavier School", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Xaviers School.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Yoda", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Yoda.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Quack", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Quack.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Rick And Morty 2", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Rick And Morty Captured.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Rick And Morty", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Rick And Morty.webp", Stock = 5 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Say My Name", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Say My Name.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Support", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "support.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Try Hard", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Tryhard.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Mug Joker Wanted", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-joker Wanted.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Mug Joker", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Joker.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Mug Nightmare", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Nightmare.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Mug Ozob", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Ozob.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Mug Rick and Morty", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Rick and Morty.jpg", Stock = 5 });
            await context.Products.AddAsync(new Product() { Name = "Mug Wonder Woman", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Wonder Woman.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Mug No Coffee No Code", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca4.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Mug Batman", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca1--batman.jpg", Stock = 5 });
            await context.Products.AddAsync(new Product() { Name = "Mug Vegeta", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca1-Vegeta.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Mug Batman Black", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Batman.jpg", Stock = 8 });
            await context.Products.AddAsync(new Product() { Name = "Mug Big Bang Theory", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-bbt.webp", Stock = 0 });
            await context.Products.AddAsync(new Product() { Name = "Mug Mushroom", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-cogumelo.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Mug Geeks", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Geeks.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Mug IronMan", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-ironman.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Debugar Black", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 75.00M, DateAdded = DateTime.Now, Image = "camiseta4.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Code Life Gray", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 99.00M, DateAdded = DateTime.Now, Image = "camiseta3.jpg", Stock = 3 });
            await context.Products.AddAsync(new Product() { Name = "Mug Star Bugs Coffee", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 20.00M, DateAdded = DateTime.Now, Image = "caneca1.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Mug Programmer Code", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 15.00M, DateAdded = DateTime.Now, Image = "caneca2.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "T-Shirt Software Developer", Description = "T-Shirt 100% cotton, high durable to wash and high temperatures washing.", Active = true, Price = 100.00M, DateAdded = DateTime.Now, Image = "camiseta1.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Mug Turn Coffee into Code", Description = "Porcelain mug with high-strength thermal printing.", Active = true, Price = 20.00M, DateAdded = DateTime.Now, Image = "caneca3.jpg", Stock = 10 });

            await context.SaveChangesAsync();
        }
    }

}
