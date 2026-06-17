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

            // Nova coluna user_id — UUID do Supabase (36 chars com hífens)
            builder.Property(a => a.UserId)
                   .IsRequired()
                   .HasMaxLength(36);

            builder.Property(a => a.Name).HasMaxLength(255);
            builder.Property(a => a.Description).HasMaxLength(500);
            builder.Property(a => a.CreatedAt).IsRequired();
            builder.Property(a => a.UpdatedAt).IsRequired();

            // Public link for sharing/publishing the app
            builder.Property(x => x.PublicLink)
                    .HasMaxLength(100);

            builder.HasIndex(x => x.PublicLink)
                   .IsUnique();
        }
    }
}