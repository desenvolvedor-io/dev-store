using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Thelema.Pagamentos.API.Data;
using Thelema.Pagamentos.API.Data.Repository;
using Thelema.Pagamentos.API.Models;
using Thelema.Pagamentos.API.Services;
using Thelema.Pagamentos.CardAntiCorruption;
using Thelema.Pagamentos.Facade;
using Thelema.WebAPI.Core.Usuario;

namespace Thelema.Pagamentos.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoFacade, PagamentoCartaoCreditoFacade>();

            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<PagamentosContext>();
        }
    }
}