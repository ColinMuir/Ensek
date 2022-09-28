using ENSEK.WebApi.DTOs;

namespace ENSEK.WebApi.Tests.DTOs;

[TestFixture]
public class BatchDtoTests
{
    [Test]
    public void Failures_ShouldEqual_TotalRecordsMinusSuccessul()
    {
        var total = 5;
        var successful = 2;

        var sut = new BatchDto() { TotalRecords = total, Successful = 2 };

        Assert.That(sut.Failures, Is.EqualTo(total - successful));
    }
}
