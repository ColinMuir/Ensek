namespace ENSEK.WebApi.Infrastucture.Repository.Entities;

public class MeterRead
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public DateTime MeterReadingDateTime { get; set; }

    public int MeterReadValue { get; set; }
}