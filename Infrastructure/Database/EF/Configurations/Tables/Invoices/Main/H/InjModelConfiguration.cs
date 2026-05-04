using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class InjModelConfiguration : IEntityTypeConfiguration<InjEntity>
{
    public void Configure(EntityTypeBuilder<InjEntity> builder)
    {
        builder.ToTable("inj").HasKey(inj => inj.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.LekPrUid).HasColumnName("lek_pr_uid");
        builder.Property(prop => prop.DateInj).HasColumnName("date_inj");
        builder.Property(prop => prop.KvInj).HasColumnName("kv_inj").HasPrecision(8, 3).IsRequired(false);
        builder.Property(prop => prop.KizInj).HasColumnName("kiz_inj").HasPrecision(8, 3).IsRequired(false);
        builder.Property(prop => prop.SInj).HasColumnName("s_inj").IsRequired(false);
        builder.Property(prop => prop.SvInj).HasColumnName("sv_inj").IsRequired(false);
        builder.Property(prop => prop.SizInj).HasColumnName("siz_inj").IsRequired(false);
        builder.Property(prop => prop.RedInj).HasColumnName("red_inj").IsRequired(false);

        builder.HasOne<LekPrEntity>()
            .WithMany(lekpr => lekpr.Injs)
            .HasForeignKey(inj => inj.LekPrUid);
    }
}