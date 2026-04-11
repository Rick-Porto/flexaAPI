using flexa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace flexa.Infra.Configurations
{
    public class AppEntityConfig : IEntityTypeConfiguration<AppEntity>
    {
        public void Configure(EntityTypeBuilder<AppEntity> builder)
        {
            builder.ToTable("app_entities");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).HasMaxLength(255);
            builder.Property(a => a.Description).HasMaxLength(500);
            builder.Property(a => a.CreatedAt).IsRequired();
            builder.Property(a => a.UpdatedAt).IsRequired();
        }
    }
}