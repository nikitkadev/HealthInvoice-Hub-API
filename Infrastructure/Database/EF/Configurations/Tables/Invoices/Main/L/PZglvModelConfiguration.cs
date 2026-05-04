using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.L;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.L;

public class PZglvModelConfiguration : IEntityTypeConfiguration<PZglvEntity>
{
    public void Configure(EntityTypeBuilder<PZglvEntity> builder)
    {
        builder.ToTable("p_zglv").HasKey(pZglv => pZglv.Uid);
        
        builder.Property(prop => prop.Uid).HasColumnName("uid").ValueGeneratedOnAdd();
        builder.Property(prop => prop.PersListUid).HasColumnName("pers_list_uid");
        builder.Property(prop => prop.Version).HasColumnName("version").HasMaxLength(5).IsRequired(false);
        builder.Property(prop => prop.Data).HasColumnName("data").IsRequired(false);
        builder.Property(prop => prop.FileName).HasColumnName("filename").HasMaxLength(128).IsRequired(false);
        builder.Property(prop => prop.FileName1).HasColumnName("filename1").HasMaxLength(128).IsRequired(false);
    }
}