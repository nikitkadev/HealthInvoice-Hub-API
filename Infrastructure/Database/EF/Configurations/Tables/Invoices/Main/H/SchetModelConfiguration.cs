using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class SchetModelConfiguration : IEntityTypeConfiguration<SchetEntity>
{
    public void Configure(EntityTypeBuilder<SchetEntity> builder)
    {
        builder.ToTable("schet").HasKey(schet => schet.Uid);
        
        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.ZlListUid).HasColumnName("zl_list_uid");
        builder.Property(prop => prop.Code).HasColumnName("code");
        builder.Property(prop => prop.CodeMO).HasColumnName("code_mo").HasMaxLength(6);
        builder.Property(prop => prop.Year).HasColumnName("year");
        builder.Property(prop => prop.Month).HasColumnName("month");
        builder.Property(prop => prop.Nschet).HasColumnName("nschet").HasMaxLength(15);
        builder.Property(prop => prop.Dschet).HasColumnName("dschet");
        builder.Property(prop => prop.Plat).HasColumnName("plat").HasMaxLength(5).IsRequired(false);
        builder.Property(prop => prop.Summav).HasColumnName("summav");
        builder.Property(prop => prop.Coments).HasColumnName("coments").HasMaxLength(250).IsRequired(false);
        builder.Property(prop => prop.Summap).HasColumnName("summap").IsRequired(false);
        builder.Property(prop => prop.SankMek).HasColumnName("sank_mek").IsRequired(false);
        builder.Property(prop => prop.SankMee).HasColumnName("sank_mee").IsRequired(false);
        builder.Property(prop => prop.SankEkmp).HasColumnName("sank_ekmp").IsRequired(false);
        builder.Property(prop => prop.SmoSummap).HasColumnName("smo_summap").IsRequired(false);
        builder.Property(prop => prop.SmoSankMek).HasColumnName("smo_sank_mek").IsRequired(false);
        builder.Property(prop => prop.SmoSankMee).HasColumnName("smo_sank_mee").IsRequired(false);
        builder.Property(prop => prop.SmoSankEkmp).HasColumnName("smo_sank_ekmp").IsRequired(false);
        builder.Property(prop => prop.Disp).HasColumnName("disp").HasMaxLength(3).IsRequired(false);
    }
}