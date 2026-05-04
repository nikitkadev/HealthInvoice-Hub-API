using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using HealthInvoice.Core.Dtos.Database.QueryResults;

namespace HealthInvoice.Infrastructure.Database.EF.Configurations.Queries;

public class LogicControlDefectModelConfiguration : IEntityTypeConfiguration<LogicControlDefectDto>
{
    public void Configure(EntityTypeBuilder<LogicControlDefectDto> builder)
    {
        builder.HasNoKey();

        builder.Property(prop => prop.Comment).HasColumnName("comment");
        builder.Property(prop => prop.BasEl).HasColumnName("bas_el");
        builder.Property(prop => prop.ImPol).HasColumnName("im_pol");
        builder.Property(prop => prop.Kod).HasColumnName("kod");
        builder.Property(prop => prop.NZap).HasColumnName("n_zap");
        builder.Property(prop => prop.IdCase).HasColumnName("idcase");
        builder.Property(prop => prop.SlId).HasColumnName("sl_id");
        builder.Property(prop => prop.Fam).HasColumnName("fam");
        builder.Property(prop => prop.Im).HasColumnName("im");
        builder.Property(prop => prop.Ot).HasColumnName("ot");
        builder.Property(prop => prop.Dr).HasColumnName("dr");
        builder.Property(prop => prop.Date1).HasColumnName("date_1");
        builder.Property(prop => prop.Date2).HasColumnName("date_2");
        builder.Property(prop => prop.Iddokt).HasColumnName("iddokt");
    }
}