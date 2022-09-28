using ENSEK.WebApi.Infrastucture.Extensions;
using ENSEK.WebApi.Infrastucture.Repository.Entities;
using ENSEK.WebApi.Infrastucture.Repository.Entities.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ENSEK.WebApi.Infrastucture.Repository;

public class MeterReadingDbContext: DbContext
{
    public MeterReadingDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Account> Accounts => Set<Account>();

    public DbSet<MeterRead> MeterReads => Set<MeterRead>();

    public DbSet<Batch> Batches => Set<Batch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountConfiguration).Assembly);

        modelBuilder.SeedDatabase();
    }
}