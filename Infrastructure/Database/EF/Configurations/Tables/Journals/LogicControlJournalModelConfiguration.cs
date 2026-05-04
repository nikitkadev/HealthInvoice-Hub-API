using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Journals;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Journals;

public class LogicControlJournalModelConfiguration : IEntityTypeConfiguration<LogicControlJournalEntity>
{
    public void Configure(EntityTypeBuilder<LogicControlJournalEntity> builder)
    {
        builder.ToTable("journal_lk").HasKey(key => key.Uid);
        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();

        builder.Property(prop => prop.SchetUid)
            .HasColumnName("schet_uid")
            .IsRequired(true);

        builder.Property(prop => prop.UploadDate)
            .HasColumnName("uploaddate")
            .IsRequired(true);

        builder.Property(prop => prop.Uploader)
            .HasColumnName("uploader")
            .HasMaxLength(64)
            .IsRequired(true);

        builder.Property(prop => prop.FileName)
            .HasColumnName("filename")
            .HasMaxLength(100)
            .IsRequired(true);

        builder.Property(prop => prop.CodeMO)
            .HasColumnName("code_mo")
            .HasMaxLength(6)
            .IsRequired(true);

        builder.Property(prop => prop.NSchet)
            .HasColumnName("nschet")
            .HasMaxLength(15)
            .IsRequired(true);

        builder.Property(prop => prop.DSchet)
            .HasColumnName("dschet")
            .IsRequired(true);

        builder.Property(prop => prop.CountSdZ)
            .HasColumnName("count_sd_z")
            .IsRequired(true);

        builder.Property(prop => prop.CountError)
            .HasColumnName("count_error")
            .IsRequired(true);

        builder.Property(prop => prop.Status)
            .HasColumnName("status")
            .IsRequired(true);
    }
}