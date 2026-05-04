using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class BProtModelConfiguration : IEntityTypeConfiguration<BProtEntity>
{
    public void Configure(EntityTypeBuilder<BProtEntity> builder)
    {
        builder.ToTable("b_prot").HasKey(bprot => bprot.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.OnkSluchUid).HasColumnName("onk_sl_uid");
        builder.Property(prop => prop.Prot).HasColumnName("prot");
        builder.Property(prop => prop.DProt).HasColumnName("d_prot");

        builder.HasOne<OnkSlEntity>()
            .WithMany(onksl => onksl.BProts)
            .HasForeignKey(bprot => bprot.OnkSluchUid);
    }
}