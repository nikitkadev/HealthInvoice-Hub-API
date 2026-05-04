using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.L;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.L;

public class PersListModelConfiguration : IEntityTypeConfiguration<PersListEntity>
{
    public void Configure(EntityTypeBuilder<PersListEntity> builder)
    {
        builder.ToTable("pers_list").HasKey(persList => persList.Uid);

        builder.Property(prop => prop.Uid).HasColumnName("uid").ValueGeneratedOnAdd();
        builder.Property(prop => prop.UploadDate).HasColumnName("uploaddate");
        builder.Property(prop => prop.Uploader).HasColumnName("uploader").HasMaxLength(64).IsRequired(false);
        builder.Property(prop => prop.Status).HasColumnName("status").IsRequired(false);

        builder.HasOne(persList => persList.PZglv)
            .WithOne()
            .HasForeignKey<PZglvEntity>(pZglv => pZglv.PersListUid);
    }
}