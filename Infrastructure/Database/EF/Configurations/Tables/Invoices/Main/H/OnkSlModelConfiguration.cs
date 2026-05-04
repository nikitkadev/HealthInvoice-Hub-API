using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class OnkSlModelConfiguration : IEntityTypeConfiguration<OnkSlEntity>
{
    public void Configure(EntityTypeBuilder<OnkSlEntity> builder)
    {
        builder.ToTable("onk_sl").HasKey(onksl => onksl.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.SluchUid).HasColumnName("sluch_uid");
        builder.Property(prop => prop.Ds1T).HasColumnName("ds1_t").IsRequired(false);
        builder.Property(prop => prop.Stad).HasColumnName("stad").IsRequired(false);
        builder.Property(prop => prop.OnkT).HasColumnName("onk_t").IsRequired(false);
        builder.Property(prop => prop.OnkN).HasColumnName("onk_n").IsRequired(false);
        builder.Property(prop => prop.OnkM).HasColumnName("onk_m").IsRequired(false);
        builder.Property(prop => prop.Mtstz).HasColumnName("mtstz").IsRequired(false);
        builder.Property(prop => prop.Sod).HasColumnName("sod").IsRequired(false);
        builder.Property(prop => prop.KFr).HasColumnName("k_fr").IsRequired(false);
        builder.Property(prop => prop.Wei).HasColumnName("wei").IsRequired(false);
        builder.Property(prop => prop.Hei).HasColumnName("hei").IsRequired(false);
        builder.Property(prop => prop.Bsa).HasColumnName("bsa").IsRequired(false);
    }
}