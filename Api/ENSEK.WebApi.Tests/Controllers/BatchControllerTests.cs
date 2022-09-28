using AutoMapper;
using ENSEK.WebApi.Controllers;
using ENSEK.WebApi.DTOs;
using ENSEK.WebApi.Infrastucture.Repository.Entities;
using ENSEK.WebApi.Infrastucture.Services;
using ENSEK.WebApi.Tests.Extensions;
using Loudwater.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ENSEK.WebApi.Tests.Controllers;

[TestFixture]
public class BatchControllerTests
{
    BatchController sut;
    Mock<ILogger<BatchController>> mockLogger;
    Mock<IBatchService> mockBatchService;
    Mock<IMapper> mockMapper;

    [SetUp]
    public void Setup()
    {
        mockLogger = new();
        mockBatchService = new();
        mockMapper = new();

        MockMapper();

        sut = new(mockLogger.Object, mockBatchService.Object, mockMapper.Object);
    }



    [Test]
    public async Task PostMeterReadBatch_NoFile_ShouldReturnBadRequest()
    {
        var result = await sut.PostMeterReadBatch(null!);

        Assert.That(result.Result, Is.TypeOf<BadRequestResult>());
    }

    [Test]
    public async Task PostMeterReadBatch_WithFile_ShouldCallProcessBatchService()
    {
        MockBatchService(1, 1);

        await sut.PostMeterReadBatch(CreateFile());

        mockBatchService.Verify(x => x.ProcessBatchAsync(It.IsAny<IFormFile>()), Times.Once);
    }

    [Test]
    public async Task PostMeterReadBatch_WithFile_ShouldReturnCreatedAtRoute()
    {
        MockBatchService(1, 1);

        var result = await sut.PostMeterReadBatch(CreateFile());

        Assert.That(result.Result, Is.TypeOf<CreatedAtRouteResult>());
    }

    [Test]
    public async Task PostMeterReadBatch_WithFile_ShouldReturnBatchDtoWithCorrectValues()
    {
        MockBatchService(1, 1);

        mockMapper.Setup(x => x.Map<BatchDto>(It.IsAny<Batch>()))
          .Returns(new BatchDto() {  Successful = 1, TotalRecords = 1});

        var actionResult = await sut.PostMeterReadBatch(CreateFile());

        var result = actionResult.GetActionResultValue();

        Assert.That(result!.Successful, Is.EqualTo(1));
        Assert.That(result.TotalRecords, Is.EqualTo(1));
    }

    [Test]
    public async Task GetMeterReadBatch_NoRecord_ShouldReturnNotFound()
    {
        int id =99;

        Batch? nullBatch = null;
        mockBatchService.Setup(x => x.GetAsync(id)).ReturnsAsync(nullBatch);

        var result = await sut.GetMeterReadBatch(id);

        Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
    }

    [Test]
    public async Task GetMeterReadBatch_ValidRecord_ShouldReturnOKResult()
    {
        mockBatchService.Setup(x => x.GetAsync(It.IsAny<int>()))
                        .ReturnsAsync(new Batch());

        var result = await sut.GetMeterReadBatch(It.IsAny<int>());

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetMeterReadBatch_ValidRecord_ShouldReturnBatchDto()
    {
        mockBatchService.Setup(x => x.GetAsync(It.IsAny<int>()))
                        .ReturnsAsync(new Batch());

        var actionResult = await sut.GetMeterReadBatch(It.IsAny<int>());

        var result = actionResult.GetActionResultValue();

        Assert.That(result, Is.TypeOf<BatchDto>());
    }

    private void MockBatchService(int totalRecords, int successful)
    {
        mockBatchService.Setup(x => x.ProcessBatchAsync(It.IsAny<IFormFile>()))
                        .ReturnsAsync(Result.Success(new Batch { TotalRecords = totalRecords, Successful = successful }));
    }

    private IFormFile CreateFile() =>
        new FormFile(new MemoryStream(new byte[0]), 0, 0, "test", "test.csv");

    private void MockMapper() =>
        mockMapper.Setup(x => x.Map<BatchDto>(It.IsAny<Batch>()))
                  .Returns(new BatchDto());
}