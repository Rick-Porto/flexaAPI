using flexa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace flexa.Infra.Configurations
{
    public class ScreenConfiguration : IEntityTypeConfiguration<Screen>
    {
        public void Configure(EntityTypeBuilder<Screen> builder)
        {
            builder.ToTable("screens");
            builder.HasKey(x => x.Id);

            // Nova coluna user_id — UUID do Supabase (36 chars com hífens)
            builder.Property(a => a.UserId)
                   .IsRequired()
                   .HasMaxLength(36);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CreatedAt);
            builder.Property(x => x.UpdatedAt);

            builder.HasOne(x => x.AppEntity)
                   .WithMany(x => x.Screens)
                   .HasForeignKey(x => x.AppEntityId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
