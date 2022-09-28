namespace ENSEK.WebApi.Infrastucture.FileProcessors;

public interface IProcessFileResult<T>
{
    int InvalidRecords { get; set; }
    IList<T> Records { get; set; }
    int TotalRecords { get; set; }
}