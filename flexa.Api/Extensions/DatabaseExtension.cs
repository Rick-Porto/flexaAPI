using flexa.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace flexa.Api.Extensions
{
    public static class DatabaseExtension
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("Supabase"),
                    b => b.MigrationsAssembly("flexa.Infra")));
        }
    }
}
