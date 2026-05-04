using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class SlModelConfiguration : IEntityTypeConfiguration<SlEntity>
{
    public void Configure(EntityTypeBuilder<SlEntity> builder)
    {
        builder.ToTable("sluch").HasKey(sluch => sluch.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.ZSlUid).HasColumnName("z_sl_uid");
        builder.Property(prop => prop.SlId).HasColumnName("sl_id").HasMaxLength(36);
        builder.Property(prop => prop.VidHmp).HasColumnName("vid_hmp").HasMaxLength(16).IsRequired(false);
        builder.Property(prop => prop.MetodHmp).HasColumnName("metod_hmp").IsRequired(false);
        builder.Property(prop => prop.Lpu1).HasColumnName("lpu_1").HasMaxLength(17).IsRequired(false);
        builder.Property(prop => prop.Podr).HasColumnName("podr").IsRequired(false);
        builder.Property(prop => prop.Profil).HasColumnName("profil");
        builder.Property(prop => prop.ProfilK).HasColumnName("profil_k").IsRequired(false);
        builder.Property(prop => prop.Det).HasColumnName("det");
        builder.Property(prop => prop.TalD).HasColumnName("tal_d").IsRequired(false);
        builder.Property(prop => prop.TalNum).HasColumnName("tal_num").HasMaxLength(20).IsRequired(false);
        builder.Property(prop => prop.TalP).HasColumnName("tal_p").IsRequired(false);
        builder.Property(prop => prop.PCel).HasColumnName("p_cel").HasMaxLength(4).IsRequired(false);
        builder.Property(prop => prop.Mop).HasColumnName("mop").HasMaxLength(3).IsRequired(false);
        builder.Property(prop => prop.NHistory).HasColumnName("nhistory").HasMaxLength(50);
        builder.Property(prop => prop.PPer).HasColumnName("p_per").IsRequired(false);
        builder.Property(prop => prop.Date1).HasColumnName("date_1");
        builder.Property(prop => prop.Date2).HasColumnName("date_2");
        builder.Property(prop => prop.KD).HasColumnName("kd").IsRequired(false);
        builder.Property(prop => prop.Wei).HasColumnName("wei").HasPrecision(4, 1).IsRequired(false);
        builder.Property(prop => prop.Ds0).HasColumnName("ds0").HasMaxLength(10).IsRequired(false);
        builder.Property(prop => prop.Ds1).HasColumnName("ds1").HasMaxLength(10);
        builder.Property(prop => prop.Ds1_Pr).HasColumnName("ds1_pr").IsRequired(false);
        builder.Property(prop => prop.CZab).HasColumnName("c_zab").IsRequired(false);
        builder.Property(prop => prop.DsOnk).HasColumnName("ds_onk").IsRequired(false);
        builder.Property(prop => prop.PrDN).HasColumnName("pr_d_n").IsRequired(false);
        builder.Property(prop => prop.ProfM).HasColumnName("prof_m").IsRequired(false);
        builder.Property(prop => prop.Dn).HasColumnName("dn").IsRequired(false);
        builder.Property(prop => prop.CodeMes1).HasColumnName("code_mes1").HasMaxLength(20).IsRequired(false);
        builder.Property(prop => prop.CodeMes2).HasColumnName("code_mes2").HasMaxLength(20).IsRequired(false);
        builder.Property(prop => prop.Reab).HasColumnName("reab").IsRequired(false);
        builder.Property(prop => prop.Prvs).HasColumnName("prvs");
        builder.Property(prop => prop.VersSpec).HasColumnName("vers_spec").HasMaxLength(4);
        builder.Property(prop => prop.Iddokt).HasColumnName("iddokt").HasMaxLength(25);
        builder.Property(prop => prop.EdCol).HasColumnName("ed_col").IsRequired(false);
        builder.Property(prop => prop.Tarif).HasColumnName("tarif").IsRequired(false);
        builder.Property(prop => prop.SumM).HasColumnName("sum_m");
        builder.Property(prop => prop.SmoSump).HasColumnName("smo_sump").IsRequired(false);
        builder.Property(prop => prop.Comentsl).HasColumnName("comentsl").HasMaxLength(250).IsRequired(false);
        builder.Property(prop => prop.SankMek).HasColumnName("sank_mek").IsRequired(false);
        builder.Property(prop => prop.SankMee).HasColumnName("sank_mee").IsRequired(false);
        builder.Property(prop => prop.SankEkmp).HasColumnName("sank_ekmp").IsRequired(false);
        builder.Property(prop => prop.LpuLevel).HasColumnName("lpu_level").HasMaxLength(2).IsRequired(false);

        builder.HasOne(sluch => sluch.OnkSl)
            .WithOne()
            .HasForeignKey<OnkSlEntity>(KsgKpg => KsgKpg.SluchUid);

        builder.HasOne(sluch => sluch.KsgKpg)
            .WithOne()
            .HasForeignKey<KsgKpgEntity>(KsgKpg => KsgKpg.SluchUid);

        builder.HasOne<ZSlEntity>()
            .WithMany(zsl => zsl.Sls)
            .HasForeignKey(sl => sl.ZSlUid);
    }
}