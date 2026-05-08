using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Dtos.Rcontrol.Filters;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Queries;

public class MedOrganizationDtoModelConfiguration : IEntityTypeConfiguration<MedOrganizationDto>
{
    public void Configure(EntityTypeBuilder<MedOrganizationDto> builder)
    {
        builder.HasNoKey();

        builder.Property(prop => prop.Code).HasColumnName("code");
        builder.Property(prop => prop.Name).HasColumnName("name_code");
    }
}
