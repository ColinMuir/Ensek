using ENSEK.WebApi.Infrastucture.FileProcessors;
using ENSEK.WebApi.Infrastucture.Repository;
using ENSEK.WebApi.Infrastucture.Repository.Entities;
using ENSEK.WebApi.Infrastucture.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ENSEK.WebApi.Infrastructure.Tests.Services;

[TestFixture]
public class BatchServiceTests
{
    private DbContextOptions<MeterReadingDbContext> options;
    Mock<IFileProcessor> mockFileProcessor;
    Mock<IMeterReadingService> mockMeterReadingService;

    [SetUp]
    public void Setup()
    {
        options = new DbContextOptionsBuilder<MeterReadingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        mockFileProcessor = new();
        mockMeterReadingService = new();
    }

    [Test]
    public void ProcessBatchAsync_NoFile_ShouldThrowArgumentNullException()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new BatchService(context, mockFileProcessor.Object, mockMeterReadingService.Object);

        Assert.That(async () => await sut.ProcessBatchAsync(null!), Throws.ArgumentNullException);
    }

    [Test]
    public async Task ProcessBatchAsync_WithFile_ShouldCallProcessFileService()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new BatchService(context, mockFileProcessor.Object, mockMeterReadingService.Object);

        MockFileProcessor();
        MockMeterReadingService();

        await sut.ProcessBatchAsync(CreateFile());

        mockFileProcessor.Verify(x => x.ProcessFile<MeterRead>(It.IsAny<IFormFile>()), Times.Once);
    }

    [Test]
    public async Task ProcessBatchAsync_WithFile_ShouldCallProcessReads()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new BatchService(context, mockFileProcessor.Object, mockMeterReadingService.Object);

        MockFileProcessor();
        MockMeterReadingService();

        await sut.ProcessBatchAsync(CreateFile());

        mockMeterReadingService.Verify(x => x.ProcesReadsAsync(It.IsAny<IEnumerable<MeterRead>>()), Times.Once);
    }

    [Test]
    public async Task ProcessBatchAsync_WithFile_ShouldCallProcessReadsWithRecordsFromProcessFile()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new BatchService(context, mockFileProcessor.Object, mockMeterReadingService.Object);

        var reads = CreateReads(1);

        MockFileProcessor(1, 0, reads);
        MockMeterReadingService();

        await sut.ProcessBatchAsync(CreateFile());

        mockMeterReadingService.Verify(x => x.ProcesReadsAsync(reads), Times.Once);
    }

    [Test]
    public async Task ProcessBatchAsync_WithFile_ShouldSaveBatchRecordToDatabase()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new BatchService(context, mockFileProcessor.Object, mockMeterReadingService.Object);

        MockFileProcessor();
        MockMeterReadingService();

        var batch = await sut.ProcessBatchAsync(CreateFile());

        var result = await sut.GetAsync(batch.Value!.Id);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task ProcessBatchAsync_WithFile_ReturnedBatchRecordShouldContainCorrectNumberOfSuccessfulReads()
    {
        using var context = new MeterReadingDbContext(options);
        var sut = new BatchService(context, mockFileProcessor.Object, mockMeterReadingService.Object);

        var reads = CreateReads(10);
        MockFileProcessor(10, 0, reads);
        MockMeterReadingService(reads.Count);

        var result = await sut.ProcessBatchAsync(CreateFile());

        Assert.That(result.HasValue);
        Assert.That(result.Value!.Successful, Is.EqualTo(reads.Count));
    }

    private IList<MeterRead> CreateReads(int howMany) =>
        Enumerable.Range(1, howMany)
                  .Select(x => new MeterRead { 
                      Id = x, 
                      AccountId = x, 
                      MeterReadingDateTime = DateTime.Now, 
                      MeterReadValue = x })
                  .ToList();

    private IFormFile CreateFile() =>
        new FormFile(new MemoryStream(new byte[0]), 0, 0, "test", "test.csv");

    private void MockFileProcessor() =>
        mockFileProcessor.Setup(x=>x.ProcessFile<MeterRead>(It.IsAny<IFormFile>()))
                         .Returns(new ProcessFileResult<MeterRead>() { });

    private void MockFileProcessor(int totalRecords, int invalidRecords, IList<MeterRead> records) =>
        mockFileProcessor.Setup(x => x.ProcessFile<MeterRead>(It.IsAny<IFormFile>()))
                         .Returns(new ProcessFileResult<MeterRead>() { TotalRecords = totalRecords, InvalidRecords = invalidRecords, Records = records });

    private void MockMeterReadingService(int successful = 0) =>
        mockMeterReadingService.Setup(x => x.ProcesReadsAsync(It.IsAny<IEnumerable<MeterRead>>()))
                               .ReturnsAsync(new ProcessReadsResult() {  Successful = successful } );
}