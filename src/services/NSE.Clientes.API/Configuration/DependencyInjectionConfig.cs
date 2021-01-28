using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSE.Clientes.API.Application.Commands;
using NSE.Clientes.API.Application.Events;
using NSE.Clientes.API.Data;
using NSE.Clientes.API.Data.Repository;
using NSE.Clientes.API.Models;
using NSE.Core.Mediator;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Clientes.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            //services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.TryAddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
            services.TryAddScoped<IRequestHandler<AdicionarEnderecoCommand, ValidationResult>, ClienteCommandHandler>();

            services.TryAddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ClientesContext>();
        }
    }
}