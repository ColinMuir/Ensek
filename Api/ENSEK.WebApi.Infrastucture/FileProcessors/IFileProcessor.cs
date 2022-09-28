using Microsoft.AspNetCore.Http;

namespace ENSEK.WebApi.Infrastucture.FileProcessors;

public interface IFileProcessor
{
    IProcessFileResult<TModel> ProcessFile<TModel>(IFormFile file);
}