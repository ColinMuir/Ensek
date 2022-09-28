namespace ENSEK.WebApi.Infrastucture.FileProcessors;

public class ProcessFileResult<T> : IProcessFileResult<T>
{
    public ProcessFileResult()
    {
        Records = new List<T>();
    }
    public int TotalRecords { get; set; }

    public int InvalidRecords { get; set; }

    public IList<T> Records { get; set; }
}