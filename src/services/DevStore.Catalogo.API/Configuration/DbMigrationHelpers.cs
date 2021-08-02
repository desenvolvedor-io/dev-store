using DevStore.Catalogo.API.Data;
using DevStore.Catalogo.API.Models;
using DevStore.WebAPI.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevStore.Catalogo.API.Configuration
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
            var context = scope.ServiceProvider.GetRequiredService<CatalogoContext>();

            await DbHealthChecker.TestConnection(context);

            if (env.IsDevelopment())
            {
                await context.Database.EnsureCreatedAsync();
                await EnsureSeedProducts(context);
            }
        }

        private static async Task EnsureSeedProducts(CatalogoContext context)
        {
            if (context.Produtos.Any())
                return;

            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta 4 Head", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "4head.webp", QuantidadeEstoque = 5 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta 4 Head Branca", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Branca 4head.webp", QuantidadeEstoque = 5 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Tiltado", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "tiltado.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Tiltado Branca", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Branco Tiltado.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Heisenberg", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Heisenberg.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Kappa", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Kappa.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta MacGyver", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "MacGyver.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Maestria", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Maestria.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Code Life Preta", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "camiseta2.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta My Yoda", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "My Yoda.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Pato", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Pato.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Xavier School", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Xaviers School.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Yoda", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Yoda.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Quack", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Quack.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Rick And Morty 2", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Rick And Morty Captured.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Rick And Morty", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Rick And Morty.webp", QuantidadeEstoque = 5 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Say My Name", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Say My Name.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Support", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "support.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Try Hard", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "Tryhard.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Joker Wanted", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca-joker Wanted.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Joker", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca-Joker.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Nightmare", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca-Nightmare.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Ozob", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca-Ozob.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Rick and Morty", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca-Rick and Morty.jpg", QuantidadeEstoque = 5 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Wonder Woma", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca-Wonder Woman.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca No Coffee No Code", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca4.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Batma", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca1--batman.jpg", QuantidadeEstoque = 5 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Vegeta", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca1-Vegeta.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Batman Preta", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca-Batman.jpg", QuantidadeEstoque = 8 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Big Bang Theory", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca-bbt.webp", QuantidadeEstoque = 0 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Cogumelo", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca-cogumelo.webp", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Geeks", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca-Geeks.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Ironma", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 50.00M, DataCadastro = DateTime.Now, Imagem = "caneca-ironman.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Debugar Preta", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 75.00M, DataCadastro = DateTime.Now, Imagem = "camiseta4.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Code Life Cinza", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 99.00M, DataCadastro = DateTime.Now, Imagem = "camiseta3.jpg", QuantidadeEstoque = 3 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Star Bugs Coffee", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 20.00M, DataCadastro = DateTime.Now, Imagem = "caneca1.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Programmer Code", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 15.00M, DataCadastro = DateTime.Now, Imagem = "caneca2.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Camiseta Software Developer", Descricao = "Camiseta 100% algodão, resistente a lavagens e altas temperaturas.", Ativo = true, Valor = 100.00M, DataCadastro = DateTime.Now, Imagem = "camiseta1.jpg", QuantidadeEstoque = 10 });
            await context.Produtos.AddAsync(new Produto() { Nome = "Caneca Turn Coffee in Code", Descricao = "Caneca de porcelana com impressão térmica de alta resistência.", Ativo = true, Valor = 20.00M, DataCadastro = DateTime.Now, Imagem = "caneca3.jpg", QuantidadeEstoque = 10 });

            await context.SaveChangesAsync();
        }
    }

}
