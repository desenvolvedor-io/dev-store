using System;
using System.Linq;
using System.Threading.Tasks;
using DevStore.Catalog.API.Data;
using DevStore.Catalog.API.Models;
using DevStore.WebAPI.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevStore.Catalog.API.Configuration
{

    public static class DbMigrationHelpers
    {
        /// <summary>
        /// Generate migrations before running this method, you can use command bellow:
        /// Nuget package manager: Add-Migration DbInit -context PagamentosContext
        /// Dotnet CLI: dotnet ef migrations add DbInit -c PagamentosContext
        /// </summary>
        public static async Task EnsureSeedData(IServiceScope serviceScope)
        {
            var services = serviceScope.ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            //var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();

            await DbHealthChecker.TestConnection(context);

            if (env.IsDevelopment())
            {
                await context.Database.EnsureCreatedAsync();
                await EnsureSeedProducts(context);
            }
        }

        private static async Task EnsureSeedProducts(CatalogContext context)
        {
            if (context.Products.Any())
                return;

            await context.Products.AddAsync(new Product() { Name = "Camiseta 4 Head", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "4head.webp", Stock = 5 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta 4 Head Branca", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Branca 4head.webp", Stock = 5 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Tiltado", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "tiltado.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Tiltado Branca", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Branco Tiltado.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Heisenberg", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Heisenberg.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Kappa", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Kappa.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta MacGyver", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "MacGyver.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Maestria", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Maestria.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Code Life Preta", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "camiseta2.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta My Yoda", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "My Yoda.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Pato", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Pato.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Xavier School", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Xaviers School.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Yoda", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Yoda.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Quack", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Quack.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Rick And Morty 2", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Rick And Morty Captured.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Rick And Morty", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Rick And Morty.webp", Stock = 5 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Say My Name", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Say My Name.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Support", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "support.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Try Hard", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "Tryhard.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Joker Wanted", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-joker Wanted.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Joker", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Joker.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Nightmare", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Nightmare.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Ozob", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Ozob.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Rick and Morty", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Rick and Morty.jpg", Stock = 5 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Wonder Woma", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Wonder Woman.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Caneca No Coffee No Code", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca4.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Batma", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca1--batman.jpg", Stock = 5 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Vegeta", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca1-Vegeta.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Batman Preta", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Batman.jpg", Stock = 8 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Big Bang Theory", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-bbt.webp", Stock = 0 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Cogumelo", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-cogumelo.webp", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Geeks", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-Geeks.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Ironma", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 50.00M, DateAdded = DateTime.Now, Image = "caneca-ironman.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Debugar Preta", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 75.00M, DateAdded = DateTime.Now, Image = "camiseta4.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Code Life Cinza", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 99.00M, DateAdded = DateTime.Now, Image = "camiseta3.jpg", Stock = 3 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Star Bugs Coffee", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 20.00M, DateAdded = DateTime.Now, Image = "caneca1.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Programmer Code", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 15.00M, DateAdded = DateTime.Now, Image = "caneca2.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Camiseta Software Developer", Description = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Active = true, Price = 100.00M, DateAdded = DateTime.Now, Image = "camiseta1.jpg", Stock = 10 });
            await context.Products.AddAsync(new Product() { Name = "Caneca Turn Coffee in Code", Description = "Caneca de porcelana com impressão térmica de alta resistência.", Active = true, Price = 20.00M, DateAdded = DateTime.Now, Image = "caneca3.jpg", Stock = 10 });

            await context.SaveChangesAsync();
        }
    }

}
