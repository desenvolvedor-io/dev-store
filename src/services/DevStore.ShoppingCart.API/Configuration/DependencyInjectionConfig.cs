using DevStore.ShoppingCart.API.Data;
using DevStore.WebAPI.Core.User;

namespace DevStore.ShoppingCart.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddScoped<ShoppingCartContext>();
            services.AddScoped<ShoppingCart>();            
        }
    }
}