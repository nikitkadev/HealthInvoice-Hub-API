using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class OnkUslModelConfiguration : IEntityTypeConfiguration<OnkUslEntity>
{
    public void Configure(EntityTypeBuilder<OnkUslEntity> builder)
    {
        builder.ToTable("onk_usl").HasKey(onkusl => onkusl.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.OnkSluchUid).HasColumnName("onk_sl_uid");
        builder.Property(prop => prop.UslTip).HasColumnName("usl_tip");
        builder.Property(prop => prop.HirTip).HasColumnName("hir_tip").IsRequired(false);
        builder.Property(prop => prop.LekTipL).HasColumnName("lek_tip_l").IsRequired(false);
        builder.Property(prop => prop.LekTipV).HasColumnName("lek_tip_v").IsRequired(false);
        builder.Property(prop => prop.LuchTip).HasColumnName("luch_tip").IsRequired(false);
        builder.Property(prop => prop.PPTR).HasColumnName("pptr").IsRequired(false);

        builder.HasOne<OnkSlEntity>()
            .WithMany(onksl => onksl.OnkUsls)
            .HasForeignKey(onkusl => onkusl.OnkSluchUid);
    }
}