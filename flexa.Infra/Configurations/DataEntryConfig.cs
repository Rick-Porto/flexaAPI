using flexa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace flexa.Infra.Configurations
{
    public class DataEntryConfiguration : IEntityTypeConfiguration<DataEntry>
    {
        public void Configure(EntityTypeBuilder<DataEntry> builder)
        {
            builder.ToTable("data_entries");
            builder.HasKey(x => x.Id);

            // ✅ SEM UserId — ownership verificado via JOIN Screen → AppEntity.UserId
            builder.Property(x => x.Data).IsRequired().HasColumnType("jsonb").HasDefaultValue("{}");
            builder.Property(x => x.Config).HasColumnType("jsonb");
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder.HasOne(x => x.Screen)
                   .WithMany(x => x.DataEntries)
                   .HasForeignKey(x => x.ScreenId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Component)
                   .WithMany(x => x.DataEntries)
                   .HasForeignKey(x => x.ComponentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
