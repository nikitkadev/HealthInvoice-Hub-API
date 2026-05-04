using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class ZglvModelConfiguration : IEntityTypeConfiguration<ZglvEntity>
{
    public void Configure(EntityTypeBuilder<ZglvEntity> builder)
    {
        builder.ToTable("zglv").HasKey(zglv => zglv.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.ZlListUid).HasColumnName("zl_list_uid");
        builder.Property(prop => prop.Version).HasColumnName("version").HasMaxLength(5).IsRequired(false);
        builder.Property(prop => prop.Data).HasColumnName("data").IsRequired(false);
        builder.Property(prop => prop.FileName).HasColumnName("filename").HasMaxLength(26).IsRequired(false);
        builder.Property(prop => prop.SdZ).HasColumnName("sd_z").IsRequired(false);
    }
}