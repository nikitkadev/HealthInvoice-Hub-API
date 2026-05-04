using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class MrUslNModelConfiguration : IEntityTypeConfiguration<MrUslNEntity>
{
    public void Configure(EntityTypeBuilder<MrUslNEntity> builder)
    {
        builder.ToTable("mr_usl_n").HasKey(mr => mr.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.UslUid).HasColumnName("usl_uid");
        builder.Property(prop => prop.MrN).HasColumnName("mr_n");
        builder.Property(prop => prop.PRVS).HasColumnName("prvs");
        builder.Property(prop => prop.CodeMd).HasColumnName("code_md").HasMaxLength(25);

        builder.HasOne<UslEntity>()
            .WithMany(usl => usl.MrUslNs)
            .HasForeignKey(mr => mr.UslUid);
    }
}