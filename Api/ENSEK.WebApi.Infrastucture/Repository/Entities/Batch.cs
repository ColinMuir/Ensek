namespace ENSEK.WebApi.Infrastucture.Repository.Entities;

public class Batch
{
    public int Id { get; set; }
    public int TotalRecords { get; set; }
    public int Successful { get; set; }
}