using flexa.App.Interfaces;
using flexa.App.Services;
using flexa.Domain.Interfaces;
using flexa.Infra.Repositories;

namespace flexa.Api.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repository registrations
            services.AddScoped<IAppEntityRepository, AppEntityRepository>();
            services.AddScoped<IAppEntityService, AppEntityService>();

            // Screen registrations
            services.AddScoped<IScreenRepository, ScreenRepository>();
            services.AddScoped<IScreenService, ScreenService>();

            // Component registrations
            services.AddScoped<IComponentRepository, ComponentRepository>();
            services.AddScoped<IComponentService, ComponentService>();

            // DataEntry
            services.AddScoped<IDataEntryRepository, DataEntryRepository>();
            services.AddScoped<IDataEntryService, DataEntryService>();

            return services;
        }
    }
}
