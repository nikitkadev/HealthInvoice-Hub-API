using System.Reflection;

using Microsoft.EntityFrameworkCore;

using HealthInvoice.Core.Entities.Domain;
using HealthInvoice.Core.Entities.Journals;
using HealthInvoice.Core.Entities.Invoices.Main.H;
using HealthInvoice.Core.Entities.Invoices.Main.L;
using HealthInvoice.Core.Dtos.Database.QueryResults;

namespace HealthInvoice.Infrastructure.Database.EF.Context;

public abstract class HealthInvoiceDbContext(
    DbContextOptions options) : DbContext(options)
{
    public DbSet<ZlListEntity> ZlLists { get; set; }
    public DbSet<PersListEntity> PersLists { get; set; }
    public DbSet<SlEntity> Cases { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<LogicControlJournalEntity> LogicControlJournalRecords { get; set; }
    public DbSet<FormatControlJournalEntity> FormatControlJournalRecords { get; set; }
    public DbSet<FormatControlDefectEntity> FormatControlDefects { get; set; }
    public DbSet<LogicControlDefectDto> LogicControlDefects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

public class SMODbContext(
    DbContextOptions<SMODbContext> options) : HealthInvoiceDbContext(options);
public class TDbContext(
    DbContextOptions<TDbContext> options) : HealthInvoiceDbContext(options);
