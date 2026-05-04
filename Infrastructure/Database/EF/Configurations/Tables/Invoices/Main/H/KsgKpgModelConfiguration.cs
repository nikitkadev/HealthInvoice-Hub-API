using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class KsgKpgModelConfiguration : IEntityTypeConfiguration<KsgKpgEntity>
{
    public void Configure(EntityTypeBuilder<KsgKpgEntity> builder)
    {
        builder.ToTable("ksg_kpg").HasKey(ksg => ksg.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.SluchUid).HasColumnName("sluch_uid");
        builder.Property(prop => prop.NKsg).HasColumnName("n_ksg").HasMaxLength(20);
        builder.Property(prop => prop.VerKsg).HasColumnName("ver_ksg");
        builder.Property(prop => prop.KsgPg).HasColumnName("ksg_pg");
        builder.Property(prop => prop.NKpg).HasColumnName("n_kpg").HasMaxLength(4).IsRequired(false);
        builder.Property(prop => prop.KoefZ).HasColumnName("koef_z").HasPrecision(8, 5);
        builder.Property(prop => prop.KoefUp).HasColumnName("koef_up").HasPrecision(7, 5);
        builder.Property(prop => prop.Bztsz).HasColumnName("bztsz");
        builder.Property(prop => prop.KoefD).HasColumnName("koef_d").HasPrecision(7, 5);
        builder.Property(prop => prop.KoefU).HasColumnName("koef_u").HasPrecision(7, 5);
        builder.Property(prop => prop.KZp).HasColumnName("k_zp").HasPrecision(8, 5).IsRequired(false);
        builder.Property(prop => prop.SlK).HasColumnName("sl_k");
        builder.Property(prop => prop.ItSl).HasColumnName("it_sl").HasPrecision(6, 5).IsRequired(false);
        builder.Property(prop => prop.PrPr).HasColumnName("pr_pr").HasMaxLength(2);
        builder.Property(prop => prop.KoefPr).HasColumnName("koef_pr").HasPrecision(6, 5).IsRequired(false);
    }
}