using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class PacientModelConfiguration : IEntityTypeConfiguration<PacientEntity>
{
    public void Configure(EntityTypeBuilder<PacientEntity> builder)
    {
        builder.ToTable("pacient").HasKey(pacient => pacient.Uid);
        
        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.IdPac).HasColumnName("id_pac").HasMaxLength(36);
        builder.Property(prop => prop.Vpolis).HasColumnName("vpolis");
        builder.Property(prop => prop.Spolis).HasColumnName("spolis").HasMaxLength(10).IsRequired(false);
        builder.Property(prop => prop.Npolis).HasColumnName("npolis").HasMaxLength(20);
        builder.Property(prop => prop.Enp).HasColumnName("enp").HasMaxLength(16).IsRequired(false);
        builder.Property(prop => prop.StOkato).HasColumnName("st_okato").HasMaxLength(5).IsRequired(false);
        builder.Property(prop => prop.Smo).HasColumnName("smo").HasMaxLength(5).IsRequired(false);
        builder.Property(prop => prop.SmoOgrn).HasColumnName("smo_ogrn").HasMaxLength(15).IsRequired(false);
        builder.Property(prop => prop.SmoOk).HasColumnName("smo_ok").HasMaxLength(5).IsRequired(false);
        builder.Property(prop => prop.SmoNam).HasColumnName("smo_nam").HasMaxLength(100).IsRequired(false);
        builder.Property(prop => prop.Inv).HasColumnName("inv").IsRequired(false);
        builder.Property(prop => prop.Mse).HasColumnName("mse").IsRequired(false);
        builder.Property(prop => prop.Novor).HasColumnName("novor").HasMaxLength(9);
        builder.Property(prop => prop.VnovD).HasColumnName("vnov_d").IsRequired(false);
        builder.Property(prop => prop.Soc).HasColumnName("soc").HasMaxLength(3);
        builder.Property(prop => prop.NextD).HasColumnName("next_d").IsRequired(false);
        builder.Property(prop => prop.MoPr).HasColumnName("mo_pr").HasMaxLength(6).IsRequired(false);
        builder.Property(prop => prop.VZ).HasColumnName("vz").HasMaxLength(2).IsRequired(false);
    }
}