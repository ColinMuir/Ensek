using ENSEK.WebApi.Infrastucture.Repository;
using Microsoft.EntityFrameworkCore;

namespace ENSEK.WebApi.Infrastucture.Services;

public class AccountService : IAccountService
{
    private readonly MeterReadingDbContext context;

    public AccountService(MeterReadingDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> ExistsAsync(int accountId) =>
        await context.Accounts.AnyAsync(x => x.AccountId == accountId);
}