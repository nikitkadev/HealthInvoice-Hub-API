using HealthInvoice.Core.Dtos.Reports;
using HealthInvoice.Core.Entities.Journals;
using HealthInvoice.Core.Interfaces.Services.Invoices.Mapping;

namespace HealthInvoice.Application.Services.Invoices.Mapping;

public class FkDefectReportMapper : IMapper<List<FormatControlDefectEntity>, ReportFormatFDto>
{
    public ReportFormatFDto MapTo(List<FormatControlDefectEntity> input)
    {
        return new ReportFormatFDto()
        {
            FName = string.Empty,
            FName1 = string.Empty,
            PRs = input.Count > 0 ? [.. input.Select(
                data => new PR_F()
                {
                    Comment = data.Comment,
                    Oshib = 0
                })] : [new() { Comment = "Без ошибок.", Oshib = 0}]
        };
    }
}
