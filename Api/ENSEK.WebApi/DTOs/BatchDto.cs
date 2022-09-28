namespace ENSEK.WebApi.DTOs;

public class BatchDto
{
    public int Id { get; set; }

    public int TotalRecords { get; set; }

    public int Successful { get; set; }

    public int Failures => TotalRecords - Successful;
}