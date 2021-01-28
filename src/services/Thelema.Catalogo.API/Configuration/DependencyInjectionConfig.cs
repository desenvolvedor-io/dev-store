using Microsoft.Extensions.DependencyInjection;
using Thelema.Catalogo.API.Data;
using Thelema.Catalogo.API.Data.Repository;
using Thelema.Catalogo.API.Models;

namespace Thelema.Catalogo.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<CatalogoContext>();
        }
    }
}