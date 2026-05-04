using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class LekDoseModelConfiguration : IEntityTypeConfiguration<LekDoseEntity>
{
    public void Configure(EntityTypeBuilder<LekDoseEntity> builder)
    {
        builder.ToTable("lek_dose").HasKey(lek => lek.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.SluchLekPrUid).HasColumnName("sluch_lek_pr_uid");
        builder.Property(prop => prop.EdIzm).HasColumnName("ed_izm").HasMaxLength(3);
        builder.Property(prop => prop.DoseInj).HasColumnName("dose_inj").HasPrecision(7, 2);
        builder.Property(prop => prop.MethodInj).HasColumnName("method_inj").HasMaxLength(3);
        builder.Property(prop => prop.ColInj).HasColumnName("col_inj");
    }
}