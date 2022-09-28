using ENSEK.WebApi.Infrastucture.Repository;
using ENSEK.WebApi.Infrastucture.Repository.Entities;
using ENSEK.WebApi.Infrastucture.Services;
using Microsoft.EntityFrameworkCore;

namespace ENSEK.WebApi.Infrastructure.Tests.Services;

[TestFixture]
public class AccountServiceTests
{
    private DbContextOptions<MeterReadingDbContext> options;

    [SetUp]
    public void Setup() =>
        options = new DbContextOptionsBuilder<MeterReadingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

    [Test]
    public async Task ExistsAsync_NoAccount_ShouldReturnFalse()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new AccountService(context);

        var result = await sut.ExistsAsync(1);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task ExistsAsync_ValidAccount_ShouldReturnTrue()
    {
        using var context = new MeterReadingDbContext(options);

        var account = new Account() { AccountId = 1, FirstName = "TestFN", LastName = "TestLN" };

        context.Add(account);
        context.SaveChanges();

        var sut = new AccountService(context);

        var result = await sut.ExistsAsync(account.AccountId);

        Assert.That(result);
    }
}