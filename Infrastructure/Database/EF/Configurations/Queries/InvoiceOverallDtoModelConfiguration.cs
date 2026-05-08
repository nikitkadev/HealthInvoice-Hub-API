using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Dtos.Rcontrol.Tables;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Queries;

public class InvoiceOverallDtoModelConfiguration : IEntityTypeConfiguration<InvoiceOverallDto>
{
    public void Configure(EntityTypeBuilder<InvoiceOverallDto> builder)
    {
        builder.HasNoKey();

        builder.Property(prop => prop.Status).HasColumnName("stat");
        builder.Property(prop => prop.NSchet).HasColumnName("nschet");
        builder.Property(prop => prop.DSchet).HasColumnName("dschet");
        builder.Property(prop => prop.Summav).HasColumnName("summav");
        builder.Property(prop => prop.SdZ).HasColumnName("sd_z");
        builder.Property(prop => prop.SchetUid).HasColumnName("schet_uid");
    }
}
