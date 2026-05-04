using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;
using HealthInvoice.Core.Entities.Invoices.Additional;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Additional;

public class Ds2ModelConfiguration : IEntityTypeConfiguration<Ds2Entity>
{
    public void Configure(EntityTypeBuilder<Ds2Entity> builder)
    {
        builder.ToTable("ds2").HasKey(ds2 => ds2.Uid);

        builder.Property(ds2 => ds2.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.SluchUid).HasColumnName("sluch_uid");
        builder.Property(prop => prop.DS2).HasColumnName("ds2").HasMaxLength(10);

        builder.HasOne<SlEntity>()
            .WithMany(sl => sl.Ds2s)
            .HasForeignKey(ds2 => ds2.SluchUid);
    }
}