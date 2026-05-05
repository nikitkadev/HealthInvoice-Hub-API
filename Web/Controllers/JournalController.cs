using Microsoft.AspNetCore.Mvc;

using HealthInvoice.Core.Interfaces.Repository.Journals;
using HealthInvoice.Core.Common;
using HealthInvoice.Core.Dtos.Service;

namespace HealthInvoice.Web.Controllers
{
    [ApiController]
    [Route("healthinvoice/api/journal")]
    public class JournalController(
        ILkJournalRepository journalLkRepository,
        IFkJournalRepository journalFkRepository) : ControllerBase
    {
        [HttpGet("lk/fetch")]
        public async Task<IActionResult> FetchJournalRecordsAsync(
            string organizationCode,
            int journalType,
            [FromQuery] LogicControlJournalFilters filters,
            int page = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            var (items, total) = await journalLkRepository.GetRecordsAsync(
                filters,
                organizationCode,
                skip: (page - 1) * pageSize,
                take: pageSize,
                (JournalType)journalType,
                cancellationToken: cancellationToken);

            return Ok(
                new
                {
                    items,
                    total,
                    page,
                    pageSize
                });
        }

        [HttpGet("fk/fetch")]
        public async Task<IActionResult> FetchFkJournalRecordsAsync(
            string organizationCode,
            int journalType,
            CancellationToken cancellationToken,
            int page = 1,
            int pageSize = 20)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 20;

            var (items, total) = await journalFkRepository.GetRecordsAsync(
                organizationCode,
                skip: (page - 1) * pageSize,
                take: pageSize,
                (JournalType)journalType,
                cancellationToken: cancellationToken);

            return Ok(
                new
                {
                    items,
                    total,
                    page,
                    pageSize
                });
        }
    }
}
