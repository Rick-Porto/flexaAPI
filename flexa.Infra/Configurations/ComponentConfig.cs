using flexa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace flexa.Infra.Configurations
{
    public class ComponentConfiguration : IEntityTypeConfiguration<Component>
    {
        public void Configure(EntityTypeBuilder<Component> builder)
        {
            builder.ToTable("components");
            builder.HasKey(x => x.Id);

            // Nova coluna user_id — UUID do Supabase (36 chars com hífens)
            builder.Property(a => a.UserId)
                   .IsRequired()
                   .HasMaxLength(36);

            builder.Property(x => x.ElementType).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Label).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Model).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Config).HasColumnType("jsonb");
            builder.Property(x => x.Order).IsRequired();
            builder.Property(x => x.Required).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder.HasOne(x => x.Screen)
                   .WithMany(x => x.Components)
                   .HasForeignKey(x => x.ScreenId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
