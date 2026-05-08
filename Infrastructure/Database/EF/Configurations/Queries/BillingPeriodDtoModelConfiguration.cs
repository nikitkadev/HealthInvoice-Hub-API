using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Dtos.Rcontrol.Filters;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Queries;

public class BillingPeriodDtoModelConfiguration : IEntityTypeConfiguration<BillingPeriodDto>
{
    public void Configure(EntityTypeBuilder<BillingPeriodDto> builder)
    {
        builder.HasNoKey();

        builder.Property(prop => prop.Year).HasColumnName("year_schet");
        builder.Property(prop => prop.Month).HasColumnName("month_schet");
    }
}
