using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class ZSlModelConfiguration : IEntityTypeConfiguration<ZSlEntity>
{
    public void Configure(EntityTypeBuilder<ZSlEntity> builder)
    {
        builder.ToTable("z_sl").HasKey(zsl => zsl.Uid);
        
        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.ZapUid).HasColumnName("zap_uid");
        builder.Property(prop => prop.Idcase).HasColumnName("idcase");
        builder.Property(prop => prop.UslOk).HasColumnName("usl_ok");
        builder.Property(prop => prop.VidPom).HasColumnName("vidpom");
        builder.Property(prop => prop.ForPom).HasColumnName("for_pom");
        builder.Property(prop => prop.NprMo).HasColumnName("npr_mo").HasMaxLength(6).IsRequired(false);
        builder.Property(prop => prop.NprDate).HasColumnName("npr_date").IsRequired(false);
        builder.Property(prop => prop.NprNum).HasColumnName("npr_num").HasMaxLength(100).IsRequired(false);
        builder.Property(prop => prop.Lpu).HasColumnName("lpu").HasMaxLength(6);
        builder.Property(prop => prop.Vbr).HasColumnName("vbr").IsRequired(false);
        builder.Property(prop => prop.EveningTime).HasColumnName("evening_time").IsRequired(false);
        builder.Property(prop => prop.DateZ1).HasColumnName("date_z_1");
        builder.Property(prop => prop.DateZ2).HasColumnName("date_z_2");
        builder.Property(prop => prop.POtk).HasColumnName("p_otk").IsRequired(false);
        builder.Property(prop => prop.RsltD).HasColumnName("rslt_d").IsRequired(false);
        builder.Property(prop => prop.KdZ).HasColumnName("kd_z").IsRequired(false);
        builder.Property(prop => prop.VnovM).HasColumnName("vnov_m").IsRequired(false);
        builder.Property(prop => prop.Rslt).HasColumnName("rslt");
        builder.Property(prop => prop.Ishod).HasColumnName("ishod");
        builder.Property(prop => prop.OsSluch).HasColumnName("os_sluch").IsRequired(false);
        builder.Property(prop => prop.VbP).HasColumnName("vb_p").IsRequired(false);
        builder.Property(prop => prop.Idsp).HasColumnName("idsp");            ;
        builder.Property(prop => prop.SumV).HasColumnName("sumv");
        builder.Property(prop => prop.Oplata).HasColumnName("oplata").HasDefaultValue((byte)2).IsRequired(false);
        builder.Property(prop => prop.Sump).HasColumnName("sump").HasDefaultValue(0m).IsRequired(false);
        builder.Property(prop => prop.SmoSump).HasColumnName("smo_sump").HasDefaultValue(0m).IsRequired(false);
        builder.Property(prop => prop.SankIt).HasColumnName("sank_it").IsRequired(false);
        builder.Property(prop => prop.SmoSankIt).HasColumnName("smo_sank_it").IsRequired(false);
    }
}