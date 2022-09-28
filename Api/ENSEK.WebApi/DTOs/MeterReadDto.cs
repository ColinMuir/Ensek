namespace ENSEK.WebApi.DTOs;

public class MeterReadDto
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public DateTime MeterReadingDateTime { get; set; }

    public int MeterReadValue { get; set; }
}