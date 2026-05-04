using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class BDiagModelConfiguration : IEntityTypeConfiguration<BDiagEntity>
{
    public void Configure(EntityTypeBuilder<BDiagEntity> builder)
    {
        builder.ToTable("b_diag").HasKey(bdiag => bdiag.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.OnkSluchUid).HasColumnName("onk_sl_uid");
        builder.Property(prop => prop.DiagDate).HasColumnName("diag_date").IsRequired(false);
        builder.Property(prop => prop.DiagTip).HasColumnName("diag_tip").IsRequired(false);
        builder.Property(prop => prop.DiagCode).HasColumnName("diag_code").IsRequired(false);
        builder.Property(prop => prop.DiagRslt).HasColumnName("diag_rslt").IsRequired(false);
        builder.Property(prop => prop.RecRslt).HasColumnName("rec_rslt").IsRequired(false);

        builder.HasOne<OnkSlEntity>()
            .WithMany(onksl => onksl.BDiags)
            .HasForeignKey(bdiag => bdiag.OnkSluchUid);
    }
}