using flexa.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace flexa.Infra.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<App> Apps { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<DataEntry> DataEntries { get; set; }
    public DbSet<Screen> Screens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
