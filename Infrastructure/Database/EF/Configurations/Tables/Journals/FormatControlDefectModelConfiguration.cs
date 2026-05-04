using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Entities.Journals;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Tables.Journals;

public class FormatControlDefectModelConfiguration : IEntityTypeConfiguration<FormatControlDefectEntity>
{
    public void Configure(EntityTypeBuilder<FormatControlDefectEntity> builder)
    {
        builder.ToTable("fk_defects").HasKey(entity => entity.Uid);
        builder.Property(prop => prop.Uid).ValueGeneratedOnAdd();

        builder.Property(prop => prop.JournalUid).HasColumnName("journal_uid");
        builder.Property(prop => prop.FormatType).HasColumnName("tip_file");
        builder.Property(prop => prop.Comment).HasColumnName("comment");

        builder.HasOne<FormatControlJournalEntity>()
            .WithMany(journal => journal.FormatControlDeffects)
            .HasForeignKey(entity => entity.JournalUid)
            .OnDelete(DeleteBehavior.Cascade);
    }
}