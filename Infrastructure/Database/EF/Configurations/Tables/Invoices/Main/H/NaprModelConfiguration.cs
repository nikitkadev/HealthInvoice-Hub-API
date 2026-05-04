using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class NaprModelConfiguration : IEntityTypeConfiguration<NaprEntity>
{
    public void Configure(EntityTypeBuilder<NaprEntity> builder)
    {
        builder.ToTable("napr").HasKey(napr => napr.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.SluchUId).HasColumnName("sluch_uid");
        builder.Property(prop => prop.NaprDate).HasColumnName("napr_date");
        builder.Property(prop => prop.NaprMo).HasColumnName("napr_mo").HasMaxLength(6).IsRequired(false);
        builder.Property(prop => prop.NaprV).HasColumnName("napr_v");
        builder.Property(prop => prop.MetIssl).HasColumnName("met_issl").IsRequired(false);
        builder.Property(prop => prop.NaprUsl).HasColumnName("napr_usl").HasMaxLength(15).IsRequired(false);

        builder.HasOne<SlEntity>()
            .WithMany(sl => sl.Naprs)
            .HasForeignKey(napr => napr.SluchUId);
    }
}