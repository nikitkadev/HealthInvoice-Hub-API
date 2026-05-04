using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Additional;
using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Additional;

public class CritModelConfiguration : IEntityTypeConfiguration<CritEntity>
{
    public void Configure(EntityTypeBuilder<CritEntity> builder)
    {
        builder.ToTable("crit").HasKey(crit => crit.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.KsgKpgUid).HasColumnName("ksg_kpg_uid");
        builder.Property(prop => prop.Crit).HasColumnName("crit").HasMaxLength(100).IsRequired(false);

        builder.HasOne<KsgKpgEntity>()
            .WithMany(ksg => ksg.CritEntities)
            .HasForeignKey(crit => crit.KsgKpgUid);
    }
}