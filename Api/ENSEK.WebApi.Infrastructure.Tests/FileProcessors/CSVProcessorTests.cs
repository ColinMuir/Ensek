using ENSEK.WebApi.Infrastucture.FileProcessors;
using ENSEK.WebApi.Infrastucture.Repository.Entities;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace ENSEK.WebApi.Infrastructure.Tests.FileProcessors;

[TestFixture]
public class CSVProcessorTests
{
    CSVProcessor sut;

    [SetUp]
    public void Setup() =>sut = new();

    [Test]
    public void ProcessFileMeterReadAsGenericType_WithAllValidLines_ResultTotalLinesEqualsNumberOfRecords()
    {
        var sb = new StringBuilder();

        sb.AppendLine("AccountId,MeterReadingDateTime,MeterReadValue,");//Header
        sb.AppendLine("2344,4/22/2019 9:24,1002,");//Valid
        sb.AppendLine("2345,4/22/2019 9:24,1002,");//Valid

        var lines = sb.ToString();

        var file = MockFile(lines);
        var result =sut.ProcessFile<MeterRead>(file);

        Assert.That(result.TotalRecords, Is.EqualTo(result.Records.Count));
    }

    [Test]
    public void ProcessFileMeterReadAsGenericType_WithInvalidLine_ResultShouldIncludeOneFailure()
    {
        var sb = new StringBuilder();

        sb.AppendLine("AccountId,MeterReadingDateTime,MeterReadValue,");//Header
        sb.AppendLine("2344,4/22/2019 9:24,1002,");//Valid
        sb.AppendLine("2345,4/22/2019 9:24,XXXX,");//Invalid

        var lines = sb.ToString();

        var file = MockFile(lines);
        var result = sut.ProcessFile<MeterRead>(file);

        Assert.That(result.InvalidRecords, Is.EqualTo(1));
    }

    private IFormFile MockFile(string content)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "testFile", "data.csv");
    }
}