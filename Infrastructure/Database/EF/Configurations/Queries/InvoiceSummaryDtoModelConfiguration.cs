using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Dtos.Rcontrol.Categories;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Queries;

public class InvoiceSummaryDtoModelConfiguration : IEntityTypeConfiguration<InvoiceSummaryDto>
{
    public void Configure(EntityTypeBuilder<InvoiceSummaryDto> builder)
    {
        builder.HasNoKey();

        builder.Property(prop => prop.Summav).HasColumnName("sumv");
        builder.Property(prop => prop.Summap).HasColumnName("tfoms_sump");
        builder.Property(prop => prop.SankMek).HasColumnName("tfoms_sank_mek");
        builder.Property(prop => prop.SankMee).HasColumnName("tfoms_sank_mee");
        builder.Property(prop => prop.SankEkmp).HasColumnName("tfoms_sank_ekmp");
        builder.Property(prop => prop.SmoSummap).HasColumnName("smo_sump");
        builder.Property(prop => prop.SmoSankMek).HasColumnName("smo_sank_mek");
        builder.Property(prop => prop.SmoSankMee).HasColumnName("smo_sank_mee");
        builder.Property(prop => prop.SmoSankEkmp).HasColumnName("smo_sank_ekmp");
        builder.Property(prop => prop.Filename).HasColumnName("schet_filename");
        builder.Property(prop => prop.SchetUid).HasColumnName("schet_uid");
        builder.Property(prop => prop.UploadDate).HasColumnName("schet_uploaddate");
    }
}