using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class SlLekPrModelConfiguration : IEntityTypeConfiguration<SlLekPrEntity>
{
    public void Configure(EntityTypeBuilder<SlLekPrEntity> builder)
    {
        builder.ToTable("sluch_lek_pr").HasKey(slkekpr => slkekpr.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.SluchUid).HasColumnName("sluch_uid");
        builder.Property(prop => prop.DataInj).HasColumnName("data_inj");
        builder.Property(prop => prop.CodeSh).HasColumnName("code_sh").HasMaxLength(10);
        builder.Property(prop => prop.Regnum).HasColumnName("regnum").HasMaxLength(6).IsRequired(false);
        builder.Property(prop => prop.CodMark).HasColumnName("cod_mark").HasMaxLength(100).IsRequired(false);

        builder.HasOne<SlEntity>()
            .WithMany(sl => sl.SluchLekPrs)
            .HasForeignKey(lekpr => lekpr.SluchUid);

        builder.HasOne(sllekpr => sllekpr.LekDose)
            .WithOne()
            .HasForeignKey<LekDoseEntity>(lekdose => lekdose.SluchLekPrUid);
    }
}