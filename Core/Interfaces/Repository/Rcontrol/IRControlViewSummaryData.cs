using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Rcontrol.Tables;
using HealthInvoice.Core.Dtos.Rcontrol.Filters;
using HealthInvoice.Core.Dtos.Rcontrol.Categories;
using HealthInvoice.Core.Entities.Invoices.Main.H;

namespace HealthInvoice.Core.Interfaces.Repository.Rcontrol;

public interface IRControlViewSummaryData
{
    Task<List<MedOrganizationDto>> GetMedOrgsAsync(
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default);

    Task<List<BillingPeriodDto>> GetPeriodsAsync(
        string codeMo,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default);

    Task<List<InvoiceOverallDto>> GetInvoicesShortlyRecordsAsync(
        string codeMo,
        int Year,
        byte Month,
        JournalType journalType = JournalType.None,
        CancellationToken cancellationToken = default);

    Task<InvoiceSummaryDto> GetInvoiceSummaryAsync(
        int schetUid, 
        JournalType journalType);

    Task<List<FinishedCaseDto>> GetFinishedCasesAsync(
        int schetUid, 
        JournalType journalType);

    Task<List<SlEntity>> GetCasesAsync(
        int zSlUid,
        JournalType journalType);
}