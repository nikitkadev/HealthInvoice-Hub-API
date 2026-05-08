using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Dtos.Rcontrol.Tables;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Queries;

public class FinishedCaseDtoModelConfiguration : IEntityTypeConfiguration<FinishedCaseDto>
{
    public void Configure(EntityTypeBuilder<FinishedCaseDto> builder)
    {
        builder.HasNoKey();

        builder.Property(prop => prop.PacientUid).HasColumnName("pcuid");
        builder.Property(prop => prop.PersUid).HasColumnName("psuid");
        builder.Property(prop => prop.ZSlUid).HasColumnName("zsluid");
        builder.Property(prop => prop.PositionNumber).HasColumnName("idcase");
        builder.Property(prop => prop.RecordNumber).HasColumnName("n_zap");
        builder.Property(prop => prop.Surname).HasColumnName("fam");
        builder.Property(prop => prop.Name).HasColumnName("im");
        builder.Property(prop => prop.Patronymic).HasColumnName("ot");
        builder.Property(prop => prop.UslOk).HasColumnName("usl_ok");
        builder.Property(prop => prop.SPolis).HasColumnName("spolis");
        builder.Property(prop => prop.NPolis).HasColumnName("npolis");
        builder.Property(prop => prop.Sumv).HasColumnName("sumv");
        builder.Property(prop => prop.Sump).HasColumnName("sump");
        builder.Property(prop => prop.SmoSump).HasColumnName("smo_sump");
    }
}