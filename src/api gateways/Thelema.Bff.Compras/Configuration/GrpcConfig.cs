using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Thelema.Bff.Compras.Services.gRPC;
using Thelema.Carrinho.API.Services.gRPC;
using Thelema.WebAPI.Core.Extensions;

namespace Thelema.Bff.Compras.Configuration
{
    public static class GrpcConfig
    {
        public static void ConfigureGrpcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GrpcServiceInterceptor>();

            services.AddScoped<ICarrinhoGrpcService, CarrinhoGrpcService>();

            services.AddGrpcClient<CarrinhoCompras.CarrinhoComprasClient>(options =>
            {
                options.Address = new Uri(configuration["CarrinhoUrl"]);
            })
                .AddInterceptor<GrpcServiceInterceptor>()
                .AllowSelfSignedCertificate();
        }
    }
}