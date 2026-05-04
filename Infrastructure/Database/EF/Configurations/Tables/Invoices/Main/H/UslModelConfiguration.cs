using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class UslModelConfiguration : IEntityTypeConfiguration<UslEntity>
{
    public void Configure(EntityTypeBuilder<UslEntity> builder)
    {
        builder.ToTable("usl").HasKey(usl => usl.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.SluchUid).HasColumnName("sluch_uid");
        builder.Property(prop => prop.Idserv).HasColumnName("idserv").HasMaxLength(36);
        builder.Property(prop => prop.Lpu).HasColumnName("lpu").HasMaxLength(6);
        builder.Property(prop => prop.Lpu1).HasColumnName("lpu_1").HasMaxLength(17);
        builder.Property(prop => prop.Podr).HasColumnName("podr").IsRequired(false);
        builder.Property(prop => prop.Profil).HasColumnName("profil");
        builder.Property(prop => prop.VidVme).HasColumnName("vid_vme").HasMaxLength(15).IsRequired(false);
        builder.Property(prop => prop.Det).HasColumnName("det");
        builder.Property(prop => prop.DateIn).HasColumnName("date_in");
        builder.Property(prop => prop.DateOut).HasColumnName("date_out");
        builder.Property(prop => prop.POtk).HasColumnName("p_otk").IsRequired(false);
        builder.Property(prop => prop.DS).HasColumnName("ds").HasMaxLength(10);
        builder.Property(prop => prop.CodeUsl).HasColumnName("code_usl").HasMaxLength(20);
        builder.Property(prop => prop.KolUsl).HasColumnName("kol_usl");
        builder.Property(prop => prop.Tarif).HasColumnName("tarif").IsRequired(false);
        builder.Property(prop => prop.SumvUsl).HasColumnName("sumv_usl");
        builder.Property(prop => prop.PRVS).HasColumnName("prvs");
        builder.Property(prop => prop.CodeMd).HasColumnName("code_md").HasMaxLength(25);
        builder.Property(prop => prop.NPL).HasColumnName("npl").IsRequired(false);
        builder.Property(prop => prop.Comentu).HasColumnName("comentu").HasMaxLength(255).IsRequired(false);
        builder.Property(prop => prop.VolumeCode).HasColumnName("VolumeCode").HasMaxLength(10).IsRequired(false);

        builder.HasOne<SlEntity>()
            .WithMany(sl => sl.Usls)
            .HasForeignKey(usl => usl.SluchUid);

        builder.HasOne(usl => usl.UslDopParams)
            .WithOne()
            .HasForeignKey<UslDopParamEntity>(usldop => usldop.UslUid);
    }
}