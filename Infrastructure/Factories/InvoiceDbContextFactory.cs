using Microsoft.EntityFrameworkCore;

using HealthInvoice.Core.Common;
using HealthInvoice.Infrastructure.Database.EF.Context;

namespace HealthInvoice.Infrastructure.Factories;


public class InvoiceDbContextFactory(
    IDbContextFactory<SMODbContext> smoFactory,
    IDbContextFactory<TDbContext> tFactory)
{
    public HealthInvoiceDbContext Create(JournalType journalType)
    {
        return journalType switch
        {
            JournalType.SMORX => smoFactory.CreateDbContext(),
            JournalType.Inogorod => tFactory.CreateDbContext(),
            JournalType.None => throw new InvalidDataException("Тип журнала не определен"),
            _ => throw new InvalidOperationException($"Неподдерживаемый тип журнала: {journalType}"),
        };
    }
}
