using flexa.Domain.Interfaces;
using flexa.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace flexa.Api.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAppEntityRepository, AppEntityRepository>();
            // Add any other necessary DI registrations here
            return services;
        }
    }
}