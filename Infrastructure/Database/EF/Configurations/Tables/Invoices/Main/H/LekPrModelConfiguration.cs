using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class LekPrModelConfiguration : IEntityTypeConfiguration<LekPrEntity>
{
    public void Configure(EntityTypeBuilder<LekPrEntity> builder)
    {
        builder.ToTable("lek_pr").HasKey(lekpr => lekpr.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.OnkUslUid).HasColumnName("onk_usl_uid");
        builder.Property(prop => prop.Regnum).HasColumnName("regnum").HasMaxLength(40);
        builder.Property(prop => prop.RegnumDop).HasColumnName("regnum_dop").HasMaxLength(45);
        builder.Property(prop => prop.CodeSh).HasColumnName("code_sh").HasMaxLength(10).IsRequired(false);

        builder.HasOne<OnkUslEntity>()
            .WithMany(onkusl => onkusl.LekPrs)
            .HasForeignKey(lekpr => lekpr.OnkUslUid);
    }
}