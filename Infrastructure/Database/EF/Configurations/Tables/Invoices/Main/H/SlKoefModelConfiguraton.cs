using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class SlKoefModelConfiguraton : IEntityTypeConfiguration<SlKoefEntity>
{
    public void Configure(EntityTypeBuilder<SlKoefEntity> builder)
    {
        builder.ToTable("sl_koef").HasKey(koef => koef.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.KsgKpgUid).HasColumnName("ksg_kpg_uid");
        builder.Property(prop => prop.Idsl).HasColumnName("idsl").HasMaxLength(5).IsRequired(false);
        builder.Property(prop => prop.ZSl).HasColumnName("z_sl").HasPrecision(6, 5);

        builder.HasOne<KsgKpgEntity>()
            .WithMany(ksg => ksg.SlKoefs)
            .HasForeignKey(koef => koef.KsgKpgUid);
    }
}