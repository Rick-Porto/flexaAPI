// flexa.Api/Extensions/DependencyInjectionExtension.cs
using flexa.App.Interfaces;
using flexa.App.Services;
using flexa.Domain.Interfaces;
using flexa.Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace flexa.Api.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repository registrations (existing)
            services.AddScoped<IAppEntityRepository, AppEntityRepository>();
            services.AddScoped<IAppEntityService, AppEntityService>();
            // Screen registrations
            services.AddScoped<IScreenRepository, ScreenRepository>();
            services.AddScoped<IScreenService, ScreenService>();
            // Component registrations
            services.AddScoped<IComponentRepository, ComponentRepository>();
            services.AddScoped<IComponentService, ComponentService>();
            return services;
        }
    }
}
