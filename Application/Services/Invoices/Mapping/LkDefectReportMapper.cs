using HealthInvoice.Core.Dtos.Database.QueryResults;
using HealthInvoice.Core.Dtos.Reports;
using HealthInvoice.Core.Interfaces.Services.Invoices.Mapping;

namespace HealthInvoice.Application.Services.Invoices.Mapping;

public class LkDefectReportMapper : IMapper<List<LogicControlDefectDto>, ReportFormatVDto>
{
    public ReportFormatVDto MapTo(List<LogicControlDefectDto> input)
    {
        return new ReportFormatVDto()
        {
            FName = string.Empty,
            FName1 = string.Empty,
            PR = input.Count != 0 ? [.. input.Select(
                record => new PR_V()
                {
                    Oshib = record.Kod,
                    BasEl = record.BasEl,
                    Comment = record.Comment,
                    Fam = record.Fam,
                    IdCase = record.IdCase,
                    Im = record.Im,
                    ImPl = record.ImPol,
                    NZap = record.NZap,
                    Ot = record.Ot,
                    SlId = record.SlId
                })] : new() { new() { Oshib = 0, Comment = "Без ошибок" } }

        };
    }
}

