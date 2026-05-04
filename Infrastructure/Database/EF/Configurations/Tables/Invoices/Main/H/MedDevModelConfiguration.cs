using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Invoices.Main.H;

public class MedDevModelConfiguration : IEntityTypeConfiguration<MedDevEntity>
{
    public void Configure(EntityTypeBuilder<MedDevEntity> builder)
    {
        builder.ToTable("med_dev").HasKey(meddev => meddev.Uid);

        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();
        builder.Property(prop => prop.UslUid).HasColumnName("usl_uid");
        builder.Property(prop => prop.DateMed).HasColumnName("date_med");
        builder.Property(prop => prop.CodeMeddev).HasColumnName("code_meddev");
        builder.Property(prop => prop.NumberSer).HasColumnName("number_ser").HasMaxLength(100);

        builder.HasOne<UslEntity>()
            .WithMany(usl => usl.MedDevs)
            .HasForeignKey(med => med.UslUid);
    }
}