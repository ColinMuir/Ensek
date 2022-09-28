using ENSEK.WebApi.Infrastucture.Repository;
using ENSEK.WebApi.Infrastucture.Repository.Entities;
using ENSEK.WebApi.Infrastucture.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ENSEK.WebApi.Infrastructure.Tests.Services;

[TestFixture]
public class MeterReadingServiceTests
{
    private DbContextOptions<MeterReadingDbContext> options;
    Mock<IAccountService> mockAccountService;

    [SetUp]
    public void Setup()
    {
        options = new DbContextOptionsBuilder<MeterReadingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        mockAccountService = new();
    }

    [Test]
    public async Task ProcesReadsAsync_WhenCalledWithNull_ShouldResultWithNoSuccessfulReads()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new MeterReadingService(context, mockAccountService.Object);

        var result = await sut.ProcesReadsAsync(null!);

        Assert.That(result.Successful, Is.EqualTo(0));
    }

    [Test]
    public async Task ProcesReadsAsync_WhenCalledEmptyList_ShouldResultWithNoSuccessfulReads()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new MeterReadingService(context, mockAccountService.Object);

        var result = await sut.ProcesReadsAsync(new List<MeterRead>());

        Assert.That(result.Successful, Is.EqualTo(0));
    }

    [Test]
    public async Task ProcesReadsAsync_WithDuplicateReads_ShouldRemoveDuplicates()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new MeterReadingService(context, mockAccountService.Object);

        MockAcountService(true);

        var currentDate = DateTime.Now;

        var duplicateReads = new List<MeterRead>
        {
            CreateRead(1, currentDate,1),
            CreateRead(1, currentDate,1)
        };

        var result = await sut.ProcesReadsAsync(duplicateReads);

        Assert.That(result.Successful, Is.EqualTo(1));
    }

    [Test]
    public async Task ProcesReadsAsync_WithMultipleReadsForSameAccount_ShouldSaveNewestRead()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new MeterReadingService(context, mockAccountService.Object);

        MockAcountService(true);

        var olderRead = CreateRead(1, readDate: DateTime.Now.AddDays(-1), 1);
        var newerRead = CreateRead(1, readDate: DateTime.Now, 1);

        var reads = new List<MeterRead> { newerRead, olderRead };

        var result = await sut.ProcesReadsAsync(reads);

        //Get Read from the database
        var dbRead = await sut.GetReadByAccountIdAsync(1);

        Assert.That(result.Successful, Is.EqualTo(1));
        Assert.That(dbRead?.MeterReadingDateTime, Is.EqualTo(newerRead.MeterReadingDateTime));
    }

    [Test]
    public async Task ProcesReadsAsync_WhenExistingReadButNewReadIsOlder_ShouldNotUpdateRecord()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new MeterReadingService(context, mockAccountService.Object);

        MockAcountService(true);

        //Seed database with an existing read
        var existingRead = CreateRead(1, readDate: DateTime.Now, 1);
        await SeedDatabaseAsync(existingRead);

        var newRead = CreateRead(1, readDate: DateTime.Now.AddDays(-1), 1);
        var reads = new List<MeterRead> { newRead };

        var result = await sut.ProcesReadsAsync(reads);

        //Get Read from the database
        var dbRead = await sut.GetReadByAccountIdAsync(1);

        Assert.That(result.Successful, Is.EqualTo(0));
        Assert.That(dbRead?.MeterReadingDateTime, Is.EqualTo(existingRead.MeterReadingDateTime));
    }

    [Test]
    public async Task ProcesReadsAsync_WhenExistingReadButNewReadIsMoreRecent_ShouldUpdateRecord()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new MeterReadingService(context, mockAccountService.Object);

        MockAcountService(true);

        //Seed database with an existing read
        var existingRead = CreateRead(1, readDate: DateTime.Now.AddDays(-1), 1);
        await SeedDatabaseAsync(existingRead);

        var newRead = CreateRead(1, readDate: DateTime.Now, 1);
        var reads = new List<MeterRead> { newRead };

        var result = await sut.ProcesReadsAsync(reads);

        //Get Read from the database
        var dbRead = await sut.GetReadByAccountIdAsync(1);

        Assert.That(result.Successful, Is.EqualTo(1));
        Assert.That(dbRead?.MeterReadingDateTime, Is.EqualTo(newRead.MeterReadingDateTime));
    }

    [Test]
    public async Task ProcesReadsAsync_WhenAccountDoesntExist_ShouldNotBeInserted()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new MeterReadingService(context, mockAccountService.Object);

        MockAcountService(false);

        var reads = new List<MeterRead> { CreateRead(1, DateTime.Now, 1) };

        var result = await sut.ProcesReadsAsync(reads);

        //There should be no read for the account
        var checkRead = await sut.GetReadByAccountIdAsync(1);

        Assert.That(result.Successful, Is.EqualTo(0));
        Assert.That(checkRead, Is.Null);
    }

    [Test]
    public async Task ProcesReadsAsync_WhenMultipleAccounts_ShouldInsertRecords()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new MeterReadingService(context, mockAccountService.Object);

        MockAcountService(true);

        var reads = new List<MeterRead> 
        { 
            CreateRead(accountId: 1, DateTime.Now,1),
            CreateRead(accountId: 2, DateTime.Now,1)
        };

        var result = await sut.ProcesReadsAsync(reads);

        Assert.That(result.Successful, Is.EqualTo(2));
    }

    [Test]
    public async Task ProcesReadsAsync_WhenReadHaveValueToHigh_ShouldNotInsertRecord()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new MeterReadingService(context, mockAccountService.Object);

        MockAcountService(true);

        var reads = new List<MeterRead>
        {
            CreateRead( 1, DateTime.Now, 999999)
        };

        var result = await sut.ProcesReadsAsync(reads);

        Assert.That(result.Successful, Is.EqualTo(0));
    }

    private MeterRead CreateRead(int accountId, DateTime readDate, int meterReadValue) =>
        new MeterRead { AccountId = accountId, MeterReadingDateTime = readDate, MeterReadValue = meterReadValue };

    private async Task SeedDatabaseAsync(MeterRead read)
    {
        await SeedDatabaseAsync(new List<MeterRead> { read });
    }

    private async Task SeedDatabaseAsync(IEnumerable<MeterRead> reads)
    {
        using var context = new MeterReadingDbContext(options);

        await context.MeterReads.AddRangeAsync(reads);

        await context.SaveChangesAsync();
    }

    private void MockAcountService(bool doesExists) =>
        mockAccountService.Setup(x => x.ExistsAsync(It.IsAny<int>()))
                          .ReturnsAsync(doesExists);
}