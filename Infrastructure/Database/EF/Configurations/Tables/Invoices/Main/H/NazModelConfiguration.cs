using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class NazModelConfiguration : IEntityTypeConfiguration<NazEntity>
{
    public void Configure(EntityTypeBuilder<NazEntity> builder)
    {
        builder.ToTable("naz").HasKey(naz => naz.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.SluchUId).HasColumnName("sluch_uid");
        builder.Property(prop => prop.NazN).HasColumnName("naz_n");
        builder.Property(prop => prop.NazR).HasColumnName("naz_r");
        builder.Property(prop => prop.NazIddokt).HasColumnName("naz_iddokt").HasMaxLength(25);
        builder.Property(prop => prop.NazV).HasColumnName("naz_v").IsRequired(false);
        builder.Property(prop => prop.NazUsl).HasColumnName("naz_usl").HasMaxLength(15).IsRequired(false);
        builder.Property(prop => prop.NaprDate).HasColumnName("napr_date").IsRequired(false);
        builder.Property(prop => prop.NaprMo).HasColumnName("napr_mo").HasMaxLength(6).IsRequired(false);
        builder.Property(prop => prop.NazPmp).HasColumnName("naz_pmp").IsRequired(false);
        builder.Property(prop => prop.NazPk).HasColumnName("naz_pk").HasMaxLength(3).IsRequired(false);

        builder.HasOne<SlEntity>()
            .WithMany(sl => sl.Nazs)
            .HasForeignKey(naz => naz.SluchUId);
    }
}