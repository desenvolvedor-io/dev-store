using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Thelema.Carrinho.API.Data;
using Thelema.WebAPI.Core.Usuario;

namespace Thelema.Carrinho.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddScoped<CarrinhoContext>();
        }
    }
}