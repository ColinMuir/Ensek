using ENSEK.WebApi.Infrastucture.Repository;
using ENSEK.WebApi.Infrastucture.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace ENSEK.WebApi.Infrastucture.Services;

public class MeterReadingService : IMeterReadingService
{
    private readonly MeterReadingDbContext context;
    private readonly IAccountService accountService;

    public MeterReadingService(MeterReadingDbContext context, IAccountService accountService)
    {
        this.context = context;
        this.accountService = accountService;
    }

    public async Task<ProcessReadsResult> ProcesReadsAsync(IEnumerable<MeterRead> reads)
    {
        var result = new ProcessReadsResult();

        if (reads == null || !reads.Any())
            return result;

        //Ordering by date selects the most recent read, selecting the first record will then remove the duplicates.
        var readsByAccountId =
            reads.GroupBy(x => x.AccountId)
                 .ToDictionary(x => x.Key, x => x.ToList()
                                                 .OrderByDescending(x => x.MeterReadingDateTime)
                                                 .First());

        foreach (var accountGroup in readsByAccountId)
        {
            if (accountGroup.Value.MeterReadValue > 99999)
                continue;

            //Does the account exists?
            if (await accountService.ExistsAsync(accountGroup.Key))
                await ProcessReadAsync(accountGroup.Value);
        }

        result.Successful = await SaveChanges();

        return result;
    }

    public IQueryable<MeterRead> Get() =>
        context.MeterReads;

    public async Task<MeterRead?> GetReadByAccountIdAsync(int accountId) =>
        await context.MeterReads.SingleOrDefaultAsync(x => x.AccountId == accountId);

    private async Task ProcessReadAsync(MeterRead newRead)
    {
   

        //Check for existing read
        var existingRead = await GetReadByAccountIdAsync(newRead.AccountId);

        if (existingRead == null)
        {
            //New read, insert and return
            await context.MeterReads.AddAsync(newRead);
            return;
        }

        //We have an existing read. If the new read is more recent then update the values; 
        if (existingRead.MeterReadingDateTime < newRead.MeterReadingDateTime)
        {
            existingRead.MeterReadingDateTime = newRead.MeterReadingDateTime;
            existingRead.MeterReadValue = newRead.MeterReadValue;
        }
    }

    private async Task<int> SaveChanges() => await context.SaveChangesAsync();
}