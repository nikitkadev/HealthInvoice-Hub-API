using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class SankModelConfiguration : IEntityTypeConfiguration<SankEntity>
{
    public void Configure(EntityTypeBuilder<SankEntity> builder)
    {
        builder.ToTable("sank").HasKey(slkekpr => slkekpr.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.SluchUid).HasColumnName("sluch_uid");
        builder.Property(prop => prop.SCode).HasColumnName("s_code").HasMaxLength(36);
        builder.Property(prop => prop.SSum).HasColumnName("s_sum");
        builder.Property(prop => prop.STip).HasColumnName("s_tip").HasMaxLength(3);
        builder.Property(prop => prop.SOsn).HasColumnName("s_osn");
        builder.Property(prop => prop.SEDCol).HasColumnName("s_ed_col");
        builder.Property(prop => prop.DateAct).HasColumnName("date_act");
        builder.Property(prop => prop.NumAct).HasColumnName("num_act").HasMaxLength(30);
        builder.Property(prop => prop.CodeExp).HasColumnName("code_exp").HasMaxLength(10).IsRequired(false);
        builder.Property(prop => prop.SCom).HasColumnName("s_com").HasMaxLength(250).IsRequired(false);
        builder.Property(prop => prop.SIst).HasColumnName("s_ist");

        builder.HasOne<SlEntity>()
            .WithMany(sl => sl.Sanks)
            .HasForeignKey(sank => sank.SluchUid);
    }
}