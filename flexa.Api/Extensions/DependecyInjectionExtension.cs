using flexa.App.Interfaces;
using flexa.App.Services;
using flexa.Domain.Interfaces;
using flexa.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace flexa.Api.Extensions
{
    public static class DependecyInjectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAppEntityRepository, AppEntityRepository>();
            services.AddScoped<IAppEntityService, AppEntityService>();
            // Add any other necessary DI registrations here
            return services;
        }
    }
}