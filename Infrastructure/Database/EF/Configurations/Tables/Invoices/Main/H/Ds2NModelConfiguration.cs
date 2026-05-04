using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class Ds2NModelConfiguration : IEntityTypeConfiguration<Ds2NEntity>
{
    public void Configure(EntityTypeBuilder<Ds2NEntity> builder)
    {
        builder.ToTable("ds2_n").HasKey(ds2n => ds2n.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.SluchUid).HasColumnName("sluch_uid");
        builder.Property(prop => prop.Ds2).HasColumnName("ds2").HasMaxLength(10);
        builder.Property(prop => prop.Ds2PR).HasColumnName("ds2_pr").IsRequired(false);
        builder.Property(prop => prop.PRDs2N).HasColumnName("pr_ds2_n");

        builder.HasOne<SlEntity>()
            .WithMany(sl => sl.Ds2Ns)
            .HasForeignKey(ds2n => ds2n.SluchUid);
    }
}