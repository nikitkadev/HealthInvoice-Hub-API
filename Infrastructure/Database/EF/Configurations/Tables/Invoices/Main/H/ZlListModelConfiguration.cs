using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class ZlListModelConfiguration : IEntityTypeConfiguration<ZlListEntity>
{
    public void Configure(EntityTypeBuilder<ZlListEntity> builder)
    {
        builder.ToTable("zl_list").HasKey(zllist => zllist.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.UploadDate).HasColumnName("uploaddate");
        builder.Property(prop => prop.Uploader).HasColumnName("uploader").HasMaxLength(64).IsRequired(false);
        builder.Property(prop => prop.Status).HasColumnName("status");

        builder.HasOne(zllist => zllist.Zglv)
            .WithOne()
            .HasForeignKey<ZglvEntity>(zglv => zglv.ZlListUid);

        builder.HasOne(zllist => zllist.Schet)
            .WithOne()
            .HasForeignKey<SchetEntity>(schet => schet.ZlListUid);
    }
}
