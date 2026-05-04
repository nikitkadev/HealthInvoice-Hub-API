using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Journals;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Journals;

public class FormatControlJournalModelConfiguration : IEntityTypeConfiguration<FormatControlJournalEntity>
{
    public void Configure(EntityTypeBuilder<FormatControlJournalEntity> builder)
    {
        builder.ToTable("journal_fk").HasKey(key => key.Uid);
        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();

        builder.Property(prop => prop.UploadDate)
            .HasColumnName("uploaddate")
            .IsRequired(true);

        builder.Property(prop => prop.OrganizationCode)
            .HasColumnName("Code_mo")
            .IsRequired(true);

        builder.Property(prop => prop.SourceArchiveFilename)
            .HasColumnName("filename_NM")
            .IsRequired(true);

        builder.Property(prop => prop.Status)
            .HasColumnName("status")
            .IsRequired(true);
    }
}