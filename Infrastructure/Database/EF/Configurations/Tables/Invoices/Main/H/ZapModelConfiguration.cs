using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class ZapModelConfiguration : IEntityTypeConfiguration<ZapEntity>
{
    public void Configure(EntityTypeBuilder<ZapEntity> builder)
    {
        builder.ToTable("zap").HasKey(zap => zap.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.ZlListUid).HasColumnName("zl_list_uid");
        builder.Property(prop => prop.PacientUid).HasColumnName("pacient");
        builder.Property(prop => prop.NZap).HasColumnName("n_zap");
        builder.Property(prop => prop.PrNov).HasColumnName("pr_nov");

        builder.HasOne<ZlListEntity>()
            .WithMany(zllist => zllist.Zaps)
            .HasForeignKey(zap => zap.ZlListUid);

        builder.HasOne(zap => zap.Pacient)
            .WithOne()
            .HasForeignKey<ZapEntity>(zap => zap.PacientUid);

        builder.HasOne(zap => zap.ZSl)
            .WithOne()
            .HasForeignKey<ZSlEntity>(zsl => zsl.ZapUid);
    }
}
